using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class OprSesione
    {
        public Guid Id { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Caducidad { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
    }
}
