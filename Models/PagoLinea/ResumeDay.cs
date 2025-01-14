using System;
using System.Data;
using System.Collections.Generic;

namespace Sicem_Blazor.Models.PagoLinea
{
    public class ResumeDay
    {

        public string Day {get;set;} = "";
        public decimal TotalIncome {get;set;} = 0m;
        public int TotalMovements {get;set;} = 0;
    }
}