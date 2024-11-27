using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.ControlRezago.Models;
using Sicem_Blazor.Data;
using Microsoft.Extensions.Logging;

namespace Sicem_Blazor.Services {
    public class ControlRezagoService {
        private readonly IConfiguration appSetting;
        private readonly SicemService sicemService;
        private readonly ILogger<ControlRezagoService> logger;
        
        public ControlRezagoService(IConfiguration c, SicemService s, ILogger<ControlRezagoService> l) {
            this.appSetting = c;
            this.sicemService = s;
            this.logger = l;
        }
        public ResumenOficina ObtenerRezagoResumenOficina(IEnlace enlace, int anio, int mes, int sb, int sect, int IdEstatus){
            var result = new ResumenOficina();
            try{
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query = $"Exec Sicem.Rezago @cAlias ='SOLO_RESUMEN', @nAf={anio}, @nMf={mes}, @nSub={sb}, @nSec={sect}, @idEstatus={IdEstatus}" ;
                    var _command = new SqlCommand(_query, sqlConnection);
                    using (var reader =_command.ExecuteReader()){
                        if(reader.Read()){
                            result = ResumenOficina.FromSqlDataReader(reader);
                        }
                    }
                    sqlConnection.Close();
                }
                result.Estatus = Data.Contracts.ResumenOficinaEstatus.Completado;
                
            }catch(Exception err){
                logger.LogError(err, "Error al tratar de obtener el resumen de de rezago del enlace :" + enlace.Nombre);
                result.Estatus = Data.Contracts.ResumenOficinaEstatus.Error;
            }
            return result;
        }
        public IEnumerable<DetalleRezago> ObtenerDetalle(IEnlace enlace, int anio, int mes, int sb, int sect, int stat = 0){
            try{
                var result = new List<DetalleRezago>();
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query = $"Exec Sicem.Rezago @cAlias ='DESGLOSE_RANGO',@nSub={sb}, @nSec={sect}, @nAf={anio}, @nMf={mes}, @nEsta={stat}";
                    var _command = new SqlCommand(_query, sqlConnection);
                    using(var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            var _newItem = new DetalleRezago();
                            _newItem.Estatus = reader["estatus"].ToString();
                            _newItem.Cuenta = reader["cuenta"].ToString();
                            _newItem.Localizacion = reader["localizacion"].ToString();
                            _newItem.Nombre = reader["nombre"].ToString();
                            _newItem.Direccion = reader["direccion"].ToString();
                            _newItem.Tarifa = reader["tarifa"].ToString();
                            _newItem.Colonia = reader["colonia"].ToString();
                            _newItem.Medidor = reader["medidor"].ToString();
                            _newItem.Calculo = reader["calculo"].ToString();
                            _newItem.Lec_ant = ConvertUtils.ParseInteger(reader["lec_ant"].ToString());
                            _newItem.Lec_act = ConvertUtils.ParseInteger(reader["lec_act"].ToString());
                            _newItem.Consumo = ConvertUtils.ParseInteger(reader["consumo"].ToString());
                            _newItem.Promedio = ConvertUtils.ParseInteger(reader["promedio"].ToString());
                            _newItem.Ma = ConvertUtils.ParseInteger(reader["ma"].ToString());
                            _newItem.Agua = ConvertUtils.ParseDecimal(reader["agua"].ToString());
                            _newItem.Dren = ConvertUtils.ParseDecimal(reader["dren"].ToString());
                            _newItem.Sane = ConvertUtils.ParseDecimal(reader["sane"].ToString());
                            _newItem.Otros = ConvertUtils.ParseDecimal(reader["otros"].ToString());
                            _newItem.Actu = ConvertUtils.ParseDecimal(reader["actu"].ToString());
                            _newItem.RezAgua = ConvertUtils.ParseDecimal(reader["ragua"].ToString());
                            _newItem.RezDren = ConvertUtils.ParseDecimal(reader["rdren"].ToString());
                            _newItem.RezTrat = ConvertUtils.ParseDecimal(reader["rtrat"].ToString());
                            _newItem.Subtotal = ConvertUtils.ParseDecimal(reader["subtotal"].ToString());
                            _newItem.Iva = ConvertUtils.ParseDecimal(reader["iva"].ToString());
                            _newItem.Total = ConvertUtils.ParseDecimal(reader["total"].ToString());
                            result.Add(_newItem);
                        }
                    }
                    sqlConnection.Close();
                }
                return result;
            }catch(Exception err){
                logger.LogError(err, "Error al obtener el detalle del enlace" + enlace.Nombre);
                return null;
            }
        }



        //<summary> Deprecated - marcado para eliminar </summary> Deprecated
        public ControlRezago_GestionCart_Resumen[] ControlRezago_Gestion(int Id_Oficina, string Fecha1, string Fecha2, int Sb, int Sect) {
            var respuesta = new ControlRezago_GestionCart_Resumen[] { };
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    xConnecton.Open();
                    var xItems = new List<ControlRezago_GestionCart_Resumen>();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = $"EXEC [SICEM].[REZAGO_APREMIOS] 'RESUMEN-GESTION','{Fecha1}','{Fecha2}',{Sb},{Sect}";
                        using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                            while(xReader.Read()) {
                                xItems.Add(new ControlRezago_GestionCart_Resumen {
                                    Descripcion = xReader.GetValue("descripcion").ToString(),
                                    Usu_inv = int.Parse(xReader.GetValue("usu_inv").ToString()),
                                    Imp_inv = decimal.Parse(xReader.GetValue("imp_inv").ToString()),
                                    Imp_re_inv = decimal.Parse(xReader.GetValue("imp_re_inv").ToString()),
                                    Usu_req = int.Parse(xReader.GetValue("usu_req").ToString()),
                                    Imp_req = decimal.Parse(xReader.GetValue("imp_req").ToString()),
                                    Imp_re_req = decimal.Parse(xReader.GetValue("imp_re_req").ToString()),
                                    Usu_val = int.Parse(xReader.GetValue("usu_val").ToString()),
                                    Imp_val = decimal.Parse(xReader.GetValue("imp_val").ToString()),
                                    Imp_re_val = decimal.Parse(xReader.GetValue("imp_re_val").ToString()),
                                    Usu_ban = int.Parse(xReader.GetValue("usu_ban").ToString()),
                                    Imp_ban = decimal.Parse(xReader.GetValue("imp_ban").ToString()),
                                    Imp_re_ban = decimal.Parse(xReader.GetValue("imp_re_ban").ToString()),
                                    Usu_tot = int.Parse(xReader.GetValue("usu_tot").ToString()),
                                    Imp_tot = decimal.Parse(xReader.GetValue("imp_tot").ToString()),
                                    Imp_re_total = decimal.Parse(xReader.GetValue("imp_re_total").ToString())
                                });
                            }
                        }
                    }
                    respuesta = xItems.ToArray<ControlRezago_GestionCart_Resumen>();
                }
            return respuesta;
        }

        public ControlRezago_GestionCart_Detalle[] ControlRezago_Detalles(int Id_Oficina, string Fecha1, string Fecha2, int Sb, int Sect) {
            var respuesta = new ControlRezago_GestionCart_Detalle[] { };
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)){
                xConnecton.Open();
                var xItems = new List<ControlRezago_GestionCart_Detalle>();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [SICEM].[REZAGO_APREMIOS] 'REPORTE-SEMANAL','{Fecha1}','{Fecha2}',{Sb},{Sect}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            xItems.Add(new ControlRezago_GestionCart_Detalle {
                                Cuenta = long.Parse(xReader.GetValue("cuenta").ToString()),
                                Usuario = xReader.GetValue("usuario").ToString(),
                                Fecha_Req = xReader.GetValue("fecha_req").ToString(),
                                Fecha_Pago = xReader.GetValue("fecha_pago").ToString(),
                                MA = int.Parse(xReader.GetValue("MA").ToString()),
                                Importe_Requerido = decimal.Parse(xReader.GetValue("importe_requerido").ToString()),
                                Importe_Pago = decimal.Parse(xReader.GetValue("importe_pago").ToString()),
                                Saldo_Actual = decimal.Parse(xReader.GetValue("saldo_actual").ToString()),
                                Situacion = xReader.GetValue("situacion").ToString(),
                                Tipo_Usuario = xReader.GetValue("tipo_usuario").ToString(),
                                Id_Situacion = int.Parse(xReader.GetValue("id_situacion").ToString()),
                                Tipo = xReader.GetValue("tp").ToString().Replace(" ", "")
                            });
                        }
                    }
                }
                respuesta = xItems.ToArray<ControlRezago_GestionCart_Detalle>();
            }
            return respuesta;
        }

        public ControlRezago_AnalisisCart_Resumen[] ControlRezago_AnalisisResumen(int Id_Oficina, int Sb, int Sect, int MesesRez, double SaldoRez) {
            var respuesta = new ControlRezago_AnalisisCart_Resumen[] { };
            
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)){
                xConnecton.Open();
                var xItems = new List<ControlRezago_AnalisisCart_Resumen>();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [Sicem].[Rezago] @cAlias= 'ANALISIS_CARTERA', @nSub={Sb}, @nSec={Sect}, @mesesRez={MesesRez}, @saldoRez={SaldoRez}";

                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            xItems.Add(new ControlRezago_AnalisisCart_Resumen {
                                Tipo_Usuario = xReader.GetValue("tipo_usuario").ToString(),
                                Usu_Regular = int.Parse(xReader.GetValue("usu_regular").ToString()),
                                Imp_Regular = decimal.Parse(xReader.GetValue("imp_regular").ToString()),
                                Usu_Moroso = int.Parse(xReader.GetValue("usu_moroso").ToString()),
                                Imp_Moroso = decimal.Parse(xReader.GetValue("imp_moroso").ToString()),
                                Usu_Total = int.Parse(xReader.GetValue("usu_total").ToString()),
                                Imp_Total = decimal.Parse(xReader.GetValue("imp_total").ToString()),
                                Id_Tipo_Usuario = int.Parse(xReader.GetValue("id_tipo_usuario").ToString())
                            });
                        }
                    }
                }
                respuesta = xItems.ToArray<ControlRezago_AnalisisCart_Resumen>();
            }
            return respuesta;
        }

        public ControlRezago_AnalisisCart_Detalle[] ControlRezago_AnalisisDetalle(int Id_Oficina, int Sb, int Sect, int MesesRez, double SaldoRez) {
            var respuesta = new ControlRezago_AnalisisCart_Detalle[] { };
            
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                var xItems = new List<ControlRezago_AnalisisCart_Detalle>();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [Sicem].[Rezago] @cAlias= 'ANTIGUEDAD_SALDOS', @nSub={Sb}, @nSec={Sect}, @mesesRez={MesesRez}, @saldoRez={SaldoRez}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            xItems.Add(new ControlRezago_AnalisisCart_Detalle {
                                Localidad = xReader.GetValue("localidad").ToString(),
                                Colonia = xReader.GetValue("colonia").ToString(),
                                Tipo_Usuario = xReader.GetValue("tipo_usuario").ToString(),
                                Sb = int.Parse(xReader.GetValue("sb").ToString()),
                                Sector = int.Parse(xReader.GetValue("sector").ToString()),
                                Id_Localidad = int.Parse(xReader.GetValue("id_localidad").ToString()),
                                Id_Colonia = int.Parse(xReader.GetValue("id_colonia").ToString()),
                                Id_Tarifa = int.Parse(xReader.GetValue("id_tarifa").ToString()),
                                Imp_3_6_MESES = decimal.Parse(xReader.GetValue("Imp_3_6_MESES").ToString()),
                                Usu_3_6_MESES = int.Parse(xReader.GetValue("Usu_3_6_MESES").ToString()),
                                Imp_7_12_MESES = decimal.Parse(xReader.GetValue("Imp_7_12_MESES").ToString()),
                                Usu_7_12_MESES = int.Parse(xReader.GetValue("Usu_7_12_MESES").ToString()),
                                Imp_1_2_ANOS = decimal.Parse(xReader.GetValue("Imp_1_2_AÑOS").ToString()),
                                Usu_1_2_ANOS = int.Parse(xReader.GetValue("Usu_1_2_AÑOS").ToString()),
                                Imp_3_5_ANOS = decimal.Parse(xReader.GetValue("Imp_3_5_AÑOS").ToString()),
                                Usu_3_5_ANOS = int.Parse(xReader.GetValue("Usu_3_5_AÑOS").ToString()),
                                Imp_5_ANOS = decimal.Parse(xReader.GetValue("Imp_5_AÑOS").ToString()),
                                Usu_5_ANOS = int.Parse(xReader.GetValue("usu_5_AÑOS").ToString()),
                                IMP_TOTAL = decimal.Parse(xReader.GetValue("IMP_TOTAL").ToString()),
                                USU_TOTAL = int.Parse(xReader.GetValue("USU_TOTAL").ToString())
                            });
                        }
                    }
                }
                respuesta = xItems.ToArray<ControlRezago_AnalisisCart_Detalle>();
            }            
            return respuesta;
        }

        public ControlRezago_Eficacia_Resumen[] ControlRezago_EficaciaResumen(int Id_Oficina, string Fecha1, string Fecha2) {
            ControlRezago_Eficacia_Resumen[] respuesta;
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)){
                xConnecton.Open();
                var xItems = new List<ControlRezago_Eficacia_Resumen>();
                using(var xCommand = new SqlCommand()){
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [Sicem].[Rezago_Apremios] @cAlias = 'EFICACIA-RESUMEN-GLOBAL', @cFecha1 = '{Fecha1}', @cFecha2 = '{Fecha2}'";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            xItems.Add(new ControlRezago_Eficacia_Resumen {
                                Id_Trabajo = int.Parse(xReader.GetValue("id_trabajo").ToString()),
                                Trabajo = xReader.GetValue("trabajo").ToString(),
                                PENDIENTES = int.Parse(xReader.GetValue("PENDIENTES").ToString()),
                                EN_EJECUCION = int.Parse(xReader.GetValue("EN_EJECUCION").ToString()),
                                REAL_EJEC = int.Parse(xReader.GetValue("REAL_EJEC").ToString()),
                                REAL_NO_EJEC = int.Parse(xReader.GetValue("REAL_NO_EJEC").ToString()),
                                REALIZADAS = int.Parse(xReader.GetValue("REALIZADAS").ToString()),
                                CANCELADAS = int.Parse(xReader.GetValue("CANCELADAS").ToString()),
                                TOTALES = int.Parse(xReader.GetValue("TOTALES").ToString()),
                            });
                        }
                    }
                }
                respuesta = xItems.ToArray<ControlRezago_Eficacia_Resumen>();
            }            
            return respuesta;
        }

        public ControlRezago_Eficacia_Detalle[] ControlRezago_EficaciaDetalle(int Id_Oficina, string Fecha1, string Fecha2, int Dias, int IdTrabajo) {
            ControlRezago_Eficacia_Detalle[] respuesta;
            var xEnlace = sicemService.ObtenerEnlaces(1).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                var xItems = new List<ControlRezago_Eficacia_Detalle>();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $" EXEC [Sicem].[Rezago_Apremios] @cAlias = 'EFICACIA-ORDENES', @cFecha1 = '{Fecha1}', @cFecha2 = '{Fecha2}', @xParam = {Dias},  @xParam2 = {IdTrabajo}";

                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            xItems.Add(new ControlRezago_Eficacia_Detalle {
                                Trabajador = xReader.GetValue("trabajador").ToString(),
                                Ord_Tot = int.Parse(xReader.GetValue("ord_tot").ToString()),
                                Ord_efe = int.Parse(xReader.GetValue("ord_efe").ToString()),
                                Porc_efic_ord = double.Parse(xReader.GetValue("porc_efic_ord").ToString()),
                                Imp_gestionado = decimal.Parse(xReader.GetValue("imp_gestionado").ToString()),
                                Imp_cobrado = decimal.Parse(xReader.GetValue("imp_cobrado").ToString()),
                                Porc_efic_cob = double.Parse(xReader.GetValue("porc_efic_cob").ToString()),
                            });
                        }
                    }
                }
                respuesta = xItems.ToArray();
            }
            return respuesta;
        }
        
        public Dictionary<int,string> ControlRezago_CatEficaciaOrdenes(int Id_Oficina) {
            var respuesta = new Dictionary<int, string> ();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = $"EXEC [Sicem].[Rezago_Apremios] @cAlias = 'EFICACIA-CAT-TRABAJOS'";
                        using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                            while(xReader.Read()) {
                                respuesta.Add( int.Parse(xReader.GetValue("id_trabajo").ToString()), xReader.GetValue("trabajo").ToString());
                            }
                        }
                    }
                }
            
            return respuesta;
        }
    

        public IEnumerable<HistSaldoTarifa> ObtenerHistorialSaldosTarifa(IEnlace enlace, int anio, int mes, int sb, int sect, int IdEstatus){
            var result = new List<HistSaldoTarifa>();
            try{
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query = $"Exec Sicem.Rezago @cAlias ='TARIFAS', @nAf={anio}, @nMf={mes}, @nSub={sb}, @nSec={sect}, @idEstatus={IdEstatus}";
                    var sqlCommand = new SqlCommand(_query, sqlConnection);
                    using(var reader = sqlCommand.ExecuteReader()){
                        while(reader.Read()){
                            result.Add( HistSaldoTarifa.FromSqlDataReader(reader));
                        }
                    }
                    sqlConnection.Close();
                }
                return result;
            }catch(Exception err){
                logger.LogError(err, "Error al obtener el rezago por tarifas del enlace " + enlace.Nombre);
                return new HistSaldoTarifa[]{};
            }
        }
        public IEnumerable<HistSaldoLocalidad> ObtenerHistorialSaldosLocalidad(IEnlace enlace, int anio, int mes, int sb, int sect, int IdEstatus){
            var result = new List<HistSaldoLocalidad>();
            try{
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query = $"Exec Sicem.Rezago @cAlias ='COLONIAS', @nAf={anio}, @nMf={mes}, @nSub={sb}, @nSec={sect}, @idEstatus={IdEstatus}";
                    var sqlCommand = new SqlCommand(_query, sqlConnection);
                    using(var reader = sqlCommand.ExecuteReader()){
                        while(reader.Read()){
                            result.Add( HistSaldoLocalidad.FromSqlDataReader(reader));
                        }
                    }
                    sqlConnection.Close();
                }
                return result;
            }catch(Exception err){
                logger.LogError(err, "Error al obtener el rezago por tarifas del enlace " + enlace.Nombre);
                return new HistSaldoLocalidad[]{};
            }
        }

    
    }
}
