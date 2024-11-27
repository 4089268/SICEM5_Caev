using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CatAuxiliare
    {
        public decimal Id { get; set; }
        public string Auxiliar { get; set; }
        public string Descripcion { get; set; }
        public bool? EsOrganismo { get; set; }
    }
}
