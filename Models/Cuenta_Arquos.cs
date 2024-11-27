using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Models{
    public class Cuenta_Arquos{
        public long Id_Oficina { get; set; }
        public string Oficina { get; set; }
        public long Id_padron {get; set; }
        public long Id_cuenta { get; set; }
        public string Razon_social { get; set; }
        public string Nombre_comercial { get; set; }
        public string Direccion { get; set; }
        public string Servicio { get; set; }
        public string Colonia { get; set; }
        public string Poblacion { get; set; }
        public string Medidor { get; set; }
        public string Estatus { get; set; }
    }
}
