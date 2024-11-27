using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Sicem_Blazor.Recaudacion.Models {
    public class Recaudacion_IngresosCajas {
        public string Caja { get; set; }
        public decimal Facturado { get; set; }        
        public decimal Cobrado { get; set; } 
        public int Recibos { get; set; }
        public string CveCaja {
            get {
                try{
                    return Caja.Split(" ")[0];
                }catch(Exception){
                    return "";
                }
            }
         }

        public Recaudacion_IngresosCajas(){
            this.Caja = "";
            this.Facturado = 0m;
            this.Cobrado = 0m;
            this.Recibos = 0;
        }

        public void LoadFromDataReader(SqlDataReader reader){
            var tmpInt = 0;
            var tmpDec = 0m;
            this.Caja = reader["caja"].ToString().Trim();
            this.Facturado = decimal.TryParse(reader["facturado"].ToString(), out tmpDec)?tmpDec:0;
            this.Cobrado = decimal.TryParse(reader["cobrado"].ToString(), out tmpDec)?tmpDec:0;
            this.Recibos = int.TryParse(reader["recibos"].ToString(), out tmpInt)?tmpInt:0;
        }
        
    }
}
