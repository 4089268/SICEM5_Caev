using System;

namespace Sicem_Blazor.Models{
    public class Atencion_Convenios{        

        public string Folio {get;set;}
        public DateTime Fecha {get;set;}
        public string Concepto {get;set;}
        public long Contrato {get;set;}
        public string Nombre {get;set;}
        public decimal Adeudo {get;set;}
        public int MesesAdeudo {get;set;}
        public decimal Anticipo {get;set;}
        public decimal Convenio {get;set;}
        public decimal Saldo {get;set;}
        public int Parcialidades {get;set;}
        public int Id_TipoUsuario {get;set;}
        public string TipoUsuario {get;set;}

    }
}



