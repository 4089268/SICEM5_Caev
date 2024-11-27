using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Recaudacion.Models{
    public class Recaudacion_IngresosDetalle {
        public DateTime Fecha {get;set;}
        public string Folio {get;set;}
        public string Cuenta {get;set;}
        public string Usuario{get;set;}
        public decimal Cobrado {get;set;}
        public string Caja {get;set;}
        public DateTime Fecha_aplicacion {get;set;}
        public int Hrs_dif {get;set;}

        public static Recaudacion_IngresosDetalle FromSqlDataReader(SqlDataReader reader){
            try{
                var newItem = new Recaudacion_IngresosDetalle();
                newItem.Fecha = ConvertUtils.ParseDateTime(reader["fecha"] ).Value;
                newItem.Folio = reader["folio"].ToString();
                newItem.Cuenta = reader["cuenta"].ToString();
                newItem.Usuario = reader["usuario"].ToString();
                newItem.Cobrado = ConvertUtils.ParseDecimal(reader["cobrado"] );
                newItem.Caja = reader["caja"].ToString();
                newItem.Fecha_aplicacion = ConvertUtils.ParseDateTime(reader["fecha_aplicacion"] ).Value;
                newItem.Hrs_dif = ConvertUtils.ParseInteger( reader["hrs_dif"] );
                return newItem;
            }catch(Exception err){
                Console.WriteLine("(xxxxxxxxxx) " + reader["fecha"] );
                Console.Write(err.Message);
                throw new NotImplementedException();

            }
        }
    }
}
