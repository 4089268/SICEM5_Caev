using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Services;
using Sicem_Blazor.PagoLinea.Models;

namespace Sicem_Blazor.PagoLinea.Data
{

    public class PagoLineaService
    {
        private readonly SicemService sicemService;
        private readonly ILogger<PagoLineaService> logger;
        
        public PagoLineaService(SicemService s, ILogger<PagoLineaService> l)
        {
            this.sicemService = s;
            this.logger = l;
        }

        public ResumeOffice ObtenerResumen(IEnlace enlace, DateRange dateRange)
        {
            
            var response = new ResumeOffice(enlace)
            {
                Estatus = ResumenOficinaEstatus.Pendiente
            };

            try
            {
                logger.LogInformation("Obteniendo ingresos por pago en linea oficina:{Oficina} del {FechaInicial} al {FechaFin} ", enlace.Nombre, dateRange.Desde, dateRange.Hasta);
                using(var connection = new SqlConnection(enlace.GetConnectionString()))
                {
                    connection.Open();
                    string sqlCommandText = @"
                    BEGIN
                        Declare @i1 as decimal(14,2)
                        Declare @i2 as decimal(14,2)
                        Declare @i3 as decimal(14,2)
                        Declare @i4 as decimal(14,2)
                        
                        Declare @u1 as int
                        Declare @u2 as int
                        Declare @u3 as int

                        Select  @i1=SUM(total),
                                @u1=COUNT(id_padron),
                                @i3=SUM(cobrado) 
                        From [Facturacion].[Opr_Abonos] With(NoLock)
                        Where CONVERT(char(8),fecha,112) between @cFecha1 and @cFecha2 
                            And id_tipomovto = 6 
                            And id_estatus!=31
                            And id_caja=(select id_caja from [ventanillas].[Cfg_ArchivosXLS] where descripcion='LAYOUT BBVA')

                        Select  @i2=SUM(total),
                                @u2=COUNT(id_padron),
                                @i4=SUM(cobrado) 
                        From [Ventanillas].[Opr_Ventas] With(NoLock) 
                        Where CONVERT(char(8),fecha,112) between @cFecha1 and @cFecha2 
                            And id_estatus!=44
                            And cve_caja = ( select id_caja from [ventanillas].[Cfg_ArchivosXLS] where descripcion='LAYOUT BBVA')
                        
                        Select 
                            isnull(@i1,0) as importe_propios,
                            isnull(@u1,0)  as usuarios_propios,
                            isnull(@i2,0)  as importe_otros,
                            isnull(@u2,0)  as usuarios_otros,
                            importe = isnull(@i1,0) + isnull(@i2,0),
                            cobrado = isnull(@i3,0) + isnull(@i4,0),
                            usuarios = isnull(@u1,0) + isnull(@u2,0)
                    END";
                    var command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = sqlCommandText
                    };
                    command.Parameters.AddWithValue("@cFecha1", dateRange.Desde_ISO);
                    command.Parameters.AddWithValue("@cFecha2", dateRange.Hasta_ISO);
                    using(var dataReader = command.ExecuteReader())
                    {
                        if(dataReader.Read())
                        {
                            response.LoadFromReader(dataReader);
                        }
                    }
                    connection.Close();
                }
            }
            catch(Exception err)
            {
                logger.LogError(0, err, "Error al obtener resumen recaudacion enlace:{Enlace}", enlace.Nombre );
                response.Estatus = ResumenOficinaEstatus.Error;
            }
            return response;
        }

        public IEnumerable<OprVenta> ObtenerDetallePagos(IEnlace enlace, DateRange dateRange)
        {
            
            var response = new List<OprVenta>();

            try
            {
                logger.LogInformation("Obteniendo listado de pagos en linea oficina:{Oficina} del {FechaInicial} al {FechaFin} ", enlace.Nombre, dateRange.Desde, dateRange.Hasta);
                using(var connection = new SqlConnection(enlace.GetConnectionString()))
                {
                    connection.Open();
                    string sqlCommandText = @"
                    BEGIN
                        ;With cajasExterna as (
                            SELECT CONCAT('E', FORMAT(id_externa, '0000')) as id_caja
                            FROM [Ventanillas].[Cat_CajasExternas]
                            WHERE descripcion like '%PAGOS EN LINEA%'
                        )
                        SELECT
                            oa.id_abono as id,
                            [table] = 'Opr_Abonos',
                            oa.id_caja as cve_caja,
                            oa.id_tipomovto,
                            oa.fecha,
                            oa.id_padron,
                            oa.id_cuenta,
                            oa.subtotal,
                            oa.iva,
                            oa.total,
                            oa.cobrado,
                            oa.af,
                            oa.mf,
                            oa.observa_a as observa
                        From [Facturacion].[Opr_Abonos]  oa
                        Inner Join cajasExterna ce on oa.id_caja = ce.id_caja
                        WHERE 
                            oa.id_tipomovto = 6 and
                            CONVERT(varchar(8), fecha, 112) BETWEEN @dateFrom and @dateTo
                        UNION ALl
                        SELECT
                            ov.id_venta as id,
                            [table] = 'Opr_Ventas',
                            ov.cve_caja,
                            ov.id_tipomovto,
                            ov.fecha,
                            ov.id_padron,
                            ov.id_cuenta,
                            ov.subtotal,
                            ov.iva,
                            ov.total,
                            ov.cobrado,
                            ov.af,
                            ov.mf,
                            ov.observa_a as observa
                        FROM [Ventanillas].[Opr_Ventas] ov
                        Inner Join cajasExterna ce on ov.cve_caja = ce.id_caja
                        Where CONVERT(varchar(8), fecha, 112) BETWEEN @dateFrom and @dateTo
                        Order by fecha desc
                    END";
                    var command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = sqlCommandText
                    };
                    command.Parameters.AddWithValue("@dateFrom", dateRange.Desde_ISO);
                    command.Parameters.AddWithValue("@dateTo", dateRange.Hasta_ISO);
                    using(var dataReader = command.ExecuteReader())
                    {
                        while(dataReader.Read())
                        {
                            response.Add(OprVenta.FromSqlDataReader(enlace, dataReader));
                        }
                    }
                    connection.Close();
                }
            }
            catch(Exception err)
            {
                logger.LogError(0, err, "Error al obtener detalle de pagos en linea enlace:{Enlace}", enlace.Nombre );
            }
            return response;
        }

    }
}
