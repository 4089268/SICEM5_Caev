using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data;
using Sicem_Blazor.CFDI.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Sicem_Blazor.CFDI.Services;

public class FacturaService
{
    private ILogger<FacturaService> logger;

    public FacturaService(ILogger<FacturaService> logger)
    {
        this.logger = logger;
    }

    public ResumenFactura ObtenerResumenFactura(IEnlace enlace, DateTime desde, DateTime hasta)
    {
        ResumenFactura _result = new ResumenFactura
        {
            Estatus = Data.Contracts.ResumenOficinaEstatus.Error,
            Enlace = enlace
        };

        try
        {
            var _query = @"
                SELECT
                    id_sucursal = c.id_sucursal,
                    sucursal = su.descripcion,
                    total_fact = Count(c.id_sucursal),
                    por_timbrar = Sum(Case When c.id_estatus = 63 Then 1 Else 0 End),
                    timbrado = Sum(Case When c.id_estatus = 64 Then 1 Else 0 End),
                    cancelado = Sum(Case When c.id_estatus = 65 Then 1 Else 0 End),
                    subtotal = Sum(dt.sub),
                    iva = Sum(dt.iva),
                    total = Sum(dt.sub + dt.iva)
                FROM CFDI.Opr_Cfdis as c
                Inner Join Global.Cat_Sucursales as su on su.id_sucursal = c.id_sucursal
                Cross Apply (
                    select sub=sum(subtotal), iva=sum(iva) from CFDI.Opr_DetCfdis as d where d.id_cfdi=c.id_cfdi
                ) as dt
                Where CONVERT(char(8), c.fecha_insert ,112) between @pDesde and @pHasta
                Group by c.id_sucursal, su.descripcion
                Order by c.id_sucursal";

            _query = _query.Replace("@pDesde", desde.ToString("yyyyMMdd"));
            _query = _query.Replace("@pHasta", hasta.ToString("yyyyMMdd"));

            using(var _connection = new SqlConnection(enlace.GetConnectionString()))
            {
                _connection.Open();
                var _command = new SqlCommand(_query, _connection);
                using(SqlDataReader _reader = _command.ExecuteReader())
                {
                    if(_reader.Read())
                    {
                        _result.IdSucursal = _reader["id_sucursal"].ToString();
                        _result.Sucursal = _reader["sucursal"].ToString();
                        _result.TotalFacturas = ConvertUtils.ParseInteger(_reader["total_fact"]);
                        _result.FactsPorTimbrara = ConvertUtils.ParseInteger(_reader["por_timbrar"]);
                        _result.FactsTimbrado = ConvertUtils.ParseInteger(_reader["timbrado"]);
                        _result.FactsCancelado = ConvertUtils.ParseInteger(_reader["cancelado"]);
                        _result.SubTotal = ConvertUtils.ParseDecimal(_reader["subtotal"]);
                        _result.Iva = ConvertUtils.ParseDecimal(_reader["iva"]);
                        _result.Total = ConvertUtils.ParseDecimal(_reader["total"]);
                        _result.Estatus = Data.Contracts.ResumenOficinaEstatus.Completado;
                    }
                }
                _connection.Close();
            }
        }
        catch(Exception err)
        {
            _result.Estatus = Data.Contracts.ResumenOficinaEstatus.Error;
            logger.LogError(err, "Error al obtener el resumen de facturas del enalce '{enlace}'", enlace.Nombre);
        }

        return _result;
    }

