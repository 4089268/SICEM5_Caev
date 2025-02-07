using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Charts;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;
using Sicem_Blazor.PonteAlCorriente.Data;
using Sicem_Blazor.PonteAlCorriente.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Syncfusion.Blazor.Data;

namespace Sicem_Blazor.PonteAlCorriente.Views
{
    public partial class PonteAlCorrientePage
    {

        [Inject]
        public IMatToaster Toaster {get;set;} = default!;
        
        [Inject]
        public SicemService SicemService {get;set;} = default!;

        [Inject]
        public PonteAlCorrienteService PonteAlCorrienteService1 {get;set;} = default!;
        
        [Inject]
        public ILogger<PonteAlCorrientePage> Logger {get;set;} = default!;


        private SfGrid<ResumeOffice> DataGrid {get;set;}
        private List<ResumeOffice> DataOffices {get;set;}
        private bool busyDialog = false;
        private DateTime f1, f2;
        private int Subsistema, Sector;
        private DetalleOficinaVtn detalleOficina;
        private bool detalleOficinaVisible = false;
        
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


            IEnlace[] enlaces = SicemService.ObtenerOficinasDelUsuario().ToArray();

            //****** Preparar filas
            DataOffices = new List<ResumeOffice>();
            var Tareas = new List<Task>();

            //****** Prepara Filas
            foreach (var enlace in enlaces)
            {
                var oficinaModel = new ResumeOffice(enlace);
                DataOffices.Add(oficinaModel);

                //*** Agregar tarea
                Tareas.Add(new Task(() => ConsultarOficina(enlace)) );
            }

            //*** Agregar fila total
            if (enlaces.Length > 1)
            {
                var oficinaModel = new ResumeOffice( new Ruta() { Oficina = "TOTAL", Id = 999 })
                {
                    Estatus = ResumenOficinaEstatus.Completado
                };
                DataOffices.Add(oficinaModel);
            }

            //****** Iniciar tareas
            foreach (var tarea in Tareas)
            {
                tarea.Start();
            }
            await Task.CompletedTask;
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
                var dateRange = new DateRange(f1, f2, Subsistema, Sector);
                var tmpData = PonteAlCorrienteService1.ObtenerResumen(enlace, dateRange);
                
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
                            item.Estatus = ResumenOficinaEstatus.Completado;
                            item.NumDescuentos = tmpData.NumDescuentos;
                            item.ImporteDescontado = tmpData.ImporteDescontado;
                            item.ImporteCobrado = tmpData.ImporteCobrado;
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
            var itemTotal = DataOffices.Where(item => item.Id == 999).FirstOrDefault();
            if (itemTotal != null) {
                var _tmpData = DataOffices.Where(item => item.Id != 999).ToList();

                itemTotal.NumDescuentos = _tmpData.Sum(item => item.NumDescuentos);
                itemTotal.ImporteDescontado = _tmpData.Sum(item => item.ImporteDescontado);
                itemTotal.ImporteCobrado = _tmpData.Sum(item => item.ImporteCobrado);
            }
            InvokeAsync(() => StateHasChanged());
        }
        
        private async Task ExportarExcel_Click(){
            var _options = new ExcelExportProperties()
            {
                IncludeHiddenColumn = true
            };
            await this.DataGrid.ExportToExcelAsync(_options);
        }
        
        private async Task DetalleClick(ResumeOffice data)
        {
            if (detalleOficinaVisible) {
                return;
            }

            this.busyDialog = true;
            await Task.Delay(200);
            var dateRange = new DateRange(f1, f2, Subsistema, Sector);
            var tmpData = PonteAlCorrienteService1.DetallePonteAlCorriente(data.Enlace, dateRange);
            if (tmpData == null) {
                Toaster.Add("Hubo un error al procesar la petición, inténtelo mas tarde.", MatToastType.Warning);
            }
            else {
                if (tmpData.Count() > 0)
                {
                    detalleOficinaVisible = true;
                    var titulo = $"{data.Enlace.Nombre.ToUpper()} - Detalle Cuentas";
                    detalleOficina.Show(tmpData, titulo);
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