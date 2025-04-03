using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Sicem_Blazor.Data;
using Sicem_Blazor.SqlManager.Models;

namespace Sicem_Blazor.SqlManager.Views.Partials;

public partial class ResultsVtn
{
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

    private async Task OnExportExcel_Click()
    {
        await Task.CompletedTask;
        // var _guid = Guid.NewGuid().ToString().Replace("-","").Substring(0,10);
        // var _titulo = Titulo.Replace(" - ",".").Replace(" ","_").ToString();
        // var _p = new ExcelExportProperties(){
        //     FileName = $"{_titulo}-{_guid}.xlsx",
        //     IncludeHiddenColumn = true
        // };
        // await DataGrid.ExportToExcelAsync(_p);
    }

}