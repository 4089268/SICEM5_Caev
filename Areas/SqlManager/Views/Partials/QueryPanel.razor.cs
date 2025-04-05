using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.Data;
using Sicem_Blazor.Services;
using Sicem_Blazor.SqlManager.Models;
using Sicem_Blazor.SqlManager.Core;
using Sicem_Blazor.Helpers;

namespace Sicem_Blazor.SqlManager.Views.Partials;

public partial class QueryPanel
{
    [Inject]
    public IJSRuntime JSRuntime {get;set;}

    [Inject]
    public IMatToaster MatToaster {get;set;}

    [Inject]
    public SicemService SicemService1 {get;set;}

    [Inject]
    public ILogger<QueryPanel> Logger {get;set;}

    [Inject]
    public IConfiguration Configuration {get;set;}
    

    [Parameter]
    public QueryModel Query {get;set;}

    [Parameter]
    public IEnumerable<IEnlace> Enlaces {get;set;}

    private List<Task> tareas;

    private bool showResultsModal = false;
    private bool processing = false;

    private ResultsVtn resultsVtn1;

    public void CheckboxChanged(ChangeEventArgs e, IEnlace enlace)
    {
        var selected = (bool)e.Value;
        if(selected)
        {
            if(!Query.Enlaces.Contains(enlace))
            {
                Query.Enlaces.Add(enlace);
            }
        }
        else
        {
            Query.Enlaces = Query.Enlaces.Where(x => x.Id != enlace.Id).ToList();
        }
        // Console.WriteLine($"{enlace.Nombre} {selected}");
    }

    public async Task RunOnClick()
    {
        processing = true;
        StateHasChanged();

        // * check if the query is not null
        if(string.IsNullOrEmpty(Query.Query))
        {
            MatToaster.Add("La consulta no puede estar vacía.", MatToastType.Warning);
            return;
        }

        // * check if any office is selected
        if(!Query.Enlaces.Any())
        {
            MatToaster.Add("Seleccione minimo una oficina.", MatToastType.Warning);
            return;
        }

        // * prepare the rows
        Query.Results = Query.Enlaces.Select(e => QueryResult.NewPending(e)).ToList();
        StateHasChanged();

        // * Prepare Tasks
        tareas = Query.Enlaces.Select( e => Task.Run( () => ProcessEnlace(e) )).ToList<Task>();

        await Task.WhenAll(tareas);

        processing = false;
        StateHasChanged();
    }

    private void ProcessEnlace(IEnlace enlace)
    {
        try
        {
            // * verify if the office is On
            if(!SicemService1.CheckOfficeConnected(enlace))
            {
                throw new Exception($"The office {enlace.Nombre} is not connected");
            }

            var _random = new Random();
            var sleep = _random.Next(3000);
            System.Threading.Thread.Sleep(sleep);

            var dataSet = new DataSet();
            using(var sqlConnection = new SqlConnection(enlace.GetConnectionString()))
            {
                sqlConnection.Open();
                using var sqlCommand = new SqlCommand(Query.Query, sqlConnection);
                using var sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataSet);
                sqlConnection.Close();
            }

            // * Refrescar datos grid
            lock(Query.Results)
            {
                var queryResult = Query.Results.FirstOrDefault( item => item.Enlace.Id == enlace.Id) ?? throw new Exception("Results not found");
                queryResult.Result = dataSet;
                queryResult.Status = QueryResult.QueryResultStatus.Success;
            }

            Logger.LogInformation("Finished office: {oficina} with {n} tables", enlace.Nombre, dataSet.Tables.Count);
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Fail at process the office: {oficina}", enlace.Nombre);
            lock(Query.Results)
            {
                var queryResult = Query.Results.FirstOrDefault( item => item.Enlace.Id == enlace.Id) ?? throw new Exception("Results not found");
                queryResult.Status = QueryResult.QueryResultStatus.Failure;
            }
        }
        InvokeAsync(StateHasChanged);
    }

    private async Task ShowResults(QueryResult queryResults)
    {
        if(showResultsModal == true)
        {
            return;
        }

        showResultsModal = true;
        resultsVtn1.Inicializar(null, queryResults);

        await InvokeAsync(StateHasChanged);
    }

    private async Task ExportExcelClick()
    {
        var results = Query.Results.Where(x => x.Status == QueryResult.QueryResultStatus.Success).ToList();
        if(!results.Any())
        {
            MatToaster.Add("No hay resultados para exportar.", MatToastType.Warning);
            return;
        }

        // * combine the datasets
        var dataSetCombiner = new DataSetCombiner(results.Select(x => x.Result));
        var dataSet = dataSetCombiner.Combine();

        // * prepare the file name
        var fileId = Guid.NewGuid();
        var _tmpFolder = Configuration.GetValue<string>("TempFolder");
        var fileName = System.IO.Path.Combine(_tmpFolder, fileId.ToString(),$"resultados-{DateTime.Now.Ticks}.xlsx");
        
        // * export data to file
        var exportDataSet = new ExportDataSet(dataSet);
        var fileName2 = exportDataSet.ExportToFile(fileName);
        Logger.LogInformation("Exportando a Excel: {path}", fileName2);

        // * download the file
        await JSRuntime.InvokeVoidAsync("downloadFromUrl", new {
            url = $"/api/download/{fileId}",
            fileName = $"resultados-{DateTime.Now.Ticks}.xlsx"
        });


    }
}
