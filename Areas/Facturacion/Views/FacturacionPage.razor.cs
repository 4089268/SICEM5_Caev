using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Grids;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Facturacion.Data;
using Sicem_Blazor.Facturacion.Models;
using Sicem_Blazor.Areas.Facturacion.Views;

namespace Sicem_Blazor.Facturacion.Pages;

public partial class FacturacionPage : ComponentBase {

    [Inject]
    public ILogger<FacturacionPage> Logger {get;set;}

    [Inject]
    public IMatToaster Toaster {get;set;}
    
    [Inject]
    public FacturacionService facturacionService {get;set;}
    
    [Inject]
    public SicemService sicemService {get;set;}
    
    [Inject]
    public IJSRuntime JSRuntime {get;set;}

    private SfGrid<Facturacion_Oficina> dataGrid {get;set;}
    private List<Facturacion_Oficina> datosFacturacion {get;set;}
    private bool busyDialog = false;

    private DateTime Fecha1, Fecha2;
    private int Subsistema, Sector;
    private Facturacion_ConceptosVtn facturacionConceptos {get;set;}
    private Facturacion_AnualVtn FacturacionAnualVtn {get;set;}
    private Facturacion_LocalidadesVtn facturacionLocalidadesVtn {get; set;}
    private bool facturacionConceptos_Visible = false, FacturacionAnual_Visible = false, FacturacionLocalidad_Visible = false;
    private IEnlace EnlaceSeleccionado;


    protected override void OnInitialized()
    {
        var _now = DateTime.Now.AddMonths(-1);
        this.Fecha1 = new DateTime(_now.Year, _now.Month, 1);
        this.Fecha2 = new DateTime(_now.Year, _now.Month, DateTime.DaysInMonth(_now.Year, _now.Month));
        this.Subsistema = 0;
        this.Sector = 0;
    }
    
    public void Procesar(SeleccionarFechaEventArgs e)
    {
        this.Fecha1 = e.Fecha1;
        this.Fecha2 = e.Fecha2;
        this.Subsistema = e.Subsistema;
        this.Sector = e.Sector;

        datosFacturacion = new List<Facturacion_Oficina>();
        var enlaces = sicemService.ObtenerOficinasDelUsuario();
        var oficinas = sicemService.ObtenerEnlaces().Where(item => enlaces.Select(i => i.Id).Contains(item.Id)).ToArray();
        var tareas = new List<Task>();

        foreach(Ruta oficina in oficinas){
            var newItem = new Facturacion_Oficina(){
                Estatus = 0,
                Id_Oficina = oficina.Id,
                Oficina = oficina.Oficina
            };
            this.datosFacturacion.Add(newItem);
            tareas.Add( new Task( async () =>
                {
                    ProcesarConsulta(oficina);
                    await InvokeAsync(StateHasChanged);
                }
            ));
        }

        //*** Fila total
        if(oficinas.Length > 1){
            var itemTotal = new Facturacion_Oficina(){
                Estatus = 0,
                Id_Oficina = 999,
                Oficina = "TOTAL"
            };
            this.datosFacturacion.Add(itemTotal);
        }

        tareas.ForEach(item => item.Start());
    }
    
    private void ProcesarConsulta(IEnlace enlace)
    {
        try
        {
            // * verify if the office is On
            if(!this.sicemService.CheckOfficeConnected(enlace))
            {
                throw new Exception($"The office {enlace.Nombre} is not connected");
            }

            var result = facturacionService.ObtenerFacturacionOficina(enlace, this.Fecha1.Year, this.Fecha1.Month, this.Subsistema, this.Sector );

            lock(datosFacturacion){
                var itemList = this.datosFacturacion.Where(item=> item.Id_Oficina == enlace.Id).FirstOrDefault();
                if(itemList != null){
                    itemList.Estatus = result.Estatus;
                    itemList.Domestico_Fact = result.Domestico_Fact;
                    itemList.Domestico_Usu = result.Domestico_Usu;
                    itemList.Hotelero_Fact = result.Hotelero_Fact;
                    itemList.Hotelero_Usu = result.Hotelero_Usu;
                    itemList.Comercial_Fact = result.Comercial_Fact;
                    itemList.Comercial_Usu = result.Comercial_Usu;
                    itemList.Industrial_Fact = result.Industrial_Fact;
                    itemList.Industrial_Usu = result.Industrial_Usu;
                    itemList.ServGener_Fact = result.ServGener_Fact;
                    itemList.ServGener_Usu = result.ServGener_Usu;
                    itemList.Subtotal = result.Subtotal;
                    itemList.Iva = result.Iva;
                    itemList.Total = result.Total;
                    itemList.Usuarios = result.Usuarios;
                }
                //**** Recalcular fila total
                RecalcularTotal();
                dataGrid.Refresh();
            }
        }
        catch(Exception)
        {
            lock(datosFacturacion)
            {
                var item = datosFacturacion.Where(item => item.Id_Oficina == enlace.Id).FirstOrDefault();
                if(item != null)
                {
                    item.Estatus = 2;
                }
            }
        }
    }
    
