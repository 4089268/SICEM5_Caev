using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.ControlRezago.Models {
    public class HistSaldoTarifa {

        public int IdTarifa {get;set;} = 0;
        public string Tarifa {get;set;}
        public int Usu_0 {get;set;}
        public int Usu_1_2 {get;set;}
        public int Usu_3_5 {get;set;}
        public int Usu_6_10 {get;set;}
        public int Usu_11 {get;set;}
        public int Usuarios {get;set;}
        public decimal Imp_0 {get;set;}
        public decimal Imp_1_2 {get;set;}
        public decimal Imp_3_5 {get;set;}
        public decimal Imp_6_10 {get;set;}
        public decimal Imp_11 {get;set;}
        public decimal Total {get;set;}
        

        public static HistSaldoTarifa FromSqlDataReader(SqlDataReader reader){
            var result = new HistSaldoTarifa();
            result.Tarifa = reader["tarifa"].ToString();
            if(result.Tarifa.Contains("TOTAL")){
                result.IdTarifa = 9999;
            }
            result.Usu_0 = ConvertUtils.ParseInteger(reader["u_ma_0"].ToString());
            result.Usu_1_2 = ConvertUtils.ParseInteger(reader["u_ma_1_2"].ToString());
            result.Usu_3_5 = ConvertUtils.ParseInteger(reader["u_ma_3_5"].ToString());
            result.Usu_6_10 = ConvertUtils.ParseInteger(reader["u_ma_6_10"].ToString());
            result.Usu_11 = ConvertUtils.ParseInteger(reader["u_ma_11"].ToString());
            result.Usuarios = ConvertUtils.ParseInteger(reader["usuarios"].ToString());
            result.Imp_0 = ConvertUtils.ParseDecimal(reader["i_ma_0"].ToString());
            result.Imp_1_2 = ConvertUtils.ParseDecimal(reader["i_ma_1_2"].ToString());
            result.Imp_3_5 = ConvertUtils.ParseDecimal(reader["i_ma_3_5"].ToString());
            result.Imp_6_10 = ConvertUtils.ParseDecimal(reader["i_ma_6_10"].ToString());
            result.Imp_11 = ConvertUtils.ParseDecimal(reader["i_ma_11"].ToString());
            result.Total = ConvertUtils.ParseDecimal(reader["total"].ToString());
            return result;
        }

    }
    
}