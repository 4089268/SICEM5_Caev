using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Grids;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;
using Sicem_Blazor.PagoLinea.Data;
using Sicem_Blazor.PagoLinea.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.PagoLinea.Views
{
    public partial class PagoLineaResumePage
    {

        [Inject]
        public NavigationManager NavManager {get;set;} = default!;
        
        [Inject]
        public IMatToaster Toaster {get;set;} = default!;
        
        [Inject]
        public PagoLineaService PagoLineaService1 {get;set;} = default!;
        
        [Inject]
        public SicemService SicemService {get;set;} = default!;
        
        [Inject]
        public ILogger<PagoLineaPage> Logger {get;set;} = default!;


        private SfGrid<PagoLineaResumen> DataGrid {get;set;}
        private bool busyDialog = false;
        private DateTime f1, f2;
        private int Subsistema, Sector;
        private List<PagoLineaResumen> DataOffices {get;set;}
        private PagoLineaResumen TotalData;
        private CultureInfo currentCultureInfo = new("es-MX");

        private ResumenDiasVtn resumenDiasVtn;
        private bool resumenDiasVisible = false;

        private PagoLineaDetalleVtn pagoLineaDetalleVtn;
        private bool detalleVtnVisible = false;
        
        protected override async Task OnInitializedAsync()
        {
            var _now = DateTime.Now;
            this.f1 = new DateTime(_now.Year, _now.Month, 1);
            this.f2 = new DateTime(_now.Year, _now.Month, DateTime.DaysInMonth(_now.Year, _now.Month));
            this.Subsistema = 0;
            this.Sector = 0;

            await Task.CompletedTask;
        }

        public async Task Procesar(SeleccionarFechaEventArgs e)
        {
            this.f1 = e.Fecha1;
            this.f2 = e.Fecha2;
            this.Subsistema = e.Subsistema;
            this.Sector = e.Sector;

            // * obtener enlaces del usuario
            IEnlace[] enlaces = SicemService.ObtenerOficinasDelUsuario().ToArray();

            // * Preparar filas
            DataOffices = new List<PagoLineaResumen>();
            var Tareas = new List<Task>();
            foreach (var enlace in enlaces)
            {
                var oficinaModel = new PagoLineaResumen(enlace);
                DataOffices.Add(oficinaModel);

                // * Preparar tareas
                Tareas.Add(new Task( async () => await ConsultarOficina(enlace)));
            }

            // * Agregar fila total
            TotalData = new PagoLineaResumen( new Ruta() { Oficina = "TOTAL", Id = 999 });
            

            // * Iniciar tareas
            foreach (var tarea in Tareas)
            {
                tarea.Start();
            }
            await Task.CompletedTask;
        }

        private async Task ConsultarOficina(IEnlace enlace)
        {
            try
            {
                // * verify if the office is On
                if(!this.SicemService.CheckOfficeConnected(enlace))
                {
                    throw new Exception($"The office {enlace.Nombre} is not connected");
                }
                
                // * Realizar consulta
                var dateRange = new DateRange(f1, f2, Subsistema, Sector);
                var tmpData = await PagoLineaService1.ObtenerRumenPagos(enlace, dateRange);
                
                var _random = new Random();
                var sleep = _random.Next(3000);
                System.Threading.Thread.Sleep(sleep);

                // * Refrescar datos grid
                lock(DataOffices)
                {
                    // * Actualizar fila grid
                    var item = DataOffices.Where(item => item.Id == enlace.Id).FirstOrDefault();
                    if (item != null)
                    {
                        if (tmpData.Estatus == ResumenOficinaEstatus.Completado)
                        {
                            item.TotalPagos = tmpData.TotalPagos;
                            item.TotalPagosAplicados = tmpData.TotalPagosAplicados;
                            item.TotalPagosPorAplicar = tmpData.TotalPagosPorAplicar;
                            item.ImportePagos = tmpData.ImportePagos;
                            item.ImportePagosAplicados = tmpData.ImportePagosAplicados;
                            item.ImportePagosPorAplicar = tmpData.ImportePagosPorAplicar;
                            item.Estatus = ResumenOficinaEstatus.Completado;
                        }
                        else
                        {
                            item.Estatus = ResumenOficinaEstatus.Error;
                        }
                    }

                    RecalcularFilaTotal();
                    DataGrid.Refresh();
                }
            }
            catch(Exception ex)
            {
                this.Logger.LogError(ex, "Fail at attemt to get data from {Oficina}", enlace.Nombre);
                lock(DataOffices){
                    var item = DataOffices.Where(item => item.Enlace.Id == enlace.Id).FirstOrDefault();
                    if(item != null)
                    {
                        item.Estatus = ResumenOficinaEstatus.Error;
                    }
                    RecalcularFilaTotal();
                    DataGrid.Refresh();
                }
            }
        }

        private void RecalcularFilaTotal()
        {
            //*** Recalcular fila total
            TotalData.TotalPagos = DataOffices.Sum(item => item.TotalPagos);
            TotalData.TotalPagosAplicados = DataOffices.Sum(item => item.TotalPagosAplicados);
            TotalData.TotalPagosPorAplicar = DataOffices.Sum(item => item.TotalPagosPorAplicar);
            TotalData.ImportePagos = DataOffices.Sum(item => item.ImportePagos);
            TotalData.ImportePagosAplicados = DataOffices.Sum(item => item.ImportePagosAplicados);
            TotalData.ImportePagosPorAplicar = DataOffices.Sum(item => item.ImportePagosPorAplicar);
            InvokeAsync(() => StateHasChanged());
        }
        
        private async Task ExportarExcel_Click()
        {
            var _options = new ExcelExportProperties()
            {
                IncludeHiddenColumn = true
            };
            await this.DataGrid.ExportToExcelAsync(_options);
        }

        private async Task ResumenDiasClick(PagoLineaResumen data)
        {
            if (resumenDiasVisible) {
                return;
            }

            this.busyDialog = true;
            await Task.Delay(200);
            var dateRange = new DateRange(f1, f2, Subsistema, Sector);
            var tmpData = await PagoLineaService1.ObtenerRumenPagosDia(data.Enlace, dateRange);
            if (tmpData == null)
            {
                Toaster.Add("Hubo un error al procesar la petición, inténtelo mas tarde.", MatToastType.Warning);
            }
            else
            {
                if(tmpData.Any())
                {
                    resumenDiasVisible = true;
                    var titulo = $"{data.Enlace.Nombre.ToUpper()} - Resumen por dias";
                    resumenDiasVtn.Show(tmpData, titulo);
                }
                else
                {
                    Toaster.Add("No hay datos disponibles para mostrar.", MatToastType.Info);
                }
            }
            await Task.Delay(200);
            this.busyDialog = false;
        }

        private async Task DetalleClick(PagoLineaResumen data)
        {
            if (detalleVtnVisible) {
                return;
            }

            this.busyDialog = true;
            await Task.Delay(200);
            var dateRange = new DateRange(f1,f2, 0, 0);
            var tmpData = await PagoLineaService1.ObtenerDetallePagos2(data.Enlace, dateRange);
            if (tmpData == null)
            {
                Toaster.Add("Hubo un error al procesar la petición, inténtelo mas tarde.", MatToastType.Warning);
            }
            else
            {
                if(tmpData.Any())
                {
                    detalleVtnVisible = true;
                    var titulo = $"{data.Enlace.Nombre.ToUpper()} - PAGOS DEL MES {f1.ToString("MMMM yyyy").ToUpper()}";
                    pagoLineaDetalleVtn.Show(data.Enlace, tmpData, titulo);
                }
                else
                {
                    Toaster.Add("No hay datos disponibles para mostrar.", MatToastType.Info);
                }
            }
            await Task.Delay(200);
            this.busyDialog = false;
        }
    }

}