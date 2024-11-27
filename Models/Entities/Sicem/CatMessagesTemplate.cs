using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CatMessagesTemplate
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaModificacion { get; set; }
    }
}
