using System;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.CFDI.Models;

public class ResumenFactura
{
    public ResumenOficinaEstatus Estatus { get; set; } = ResumenOficinaEstatus.Pendiente;
    public IEnlace Enlace { get; set; }
    public string IdSucursal { get; set; }
    public string Sucursal { get; set; }
    public int TotalFacturas { get; set; }
    public int FactsPorTimbrara { get; set; }
    public int FactsTimbrado { get; set; }
    public int FactsCancelado { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Iva { get; set; }
    public decimal Total { get; set; }

    public string Oficina
    {
        get => Enlace == null ? string.Empty : Enlace.Nombre;
    }

    public int Id
    {
        get => Enlace == null ? 0 : Enlace.Id;
    }

    public string DescSucursal
    {
        get => $"{Enlace.Nombre} - {Sucursal}";
    }
}
