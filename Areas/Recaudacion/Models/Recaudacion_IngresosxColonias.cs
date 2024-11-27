using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Recaudacion.Models {
    public class RecaudacionIngresosxColonias {
        public int Id_Colonia { get; set; }
        public string Descripcion { get; set; }
        public string Colonia { get; set; }
        public decimal Facturado { get; set; }
        public decimal Cobrado { get; set; }
        public int Recibos { get; set; }

        public static RecaudacionIngresosxColonias FromDataReader(SqlDataReader reader){
            var newItem = new RecaudacionIngresosxColonias();
            newItem.Id_Colonia = ConvertUtils.ParseInteger(reader["id_colonia"].ToString());
            newItem.Descripcion = reader["descripcion"].ToString();
            newItem.Colonia = reader["colonia"].ToString();
            newItem.Facturado = ConvertUtils.ParseDecimal(reader["facturado"].ToString());
            newItem.Cobrado = ConvertUtils.ParseDecimal(reader["Cobrado"].ToString());
            newItem.Recibos = ConvertUtils.ParseInteger(reader["Recibos"].ToString());
            return newItem;
        }
        
    }
}
