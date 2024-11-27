using System;
using System.Collections.Generic;
using System.Linq;

namespace Sicem_Blazor.Models {
    public class SesionInfo {
        public String IdSesion { get; set; } = "";
        public int IdUsuario { get; set; } = 0;
        public String Usuario { get; set; } = "";
        public DateTime? Fecha_Inicio { get; set; } 
        public DateTime? Fecha_Fin { get; set; }
        public string IpAddress { get; set; } = "";
        public string MacAddress { get; set; } = "";
        
        public static SesionInfo FromEntity(OprSesione sesion){
            if(sesion == null){
                return null;
            }
            var info = new SesionInfo();
            info.IdSesion = sesion.Id.ToString();
            info.IdUsuario = sesion.IdUsuario;
            info.Fecha_Inicio = sesion.Inicio;
            info.Fecha_Fin = sesion.Caducidad;
            info.IpAddress = sesion.IpAddress;
            info.MacAddress = sesion.MacAddress;
            return info;
        }

        public TimeSpan TotalTiempo {
            get {
                if(Fecha_Inicio == null){
                    return new TimeSpan(0,0,0);
                }else{
                    if(Fecha_Fin == null){
                        return DateTime.Now - (DateTime) Fecha_Inicio;
                    }else{
                        return ( Fecha_Fin - Fecha_Inicio)?? new TimeSpan(0,0,0);
                    }
                }
            }
        }
        
    }

}
