using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data;
using Sicem_Blazor.CFDI.Models;
using System.Data.SqlClient;

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

}
