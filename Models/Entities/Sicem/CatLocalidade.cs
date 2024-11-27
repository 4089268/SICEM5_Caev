using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CatLocalidade
    {
        public int? IdOficina { get; set; }
        public int? No { get; set; }
        public string Oficina { get; set; }
        public string CveOrg { get; set; }
        public string CveLocAnt { get; set; }
        public decimal? NoRural { get; set; }
        public string CveLoc { get; set; }
        public string Programa { get; set; }
        public string Zona { get; set; }
        public string Enlace { get; set; }
        public decimal? Sb { get; set; }
        public decimal? Sector { get; set; }
        public decimal? Manzana { get; set; }
    }
}
