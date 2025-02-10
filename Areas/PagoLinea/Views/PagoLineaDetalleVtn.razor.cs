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
public partial class PagoLineaDetalleVtn
{
    [Inject]
    public IMatToaster Toaster {get;set;} = default!;
    
    [Inject]
    public PagoLineaService PagoLineaService1 {get;set;} = default!;
    
    [Inject]
    public ILogger<DetallePagosVtn> Logger {get;set;} = default!;
    
    [Parameter]
    public EventCallback CerrarModal { get; set; }


    public bool mostrarVentana = false;
    
    private IEnlace enlace = default!;

    public string titulo = "Detalle venta en linea";

    private SfGrid<PagoLineaDetalle> DataGrid {get;set;}
    public List<PagoLineaDetalle> Datos {get;set;}

    private bool taskOnProgress = false;

    public void Show(IEnlace enlace, IEnumerable<PagoLineaDetalle> datos, string titulo = "")
    {
        if(!string.IsNullOrWhiteSpace(titulo))
        {
            this.titulo = titulo;
        }
        this.enlace = enlace;
        this.Datos = datos.ToList();
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
        await this.DataGrid.ExcelExport(new ExcelExportProperties
        {
            FileName = string.Format("sicem_detallePagoLinea_{0}.xlsx", Guid.NewGuid().ToString().Replace("-", ""))
        });
    }

    private async Task AplicarPagoClick(PagoLineaDetalle detallePago)
    {
        var data = this.Datos.FirstOrDefault( item => item.Id == detallePago.Id);
        if(data == null) return;

        data.aplicarPagoResult = new AplicarPagoResult
        {
            Id = detallePago.Id,
            Fecha = detallePago.Fecha,
            Importe = detallePago.Importe,
            Estatus = ResumenOficinaEstatus.Pendiente
        };
        await Task.Delay(100);
        await InvokeAsync(() => StateHasChanged());
        this.DataGrid.Refresh();

        // * attempt to send the paymeny
        var response = await this.PagoLineaService1.AplicarPago(this.enlace, detallePago);
        if(response.Error > 0)
        {
            if(response.Mensaje.Contains("->"))
            {
                var message = $"rror al aplicar el pago: {response.Mensaje.Split("->").First().Trim()}".ToLower();
                Toaster.Add("E" + message, MatToastType.Danger, $"Pago {detallePago.Id} {detallePago.Fecha.ToShortDateString()}");
            }
            else
            {
                var message = $"error al aplicar el pago: {response.Mensaje}".ToLower();
                Toaster.Add("E" + message, MatToastType.Danger, $"Pago {detallePago.Id} {detallePago.Fecha.ToShortDateString()}");
            }
        }
        else
        {
            Toaster.Add("Pago aplicado con folio: " + response.FolioPago, MatToastType.Success, $"Pago {detallePago.Id} {detallePago.Fecha.ToShortDateString()}");
            data.Aplicado = data.Importe;
            data.Diferencia = 0;
        }

        // * updated the status on the grid
        data.aplicarPagoResult.Estatus = response.Estatus;
        await Task.Delay(100);
        await InvokeAsync(() => StateHasChanged());
        this.DataGrid.Refresh();
    }

    private async Task AplicarPagosClick()
    {
        if(taskOnProgress) return;


        // * Preparar filas
        taskOnProgress = true;
        var detallesPorAplicar = this.Datos.Where(item => item.Aplicado <= 0).ToList();
        foreach(var detalle in detallesPorAplicar)
        {
            var det = Datos.FirstOrDefault(item => item.Id == detalle.Id);
            if(det != null)
            {
                det.aplicarPagoResult = new AplicarPagoResult
                {
                    Id = det.Id,
                    Fecha = det.Fecha,
                    Importe = det.Importe,
                    Estatus = ResumenOficinaEstatus.Pendiente
                };
            }
        }
        await InvokeAsync( () => StateHasChanged());

        foreach(var detalle in detallesPorAplicar)
        {
            await Task.Run( async() => await AplicarPagoClick(detalle));
        }


        taskOnProgress = false;
        await InvokeAsync( () => StateHasChanged());
    }
}