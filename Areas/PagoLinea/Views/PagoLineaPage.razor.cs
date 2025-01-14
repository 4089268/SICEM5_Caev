using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Charts;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Services.PagoLinea;
using Sicem_Blazor.Models;
using Sicem_Blazor.Models.PagoLinea;
using System.Globalization;

namespace Sicem_Blazor.PagoLinea.Views
{
    public partial class PagoLineaPage
    {

        [Inject]
        public NavigationManager NavManager {get;set;} = default!;
        
        [Inject]
        public IMatToaster Toaster {get;set;} = default!;
        
        [Inject]
        public HttpPagoLineaService PagoLineaService {get;set;} = default!;
        
        [Inject]
        public SicemService SicemService {get;set;} = default!;
        
        [Inject]
        public ILogger<PagoLineaPage> Logger {get;set;} = default!;


        private SfGrid<ResumeOffice> DataGrid {get;set;}
        private SfChart GraficaIngresos { get; set; }
        private DetallePagosVtn detallePagosVtn;
        private DetalleOficinaVtn detalleOficinaVtn;

        private bool busyDialog = false;
        private DateTime f1, f2;
        private int Subsistema, Sector;
        private ResumeMonth resumeIncomeMonth = null;
        private List<ResumeOffice> DataOffices {get;set;}
        private List<ChartItem> DatosGraficaImporte { get; set; }

        private CultureInfo currentCultueInfo = new("es-MX");

        protected override async Task OnInitializedAsync()
        {
            var _now = DateTime.Now.AddMonths(-1);
            this.f1 = new DateTime(_now.Year, _now.Month, 1);
            this.f2 = new DateTime(_now.Year, _now.Month, DateTime.DaysInMonth(_now.Year, _now.Month));
            this.Subsistema = 0;
            this.Sector = 0;

            await Procesar(new SeleccionarFechaEventArgs {
                Fecha1 = f1,
                Fecha2 = f2,
            });
        }

        public async Task Procesar(SeleccionarFechaEventArgs e)
        {
            this.busyDialog = true;
            await Task.Delay(100);

            this.f1 = e.Fecha1;
            this.f2 = e.Fecha2;
            this.Subsistema = e.Subsistema;
            this.Sector = e.Sector;

            resumeIncomeMonth = await this.PagoLineaService.GetResumeMonth(f1.Year, f1.Month);
            this.DataOffices = resumeIncomeMonth.Offices.OrderBy(item => item.OfficeName).ToList();


            // * generate the chart data
            DatosGraficaImporte = this.DataOffices.Select(item => new ChartItem {
                Id = item.OfficeId,
                Descripcion = item.OfficeName,
                Valor1 = item.TotalIncome
            }).OrderBy( item => item.Valor1).ToList();
            await GraficaIngresos.RefreshAsync();

            this.busyDialog = false;
            StateHasChanged();
        }
        
        private async Task ExportarExcel_Click(){
            var _options = new ExcelExportProperties()
            {
                IncludeHiddenColumn = true
            };
            await this.DataGrid.ExportToExcelAsync(_options);
        }

        private async Task HandleShowDetails(ResumeOffice office)
        {
            await Task.CompletedTask;
            throw new NotImplementedException("");
            // this.busyDialog = true;
            // await Task.Delay(100);

            // IEnumerable<OprVenta> data = [];
            // try
            // {
            //     data = this.PagoLineaService.ObtenerDetallePagos(office.Enlace, new DateRange(f1,f2));
            //     if (!data.Any())
            //     {
            //         throw new KeyNotFoundException("No hay datos disponibles");
            //     }
            // }
            // catch (KeyNotFoundException)
            // {
            //     Toaster.Add("No hay datos disponibles para este periodo", MatToastType.Info);
            //     this.busyDialog = false;
            //     return;
            // }
            // catch (System.Exception)
            // {
            //     Toaster.Add("Erro al obtener el detalle", MatToastType.Danger);
            //     this.busyDialog = false;
            //     return;
            // }


            // if(detallePagosVtn != null)
            // {
            //     var titulo = string.Format("Detalle de pagos del {0} al {1}, {2}", f1.ToShortDateString(), f2.ToShortDateString(), office.Enlace.Nombre);
            //     detallePagosVtn.Show(office.Enlace, data, titulo);
            // }

            // this.busyDialog = false;
        }

        private async Task HandleClosedDetallePagos(object e)
        {
            this.Logger.LogDebug("Detalle Pagos vtn closed");
            await Task.CompletedTask;
        }

        private async Task HandleShowDetailsOffice(ResumeOffice office)
        {
            this.busyDialog = true;
            await Task.Delay(100);

            try
            {
                var dataMovements = await this.PagoLineaService.GetMovements(f1, f2, office.OfficeId);
                var dataMovementsDays = await this.PagoLineaService.GetMovementsByDays(f1.Year, f2.Month, office.OfficeId);

                if(!dataMovements.Any())
                {
                    throw new KeyNotFoundException("No hay datos disponibles");
                }

                if(detalleOficinaVtn != null)
                {
                    var titulo = string.Format("{0}, Pagos realizados {1}", office.OfficeName, f1.ToString("MMM yyyy"));
                    detalleOficinaVtn.Show(office, dataMovements, dataMovementsDays, titulo);
                }

                this.busyDialog = false;

            }
            catch (KeyNotFoundException)
            {
                Toaster.Add("No hay datos disponibles para este periodo", MatToastType.Info);
                this.busyDialog = false;
                return;
            }
            catch(Exception err)
            {
                Toaster.Add("Erro al obtener los datos de la oficina.", MatToastType.Danger);
                this.busyDialog = false;
                return;
            }

        }

        private async Task HandleClosedDetalleOficina(object e)
        {
            this.Logger.LogDebug("Detalle Oficina vtn closed");
            await Task.CompletedTask;
        }

    }

}