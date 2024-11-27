using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Recaudacion.Models {
    public class Recaudacion_Analitico {

        public Recaudacion_AnaliticoMensual[] Analitico_Mensual { get; set; }
        public Recaudacion_AnaliticoQuincenal[] Analitico_Quincenal { get; set; }
        public Recaudacion_AnaliticoSemanal[] Analitico_Semanal { get; set; }

        public Recaudacion_AnaliticoMensual[] AnaliticoRez_Mensual { get; set; }
        public Recaudacion_AnaliticoQuincenal[] AnaliticoRez_Quincenal { get; set; }
        public Recaudacion_AnaliticoSemanal[] AnaliticoRez_Semanal { get; set; }

    }
    public class Recaudacion_AnaliticoMensual {
        public string Oficina { get; set; }
        public int Año { get; set; }
        public decimal Ene { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Abr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Ago { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dic { get; set; }
        public decimal Total { get; set; }

    }
    public class Recaudacion_AnaliticoQuincenal {
        public string Oficina { get; set; }
        public int Año { get; set; }
        public decimal Ene_1 { get; set; }
        public decimal Ene_2 { get; set; }
        public decimal Feb_1 { get; set; }
        public decimal Feb_2 { get; set; }
        public decimal Mar_1 { get; set; }
        public decimal Mar_2 { get; set; }
        public decimal Abr_1 { get; set; }
        public decimal Abr_2 { get; set; }
        public decimal May_1 { get; set; }
        public decimal May_2 { get; set; }
        public decimal Jun_1 { get; set; }
        public decimal Jun_2 { get; set; }
        public decimal Jul_1 { get; set; }
        public decimal Jul_2 { get; set; }
        public decimal Ago_1 { get; set; }
        public decimal Ago_2 { get; set; }
        public decimal Sep_1 { get; set; }
        public decimal Sep_2 { get; set; }
        public decimal Oct_1 { get; set; }
        public decimal Oct_2 { get; set; }
        public decimal Nov_1 { get; set; }
        public decimal Nov_2 { get; set; }
        public decimal Dic_1 { get; set; }
        public decimal Dic_2 { get; set; }
        public decimal Total { get; set; }

    }
    public class Recaudacion_AnaliticoSemanal {
        public string Oficina { get; set; }
        public int Año { get; set; }
        public decimal Ene_1 { get; set; }
        public decimal Ene_2 { get; set; }
        public decimal Ene_3 { get; set; }
        public decimal Ene_4 { get; set; }
        public decimal Feb_1 { get; set; }
        public decimal Feb_2 { get; set; }
        public decimal Feb_3 { get; set; }
        public decimal Feb_4 { get; set; }
        public decimal Mar_1 { get; set; }
        public decimal Mar_2 { get; set; }
        public decimal Mar_3 { get; set; }
        public decimal Mar_4 { get; set; }
        public decimal Abr_1 { get; set; }
        public decimal Abr_2 { get; set; }
        public decimal Abr_3 { get; set; }
        public decimal Abr_4 { get; set; }
        public decimal May_1 { get; set; }
        public decimal May_2 { get; set; }
        public decimal May_3 { get; set; }
        public decimal May_4 { get; set; }
        public decimal Jun_1 { get; set; }
        public decimal Jun_2 { get; set; }
        public decimal Jun_3 { get; set; }
        public decimal Jun_4 { get; set; }
        public decimal Jul_1 { get; set; }
        public decimal Jul_2 { get; set; }
        public decimal Jul_3 { get; set; }
        public decimal Jul_4 { get; set; }
        public decimal Ago_1 { get; set; }
        public decimal Ago_2 { get; set; }
        public decimal Ago_3 { get; set; }
        public decimal Ago_4 { get; set; }
        public decimal Sep_1 { get; set; }
        public decimal Sep_2 { get; set; }
        public decimal Sep_3 { get; set; }
        public decimal Sep_4 { get; set; }
        public decimal Oct_1 { get; set; }
        public decimal Oct_2 { get; set; }
        public decimal Oct_3 { get; set; }
        public decimal Oct_4 { get; set; }
        public decimal Nov_1 { get; set; }
        public decimal Nov_2 { get; set; }
        public decimal Nov_3 { get; set; }
        public decimal Nov_4 { get; set; }
        public decimal Dic_1 { get; set; }
        public decimal Dic_2 { get; set; }
        public decimal Dic_3 { get; set; }
        public decimal Dic_4 { get; set; }
        public decimal Total { get; set; }

    }
}
