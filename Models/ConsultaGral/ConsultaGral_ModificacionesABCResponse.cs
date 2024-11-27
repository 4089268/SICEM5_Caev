using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Models{
    public class ConsultaGral_ModificacionesABCResponse {
        public int Id_abc {get;set;}
        public DateTime Fecha {get;set;}
        public int Id_padron { get; set; }
        public string Id_operador { get; set; }
        public int Id_sucursal { get; set; }
        public string Observacion { get; set; }
        public string Maquina { get; set; }
        public string Operador { get; set; }
        public string Sucursal { get; set; }

        public List<ConsultaGral_ModificacionesABCResponse_Item> Detalle { get; set; }

    }
    public class ConsultaGral_ModificacionesABCResponse_Item {
        public int? Id_abc { get; set; }
        public string Campo { get; set; }
        public string Valor_Ant { get; set; }
        public string Valor_Act { get; set; }
    }
}
