using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Micromedicion.Models {
    public class Micromedicion_Padron {
        public string Estatus { get; set; }
        public long Contrato { get; set; }
        public string Usuario { get; set; }
        public int Sb { get; set; }
        public int Sector { get; set; }
        public string Localidad { get; set; }
        public string Colonia { get; set; }
        public string Tipo_usuario { get; set; }
        public string Servicio { get; set; }
        public string Medidor { get; set; }
        public string Diametro { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public DateTime Fecha_inst { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }
        public string Trabajo { get; set; }
        public string Fecha_realizo { get; set; }
        public string Funcionando {get;set;}
    }
}
