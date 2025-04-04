using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MatBlazor;
using Sicem_Blazor.Data;
using Sicem_Blazor.SqlManager.Models;
using Sicem_Blazor.Helpers;

namespace Sicem_Blazor.SqlManager.Views.Partials;

public partial class ResultsVtn
{
    [Inject]
    public IMatToaster MatToaster {get;set;}

    [Inject]
    public IJSRuntime JSRuntime {get;set;}
    
    [Inject]
    public IConfiguration Configuration {get;set;}

    [Inject]
    public ILogger<ResultsVtn> Logger {get;set;}


    [Parameter]
    public EventCallback OnClose {get;set;}

    [Parameter]
    public bool Visible {get;set;}

    public QueryResult Results {get;set;}

    public IEnlace Enlace {get;set;}
    public string Titulo {get;set;} = "Ventana Resultados";

    private bool busyDialog = false;

    public void Inicializar(IEnlace enlace, QueryResult result)
    {
        this.Enlace = enlace;
        this.Results = result;
        // Titulo = $"{Enlace.Nombre} - Recaudacion Por Localidades";
    }

    private async Task CerrarModal()
    {
        await OnClose.InvokeAsync(null);
    }

    private async Task ExportExcelClick()
    {
        MatToaster.Add("Exportando a Excel", MatToastType.Info);

        // * prepare the file name
        var fileId = Guid.NewGuid();
        var _tmpFolder = Configuration.GetValue<string>("TempFolder");
        var fileName = System.IO.Path.Combine(_tmpFolder, fileId.ToString(),$"resultados-{DateTime.Now.Ticks}.xlsx");
        
        // * export data to file
        var exportDataSet = new ExportDataSet(Results.Result);
        var fileName2 = exportDataSet.ExportToFile(fileName);
        Logger.LogInformation("Exportando a Excel: {path}", fileName2);

        // * download the file
        await JSRuntime.InvokeVoidAsync("downloadFromUrl", new {
            url = $"/api/download/{fileId}",
            fileName = $"resultados-{DateTime.Now.Ticks}.xlsx"
        });
    }

}