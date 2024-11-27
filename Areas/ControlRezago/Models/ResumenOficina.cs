using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.ControlRezago.Models{
    public class ResumenOficina : IResumOficina {
        public ResumenOficinaEstatus Estatus { get; set;}
        public IEnlace Enlace {get;set;}
        public int Id {get => Enlace==null?999:Enlace.Id; }
        public string Oficina {get => Enlace==null?"TOTAL":Enlace.Nombre; }
        
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
        

        public ResumenOficina(){
            Estatus = ResumenOficinaEstatus.Pendiente;
            this.Usu_0 = 0;
            this.Usu_1_2 = 0;
            this.Usu_3_5 = 0;
            this.Usu_6_10 = 0;
            this.Usu_11 = 0;
            this.Usuarios = 0;
            this.Imp_0 = 0m;
            this.Imp_1_2 = 0m;
            this.Imp_3_5 = 0m;
            this.Imp_6_10 = 0m;
            this.Imp_11 = 0m;
            this.Total = 0m;
        }

        public static ResumenOficina FromSqlDataReader(SqlDataReader reader){
            var result = new ResumenOficina();
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