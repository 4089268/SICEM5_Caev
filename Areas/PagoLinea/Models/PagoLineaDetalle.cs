using System;
using System.Data;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.PagoLinea.Models;

public class PagoLineaDetalle
{
    public IEnlace Enlace {get;set;}
    public long Id {get; set;}
    public DateTime Fecha {get;set;}
    public decimal Importe {get;set;}
    public string MedioPago {get;set;}
    public string Referencia {get;set;}
    public string ReferenciaTarjeta {get;set;}
    public string TipoTarjeta {get;set;}
    public string Autorizacinon {get;set;}
    public string Estado {get;set;}
    public string Email {get;set;}
    public string Telefono {get;set;}
    public string Plataforma {get;set;}
    public string Localidad {get;set;}
    public int Cuenta {get;set;}
    public string Tarifa {get;set;}
    public decimal Aplicado {get;set;}
    public decimal Diferencia {get;set;}

    public PagoLineaDetalle(IEnlace enlace)
    {
        this.Enlace = enlace;
    }

    public static PagoLineaDetalle FromDataReader(IEnlace enlace, IDataReader reader)
    {
        var newItem = new PagoLineaDetalle(enlace)
        {
            Id = Convert.ToInt32(reader["id"]),
            Fecha = Convert.ToDateTime(reader["fecha"]),
            Importe = Convert.ToDecimal(reader["importe"]),
            MedioPago = reader["medio_pago"].ToString(),
            Referencia = reader["referencia"].ToString(),
            ReferenciaTarjeta = reader["referencia_tarjeta"].ToString(),
            TipoTarjeta = reader["tipo_tarjeta"].ToString(),
            Autorizacinon = reader["autorizacion"].ToString(),
            Estado = reader["estado"].ToString(),
            Email = reader["email"].ToString(),
            Telefono = reader["telefono"].ToString(),
            Plataforma = reader["plataforma"].ToString(),
            Localidad = reader["localidad"].ToString(),
            Cuenta = Convert.ToInt32(reader["cuenta"]),
            Tarifa = reader["tarifa"].ToString(),
            Aplicado = Convert.ToDecimal(reader["aplicado"]),
            Diferencia = Convert.ToDecimal(reader["dif"])
        };
        return newItem;
    }
}
