using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Padron.Models{
    public class Padron_ModifTarifa {
        public long Cuenta {get;set;}
        public string Localizacion {get;set;}
        public string Usuario {get;set;}
        public DateTime Fecha {get;set;}
        public string Valor_Ant {get;set;}
        public string Valor_Act {get;set;}
        public string Realizo {get;set;}

    }
}