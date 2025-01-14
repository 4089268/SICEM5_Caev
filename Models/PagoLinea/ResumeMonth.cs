using System;
using System.Data;
using System.Collections.Generic;

namespace Sicem_Blazor.Models.PagoLinea
{
    public class ResumeMonth
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalIncomePrev { get; set; }

        public int TotalMovements {get;set;}
        public int TotalMovementsPrev {get;set;}

        public ICollection<ResumeOffice> Offices {get;set;}

        public ResumeMonth()
        {
            this.TotalIncome = 0m;
            this.TotalIncomePrev = 0m;
            this.TotalMovements = 0;
            this.TotalMovementsPrev = 0;
            Offices = [];
        }

    }

}