using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicem_Blazor.Models {
    public class BusquedaCuentaModel {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Colonia { get; set; }
        public string Medidor { get; set; }
        public bool SoloActivos { get; set; }
        public BusquedaCuentaModel() {
            this.Nombre = "";
            this.Direccion = "";
            this.Colonia = "";
            this.Medidor = "";
            this.SoloActivos = false;
        }
    }
}
