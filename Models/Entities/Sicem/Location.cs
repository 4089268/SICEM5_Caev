using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class Location
    {
        public int Id { get; set; }
        public decimal? IdMovil { get; set; }
        public string Imei { get; set; }
        public decimal? Accuracy { get; set; }
        public decimal? Altitude { get; set; }
        public decimal? Bearing { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Provider { get; set; }
        public decimal? Speed { get; set; }
        public DateTime? FechaInsert { get; set; }
        public DateTime? FechaUpdate { get; set; }
        public DateTime? Dateadded { get; set; }
        public bool? Aplicado { get; set; }
        public bool? Inactivo { get; set; }
    }
}
