using System;
using System.Data;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.PonteAlCorriente.Models;

public class DetalleOficina
{
    public string Localidad {get;set;}
    public string Estatus {get;set;}
    public string Cuenta {get;set;}
    public string Fecha {get;set;}
    public string Usuario {get;set;}
    public string Tarifa {get;set;}
    public decimal ImporteDescontado {get;set;}
    public decimal ImportePagado {get;set;}

    public static DetalleOficina FromDataReader(IDataReader reader)
    {
        var item = new DetalleOficina();
        item.Localidad = reader["localidad"].ToString();
        item.Estatus = reader["estatus"].ToString();
        item.Cuenta = reader["cuenta"].ToString();
        item.Fecha = reader["fecha"].ToString();
        item.Usuario = reader["usuario"].ToString();
        item.Tarifa = reader["tarifa"].ToString();
        item.ImporteDescontado = Convert.ToDecimal(reader["importe_descontado"]);
        item.ImportePagado = Convert.ToDecimal(reader["importe_pago"]);
        return item;
    }
}
