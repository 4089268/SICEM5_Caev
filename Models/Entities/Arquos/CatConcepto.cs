using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models.Entities.Arquos
{
    public partial class CatConcepto
    {
        public decimal IdConcepto { get; set; }
        public string Descripcion { get; set; }
        public decimal Importe { get; set; }
        public decimal TasaIva { get; set; }
        public decimal IdTipoconcepto { get; set; }
        public bool ClaveFija { get; set; }
        public bool Credito { get; set; }
        public bool Mostrar { get; set; }
        public decimal IdSystema { get; set; }
        public decimal IdEstatus { get; set; }
        public bool Inactivo { get; set; }
        public string Clave { get; set; }
        public string ClaveRezago { get; set; }
        public decimal? Ordinal { get; set; }
        public decimal? RezOrdinal { get; set; }
        public string TipoIva { get; set; }
        public bool? CostoEstatico { get; set; }
    }
}
