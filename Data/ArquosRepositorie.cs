using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Sicem_Blazor.Models;
using Sicem_Blazor.Facturacion.Models;
using Sicem_Blazor.Facturacion.Data;

namespace Sicem_Blazor.Data {
    public class ArquosRepositorie {
        public IEnlace enlace {get; private set;}
        public ArquosRepositorie(IEnlace e)
        {
            this.enlace = e;
        }
        
        public IEnumerable<dynamic> ObtenerInfoCuenta(int idCuenta)
        {
            var response = new List<dynamic>();

            try
            {
                using(var conexion = new SqlConnection(enlace.GetConnectionString()))
                {
                    conexion.Open();
                    var query = "Select id_padron, id_cuenta, id_localidad, razon_social From [Padron].[Cat_Padron] Where id_cuenta = @idCuenta";
                    var sqlCommand = new SqlCommand(query, conexion)
                    {
                        CommandType = CommandType.Text
                    };
                    sqlCommand.Parameters.AddWithValue("@idCuenta", idCuenta);
                    
                    using(var reader = sqlCommand.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            response.Add( new
                            {
                                IdPadron = ConvertUtils.ParseInteger(reader["id_padron"]),
                                IdCuenta = ConvertUtils.ParseInteger(reader["id_cuenta"]),
                                IdLocalidad = ConvertUtils.ParseInteger(reader["id_localidad"]),
                                RazonSocial = reader["razon_social"].ToString()
                            });
                        }
                    }
                    conexion.Close();
                }
            }
            catch(Exception err)
            {
                Console.WriteLine($">> Error al obtener el listado de facturas de la oficina {enlace.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
            }
            return response;

        }

        public IEnumerable<Factura> ObtenerFacturas(int ano, int mes, int sb, int sec){
            var result = new List<Factura>();
            try{
                using( var conexion = new SqlConnection(enlace.GetConnectionString() )){
                    conexion.Open();
                    var _query = $"Exec [SICEM_QROO].[Facturacion_04] @nAno = {ano}, @nMes = {mes}, @nSb = {sb}, @nSec = {sec}";
                    var _command = new SqlCommand(_query, conexion);
                    using(var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            result.Add( FacturaAdapter.FromSqlDataReader(reader));
                        }
                    }
                    conexion.Close();
                }
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el listado de facturas de la oficina {enlace.Nombre }\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
            }
            return result;

        }

        public IEnumerable<FacturacionAnual> ObtenerFacturacionAnual(int ano, int sb, int sec){
            var result = new List<FacturacionAnual>();
            try{
                using( var conexion = new SqlConnection(enlace.GetConnectionString() )){
                    conexion.Open();
                    var _query = $"Exec [SICEM_QROO].[Facturacion_03] @nAno = {ano}, @nSb = {sb}, @nSec = {sec}";
                    var _command = new SqlCommand(_query, conexion);
                    using(SqlDataReader reader = _command.ExecuteReader() ){
                        if(reader.Read()){
                            var tmpInt = 0;
                            var tmpDec = 0m;

                            result.Add(new FacturacionAnual(){
                                Mes = 1,
                                Descripcion = "Enero",
                                Usuarios = int.TryParse(reader["ene_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["ene_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["ene_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["ene_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["ene_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["ene_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 2,
                                Descripcion = "Febrero",
                                Usuarios = int.TryParse(reader["feb_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["feb_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["feb_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["feb_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["feb_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["feb_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 3,
                                Descripcion = "Marzo",
                                Usuarios = int.TryParse(reader["mar_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["mar_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["mar_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["mar_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["mar_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["mar_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 4,
                                Descripcion = "Abril",
                                Usuarios = int.TryParse(reader["abr_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["abr_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["abr_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["abr_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["abr_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["abr_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 5,
                                Descripcion = "Mayo",
                                Usuarios = int.TryParse(reader["may_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["may_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["may_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["may_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["may_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["may_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 6,
                                Descripcion = "Junio",
                                Usuarios = int.TryParse(reader["jun_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["jun_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["jun_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["jun_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["jun_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["jun_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 7,
                                Descripcion = "Julio",
                                Usuarios = int.TryParse(reader["jul_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["jul_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["jul_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["jul_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["jul_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["jul_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 8,
                                Descripcion = "Agosto",
                                Usuarios = int.TryParse(reader["ago_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["ago_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["ago_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["ago_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["ago_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["ago_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 9,
                                Descripcion = "Septiembre",
                                Usuarios = int.TryParse(reader["sep_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["sep_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["sep_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["sep_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["sep_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["sep_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 10,
                                Descripcion = "Octubre",
                                Usuarios = int.TryParse(reader["oct_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["oct_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["oct_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["oct_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["oct_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["oct_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 11,
                                Descripcion = "Noviembre",
                                Usuarios = int.TryParse(reader["nov_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["nov_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["nov_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["nov_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["nov_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["nov_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 12,
                                Descripcion = "Diciembre",
                                Usuarios = int.TryParse(reader["dic_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["dic_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["dic_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["dic_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["dic_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["dic_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                            result.Add(new FacturacionAnual(){
                                Mes = 666,
                                Descripcion = "Total",
                                Usuarios = int.TryParse(reader["total_usu"].ToString(), out tmpInt)?tmpInt:0,
                                Total = decimal.TryParse(reader["total_tot"].ToString(), out tmpDec)?tmpDec:0m,
                                Volumne = int.TryParse(reader["total_vol"].ToString(), out tmpInt)?tmpInt:0,
                                SubTotal = decimal.TryParse(reader["total_sub"].ToString(), out tmpDec)?tmpDec:0m,
                                Iva = decimal.TryParse(reader["total_iva"].ToString(), out tmpDec)?tmpDec:0m,
                                VolumneFact = int.TryParse(reader["total_volFact"].ToString(), out tmpInt)?tmpInt:0
                            });

                        }
                    }
                    conexion.Close();
                }
                return result;
            }catch(Exception err){
                Console.WriteLine($">>Error al consultar facturacion anual oficina: {enlace.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return new List<FacturacionAnual>();
            }
        }

    }
}
