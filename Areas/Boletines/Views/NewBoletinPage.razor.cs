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

namespace Sicem_Blazor.Boletines.Views
{
    public partial class NewBoletinPage
    {

        [Inject]
        public NavigationManager NavManager {get;set;} = default!;
        
        [Inject]
        public IMatToaster Toaster {get;set;} = default!;
        
        [Inject]
        public SicemService SicemService {get;set;} = default!;
        
        [Inject]
        public ILogger<NewBoletinPage> Logger {get;set;} = default!;

        [Inject]
        public NavigationManager NavigationManager1 {get;set;} = default!;


        private SfGrid<ResumeOffice> DataGrid {get;set;}
        
        private bool busyDialog = false;
        private DateTime f1, f2;
        private int Subsistema, Sector;
        private ResumeMonth resumeIncomeMonth = null;
        private List<ResumeOffice> DataOffices {get;set;}
        private CultureInfo currentCultueInfo = new("es-MX");

        protected override async Task OnInitializedAsync()
        {
            // var _now = DateTime.Now;
            // this.f1 = new DateTime(_now.Year, _now.Month, 1);
            // this.f2 = new DateTime(_now.Year, _now.Month, DateTime.DaysInMonth(_now.Year, _now.Month));
            // this.Subsistema = 0;
            // this.Sector = 0;

            // await Procesar(new SeleccionarFechaEventArgs {
            //     Fecha1 = f1,
            //     Fecha2 = f2,
            // });
        }

        public async Task Procesar(SeleccionarFechaEventArgs e)
        {
            // this.busyDialog = true;
            // await Task.Delay(100);

            // this.f1 = e.Fecha1;
            // this.f2 = e.Fecha2;
            // this.Subsistema = e.Subsistema;
            // this.Sector = e.Sector;

            // resumeIncomeMonth = await this.PagoLineaService.GetResumeMonth(f1.Year, f1.Month);
            // this.DataOffices = resumeIncomeMonth.Offices.OrderBy(item => item.OfficeName).ToList();


            // // * generate the chart data
            // DatosGraficaImporte = this.DataOffices.Select(item => new ChartItem {
            //     Id = item.OfficeId,
            //     Descripcion = item.OfficeName,
            //     Valor1 = item.TotalIncome
            // }).OrderBy( item => item.Valor1).ToList();
            // await GraficaIngresos.RefreshAsync();

            // this.busyDialog = false;
            // StateHasChanged();
        }
        
        private async Task GoBackClick()
        {
            await Task.CompletedTask;
            NavigationManager1.NavigateTo("/Boletines");
        }

    }

}