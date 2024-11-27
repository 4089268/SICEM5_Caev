using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.ControlRezago.Models {
    public class ControlRezago_GestionCart_Detalle {

        public long Cuenta { get; set; }
        public string Usuario { get; set; }
        public string Fecha_Req { get; set; }
        public string Fecha_Pago { get; set; }
        public int MA { get; set; }
        public decimal Importe_Requerido { get; set; }
        public decimal Importe_Pago { get; set; }
        public decimal Saldo_Actual { get; set; }
        public string Situacion { get; set; }
        public string Tipo_Usuario { get; set; }
        public int Id_Situacion { get; set; }
        public string Tipo { get; set; }

    }

}
