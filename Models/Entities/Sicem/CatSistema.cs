using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CatSistema
    {
        public decimal Id { get; set; }
        public int? Oficina { get; set; }
        public string Auxiliar { get; set; }
        public string Descripcion { get; set; }
        public decimal Sb { get; set; }
        public decimal Sector { get; set; }
        public decimal CvePoliza { get; set; }
        public int? IdTipoOficina { get; set; }
    }
}
