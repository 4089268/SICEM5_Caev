
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Charts;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Recaudacion.Models;
using Sicem_Blazor.Recaudacion.Data;
using Sicem_Blazor.Recaudacion.Services;
using System.Threading;
using Microsoft.Extensions.Logging;


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
    private DateTime f1, f2;

    public List<AnaliticoResumen> analiticoResumenList;
    public AnaliticoResumen analiticoResumenGlobal;
    public ChartItem[] itemsGrafica;
    public string[] GraficaLabels
    {
        get {
            if( analiticoResumenGlobal == null)
            {
                return new string[]{"2025", "2024", "2023"};
            }
            return analiticoResumenGlobal.Anos.Select(item => item.Ano.ToString()).OrderByDescending(item => item).ToArray();
        }
    }
    private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);


    protected override void OnInitialized()
    {
        InitChartItems();
    }

    public async Task Procesar(SeleccionarFechaEventArgs e)
    {
        this.busyDialog = true;
        await Task.Delay(100);
        StateHasChanged();

        // * prepare the range
        var targetYear = e.Fecha1.Year;
        f1 = new DateTime(targetYear-3, 1, 1);
        f2 = new DateTime(targetYear, 12, 31);

        // * get the offices related of the user
        IEnlace[] enlaces = SicemService.ObtenerOficinasDelUsuario().ToArray();

        // * prepare chart items
        InitChartItems();
        await chartAnalitico.RefreshAsync();

        InitAnaliticoResumenGlobal();
        
        // * prepara Filas
        var tareas = new List<Task>();
        analiticoResumenList = new List<AnaliticoResumen>();
        foreach (var enlace in enlaces)
        {
            var resumenOficina = new AnaliticoResumen(enlace, f2.Year);
            analiticoResumenList.Add(resumenOficina);

            //*** Agregar tarea
            tareas.Add(new Task( async () => await ConsultarOficina(enlace)) );
        }
        StateHasChanged();

        // * iniciar tareas
        foreach (var tarea in tareas)
        {
            tarea.Start();
        }

        // * esperar tareas
        Task.WaitAll(tareas.ToArray());
        this.busyDialog = false;
    }

    private void InitChartItems()
    {
        itemsGrafica = new ChartItem[12];
        for(int i = 0; i < 12; i++)
        {
            itemsGrafica[i] = new ChartItem {
                Descripcion = (new DateTime(2025, i+1 ,1)).ToString("MMM"),
                Valor1 = 0,
                Valor2 = 0,
                Valor3 = 0
            };
        }
    }

    private void InitAnaliticoResumenGlobal()
    {
        var anosList = new List<AnaliticoResumenAno>();
        for(int year = f2.Year; year > f1.Year; year --)
        {
            anosList.Add(new AnaliticoResumenAno(year));
        }

        analiticoResumenGlobal = new AnaliticoResumen(new Ruta {
            Id = 999,
            Oficina = "Total",
        }){
            Anos = anosList
        };
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

            var tmpResponse = AnaliticoService1.ObtenerAnaliticoOficina(enlace, f2.Year);

            // **** print response for debug ****
            // Console.WriteLine(">> Oficina: " + tmpResponse.Enlace.Nombre);
            // foreach(var resumenAno in tmpResponse.Anos)
            // {
            //     Console.Write(resumenAno.Ano);
            //     for(int i = 0; i < 12; i++)
            //     {
            //         Console.Write( $" {i+1}:{resumenAno.Meses[i].ToString("c2")}");
            //     }
            //     Console.Write("\n");
            // }
            // **********************************

            var _random = new Random();
            var sleep = _random.Next(3000);
            System.Threading.Thread.Sleep(sleep);

            // * Refrescar datos grid
            await _lock.WaitAsync();
            try
            {
                // * update with the response data
                var targetResume = analiticoResumenList.FirstOrDefault(item => item.Id == tmpResponse.Id);
                if (targetResume != null)
                {
                    if (tmpResponse.Estatus == ResumenOficinaEstatus.Completado)
                    {
                        targetResume.Estatus = ResumenOficinaEstatus.Completado;
                        targetResume.Anos = tmpResponse.Anos;
                    }
                    else
                    {
                        targetResume.Estatus = ResumenOficinaEstatus.Error;
                    }
                }

                RecalcularTotal();
                await RecalcularGrafica(); // Now it can be awaited safely
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
            await _lock.WaitAsync();
            try
            {
                var targetResume = analiticoResumenList.FirstOrDefault(item => item.Id == enlace.Id);
                if (targetResume != null)
                {
                    targetResume.Estatus = ResumenOficinaEstatus.Error;
                }
            }
            finally
            {
                _lock.Release(); // Always release the lock
            }
        }
    }

    private void RecalcularTotal()
    {
        var responseTotal = new Dictionary<int, decimal[]>();

        // * sum the months
        var anios = analiticoResumenGlobal.Anos.Select(item => item.Ano);
        foreach(int targetYear in anios)
        {
            // * get the data from the target year
            var analiticoByYear = analiticoResumenList
                .Select(rm => rm.Anos.FirstOrDefault(peridod => peridod.Ano == targetYear))
                .Where(item => item != null)
                .ToArray();

            this.Logger.LogDebug(">> Total analiticoByYear: {total}", analiticoByYear.Count() );
            
            try
            {
                decimal[] sumAnalitico = analiticoByYear.Aggregate((acc, next) => {
                    for (int i = 0; i < acc.Meses.Length; i++)
                    {
                        acc.Meses[i] += next.Meses[i];
                    }
                    return acc;
                }).Meses;
                responseTotal.Add(targetYear, sumAnalitico);
            }
            catch(Exception err)
            {
                this.Logger.LogError(err, "Fail at Agregate: {message}", err.Message);
                var sumAnalitico = new decimal[12]{ 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m };
                responseTotal.Add(targetYear, sumAnalitico);
            }
        }

        // * update the data
        foreach(var y in responseTotal.Keys)
        {
            var item = this.analiticoResumenGlobal.Anos.FirstOrDefault(item => item.Ano == y);
            if(item != null)
            {
                item.Meses = responseTotal[y];
            }
        }

        // ****** print the data for debug ******
        foreach(var resumenAno in analiticoResumenGlobal.Anos)
        {
            Console.Write("\n");
            Console.Write(resumenAno.Ano);
            for(int i = 0; i < 12; i++)
            {
                Console.Write( $" {i+1}:{resumenAno.Meses[i].ToString("c2")}");
            }
            Console.Write("\n");
        }
        // ***************************************

    }

    private async Task RecalcularGrafica()
    {
        for(int monthN = 0; monthN < itemsGrafica.Length; monthN ++)
        {
            itemsGrafica[monthN] = new ChartItem
            {
                Descripcion = (new DateTime(2025, monthN+1 ,1)).ToString("MMM"),
                Valor1 = analiticoResumenGlobal.Anos.ElementAt(0).Meses[monthN],
                Valor2 = analiticoResumenGlobal.Anos.ElementAt(1).Meses[monthN],
                Valor3 = analiticoResumenGlobal.Anos.ElementAt(2).Meses[monthN]
            };
        }
        await chartAnalitico.RefreshAsync();
    }
}