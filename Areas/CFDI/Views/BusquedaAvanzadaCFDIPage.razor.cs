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

    private SfGrid<ClienteCFDI> DataGrid {get;set;} = default;
    private List<ClienteCFDI> datosGrid = new();
    private bool cargando = false;
    private bool mostrarResultados = true;
    private bool enableGridPaging = false;

    private string rfc = string.Empty;
    private string razonSocial = string.Empty;

    private async Task RealizarConsulta()
    {
        if(cargando)
        {
            return;
        }

        // * Validar datos
        if (string.IsNullOrWhiteSpace(this.rfc) && string.IsNullOrEmpty(razonSocial))
        {
            MatToaster.Add("Debe ingresar el RFC o la Razón Social", MatToastType.Danger);
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

        var oficinas = SicemService1.ObtenerEnlaces().Where(e => e.Inactivo != true).ToList();
        this.datosGrid = new List<ClienteCFDI>();

        // * realizar consulta
        var tareas = oficinas.Select(ofi => ConsultarOficina(ofi, this.rfc, this.razonSocial)).ToList();
        var results = await Task.WhenAll(tareas);

        // * combine the results from all tasks
        var datosCombinados = results.Where(r => r != null).SelectMany(r => r).ToList();

        this.datosGrid = datosCombinados;

        this.mostrarResultados = true;
        this.cargando = false;
        StateHasChanged();
    }

    private async Task<IEnumerable<ClienteCFDI>> ConsultarOficina(IEnlace enlace, string rfc, string razonSocial)
    {
        try
        {
            if(!this.SicemService1.CheckOfficeConnected(enlace))
            {
                throw new KeyNotFoundException("La oficina no esta disponible");
            }

            var data = this.FacturaService1.RealizarConsulta(enlace, rfc, razonSocial);
            return data;
        }
        catch(Exception ex)
        {
            this.Logger.LogError(ex, "Error al realizar la consulta de cientes CFDI de la oficina {oficina}: {message}", enlace.Nombre, ex.Message);
            return Array.Empty<ClienteCFDI>();
        }
    }

    private async Task ExportarExcel_Click()
    {
        // cargando = true;
        // await Task.Delay(100);

        // var _tmpFolder = Configuration.GetValue<string>("TempFolder");
        // var _exportador = new ExportarExcel<CatPadron>(DatosGrid, new Uri(_tmpFolder) );
        // var _archivo = _exportador.GenerarExcel();

        // if(!String.IsNullOrEmpty(_archivo)){
        //     var _endPointDownload = Configuration.GetSection("AppSettings").GetValue<string>("Direccion_Api");
        //     var _targetUrl = $"{_endPointDownload}/download/{_archivo}";
        //     await JSRuntime.InvokeVoidAsync("OpenNewTabUrl", _targetUrl);
        // }
        
        // await Task.Delay(500);
        // cargando = false;
    }
}