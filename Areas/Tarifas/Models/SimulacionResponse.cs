using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Tarifas.Models {
    public class SimulacionResponse {
        public IEnumerable<SimulacionResumen> DatosGlobal {get;set;}
        public IEnumerable<SimulacionResumen> DatosMedidos {get;set;}
        public IEnumerable<SimulacionResumen> DatosPromedios {get;set;}
        public IEnumerable<SimulacionResumen> DatosFijos {get;set;}


        public IEnumerable<SimulacionRangos> DatosRangos {get;set;}
        public IEnumerable<SimulacionRangos> DatosRangosDomestico {get;set;}
        public IEnumerable<SimulacionRangos> DatosRangosHotelero {get;set;}
        public IEnumerable<SimulacionRangos> DatosRangosComercial {get;set;}
        public IEnumerable<SimulacionRangos> DatosRangosIndustrial {get;set;}
        public IEnumerable<SimulacionRangos> DatosRangosServiciosGen {get;set;}
    }

    public class SimulacionRangos {
        public string Rango {get;set;}
        public decimal Importe {get;set;}
        public int Metros {get;set;}
        public int Usuarios {get;set;}
        public double Porcentaje {get;set;}
    }
}