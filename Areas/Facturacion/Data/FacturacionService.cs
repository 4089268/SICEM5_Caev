using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.Data;
using Sicem_Blazor.Facturacion.Models;
using Microsoft.Extensions.Logging;

namespace Sicem_Blazor.Services {
    public class FacturacionService {
        private readonly IConfiguration appSettings;
        private readonly SicemService sicemService;
        private readonly ILogger<FacturacionService> logger;

        public FacturacionService(IConfiguration c, SicemService s, ILogger<FacturacionService> l) {
            this.appSettings = c;
            this.sicemService = s;
            this.logger = l;
        }
        
        public Facturacion_Oficina ObtenerFacturacionOficina(IEnlace enlace, int anio, int mes, int sb, int sec){
            var result = new Facturacion_Oficina(){
                Id_Oficina = enlace.Id,
                Oficina = enlace.Nombre
            };
            try {
                using(var _conexion = new SqlConnection(enlace.GetConnectionString() )){
                    _conexion.Open();
                    var _query = string.Format("Execute [SICEM_QROO].[Facturacion_01] @nAno = {0}, @nMes = {1}, @nSb = {2}, @nSect = {3}", anio, mes, sb, sec);
                    var _command = new SqlCommand(_query, _conexion);
                    using(SqlDataReader _reader = _command.ExecuteReader() ){
                        if(_reader.Read()){
                            var tmpInt = 0;
                            var tmpDec = 0m;
                            result.Domestico_Usu = int.TryParse(_reader["usu_domestico"].ToString(), out tmpInt)?tmpInt:0;
                            result.Domestico_Fact = decimal.TryParse(_reader["fac_domestico"].ToString(), out tmpDec)?tmpDec:0m;
                            result.Hotelero_Usu = int.TryParse(_reader["Usu_Hotelera"].ToString(), out tmpInt)?tmpInt:0;
                            result.Hotelero_Fact = decimal.TryParse(_reader["fac_hotelera"].ToString(), out tmpDec)?tmpDec:0m;
                            result.Comercial_Usu = int.TryParse(_reader["usu_comercial"].ToString(), out tmpInt)?tmpInt:0;
                            result.Comercial_Fact = decimal.TryParse(_reader["fac_comercial"].ToString(), out tmpDec)?tmpDec:0m;
                            result.Industrial_Usu = int.TryParse(_reader["usu_industrial"].ToString(), out tmpInt)?tmpInt:0;
                            result.Industrial_Fact = decimal.TryParse(_reader["fac_industrial"].ToString(), out tmpDec)?tmpDec:0m;
                            result.ServGener_Usu = int.TryParse(_reader["usu_servGen"].ToString(), out tmpInt)?tmpInt:0;
                            result.ServGener_Fact = decimal.TryParse(_reader["fac_servGen"].ToString(), out tmpDec)?tmpDec:0m;
                            result.Subtotal = decimal.TryParse(_reader["subtotal"].ToString(), out tmpDec)?tmpDec:0m;
                            result.Iva = decimal.TryParse(_reader["iva"].ToString(), out tmpDec)?tmpDec:0m;
                            result.Total = decimal.TryParse(_reader["total"].ToString(), out tmpDec)?tmpDec:0m;
                            result.Usuarios = int.TryParse(_reader["usuarios"].ToString(), out tmpInt)?tmpInt:0;
                        }
                    }
                    _conexion.Close();
                }
                result.Estatus = 1;
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener la facturacion de la oficina {enlace.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                result.Estatus = 2;
            }
            return result;
        }

        public IEnumerable<Facturacion_Conceptos> ObtenerFacturacionConceptos(IEnlace enlace, int anio, int mes, int sb, int sec, int idLocalidad = 0) {
            var respuesta = new List<Facturacion_Conceptos>();
            try{
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText =  string.Format("Execute [SICEM_QROO].[Facturacion_02] @nAno = {0}, @nMes = {1}, @idLocalidad = {2}, @nSub = {3}, @nSec = {4}", anio, mes, idLocalidad, sb, sec);
                        using(SqlDataReader _reader = xCommand.ExecuteReader()) {
                            while(_reader.Read()) {
                                var newItem = new Facturacion_Conceptos();
                                int tmpInt = 0;
                                decimal tmpDec = 0m;
                                newItem.Id_Oficina = enlace.Id;
                                newItem.Oficina = enlace.Nombre;
                                newItem.Es_Rezago = 0;
                                newItem.Id_Concepto = int.TryParse(_reader["id_concepto"].ToString(), out tmpInt)?tmpInt:0;
                                newItem.Concepto = _reader["concepto"].ToString().ToUpper();
                                newItem.Domestico_Sub = decimal.TryParse(_reader.GetValue("sub_domestico").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Domestico_Iva = decimal.TryParse(_reader.GetValue("iva_domestico").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Domestico_Total = decimal.TryParse(_reader.GetValue("tot_domestico").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Hotelero_Sub = decimal.TryParse(_reader.GetValue("sub_hotelero").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Hotelero_Iva = decimal.TryParse(_reader.GetValue("iva_hotelero").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Hotelero_Total = decimal.TryParse(_reader.GetValue("tot_hotelero").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Comercial_Sub = decimal.TryParse(_reader.GetValue("sub_comercial").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Comercial_Iva = decimal.TryParse(_reader.GetValue("iva_comercial").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Comercial_Total = decimal.TryParse(_reader.GetValue("tot_comercial").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Industrial_Sub = decimal.TryParse(_reader.GetValue("sub_industrial").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Industrial_Iva = decimal.TryParse(_reader.GetValue("iva_industrial").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Industrial_Total = decimal.TryParse(_reader.GetValue("tot_industrial").ToString(), out tmpDec)?tmpDec:0m;                                
                                newItem.ServGen_Sub = decimal.TryParse(_reader.GetValue("sub_servGen").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.ServGen_Iva = decimal.TryParse(_reader.GetValue("iva_servGen").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.ServGen_Total = decimal.TryParse(_reader.GetValue("tot_servGen").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Subtotal = decimal.TryParse(_reader.GetValue("subtotal").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Iva = decimal.TryParse(_reader.GetValue("iva").ToString(), out tmpDec)?tmpDec:0m;
                                newItem.Total = decimal.TryParse(_reader.GetValue("total").ToString(), out tmpDec)?tmpDec:0m;
                                respuesta.Add(newItem);
                            }
                        }
                    }
                    xConnecton.Close();
                }            
                return respuesta;
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener la facturacion por conceptos de la oficina {enlace.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return null;
            }
        }

        public Facturacion_Usuarios[] ObtenerFacturacionUsuarios(IEnlace enlace, int anio, int mes, int sb, int sec) {
            var respuesta = new List<Facturacion_Usuarios>();            
            using(var xConnecton = new SqlConnection(enlace.GetConnectionString() )) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [SICEM].[Facturacion] 'FACTURACION_DETALLE_CONPAGO',{sb},{sec},{anio},{mes}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            respuesta.Add(new Facturacion_Usuarios {
                                Id_Cuenta = long.Parse(xReader.GetValue("CUENTA").ToString()),
                                Localizacion = xReader.GetValue("LOCALIZACION").ToString(),
                                Nombre = xReader.GetValue("NOMBRE").ToString(),
                                Direccion = xReader.GetValue("DIRECCION").ToString(),
                                Estatus = xReader.GetValue("ESTATUS").ToString(),
                                Pago = true
                            });
                        }
                    }
                }
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [SICEM].[Facturacion] 'FACTURACION_DETALLE_SINPAGO',{sb},{sec},{anio},{mes}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            respuesta.Add(new Facturacion_Usuarios {
                                Id_Cuenta = long.Parse(xReader.GetValue("CUENTA").ToString()),
                                Localizacion = xReader.GetValue("LOCALIZACION").ToString(),
                                Nombre = xReader.GetValue("NOMBRE").ToString(),
                                Direccion = xReader.GetValue("DIRECCION").ToString(),
                                Estatus = xReader.GetValue("ESTATUS").ToString(),
                                Pago = false
                            });
                        }
                    }
                }
            }            
            return respuesta.ToArray();
        }


        public IEnumerable<FacturacionAnual> ObtenerFacturacionAnual(IEnlace enlace, int ano, int sb, int sec){
            var _arquos = new ArquosRepositorie(enlace);
            return _arquos.ObtenerFacturacionAnual(ano, sb, sec).ToList();
        }
        public IEnumerable<Factura> ObtenerFacturas(IEnlace oficina, int ano, int mes, int sb, int sec){
            var _arquos = new ArquosRepositorie(oficina);
            return _arquos.ObtenerFacturas(ano, mes, sb, sec);
        }

        public IEnumerable<FacturacionLocalidad> ObtenerFacturacionLocalidades(IEnlace enlace, int anio, int mes, int sb, int sec){
            var result = new List<FacturacionLocalidad>();
            try{
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query = string.Format("exec sicem.facturacion 'FACTURACION_LOCALIDADES', {0}, {1}, {2}, {3}", sb, sec, anio, mes);
                    var _command = new SqlCommand(_query, sqlConnection);
                    _command.CommandTimeout = (int)TimeSpan.FromMinutes(15).TotalSeconds;
                    using( var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            result.Add( FacturacionLocalidad.FromDataReader(reader));
                        }
                    }
                    sqlConnection.Close();
                }
                return result;

            }catch(Exception err){
                logger.LogError(err, $"Error al obtener la facturacion por localidades de {enlace.Nombre}");
                return new List<FacturacionLocalidad>();
            }
        }

        public IEnumerable<FacturacionColonia> ObtenerFacturacionColonias(IEnlace enlace, int anio, int mes, int sb, int sec, int idLocalidad){
            var result = new List<FacturacionColonia>();
            try{
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query = string.Format("exec sicem.facturacion 'FACTURACION_LOCALIDADES_COLONIAS', {0}, {1}, {2}, {3}, @nLocalidad = {4}", sb, sec, anio, mes, idLocalidad);
                    var _command = new SqlCommand(_query, sqlConnection);
                    _command.CommandTimeout = (int)TimeSpan.FromMinutes(15).TotalSeconds;
                    using( var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            result.Add( FacturacionColonia.FromDataReader(reader));
                        }
                    }
                    sqlConnection.Close();
                }
                return result;

            }catch(Exception err){
                logger.LogError(err, $"Error al obtener la facturacion por localidades de {enlace.Nombre}");
                return new List<FacturacionColonia>();
            }
        }
    
    }
}