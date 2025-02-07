using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data;
using Sicem_Blazor.SeguimientoCobros.Models;
using Sicem_Blazor.Models;
using Microsoft.EntityFrameworkCore;

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
            var offices = this.sicemContext.Rutas
                .Include(item => item.LocalizacionRuta)
                .Where(item => item.Inactivo != true && item.LocalizacionRuta != null && item.LocalizacionRuta.Any())
                .Select( item => new OfficePushpinMap(item.Id, item.Oficina)
                    {
                        Lat = item.LocalizacionRuta.First().Latitud.ToString(),
                        Lon = item.LocalizacionRuta.First().Longitud.ToString()
                    }
                )
                .ToList();
            return offices;
        }
    }
}