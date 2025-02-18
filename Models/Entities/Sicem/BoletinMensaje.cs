using System;
using System.Collections.Generic;
using Sicem_Blazor.Boletines.Models;

namespace Sicem_Blazor.Models
{
    public partial class BoletinMensaje : IBoletinMensaje
    {
        public Guid Id { get; set; }
        public Guid BoletinId { get; set; }
        public bool? EsArchivo { get; set; }
        public string Mensaje { get; set; }
        public string MimmeType { get; set; }
        public int? FileSize { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual OprBoletin Boletin { get; set; }

        // * Implemented interface
        bool IBoletinMensaje.EsArchivo
        {
            get => EsArchivo ?? false;
            set => EsArchivo = value;
        }
        int IBoletinMensaje.FileSize
        {
            get => FileSize ?? 0;
            set => FileSize = value;
        }
    }
}
