using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MatBlazor;
using Syncfusion.Blazor.Grids;
using Sicem_Blazor.Models;
using Sicem_Blazor.SqlManager.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Services;

namespace Sicem_Blazor.Areas.SqlManager.Views;

public partial class Index
{
    
    [Inject]
    public SicemService SicemService1 {get; set;}

    [Inject]
    public IMatToaster Toaster {get; set;}

    private SfGrid<NotificacionTemplate> DataGrid {get;set;}
    private List<NotificacionTemplate> DatosGrid {get;set;}
    private NotificacionTemplate TemplateSeleccionado {get;set;}

    private string searchText = "";
    private bool cargando = false;
    private QueryModel selectedQuery = null;
    private IEnumerable<IEnlace> enlacesDisponibles = [];


    protected override void OnInitialized()
    {
        this.enlacesDisponibles = this.SicemService1.ObtenerEnlaces();
        this.selectedQuery = new QueryModel
        {
            Name = "Query 1",
            Query = "SELECT 'HOLA MUNDO' as message",
            Results = []
        };
    }

    public void AgregarNuevoClick()
    {
        Toaster.Add("Agregar nuevo", MatToastType.Success);
    }

    public void ClearSearchClick()
    {
        searchText = "";
    }

    public void OnGridSearchKeyUp(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            Toaster.Add("Buscar: " + searchText, MatToastType.Info);
        }
    }

    private void OnDataGrid_SelectionChanged(RowSelectEventArgs<NotificacionTemplate> e)
    {
        TemplateSeleccionado = e.Data;
    }
}
