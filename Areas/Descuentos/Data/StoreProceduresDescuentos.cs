using System;

namespace Sicem_Blazor.Descuentos.Data {
    public class StoreProceduresDescuentos {

        public static readonly string DESCUENTOS_RESUMEN = "EXEC [SICEM].[Descuentos] @cAlias = 'RESUMEN', @cFecha1 = '{0}', @cFecha2 = '{1}' ";

        public static readonly string DESCUENTOS_TOTAL = "EXEC [SICEM].[Descuentos] @cAlias = 'SOLO_TOTAL', @cFecha1 = '{0}', @cFecha2 = '{1}' ";

        public static readonly string DESCUENTOS_POR_CONCEPTOS = "EXEC [SICEM].[Descuentos] @cAlias = 'POR_CONCEPTOS', @cFecha1 = '{0}', @cFecha2 = '{1}'";

        public static readonly string DESCUENTOS_POR_USUARIO_AUTORIZO = "EXEC [SICEM].[Descuentos] @cAlias = 'POR_AUTORIZO_GLOBAL', @cFecha1 = '{0}', @cFecha2 = '{1}'";
        
        public static readonly string DESCUENTOS_POR_USUARIO_AUTORIZO_DETALLE = "EXEC [SICEM].[Descuentos] @cAlias = 'POR_AUTORIZO_DETALLE', @cFecha1 = '{0}', @cFecha2 = '{1}', @cAutorizo = '{2}'";

        public static readonly string DESCUENTOS_DETALLE_CONCEPTOS = "EXEC [SICEM].[Descuentos] @cAlias = 'POR_CONCEPTOS_DETALLE',  @cFecha1 = '', @cFecha2 = '', @cAbono = '{0}'";

        public static readonly string DESCUENTOS_DETALLE_CLAVES = "EXEC [SICEM].[Descuentos] @cAlias = 'POR_CLAVE_DETALLE',  @cFecha1 = '', @cFecha2 = '', @cAbono = '{0}'";

    }
}