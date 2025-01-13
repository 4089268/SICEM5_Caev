using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sicem_Blazor.Services.Whatsapp;

public static class WhatsappServiceCollectionExtension
{

    public static void AddWhatsappService(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<WhatsappSettings>(configuration.GetSection("WhatsappSettings"));
        serviceCollection.AddHttpClient("WhatsappService", client => {
            client.BaseAddress = new Uri(configuration["WhatsappSettings:Endpoint"]);
            client.DefaultRequestHeaders.Add("x-token", configuration["WhatsappSettings:Token"]);
        });
        serviceCollection.AddTransient<WHttpService>();
    }
}