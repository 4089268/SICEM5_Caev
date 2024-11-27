using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Recaudacion.Models{
    public class Recaudacion_IngresosDetalleConceptos {
        public string Descripcion {get;set;}
        public decimal Concepto_Con_Iva {get;set;}
        public decimal Iva {get;set;}
        public decimal Aplicado_Con_Iva{get;set;}
        public decimal Concepto_Sin_Iva{get;set;}
        public decimal Total_Aplicado {get;set;}
        public int Usuarios {get;set;}
        public int Id_Concepto {get;set;}

        public override string ToString(){
            return $"{Id_Concepto}-{Descripcion} Total:{Total_Aplicado.ToString("c2")}  Usuarios:{Usuarios}";
        }

        public static Recaudacion_IngresosDetalleConceptos FromSqlDataReader(SqlDataReader reader){
            var r = new Recaudacion_IngresosDetalleConceptos();
            r.Id_Concepto = ConvertUtils.ParseInteger(reader["Id_Concepto"].ToString());
            r.Descripcion = reader["Descripcion"].ToString();
            r.Concepto_Con_Iva = ConvertUtils.ParseDecimal(reader["Conc_Con_Iva"].ToString());
            r.Iva = ConvertUtils.ParseDecimal(reader["Iva"].ToString());
            r.Aplicado_Con_Iva = ConvertUtils.ParseDecimal(reader["Aplicado_Con_Iva"].ToString());
            r.Concepto_Sin_Iva = ConvertUtils.ParseDecimal(reader["Conc_Sin_Iva"].ToString());
            r.Total_Aplicado = ConvertUtils.ParseDecimal(reader["Total_Aplicado"].ToString());
            r.Usuarios = ConvertUtils.ParseInteger(reader["Usuarios"].ToString());
            return r;
        }
        
    }
}
