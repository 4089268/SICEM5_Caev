
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Charts;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Recaudacion.Models;
using Sicem_Blazor.Recaudacion.Data;
using Sicem_Blazor.Recaudacion.Services;


namespace Sicem_Blazor.Recaudacion.Views;

public partial class RecaudacionAnaliticoPage
{

    [Inject]
    public NavigationManager NavManager {get; set;}
    
    [Inject]
    public IMatToaster Toaster {get; set;}
    
    [Inject]
    public AnaliticoService AnaliticoService1 {get; set;}
    
    [Inject]
    public SicemService SicemService {get; set;}


    private bool busyDialog = false;
    private DateTime f1, f2;
    private int Subsistema, Sector;

    public List<AnaliticoResumen> analiticoResumen;
    public List<ChartItem> itemsGrafica = new();


    protected override void OnInitialized()
    {
        var _now = DateTime.Now;
        this.f1 = new DateTime(_now.Year, _now.Month, 1);
        this.f2 = new DateTime(_now.Year, _now.Month, DateTime.DaysInMonth(_now.Year, _now.Month));
        this.Subsistema = 0;
        this.Sector = 0;
    }

    public void Procesar(SeleccionarFechaEventArgs e)
    {
        // * get the offices related of the user
        IEnlace[] enlaces = SicemService.ObtenerOficinasDelUsuario().ToArray();

        // * prepare chart items
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

    private void ConsultarOficina(IEnlace enlace)
    {
        try
        {
            // * verify if the office is On
            if(!this.SicemService.CheckOfficeConnected(enlace))
            {
                throw new Exception($"The office {enlace.Nombre} is not connected");
            }

            var tmpResponse = AnaliticoService1.ObtenerAnaliticoOficina(enlace, f1.Year);

            var _random = new Random();
            var sleep = _random.Next(3000);
            System.Threading.Thread.Sleep(sleep);

            // * Refrescar datos grid
            lock(analiticoResumen)
            {
                // * update witht he response data
                var targetResume = analiticoResumen.FirstOrDefault(item => item.Id == tmpResponse.Id);
                if (targetResume != null)
                {
                    if (tmpResponse.Estatus == ResumenOficinaEstatus.Completado)
                    {
                        targetResume.Estatus = ResumenOficinaEstatus.Completado;
                        targetResume.Anos = tmpResponse.Anos;
                    }
                    else
                    {
                        targetResume.Estatus = ResumenOficinaEstatus.Error;
                    }
                }

                RecalcularGrafica();
            }
        }
        catch(Exception)
        {
            lock(analiticoResumen)
            {
                var targetResume = analiticoResumen.FirstOrDefault(item => item.Id == enlace.Id);
                if (targetResume != null)
                {
                    targetResume.Estatus = ResumenOficinaEstatus.Error;
                }
                RecalcularGrafica();
            }
        }
    }

    private void RecalcularGrafica()
    {
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
    
}