using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sicem_Blazor.Models
{
    public partial class CatInpc
    {
        public decimal Af { get; set; }
        public decimal Mf { get; set; }
        public decimal? Inpc { get; set; }
        public decimal? Factor { get; set; }
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
