using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Sicem_Blazor.Data;
using Sicem_Blazor.ModsUbitoma.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using Sicem_Blazor.ModsUbitoma.Data;
using Sicem_Blazor.ModsUbitoma.Mods;

namespace Sicem_Blazor.Services {
    public class UbitomaService : IUbitomaService {

        private readonly ILogger<EficienciaService> logger;
        private readonly UbitomaHttpClient ubitomaHttpClient;
        private readonly SicemService sicemService;

        public UbitomaService(ILogger<EficienciaService> l, UbitomaHttpClient ubitomaHttpClient, SicemService s){
            this.logger = l;
            this.ubitomaHttpClient = ubitomaHttpClient;
            this.sicemService = s;
        }

        public async Task<IEnumerable<ResumenOficina>> ObtnerResumenPorOficinas(ConsultaRequest request){
            var result = new List<ResumenOficina>();

            // Realizar consulta
            var _response = (await ubitomaHttpClient.ObterResumenOficinas(request)).ToList();

            // Obtener catalogo rutas
            IEnumerable<IEnlace> _rutas = sicemService.ObtenerEnlaces();
            
            foreach( var resp in _response){
                var _enlace = _rutas.Where(item => item.Id == resp.idOficina).FirstOrDefault();
                if(_enlace != null){
                    var resumen = new ResumenOficina(_enlace);
                    resumen.SetValuesFromResponse(resp);
                    result.Add(resumen);
                }
            }

            return result;
        }
        public async Task<IEnumerable<HistActualizacion>> ObtenerHistorialModificacionesOficina(IEnlace enlace, ConsultaRequest request){
            var result = new List<HistActualizacion>();
            result = (await ubitomaHttpClient.ObtenerDetalleOficina(enlace, request)).ToList();
            return result;
        }
        public async Task<IEnumerable<ImagenInfoResponse>> ObtenerImagenesOficina(IEnlace enlace, DateTime fecha1, DateTime fecha2){
            var result = new List<ImagenInfoResponse>();
            result = (await ubitomaHttpClient.ObtenerImagenesOficina(enlace, fecha1, fecha2)).ToList();
            return result;
        }
        
    }
}