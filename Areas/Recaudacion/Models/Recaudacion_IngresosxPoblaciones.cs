using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Recaudacion.Models {
    public class RecaudacionIngresosxPoblaciones {
        public string Localidad { get; set; }
        public decimal Facturado { get; set; }
        public decimal Cobrado { get; set; }
        public int Recibos { get; set; }
        public int Id_localidad
        {
            get {
                if(Localidad.ToLower().Contains("total")){
                    return 9999;
                }
                try{
                    return int.Parse(Localidad.Trim().Split(" ").First());
                }catch(Exception){
                    return 0;
                }
            }
        }

        public RecaudacionIngresosxPoblaciones(){
            this.Localidad = "";
            this.Facturado = 0m;
            this.Cobrado = 0m;
            this.Recibos = 0;
        }
        
        public void LoadFromDataReader(SqlDataReader reader){
            var tmpDec = 0m;
            this.Localidad = reader["localidad"].ToString();
            this.Facturado = decimal.TryParse(reader["facturado"].ToString(), out tmpDec)?tmpDec:0m;
            this.Cobrado = decimal.TryParse(reader["cobrado"].ToString(), out tmpDec)?tmpDec:0m;
            this.Recibos = int.TryParse(reader["recibos"].ToString(), out int tmpInt)?tmpInt:0;
        }
        
    }
}
