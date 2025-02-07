using System;
using System.Data;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.PonteAlCorriente.Models;

public class TPVData
{
    public IEnlace Enlace {get;set;}
    public int Id {get => Enlace.Id;}
    public string Oficina {get => Enlace.Nombre;}
    public int Recibos {get;set;}
    public decimal Cobrado {get;set;}

    public TPVData(IEnlace enlace)
    {
        this.Enlace = enlace;
    }

    public static TPVData FromDataReader(IEnlace enlace, IDataReader reader)
    {
        var item = new TPVData(enlace);
        item.Recibos = Convert.ToInt32(reader["recibos"]);
        item.Cobrado = Convert.ToDecimal(reader["cobrado"]);
        return item;
    }
}
