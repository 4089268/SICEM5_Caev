using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sicem_Blazor.ModsUbitoma.Models {
    public class ConsultaRequest {
        public int Geolocalizado {get;set;}
        public int ConObservaciones {get;set;}
        public int ModEstatus {get;set;}
        public int ModSituacionTomas {get;set;}
        public int ModTarifas {get;set;}
        public int ModGiros {get;set;}
        public int ModAnomaliaPredio {get;set;}

        public DateTime FechaDesde;
        public DateTime FechaHasta;

        public ConsultaRequest(){
            this.Geolocalizado = 0;
            this.ConObservaciones = 0;
            this.ModEstatus = 0;
            this.ModSituacionTomas = 0;
            this.ModTarifas = 0;
            this.ModGiros = 0;
            this.ModAnomaliaPredio = 0;
        }

    }
}