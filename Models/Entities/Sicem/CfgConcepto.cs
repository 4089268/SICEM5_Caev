using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CfgConcepto
    {
        public decimal IdConcepto { get; set; }
        public string Descripcion { get; set; }
        public decimal Importe { get; set; }
        public decimal IdTipoConcepto { get; set; }
        public bool ClaveFija { get; set; }
        public bool Credito { get; set; }
        public bool Mostrar { get; set; }
        public decimal IdSystema { get; set; }
        public decimal IdEstatus { get; set; }
        public bool Inactivo { get; set; }
        public string TipoIva { get; set; }
        public bool? CostoEstatico { get; set; }
        public decimal? IdTrabajo { get; set; }
        public bool? SolicitarMetros { get; set; }
        public bool? SolicitarNombre { get; set; }
        public string ClaveProdServ { get; set; }
        public string ClaveUnidad { get; set; }
        public bool? SolicitarCantidad { get; set; }
    }
}
