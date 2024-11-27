using System;
using System.Collections.Generic;
using System.Linq;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Models {
    public class UsuarioExterno:IUsuario {
        public string Id {get;set;}
        public string Nombre {get;set;}
        public string Usuario { get; set; }
        public IEnlace Enlace {get;set;}
        public bool Administrador { get;}
        public IEnumerable<IEnlace> Enlaces { 
            get {
                if(Enlace != null){
                    return new IEnlace[]{Enlace};
                }else{
                    return new IEnlace[]{};
                }
            }
        }

        public UsuarioExterno(){
            Id = "";
            Nombre = "";
            Usuario = "";
            Administrador = false;
        }

        public void SetEnlaces(IEnumerable<IEnlace> enlaces){
            Enlace = enlaces.FirstOrDefault();
        }

        public string GetCadEnlaces()
        {
            return $"{Enlace.Id};";
        }
    }
}