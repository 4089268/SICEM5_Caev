using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Recaudacion.Models;

namespace Sicem_Blazor.Recaudacion.Services;

public class AnaliticoService
{
    private readonly ILogger<AnaliticoService> logger;

    public AnaliticoService(ILogger<AnaliticoService> logger)
    {
        this.logger = logger;
    }


    public AnaliticoResumen ObtenerAnaliticoOficina(IEnlace enlace, int year)
    {
        var analiticoResumen = new AnaliticoResumen(enlace)
        {
            Estatus = ResumenOficinaEstatus.Error
        };

        var from = new DateTime(year - 2, 1,1);
        var to = new DateTime(year, 12,31);

        // * get the data from the office
        var listResponse = new List<AnaliticoResumenResponse>();
        try
        {
            using(var connection = new SqlConnection(enlace.GetConnectionString()))
            {
                connection.Open();
                var query = @"
                    ;With xTarjeta as
                    (
                        SELECT
                            YEAR(fecha) as ano,
                            MONTH(fecha) as mes,
                            SUM(a.cobrado) as cobrado
                        FROM [Facturacion].[Opr_Abonos] a With(NoLock)
                        WHERE
                        (convert(Varchar(8),a.fecha,112) BetWeen @cFec1 AND @cFec2) AND a.id_tipomovto = 6 AND a.id_estatus != 31
                        Group By year(fecha),MONTH(fecha)
                        UNION
                        SELECT
                            YEAR(fecha) as ano,
                            MONTH(fecha) as mes,
                            SUM(v.cobrado) as cobrado
                        FROM [Ventanillas].[Opr_Ventas] v With(NoLock)
                        WHERE
                        (Convert(Varchar(8),v.fecha,112) BetWeen @cFec1 And @cFec2) And v.id_estatus != 44
                        Group By YEAR(fecha), MONTH(fecha)
                    )
                    SELECT ano, mes, SUM(cobrado) as cobrado
                    FROM xTarjeta
                    GROUP BY ano, mes
                    ORDER BY ano desc, mes desc";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@cFec1", from.ToString("yyyyMMdd"));
                command.Parameters.AddWithValue("@cFec2", to.ToString("yyyyMMdd"));
                using(var dataReader = command.ExecuteReader())
                {
                    while(dataReader.Read())
                    {
                        listResponse.Add( new AnaliticoResumenResponse
                            {
                                Enlace = enlace,
                                Ano = ConvertUtils.ParseInteger(dataReader["ano"]),
                                Mes = ConvertUtils.ParseInteger(dataReader["mes"]),
                                Cobrado = ConvertUtils.ParseDecimal(dataReader["cobrado"])
                            }
                        );
                    }
                }
                connection.Close();
            }
        }
        catch(Exception err)
        {
            logger.LogError(err, "Error al obtener resumen analitico enlace:{Enlace}", enlace.Nombre);
            return analiticoResumen;
        }
        
        // * process the response
        var analiticoResumenAnoList = new List<AnaliticoResumenAno>();
        foreach(var group in listResponse.GroupBy(item => item.Ano))
        {
            var analiticoResumenAno = new AnaliticoResumenAno(enlace)
            {
                Ano = group.Key
            };
            foreach(var m in group)
            {
                analiticoResumenAno.Meses[m.Mes - 1] = m.Cobrado;
            }
            analiticoResumenAnoList.Add(analiticoResumenAno);
        }
        // * fill the response with the data
        analiticoResumen.Anos = analiticoResumenAnoList;
        analiticoResumen.Estatus = ResumenOficinaEstatus.Completado;

        return analiticoResumen;
    }
}