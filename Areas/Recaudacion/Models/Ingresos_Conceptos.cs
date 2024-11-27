using System;
using System.Data;
using System.Data.SqlClient;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Recaudacion.Models {
    public class Ingresos_Conceptos {

        public int Id_Concepto { get; set; }
        public string Concepto { get; set; }
        
        public decimal Domestico_Sub{ get; set; }
        public decimal Domestico_Iva{ get; set; }
        public decimal Domestico_Total{ get; set; }

        public decimal Comercial_Sub{ get; set; }
        public decimal Comercial_Iva{ get; set; }
        public decimal Comercial_Total{ get; set; }

        public decimal Industrial_Sub{ get; set; }
        public decimal Industrial_Iva{ get; set; }
        public decimal Industrial_Total{ get; set; }

        public decimal Publico_Sub{ get; set; }
        public decimal Publico_Iva{ get; set; }
        public decimal Publico_Total{ get; set; }

        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }

        public override string ToString()
        {
            return $"{Id_Concepto}-{Concepto} Total: {Subtotal.ToString("c2")} + {Iva.ToString("c2")} = {Total.ToString("c2")} ";
        }
        public void LoadFromSqlDataReader(SqlDataReader reader){

            this.Id_Concepto = ConvertUtils.ParseInteger(reader["Id_Concepto"].ToString());
            this.Concepto = reader["Descripcion"].ToString();

            this.Subtotal = ConvertUtils.ParseDecimal(reader["Subtotal"].ToString());
            this.Iva = ConvertUtils.ParseDecimal(reader["IVA"].ToString());
            this.Total = ConvertUtils.ParseDecimal(reader["Total"].ToString());

            this.Domestico_Sub = ConvertUtils.ParseDecimal(reader["Sbt1"].ToString());
            this.Domestico_Iva = ConvertUtils.ParseDecimal(reader["IVA1"].ToString());
            this.Domestico_Total = ConvertUtils.ParseDecimal(reader["Tot1"].ToString());

            this.Comercial_Sub = ConvertUtils.ParseDecimal(reader["Sbt2"].ToString());
            this.Comercial_Iva = ConvertUtils.ParseDecimal(reader["IVA2"].ToString());
            this.Comercial_Total = ConvertUtils.ParseDecimal(reader["Tot2"].ToString());

            this.Industrial_Sub = ConvertUtils.ParseDecimal(reader["Sbt3"].ToString());
            this.Industrial_Iva = ConvertUtils.ParseDecimal(reader["IVA3"].ToString());
            this.Industrial_Total = ConvertUtils.ParseDecimal(reader["Tot3"].ToString());

            this.Publico_Sub = ConvertUtils.ParseDecimal(reader["Sbt4"].ToString());
            this.Publico_Iva = ConvertUtils.ParseDecimal(reader["IVA4"].ToString());
            this.Publico_Total = ConvertUtils.ParseDecimal(reader["Tot4"].ToString());
            
        }
    }
}