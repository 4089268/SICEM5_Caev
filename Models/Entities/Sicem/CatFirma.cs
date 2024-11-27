using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CatFirma
    {
        public int? IdOficina { get; set; }
        public decimal? Numero { get; set; }
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public string Op { get; set; }
        public bool? Activo { get; set; }
    }
}
