using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.ControlRezago.Models {
    public class ControlRezago_GestionCart_Resumen {

        public string Descripcion { get; set; }
        public int Usu_inv { get; set; }
        public decimal Imp_inv { get; set; }
        public decimal Imp_re_inv { get; set; }
        public int Usu_req { get; set; }
        public decimal Imp_req { get; set; }
        public decimal Imp_re_req { get; set; }
        public int Usu_val { get; set; }
        public decimal Imp_val { get; set; }
        public decimal Imp_re_val { get; set; }
        public int Usu_ban { get; set; }
        public decimal Imp_ban { get; set; }
        public decimal Imp_re_ban { get; set; }
        public int Usu_tot { get; set; }
        public decimal Imp_tot { get; set; }
        public decimal Imp_re_total { get; set; }

    }

}
