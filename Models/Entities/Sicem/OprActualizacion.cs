using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class OprActualizacion
    {
        public int Id { get; set; }
        public int IdOficina { get; set; }
        public int IdPadron { get; set; }
        public int IdCuenta { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Observaciones { get; set; }
        public int? IdEstatus { get; set; }
        public string Estatus { get; set; }
        public int? IdSituacion { get; set; }
        public string Situacion { get; set; }
        public int? IdTarifa { get; set; }
        public string Tarifa { get; set; }
        public int? IdGiro { get; set; }
        public string Giro { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdAnomaliaPredio { get; set; }
        public string AnomaliaPredio { get; set; }
        public int? IdActualizacionAnt { get; set; }
    }
}
