using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class ModsOficina
    {
        public ModsOficina()
        {
            DetModsOficinas = new HashSet<DetModsOficina>();
        }

        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdOficina { get; set; }
        public string Tabla { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }

        public virtual Ruta IdOficinaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<DetModsOficina> DetModsOficinas { get; set; }
    }
}
