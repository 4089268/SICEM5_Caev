using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sicem_Blazor.Models;
using Sicem_Blazor.Sessiones.Models;
using Sicem_Blazor.Sessiones.Data;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Services {
    public class SessionService : ISessionPublisher {
        private List<Sesion> sesionesActivas = new List<Sesion>();
        private List<ISesionSuscriber> Suscribers = new List<ISesionSuscriber>();

        public string IniciarSesion(IUsuario u, string ipAddress = ""){
            var _sesion = new Sesion(u);
            _sesion.IpAddress = ipAddress;
            this.sesionesActivas.Add(_sesion);
            NotifySuscribers();
            return _sesion.IdSesion;
        }
        public void FinalizarSesion(string id){
            var _sesion = sesionesActivas.Where( item => item.IdSesion == id).FirstOrDefault();
            if(_sesion != null){
                sesionesActivas.Remove(_sesion);
                _sesion.Dispose();
            }
            NotifySuscribers();
        }
        public IUsuario ObtenerUsuario(string id){
            Sesion _session = sesionesActivas.Where(item => item.IdSesion == id).FirstOrDefault();
            if(_session == null){
                return null;
            }else{
                return _session.Usuario;
            }
        }
        public IEnumerable<Sesion> ObtenerListaSesiones(){
            return sesionesActivas;
        }
        public bool ValidarSession(string id){
            return sesionesActivas.Select(item => item.IdSesion ).Contains(id);
        }


        // *** Implementacion de la interfas
        public void AddSuscriber(ISesionSuscriber s)
        {
            if(!Suscribers.Select(item => item.IDSesionSuscriber).Contains(s.IDSesionSuscriber)){
                Suscribers.Add(s);
            }
        }

        public void RemoveSuscriber(String IdSuscriber)
        {   
            if(Suscribers.Select(item => item.IDSesionSuscriber).Contains(IdSuscriber)){
                ISesionSuscriber _suscriber = Suscribers.Where(item => item.IDSesionSuscriber == IdSuscriber ).FirstOrDefault();
                Suscribers.Remove( _suscriber );
            }
        }

        public void NotifySuscribers()
        {
            foreach(var suscriber in Suscribers){
                suscriber.SessionAdded();
            }
        }
    }

}