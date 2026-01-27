using System;

namespace Sicem_Blazor.CFDI.Models;

public class ConceptoFactura
{
    public int IdCFDI {get; set;} = 0;
    public int IdConcepto {get; set;} = 0;
    public string Concepto {get; set;} = "";
    public decimal Subtotal {get; set;} = 0;
    public decimal Iva {get; set;} = 0m;
    public decimal Total {get; set;} = 0m;
    public string ClaveProdServ {get; set;} = "";
    public string ClaveUnidad {get; set;} = "";
    public string Cantidad {get; set;} = "";
}
