using System;
using System.Collections.Generic;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.Data {
    public interface IAtencionService {
        public Atencion_Resumen ObtenerResumenOficina(IEnlace enlace, DateTime fecha1, DateTime fecha2, int sb, int sect);
        public IEnumerable<Atencion_Detalle> ObtenerAtencionDetalle(IEnlace enlace, DateTime fecha1, DateTime fecha2, int sb, int sect, string filtro);
        public IEnumerable<Atencion_Descuento> ObtenerDescuentos(IEnlace enlace, DateTime fecha1, DateTime fecha2, int sb, int sect);
        public IEnumerable<Atencion_Convenios> ObtenerConvenios(IEnlace enlace, DateTime fecha1, DateTime fecha2, int sb, int sect);
        public IEnumerable<Atencion_Group> ObtenerAtencionPorColonias(IEnlace enalce, DateTime fecha1, DateTime fecha2, int sb, int sect);
        public IEnumerable<Atencion_Group> ObtenerAtencionPorAsuntos(IEnlace enalce, DateTime fecha1, DateTime fecha2, int sb, int sect);
        public IEnumerable<Atencion_Group> ObtenerAtencionPorOrigen(IEnlace enalce, DateTime fecha1, DateTime fecha2, int sb, int sect);


    }
}