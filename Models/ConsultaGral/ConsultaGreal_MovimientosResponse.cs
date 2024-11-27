using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Models{
    public class ConsultaGreal_MovimientosResponse {
        public int Id { get; set; }
        public int Id_tipomovto { get; set; }
        public string Folio_movto { get; set; }
        public string Estatus { get; set; }
        public string Operacion { get; set; }
        public double Cargo { get; set; }
        public double Abono { get; set; }
        public double Saldo { get; set; }
        public DateTime? Fecha { get; set; }
        public string Quien { get; set; }
        public string Sucursal { get; set; }
        public string Id_movto { get; set; }
        public string Tipomovto { get; set; }
        public string Observacion { get; set; }

        public static ConsultaGreal_MovimientosResponse FromDataReader(SqlDataReader reader){
            var item = new ConsultaGreal_MovimientosResponse();
            item.Id = int.Parse(reader.GetValue("id").ToString());
            item.Id_tipomovto = int.Parse(reader.GetValue("id_tipomovto").ToString());
            item.Folio_movto = reader.GetValue("folio_movto").ToString();
            item.Estatus = reader.GetValue("estatus").ToString();
            item.Operacion = reader.GetValue("operacion").ToString();
            item.Cargo = double.TryParse(reader.GetValue("cargo").ToString(), out double tmpC)?tmpC:0;
            item.Abono = double.TryParse(reader.GetValue("abono").ToString(), out double tmpA)?tmpA:0;
            item.Saldo = double.TryParse(reader.GetValue("saldo").ToString(), out double tmpS)?tmpS:0;
            item.Fecha = DateTime.Parse(reader.GetValue("fecha").ToString());
            item.Quien = reader.GetValue("quien").ToString();
            item.Sucursal = reader.GetValue("sucursal").ToString();
            item.Id_movto = reader.GetValue("id_movto").ToString();
            try {
                item.Tipomovto = reader.GetValue("tipoMovto").ToString();
            }
            catch(Exception) {
                item.Tipomovto = "";
            }
            return item;
        }
        public static ConsultaGreal_MovimientosResponse FromDataReaderV2(SqlDataReader reader){
            var item = new ConsultaGreal_MovimientosResponse();
            item.Id = ConvertUtils.ParseInteger(reader.GetValue("recno").ToString());
            item.Id_tipomovto = ConvertUtils.ParseInteger(reader.GetValue("id_tipomovto").ToString());
            item.Folio_movto = reader.GetValue("folio").ToString();
            item.Estatus = reader.GetValue("estatus").ToString();
            item.Operacion = reader.GetValue("operacion").ToString();
            item.Cargo = ConvertUtils.ParseDouble(reader.GetValue("cargo").ToString());
            item.Abono = ConvertUtils.ParseDouble(reader.GetValue("abono").ToString());
            item.Saldo = ConvertUtils.ParseDouble(reader.GetValue("saldo").ToString());
            item.Fecha = ConvertUtils.ParseDateTime(reader.GetValue("fecha").ToString());
            item.Quien = reader.GetValue("quien").ToString();
            item.Sucursal = reader.GetValue("sucursal").ToString();
            item.Id_movto = reader.GetValue("id_tipomovto").ToString();
            item.Tipomovto = "";
            item.Observacion = reader.GetValue("observacion").ToString();
            return item;
        }
        
    }

}
