using System;
using System.Data;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.CFDI.Models;

public class Factura
{
    public IEnlace Enlace {get;set;}
    public int IdOficina {get => Enlace?.Id ?? 0; }
    public string Oficina {get => Enlace?.Nombre ?? string.Empty; }

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

    public Factura(IEnlace enalce)
    {
        this.Enlace = enalce;
    }

    public static Factura FromDataReader(IEnlace enlace, IDataReader reader)
    {
        var factura = new Factura(enlace);
        var tmpInt = 0;
        var tmpDoub = 0.0;
        var tmpDec = 0m;
        DateTime tmpDate;
    
        factura.IdCFDI = int.TryParse(reader["id_cfdi"].ToString(), out tmpInt) ? tmpInt : 0;
        factura.IdEstatus = int.TryParse(reader["id_estatus"].ToString(), out tmpInt) ? tmpInt : 0;
        factura.IdSucursal = int.TryParse(reader["id_sucursal"].ToString(), out tmpInt) ? tmpInt : 0;
        factura.IdFormaPago = int.TryParse(reader["id_formaPago"].ToString(), out tmpInt) ? tmpInt : 0;
        factura.IdTipoComprobante = int.TryParse(reader["id_tipoComprobante"].ToString(), out tmpInt) ? tmpInt : 0;
        factura.IdClaveRegimen = int.TryParse(reader["id_claveregimen"].ToString(), out tmpInt) ? tmpInt : 0;
        factura.IdFolio = int.TryParse(reader["id_folio"].ToString(), out tmpInt) ? tmpInt : 0;
        factura.IdLugarExpedicion = int.TryParse(reader["id_lugarExpedicion"].ToString(), out tmpInt) ? tmpInt : 0;
        factura.VersionCfdi = double.TryParse(reader["version_cfdi"].ToString(), out tmpDoub) ? tmpDoub : 0.0;
        factura.VersionTimbre = double.TryParse(reader["version_timbre"].ToString(), out tmpDoub) ? tmpDoub : 0.0;
        factura.Subtotal = decimal.TryParse(reader["subtotal"].ToString(), out tmpDec) ? tmpDec : 0m;
        factura.Iva = decimal.TryParse(reader["iva"].ToString(), out tmpDec) ? tmpDec : 0m;
        factura.Total = decimal.TryParse(reader["total"].ToString(), out tmpDec) ? tmpDec : 0m;
        factura.Tipocambio = decimal.TryParse(reader["tipo_cambio"].ToString(), out tmpDec) ? tmpDec : 0m;
        factura.Pagado = decimal.TryParse(reader["pagado"].ToString(), out tmpDec) ? tmpDec : 0m;
        factura.Parcial = decimal.TryParse(reader["parcial"].ToString(), out tmpDec) ? tmpDec : 0m;
        factura.Saldo = decimal.TryParse(reader["saldo"].ToString(), out tmpDec) ? tmpDec : 0m;
        factura.Inactivo = reader.GetBoolean(reader.GetOrdinal("inactivo"));
        factura.Estatus = reader["estatus"].ToString();
        factura.Sucursal = reader["sucursal"].ToString();
        factura.FormaPago = reader["forma_pago"].ToString();
        factura.MetodoPago = reader["metodo_pago"].ToString();
        factura.CveMetodoPago = reader["id_metodopago"].ToString();
        factura.Moneda = reader["moneda"].ToString();
        factura.CveMoneda = reader["id_moneda"].ToString();
        factura.UsoCFDI = reader["uso_cfdi"].ToString();
        factura.CveUsoCFDI = reader["id_usucfdi"].ToString();
        factura.TipoComprobante = reader["tipo_comprobante"].ToString();
        factura.ClaveRegimen = reader["clave_regimen"].ToString();
        factura.Cancelo = reader["cancelo"].ToString();
        factura.Genero = reader["genero"].ToString();
        factura.CveGenero = reader["id_genero"].ToString();
        factura.Serie = reader["serie"].ToString();
        factura.CveSerie = reader["cve_serie"].ToString();
        factura.MotivoCancela = reader["motivo_cancela"].ToString();
        factura.TotalLetras = reader["total_letras"].ToString();
        factura.Rfc = reader["rfc"].ToString();
        factura.RazonSocial = reader["razon_social"].ToString();
        factura.Email = reader["email"].ToString();
        factura.Folio = reader["folio"].ToString();
        factura.UUID = reader["UUID"].ToString();
        factura.FileXml = reader["file_xml"].ToString();
        factura.Sello = reader["sello"].ToString();
        factura.NoCertificado = reader["no_certificado"].ToString();
        factura.Certificado = reader["certificado"].ToString();
        factura.SelloCFD = reader["sello_cfd"].ToString();
        factura.Maquina = reader["maquina"].ToString();
        factura.IpAddress = reader["ip_address"].ToString();
        factura.NoCertSat = reader["no_cert_sat"].ToString();
        factura.SellloSat = reader["sello_sat"].ToString();
        factura.CadOrigiSat = reader["cad_origi_sat"].ToString();
        factura.ObervacionA = reader["observa_a"].ToString();
        factura.ObervacionC = reader["observa_c"].ToString();
        factura.RfcProveedorCertificante = reader["rfc_provedor_cert"].ToString();
        factura.Fecha = DateTime.TryParse(reader["fecha"].ToString(), out tmpDate) ? tmpDate : new DateTime();
        factura.FechaTimbrado = DateTime.TryParse(reader["fecha_timbrado"].ToString(), out tmpDate) ? tmpDate : new DateTime();
        factura.FechaInsert = DateTime.TryParse(reader["fecha_insert"].ToString(), out tmpDate) ? tmpDate : new DateTime();
        factura.FechaUpdate = DateTime.TryParse(reader["fecha_update"].ToString(), out tmpDate) ? tmpDate : new DateTime();
        factura.FechaPago = DateTime.TryParse(reader["fecha_pago"].ToString(), out tmpDate) ? tmpDate : null;
        factura.FechaCancelo = DateTime.TryParse(reader["fecha_cancelo"].ToString(), out tmpDate) ? tmpDate : null;

        return factura;
    }

}
