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
                            Count(a.id) as num_desctos,
                            Sum(Isnull(a.total,0)) as imp_descontado,
                            Sum(Isnull(p.cobrado,0)) as imp_cobrado
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

    }
}
