using System;

namespace Sicem_Blazor.Models{
    public class Atencion_Descuento{
        public string Folio {get;set;}
        public int Sb {get;set;}
        public int Sector {get;set;}
        public string Fecha {get;set;}
        public long Contrato {get;set;}
        public string Nombre {get;set;}
        public int Id_TipoUsuario {get;set;}
        public string Tipo_Usuario {get;set;} 
        public string Id_Ajuste {get;set;}
        public string Tipo_ajuste {get;set;}
        public int MesesAdeudo {get;set;}
        public decimal Adeudo_Inicial {get;set;}
        public decimal Importe_Ajustado {get;set;}
        public decimal Saldo_Cuenta {get;set;}
        public string Id_Genero {get;set;}
        public string Genero {get;set;}
    }
}