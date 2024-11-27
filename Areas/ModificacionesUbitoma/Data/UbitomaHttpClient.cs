using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Sicem_Blazor.Data;
using Sicem_Blazor.ModsUbitoma.Models;
using Sicem_Blazor.ModsUbitoma.Mods;

namespace Sicem_Blazor.ModsUbitoma.Data
{
    public class UbitomaHttpClient
    {
        public HttpClient Client { get; }
        public UbitomaHttpClient(HttpClient client){
            Client = client;
        }

        public async Task<IEnumerable<ResumenOficina_Response>> ObterResumenOficinas(ConsultaRequest request){
            var result = new List<ResumenOficina_Response>();

            // Prepara request
            var _requestContetnJson = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var requestHttp = new HttpRequestMessage(){
                Method = HttpMethod.Get,
                RequestUri = new Uri(Client.BaseAddress,$"api/Consulta/Resumen?f1={request.FechaDesde.ToString("yyyyMMdd")}&f2={request.FechaHasta.ToString("yyyyMMdd")}"),
                Content = new StringContent(_requestContetnJson, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json ),
            };

            // Realizar consulta
            var response = await Client.SendAsync(requestHttp);
            
            // Prosesar respuesta
            if( response.IsSuccessStatusCode){
                var _responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(_responseContent);
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ResumenOficina_Response>>(_responseContent).ToList();
            }

            return result;

        }
        public async Task<IEnumerable<HistActualizacion>> ObtenerDetalleOficina(IEnlace enlace, ConsultaRequest request){
            var result = new List<HistActualizacion>();

            // Prepara request
            var _requestContetnJson = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var requestHttp = new HttpRequestMessage(){
                Method = HttpMethod.Get,
                RequestUri = new Uri(Client.BaseAddress, $"api/Consulta/Detalle/{enlace.Id}?f1={request.FechaDesde.ToString("yyyyMMdd")}&f2={request.FechaHasta.ToString("yyyyMMdd")}"),
                Content = new StringContent(_requestContetnJson, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json ),
            };

            // Realizar consulta
            var response = await Client.SendAsync(requestHttp);
            
            // Prosesar respuesta
            if( response.IsSuccessStatusCode){
                var _responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(_responseContent);
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<HistActualizacion>>(_responseContent).ToList();
            }

            return result;

        }
        public async Task<IEnumerable<ImagenInfoResponse>> ObtenerImagenesOficina(IEnlace enlace, DateTime fecha1, DateTime fecha2){
            var result = new List<ImagenInfoResponse>();

            // Realizar consulta
            //{{ubitoma_api}}/api/Consulta/Imagenes/1?f1=20220801&f2=20220831
            var _url = string.Format("api/Consulta/Imagenes/{0}?f1={1}&f2={2}", enlace.Id, fecha1.ToString("yyyyMMdd"), fecha2.ToString("yyyyMMdd"));
            var response = await Client.GetAsync(_url);

            // Prosesar respuesta
            if( response.IsSuccessStatusCode){
                var _responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(_responseContent);
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ImagenInfoResponse>>(_responseContent).ToList();
            }

            return result;
        }

    }
}