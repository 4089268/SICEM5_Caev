using System;
using System.Collections.Generic;
using Sicem_Blazor.Data;
using Sicem_Blazor.Recaudacion.Models;

namespace Sicem_Blazor.Recaudacion.Data {
    public interface IRecaudacionService {

        public Recaudacion_Resumen ObtenerResumen(IEnlace enlace, DateRange dateRange);
        public Recaudacion_Analitico ObtenerAnalisisIngresos(IEnlace enlace, DateRange dateRange);
        public IEnumerable<Recaudacion_Rezago> ObtenerRezago(IEnlace enlace, DateRange dateRange);
        public IEnumerable<Recaudacion_IngresosDias> ObtenerIngresosPorDias(IEnlace enlace, DateRange dateRange);
        public IEnumerable<Recaudacion_IngresosCajas> ObtenerIngresosPorCajas(IEnlace enlace, DateRange dateRange);
        public IEnumerable<IngresosxConceptos> ObtenerIngresosPorConceptos(IEnlace enlace, DateRange dateRange);
        public IEnumerable<IngresosTipoUsuario> ObtenerIngresosPorTipoUsuarios(IEnlace enlace, DateRange dateRange);
        public IEnumerable<Ingresos_FormasPago> ObtenerIngresosPorFormasPago(IEnlace enlace, DateRange dateRange);
        public Recaudacion_PagosMayores_Response ObtenerPagosMayores(IEnlace enlace, DateRange dateRange, int total);
        public IEnumerable<Recaudacion_IngresosDetalle> ObtenerDetalleIngresos(IEnlace enlace, DateRange dateRange);
        public IEnumerable<Recaudacion_IngresosDetalleConceptos> ObtenerDetalleConceptos(IEnlace enlace, DateRange dateRange, int id_poblacion, int id_colonia);
        public IEnumerable<Recaudacion_Rezago_Detalle> ObtenerDetalleRezago(IEnlace enlace, DateRange dateRange, int mes, bool acumulativo);
        public IEnumerable<ConceptoTipoUsuario> ObtenerRecaudacionPorConceptosYTipoUsuario(IEnlace enlace, DateRange dateRange );
        public IEnumerable<RecaudacionIngresosxPoblaciones> ObtenerRecaudacionLocalidades(IEnlace enlace, DateRange dateRange);
        public IEnumerable<RecaudacionIngresosxColonias> ObtenerRecaudacionColonias(IEnlace enlace, DateRange dateRange, int idLocalidad);
        public IEnumerable<Recaudacion_IngresosDetalleConceptos> ObtenerIngresosConceptosPorLocalidadColonia(IEnlace enlace, DateRange dateRange, int idLocalidad, int idColonia);
        public IEnumerable<Ingresos_Conceptos> ObtenerIngresosPorConceptosTipoUsuarios(IEnlace enlace, DateRange dateRange);
        public IEnumerable<Ingreso_Gravamen> IngresosGravamen(IEnlace enlace, DateRange dateRange);
    }

}
