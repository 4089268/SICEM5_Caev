using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Recaudacion.Models {
    public class IngresosTipoUsuario{
		public int Id_TipoCalculo {get;set;}
		public int Id_Concepto {get;set;}
		public string Descripcion {get;set;}

		public decimal Dom_Sbt {get;set;}
		public decimal Dom_IVA {get;set;}
		public decimal Dom_Tot {get;set;}

        public decimal Hot_Sbt { get; set; }
        public decimal Hot_IVA { get; set; }
        public decimal Hot_Tot { get; set; }

        public decimal Com_Sbt {get;set;}
		public decimal Com_IVA {get;set;}
		public decimal Com_Tot {get;set;}

		public decimal Ind_Sbt {get;set;}
		public decimal Ind_IVA {get;set;}
		public decimal Ind_Tot {get;set;}

		public decimal Pub_Sbt {get;set;}
		public decimal Pub_IVA {get;set;}
		public decimal Pub_Tot {get;set;}

		public decimal Subtotal {get;set;}
		public decimal IVA {get;set;}
		public decimal Total {get;set;}	
    }
	
}
