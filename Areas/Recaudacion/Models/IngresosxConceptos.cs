using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Recaudacion.Models {
    public class IngresosxConceptos {
        public int Id_Concepto { get; set; }
        public string Descripcion { get; set; }
        public decimal Conc_Con_Iva { get; set; }
        public decimal Iva { get; set; }
        public decimal Aplicado_Con_Iva { get; set; }
        public decimal Conc_Sin_Iva { get; set; }
        public decimal Total_Aplicado { get; set; }
        public int Usuarios { get; set; }
        
        public IngresosxConceptos(){
            this.Id_Concepto = 0;
            this.Descripcion = "";
            this.Conc_Con_Iva = 0m;
            this.Iva = 0m;
            this.Aplicado_Con_Iva = 0m;
            this.Conc_Sin_Iva = 0m;
            this.Total_Aplicado = 0m;
            this.Usuarios = 0;
        }

        public void LoadFromDataReader(SqlDataReader reader){
            this.Id_Concepto = ConvertUtils.ParseInteger(reader["Id_Concepto"].ToString());
            this.Descripcion = reader["Descripcion"].ToString();
            this.Conc_Con_Iva = ConvertUtils.ParseDecimal(reader["Conc_Con_Iva"].ToString());
            this.Iva = ConvertUtils.ParseDecimal(reader["Iva"].ToString());
            this.Aplicado_Con_Iva = ConvertUtils.ParseDecimal(reader["Aplicado_Con_Iva"].ToString());
            this.Conc_Sin_Iva = ConvertUtils.ParseDecimal(reader["Conc_Sin_Iva"].ToString());
            this.Total_Aplicado = ConvertUtils.ParseDecimal(reader["Total_Aplicado"].ToString());
            this.Usuarios = ConvertUtils.ParseInteger(reader["Usuarios"].ToString());

        }

    }
}