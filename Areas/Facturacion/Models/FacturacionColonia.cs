using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Facturacion.Models {
    
    public class FacturacionColonia {
        public int IdLocalidad {get; set;}
        public int IdColonia {get; set;}
        public string Colonia {get; set;}
        public decimal Agua {get; set;}
        public decimal Drenaje {get; set;}
        public decimal Saneamiento {get; set;}
        public decimal Actu {get; set;}
        public decimal Otros {get; set;}
        public decimal Subtotal {get; set;}
        public decimal Iva {get; set;}
        public decimal Total {get;set;}
        public int Usuarios {get;set;}
        public int M3Consumidos {get;set;}
        public int M3Facturados {get;set;}

        public static FacturacionColonia FromDataReader(SqlDataReader reader){
            var result = new FacturacionColonia();
            result.IdLocalidad = ConvertUtils.ParseInteger(reader["id_localidad"].ToString());
            result.IdColonia = ConvertUtils.ParseInteger(reader["id_colonia"].ToString());
            result.Colonia = reader["localidad_colonia"].ToString();
            result.Agua = ConvertUtils.ParseDecimal(reader["agua"].ToString());
            result.Drenaje = ConvertUtils.ParseDecimal(reader["dren"].ToString());
            result.Saneamiento = ConvertUtils.ParseDecimal(reader["sane"].ToString());
            result.Actu = ConvertUtils.ParseDecimal(reader["actu"].ToString());
            result.Otros = ConvertUtils.ParseDecimal(reader["otros"].ToString());
            result.Subtotal = ConvertUtils.ParseDecimal(reader["subtotal"].ToString());
            result.Iva = ConvertUtils.ParseDecimal(reader["iva"].ToString());
            result.Total = ConvertUtils.ParseDecimal(reader["total"].ToString());
            result.Usuarios = ConvertUtils.ParseInteger(reader["usuarios"].ToString());
            result.M3Consumidos = ConvertUtils.ParseInteger(reader["m3_consumidos"].ToString());
            result.M3Facturados = ConvertUtils.ParseInteger(reader["m3_facturados"].ToString());
            return result;
        }							
    }
    
}