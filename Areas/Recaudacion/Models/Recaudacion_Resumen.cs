using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.Recaudacion.Models {
    public class Recaudacion_Resumen : IResumOficina {
        public ResumenOficinaEstatus Estatus { get;  set; }
        public IEnlace Enlace { get; set; }
        
        public int Usuarios_Propios {get;set;}
        public decimal Ingresos_Propios {get;set;}

        public int Usuarios_Otros {get;set;}
        public decimal Ingresos_Otros {get;set;}

        public decimal Iva { get; set; }
        public decimal Importe_Total { get; set; }
        public decimal Cobrado { get; set; }
        public int Usuarios_Total { get; set; }


        public int Id {
            get {
                if(Enlace != null){
                    return Enlace.Id;
                }else{
                    return 0;
                }
            }
        }
        public string Oficina {
            get {
                if(Enlace != null){
                    return Enlace.Nombre;
                }else{
                    return "";
                }
            }
        }

        public Recaudacion_Resumen() {
            Estatus =  ResumenOficinaEstatus.Pendiente;
            Usuarios_Propios = 0;
            Ingresos_Propios = 0m;
            Usuarios_Otros = 0;
            Ingresos_Otros = 0m;
            Iva = 0m;
            Importe_Total = 0m;
            Cobrado = 0m;
            Usuarios_Total = 0;
        }
        public Recaudacion_Resumen(IEnlace enlace) {
            Enlace = enlace;
            Estatus = 0;
            Usuarios_Propios = 0;
            Ingresos_Propios = 0m;
            Usuarios_Otros = 0;
            Ingresos_Otros = 0m;
            Iva = 0m;
            Importe_Total = 0m;
            Cobrado = 0m;
            Usuarios_Total = 0;
        }


        // i1	u1	i2	u2	importe	cobrado	usuarios	abc	iva
        // 210824.10	395	54782.72	49	265606.82	265606.82	444	0.00	0

        public void LoadFromDataReader(SqlDataReader reader){
            var tmpInt = 0;
            var tmpDec = 0m;

            this.Estatus = ResumenOficinaEstatus.Completado;
            this.Usuarios_Propios = int.TryParse(reader["u1"].ToString(), out tmpInt)?tmpInt:0;
            this.Usuarios_Otros = int.TryParse(reader["u2"].ToString(), out tmpInt)?tmpInt:0;
            this.Usuarios_Total = int.TryParse(reader["usuarios"].ToString(), out tmpInt)?tmpInt:0;
            this.Ingresos_Propios = decimal.TryParse(reader["i1"].ToString(), out tmpDec)?tmpDec:0m;
            this.Ingresos_Otros = decimal.TryParse(reader["i2"].ToString(), out tmpDec)?tmpDec:0m;
            this.Iva = decimal.TryParse(reader["iva"].ToString(), out tmpDec)?tmpDec:0m;
            this.Importe_Total = decimal.TryParse(reader["importe"].ToString(), out tmpDec)?tmpDec:0m;
            this.Cobrado = decimal.TryParse(reader["cobrado"].ToString(), out tmpDec)?tmpDec:0m;
        }

    }
}
