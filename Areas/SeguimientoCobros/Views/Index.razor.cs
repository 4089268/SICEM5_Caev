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

    [Inject]
    public IncomeDataService IncomeDataService1 {get;set;}

    private DotNetObjectReference<Index> objRef;
    private static Timer fetchTimer;
    private List<OfficePushpinMap> incomeData {get; set;}
    private SfGrid<OfficePushpinMap> dataGrid;
    private string[] palettes = {"#2e86c1", "#28b463", "#d68910", "#884ea0"};
    private List<LiveIncome> dataList = new();
    private List<MapMark> listMarks = new();

    protected override void OnInitialized()
    {
        objRef = DotNetObjectReference.Create(this);

        // * load offices with coordenates
        incomeData = SeguimientoCobroService1.GetOffices().ToList();

        dataList = IncomeDataService1.GetData();
        IncomeDataService1.OnChange += IncomeDataHasChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            await MapJsInterop.InitializedMapAsync(objRef, "map", new MapMark() );
            // await Task.Delay(8000);
            // fetchTimer = new Timer(FetchData, null, 0, 6000);

            listMarks = this.incomeData.Select( item => new MapMark()
            {
                IdOficina = item.Id,
                Descripcion = item.Office,
                Latitude = double.Parse(item.Lat),
                Longitude = double.Parse(item.Lon)
                
            }).ToList();

            await MapJsInterop.UpdateMarks(objRef, listMarks);
        }
    }

    public async ValueTask DisposeAsync()
    {

        IncomeDataService1.OnChange -= IncomeDataHasChanged;
        
        try
        {
            fetchTimer.Dispose();
        }
        catch(Exception)
        {
            //
        }
        await Task.CompletedTask;
    }

    [JSInvokable]
    public void MapLoaded()
    {
        //Logger.LogInformation("Mapa cargado");
    }

    [JSInvokable("PushpinClick")]
    public void PushpinClick()
    {
        //
    }

    public void IncomeDataHasChanged()
    {
        dataList = IncomeDataService1.GetData();

        var task = Task.Run( async () => {

            foreach(var m in listMarks)
            {
                if(dataList.Where(item => item.OficinaId == m.IdOficina).Any())
                {
                    var random = new Random();
                    var cobrado = dataList.FirstOrDefault(item => item.OficinaId == m.IdOficina).Cobrado + random.Next(1, 1000);
                    m.Subtitulo = cobrado.ToString("c2");
                }
            }

            await MapJsInterop.UpdateMarks(objRef, listMarks);
            await InvokeAsync(StateHasChanged);
        });
    }
}