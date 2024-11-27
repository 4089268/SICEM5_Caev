using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Sicem_Blazor.Recaudacion.Models {
    public class Recaudacion_Rezago {
        public string Mes { get; set; }
        public int Usuarios { get; set; }
        public decimal Rez_agua { get; set; }
        public decimal Rez_dren { get; set; }
        public decimal Rez_sane { get; set; }
        public decimal Rez_otros { get; set; }
        public decimal Rez_recargos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public Recaudacion_Rezago(){
            this.Mes = "";
            this.Usuarios = 0;
            this.Rez_agua = 0m;
            this.Rez_dren = 0m;
            this.Rez_sane = 0m;
            this.Rez_otros = 0m;
            this.Rez_recargos = 0m;
            this.Subtotal = 0m;
            this.Iva = 0m;
            this.Total = 0m;
        }

        public void LoadFromDataReader(SqlDataReader reader){
            var tmpDec = 0m;
            this.Mes = reader["mes"].ToString();
            this.Usuarios = int.TryParse(reader["usuarios"].ToString(), out int tmpInt)? tmpInt:0;
            this.Rez_agua = decimal.TryParse(reader["rez_agua"].ToString(), out tmpDec)?tmpDec:0m;
            this.Rez_dren = decimal.TryParse(reader["rez_dren"].ToString(), out tmpDec)?tmpDec:0m;
            this.Rez_sane = decimal.TryParse(reader["rez_sane"].ToString(), out tmpDec)?tmpDec:0m;
            this.Rez_otros = decimal.TryParse(reader["rez_otros"].ToString(), out tmpDec)?tmpDec:0m;
            this.Rez_recargos = decimal.TryParse(reader["rez_recargos"].ToString(), out tmpDec)?tmpDec:0m;
            this.Subtotal = decimal.TryParse(reader["subtotal"].ToString(), out tmpDec)?tmpDec:0m;
            this.Iva = decimal.TryParse(reader["iva"].ToString(), out tmpDec)?tmpDec:0m;
            this.Total = decimal.TryParse(reader["total"].ToString(), out tmpDec)?tmpDec:0m;
        }


    }
}
