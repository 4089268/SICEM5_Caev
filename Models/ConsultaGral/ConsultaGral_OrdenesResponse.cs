using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Models{
    public class ConsultaGral_Ordenesresponse{
        public string  Id_Orden {get;set;}
        public DateTime? Fecha_Genero {get;set;}
        public bool Ejecutada {get;set;}
        public int Tipo_Orden {get;set;} //Rezago o Micromedicion
        public string Tipo_OrdenDesc {get;set;} //Rezago o Micromedicion


        /* Datos de la Orden */
        public string Estatus {get;set;}
        public string Trabajo {get;set;}
        public string Genero {get;set;}
        public string Imprimio {get;set;}
        public DateTime? Fecha_Imprimio {get;set;}
        public string Observaciones_Orden {get;set;}

        
        /* Resultados de la Orden */
        public DateTime? Fecha_Ini {get;set;}
        public DateTime? Fecha_Fin {get;set;}
        public string Realizo {get;set;} = "";
        public string Capturo {get;set;} = "";
        public DateTime? Fecha_Capturo {get;set;}
        public int Duracion {get;set;}
        // public string Ord_Ejecutada {get;set;}
        public string Respuesta_Orden {get;set;}
        

        /* Datos Medidor */
        public string Medidor_Reg{get;set;}
        public string Marca_Reg{get;set;}
        public string Modelo_Reg{get;set;}
        public string Diametro_Reg{get;set;}
        
        public string Medidor_Enc{get;set;}
        public string Marca_Enc{get;set;}
        public string Modelo_Enc{get;set;}
        public string Diametro_Enc{get;set;}

        public string Medidor_Ret{get;set;}
        public string Marca_Ret{get;set;}
        public string Modelo_Ret{get;set;}
        public string Diametro_Ret{get;set;}

        public string Medidor_Inst{get;set;}
        public string Marca_Inst{get;set;}
        public string Modelo_Inst{get;set;}
        public string Diametro_Inst{get;set;}

        public string Anomalia_Reg{get;set;}
        public int Lectura_Reg{get;set;}

        public string Anomalia_Act{get;set;}
        public int Lectura_Act{get;set;}
        public bool ErrorLectura{get;set;}


        /* Datos Inspeccion */
        public double Lecutar_Ini{get;set;} = 0;
        public double Lecutar_Fin{get;set;} = 0;
        public int Consumo_Estimado{get;set;} = 0;
        public double PruebaMed_A{get;set;} = 0;
        public double PruebaMed_B{get;set;} = 0;
        public int Sanit_FV{get;set;} = 0;
        public int Sanit_FS{get;set;} = 0;
        public int Sanit_HNA{get;set;} = 0;
        public int Sanit_Nor{get;set;} = 0;
        public int Llav_Coc{get;set;} = 0;
        public int Llav_Arc{get;set;} = 0;
        public int Llav_Ban{get;set;} = 0;
        public int Llav_Tom{get;set;} = 0;
        public int Llav_Tinacos{get;set;} = 0;
        public int Llav_Jard{get;set;} = 0;
        public int Llav_Lavado{get;set;} = 0;
        public int Llav_Pilas{get;set;} = 0;
        public int Llav_Cisterna{get;set;} = 0;
        public int Llav_Otros{get;set;} = 0;

        public int Person_Adultos{get;set;} = 0;
        public int Person_Menore{get;set;} = 0;
        public int Promedio{get;set;} = 0;
        public string Tarifa {get;set;} = "";

    }

}