
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Charts;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Recaudacion.Models;
using Sicem_Blazor.Recaudacion.Data;
using Sicem_Blazor.Recaudacion.Services;
using System.Data;
using System.Reflection;


namespace Sicem_Blazor.Recaudacion.Views;

public partial class RecaudacionAnaliticoPage
{

    [Inject]
    public NavigationManager NavManager {get; set;}
    
    [Inject]
    public IMatToaster Toaster {get; set;}
    
    [Inject]
    public AnaliticoService AnaliticoService1 {get; set;}

    [Inject]
    public ILogger<RecaudacionAnaliticoPage> Logger {get; set;}
    
    [Inject]
    public SicemService SicemService {get; set;}

    private SfChart chartAnalitico = default!;
    private bool busyDialog = false;
    private bool processing = false;
    private DateTime dateTime;
    private int totalYears = 3;
    public List<AnaliticoResumenAno> analiticoResumenList;
    public List<OfficeStatus> officeStatuses;
    public Dictionary<int, decimal> analiticoResumenTotal;
    public List<ChartItem> itemsGrafica;
    public string[] GraficaLabels
    {
        get {
            if( analiticoResumenList == null)
            {
                return new string[]{"2025"};
            }
            return analiticoResumenList.GroupBy(item => item.Ano).Select(g => g.Key.ToString()).ToArray();
        }
    }
    private static readonly SemaphoreSlim _lock = new(1, 1);
    private CultureInfo currentCultureInfo = new("es-MX");

    protected override void OnInitialized()
    {
        InitChartItems();
    }

    public async Task Procesar(SeleccionarFechaEventArgs e)
    {
        this.processing = true;
        await Task.Delay(100);
        StateHasChanged();

        // * prepare the range
        this.dateTime = e.Fecha1;
        
        // * get the offices related of the user
        IEnlace[] enlaces = SicemService.ObtenerOficinasDelUsuario().ToArray();

        // * prepara Filas
        var tareas = new List<Task>();
        analiticoResumenList = new List<AnaliticoResumenAno>();
        officeStatuses = new List<OfficeStatus>();
        foreach (var enlace in enlaces)
        {
            officeStatuses.Add( new OfficeStatus {
                Enlace = enlace,
                Estatus = ResumenOficinaEstatus.Pendiente
            });

            // * agregar tarea
            tareas.Add(Task.Run( async () => await ConsultarOficina(enlace)));
        }
        StateHasChanged(); // * refresh for draw the card with the status of the offices

        // * esperar tareas
        await Task.WhenAll(tareas.ToArray());
        this.processing = false;
        await InvokeAsync(StateHasChanged);

        // * process the chart
        await RecalcularGrafica();
    }

    private void InitChartItems()
    {
        itemsGrafica = new List<ChartItem>();
        for(int i = 0; i < 12; i++)
        {
            itemsGrafica.Add(
                new ChartItem {
                Descripcion = (new DateTime(2025, i+1 ,1)).ToString("MMM"),
                Valor1 = 0,
                Valor2 = 0,
                Valor3 = 0
                }
            );
        }
    }

    private async Task ConsultarOficina(IEnlace enlace)
    {
        try
        {
            // * verify if the office is On
            if(!this.SicemService.CheckOfficeConnected(enlace))
            {
                throw new Exception($"The office {enlace.Nombre} is not connected");
            }

            var tmpResponse = AnaliticoService1.ObtenerAnaliticoOficina(enlace, dateTime.Year, totalYears) ?? throw new Exception("Fail at attempt to get the data.");
            if(!tmpResponse.Any())
            {
                throw new DataException("The response has not elements.");
            }

            var _random = new Random();
            var sleep = _random.Next(3000);
            System.Threading.Thread.Sleep(sleep);

            // * update the status of the office
            var _office = this.officeStatuses.FirstOrDefault(item => item.Id == tmpResponse.First().Id);
            if(_office != null)
            {
                _office.Estatus = ResumenOficinaEstatus.Completado;
            }
            await InvokeAsync(StateHasChanged);

            // * append the data
            await _lock.WaitAsync();
            try
            {
                this.analiticoResumenList.AddRange(tmpResponse);
                RecalcularTotal();
            }
            catch(Exception err)
            {
                Logger.LogError(err, "Fail at process the data");
            }
            finally
            {
                _lock.Release(); // Always release the lock
            }
        }
        catch(Exception)
        {
            this.Logger.LogDebug(">> Exception office: " + enlace.Nombre);
            var targetResume = officeStatuses.FirstOrDefault(item => item.Id == enlace.Id);
            if (targetResume != null)
            {
                targetResume.Estatus = ResumenOficinaEstatus.Error;
            }
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    private void RecalcularTotal()
    {
        var _ruta = new Ruta {Id = 999, Oficina = "Total"};
        this.analiticoResumenTotal = this.analiticoResumenList
            .GroupBy(item => item.Ano)
            .Select(group => new {
                Ano = group.Key,
                Total = group.Sum(item => item.Total)
            })
            .ToDictionary(g => g.Ano, g => g.Total);
    }

    private async Task RecalcularGrafica()
    {
        var anios = this.analiticoResumenList.GroupBy(item => item.Ano).Select(grp => grp.Key).ToList();
        this.itemsGrafica = new List<ChartItem>();

        for(int monthN = 0; monthN < 12; monthN++)
        {
            var newChartitem = new ChartItem
            {
                Descripcion = (new DateTime(2025, monthN+1 ,1)).ToString("MMM"),
                Valor1 = 0m
            };

            for(int i = 0; i < anios.Count; i++)
            {
                try
                {
                    string propertyName = $"Valor{i+1}";
                    this.Logger.LogDebug("I: " + i + " " + propertyName);
                    var propertyInfo = newChartitem.GetType().GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        decimal value = this.analiticoResumenList.Where(item => item.Ano == anios[i]).Sum(item => item.GetValueByMonth(monthN+1));
                        propertyInfo.SetValue(newChartitem, value);
                    }
                    else
                    {
                        this.Logger.LogError("The property {name} is not found", propertyName );
                    }
                }
                catch(Exception err)
                {
                    this.Logger.LogError(err, "Fail at genereate the chart" );
                }
            }

            itemsGrafica.Add(newChartitem);
        }
        chartAnalitico.RefreshLiveData();
        await Task.CompletedTask;
    }
}

public class OfficeStatus
{
    public IEnlace Enlace {get;set;}
    public int Id { get => Enlace?.Id ?? 0; }
    public string Oficina {get => Enlace?.Nombre ?? String.Empty;}
    public ResumenOficinaEstatus Estatus {get; set;}
}