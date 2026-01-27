using System;

namespace Sicem_Blazor.CFDI.Models;

public class Factura
{
    public int IdCFDI { get; set; }
    public string Estatus {get;set;}
    public int IdEstatus { get; set; }
    public string Sucursal {get;set;}
    public int IdSucursal { get; set; }
    public string FormaPago {get;set;}
    public int IdFormaPago { get; set; }
    public string MetodoPago {get;set;}
    public string CveMetodoPago { get; set; }
    public string Moneda {get;set;}
    public string CveMoneda { get; set; }
    public string UsoCFDI{get;set;}
    public string CveUsoCFDI { get; set; }
    public string TipoComprobante {get;set;}
    public int IdTipoComprobante { get; set; }
    public string ClaveRegimen {get;set;}
    public int  IdClaveRegimen { get; set; }
    public string Cancelo {get;set;}
    public string Genero {get;set;}
    public string CveGenero { get; set; }
    public string Serie {get;set;}
    public string CveSerie { get; set; }
    public string  MotivoCancela {get;set;}
    public decimal Subtotal {get;set;}
    public decimal Iva {get;set;}
    public decimal Total { get; set; }
    public string TotalLetras { get; set; }
    public bool Inactivo {get;set;}
    public string Rfc {get;set;}
    public string RazonSocial {get;set;}
    public string Email {get;set;}
    public string Folio {get;set;}
    public int IdFolio {get;set;}
    public DateTime Fecha {get;set;}
    public decimal Tipocambio {get;set;}
    public int IdLugarExpedicion {get;set;}
    public double VersionCfdi {get;set;}
    public string UUID {get;set;}
    public DateTime FechaTimbrado {get;set;}
    public string FileXml {get;set;}
    public string Sello {get;set;}
    public string NoCertificado {get;set;}
    public string Certificado {get;set;}
    public string SelloCFD {get;set;}
    public DateTime FechaInsert {get;set;}
    public DateTime FechaUpdate {get;set;}
    public string Maquina {get;set;}
    public string IpAddress {get;set;}
    public string RfcProveedorCertificante {get;set;}
    public double VersionTimbre {get;set;}
    public string NoCertSat{get;set;}
    public string SellloSat {get;set;}
    public string CadOrigiSat {get;set;}
    public decimal Pagado {get;set;}
    public decimal Parcial {get;set;}
    public decimal Saldo {get;set;}
    public DateTime? FechaPago {get;set;}
    public DateTime? FechaCancelo { get; set; }
    public string ObervacionA { get; set; }
    public string ObervacionC { get; set; }
    public string DescripcionConceptos {get;set;}
}
