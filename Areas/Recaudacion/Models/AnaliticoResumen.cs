using System;
using System.Collections.Generic;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.Recaudacion.Models
{

    public class AnaliticoResumen
    {
        public IEnlace Enlace {get;set;}
        public int Id { get => Enlace?.Id ?? 0; }
        public string Oficina {get => Enlace?.Nombre ?? String.Empty;}
        public IEnumerable<AnaliticoResumenAno> Anos {get;set;}
        public ResumenOficinaEstatus Estatus {get;set;}

        public AnaliticoResumen()
        {
            Anos = Array.Empty<AnaliticoResumenAno>();
            Estatus = ResumenOficinaEstatus.Pendiente;
        }
    }

    public class AnaliticoResumenAno
    {
        public int Ano {get;set;}
        public decimal[] Meses {get;set;}

        public AnaliticoResumenAno()
        {
            Meses = new decimal[12];
        }
    }
}