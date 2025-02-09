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


namespace Sicem_Blazor.PagoLinea.Views;
public partial class ResumenDiasVtn
{
    [Inject]
    public IMatToaster Toaster {get;set;} = default!;
    
    [Inject]
    public PagoLineaService PagoLineaService1 {get;set;} = default!;
    
    [Inject]
    public ILogger<ResumenDiasVtn> Logger {get;set;} = default!;
    
    [Parameter]
    public EventCallback CerrarModal { get; set; }

    public bool mostrarVentana = false;

    public string titulo = "Pagos realizados";
    private SfGrid<PagoLineaResumenDia> DataGrid {get;set;}
    public List<PagoLineaResumenDia> Datos {get;set;}
    private bool busyDialog = false;

    private PagoLineaDetalleVtn pagoLineaDetalleVtn;
    private bool detalleVtnVisible = false;

    public void Show(IEnumerable<PagoLineaResumenDia> datos, string titulo = "")
    {
        this.Datos = datos.ToList();
        if(!string.IsNullOrWhiteSpace(titulo))
        {
            this.titulo = titulo;
        }
        mostrarVentana = true;
        StateHasChanged();
    }

    private async Task HandleCloseModal()
    {
        mostrarVentana = false;
        StateHasChanged();
        await CerrarModal.InvokeAsync("");
    }

    private async Task ExportData()
    {
        await this.DataGrid.ExcelExport(new ExcelExportProperties {
            FileName = string.Format("sicem_ingresosxConceptos_{0}.xlsx", Guid.NewGuid().ToString().Replace("-", ""))
        });
    }

    private async Task DetalleClick(PagoLineaResumenDia data)
    {
        if (detalleVtnVisible) {
            return;
        }

        this.busyDialog = true;
        await Task.Delay(200);
        var dateRange = new DateRange(data.Fecha, data.Fecha, 0, 0);
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
                var titulo = $"{data.Enlace.Nombre.ToUpper()} - PAGOS DEL DIA {data.FechaString} ";
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