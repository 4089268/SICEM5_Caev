using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Sicem_Blazor.Data;
using Sicem_Blazor.Services;
using Sicem_Blazor.SqlManager.Models;

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

    [Parameter]
    public QueryModel Query {get;set;}

    [Parameter]
    public IEnumerable<IEnlace> Enlaces {get;set;}

    private List<Task> tareas;

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

        MatToaster.Add("Procesando consulta.", MatToastType.Success);
        await Task.CompletedTask;

        // * prepare the rows
        Query.Results = Query.Enlaces.Select(e => QueryResult.NewPending(e)).ToList();
        StateHasChanged();

        // * Prepare Tasks
        tareas = Query.Enlaces.Select( e => Task.Run( () => ProcessEnlace(e) )).ToList<Task>();

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

}
