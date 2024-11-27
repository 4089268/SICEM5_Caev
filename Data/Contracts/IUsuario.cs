using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Data {
    public interface IUsuario {
        public string Id {get;}
        public string Nombre { get; }
        public string Usuario { get; }
        public IEnumerable<IEnlace> Enlaces { get; }
        public bool Administrador { get; }

        public void SetEnlaces(IEnumerable<IEnlace> enlaces);
        public string GetCadEnlaces();

    }
}