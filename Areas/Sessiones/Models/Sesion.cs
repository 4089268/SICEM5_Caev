using System;
using Sicem_Blazor.Data;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.Sessiones.Models {
    
    public class Sesion : IDisposable{
        public string IdSesion {get;set;}
        public IUsuario Usuario {get;set;}
        public DateTime FechaInicio {get;set;}
        public DateTime? FechaFin {get;set;}
        public string IpAddress {get;set;}

        public string NombreUsuario {
            get {
                if(Usuario == null){
                    return "";
                }else{
                    return Usuario.Nombre;
                }
            }
        }

        public Sesion(IUsuario u){
            IdSesion = Guid.NewGuid().ToString();
            FechaInicio = DateTime.Now;
            Usuario = u;
            IpAddress = "";
        }
        public Sesion(){
            IdSesion = Guid.NewGuid().ToString();
            FechaInicio = DateTime.Now;
            IpAddress = "";
        }

        public void Dispose(){
            FechaFin = DateTime.Now;
        }

        public override string ToString()
        {
            return $"IdSession:{IdSesion} NombreUsuario:{NombreUsuario}";
        }
    }
}