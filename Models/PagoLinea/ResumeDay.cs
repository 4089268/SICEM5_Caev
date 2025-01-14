using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;

namespace Sicem_Blazor.Models.PagoLinea
{
    public class ResumeDay
    {

        public string Day {get;set;} = "";
        public decimal TotalIncome {get;set;} = 0m;
        public int TotalMovements {get;set;} = 0;

        public string DayFormat
        {
            get {
                try
                {
                    return DateTime.TryParseExact(Day, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d)
                        ? d.ToString("d MMM").ToUpper()
                        : Day;
                }
                catch (System.Exception)
                {
                    return Day;
                }
            }
        }
        
    }
}