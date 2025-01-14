using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;

namespace Sicem_Blazor.Models.PagoLinea
{
    public class PaymentDetails
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaDispersion { get; set; }
        public string Comercio { get; set; } = default!;
        public string Unidad { get; set; } = default!;
        public string Concepto { get; set; } = default!;
        public string ReferenciaComercio { get; set; } = default!;
        public string Orden { get; set; } = default!;
        public string Tipo { get; set; } = default!;
        public string MedioPago { get; set; } = default!;
        public string Titular { get; set; } = default!;
        public string Banco { get; set; } = default!;
        public string ReferenciaTarjeta { get; set; } = default!;
        public string TipoTarjeta { get; set; } = default!;
        public string Autorizacion { get; set; } = default!;
        public decimal IvaMensajeria { get; set; }
        public decimal Servicio { get; set; }
        public decimal IvaServicio { get; set; }
        public decimal ComisionComercio { get; set; }
        public decimal ComisionUsuario { get; set; }
        public decimal IvaComision { get; set; }
        public decimal TotalImporte { get; set; }
        public decimal TotalCobrado { get; set; }
        public string Promocion { get; set; } = default!;
        public string Estado { get; set; } = default!;
        public string Contrato { get; set; } = default!;
        public string MensajeMedioPago { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Telefono { get; set; } = default!;
        public string Plataforma { get; set; } = default!;
        public string EstatusReclamacion { get; set; } = default!;
        public int Localidad { get; set; }
        public int IdPadron { get; set; }
        public int Af { get; set; }
        public int Mf { get; set; }
        public int IdCuenta { get; set; }
        public string RazonSocial { get; set; } = default!;
        public int OficinaId { get; set; }
        public string Oficina { get; set; } = default!;

        public string IvaMensajeriaCurrency { get => IvaMensajeria.ToString("c2", new CultureInfo("es-MX")); }
        public string ServicioCurrency { get => Servicio.ToString("c2", new CultureInfo("es-MX")); }
        public string IvaServicioCurrency { get => IvaServicio.ToString("c2", new CultureInfo("es-MX")); }
        public string ComisionComercioCurrency { get => ComisionComercio.ToString("c2", new CultureInfo("es-MX")); }
        public string ComisionUsuarioCurrency { get => ComisionUsuario.ToString("c2", new CultureInfo("es-MX")); }
        public string IvaComisionCurrency { get => IvaComision.ToString("c2", new CultureInfo("es-MX")); }
        public string TotalImporteCurrency { get => TotalImporte.ToString("c2", new CultureInfo("es-MX")); }
        public string TotalCobradoCurrency { get => TotalCobrado.ToString("c2", new CultureInfo("es-MX")); }

    }

}