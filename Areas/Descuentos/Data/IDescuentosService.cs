using System;
using System.Collections.Generic;
using Sicem_Blazor.Data;
using Sicem_Blazor.Descuentos.Models;

namespace Sicem_Blazor.Descuentos.Data {
    public interface IDescuentosService {

        public DescuentosResumen ObtenerDescuentosResumen(IEnlace enlace, DateRange dateRange);

        public DescuentosTotal ObtenerDescuentosTotal(IEnlace enlace, DateRange dateRange);

        public IEnumerable<DescuentosConcepto> ObtenerDescuentosPorConceptos(IEnlace enlace, DateRange dateRange);
        
        public IEnumerable<DescuentosUsuarioAutorizo> ObtenerDescuentosPorUsuarioAutorizo(IEnlace enlace, DateRange dateRange);
        
        public IEnumerable<DescuentosUsuarioAutorizoDetalle> ObtenerDetalleDescuentosPorUsuarioAutorizo(IEnlace enlace, DateRange dateRange, string usuarioId);

        public IEnumerable<DescuentosConcepto> ObtenerDescuentosConceptosDetalle(IEnlace enlace, string cveAbono);
        public IEnumerable<DescuentosClave> ObtenerDescuentosClavesDetalle(IEnlace enlace, string cveAbono);
        
    }
}