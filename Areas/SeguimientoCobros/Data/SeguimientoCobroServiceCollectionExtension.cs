using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sicem_Blazor.SeguimientoCobros.Data;

public static class SeguimientoCobroServiceCollectionExtension
{
    public static void AddSeguimientoCobroServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddTransient<SeguimientoCobroService>();
        serviceCollection.AddTransient<SeguimientoCobroMapJsInterop>();
        serviceCollection.AddHostedService<IncomeBackgroundService>();
    }
}