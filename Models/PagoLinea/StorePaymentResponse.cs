using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models.PagoLinea
{
    public class StorePaymentResponse
    {
        public string Title { get; set; }
        public IEnumerable<long> Data { get; set; }
    }
}
