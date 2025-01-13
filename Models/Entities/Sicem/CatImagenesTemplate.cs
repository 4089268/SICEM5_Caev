using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CatImagenesTemplate
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Path { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
