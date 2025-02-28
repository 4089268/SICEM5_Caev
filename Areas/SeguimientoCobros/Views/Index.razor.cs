using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Grids;
using MatBlazor;
using Microsoft.JSInterop;
using System.Threading;
using Sicem_Blazor.SeguimientoCobros.Models;
using Sicem_Blazor.SeguimientoCobros.Data;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.SeguimientoCobros.Views;

public partial class Index : IAsyncDisposable
{
    [Inject]
    public IJSRuntime JSRuntime {get;set;}

    [Inject]
    public SeguimientoCobroService SeguimientoCobroService1 {get;set;}

    [Inject]
    public IMatToaster Toaster {get;set;}

    [Inject]
    public ILogger<Index> Logger {get;set;}

    [Inject]
    public SeguimientoCobroMapJsInterop MapJsInterop {get;set;}

    private DotNetObjectReference<Index> objRef;
    // private static Timer fetchTimer;
    private List<OfficePushpinMap> incomeData {get; set;}
    // private SfGrid<OfficePushpinMap> dataGrid;
    private string[] palettes = {"#2e86c1", "#28b463", "#d68910", "#884ea0"};

    protected override void OnInitialized()
    {
        objRef = DotNetObjectReference.Create(this);
        // * load offices with coordenates
        incomeData = SeguimientoCobroService1.GetOffices().ToList();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            await MapJsInterop.InitializedMapAsync(objRef, "map", new MapMark() );
            // await Task.Delay(8000);
            // fetchTimer = new Timer(FetchData, null, 0, 6000);

            var marks = this.incomeData.Select( item => new MapMark()
            {
                IdOficina = item.Id,
                Descripcion = item.Office,
                Latitude = double.Parse(item.Lat),
                Longitude = double.Parse(item.Lon)
                
            }).ToList();

            await MapJsInterop.UpdateMarks(objRef, marks);


        }
    }

    private async void FetchData(object state)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
        // try
        // {
        //     var tmpIncomeData = incomeOfficeService.GetIncomes().ToList();
        //     refreIncomesData(tmpIncomeData);
        //     await JSRuntime.InvokeVoidAsync("actualizarItems", tmpIncomeData );
        // }
        // catch(Exception err)
        // {
        //     Console.WriteLine(err.Message);
        // }
    }

    private void refreIncomesData(List<OfficePushpinMap> data)
    {
        // try
        // {
        //     foreach(var newData in data)
        //     {
        //         var refdata = incomeData.Where(item => item.Id == newData.Id).FirstOrDefault();
        //         if(refdata != null)
        //         {
        //             refdata.Bills = newData.Bills;
        //             refdata.Income = newData.Income;
        //         }
        //     }
        //     dataGrid.Refresh();
        // }
        // catch(Exception err)
        // {
        //     Console.WriteLine(err.Message);
        // }
    }
    
    public async ValueTask DisposeAsync()
    {
        // await JSRuntime.InvokeVoidAsync("cerrarVentanaCobroCaja");
        // try
        // {
        //     fetchTimer.Dispose();
        // }
        // catch(Exception)
        // {
        //     //
        // }
        await Task.CompletedTask;
    }

    [JSInvokable]
    public void MapLoaded()
    {
        //Logger.LogInformation("Mapa cargado");
    }
}