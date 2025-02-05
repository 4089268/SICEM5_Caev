using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data;
using Sicem_Blazor.SeguimientoCobros.Models;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.SeguimientoCobros.Data
{

    public class SeguimientoCobroService
    {

        private readonly ILogger<SeguimientoCobroService> logger;
        private readonly SicemContext sicemContext;

        public SeguimientoCobroService(ILogger<SeguimientoCobroService> l, SicemContext sc)
        {
            this.logger = l;
            this.sicemContext = sc;
        }

        public IEnumerable<OfficePushpinMap> GetOffices()
        {
            
            throw new NotImplementedException();
            
            // var results = new List<OfficePushpinMap>();
            // using(var sqlConnection = new SqlConnection(cadConexion))
            // {
            //     sqlConnection.Open();
            //     var command = new SqlCommand(StoreProceduresIncomeOffice.COBROS_VIVO, sqlConnection);
            //     command.CommandType = CommandType.StoredProcedure;
            //     using(SqlDataReader reader = command.ExecuteReader())
            //     {
            //         while(reader.Read())
            //         {
            //             var id = ConvertUtils.ParseInteger( reader["id_sucursal"].ToString() );
            //             var oficina = reader["sucursal"].ToString();
            //             results.Add( new OfficePushpinMap(id, oficina)
            //             {
            //                 Lat = reader["latitud"].ToString(),
            //                 Lon = reader["longitud"].ToString(),
            //                 Bills = ConvertUtils.ParseInteger( reader["recibos"].ToString() ),
            //                 Income = ConvertUtils.ParseDecimal( reader["cobrado"].ToString() ),
            //             });
            //         }
            //     }
            //     sqlConnection.Close();
            // }
            // return results;
        }
    }
}