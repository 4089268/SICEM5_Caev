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
        TotalPagos = ConvertUtils.ParseInteger(reader["num_pagos"]);
        TotalPagosAplicados = ConvertUtils.ParseInteger(reader["aplicados"]);
        TotalPagosPorAplicar = ConvertUtils.ParseInteger(reader["dif_pagos"]);
        ImportePagos = ConvertUtils.ParseDecimal(reader["imp_pagos"]);
        ImportePagosAplicados = ConvertUtils.ParseDecimal(reader["imp_aplicados"]);
        ImportePagosPorAplicar = ConvertUtils.ParseDecimal(reader["dif_imp_pagos"]);
        Estatus = ResumenOficinaEstatus.Completado;
    }
}
