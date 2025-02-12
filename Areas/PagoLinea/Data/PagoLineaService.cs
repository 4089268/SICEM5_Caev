﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Services;
using Sicem_Blazor.PagoLinea.Models;
using Sicem_Blazor.Models;
using Microsoft.VisualBasic;

namespace Sicem_Blazor.PagoLinea.Data
{

    public class PagoLineaService
    {
        private readonly SicemService sicemService;
        private readonly ILogger<PagoLineaService> logger;

        private IEnumerable<Ruta> enlaces = [];
        
        public PagoLineaService(SicemService s, ILogger<PagoLineaService> l)
        {
            this.sicemService = s;
            this.logger = l;
        }

        public ResumeOfficeOld ObtenerResumen(IEnlace enlace, DateRange dateRange)
        {
            
            var response = new ResumeOfficeOld(enlace)
            {
                Estatus = ResumenOficinaEstatus.Pendiente
            };

            try
            {
                logger.LogInformation("Obteniendo ingresos por pago en linea oficina:{Oficina} del {FechaInicial} al {FechaFin} ", enlace.Nombre, dateRange.Desde.Date, dateRange.Hasta.Date);
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

        public async Task<IEnumerable<TransactionRecord>> CalulateStatusPayment(IEnumerable<TransactionRecord> records)
        {
            // * load the enlaces
            this.enlaces = this.sicemService.ObtenerEnlaces();

            // * group the records by Concepto (Office)
            var recordsGroupedByConcepto = records.GroupBy(item => item.Concepto);

            // * process each group asynchronously
            var processedRecords = await Task.WhenAll(
                recordsGroupedByConcepto.Select(async group =>
                    await ProcessOffice(group.Key, group.ToArray())
                )
            );

            // * flatten the results and return
            return processedRecords.SelectMany(records => records);
        }

        public async Task<IEnumerable<TransactionRecord>> ProcessOffice(string concepto, IEnumerable<TransactionRecord> records)
        {
            // * TODO: Create adapter to retrive the enlace by the concept
            await Task.CompletedTask;

            try
            {
                var enlace = this.enlaces.FirstOrDefault( item => concepto.ToLower().Contains( item.Nombre, StringComparison.CurrentCultureIgnoreCase));
                if( enlace == null)
                {
                    throw new ArgumentNullException($"Enlace of concept {concepto} not found");
                }

                var f1 = records.OrderBy( item => item.Fecha).First().Fecha;
                var f2 = records.OrderByDescending( item => item.Fecha).First().Fecha.AddDays(1);
                var payments = this.ObtenerDetallePagos(enlace, new DateRange(f1, f2));
                var referencesPaided = payments.Select( item => item.ReferenciaComercio.Trim()).ToArray();
                
                // * compare the reference with the references stored in the office
                foreach( var r in records)
                {
                    r.Status = referencesPaided.Contains(r.ReferenciaComercio.Trim())
                        ?TransactionRecord.ProcessFileStatues.Paided
                        :TransactionRecord.ProcessFileStatues.NoPaided;
                }

                return records;
            }
            catch (Exception err)
            {
                this.logger.LogError(err, "Fail at retrive payments: {message}", err.Message);
                foreach(var r in records)
                {
                    r.Status = TransactionRecord.ProcessFileStatues.Fail;
                };
                return records;
                
            }
        }
    

        public async Task<PagoLineaResumen> ObtenerRumenPagos(IEnlace enlace, DateRange dateRange)
        {
            var response = new PagoLineaResumen(enlace)
            {
                Estatus = ResumenOficinaEstatus.Pendiente
            };

            try
            {
                logger.LogInformation("Obteniendo resumen pago en linea oficina:{Oficina} del {FechaInicial} al {FechaFin} ", enlace.Nombre, dateRange.Desde.Date, dateRange.Hasta.Date);
                using(var connection = new SqlConnection(enlace.GetConnectionString()))
                {
                    connection.Open();
                    var sqlCommandText = "EXEC [Sicem].[Pago_en_Linea] 'RESUMEN', @oficeId, @f1, @f2";
                    var command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = sqlCommandText
                    };
                    command.Parameters.AddWithValue("@oficeId", enlace.Id);
                    command.Parameters.AddWithValue("@f1", dateRange.Desde_ISO);
                    command.Parameters.AddWithValue("@f2", dateRange.Hasta_ISO);
                    using(var dataReader = await command.ExecuteReaderAsync())
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

        public async Task<IEnumerable<PagoLineaResumenDia>> ObtenerRumenPagosDia(IEnlace enlace, DateRange dateRange)
        {
            var responseList = new List<PagoLineaResumenDia>();
            try
            {
                logger.LogInformation("Obteniendo resumen por dias de por pago en linea oficina:{Oficina} del {FechaInicial} al {FechaFin} ", enlace.Nombre, dateRange.Desde.Date, dateRange.Hasta.Date);
                using(var connection = new SqlConnection(enlace.GetConnectionString()))
                {
                    connection.Open();
                    var sqlCommandText = "EXEC [Sicem].[Pago_en_Linea] 'RESUMEN_X_DIA',@oficeId, @f1, @f2";
                    var command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = sqlCommandText
                    };
                    command.Parameters.AddWithValue("@oficeId", enlace.Id);
                    command.Parameters.AddWithValue("@f1", dateRange.Desde_ISO);
                    command.Parameters.AddWithValue("@f2", dateRange.Hasta_ISO);
                    using(var dataReader = await command.ExecuteReaderAsync())
                    {
                        while(dataReader.Read())
                        {
                            var item = new PagoLineaResumenDia(enlace);
                            item.LoadFromReader(dataReader);
                            responseList.Add(item);
                        }
                    }
                    connection.Close();
                }
            }
            catch(Exception err)
            {
                logger.LogError(0, err, "Error al obtener resumen por dias enlace:{Enlace}", enlace.Nombre);
                return null;
            }
            return responseList;
        }

        public async Task<IEnumerable<PagoLineaDetalle>> ObtenerDetallePagos2(IEnlace enlace, DateRange dateRange)
        {
            var responseList = new List<PagoLineaDetalle>();
            try
            {
                logger.LogInformation("Obteniendo resumen por dias de por pago en linea oficina:{Oficina} del {FechaInicial} al {FechaFin} ", enlace.Nombre, dateRange.Desde.Date, dateRange.Hasta.Date);
                using(var connection = new SqlConnection(enlace.GetConnectionString()))
                {
                    connection.Open();
                    var sqlCommandText = "EXEC [Sicem].[Pago_en_Linea] 'DETALLE', @oficeId, @f1, @f2";
                    var command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = sqlCommandText
                    };
                    command.Parameters.AddWithValue("@oficeId", enlace.Id);
                    command.Parameters.AddWithValue("@f1", dateRange.Desde_ISO);
                    command.Parameters.AddWithValue("@f2", dateRange.Hasta_ISO);
                    using(var dataReader = await command.ExecuteReaderAsync())
                    {
                        while(dataReader.Read())
                        {
                            responseList.Add(PagoLineaDetalle.FromDataReader(enlace, dataReader));
                        }
                    }
                    connection.Close();
                }
            }
            catch(Exception err)
            {
                logger.LogError(0, err, "Error al obtener resumen por dias enlace:{Enlace}", enlace.Nombre);
                return null;
            }
            return responseList;
        }

        public async Task<AplicarPagoResult> AplicarPago(IEnlace enlace, PagoLineaDetalle detallePago)
        {
            var responseResult = new AplicarPagoResult()
            {
                Id = detallePago.Id,
                Fecha = detallePago.Fecha,
                Importe = detallePago.Importe,
                Estatus = ResumenOficinaEstatus.Pendiente
            };

            try
            {

                // * obtener la cve de la caja
                var cveCaja = await this.GetCveCaja(enlace);

                logger.LogInformation("Aplicando pago oficina:{Oficina}", enlace.Nombre);
                using(var connection = new SqlConnection(enlace.GetConnectionString()))
                {
                    connection.Open();
                    var sqlCommandText = "EXEC [Ventanillas].[usp_AplicarPagoWEB] @cAlias=N'APLICAR_PAGO',@cIdReferencia=N'', @xmlTables=@cXML";
                    var command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = sqlCommandText
                    };
                    command.Parameters.AddWithValue("@cXML", detallePago.ToXml(cveCaja));
                    this.logger.LogDebug(detallePago.ToXml(cveCaja));
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // * Check first result set
                        if (await reader.ReadAsync())
                        {
                            responseResult.Estatus = ResumenOficinaEstatus.Error;
                            responseResult.Fecha = DateTime.Now;
                            responseResult.Operacion = reader["Operacion"].ToString();
                            responseResult.Error = reader.GetInt32(reader.GetOrdinal("Error"));
                            responseResult.Mensaje = reader["Mensaje"].ToString();
                        }
                        else
                        {
                            // * Move to second result set
                            if (await reader.NextResultAsync() && await reader.ReadAsync())
                            {
                                responseResult.Estatus = ResumenOficinaEstatus.Completado;
                                responseResult.FolioPago = reader["id_folio"].ToString();
                            }
                            else
                            {
                                responseResult.Estatus = ResumenOficinaEstatus.Error;
                                responseResult.Error = 0;
                                responseResult.Mensaje = "Sin respuesta";
                            }
                        }

                    }
                }
            }
            catch(Exception err)
            {
                logger.LogError(0, err, "Error al obtener resumen por dias enlace:{Enlace}", enlace.Nombre);
                responseResult.Estatus = ResumenOficinaEstatus.Error;
                responseResult.Error = 0;
                responseResult.Mensaje = err.Message;
            }
            return responseResult;
        }
    
        public async Task<string> GetCveCaja(IEnlace enlace)
        {
            string cveCaja = string.Empty;
            try
            {
                using(var connection = new SqlConnection(enlace.GetConnectionString()))
                {
                    connection.Open();
                    var sqlCommandText = "Select id_caja From [Ventanillas].[Cfg_ArchivosXLS] Where Descripcion='LAYOUT BBVA'";
                    var command = new SqlCommand(sqlCommandText, connection);
                    cveCaja = (await command.ExecuteScalarAsync()).ToString();
                    connection.Close();
                }
            }
            catch(Exception err)
            {
                logger.LogError(0, err, "Error al obtener resumen por dias enlace:{Enlace}", enlace.Nombre);
            }
            return cveCaja;
            
        }
    }
}
