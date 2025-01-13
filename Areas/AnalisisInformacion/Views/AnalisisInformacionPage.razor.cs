using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.AnalisisInformacion.Models;

namespace Sicem_Blazor.AnalisisInformacion.Views;

public partial class AnalisisInformacionPage
{
    [Inject]
    public SicemService SicemService1 {get;set;}
    
    [Inject]
    public ILogger<AnalisisInformacionPage> Logger {get;set;}
    
    [Inject]
    public AnalisisInformacionService AnalisisInfoService {get;set;}
    
    [Inject]
    public IMatToaster MatToaster {get;set;}
    
    [Inject]
    public IJSRuntime JSRuntime {get;set;}
    
    [Inject]
    public IConfiguration Configuration {get;set;}


    private SfGrid<CatPadron> DataGrid {get;set;}
    private List<CatPadron> DatosGrid {get;set;} = new();
    private bool cargando = false;
    private bool mostrarResultados = false;
    private bool enableGridPaging = false;
    
    private AnalisysInfoFilter filtroBusqueda = new AnalisysInfoFilter();
    private bool datosGeneralesVisible = false;
    private string cuentaActual = "";
    private int oficinaActual = 0;
    private bool mostrarUbicaciones = false;
    private bool notificarDialogVisible = false;
    private List<MapMark> mapPoints = new List<MapMark>();
    private List<CatPadron> UsuariosANotificar { get; set;} = new List<CatPadron>();

    private Dictionary<int, string> catOficinas = new();
    private Dictionary<int,string> catEstatus = new();
    private Dictionary<int,string> catTipoCalculo = new();
    private Dictionary<int,string> catSetvicios = new();
    private Dictionary<int,string> catTarifas = new();
    private Dictionary<int,string> catAnomalias = new();
    private Dictionary<int,string> catGiro = new();
    private Dictionary<int,string> catClaseUsuaro = new();

    protected override async Task OnParametersSetAsync()
    {
        this.cargando = true;
        await Task.Delay(200);

        // * getting the catalog of offices availables
        catOficinas = SicemService1.ObtenerEnlaces().Where(item => item.Inactivo != true).ToList().ToDictionary(el => el.Id, el => el.Oficina);

        // * get offices assigned to the user
        var userOffices = SicemService1.ObtenerOficinasDelUsuario();
        IEnlace firstOffice = userOffices.FirstOrDefault() ?? throw new Exception("The user has no offices assigned");
        
        Logger.LogDebug("Getting the catalogs from the office {officeId}:{officeName}", firstOffice.Id, firstOffice.Nombre);
        await LoadTheCatalogs(firstOffice);
        
        this.cargando = false;

        StateHasChanged();
    }

    private async Task LoadTheCatalogs(IEnlace e)
    {
        catEstatus = SicemService1.ObtenerCatalogoEstatus(e.Id, "CAT_PADRON");
        catTipoCalculo = SicemService1.ObtenerCatalogoTipoCalculo(e.Id);
        catSetvicios = SicemService1.ObtenerCatalogoServicios(e.Id);
        catTarifas = SicemService1.ObtenerCatalogoTarifas(e.Id);
        catAnomalias = SicemService1.ObtenerCatalogoAnomalias(e.Id);
        catGiro = SicemService1.ObtenerCatalogoGiros(e.Id);
        catClaseUsuaro = SicemService1.ObtenerCatalogoClaseUsuario(e.Id);
        await Task.CompletedTask;
    }

    private async Task RealizarConsulta(AnalisysInfoFilter datosFiltro){
        if( datosFiltro == null ){
            return;
        }

        if( datosFiltro.Id_Oficinas.Count() <= 0 ){
            MatToaster.Add("Seleccione una oficina", MatToastType.Info);
            return;
        }

        Console.WriteLine(datosFiltro.Localidad);

        this.cargando = true;
        await Task.Delay(200);

        var _oficinas = SicemService1.ObtenerEnlaces().Where(item => item.Inactivo != true && datosFiltro.Id_Oficinas.Contains(item.Id) ).ToList();
        var _tmpDatos = await AnalisisInfoService.ObtenerAnalisisInfo(_oficinas, datosFiltro);
        this.enableGridPaging = _tmpDatos.Count() > 1000;
        this.DatosGrid = _tmpDatos;

        this.mostrarResultados = true;

        await Task.Delay(200);
        this.cargando = false;
    }

    private async Task ExportarExcel_Click(){
        cargando = true;
        await Task.Delay(100);

        var _tmpFolder = Configuration.GetValue<string>("TempFolder");
        var _exportador = new ExportarExcel<CatPadron>(DatosGrid, new Uri(_tmpFolder) );
        var _archivo = _exportador.GenerarExcel();

        if(!String.IsNullOrEmpty(_archivo)){
            var _endPointDownload = Configuration.GetSection("AppSettings").GetValue<string>("Direccion_Api");
            var _targetUrl = $"{_endPointDownload}/download/{_archivo}";
            await JSRuntime.InvokeVoidAsync("OpenNewTabUrl", _targetUrl);
        }
        
        await Task.Delay(500);
        cargando = false;
    }
    
    private void MostrarEnConsultaGeneral(CatPadron e){
        this.cuentaActual = e.Id_Cuenta.ToString();
        this.oficinaActual = e.Id_Oficina;
        datosGeneralesVisible = true;
    }
    
    private void MostrarUbicacion_Click(CatPadron e){
        var _newItem = new MapMark();
        _newItem.Latitude = (double)e.Latitude;
        _newItem.Longitude = (double)e.Longitude;
        _newItem.Zoom = 17;
        _newItem.Descripcion = $"{e.Id_Cuenta}";
        _newItem.Subtitulo = e.RazonSocial;
        _newItem.Padron = e;
        mapPoints.Clear();
        mapPoints.Add(_newItem);
        
        this.mostrarUbicaciones = true;
    }

    private void MostrarUbicacionesClick()
    {
        if(DatosGrid.Count() <= 0){
            return;
        }

        mapPoints.Clear();
        foreach(var pad in DatosGrid){
            var _newItem = new MapMark();
            _newItem.Latitude = (double)pad.Latitude;
            _newItem.Longitude = (double)pad.Longitude;
            _newItem.Zoom = 17;
            _newItem.Descripcion = $"{pad.Id_Cuenta}";
            _newItem.Subtitulo = pad.RazonSocial;
            _newItem.Padron = pad;
            _newItem.IdOficina = pad.Id_Oficina;
            mapPoints.Add(_newItem);
        }

        this.mostrarUbicaciones = true;

    }

    private async void HandleNotificationPanelClick(long? id_padron)
    {
        UsuariosANotificar.Clear();
        if(id_padron != null)
        {
            var _padron = this.DatosGrid.Where(item => item.Id_Padron == id_padron).First();
            UsuariosANotificar.Add( _padron );
        }
        else
        {
            UsuariosANotificar.AddRange( DatosGrid.Where(item => item.TieneTelefono) );
        }

        notificarDialogVisible = true;
        await Task.Delay(100);
    }

    private void HandleNotificationClose()
    {
        notificarDialogVisible = false;
        UsuariosANotificar.Clear();
    }
}