using System;
namespace Sicem_Blazor.Tarifas.Models{
    public class SimulacionParams{
        public int Anio {get;set;}
        public int Mes {get;set;}
        public int Subsistema {get;set;}
        public int Sector {get;set;}
        public decimal FactorDren {get;set;}
        public decimal FactorSane {get;set;}

        public SimulacionParams(){
            Anio = DateTime.Now.Year;
            Mes = DateTime.Now.Month;
            Subsistema = 0;
            Sector = 0;
            FactorDren = 0.35m;
            FactorSane = 0.05m;
        }
    }
}