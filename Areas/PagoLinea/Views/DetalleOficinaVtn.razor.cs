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
public partial class DetalleOficinaVtn
{
    [Inject]
    public IMatToaster Toaster {get;set;} = default!;
    
    [Inject]
    public PagoLineaService PagoLineaService {get;set;} = default!;
    
    [Inject]
    public ILogger<DetalleOficinaVtn> Logger {get;set;} = default!;
    
    [Parameter]
    public EventCallback CerrarModal { get; set; }

    public bool mostrarVentana = false;
    
    private ResumeOffice resumeOffice1;
    public string titulo = "Pagos realizados";
    private SfGrid<PaymentDetails> DataGrid {get;set;}
    public List<PaymentDetails> Datos {get;set;}
    public List<ResumeDay> IncomeDays {get;set;}
    
    public void Show(ResumeOffice resumeOffice, IEnumerable<PaymentDetails> datos, IEnumerable<ResumeDay> days, string titulo = "")
    {
        this.resumeOffice1 = resumeOffice;
        this.Datos = datos.ToList();
        this.IncomeDays = days.ToList();
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