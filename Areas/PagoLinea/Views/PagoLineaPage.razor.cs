using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Charts;
using MatBlazor;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Models;
using Sicem_Blazor.Services;
using Sicem_Blazor.PagoLinea.Models;
using Sicem_Blazor.PagoLinea.Data;

namespace Sicem_Blazor.PagoLinea.Views
{
    public partial class PagoLineaPage
    {

        [Inject]
        public NavigationManager NavManager {get;set;} = default!;
        
        [Inject]
        public IMatToaster Toaster {get;set;} = default!;
        
        [Inject]
        public PagoLineaService PagoLineaService {get;set;} = default!;
        
        [Inject]
        public SicemService SicemService {get;set;} = default!;
        
        [Inject]
        public ILogger<PagoLineaPage> Logger {get;set;} = default!;


        private SfGrid<ResumeOffice> DataGrid {get;set;}
        private SfChart GraficaIngresos {get;set;}
        private SfChart GraficaUsuarios {get;set;}

        private bool busyDialog = false;
        private DateTime f1, f2;
        private int Subsistema, Sector;
        private List<ResumeOffice> DatosRecaudacion {get;set;}

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
            DatosRecaudacion = new List<ResumeOffice>();
            
            var Tareas = new List<Task>();

            // * Prepara Filas
            foreach (var enlace in enlaces)
            {
                var oficinaModel = new ResumeOffice (enlace);
                DatosRecaudacion.Add(oficinaModel);

                // * Agregar tarea
                Tareas.Add( Task.Run(() => ConsultarOficina(enlace)) );
            }

            // * Agregar fila total
            if (enlaces.Length > 1) {
                var enlaceTotal = new Ruta {
                    Id = 999,
                    Oficina =" TOTAL"
                };
                var oficinaModel = new ResumeOffice(enlaceTotal){
                    Estatus = ResumenOficinaEstatus.Completado
                };
                DatosRecaudacion.Add(oficinaModel);
            }

            // * Iniciar tareas
            foreach (var tarea in Tareas) {
                tarea.Start();
            }

        }

        private void ConsultarOficina(IEnlace enlace)
        {
            // * Realizar consulta
            var dateRange = new DateRange(f1, f2, Subsistema, Sector);
            ResumeOffice tmpDatos = PagoLineaService.ObtenerResumen(enlace, dateRange);
            
            var _random = new Random();
            var sleep = _random.Next(3000);
            System.Threading.Thread.Sleep(sleep);

            // * Refrescar datos grid
            lock(DatosRecaudacion)
            {
                // * Actualizar fila grid
                ResumeOffice item = DatosRecaudacion.Where(item => item.Id == enlace.Id).FirstOrDefault();
                if (item != null) {
                    if (tmpDatos.Estatus == ResumenOficinaEstatus.Completado) {
                        item.Estatus = ResumenOficinaEstatus.Completado;
                        item.UsuariosPropios = tmpDatos.UsuariosPropios;
                        item.ImportePropio = tmpDatos.ImportePropio;
                        item.UsuariosOtros = tmpDatos.UsuariosOtros;
                        item.ImporteOtros = tmpDatos.ImporteOtros;
                        item.Importe = tmpDatos.Importe;
                        item.Cobrado = tmpDatos.Cobrado;
                        item.Usuarios = tmpDatos.Usuarios;
                    }
                    else {
                        item.Estatus = ResumenOficinaEstatus.Error;
                    }
                }
                RecalcularFilaTotal();
                DataGrid.Refresh();
            }

            // * Actualizar grafica ingresos
            // lock(DatosGrafica_Ingresos){
            //     try{
            //         var itemGraf1 = DatosGrafica_Ingresos.Where(item => item.Id == enlace.Id).FirstOrDefault();
            //         if(itemGraf1 != null) {
            //             itemGraf1.Valor1 = tmpDatos.Ingresos_Propios;
            //             itemGraf1.Valor2 = tmpDatos.Ingresos_Otros;
            //         }
            //         graficaIngresos.RefreshAsync();
            //     }catch(Exception){ }
            // }

            // * Actualizar grafica usuarios
            // lock(DatosGrafica_Usuarios){
            //     try{
            //         var itemGraf2 = DatosGrafica_Usuarios.Where(item => item.Id == enlace.Id).FirstOrDefault();
            //         if (itemGraf2 != null) {
            //             itemGraf2.Valor1 = tmpDatos.Usuarios_Propios;
            //             itemGraf2.Valor2 = tmpDatos.Usuarios_Otros;
            //         }
            //         graficaUsuarios.RefreshAsync();
            //     }catch(Exception){ }
            // }
        }

        private void RecalcularFilaTotal()
        {
            // * recalcular fila total
            var itemTotal = DatosRecaudacion.Where(item => item.Id == 999).FirstOrDefault();
            if (itemTotal != null)
            {
                var _tmpData = DatosRecaudacion.Where(item => item.Id != 999).ToList();
                itemTotal.UsuariosPropios = _tmpData.Sum(item => item.UsuariosPropios);
                itemTotal.ImportePropio = _tmpData.Sum(item => item.ImportePropio);
                itemTotal.UsuariosOtros = _tmpData.Sum(item => item.UsuariosOtros);
                itemTotal.ImporteOtros = _tmpData.Sum(item => item.ImporteOtros);
                itemTotal.Importe = _tmpData.Sum(item => item.Importe);
                itemTotal.Cobrado = _tmpData.Sum(item => item.Cobrado);
                itemTotal.Usuarios = _tmpData.Sum(item => item.Usuarios);
            }
        }

        private async Task ExportarExcel_Click(){
            var _options = new ExcelExportProperties()
            {
                IncludeHiddenColumn = true
            };
            await this.DataGrid.ExportToExcelAsync(_options);
        }

        private async Task IngresosDiasClick(ResumeOffice data)
        {
            this.busyDialog = true;
            await Task.Delay(2000);
            this.busyDialog = false;
        }

    }
}