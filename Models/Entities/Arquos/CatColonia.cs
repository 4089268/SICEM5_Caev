using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models.Entities.Arquos
{
    public partial class CatColonia
    {
        public decimal IdColonia { get; set; }
        public string Descripcion { get; set; }
        public decimal Sb { get; set; }
        public decimal Sector { get; set; }
        public decimal ManzanaIni1 { get; set; }
        public decimal ManzanaFin1 { get; set; }
        public decimal ManzanaIni2 { get; set; }
        public decimal ManzanaFin2 { get; set; }
        public decimal ManzanaIni3 { get; set; }
        public decimal ManzanaFin3 { get; set; }
        public decimal IdTipousuario { get; set; }
        public decimal IdServicio { get; set; }
        public decimal IdMaterialcalle { get; set; }
        public decimal Plazos { get; set; }
        public decimal Anticipo { get; set; }
        public string Politicas { get; set; }
        public bool? Inactivo { get; set; }
        public decimal? IdPoblacion { get; set; }
        public decimal? IdRegion { get; set; }
        public decimal? CalidadServicio { get; set; }
        public string Horario { get; set; }
        public string Suministro { get; set; }

        public virtual CatServicio IdServicioNavigation { get; set; }
        public virtual CatTiposUsuario IdTipousuarioNavigation { get; set; }
    }
}
