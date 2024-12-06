using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Models
{
    public partial class CatOpcione : IOpcionSistema
    {
        public decimal? IdOpcion { get; set; }
        public string Descripcion { get; set; }

        [NotMapped]
        public int Id {
            get {
                return (int)(IdOpcion??0m);
            }
            set{
                IdOpcion = (decimal)value;
            }
        } 
    }
}
