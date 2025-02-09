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
            FileName = string.Format("sicem_ingresosxConceptos_{0}.xlsx", Guid.NewGuid().ToString().Replace("-", ""))
        });
    }

}