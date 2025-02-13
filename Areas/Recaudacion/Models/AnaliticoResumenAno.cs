using System;
using System.Collections.Generic;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.Recaudacion.Models
{

    public class AnaliticoResumenAno
    {
        public IEnlace Enlace {get;set;}
        public int Ano {get;set;}
        public decimal[] Meses {get;set;}

        public int Id { get => Enlace?.Id ?? 0; }
        public string Oficina {get => Enlace?.Nombre ?? String.Empty;}

        public decimal Ene { get => Meses?[0] ?? 0m; }
        public decimal Feb { get => Meses?[1] ?? 0m; }
        public decimal Mar { get => Meses?[2] ?? 0m; }
        public decimal Abr { get => Meses?[3] ?? 0m; }
        public decimal May { get => Meses?[4] ?? 0m; }
        public decimal Jun { get => Meses?[5] ?? 0m; }
        public decimal Jul { get => Meses?[6] ?? 0m; }
        public decimal Ago { get => Meses?[7] ?? 0m; }
        public decimal Sep { get => Meses?[8] ?? 0m; }
        public decimal Oct { get => Meses?[9] ?? 0m; }
        public decimal Nov { get => Meses?[10] ?? 0m; }
        public decimal Dic { get => Meses?[11] ?? 0m; }

        public AnaliticoResumenAno(IEnlace enlace)
        {
            this.Enlace = enlace;
            this.Meses = new decimal[12];
        }
        public AnaliticoResumenAno(IEnlace enlace, int year)
        {
            this.Enlace = enlace;
            this.Ano = year;
            this.Meses = new decimal[12];
        }
    }
}