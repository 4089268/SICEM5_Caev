using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class OprGrupSub
    {
        public int Id { get; set; }
        public int? IdOficina { get; set; }
        public int? IdGrupSub { get; set; }
        public string Subsistema { get; set; }
    }
}
