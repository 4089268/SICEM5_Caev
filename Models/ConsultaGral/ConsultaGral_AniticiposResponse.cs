using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Models{
    public class ConsultaGral_AniticiposResponse{
        public List<AnticipoItem> Anticipos {get;set;}
        public List<AnticipoAplicadoItem> Anticipos_Aplicados {get;set;}        
    }


    public class AnticipoItem_Ant{
        public long Id_Anticipo {get;set;} = 0;
        public long Id_Padron {get;set;} = 0;
        public long Id_TipoAnticipo {get;set;} = 0;
        public bool Cancelado {get;set;} = false;
        public DateTime Fecha{get;set;}
        public string Periodo {get;set;} = "";
        public string Tipo {get;set;} = "";
        public double ImportexAplicar {get;set;} = 0;
        public double Metros {get;set;} = 0;
        public int Meses {get;set;} = 0;
        public double Importe_Cobrado {get;set;} = 0;
        public double Importe_Aplicado {get;set;} = 0;
        public double Importe_Cancelado {get;set;} = 0;
        public int Metros_Cobrados {get;set;} = 0;
        public int Metros_Aplicados {get;set;} = 0;
        public int Metros_Cancelados {get;set;} = 0;
        public int Meses_Cobrados {get;set;} = 0;
        public int Meses_Aplicados {get;set;} = 0;
        public int Meses_Cancelados {get;set;} = 0;
        public string  Genero {get;set;} = "";
        public string  Sucursal {get;set;} = "";
        public DateTime Fecha_Genero {get;set;}
        public string  Cancelo {get;set;} = "";
        public DateTime Fecha_Cancelo {get;set;}
        public string Descripcion_Genero {get;set;} ="";
        public string Descripcion_Cancelo {get;set;} ="";
        public string Folio_Venta {get;set;} ="";

    }
    public class AnticipoAplicadoItem_ant{
        public long Id_Anticipo {get;set;}
        public string Folio {get;set;}
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

    public class AnticipoConceptoItem_ant{
        public int Id_Concepto {get;set;}
        public string Concepto {get;set;}
        public double Sub_Total {get;set;}
        public double IVA {get;set;}
        public double Total {get;set;}
    }

}