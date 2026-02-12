using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Data;
using Sicem_Blazor.CFDI.Services;
using Sicem_Blazor.CFDI.Models;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.CFDI.Pages;
public partial class BusquedaAvanzadaCFDIPage
{
    [Inject]
    public SicemService SicemService1 {get;set;}
    
    [Inject]
    public ILogger<BusquedaAvanzadaCFDIPage> Logger {get;set;}
    
    [Inject]
    public IMatToaster MatToaster {get;set;}
    
    [Inject]
    public IJSRuntime JSRuntime {get;set;}

    [Inject]
    public FacturaService FacturaService1 {get;set;}

    private SfGrid<Factura> DataGrid {get;set;} = default;
    private List<Factura> datosGrid = new();
    private bool cargando = false;
    private DateTime desde = DateTime.Now.AddDays(-1);
    private DateTime hasta = DateTime.Now;
    private string rfc = string.Empty;
    private string razonSocial = string.Empty;
    private string busyIndicatorText = "Realizando la búsqueda...";
    private Factura RegistroSeleccionado {get;set;} = null;
    private bool CargandoConceptos {get;set;} = true;
    private List<ConceptoFactura> ConceptosFacturados {get;set;}
    private IEnumerable<Ruta> oficinas = default;
    private bool[] oficinasFinalizadas = default;

    private async Task RealizarConsulta()
    {
        if(cargando)
        {
            return;
        }

        if(!string.IsNullOrEmpty(rfc))
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(this.rfc, @"^[A-ZÑ&]{3,4}\d{6}(?:(?=[A-Z0-9]{3})([A-Z0-9]{3})|(?=.*\d))?$"))
            {
                MatToaster.Add("El formato del RFC no es válido", MatToastType.Danger);
                return;
            }
        }

        this.cargando = true;
        await Task.Delay(200);

        oficinas = SicemService1.ObtenerEnlaces().Where(e => e.Inactivo != true).ToList();
        var _maxIdOficinas = oficinas.OrderByDescending(o => o.Id).First().Id + 1;
        oficinasFinalizadas = new bool[_maxIdOficinas];
        this.busyIndicatorText = string.Format("Realizando la búsqueda: {0} de {1}", 0, oficinas.Count() );
        StateHasChanged();

        this.datosGrid = new List<Factura>();

        // * realizar consulta
        var tareas = oficinas.Select(ofi => ConsultarOficina(ofi, this.rfc, this.razonSocial)).ToList();
        var results = await Task.WhenAll(tareas);

        // * combine the results from all tasks
        var datosCombinados = results.Where(r => r != null).SelectMany(r => r).ToList();

        this.datosGrid = datosCombinados;

        this.cargando = false;
        StateHasChanged();
    }

    private async Task<IEnumerable<Factura>> ConsultarOficina(IEnlace enlace, string rfc, string razonSocial)
    {
        IEnumerable<Factura> response = null;
        try
        {
            if(!this.SicemService1.CheckOfficeConnected(enlace))
            {
                throw new KeyNotFoundException("La oficina no esta disponible");
            }

            response = this.FacturaService1.RealizarConsulta(enlace, desde, hasta, rfc, razonSocial);
        }
        catch(Exception ex)
        {
            this.Logger.LogError(ex, "Error al realizar la consulta de cientes CFDI de la oficina {oficina}: {message}", enlace.Nombre, ex.Message);
            response = Array.Empty<Factura>();
        }

        lock(this.oficinasFinalizadas)
        {
            this.oficinasFinalizadas[enlace.Id] = true;
            this.busyIndicatorText = string.Format("Realizando la búsqueda: {0} de {1}", oficinasFinalizadas.Where(e => e).Count(), oficinas.Count() );
        }
        await InvokeAsync(StateHasChanged);
        return response;
    }

    private async Task OnGridRecord_Selected(RowSelectEventArgs<Factura> args)
    {
        if(args.Data != null){
            RegistroSeleccionado = args.Data;
            await CargarConceptos();
        }
    }
    private void OnGridRecord_UnSelected(RowDeselectEventArgs<Factura> args)
    {
        RegistroSeleccionado = null;
    }

    private async Task CargarConceptos()
    {
        if(RegistroSeleccionado == null)
        {
            return;
        }

        this.CargandoConceptos = true;
        await Task.Delay(200);

        var _idCFDI = RegistroSeleccionado.IdCFDI;
        this.ConceptosFacturados = this.FacturaService1.ObtenerConceptosFactura(RegistroSeleccionado.Enlace, _idCFDI).ToList();
        
        await Task.Delay(200);
        this.CargandoConceptos = false;
    }

    private async Task ExportarExcel_Click()
    {
        cargando = true;
        await Task.Delay(100);
        var props = new ExcelExportProperties
        {
            FileName = string.Format("Facturas_del_{0}_al_desde_{1}_{2}.xlsx", desde.ToString("yyyyMMdd"), hasta.ToString("yyyyMMdd"), Guid.NewGuid().ToString("n"))
        };
        await this.DataGrid.ExportToExcelAsync(props);
        cargando = false;
    }
}