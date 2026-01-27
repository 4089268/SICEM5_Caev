
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.CFDI.Services;
using Sicem_Blazor.CFDI.Models;

namespace Sicem_Blazor.CFDI.Pages;
public partial class IndexPage
{

    [Inject]
    public NavigationManager NavManager {get; set;}
    
    [Inject]
    public IMatToaster Toaster {get; set;}
    
    [Inject]
    public FacturaService FacturaService1 {get; set;}
    
    [Inject]
    public SicemService SicemService {get; set;}

    private SfGrid<ResumenFactura> dataGrid = default;
    private List<ResumenFactura> datosOficina = new ();
    private bool busyDialog = false;
    private DateTime f1, f2;
    private int Subsistema, Sector;

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
        this.f1 = e.Fecha1;
        this.f2 = e.Fecha2;
        this.Subsistema = e.Subsistema;
        this.Sector = e.Sector;

        IEnlace[] enlaces = SicemService.ObtenerOficinasDelUsuario().ToArray();

        // * Preparar filas
        datosOficina = new List<ResumenFactura>();
        var tareas = new List<Task>();

        // * Prepara Filas
        foreach (var enlace in enlaces)
        {
            var oficinaModel = new ResumenFactura();
            oficinaModel.Enlace = enlace;
            datosOficina.Add(oficinaModel);

            // * Agregar tarea
            tareas.Add(new Task(() => ConsultarOficina(enlace)) );
        }

        // * Agregar fila total
        if (enlaces.Length > 1)
        {
            var oficinaModel = new ResumenFactura()
            {
                Estatus = ResumenOficinaEstatus.Completado,
                Enlace = new Ruta(){
                    Oficina = " TOTAL",
                    Id = 999
                }
            };
            datosOficina.Add(oficinaModel);
        }

        // * Iniciar tareas
        foreach (var tarea in tareas)
        {
            tarea.Start();
        }
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

            // * Realizar consulta
            ResumenFactura tmpDatos = this.FacturaService1.ObtenerResumenFactura(enlace, f1, f2);
            
            var _random = new Random();
            var sleep = _random.Next(3000);
            System.Threading.Thread.Sleep(sleep);

            // * Refrescar datos grid
            lock(datosOficina)
            {
                // * Actualizar fila grid
                ResumenFactura item = this.datosOficina.Where(item => item.Id == enlace.Id).FirstOrDefault();
                if (item != null)
                {
                    if (tmpDatos.Estatus == ResumenOficinaEstatus.Completado)
                    {
                        item.Estatus = ResumenOficinaEstatus.Completado;
                        item.TotalFacturas = tmpDatos.TotalFacturas;
                        item.FactsPorTimbrara = tmpDatos.FactsPorTimbrara;
                        item.FactsTimbrado = tmpDatos.FactsTimbrado;
                        item.FactsCancelado = tmpDatos.FactsCancelado;
                        item.SubTotal = tmpDatos.SubTotal;
                        item.Iva = tmpDatos.Iva;
                        item.Total = tmpDatos.Total;
                    }
                    else
                    {
                        item.Estatus = ResumenOficinaEstatus.Error;
                    }
                }

                RecalcularFilaTotal();
                dataGrid.Refresh();
            }
        }
        catch(Exception)
        {
            lock(datosOficina)
            {
                var item = datosOficina.Where(item => item.Enlace.Id == enlace.Id).FirstOrDefault();
                if(item != null)
                {
                    item.Estatus = ResumenOficinaEstatus.Error;
                }
                RecalcularFilaTotal();
                dataGrid.Refresh();
            }
        }
    }

    private void RecalcularFilaTotal()
    {
        // * Recalcular fila total
        var itemTotal = datosOficina.Where(item => item.Id == 999).FirstOrDefault();
        if (itemTotal != null) {
            var _tmpData = datosOficina.Where(item => item.Id != 999).ToList();
            itemTotal.TotalFacturas = _tmpData.Sum(item => item.TotalFacturas);
            itemTotal.FactsPorTimbrara = _tmpData.Sum(item => item.TotalFacturas);
            itemTotal.FactsTimbrado = _tmpData.Sum(item => item.FactsTimbrado);
            itemTotal.FactsCancelado = _tmpData.Sum(item => item.FactsCancelado);
            itemTotal.SubTotal = _tmpData.Sum(item => item.SubTotal);
            itemTotal.Iva = _tmpData.Sum(item => item.Iva);
            itemTotal.Total = _tmpData.Sum(item => item.Total);
        }
    }

    private async Task ExportarExcel_Click()
    {
        var _options = new ExcelExportProperties()
        {
            IncludeHiddenColumn = true
        };
        await this.dataGrid.ExportToExcelAsync(_options);
    }

}