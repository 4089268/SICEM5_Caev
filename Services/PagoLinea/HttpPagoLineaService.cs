using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sicem_Blazor.Data;
using Sicem_Blazor.Models.PagoLinea;

namespace Sicem_Blazor.Services.PagoLinea;

public class HttpPagoLineaService : HttpClient
{
    private readonly HttpClient _client;
    private readonly PagoLineaSettings _settings;
    private readonly ILogger<HttpPagoLineaService> _logger;
    
    public HttpPagoLineaService(IHttpClientFactory httpClientFactory, IOptions<PagoLineaSettings> settings, ILogger<HttpPagoLineaService> logger)
    {
        this._client = httpClientFactory.CreateClient("PagoLineaClient");
        this._settings = settings.Value;
        this._logger = logger;
    }

    public async Task<ResumeMonth> GetResumeMonth(int year, int month)
    {
        _logger.LogInformation("Attempt to get the resumen of the month {month}-{year}", year, month);

        // * prepara request
        var requestHttp = new HttpRequestMessage(){
            Method = HttpMethod.Get,
            RequestUri = new Uri( _client.BaseAddress, $"/api/invoice/resume?year={year}&month={month}")
        };
        requestHttp.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.Token);

        try
        {
            // * send the request
            var response = await _client.SendAsync(requestHttp);
            
            // * prosesar respuesta
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<ResumeMonth>();
            return data;
        }
        catch(Exception ex)
        {
            this._logger.LogError(ex, "Fail at get the resumen of the month: {message}", ex.Message);
            return null;
        }
    }

    public async Task<ICollection<PaymentDetails>> GetMovements(DateTime from, DateTime to, int officeId = 0)
    {
        _logger.LogInformation("Attempt to get the movements of the date range {d1}-{d2}", from, to);

        // * prepara request
        var requestHttp = new HttpRequestMessage(){
            Method = HttpMethod.Get,
            RequestUri = new Uri( _client.BaseAddress, $"/api/invoice/movement?from={from.ToString("yyyy-MM-dd")}&to={to.ToString("yyyy-MM-dd")}&officeId={officeId}")
        };
        requestHttp.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.Token);

        try
        {
            // * send the request
            var response = await _client.SendAsync(requestHttp);
            
            // * prosesar respuesta
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<ICollection<PaymentDetails>>();
            return data;
        }
        catch(Exception ex)
        {
            this._logger.LogError(ex, "Fail at get the movements: {message}", ex.Message);
            return null;
        }
    }

    public async Task<ICollection<ResumeDay>> GetMovementsByDays(int year, int month, int officeId = 0)
    {
        _logger.LogInformation("Attempt to get the movements by days of the month {m}-{y}", month, year);

        // * prepara request
        var requestHttp = new HttpRequestMessage(){
            Method = HttpMethod.Get,
            RequestUri = new Uri( _client.BaseAddress, $"/api/invoice/movement/days?year={year}&month={month}&officeId={officeId}")
        };
        requestHttp.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.Token);

        try
        {
            // * send the request
            var response = await _client.SendAsync(requestHttp);
            
            // * prosesar respuesta
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<ICollection<ResumeDay>>();
            return data;
        }
        catch(Exception ex)
        {
            this._logger.LogError(ex, "Fail at get the movements by days of the month: {message}", ex.Message);
            return null;
        }
    }

}