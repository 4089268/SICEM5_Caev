using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sicem_Blazor.Recaudacion.Services;
using Sicem_Blazor.Recaudacion.Data;

namespace Sicem_Blazor.Services;

public static class RecaudacionServiceCollectionExtension
{

    public static void AddRecaudacionServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IRecaudacionService, RecaudacionService>();
        serviceCollection.AddScoped<AnaliticoService>();
    }
}