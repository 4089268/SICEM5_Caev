
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Charts;
using MatBlazor;
using Sicem_Blazor.Recaudacion.Models;
using Sicem_Blazor.Recaudacion.Data;
using Sicem_Blazor.Services;
using Sicem_Blazor.Areas.Recaudacion.Views;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.Recaudacion.Pages;
public partial class RecaudacionPage
{

    [Inject]
    public NavigationManager NavManager {get; set;}
    
    [Inject]
    public IMatToaster Toaster {get; set;}
    
    [Inject]
    public IRecaudacionService recaudacionService {get; set;}
    
    [Inject]
    public SicemService sicemService {get; set;}

    private SfGrid<Recaudacion_Resumen> dataGrid { get; set; }
    
    private bool busyDialog = false;
    private DateTime f1, f2;
    private int Subsistema, Sector;
    private List<Recaudacion_Resumen> datosRecaudacion { get; set; }
    
    private Recaudacion_IngresosAnalitico VtnAnalitico;
    private bool VtnAnalitico_visible = false;

    private Recaudacion_IngresosRezago VtnRezago;
    private bool VtnRezago_visible = false;

    private Recaudacion_IngresosxDias VtnDias;
    private bool VtnDias_visible = false;

    private Recaudacion_IngresosxCajas VtnCajas;
    private bool VtnCajas_visible = false;

    private Recaudacion_IngresosConceptos VtnConceptos;
    private bool VtnConceptos_visible = false;

    private Recaudacion_LocalidadesVtn VtnPoblaciones;
    private bool VtnPoblaciones_Visible = false;

    private Recaudacion_ConceptosTUsuariosVtn VtnRConceptos;
    private bool VtnRConceptos_Visible = false;


    protected override void OnInitialized() {
        var _now = DateTime.Now;
        this.f1 = new DateTime(_now.Year, _now.Month, 1);
        this.f2 = new DateTime(_now.Year, _now.Month, DateTime.DaysInMonth(_now.Year, _now.Month));
        this.Subsistema = 0;
        this.Sector = 0;
    }

    public void Procesar(SeleccionarFechaEventArgs e) {

        this.f1 = e.Fecha1;
        this.f2 = e.Fecha2;
        this.Subsistema = e.Subsistema;
        this.Sector = e.Sector;

        IEnlace[] enlaces = sicemService.ObtenerOficinasDelUsuario().ToArray();

        //****** Preparar filas
        datosRecaudacion = new List<Recaudacion_Resumen>();
        var Tareas = new List<Task>();

        //****** Prepara Filas
        foreach (var enlace in enlaces) {
            var oficinaModel = new Recaudacion_Resumen();
            oficinaModel.Enlace  = enlace;
            datosRecaudacion.Add(oficinaModel);

            //*** Agregar tarea
            Tareas.Add(new Task(() => ConsultarOficina(enlace)) );
        }

        //*** Agregar fila total
        if (enlaces.Length > 1) {
            var oficinaModel = new Recaudacion_Resumen(){
                Estatus = ResumenOficinaEstatus.Completado,
                Enlace = new Ruta(){
                    Oficina =" TOTAL",
                    Id = 999
                }
            };
            datosRecaudacion.Add(oficinaModel);
        }

        //****** Iniciar tareas
        foreach (var tarea in Tareas) { tarea.Start(); }

    }

    private void ConsultarOficina(IEnlace enlace) {

        //*** Realizar consulta
        var dateRange = new DateRange(f1, f2, Subsistema, Sector);
        Recaudacion_Resumen tmpDatos = recaudacionService.ObtenerResumen(enlace, dateRange);
        
        var _random = new Random();
        var sleep = _random.Next(3000);
        System.Threading.Thread.Sleep(sleep);

        //*** Refrescar datos grid
        lock(datosRecaudacion){

            //*** Actualizar fila grid
            Recaudacion_Resumen item = datosRecaudacion.Where(item => item.Id == enlace.Id).FirstOrDefault();
            
            if (item != null) {
                if (tmpDatos.Estatus == ResumenOficinaEstatus.Completado) {
                    item.Estatus = ResumenOficinaEstatus.Completado;
                    item.Usuarios_Propios = tmpDatos.Usuarios_Propios;
                    item.Ingresos_Propios = tmpDatos.Ingresos_Propios;
                    item.Usuarios_Otros = tmpDatos.Usuarios_Otros;
                    item.Ingresos_Otros = tmpDatos.Ingresos_Otros;
                    item.Iva = tmpDatos.Iva;
                    item.Importe_Total = tmpDatos.Importe_Total;
                    item.Cobrado = tmpDatos.Cobrado;
                    item.Usuarios_Total = tmpDatos.Usuarios_Total;
                }
                else {
                    item.Estatus = ResumenOficinaEstatus.Error;
                }
            }

            RecalcularFilaTotal();
            dataGrid.Refresh();
        }
    }

