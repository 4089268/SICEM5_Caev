using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sicem_Blazor.Models
{
    public partial class CatCfe
    {
        public decimal Af { get; set; }
        public decimal Mf { get; set; }
        public decimal? CostoKw { get; set; }
        public decimal? FiKw { get; set; }
        public decimal? PorcInc { get; set; }
        public decimal? PorcApli { get; set; }
        public int Id { get; set; }

        [NotMapped]
        public string MesLetras {
            get {
                var _dateTime = new DateTime((int)Af, (int) Mf, 1);
                return _dateTime.ToString("MMMM").ToString().ToUpper();
            }
        }
    }
}
