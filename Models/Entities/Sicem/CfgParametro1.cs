using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CfgParametro1
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Parametro { get; set; }
        public string Valor { get; set; }
        public string PosiblesValores { get; set; }
        public DateTime? FechaInsert { get; set; }
        public DateTime? FechaUpdate { get; set; }
    }
}
