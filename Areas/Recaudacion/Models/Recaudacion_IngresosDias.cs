using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Sicem_Blazor.Recaudacion.Models {
    public class Recaudacion_IngresosDias{
        public string Fecha112 { get; set; }
        public string Fecha_Letras { get; set; }

        public int Usuarios_Propios {get;set;}
        public decimal Ingresos_Propios {get;set;}

        public int Usuarios_Otros {get;set;}
        public decimal Ingresos_Otros {get;set;}

        public decimal Importe_Total { get; set; }
        public decimal Cobrado { get; set; }
        public int Usuarios_Total { get; set; }


        public DateTime? Fecha { 
            get {
                try {
                    var dia = int.Parse(Fecha112.Substring(6,2));
                    var mes = int.Parse(Fecha112.Substring(4,2));
                    var anio = int.Parse(Fecha112.Substring(0,4));

                    return new DateTime(anio, mes, dia);
                }
                catch(Exception) {
                    return null;
                }
            }
        }

        public Recaudacion_IngresosDias(){
            this.Fecha112 = "";
            this.Fecha_Letras = "";
            this.Usuarios_Propios = 0;
            this.Usuarios_Otros = 0;
            this.Usuarios_Total = 0;
            this.Ingresos_Propios = 0m;
            this.Ingresos_Otros = 0m;
            this.Importe_Total = 0m;
            this.Cobrado = 0m;
        }

        
        public void LoadFromDataReader(SqlDataReader reader){
            var tmpInt = 0;
            var tmpDec = 0m;
            this.Fecha112 = reader["dias"].ToString();
            this.Fecha_Letras = reader["dia"].ToString();
            this.Usuarios_Propios = int.TryParse(reader["u1"].ToString(), out tmpInt)?tmpInt:0;
            this.Usuarios_Otros = int.TryParse(reader["u2"].ToString(), out tmpInt)?tmpInt:0;
            this.Usuarios_Total = int.TryParse(reader["usuarios"].ToString(), out tmpInt)?tmpInt:0;
            this.Ingresos_Propios = decimal.TryParse(reader["i1"].ToString(), out tmpDec)?tmpDec:0m;
            this.Ingresos_Otros = decimal.TryParse(reader["i2"].ToString(), out tmpDec)?tmpDec:0m;
            this.Importe_Total = decimal.TryParse(reader["importe"].ToString(), out tmpDec)?tmpDec:0m;
            this.Cobrado = decimal.TryParse(reader["cobrado"].ToString(), out tmpDec)?tmpDec:0m;
        }

    }
}
