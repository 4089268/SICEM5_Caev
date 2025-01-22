using System;

namespace Sicem_Blazor.Models.PagoLinea
{
    public class StorePaymentRequest
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaDeDispersion { get; set; }
        public string Comercio { get; set; } = string.Empty;
        public string Unidad { get; set; } = string.Empty;
        public string Concepto { get; set; } = string.Empty;
        public string ReferenciaComercio { get; set; } = string.Empty;
        public string Orden { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string MedioDePago { get; set; } = string.Empty;
        public string Titular { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;
        public string ReferenciaTarjeta { get; set; } = string.Empty;
        public string TipoTarjeta { get; set; } = string.Empty;
        public string Autorizacion { get; set; } = string.Empty;
        public decimal Mensajeria { get; set; } = 0m;
        public decimal IvaMensajeria { get; set; } = 0m;
        public decimal Servicio { get; set; } = 0m;
        public decimal IvaServicio { get; set; } = 0m;
        public decimal ComisionComercio { get; set; } = 0m;
        public decimal ComisionUsuario { get; set; } = 0m;
        public decimal IvaComision { get; set; } = 0m;
        public decimal TotalImporte { get; set; } = 0m;
        public decimal TotalCobrado { get; set; } = 0m;
        public string Promocion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Contrato { get; set; } = string.Empty;
        public string MensajeMedioDePago { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Plataforma { get; set; } = string.Empty;
        public string EstatusReclamacion { get; set; } = string.Empty;
    }
}
