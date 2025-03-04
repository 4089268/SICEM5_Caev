using System;
using System.Data;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.SeguimientoCobros.Models;

public class LiveIncome
{
    public IEnlace Enlace {get;set;}

    public int IdSucursal {get;set;}
    public string Sucursal {get;set;}
    public double Latitud {get;set;}
    public double Longitud {get;set;}
    public int Recibos {get;set;}
    public decimal Cobrado {get;set;}

    public int OficinaId {get => Enlace.Id; }
    public string Oficina {get => Enlace.Nombre;}

    public LiveIncome(IEnlace enlace)
    {
        this.Enlace = enlace;
    }

    public static LiveIncome FromDataReader(IEnlace enlace, IDataReader reader)
    {
        var item = new LiveIncome(enlace);
        item.IdSucursal = Convert.ToInt32(reader["id_sucursal"]);
        item.Sucursal = reader["sucursal"].ToString();
        item.Latitud = 0;
        item.Longitud = 0;
        item.Recibos = int.TryParse(reader["recibos"].ToString(), out int r) ?r :0;
        item.Cobrado = decimal.TryParse(reader["cobrado"].ToString(), out decimal c) ?c :0m;
        return item;
    }
}
