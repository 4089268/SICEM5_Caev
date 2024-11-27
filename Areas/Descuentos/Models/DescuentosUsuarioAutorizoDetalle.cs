using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Descuentos.Models{
    public class DescuentosUsuarioAutorizoDetalle {
        public string Id_Abono { get; set; }
        public long Cuenta { get; set; }
        public string Usuarios { get; set; }
        public string Colonia { get; set; }
        public string Tipo_Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Cve { get; set; }
        public string Autorizo { get; set; }
        public string Justifica { get; set; }
        public decimal Agua { get; set; }
        public decimal Drenaje { get; set; }
        public decimal Saneamiento { get; set; }
        public decimal Rez_Agua { get; set; }
        public decimal Rez_Drenaje { get; set; }
        public decimal Rez_Saneamiento { get; set; }
        public decimal Otros { get; set; }
        public decimal Recargos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }

    }
}
