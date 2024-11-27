using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Sicem_Blazor.Descuentos.Models{
    public class DescuentosConcepto {
        public int Id_Concepto { get; set; }
        public string Descripcion { get; set; }
        public decimal Conc_Con_Iva { get; set; }
        public decimal Iva { get; set; }
        public decimal Aplicado_Con_Iva { get; set; }
        public decimal Conc_Sin_Iva { get; set; }
        public decimal Total_Aplicado { get; set; }
        public int Usuarios { get; set; }


        public static DescuentosConcepto FromDataReader(SqlDataReader reader){
            var response = new DescuentosConcepto();
            
            return response;
        }

    }
    
}
