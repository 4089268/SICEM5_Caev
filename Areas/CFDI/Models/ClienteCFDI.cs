using System;
using System.Data;
using Sicem_Blazor.Data;

public class ClienteCFDI
{
    public IEnlace Enlace {get;set;}
    public int Id_Oficina => Enlace.Id;
    public string Oficina => Enlace.Nombre;
    public string Rfc { get; set; }
    public string RazonSocial { get; set; }
    public string RegimenFiscal { get; set; }
    public string UsoCfdi { get; set; }
    public string CpReceptor { get; set; }
    public string Correo { get; set; }
    public DateTime? UltimaFactura { get; set; }

    public ClienteCFDI(IEnlace enlace)
    {
        this.Enlace = enlace;
    }

    public static ClienteCFDI FromDataReader(IEnlace enlace, IDataReader reader)
    {
        return new ClienteCFDI(enlace)
        {
            Rfc = reader["rfc"] as string,
            RazonSocial = reader["razon_social"] as string,
            RegimenFiscal = reader["regimenFiscal"] as string,
            UsoCfdi = reader["usoCFDI"] as string,
            CpReceptor = reader["cp_receptor"] as string,
            Correo = reader["correo"] as string,
            UltimaFactura = reader["ultimaFactura"] == DBNull.Value
                ? null
                : Convert.ToDateTime(reader["ultimaFactura"])
        };
    }
}
