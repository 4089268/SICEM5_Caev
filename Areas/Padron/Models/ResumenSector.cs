using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Padron.Models {
    public class ResumenSector {
        public string Sector {get;set;}
        public int Usuarios {get;set;}
        public decimal Subtotal {get;set;}
        public decimal IVA {get;set;}
        public decimal Total {get;set;}

    }

}