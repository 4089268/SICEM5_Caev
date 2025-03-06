using System;
using System.Collections.Generic;
using Sicem_Blazor.Boletines.Models;

namespace Sicem_Blazor.Models
{
    public partial class Destinatario : IBoletinDestinatario
    {
        public Guid Id { get; set; }
        public Guid BoletinId { get; set; }
        public string Titulo { get; set; }
        public long Telefono { get; set; }
        public string Lada { get; set; }
        public bool? Error { get; set; }
        public string Resultado { get; set; }
        public string EnvioMetadata { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string Correo { get; set; }

        public virtual OprBoletin Boletin { get; set; }


        // * Implemented interface
        bool IBoletinDestinatario.Error
        {
            get => Error ?? false;
            set => Error = value;
        }
    }
}
