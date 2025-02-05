using System;
using System.Data;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.PonteAlCorriente.Models;

public class ResumeOffice
{
    public IEnlace Enlace {get;set;}
    public int Id {get => Enlace.Id;}
    public string Oficina {get => Enlace.Nombre;}
    public ResumenOficinaEstatus Estatus {get;set;}
    public decimal ImporteDescontado {get;set;}
    public decimal ImporteCobrado {get;set;}
    public int NumDescuentos {get;set;}

    public ResumeOffice(IEnlace enlace)
    {
        this.Enlace = enlace;
        Estatus = ResumenOficinaEstatus.Pendiente;
    }

    public static ResumeOffice FromDataReader(IEnlace enlace, IDataReader reader)
    {
        var item = new ResumeOffice(enlace);
        item.Estatus = ResumenOficinaEstatus.Completado;
        item.ImporteDescontado = Convert.ToDecimal(reader["imp_descontado"]);
        item.ImporteCobrado = Convert.ToDecimal(reader["imp_cobrado"]);
        item.NumDescuentos = Convert.ToInt32(reader["num_desctos"]);
        return item;
    }
}
