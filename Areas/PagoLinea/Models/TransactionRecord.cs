using System;
using System.Data;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.PagoLinea.Models;

public class TransactionRecord
{
    public long ID { get; set; }
    public DateTime Fecha { get; set; }
    public DateTime? FechaDispersion { get; set; }
    public string Comercio { get; set; }
    public string Unidad { get; set; }
    public string Concepto { get; set; }
    public string ReferenciaComercio { get; set; }
    public string Orden { get; set; }
    public string Tipo { get; set; }
    public string MedioDePago { get; set; }
    public string Titular { get; set; }
    public string Banco { get; set; }
    public string ReferenciaTarjeta { get; set; }
    public string TipoTarjeta { get; set; }
    public string Autorizacion { get; set; }
    public decimal Mensajeria { get; set; }
    public decimal IvaMensajeria { get; set; }
    public decimal Servicio { get; set; }
    public decimal IvaServicio { get; set; }
    public decimal ComisionComercio { get; set; }
    public decimal ComisionUsuario { get; set; }
    public decimal IvaComision { get; set; }
    public decimal TotalImporte { get; set; }
    public decimal TotalCobrado { get; set; }
    public string Promocion { get; set; }
    public string Estado { get; set; }
    public long Contrato { get; set; }
    public string MensajeMedioDePago { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
    public string Plataforma { get; set; }
    public string EstatusReclamacion { get; set; }
    public string FileName { get; set; }
}