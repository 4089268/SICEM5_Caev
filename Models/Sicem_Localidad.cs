using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Models {
    public class Sicem_Localidad {
        public int Id_Poblacion { get; set; }
        public string Descripcion { get; set; }
        public int Id_sucursal { get; set; }
        public int Sb { get; set; }
        public int Sector { get; set; }
        public int Inactivo { get; set; }

    }
}
