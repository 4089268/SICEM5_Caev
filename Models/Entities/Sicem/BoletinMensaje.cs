﻿using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class BoletinMensaje
    {
        public Guid Id { get; set; }
        public Guid BoletinId { get; set; }
        public bool? EsArchivo { get; set; }
        public string Mensaje { get; set; }
        public string MimmeType { get; set; }
        public int? FileSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        public virtual OprBoletin Boletin { get; set; }
    }
}
