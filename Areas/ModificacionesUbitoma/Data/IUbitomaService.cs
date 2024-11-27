using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.Data;
using Sicem_Blazor.ModsUbitoma.Models;
using Sicem_Blazor.ModsUbitoma.Mods;

namespace Sicem_Blazor.ModsUbitoma.Data {
    public interface IUbitomaService {
        public Task<IEnumerable<ResumenOficina>> ObtnerResumenPorOficinas(ConsultaRequest request);
        public Task<IEnumerable<HistActualizacion>> ObtenerHistorialModificacionesOficina(IEnlace enlace, ConsultaRequest request);
        public Task<IEnumerable<ImagenInfoResponse>> ObtenerImagenesOficina(IEnlace enlace, DateTime fecha1, DateTime fecha2);
        
    }
}