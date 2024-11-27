using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.ControlRezago.Models{
    public class ControlRezago_AnalisisCart_Resumen {

        public string Tipo_Usuario { get; set; }
        public int Usu_Regular { get; set; }
        public decimal Imp_Regular { get; set; }
        public int Usu_Moroso { get; set; }
        public decimal Imp_Moroso { get; set; }
        public int Usu_Total { get; set; }
        public decimal Imp_Total { get; set; }
        public int Id_Tipo_Usuario{ get; set; }

}

}
