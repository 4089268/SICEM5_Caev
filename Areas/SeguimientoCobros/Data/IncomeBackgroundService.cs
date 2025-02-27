using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Sicem_Blazor.Models;
using Sicem_Blazor.SeguimientoCobros.Models;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.SeguimientoCobros.Data;

public class IncomeBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<IncomeBackgroundService> _logger;
    // private IHubContext<IncomeLiveDataHub> _hubContext;

    public IncomeBackgroundService(IServiceScopeFactory scf, ILogger<IncomeBackgroundService> l)
    {
        this._serviceScopeFactory = scf;
        this._logger = l;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug($"BG Start: {DateTime.Now.ToShortTimeString()}");
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SicemContext>();

                // get the routes
                var ruta = dbContext.Rutas.Where(r => r.Inactivo != true).ToList();

                // get the income data
                var fetchDataTasks = ruta.Select(async enlace => {
                    try
                    {
                        return await FetchData(enlace);
                    }
                    catch (Exception err)
                    {
                        this._logger.LogError(err, "Fail at fetching the data: {message}", err.Message);
                        return Array.Empty<LiveIncome>();
                    }
                });
                var results = await Task.WhenAll(fetchDataTasks);

                // Flatten the results into a single list
                List<LiveIncome> dataList = results.SelectMany(r => r).ToList();

                foreach(var item in dataList.GroupBy(e => e.OficinaId))
                {
                    _logger.LogDebug(">> {oficina}: {sucursal} {recibo} {income}", item.First().Oficina, item.First().Sucursal, item.First().Recibos, item.First().Cobrado);
                }

                _logger.LogDebug($"BG Finished: {DateTime.Now.ToShortTimeString()}");
            }
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }

    private async Task<IEnumerable<LiveIncome>> FetchData(IEnlace enlace)
    {
        try
        {
            var responseList = new List<LiveIncome>();
            using(var sqlConnection = new SqlConnection(enlace.GetConnectionString()))
            {
                await sqlConnection.OpenAsync();
                using var command = new SqlCommand("[Sicem].[usp_Cobranza_en_vivo]", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                using(var reader = await command.ExecuteReaderAsync())
                {
                    while(reader.Read())
                    {
                        responseList.Add(LiveIncome.FromDataReader(enlace, reader));
                    }
                }
                sqlConnection.Close();
            }
            return responseList;

        }catch(Exception err)
        {
            this._logger.LogError(err, "Fail to fetch the data from {oficina}: {message}", enlace.Nombre, err.Message);
            return Array.Empty<LiveIncome>();
        }
    }
}
