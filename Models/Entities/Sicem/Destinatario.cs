using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class Destinatario
    {
        public Guid Id { get; set; }
        public Guid BoletinId { get; set; }
        public string Titulo { get; set; }
        public int Telefono { get; set; }
        public string Lada { get; set; }
        public bool? Error { get; set; }
        public string Resultado { get; set; }
        public string EnvioMetadata { get; set; }
        public DateTime? FechaEnvio { get; set; }

        public virtual OprBoletin Boletin { get; set; }
    }
}
