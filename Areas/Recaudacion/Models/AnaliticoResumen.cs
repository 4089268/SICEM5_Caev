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

        public AnaliticoResumen(IEnlace enlace)
        {
            this.Enlace = enlace;
            Anos = new AnaliticoResumenAno[3]{
                new AnaliticoResumenAno(enlace){ Ano = DateTime.Now.Year },
                new AnaliticoResumenAno(enlace){ Ano = DateTime.Now.Year - 1},
                new AnaliticoResumenAno(enlace){ Ano = DateTime.Now.Year - 2},
            };
            Estatus = ResumenOficinaEstatus.Pendiente;
        }

        public AnaliticoResumen(IEnlace enlace, int anio)
        {
            this.Enlace = enlace;
            Anos = new AnaliticoResumenAno[3]{
                new AnaliticoResumenAno(enlace){ Ano = anio},
                new AnaliticoResumenAno(enlace){ Ano = anio - 1},
                new AnaliticoResumenAno(enlace){ Ano = anio - 2},
            };
            Estatus = ResumenOficinaEstatus.Pendiente;
        }
    }
}