    public async Task<IEnumerable<Factura>> ObtenerFacturasOficina(IEnlace enlace, DateTime desde, DateTime hasta)
    {
        await Task.CompletedTask;
        var _result = new List<Factura>();
        try
        {
            var _query = @"
            SELECT c.id_cfdi
                ,[estatus] = s.descripcion 
                ,[id_estatus] = s.id_estatus
                ,[sucursal] = su.descripcion 
                ,[id_sucursal] = su.id_sucursal
                ,[forma_pago] = fp.descripcion 
                ,[id_formaPago] = fp.id_formapago
                ,[metodo_pago] = mp.descripcion 
                ,[id_metodopago] = c.id_metodo_pago
                ,[moneda] = m.descripcion 
                ,[id_moneda] = c.id_moneda
                ,[uso_cfdi] = u.descripcion
                ,[id_usucfdi] = c.id_usocfdi
                ,[tipo_comprobante] = UPPER(tc.descripcion) 
                ,[id_tipoComprobante] = tc.id_tipocomprobante
                ,[clave_regimen] = rg.descripcion 
                ,[id_claveregimen] = rg.id_claveregimen
                ,[cancelo] = us.descripcion 
                ,[genero] = c.id_genero +'-'+gn.descripcion 
                ,[id_genero] = c.id_genero
                ,[serie] = sf.descripcion 
                ,[cve_serie] = c.serie
                ,[motivo_cancela] = mt.descripcion 
                ,[subtotal] = dt.sub
                ,[iva] = dt.iva 
                ,[total] = dt.sub + dt.iva
                ,[total_letras] = c.total_en_letras
                ,[inactivo] = c.inactivo
                ,[rfc] = c.rfc
                ,[razon_social] = c.razon_social
                ,[email] = c.email
                ,[folio] = c.id_folio
                ,[id_folio] = c.folio
                ,[fecha] = c.fecha
                ,[tipo_cambio] = c.tipocambio
                ,[id_lugarExpedicion] = c.lugar_expedicion
                ,[version_cfdi] = c.version_cfdi
                ,[UUID] = c.UUID
                ,[fecha_timbrado] = c.fecha_timbrado
                ,[file_xml] = c.file_xml
                ,[sello] = c.sello
                ,[no_certificado] = c.nocertificado
                ,[certificado] = c.certificado
                ,[sello_cfd] = c.sello_CFD
                ,[fecha_insert] = c.fecha_insert
                ,[fecha_update] = c.fecha_update
                ,[maquina] = c.maquina
                ,[ip_address] = c.ip_address
                ,[rfc_provedor_cert] = c.rfc_proveedor_certificante
                ,[version_timbre] = c.version_timbre
                ,[no_cert_sat] = c.no_certificado_SAT
                ,[sello_sat] = c.sello_sat
                ,[cad_origi_sat] = c.cadena_original_SAT
                ,[pagado] = IsNUll(c.pagado, 0.0)
                ,[parcial] = IsNull(c.parcial, 0.0)
                ,[saldo] = c.saldo
                ,[fecha_pago] = c.fecha_pago
                ,[fecha_cancelo] = c.fecha_cancelo
                ,[observa_a] = c.observa_a
                ,[observa_c] = c.observa_c
                --,c.* 
            FROM CFDI.Opr_Cfdis as c 
                inner join Global.Cat_Estatus		as s on s.id_estatus = c.id_estatus 
                inner join Global.Cat_Sucursales	as su on su.id_sucursal = c.id_sucursal 
                inner join CFDI.Cat_FormasPago		as fp on fp.clave = c.id_forma_pago 
                inner join CFDi.Cat_MetodosPago		as mp on mp.clave = c.id_metodo_pago 
                inner join CFDI.Cat_Monedas			as m on m.clave = c.id_moneda 
                inner join CFDI.Cat_UsosCFDI		as u on u.clave = c.id_usocfdi 
                inner join CFDI.Cat_RegimenFiscal	as rg on rg.id_claveregimen = c.id_claveregimen 
                inner join CFDI.Cat_TiposComprobante as tc on tc.clave = c.id_tipo_comprobante 
                inner join Ventanillas.vw_Cat_Cajas as us on us.id_caja = isnull(c.id_cancelo,'') 
                inner join Ventanillas.vw_Cat_Cajas as gn on gn.id_caja = isnull(c.id_genero ,'') 
                inner join CFDI.Cat_Series			as sf on sf.prefijo = c.serie 
                inner join CFDI.Cat_MotivosCancela	as mt on mt.clave = c.id_clavemotivocancela 
                cross apply (
                    select sub=sum(subtotal), iva=sum(iva) from CFDI.Opr_DetCfdis as d where d.id_cfdi=c.id_cfdi
                ) as dt
            Where CONVERT(char(8), c.fecha_insert ,112) between '@pDesde' and '@pHasta'
            Order by fecha_insert desc
            ";
            _query = _query.Replace("@pDesde", desde.ToString("yyyyMMdd"));
            _query = _query.Replace("@pHasta", hasta.ToString("yyyyMMdd"));
            using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                sqlConnection.Open();
                var command = new SqlCommand(_query, sqlConnection);
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _newFactura = new Factura();
                        var tmpInt = 0;
                        var tmpDoub = 0.0;
                        var tmpDec = 0m;
                        DateTime tmpDate;
                        _newFactura.IdCFDI = int.TryParse(reader["id_cfdi"].ToString(),out tmpInt)?tmpInt: 0;
                        _newFactura.IdEstatus = int.TryParse(reader["id_estatus"].ToString(),out tmpInt)?tmpInt:0;
                        _newFactura.IdSucursal = int.TryParse(reader["id_sucursal"].ToString(),out tmpInt)?tmpInt:0;
                        _newFactura.IdFormaPago = int.TryParse(reader["id_formaPago"].ToString(),out tmpInt)?tmpInt:0;
                        _newFactura.IdTipoComprobante = int.TryParse(reader["id_tipoComprobante"].ToString(),out tmpInt)?tmpInt:0;
                        _newFactura.IdClaveRegimen = int.TryParse(reader["id_claveregimen"].ToString(),out tmpInt)?tmpInt:0;
                        _newFactura.IdFolio = int.TryParse(reader["id_folio"].ToString(), out tmpInt)?tmpInt:0;
                        _newFactura.IdLugarExpedicion = int.TryParse(reader["id_lugarExpedicion"].ToString(), out tmpInt) ? tmpInt : 0;
                        _newFactura.VersionCfdi = double.TryParse(reader["version_cfdi"].ToString(), out tmpDoub) ? tmpDoub : 0.0;
                        _newFactura.VersionTimbre = double.TryParse(reader["version_timbre"].ToString(), out tmpDoub) ? tmpDoub : 0.0;
                        _newFactura.Subtotal = decimal.TryParse(reader["subtotal"].ToString(), out tmpDec) ? tmpDec : 0m;
                        _newFactura.Iva = decimal.TryParse(reader["iva"].ToString(), out tmpDec) ? tmpDec : 0m;
                        _newFactura.Total = decimal.TryParse(reader["total"].ToString(), out tmpDec) ? tmpDec : 0m;
                        _newFactura.Tipocambio = decimal.TryParse(reader["tipo_cambio"].ToString(), out tmpDec) ? tmpDec : 0m;
                        _newFactura.Pagado = decimal.TryParse(reader["pagado"].ToString(), out tmpDec) ? tmpDec : 0m;
                        _newFactura.Parcial = decimal.TryParse(reader["parcial"].ToString(), out tmpDec) ? tmpDec : 0m;
                        _newFactura.Saldo = decimal.TryParse(reader["saldo"].ToString(), out tmpDec) ? tmpDec : 0m;
                        _newFactura.Inactivo = reader.GetBoolean("inactivo");
                        _newFactura.Estatus = reader["estatus"].ToString();
                        _newFactura.Sucursal = reader["sucursal"].ToString();
                        _newFactura.FormaPago = reader["forma_pago"].ToString();
                        _newFactura.MetodoPago = reader["metodo_pago"].ToString();
                        _newFactura.CveMetodoPago = reader["id_metodopago"].ToString();
                        _newFactura.Moneda = reader["moneda"].ToString();
                        _newFactura.CveMoneda = reader["id_moneda"].ToString();
                        _newFactura.UsoCFDI = reader["uso_cfdi"].ToString();
                        _newFactura.CveUsoCFDI = reader["id_usucfdi"].ToString();
                        _newFactura.TipoComprobante = reader["tipo_comprobante"].ToString();
                        _newFactura.ClaveRegimen = reader["clave_regimen"].ToString();
                        _newFactura.Cancelo = reader["cancelo"].ToString();
                        _newFactura.Genero = reader["genero"].ToString();
                        _newFactura.CveGenero = reader["id_genero"].ToString();
                        _newFactura.Serie = reader["serie"].ToString();
                        _newFactura.CveSerie = reader["cve_serie"].ToString();
                        _newFactura.MotivoCancela = reader["motivo_cancela"].ToString();
                        _newFactura.TotalLetras = reader["total_letras"].ToString();
                        _newFactura.Rfc =           reader["rfc"].ToString();
                        _newFactura.RazonSocial =   reader["razon_social"].ToString();
                        _newFactura.Email =         reader["email"].ToString();
                        _newFactura.Folio =         reader["folio"].ToString();
                        _newFactura.UUID =          reader["UUID"].ToString();
                        _newFactura.FileXml =       reader["file_xml"].ToString();
                        _newFactura.Sello =         reader["sello"].ToString();
                        _newFactura.NoCertificado = reader["no_certificado"].ToString();
                        _newFactura.Certificado =   reader["certificado"].ToString();
                        _newFactura.SelloCFD =      reader["sello_cfd"].ToString();
                        _newFactura.Maquina =       reader["maquina"].ToString();
                        _newFactura.IpAddress =     reader["ip_address"].ToString();
                        _newFactura.NoCertSat =     reader["no_cert_sat"].ToString();
                        _newFactura.SellloSat =     reader["sello_sat"].ToString();
                        _newFactura.CadOrigiSat =   reader["cad_origi_sat"].ToString();
                        _newFactura.ObervacionA =   reader["observa_a"].ToString();
                        _newFactura.ObervacionC =   reader["observa_c"].ToString();
                        _newFactura.RfcProveedorCertificante = reader["rfc_provedor_cert"].ToString();
                        _newFactura.Fecha =         DateTime.TryParse(reader["fecha"].ToString(), out tmpDate)?tmpDate:new DateTime();
                        _newFactura.FechaTimbrado = DateTime.TryParse(reader["fecha_timbrado"].ToString(), out tmpDate)?tmpDate:new DateTime();
                        _newFactura.FechaInsert =   DateTime.TryParse(reader["fecha_insert"].ToString(), out tmpDate)?tmpDate:new DateTime();
                        _newFactura.FechaUpdate =   DateTime.TryParse(reader["fecha_update"].ToString(), out tmpDate)?tmpDate:new DateTime();
                        _newFactura.FechaPago =     DateTime.TryParse(reader["fecha_pago"].ToString(), out tmpDate)?tmpDate:null;
                        _newFactura.FechaCancelo =  DateTime.TryParse(reader["fecha_cancelo"].ToString(), out tmpDate)?tmpDate:null;
                    
                        _result.Add(_newFactura);
                    }
                }
                sqlConnection.Close();
            }
            return _result;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, $"Error al tratar de obtener el listado de facturas del enlace ({enlace.ToString()})");
            return new Factura[]{};
        }
    }

    public IEnumerable<ConceptoFactura> ObtenerConceptosFactura(IEnlace enlace, int id_cfdi)
        {
            var _result = new List<ConceptoFactura>();

            try
            {
                var _query = @"SELECT d.*
                    FROM CFDI.Opr_DetCfdis as d
                    Inner join CFDI.Opr_Cfdis as a on d.id_cfdi = a.id_cfdi 
                    Where d.id_cfdi = Case When @idCfdi = -1 Then d.id_cfdi Else @idCfdi End
                    Order by iva, CASE WHEN concepto like 'REZ.%' THEN 2 WHEN concepto like 'ABONO%' THEN 1 ELSE 0 END,id_concepto";
                _query = _query.Replace("@idCfdi", id_cfdi.ToString());
                
                using( var _connection = new SqlConnection(enlace.GetConnectionString()))
                {
                    _connection.Open();

                    var _command = new SqlCommand(_query, _connection);
                    using(SqlDataReader _reader = _command.ExecuteReader())
                    {
                        while(_reader.Read())
                        {
                            var _newItem = new ConceptoFactura();
                            _newItem.IdCFDI = ConvertUtils.ParseInteger(_reader["id_cfdi"].ToString());
                            _newItem.IdConcepto = ConvertUtils.ParseInteger(_reader["id_concepto"].ToString());
                            _newItem.Concepto = _reader["concepto"].ToString();
                            _newItem.Subtotal = ConvertUtils.ParseDecimal(_reader["subtotal"].ToString());
                            _newItem.Iva = ConvertUtils.ParseDecimal(_reader["iva"].ToString());
                            _newItem.Total = ConvertUtils.ParseDecimal(_reader["total"].ToString());
                            _newItem.ClaveProdServ = _reader["ClaveProdServ"].ToString();
                            _newItem.ClaveUnidad = _reader["ClaveUnidad"].ToString();
                            _newItem.Cantidad = _reader["cantidad"].ToString();
                            _result.Add(_newItem);
                        }
                    }
                    _connection.Close();
                }

                return _result;
            }
            catch(Exception err)
            {
                logger.LogError(err, $"Error al obtener el resumen de facturas del enalce ({enlace.ToString()})");
                return new ConceptoFactura[] { };
            }
        }
}
