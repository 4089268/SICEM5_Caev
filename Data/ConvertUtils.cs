using System;

namespace Sicem_Blazor.Data {
    public class ConvertUtils {
        public static decimal ParseDecimal(object val, decimal def = 0m) =>  decimal.TryParse(val.ToString(), out decimal tmpDec) ? tmpDec : def;
        public static int ParseInteger(object val, int def = 0) => int.TryParse(val.ToString(), out int tmpDec) ? tmpDec : def;
        public static double ParseDouble(object val, double def = 0) => double.TryParse(val.ToString(), out double tmpDec) ? tmpDec : def;
        public static DateTime? ParseDateTime(object val, DateTime? def = null) => DateTime.TryParse(val.ToString(), out DateTime tmpDec) ? tmpDec : def;

    }
}