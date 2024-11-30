using System;
using System.Collections.Generic;
using System.Globalization;
using Sicem_Blazor.PagoLinea.Models;

public static class TransactionRecordAdapter
{
    public static TransactionRecord Adapt(string fileName, string[] csvRow)
    {
        return new TransactionRecord
        {
            ID = long.Parse(csvRow[0]),
            Fecha = DateTime.ParseExact(csvRow[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
            FechaDispersion = DateTime.TryParseExact(csvRow[2], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dp) ?dp :null,
            Comercio = csvRow[3],
            Unidad = csvRow[4],
            Concepto = csvRow[5],
            ReferenciaComercio = csvRow[6],
            Orden = csvRow[7],
            Tipo = csvRow[8],
            MedioDePago = csvRow[9],
            Titular = csvRow[10],
            Banco = csvRow[11],
            ReferenciaTarjeta = csvRow[12],
            TipoTarjeta = csvRow[13],
            Autorizacion = csvRow[14],
            Mensajeria = ParseDecimal(csvRow[15]),
            IvaMensajeria = ParseDecimal(csvRow[16]),
            Servicio = ParseDecimal(csvRow[17]),
            IvaServicio = ParseDecimal(csvRow[18]),
            ComisionComercio = ParseDecimal(csvRow[19]),
            ComisionUsuario = ParseDecimal(csvRow[20]),
            IvaComision = ParseDecimal(csvRow[21]),
            TotalImporte = ParseDecimal(csvRow[22]),
            TotalCobrado = ParseDecimal(csvRow[23]),
            Promocion = csvRow[24],
            Estado = csvRow[25],
            Contrato = long.Parse(csvRow[26]),
            MensajeMedioDePago = csvRow[27],
            Email = csvRow[28],
            Telefono = csvRow[29],
            Plataforma = csvRow[30],
            EstatusReclamacion = csvRow.Length > 31 ? csvRow[31] : null, // Handles empty columns
            FileName = fileName
        };
    }

    static private decimal ParseDecimal(string value)
    {
        return decimal.TryParse(value.Trim('$'), NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
            ? result
            : 0m;
    }
}