    private void RecalcularFilaTotal() {
        //*** Recalcular fila total
        var itemTotal = datosRecaudacion.Where(item => item.Id == 999).FirstOrDefault();
        if (itemTotal != null) {
            var _tmpData = datosRecaudacion.Where(item => item.Id != 999).ToList();

            itemTotal.Usuarios_Propios = _tmpData.Sum(item => item.Usuarios_Propios);
            itemTotal.Ingresos_Propios = _tmpData.Sum(item => item.Ingresos_Propios);

            itemTotal.Usuarios_Otros = _tmpData.Sum(item => item.Usuarios_Otros);
            itemTotal.Ingresos_Otros = _tmpData.Sum(item => item.Ingresos_Otros);
            
            itemTotal.Iva = _tmpData.Sum(item => item.Iva);
            itemTotal.Importe_Total = _tmpData.Sum(item => item.Importe_Total);
            itemTotal.Cobrado = _tmpData.Sum(item => item.Cobrado);
            itemTotal.Usuarios_Total = _tmpData.Sum(item => item.Usuarios_Total);
        }
    }
    private async Task ExportarExcel_Click(){
        var _options = new ExcelExportProperties(){
            IncludeHiddenColumn = true
        };
        await this.dataGrid.ExportToExcelAsync(_options);
    }


