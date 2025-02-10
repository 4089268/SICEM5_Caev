using System;
using System.Data;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.PagoLinea.Models;

public class PagoLineaResumenDia
{
    public IEnlace Enlace {get;set;}
    public int Id {get => Enlace.Id;}
    public string Oficina {get => Enlace.Nombre;}

    public int FechaInt {get;set;}
    public string FechaString {get;set;}
    public int TotalPagos {get;set;}
    public decimal ImportePagos {get;set;}

    public int TotalPagosAplicados {get;set;}
    public decimal ImportePagosAplicados {get;set;}
    
    public int TotalPagosPorAplicar {get;set;}
    public decimal ImportePagosPorAplicar {get;set;}

    public DateTime Fecha
    {
        get {
            return DateTime.ParseExact(FechaInt.ToString(), "yyyyMMdd", null);
        }
    }

    public PagoLineaResumenDia(IEnlace enlace)
    {
        this.Enlace = enlace;
    }

    public void LoadFromReader(IDataReader reader)
    {
        FechaInt = ConvertUtils.ParseInteger(reader["fecha"]);
        FechaString = reader["fec"].ToString();
        TotalPagos = ConvertUtils.ParseInteger(reader["num_pagos"]);
        TotalPagosAplicados = ConvertUtils.ParseInteger(reader["aplicados"]);
        TotalPagosPorAplicar = ConvertUtils.ParseInteger(reader["dif_pagos"]);
        ImportePagos = ConvertUtils.ParseDecimal(reader["imp_pagos"]);
        ImportePagosAplicados = ConvertUtils.ParseDecimal(reader["imp_aplicados"]);
        ImportePagosPorAplicar = ConvertUtils.ParseDecimal(reader["dif_imp_pagos"]);
    }
}
