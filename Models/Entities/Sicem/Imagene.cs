using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class Imagene
    {
        public byte[] Imagen { get; set; }
        public int? Oficina { get; set; }
        public string Opcion { get; set; }
        public string Fecha { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaInsercion { get; set; }
        public decimal Id { get; set; }
    }
}
