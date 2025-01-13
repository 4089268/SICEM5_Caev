using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Services.Whatsapp;

public class WHttpService : HttpClient
{
    private readonly HttpClient _client;
    private readonly WhatsappSettings _settings;
    private readonly ILogger<WHttpService> _logger;
    private static readonly int _MXCODE = 521;
    
    
    public WHttpService(IHttpClientFactory httpClientFactory,  IOptions<WhatsappSettings> settings, ILogger<WHttpService> logger)
    {
        this._client = httpClientFactory.CreateClient("WhatsappService");
        this._settings = settings.Value;
        this._logger = logger;
    }

    public async Task<int> SendMessage(string phoneNumber, string message)
    {
        this._logger.LogInformation("Attempt to send the message to the phone {phone}", MaskPhoneNumber(phoneNumber) );

        // * prepara request
        var _requestContetnJson = Newtonsoft.Json.JsonConvert.SerializeObject(new {
            tipo = "texto",
            telefono = string.Format("{0}{1}{2}", _MXCODE, phoneNumber, _settings.Suffix),
            mensaje = message
        });
        var requestHttp = new HttpRequestMessage(){
            Method = HttpMethod.Post,
            RequestUri = new Uri( _client.BaseAddress, "/enviar/mensaje"),
            Content = new StringContent(_requestContetnJson, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json ),
        };
        requestHttp.Headers.Add("x-token", _settings.Token);

        try
        {
            // * send the request
            var response = await _client.SendAsync(requestHttp);
            
            // * prosesar respuesta
            response.EnsureSuccessStatusCode();

            this._logger.LogInformation("Message successfully sent to the phone {phone}", MaskPhoneNumber(phoneNumber) );
            return 1;
        }
        catch(Exception ex)
        {
            this._logger.LogError(ex, "Fail at sending the message to the phone {phone}: {message}", MaskPhoneNumber(phoneNumber), ex.Message);
            return 0;
        }

    }

    public async Task<int> SendFile(string phoneNumber, string content, string mimetype, string name)
    {
        this._logger.LogInformation("Attempt to send the message to the phone {phone}", MaskPhoneNumber(phoneNumber) );

        // * prepara request
        var _requestContetnJson = Newtonsoft.Json.JsonConvert.SerializeObject(new {
            tipo = "archivo",
            telefono = string.Format("{0}{1}{2}", _MXCODE, phoneNumber, _settings.Suffix),
            nombre = name,
            mimetype = mimetype,
            mensaje = content
        });
        var requestHttp = new HttpRequestMessage(){
            Method = HttpMethod.Post,
            RequestUri = new Uri( _client.BaseAddress, "/enviar/mensaje"),
            Content = new StringContent(_requestContetnJson, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json ),
        };
        requestHttp.Headers.Add("x-token", _settings.Token);

        try
        {
            // * send the request
            var response = await _client.SendAsync(requestHttp);
            
            // * prosesar respuesta
            response.EnsureSuccessStatusCode();

            this._logger.LogInformation("Message successfully sent to the phone {phone}", MaskPhoneNumber(phoneNumber) );
            return 1;
        }
        catch(Exception ex)
        {
            this._logger.LogError(ex, "Fail at sending the message to the phone {phone}: {message}", MaskPhoneNumber(phoneNumber), ex.Message);
            return 0;
        }

    }

    static string MaskPhoneNumber(string phoneNumber)
    {
        // Ensure the phone number is at least 4 characters long
        if (phoneNumber.Length < 4)
            return new string('*', phoneNumber.Length);

        // Mask the first part of the phone number, leaving the last 4 digits
        string masked = new string('*', phoneNumber.Length - 4) + phoneNumber.Substring(phoneNumber.Length - 4);
        return masked;
    }

}