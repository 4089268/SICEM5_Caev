using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Facturacion.Models{
    public class Facturacion_Usuarios {

        public long Id_Cuenta{ get; set; }
        public string Localizacion { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Estatus { get; set; }
        public bool Pago { get; set; }

    }

}
