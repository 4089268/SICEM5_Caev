using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models.Entities.Arquos
{
    public partial class CatTiposUsuario
    {
        public CatTiposUsuario()
        {
            CatColonia = new HashSet<CatColonia>();
            CatTarifas = new HashSet<CatTarifa>();
        }

        public decimal IdTipousuario { get; set; }
        public string Descripcion { get; set; }
        public bool Inactivo { get; set; }
        public decimal? TasaIva { get; set; }
        public decimal? FactorDrenaje { get; set; }
        public decimal? Tipo { get; set; }
        public decimal? ImpDrenajeFijo { get; set; }
        public decimal? ImpAguaFijo { get; set; }
        public decimal? ImpSaneamientoFijo { get; set; }
        public decimal? MetrosBase { get; set; }
        public decimal? VecesPromAlto { get; set; }
        public decimal? VecesPromMenor { get; set; }
        public decimal? FactorTratamto { get; set; }

        public virtual ICollection<CatColonia> CatColonia { get; set; }
        public virtual ICollection<CatTarifa> CatTarifas { get; set; }
    }
}
