using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CatTipoUsuario
    {
        public CatTipoUsuario()
        {
            HistTarifas = new HashSet<HistTarifa>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal? TasaIva { get; set; }
        public decimal? FactorDrenaje { get; set; }
        public decimal? FactorTratmto { get; set; }
        public decimal? ImpAguaFijo { get; set; }
        public decimal? ImpDrenajeFijo { get; set; }
        public decimal? ImpSaneamientoFijo { get; set; }
        public int? MetrosBase { get; set; }
        public DateTime? UltimaModif { get; set; }

        public virtual ICollection<HistTarifa> HistTarifas { get; set; }
    }
}
