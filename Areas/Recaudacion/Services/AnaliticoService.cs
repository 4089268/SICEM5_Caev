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


    public ICollection<AnaliticoResumenAno> ObtenerAnaliticoOficina(IEnlace enlace, int year, int years = 2)
    {
        var responseList = new List<AnaliticoResumenAno>();
        try
        {
            using(var connection = new SqlConnection(enlace.GetConnectionString()))
            {
                connection.Open();
                var query = @"[Sicem].[Recaudacion_Global]";
                var command = new SqlCommand(query, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@nAno", year);
                command.Parameters.AddWithValue("@nAnos", years);
                using(var dataReader = command.ExecuteReader())
                {
                    while(dataReader.Read())
                    {
                        responseList.Add(AnaliticoResumenAno.FromDataReader(enlace, dataReader));
                    }
                }
                connection.Close();
            }
        }
        catch(Exception err)
        {
            logger.LogError(err, "Error al obtener resumen analitico enlace:{Enlace}", enlace.Nombre);
            return null;
        }
        return responseList;
    }
}