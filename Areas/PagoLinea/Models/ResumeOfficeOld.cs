using System;
using System.Data;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.PagoLinea.Models;

public class ResumeOfficeOld
{
    public IEnlace Enlace {get;set;}
    public int Id {get => Enlace.Id;}
    public string Oficina {get => Enlace.Nombre;}
    public ResumenOficinaEstatus Estatus {get;set;}
    public decimal ImportePropio {get;set;}
    public int UsuariosPropios {get;set;}
    public decimal ImporteOtros {get;set;}
    public int UsuariosOtros {get;set;}
    public decimal Importe {get;set;}
    public decimal Cobrado {get;set;}
    public int Usuarios {get;set;}

    public ResumeOfficeOld(IEnlace enlace)
    {
        this.Enlace = enlace;
    }

    public void LoadFromReader(IDataReader reader)
    {
        ImportePropio = Convert.ToDecimal(reader["importe_propios"]);
        UsuariosPropios = Convert.ToInt32(reader["usuarios_propios"]);
        ImporteOtros = Convert.ToDecimal(reader["importe_otros"]);
        UsuariosOtros = Convert.ToInt32(reader["usuarios_otros"]);
        Importe = Convert.ToDecimal(reader["importe"]);
        Cobrado = Convert.ToDecimal(reader["cobrado"]);
        Usuarios = Convert.ToInt32(reader["usuarios"]);
        Estatus = ResumenOficinaEstatus.Completado;
    }
}
