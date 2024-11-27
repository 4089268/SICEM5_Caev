using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.ControlRezago.Models {
    public class ControlRezago_AnalisisCart_Detalle {

        public string Localidad { get; set; } 
        public string Colonia { get; set; }
        public string Tipo_Usuario { get; set; }
        public int Sb { get; set; }
        public int Sector { get; set; }
        public int Id_Localidad { get; set; }
        public int Id_Colonia { get; set; }
        public int Id_Tarifa { get; set; }
        public decimal Imp_3_6_MESES { get; set; }
        public int Usu_3_6_MESES { get; set; }
        public decimal Imp_7_12_MESES { get; set; }
        public int Usu_7_12_MESES { get; set; }
        public decimal Imp_1_2_ANOS { get; set; }
        public int Usu_1_2_ANOS { get; set; }
        public decimal Imp_3_5_ANOS { get; set; }
        public int Usu_3_5_ANOS { get; set; }
        public decimal Imp_5_ANOS { get; set; }
        public int Usu_5_ANOS { get; set; }
        public decimal IMP_TOTAL { get; set; }
        public int USU_TOTAL{ get; set; }

    }

}