    //********* Funciones Ventanas Secundarias *********
    private async Task Analitico_Click(Recaudacion_Resumen data) {
        if (VtnAnalitico_visible) {
            return;
        }
        this.busyDialog = true;
        await Task.Delay(200);
        var dateRange = new DateRange(f1, f2, Subsistema, Sector);
        var tmpdata = recaudacionService.ObtenerAnalisisIngresos(data.Enlace, dateRange);
        if (tmpdata == null) {
            Toaster.Add("Hubo un error al procesar la peticion, intentelo mas tarde.", MatToastType.Warning);
        }
        else {
            VtnAnalitico_visible = true;
            VtnAnalitico.Titulo = $"{data.Enlace.Nombre.ToUpper()} - INGRESOS ANALITICO";
            await VtnAnalitico.Inicializar(data.Enlace, tmpdata);
        }
        await Task.Delay(200);
        this.busyDialog = false;
    }
    private async Task Rezago_Click(Recaudacion_Resumen data) {
        if (VtnRezago_visible) {
            return;
        }
        this.busyDialog = true;
        await Task.Delay(200);
        var dateRange = new DateRange(f1, f2, Subsistema, Sector);
        var tmpData = recaudacionService.ObtenerRezago(data.Enlace, dateRange);
        if (tmpData == null) {
            Toaster.Add("Hubo un error al procesar la petición, inténtelo mas tarde.", MatToastType.Warning);
        }
        else {
            VtnRezago_visible = true;
            VtnRezago.Titulo = $"{data.Enlace.Nombre.ToUpper()} - ANALISIS DE REZAGO";
            await VtnRezago.Inicializar(data.Enlace, tmpData);
        }
        await Task.Delay(200);
        this.busyDialog = false;
    }
    private async Task IngresosDias_Click(Recaudacion_Resumen data) {
        if (VtnDias_visible) {
            return;
        }
        this.busyDialog = true;
        await Task.Delay(200);
        var dateRange = new DateRange(f1, f2, Subsistema, Sector);
        var tmpData = recaudacionService.ObtenerIngresosPorDias(data.Enlace, dateRange);
        if (tmpData == null) {
            Toaster.Add("Hubo un error al procesar la petición, inténtelo mas tarde.", MatToastType.Warning);
        }
        else {
            VtnDias_visible = true;
            VtnDias.Titulo = $"{data.Enlace.Nombre.ToUpper()} - INGRESOS DIAS";
            VtnDias.Inicializar(data.Enlace, tmpData);
        }
        await Task.Delay(200);
        this.busyDialog = false;
    }
    private async Task IngresosCajas_Click(Recaudacion_Resumen data) {
        //Toaster.Add("No Implementado", MatToastType.Danger);
        //return;

        if (VtnCajas_visible) {
            return;
        }
        this.busyDialog = true;
        await Task.Delay(200);
        var dateRange = new DateRange(f1, f2, Subsistema, Sector);
        var tmpData = recaudacionService.ObtenerIngresosPorCajas(data.Enlace, dateRange);
        if (tmpData == null) {
            Toaster.Add("Hubo un error al procesar la petición, inténtelo mas tarde.", MatToastType.Warning);
        }
        else {
            if (tmpData.Count() > 0) {
                VtnCajas_visible = true;
                VtnCajas.Titulo = $"{data.Enlace.Nombre.ToUpper()} - INGRESOS POR CAJAS";
                VtnCajas.Inicializar(data.Enlace, tmpData);
            }
            else {
                Toaster.Add("No hay datos disponibles para mostrar.", MatToastType.Info);
            }
        }
        await Task.Delay(200);
        this.busyDialog = false;
    }
    private async Task IngresosPoPoblacion_Click(Recaudacion_Resumen data) {
        this.busyDialog = true;
        await Task.Delay(100);
        if (!VtnPoblaciones_Visible) {
            var dateRange = new DateRange(f1, f2, Subsistema, Sector);
            var tmpData = recaudacionService.ObtenerRecaudacionLocalidades(data.Enlace, dateRange);
            if (tmpData == null) {
                Toaster.Add("Hubo un error al procesar la peticion, intentelo mas tarde.", MatToastType.Warning);
            }
            else {
                if (tmpData.Count() > 0) {
                    VtnPoblaciones_Visible = true;
                    VtnPoblaciones.Titulo = $"{data.Enlace.Nombre} - INGRESOS POR POBLACIONES";
                    VtnPoblaciones.Inicializar(tmpData, data.Enlace);
                }
                else {
                    Toaster.Add("No hay datos disponibles para mostrar.", MatToastType.Info);
                }
            }
        }
        await Task.Delay(100);
        this.busyDialog = false;
    }
    private async Task IngresosPoConceptos_Click(Recaudacion_Resumen data){
        this.busyDialog = true;
        await Task.Delay(100);
        var dateRange = new DateRange(f1, f2, Subsistema, Sector);
        var _datos = recaudacionService.ObtenerIngresosPorConceptos(data.Enlace, dateRange).ToList();
        if( _datos == null){
            Toaster.Add("Error al tratar de obtener los ingresos por conceptos", MatToastType.Danger);
        }else{
            if(_datos.Count() <= 0){
                Toaster.Add("No hay datos disponibles para este periodo", MatToastType.Info);
            }else{
                
                // Inicializar ventana secundaria
                VtnConceptos_visible = true;
                VtnConceptos.Inicializar(data.Enlace, _datos);
            }
        }

        await Task.Delay(100);
        this.busyDialog = false;
    }
    private async Task IngresosPorConceptosTiposUsuarios(Recaudacion_Resumen data){
        this.busyDialog = true;
        await Task.Delay(100);

        var dateRange = new DateRange(f1, f2, Subsistema, Sector);
        var _datos = recaudacionService.ObtenerIngresosPorConceptosTipoUsuarios(data.Enlace, dateRange).ToList();
        if( _datos == null){
            Toaster.Add("Error al tratar de obtener los ingresos por conceptos y tipos de usuario", MatToastType.Danger);
        }else{
            if(_datos.Count() <= 0){
                Toaster.Add("No hay datos disponibles para este periodo", MatToastType.Info);
            }else{
                
                // Inicializar ventana secundaria
                VtnRConceptos_Visible = true;
                VtnRConceptos.Inicializar(data.Enlace, _datos);
            }
        }

        await Task.Delay(100);
        this.busyDialog = false;
    }
    
}