    private void RecalcularTotal()
    {
        var filaTotal = this.datosFacturacion.Where(item => item.Id_Oficina == 999).FirstOrDefault();
        if( filaTotal != null){
            var _tmpDatos = this.datosFacturacion.Where(item => item.Id_Oficina > 0 && item.Id_Oficina < 999).ToList();
            filaTotal.Domestico_Fact = _tmpDatos.Sum( item => item.Domestico_Fact);
            filaTotal.Domestico_Usu = _tmpDatos.Sum( item => item.Domestico_Usu);
            filaTotal.Hotelero_Fact = _tmpDatos.Sum( item => item.Hotelero_Fact);
            filaTotal.Hotelero_Usu = _tmpDatos.Sum( item => item.Hotelero_Usu);
            filaTotal.Comercial_Fact = _tmpDatos.Sum( item => item.Comercial_Fact);
            filaTotal.Comercial_Usu = _tmpDatos.Sum( item => item.Comercial_Usu);
            filaTotal.Industrial_Fact = _tmpDatos.Sum( item => item.Industrial_Fact);
            filaTotal.Industrial_Usu = _tmpDatos.Sum( item => item.Industrial_Usu);
            filaTotal.ServGener_Fact = _tmpDatos.Sum( item => item.ServGener_Fact);
            filaTotal.ServGener_Usu = _tmpDatos.Sum( item => item.ServGener_Usu);
            filaTotal.Subtotal = _tmpDatos.Sum( item => item.Subtotal);
            filaTotal.Iva = _tmpDatos.Sum( item => item.Iva);
            filaTotal.Total = _tmpDatos.Sum( item => item.Total);
            filaTotal.Usuarios = _tmpDatos.Sum( item => item.Usuarios);
        }
    }
    
    private async Task ExportarExcel_Click()
    {
        var p = new ExcelExportProperties();
        p.IncludeHiddenColumn = true;
        await this.dataGrid.ExportToExcelAsync(p);
    }

    
    #region Funciones Ventanas Secundarias
    private async Task FacturacionConceptos_Click(Facturacion_Oficina context)
    {
        if(facturacionConceptos_Visible)
        {
            return;
        }

        this.busyDialog = true;
        await Task.Delay(100);

        Ruta oficina = sicemService.ObtenerEnlaces(context.Id_Oficina).FirstOrDefault();
        var datos = facturacionService.ObtenerFacturacionConceptos(oficina, this.Fecha1.Year, this.Fecha1.Month, this.Subsistema, this.Sector);
        
        var catLocalidades = new Dictionary<int,string>();
        var _localidades = sicemService.ObtenerCatalogoLocalidades(oficina.Id).Where(i => i.Id_Poblacion > 0).ToList();
        catLocalidades.Add(0, "TODOS");
        foreach( var loc in _localidades){
            catLocalidades.Add(loc.Id_Poblacion, loc.Descripcion.ToUpper().Trim());
        }

        if(datos == null){
            Toaster.Add("Error al procesar la consulta, intentelo mas tarde.", MatToastType.Warning);
        }else{
            facturacionConceptos_Visible = true;
            facturacionConceptos.Titulo = $"CONCEPTOS FACTURADOS DEL {Fecha1.ToString("dd/MM/yyyy")} AL {Fecha2.ToString("dd/MM/yyyy")}  DE LA OFICINA {context.Oficina}";
            facturacionConceptos.Inicializar(oficina, datos, catLocalidades);
            
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("iniciarVentanaConceptos");
        }

        await Task.Delay(100);
        this.busyDialog = false;
    }
    
    private async Task FacturacionAnual_Click(Facturacion_Oficina context){
        if(FacturacionAnual_Visible || FacturacionAnualVtn == null){
            return;
        }

        this.busyDialog = true;
        await Task.Delay(100);

        var _oficina = sicemService.ObtenerEnlaces(context.Id_Oficina).FirstOrDefault();
        var _datos = facturacionService.ObtenerFacturacionAnual(_oficina, this.Fecha1.Year, this.Subsistema, this.Sector).ToList();
        if(_datos.Count > 0){
            FacturacionAnualVtn.Titulo = $"{_oficina.Oficina} - ANALISIS DE FACTURACION {this.Fecha1.Year}";
            FacturacionAnual_Visible = true;
            FacturacionAnualVtn.Inicializar(_datos, _oficina);
        }

        await Task.Delay(100);
        this.busyDialog = false;
    }
    
    private async Task FacturacionLocalidades_Click(Facturacion_Oficina context){
        if(FacturacionLocalidad_Visible){
            return;
        }

        this.busyDialog = true;
        await Task.Delay(100);

        this.EnlaceSeleccionado  = sicemService.ObtenerEnlaces(context.Id_Oficina).FirstOrDefault();
        var _datos = facturacionService.ObtenerFacturacionLocalidades(EnlaceSeleccionado, this.Fecha1.Year, this.Fecha1.Month, this.Subsistema, this.Sector).ToList();
        if(_datos.Count > 0){
            FacturacionLocalidad_Visible = true;
            facturacionLocalidadesVtn.Inicializar(_datos, EnlaceSeleccionado);
        }else{
            Toaster.Add("Error al realizar la consulta", MatToastType.Warning);
        }

        await Task.Delay(100);
        this.busyDialog = false;
    }
    #endregion
}