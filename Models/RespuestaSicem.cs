using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Models {
    public class RespuestaSicem<T> {        
        public int Ok { get; set; }
        public string Mensaje { get; set; }
        public T Datos { get; set; }        
    }
}
