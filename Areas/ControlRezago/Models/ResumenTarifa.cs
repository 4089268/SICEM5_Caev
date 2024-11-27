using System;
using System.Collections.Generic;

namespace Sicem_Blazor.ControlRezago.Models {
    public class ResumenTarifa {
        public string Tarifa {get;set;}
        public int Usuarios {get;set;}
        public decimal Subtotal {get;set;}
        public decimal IVA {get;set;}
        public decimal Total {get;set;}

    }

}