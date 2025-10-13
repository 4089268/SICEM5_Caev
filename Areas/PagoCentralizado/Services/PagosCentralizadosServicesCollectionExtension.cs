using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sicem_Blazor.PagoCentralizado.Services;

public static class PagosCentralizadosServicesCollectionExtension
{
    public static void AddPagosCentralizadosServices(this IServiceCollection services)
    {
        services.AddScoped<PagosCentralizadosService>();
    }
}
