using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;
using Sicem_Blazor.PonteAlCorriente.Models;

namespace Sicem_Blazor.PonteAlCorriente.Data
{

    public class PonteAlCorrienteService
    {
        private readonly SicemService sicemService;
        private readonly ILogger<PonteAlCorrienteService> logger;

        public PonteAlCorrienteService(SicemService s, ILogger<PonteAlCorrienteService> l)
        {
            this.sicemService = s;
            this.logger = l;
        }

        public ResumeOffice ObtenerResumen(IEnlace enlace, DateRange dateRange)
        {
            var response = new ResumeOffice(enlace)
            {
                Estatus = ResumenOficinaEstatus.Error
            };

            try
            {
                logger.LogInformation("Obteniendo resumen ponte al corriente oficina:{Oficina} del {FechaInicial} al {FechaFin} ", enlace.Nombre, dateRange.Desde.Date, dateRange.Hasta.Date);
                using(var connection = new SqlConnection(enlace.GetConnectionString()))
                {
                    connection.Open();
                    string sqlCommandText = @"
                        SELECT
                            IsNull(Count(a.id),0) as num_desctos,
                            IsNull(Sum(Isnull(a.total,0)),0) as imp_descontado,
                            IsNull(Sum(Isnull(p.cobrado,0)),0) as imp_cobrado
                        FROM Facturacion.Opr_DetDescuentos d with(nolock)
                        INNER JOIN Facturacion.Opr_Abonos a with(nolock) on a.id_abono=d.id_abono
                        OUTER APPLY
                        (
                            SELECT Sum(Isnull(cobrado,0)) as cobrado
                            FROM Facturacion.Opr_Abonos with(nolock)
                            WHERE id_padron = a.id_padron and id_tipomovto = 6 and fecha_amd = a.fecha_amd and id_estatus != 31
                        ) p
                        WHERE
                            d.cve_descuento='RB10' AND a.fecha_amd BetWeen @dateFrom and @dateTo";
                    var command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = sqlCommandText
                    };
                    command.Parameters.AddWithValue("@dateFrom", dateRange.Desde_ISO);
                    command.Parameters.AddWithValue("@dateTo", dateRange.Hasta_ISO);
                    using(var dataReader = command.ExecuteReader())
                    {
                        if(dataReader.Read())
                        {
                            response = ResumeOffice.FromDataReader(enlace, dataReader);
                        }
                    }
                    connection.Close();
                }
            }
            catch(Exception err)
            {
                logger.LogError(0, err, "Error al obtener resumen ponte al corriente enlace:{Enlace}", enlace.Nombre );
                response.Estatus = ResumenOficinaEstatus.Error;
            }
            return response;
        }

        public IEnumerable<DetalleOficina> DetallePonteAlCorriente(IEnlace enlace, DateRange dateRange)
        {
            var response = new List<DetalleOficina>();

            var query = @"SELECT
                    Upper(po.descripcion) as localidad,
                    Upper(e.descripcion) as estatus,
                    a.id_cuenta as cuenta,
                    a.fecha as fecha,
                    p.razon_social as usuario,
                    Upper(t.descripcion) as tarifa,
                    IsNull(a.total,0) as importe_descontado,
                    IsNull(pp.cobrado,0) as importe_pago
                FROM Facturacion.Opr_DetDescuentos d with(nolock)
                INNER JOIN Facturacion.Opr_Abonos a with(nolock) on a.id_abono=d.id_abono
                INNER JOIN Padron.Cat_TiposUsuario t with(nolock) on t.id_tipousuario=a.id_tarifa
                INNER JOIN Padron.Cat_Padron p with(nolock) on p.id_padron=a.id_padron
                INNER JOIN Padron.Cat_Poblaciones po with(nolock) on po.id_poblacion=p.id_localidad
                INNER JOIN Global.Cat_Estatus e with(nolock) on e.id_estatus=p.id_estatus
                Outer Apply
                (
                    SELECT Sum(Isnull(cobrado,0)) as cobrado
                    FROM Facturacion.Opr_Abonos with(nolock)
                    WHERE id_padron=a.id_padron and id_tipomovto=6 and fecha_amd=a.fecha_amd and id_estatus!=31
                ) pp
                WHERE d.cve_descuento='RB10' And a.fecha_amd BetWeen @Fecha1 and @Fecha2
                ORDER BY fecha desc";

            using(var connection = new SqlConnection(enlace.GetConnectionString()))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Fecha1", dateRange.Desde_ISO);
                command.Parameters.AddWithValue("@Fecha2", dateRange.Hasta_ISO);
                using(var dataReader = command.ExecuteReader())
                {
                    while(dataReader.Read())
                    {
                        response.Add(DetalleOficina.FromDataReader(dataReader));
                    }
                }
                connection.Close();
            }
            return response;
        }

        public async Task<IEnumerable<TPVData>> GetTPV(IEnlace enlace, DateRange dateRange)
        {
            var responseList = new List<TPVData>();
            try
            {
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString()))
                {
                    await sqlConnection.OpenAsync();
                    var query = @"
                        ;With xTarjeta as
                        (
                            SELECT
                                a.cobrado as cobrado
                            FROM [Facturacion].[Opr_Abonos] a With(NoLock)
                            LEFT JOIN [ventanillas].[Cfg_ArchivosXLS] c ON a.id_caja = c.id_caja AND c.descripcion='LAYOUT BBVA'
                            WHERE
                                Convert(Varchar(8),a.fecha,112) BetWeen @cFec1 And @cFec2
                                And a.id_tipomovto = 6
                                And a.id_estatus != 31
                                And a.id_formapago=4
                                And c.id_caja IS NULL
                            UNION
                            SELECT
                                v.cobrado as cobrado
                            FROM Ventanillas.Opr_Ventas v With(NoLock)
                            LEFT JOIN [ventanillas].[Cfg_ArchivosXLS] c ON v.cve_caja = c.id_caja AND c.descripcion='LAYOUT BBVA'
                                WHERE
                                Convert(Varchar(8),v.fecha,112) BetWeen @cFec1 And @cFec2
                                And v.id_estatus != 44
                                And c.id_caja IS NULL
                                And v.id_formapago=4
                        )
                        SELECT COUNT(cobrado) as recibos, SUM(cobrado) as cobrado FROM xTarjeta";

                    var command = new SqlCommand(query, sqlConnection);
                    using(SqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        while(await dataReader.ReadAsync())
                        {
                            responseList.Add(TPVData.FromDataReader(enlace, dataReader));
                            // TODO: Implemented cast
                        }
                    }
                    sqlConnection.Close();
                }
                return responseList;
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Fail at attempt to get ehe TPV data from {enlace}: {message}", enlace.Nombre, err.Message);
                return Array.Empty<TPVData>();
            }
        }

    }
}
