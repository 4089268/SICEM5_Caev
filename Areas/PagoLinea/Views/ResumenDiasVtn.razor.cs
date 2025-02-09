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
using Sicem_Blazor.Models.PagoLinea;

namespace Sicem_Blazor.PagoLinea.Views;
public partial class ResumenDiasVtn
{
    [Inject]
    public IMatToaster Toaster {get;set;} = default!;
    
    [Inject]
    public PagoLineaService PagoLineaService {get;set;} = default!;
    
    [Inject]
    public ILogger<ResumenDiasVtn> Logger {get;set;} = default!;
    
    [Parameter]
    public EventCallback CerrarModal { get; set; }

    public bool mostrarVentana = false;
    
    private ResumeOffice resumeOffice1;
    public string titulo = "Pagos realizados";
    private SfGrid<PagoLineaResumenDia> DataGrid {get;set;}
    public List<PagoLineaResumenDia> Datos {get;set;}
    
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

}