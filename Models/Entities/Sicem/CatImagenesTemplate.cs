using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CatImagenesTemplate
    {
        public CatImagenesTemplate()
        {
            CatMessagesTemplates = new HashSet<CatMessagesTemplate>();
        }

        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Path { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<CatMessagesTemplate> CatMessagesTemplates { get; set; }
    }
}
