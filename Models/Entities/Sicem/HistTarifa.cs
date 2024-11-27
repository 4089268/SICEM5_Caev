using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class HistTarifa
    {
        public int Id { get; set; }
        public int? IdOficina { get; set; }
        public int? Af { get; set; }
        public int? Mf { get; set; }
        public int? IdTarifa { get; set; }
        public int? IdTipoUsuario { get; set; }
        public int? Dese { get; set; }
        public int? Hasta { get; set; }
        public decimal? Costo { get; set; }
        public decimal? CostoBase { get; set; }
        public string IdEdito { get; set; }
        public decimal? CuotaBase { get; set; }
        public decimal? AdicionalM3 { get; set; }
        public DateTime? UltimAct { get; set; }

        public virtual Ruta IdOficinaNavigation { get; set; }
        public virtual CatTipoUsuario IdTipoUsuarioNavigation { get; set; }
    }
}
