using System.Data;
using System.Data.SqlClient;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Recaudacion.Models{
    public class ConceptoTipoUsuario {
        public int Id_Concepto {get;set;}
        public string Descripcion {get;set;}
        public bool EsRezago {get;set;} = false;

        //Domestico
        public decimal DomesticoSubTot {get;set;} = 0m;
        public decimal DomesticoIVA {get;set;} = 0m;
        public decimal DomesticoTotal {get => DomesticoSubTot + DomesticoIVA;}
        public int DomesticoUsu {get;set;}

        //Hotelero
        public decimal HoteleroSubTot {get;set;}= 0m;
        public decimal HoteleroIVA {get;set;}= 0m;
        public decimal HoteleroTotal {get => HoteleroSubTot + HoteleroIVA;}
        public int HoteleroUsu {get;set;}
        
        //Comercial
        public decimal ComercialSubTot {get;set;} = 0m;
        public decimal ComercialIVA {get;set;} = 0m;
        public decimal ComercialTotal {get => ComercialSubTot + ComercialIVA;}
        public int ComercialUsu {get;set;}
        
        //Industrial
        public decimal IndustrialSubTot {get;set;} = 0m;
        public decimal IndustrialIVA {get;set;} = 0m;
        public decimal IndustrialTotal {get => IndustrialSubTot + IndustrialIVA;}
        public int IndustrialUsu {get;set;}
        
        //ServGen
        public decimal GeneralSubTot {get;set;} = 0m;
        public decimal GeneralIVA {get;set;} = 0m;
        public decimal GeneralTotal { get => GeneralSubTot + GeneralIVA;}
        public int GeneralUsu {get;set;}
        
        public decimal Subtotal {get;set;}
        public decimal IVA {get;set;}
        public decimal Total {get;set;}
        public int Usuarios {get;set;}
        public int Id_Tipo {get;set;}
        public string TipoConcepto {get;set;}

        public  static ConceptoTipoUsuario FromSqlReader(SqlDataReader reader){
            var newItem = new ConceptoTipoUsuario();
            newItem.Id_Concepto = ConvertUtils.ParseInteger("Id_Concepto");
            newItem.Descripcion = reader["Descripcion"].ToString();
            newItem.DomesticoSubTot = ConvertUtils.ParseDecimal(reader["Tot1"].ToString());
            newItem.DomesticoIVA = ConvertUtils.ParseDecimal(reader["IVA1"].ToString());
            newItem.DomesticoUsu = ConvertUtils.ParseInteger(reader["Usu1"].ToString());
            newItem.HoteleroSubTot = ConvertUtils.ParseDecimal(reader["Tot2"].ToString());
            newItem.HoteleroIVA = ConvertUtils.ParseDecimal(reader["IVA2"].ToString());
            newItem.HoteleroUsu = ConvertUtils.ParseInteger(reader["Usu2"].ToString());
            newItem.ComercialSubTot = ConvertUtils.ParseDecimal(reader["Tot3"].ToString());
            newItem.ComercialIVA = ConvertUtils.ParseDecimal(reader["IVA3"].ToString());
            newItem.ComercialUsu = ConvertUtils.ParseInteger(reader["Usu3"].ToString());
            newItem.IndustrialSubTot = ConvertUtils.ParseDecimal(reader["Tot4"].ToString());
            newItem.IndustrialIVA = ConvertUtils.ParseDecimal(reader["IVA4"].ToString());
            newItem.IndustrialUsu = ConvertUtils.ParseInteger(reader["Usu4"].ToString());
            newItem.GeneralIVA = ConvertUtils.ParseDecimal(reader["IVA5"].ToString());
            newItem.GeneralSubTot = ConvertUtils.ParseDecimal(reader["Tot5"].ToString());
            newItem.GeneralUsu = ConvertUtils.ParseInteger(reader["Usu5"].ToString());
            newItem.Subtotal = ConvertUtils.ParseDecimal(reader["Total"].ToString());
            newItem.IVA = ConvertUtils.ParseDecimal(reader["IVA"].ToString());
            newItem.Total = ConvertUtils.ParseDecimal(reader["Tot"].ToString());
            newItem.Id_Tipo = ConvertUtils.ParseInteger(reader["id_tipo"].ToString());
            newItem.Usuarios = ConvertUtils.ParseInteger(reader["Usuarios"].ToString());
            newItem.EsRezago = newItem.Descripcion.Contains("REZ. ");
            return newItem;
        }

    }

}