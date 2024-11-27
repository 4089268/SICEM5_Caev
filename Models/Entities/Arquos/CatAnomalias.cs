using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models.Entities.Arquos
{
    public partial class CatAnomalias
    {
        public decimal IdAnomalia { get; set; }
        public string Descripcion { get; set; }
        public string Observaciones { get; set; }
        public decimal IdGenera1 { get; set; }
        public decimal IdGenera2 { get; set; }
        public decimal IdTipocalculo { get; set; }
        public decimal IdSystema { get; set; }
        public string CveWang { get; set; }
        public bool Inactivo { get; set; }
        public bool? EnCampo { get; set; }
        public decimal? IdTipoanomalia { get; set; }
        public bool? FuncionaMedidor { get; set; }

        public virtual CatTiposCalculo IdTipocalculoNavigation { get; set; }
    }
}
