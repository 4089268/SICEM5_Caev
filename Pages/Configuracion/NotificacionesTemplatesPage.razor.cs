using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.Pages.Configuracion;
public partial class NotificacionesTemplatesPage
{
    [Inject]
    public SicemService SicemService1 {get;set;}
    
    [Inject]
    public NotificacionesTemplateService NotificacionesTemplateService1 {get;set;}

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    private SfGrid<NotificacionTemplate> DataGrid {get;set;}
    private List<NotificacionTemplate> DatosGrid {get;set;}
    private NotificacionTemplate TemplateSeleccionado {get;set;}

    private bool cargando = false;
    private string searchText = "";

    protected override async Task OnInitializedAsync()
    {
        await CargarTemplates();
    }
    
    private void OnDataGrid_SelectionChanged(RowSelectEventArgs<NotificacionTemplate> e){
        TemplateSeleccionado = e.Data;
    }

    private async Task OnGridSearch_KeyUp(KeyboardEventArgs e){
        if(e.Key == "Enter"){
            await DataGrid.SearchAsync(searchText);
        }
    }
    private async Task ClearSearch_Click(){
        this.searchText = "";
        await OnGridSearch_KeyUp(new KeyboardEventArgs(){Key = "Enter"});
    }

    private void HandleOnPropClick(string propName){
        if(TemplateSeleccionado != null){
            TemplateSeleccionado.Texto = TemplateSeleccionado.Texto + @" ${" + propName + "}";
        }
    }

    private async Task CargarTemplates(){
        this.cargando = true;
        await Task.Delay(100);

        TemplateSeleccionado = null;
        this.DatosGrid = NotificacionesTemplateService1.ObtenerTemplateNotificaciones().ToList();

        await Task.Delay(100);
        this.cargando = false;
    }
    private async Task GuardarOCrear(){

        if(this.TemplateSeleccionado == null){
            return;
        }

        this.cargando = true;
        await Task.Delay(100);

        // Guarar template en la base de datos
        var id = NotificacionesTemplateService1.ActualizarTemplateNotificaciones(this.TemplateSeleccionado);
        Console.WriteLine($"Template {id}, modificado");

        await CargarTemplates();
    }
    private async Task GuardarCopia(){

        if(this.TemplateSeleccionado == null){
            return;
        }

        this.cargando = true;
        await Task.Delay(100);

        var newTemplate = new NotificacionTemplate(){
            Titulo = TemplateSeleccionado.Titulo,
            Texto = TemplateSeleccionado.Texto,
            FCreacion = DateTime.Now,
            UltimaMod = DateTime.Now
        };

        // Guarar template en la base de datos
        var id = NotificacionesTemplateService1.ActualizarTemplateNotificaciones(newTemplate);
        Console.WriteLine($"Template {id}, modificado");

        await CargarTemplates();
    }
    private void AgregarNuevoTemplateClick(){
        this.TemplateSeleccionado = new NotificacionTemplate(){
            Titulo = "Nuevo Template",
            Texto = "Mensaje a notificar",
            FCreacion = DateTime.Now,
            UltimaMod = DateTime.Now
        };
    }

}