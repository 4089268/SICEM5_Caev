using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sicem_Blazor.Services.PagoLinea;

public static class PagoLineaServiceCollectionExtension
{

    public static void AddPagoLineaServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<PagoLineaSettings>(configuration.GetSection("PagoLineaSettings"));
        serviceCollection.AddHttpClient("PagoLineaClient", client => {
            client.BaseAddress = new Uri(configuration["PagoLineaSettings:Endpoint"]);
            client.DefaultRequestHeaders.Add("x-token", configuration["PagoLineaSettings:Token"]);
        });
        serviceCollection.AddTransient<HttpPagoLineaService>();
    }
}