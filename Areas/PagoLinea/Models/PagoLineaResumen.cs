using System;
using System.Data;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.PagoLinea.Models;

public class PagoLineaResumen
{
    public IEnlace Enlace {get;set;}
    public int Id {get => Enlace.Id;}
    public string Oficina {get => Enlace.Nombre;}
    public ResumenOficinaEstatus Estatus {get;set;}

    public int TotalPagos {get;set;}
    public decimal ImportePagos {get;set;}

    public int TotalPagosAplicados {get;set;}
    public decimal ImportePagosAplicados {get;set;}
    
    public int TotalPagosPorAplicar {get;set;}
    public decimal ImportePagosPorAplicar {get;set;}

    public PagoLineaResumen(IEnlace enlace)
    {
        this.Enlace = enlace;
    }

    public void LoadFromReader(IDataReader reader)
    {
        TotalPagos = Convert.ToInt32(reader["num_pagos"]);
        TotalPagosAplicados = Convert.ToInt32(reader["aplicados"]);
        TotalPagosPorAplicar = Convert.ToInt32(reader["dif_pagos"]);
        ImportePagos = Convert.ToDecimal(reader["imp_pagos"]);
        ImportePagosAplicados = Convert.ToDecimal(reader["imp_aplicados"]);
        ImportePagosPorAplicar = Convert.ToDecimal(reader["dif_imp_pagos"]);
        Estatus = ResumenOficinaEstatus.Completado;
    }
}
