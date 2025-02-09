using System;
using System.Data;
using System.Xml.Linq;
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
    public string Autorizacion {get;set;}
    public string Estado {get;set;}
    public string Email {get;set;}
    public string Telefono {get;set;}
    public string Plataforma {get;set;}
    public string Localidad {get;set;}
    public int Cuenta {get;set;}
    public string Tarifa {get;set;}
    public decimal Aplicado {get;set;}
    public decimal Diferencia {get;set;}


    public AplicarPagoResult aplicarPagoResult {get;set;}

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
            Autorizacion = reader["autorizacion"].ToString(),
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

    public string ToXml()
    {
        string formattedFecha = Fecha.ToString("yyyy-MM-ddTHH:mm:sszzz");

        XElement xml = new XElement("FILTROS",
            new XElement("PAGOWEB",
                new XElement("id_caja", "E0005"),
                new XElement("id_operador", "CAEV"),
                new XElement("id_transaccion", Id.ToString().Trim()),
                new XElement("fecha", formattedFecha),
                new XElement("referencia_comercio", Referencia?.Trim()),
                new XElement("medio_de_pago", MedioPago?.Trim()),
                new XElement("referencia_tarjeta", ReferenciaTarjeta?.Trim()),
                new XElement("tipo_tarjeta", TipoTarjeta?.Trim()),
                new XElement("autorizacion", Autorizacion?.Trim()),
                new XElement("total_cobrado", Importe.ToString("F2")),
                new XElement("estado", Estado?.Trim()),
                new XElement("email", Email?.Trim()),
                new XElement("telefono", Telefono?.Trim()),
                new XElement("plataforma", Plataforma?.Trim())
            )
        );

        return xml.ToString();
    }

}
