using System;
using System.Collections.Generic;
using Sicem_Blazor.Boletines.Models;

namespace Sicem_Blazor.Models
{
    public partial class OprBoletin : IBoletin
    {
        public OprBoletin()
        {
            BoletinMensajes = new HashSet<BoletinMensaje>();
            Destinatarios = new HashSet<Destinatario>();
        }

        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime FinishedAt { get; set; }

        public virtual ICollection<BoletinMensaje> BoletinMensajes { get; set; }
        public virtual ICollection<Destinatario> Destinatarios { get; set; }
        
    }
}
