using System;
using System.Data.SqlClient;
using Sicem_Blazor.Data;
using Sicem_Blazor.Facturacion.Models;
using Sicem_Blazor.Models.Entities.Arquos;

namespace Sicem_Blazor.Facturacion.Data {
    public class FacturaAdapter
    {
        public static Factura FromVwOprFactua(VwOprFactura data){
            var factura = new Factura();

            factura.IdFactura = data.IdFactura;
            factura.Fecha = data.Fecha;
            factura.IdPadron  = (long) data.IdPadron;
            factura.IdCuenta  = (long) data.IdCuenta;
            factura.Subtotal = (decimal) data.FSubtotal;
            factura.Iva = (decimal) data.FIva;
            factura.Total = (decimal) data.FTotal;
            factura.IdSucursal  = (int) data.IdSucursal;
            factura.Sucursal = data.Sucursal;
            factura.IdGenero  = int.TryParse(data.IdGenero, out int tmpidGen)?tmpidGen:0;
            factura.Genero = data.Genero;
            factura.IdEstatus  = (int)data.IdEstatus;
            factura.Estatus = data.Estatus;
            factura.Mf  = (int)data.Mf;
            factura.Af  = (int) data.Af;
            factura.IdServicio  = (int) data.IdServicio;
            factura.Servicio = data.Servicio;
            factura.IdTarifa  = (int) data.IdTarifa;
            factura.Tarifa = data.Tarifa;
            factura.Consumo  = (int) data.ConsumoAct;
            factura.IdTipoCalculado  = (int) data.IdTipocalculado;
            factura.TipoCalculado = data.Tipocalculado;
            factura.IdLecturista  = (int)data.IdLecturista;
            factura.Lecturista = data.Lecturista;
            factura.IdCapturo  = int.TryParse(data.IdCapturo, out int tmpidCap)?tmpidCap:0;
            factura.Capturo = data.Capturo;
            factura.IdLocalidad = (int)data.IdLocalidad;
            factura.Localidad = data.Localidad.ToString();
            factura.Subsistema  = (int) data.Sb;
            factura.Sector  = (int) data.Sector;
            factura.AguaSb            = 0m;
            factura.AguaIva           = 0m;
            factura.AguaTotal         = 0m;
            factura.DrenajeSb         = 0m;
            factura.DrenajeIva        = 0m;
            factura.DrenajeTotal      = 0m;
            factura.SaneamientoSb     = 0m;
            factura.SaneamientoIva    = 0m;
            factura.SaneamientoTotal  = 0m;
            factura.ActualizacionSb   = 0m;
            factura.ActualizacionIva  = 0m;
            factura.ActualizacionTotal = 0m;
            factura.OtrosSb         = 0m;
            factura.OtrosIva        = 0m;
            factura.OtrosTotal      = 0m;

            return factura;
        }  
        public static Factura FromSqlDataReader(SqlDataReader reader){
            var factura = new Factura();
            factura.IdFactura = reader["id_factura"].ToString();
            factura.Fecha        = ConvertUtils.ParseDateTime(reader["fecha"]);
            factura.IdPadron    = ConvertUtils.ParseInteger(reader["id_padron"]);
            factura.IdCuenta    = ConvertUtils.ParseInteger(reader["id_cuenta"]);
            factura.Subtotal    = ConvertUtils.ParseDecimal(reader["subtotal"]);
            factura.Iva         = ConvertUtils.ParseDecimal(reader["iva"]);
            factura.Total       = ConvertUtils.ParseDecimal(reader["total"]);
            factura.IdSucursal  = ConvertUtils.ParseInteger(reader["id_sucursal"]);
            factura.Sucursal    = reader["sucursal"].ToString();
            factura.IdGenero    = ConvertUtils.ParseInteger(reader["id_genero"]);
            factura.Genero      = reader["genero"].ToString();
            factura.IdEstatus   = ConvertUtils.ParseInteger(reader["id_estatus"]);
            factura.Estatus     = reader["estatus"].ToString();
            factura.Mf          = ConvertUtils.ParseInteger(reader["mf"]);
            factura.Af          = ConvertUtils.ParseInteger(reader["af"]);
            factura.IdServicio  = ConvertUtils.ParseInteger(reader["id_servicio"]);
            factura.Servicio    = reader["servicio"].ToString();
            factura.IdTarifa    = ConvertUtils.ParseInteger(reader["id_tarifa"]);
            factura.Tarifa      = reader["tarifa"].ToString();
            factura.Consumo     = ConvertUtils.ParseInteger(reader["consumo_act"]);
            factura.IdTipoCalculado = ConvertUtils.ParseInteger(reader["id_tipocalculado"]);
            factura.TipoCalculado   = reader["tipocalculado"].ToString();
            factura.IdLecturista    = ConvertUtils.ParseInteger(reader["id_lecturista"]);
            factura.Lecturista      = reader["lecturista"].ToString();
            factura.IdCapturo       = ConvertUtils.ParseInteger(reader["id_capturo"]);
            factura.Capturo         = reader["capturo"].ToString();
            factura.AguaSb            = ConvertUtils.ParseDecimal(reader["Agua_Sb"]);
            factura.AguaIva           = ConvertUtils.ParseDecimal(reader["Agua_Iva"]);
            factura.AguaTotal         = ConvertUtils.ParseDecimal(reader["Agua_Total"]);
            factura.DrenajeSb         = ConvertUtils.ParseDecimal(reader["Drenaje_Sb"]);
            factura.DrenajeIva        = ConvertUtils.ParseDecimal(reader["Drenaje_Iva"]);
            factura.DrenajeTotal      = ConvertUtils.ParseDecimal(reader["Drenaje_Total"]);
            factura.SaneamientoSb     = ConvertUtils.ParseDecimal(reader["Saneamiento_Sb"]);
            factura.SaneamientoIva    = ConvertUtils.ParseDecimal(reader["Saneamiento_Iva"]);
            factura.SaneamientoTotal  = ConvertUtils.ParseDecimal(reader["Saneamiento_Total"]);
            factura.ActualizacionSb   = ConvertUtils.ParseDecimal(reader["Actualizacion_Sb"]);
            factura.ActualizacionIva  = ConvertUtils.ParseDecimal(reader["Actualizacion_Iva"]);
            factura.ActualizacionTotal = ConvertUtils.ParseDecimal(reader["Actualizacion_Total"]);
            factura.OtrosSb         = ConvertUtils.ParseDecimal(reader["Otros_Sb"]);
            factura.OtrosIva        = ConvertUtils.ParseDecimal(reader["Otros_Iva"]);
            factura.OtrosTotal      = ConvertUtils.ParseDecimal(reader["Otros_Total"]);
            factura.IdLocalidad     = ConvertUtils.ParseInteger(reader["id_localidad"]);
            factura.Localidad       = reader["localidad"].ToString();
            factura.Subsistema      = ConvertUtils.ParseInteger(reader["sb"]);
            factura.Sector          = ConvertUtils.ParseInteger(reader["sector"]);
            return factura;
        }   
    }

}