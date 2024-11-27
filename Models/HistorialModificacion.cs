using System;
using System.Collections.Generic;
using System.Linq;

namespace Sicem_Blazor.Models {
    public class HistorialModificacion {
        public int Id {get;set;} = 0;
        public int Id_Usuario {get;set;} = 0;
        public string Usuario {get;set;}  = "";
        public string Tabla {get;set;} = "";
        public string Descripcion {get;set;} = "";
        public int Id_Oficina {get;set;} = 0;
        public string Oficina {get;set;} = "";
        public DateTime? Fecha {get;set;}
        
        public List<HistorialModificacionDetalle> Detalle {get;set;}  = new List<HistorialModificacionDetalle>();

        public int CamposAfectados {
            get {
                return Detalle.Count;
            }
        }

         public static HistorialModificacion FromEntity(ModsOficina e){
            var _result = new HistorialModificacion();
            if(e == null){
                return _result;
            }
            _result.Id = e.Id;
            _result.Id_Usuario = e.IdUsuario;
            _result.Usuario = e.IdUsuarioNavigation.Nombre;
            _result.Tabla = e.Tabla;
            _result.Descripcion = e.Descripcion;
            _result.Id_Oficina = e.IdOficina;
            //_result.Oficina = e.IdOficinaNavigation.Oficina;
            //_result.Detalle = e.DetModsOficinas.Select( item => HistorialModificacionDetalle.FromEntity(item)).ToList<HistorialModificacionDetalle>();
            _result.Fecha = e.Fecha;
            return _result;
        }

    }

    public class HistorialModificacionDetalle {
        public int Id {get;set;} = 0;
        public string Descripcion {get;set;} = "";
        public string Valor_Ant {get;set;} = "";
        public string Valor_Nuevo {get;set;} = "";
        
        public static HistorialModificacionDetalle FromEntity(DetModsOficina e){
            var _result = new HistorialModificacionDetalle();
            if(e == null){
                return _result;
            }
            _result.Id = e.Id;
            _result.Descripcion = e.Descripcion;
            _result.Valor_Ant = e.ValorAnt;
            _result.Valor_Nuevo = e.ValorNuevo;
            return _result;
        }

    }

}