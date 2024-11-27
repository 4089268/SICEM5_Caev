using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Models{
    public class ConsultaGral_Aniticipos_v2 {
        public List<AnticipoItem> Anticipos {get;set;}
        public List<Anticipo_Concepto> Conceptos { get; set; }
        public List<AnticipoAplicadoItem> Anticipos_Aplicados {get;set;}        
    }


    public class AnticipoItem {
        public string Id_Abono { get; set; }
        public DateTime Fecha { get; set; }
        public int Id_Estatus { get; set; }
        public string Estatus { get; set; }
        public string Sucursal { get; set; } = "";
        public string Autorizo { get; set; } = "";

        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public int Mf { get; set; }
        public int Af { get; set; }

        public string Ciclo { get; set; }
        public int Promedio { get; set; }
        public string Observa_a { get; set; }
        public string Observa_c { get; set; }
        public string Tarifa { get; set; }
        public string Servicio { get; set; }
        public string Situacion { get; set; }
        public string Anomalia { get; set; }
        public string Calculo { get; set; }
        public string Tarifa_Fija { get; set; }
        public string Movimiento { get; set; }
        public string Cancelo { get; set; }

        public DateTime Fecha_Cancelacion { get; set; }
        public string Id_Caja { get; set; }
        public string Id_Operacion { get; set; }
        public decimal Cobrado { get; set; }
        public string Tipo_Pago { get; set; }
        public string Cajero { get; set; }
        public int Lectura_Actual { get; set; }
        public int Consumo_Actual {get;set;}
        public int Meses_Adeudo { get; set; }
        public int M3_Aplicados { get; set; }
        public int Recno { get; set; }

    }
    public class AnticipoAplicadoItem{
        public long Id_Anticipo {get;set;}
        public DateTime Fecha {get;set;}
        public string Aplico {get;set;}
        public double Importe {get;set;}
        public int MetrosAplicados {get;set;}
        public string Estatus {get;set;}
        public string Tarifa {get;set;}
        public string Servicio {get;set;}
        public string Anomalia {get;set;}
        public string Calculo {get;set;}
        public string Tarifa_Fija {get;set;}
        public int MF {get;set;}
        public int AF {get;set;}
        public int Lectura {get;set;}
        public int Consumo {get;set;}
        public int Promedio {get;set;}
        public int MesesAdeudo {get;set;}

    }
    public class Anticipo_Concepto{
        public string Id_Abono { get; set; }
        public bool Rezago { get; set; }
        public int Id_Concepto {get;set;}
        public string Concepto {get;set;}
        public double Sub_Total {get;set;}
        public double IVA {get;set;}
        public double Total {get;set;}
    }

}