using System;
using System.Data;
using System.Collections.Generic;

namespace Sicem_Blazor.Models.PagoLinea
{
    public class ResumeOffice {
        public int OfficeId {get;set;}
        public string OfficeName {get;set;}
        public decimal TotalIncome { get; set; }
        public decimal TotalIncomePrev { get; set; }
        public int TotalMovements {get;set;}
        public int TotalMovementsPrev {get;set;}

        public ResumeOffice()
        {
            OfficeId = 0;
            OfficeName = "";
            TotalIncome = 0m;
            TotalIncomePrev = 0m;
            TotalMovements = 0;
            TotalMovementsPrev = 0;
        }

    }
}