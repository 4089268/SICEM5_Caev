using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.PagoLinea.Models;

public class OprVenta
{
    public IEnlace Enlace { get; set; } // P-0001716464
    public string Id { get; set; } // P-0001716464
    public string Table { get; set; } // Opr_Abonos
    public string CveCaja { get; set; } // E0026
    public int IdTipoMovto { get; set; } // 6
    public DateTime Fecha { get; set; } // 2024-11-26 00:00:00.000
    public int IdPadron { get; set; } // 12037
    public int IdCuenta { get; set; } // 12647
    public decimal Subtotal { get; set; } // 196.13
    public decimal Iva { get; set; } // 10.72
    public decimal Total { get; set; } // 206.85
    public decimal Cobrado { get; set; } // 206.85
    public int Af { get; set; } // 2024
    public int Mf { get; set; } // 10
    public string Observa { get; set; } // PAGO [$206.85] RECIBIDO EN [PAGOS EN LINEA (SITIO WEB)] EL [26/Nov/2024][[FECHA:26/11/2024 22:30][REFERENCIA_COMERCIO:275000001203720241000000020685][MEDIO_DE_PAGO:VISA/MASTERCARD][TIPO_TARJETA:D][TOTAL_COBRADO:206.85][ESTADO:Dispersado][EMAIL:gen_galeana@hotmail.es][PLATAFORMA:OPENPAYWEB]]
    public string Oficina
    {
        get => Enlace.Nombre;
    }


    public OprVenta(IEnlace enlace){
        this.Enlace = enlace;
    }
    
    public static OprVenta FromSqlDataReader(IEnlace enlace, SqlDataReader reader)
    {
        return new OprVenta(enlace)
        {
            Id = reader["id"] as string,
            Table = reader["table"] as string,
            CveCaja = reader["cve_caja"] as string,
            IdTipoMovto = reader["id_tipomovto"] != DBNull.Value ? Convert.ToInt32(reader["id_tipomovto"]) : 0,
            Fecha = reader["fecha"] != DBNull.Value ? Convert.ToDateTime(reader["fecha"]) : DateTime.MinValue,
            IdPadron = reader["id_padron"] != DBNull.Value ? Convert.ToInt32(reader["id_padron"]) : 0,
            IdCuenta = reader["id_cuenta"] != DBNull.Value ? Convert.ToInt32(reader["id_cuenta"]) : 0,
            Subtotal = reader["subtotal"] != DBNull.Value ? Convert.ToDecimal(reader["subtotal"]) : 0m,
            Iva = reader["iva"] != DBNull.Value ? Convert.ToDecimal(reader["iva"]) : 0m,
            Total = reader["total"] != DBNull.Value ? Convert.ToDecimal(reader["total"]) : 0m,
            Cobrado = reader["cobrado"] != DBNull.Value ? Convert.ToDecimal(reader["cobrado"]) : 0m,
            Af = reader["af"] != DBNull.Value ? Convert.ToInt32(reader["af"]) : 0,
            Mf = reader["mf"] != DBNull.Value ? Convert.ToInt32(reader["mf"]) : 0,
            Observa = reader["observa"] as string
        };
    }
    // Read-only property to extract REFERENCIA_COMERCIO from the Observa string
    public string ReferenciaComercio
    {
        get
        {
            if (!string.IsNullOrEmpty(Observa))
            {
                // Use regular expression to extract the value of 'REFERENCIA_COMERCIO'
                var match = Regex.Match(Observa, @"REFERENCIA_COMERCIO:([\d]+)");
                if (match.Success)
                {
                    return match.Groups[1].Value; // Return the captured value
                }
            }
            return string.Empty; // Return empty string if not found or Observa is null/empty
        }
    }
}
