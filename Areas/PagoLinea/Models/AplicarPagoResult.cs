using System;
using Sicem_Blazor.Data.Contracts;
public class AplicarPagoResult
{

    public long Id {get; set;}
    public DateTime Fecha {get;set;}
    public decimal Importe {get;set;}
    public ResumenOficinaEstatus Estatus {get;set;}


    public string FolioPago { get; set; }

    public DateTime FechaInsert { get; set; }
    public string Operacion { get; set; }
    public int Error { get; set; }
    public string Mensaje { get; set; }

    public AplicarPagoResult()
    {
        Estatus = ResumenOficinaEstatus.Pendiente;
    }
}