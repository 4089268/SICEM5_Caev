using System;
using System.Collections.Generic;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Recaudacion.Models
{

    public class AnaliticoResumenResponse
    {
        public IEnlace Enlace {get;set;}
        public int Ano { get;set; }
        public int Mes { get;set; }
        public decimal Cobrado {get;set;}
    }
}