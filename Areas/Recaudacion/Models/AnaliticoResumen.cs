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
                new AnaliticoResumenAno{ Ano = DateTime.Now.Year },
                new AnaliticoResumenAno{ Ano = DateTime.Now.Year - 1},
                new AnaliticoResumenAno{ Ano = DateTime.Now.Year - 2},
            };
            Estatus = ResumenOficinaEstatus.Pendiente;
        }

        public AnaliticoResumen(IEnlace enlace, int anio)
        {
            this.Enlace = enlace;
            Anos = new AnaliticoResumenAno[3]{
                new AnaliticoResumenAno{ Ano = anio},
                new AnaliticoResumenAno{ Ano = anio - 1},
                new AnaliticoResumenAno{ Ano = anio - 2},
            };
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
        public AnaliticoResumenAno(int year)
        {
            Ano = year;
            Meses = new decimal[12];
        }
    }
}