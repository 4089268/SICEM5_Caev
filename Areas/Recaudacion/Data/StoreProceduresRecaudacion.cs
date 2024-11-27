namespace Sicem_Blazor.Recaudacion.Data {

    public class StoreProceduresRecaudacion {
        public static readonly string RECAUDACION_RESUMEN = "EXEC [Sicem].[Recaudacion] @cAlias = 'TOTALES', @cFecha1 = '{0}', @cFecha2 = '{1}', @xSb = {2}, @xSec = {3}";     
        public static readonly string RECAUDACION_ANALITICO = "Exec [Sicem].[Recaudacion] @cAlias ='ANALITICO', @cFecha1 ='{0}', @cFecha2 ='{1}', @xSb={2}, @xSec={3}";
        public static readonly string RECAUDACION_ANALITICO_REZAGO = "Exec [Sicem].[Recaudacion] @cAlias ='ANALITICO_REZAGO', @cFecha1 ='{0}', @cFecha2 ='{1}', @xSb={2}, @xSec={3}";
        public static readonly string RECAUDACION_DIAS = "Exec [Sicem].[Recaudacion] @cAlias ='TOTALES_X_DIA', @cFecha1 ='{0}', @cFecha2 ='{1}', @xSb={2}, @xSec={3}";
        public static readonly string RECAUDACION_CAJAS = "Exec [Sicem].[Recaudacion] @cAlias ='POR_CAJA', @cFecha1 ='{0}', @cFecha2 ='{1}', @xSb={2}, @xSec={3}";
        public static readonly string RECAUDACION_LOCALIDADES = "Exec [Sicem].[Recaudacion] @cAlias ='POR_POBLACION', @cFecha1 ='{0}', @cFecha2 ='{1}', @xSb={2}, @xSec={3}";
        public static readonly string RECAUDACION_REZAGO = "Exec [Sicem].[Recaudacion] @cAlias ='INGRESOS_REZAGO', @cFecha1 ='{0}', @cFecha2 ='{1}', @xSb={2}, @xSec={3}";
        public static readonly string RECAUDACION_CONCEPTOS = "Exec [Sicem].[Recaudacion] @cAlias ='POR_CONCEPTOS', @cFecha1 ='{0}', @cFecha2 ='{1}', @xSb={2}, @xSec={3}";
        public static readonly string RECAUDACION_CONCEPTOS_GRAV = "Exec [Sicem].[Recaudacion] @cAlias ='POR_CONCEPTOPS_GRAVAMEN', @cFecha1 ='{0}', @cFecha2 ='{1}', @xSb={2}, @xSec={3}";
        public static readonly string RECAUDACION_CONCEPTOS_TARIFAS = "Exec [Sicem].[Recaudacion] @cAlias ='POR_CONCEPTOS_TARIFAS', @cFecha1 ='{0}', @cFecha2 ='{1}', @xSb={2}, @xSec={3}";
        public static readonly string RECAUDACION_TOP_VENTAS = "Exec [Sicem].[Recaudacion] @cAlias = 'TOP_VENTAS', @cFecha1 = '{0}', @cFecha2 ='{1}', @xParam = {2}";

        public static readonly string RECAUDACION_DETALLE = "Exec [Sicem].[Recaudacion] @cAlias = 'DETALLE', @cFecha1 = '{0}', @cFecha2 ='{1}', @xSb={2}, @xSec={3}";
        public static readonly string RECAUDACION_DETALLE_CONCEPTOS = "Exec [Sicem].[Recaudacion] @cAlias = 'POR_CONCEPTOS', @cFecha1 = '{0}', @cFecha2 ='{1}', @xSb={2}, @xSec={3}";


    }
}