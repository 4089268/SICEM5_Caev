using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sicem_Blazor.Data;
using Sicem_Blazor.Models;
using Sicem_Blazor.Tarifas.Models;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Tarifas.Data;
using Microsoft.Extensions.Configuration;

namespace Sicem_Blazor.Services {
    public class TarifasService : ITarifasService {

        private readonly SicemContext context;
        private ILogger<TarifasService> logger;
        private IConfiguration configuration;

        public TarifasService(SicemContext s, ILogger<TarifasService> l, IConfiguration c ){
            this.context = s;
            this.logger = l;
            this.configuration = c;
        }

        public List<CfgTarifa> ObtenerTarifas(){
            throw new NotImplementedException();
            // try{
            //     var result = context.CfgTarifas.ToList<CfgTarifa>();
            //     return result;
            // }catch(Exception err){
            //     Console.WriteLine($">> Error al obtener las tarifas Err:{err.Message}");
            //     return null;
            // }
        }
        public IEnumerable<ITarifa> ObtenerTarifasSicem(){

            var tarifas = this.ObtenerTarifas();
            var catalogoTipoUsuarios = ObtenerCatalogoTiposUsuarios();

            if(tarifas == null){
                return null;
            }

            var result = new List<Tarifa>();

            tarifas.ForEach( t => {
                var newItem = new Tarifa();
                newItem.Id_Tarifa = (int) t.IdTarifa;
                newItem.Id_TipoUsuario = (int) t.IdTipoUsuario;
                newItem.TipoUsuario = catalogoTipoUsuarios.Where(item => item.Id == (int) t.IdTipoUsuario ).FirstOrDefault().Descripcion;
                newItem.Desde = (int) (t.Desde??0m);
                newItem.Hasta = (int) (t.Hasta??0m);
                newItem.Costo = t.Costo??0m;
                newItem.CostoBase = t.CostoBase??0m;
                newItem.CuotaBase = t.CuotaBase??0m;
                newItem.AdicionalM3 = t.AdicionalM3??0m;
                result.Add(newItem);
            });
            return result;

        }
        public IEnumerable<CatTipoUsuario> ObtenerCatalogoTiposUsuarios(){
            throw new NotImplementedException();
            // try{
            //     List<CatTipoUsuario> result = context.CatTipoUsuarios.ToList();
            //     return result;
            // }catch(Exception err){
            //     Console.WriteLine($">> Error al tratar de obtener el catalogo de tipos usuarios \n\tErr:{err.Message}\n\tStack:{err.StackTrace}");
            //     return new List<CatTipoUsuario>();
            // }
        }
        public bool ModificarTarifaSicem(ITarifa tarifa){
            throw new NotImplementedException();
            // try{
            //     var tmpT = context.CfgTarifas.Where(item => item.IdTarifa == tarifa.Id_Tarifa).FirstOrDefault();
            //     if(tmpT != null){
            //         tmpT.Desde = tarifa.Desde;
            //         tmpT.Hasta = tarifa.Hasta;
            //         tmpT.Costo = tarifa.Costo;
            //         tmpT.CostoBase = tarifa.CostoBase;
            //         tmpT.CuotaBase = tarifa.CuotaBase;
            //         tmpT.AdicionalM3 = tarifa.AdicionalM3;
            //         context.CfgTarifas.Update(tmpT);
            //         context.SaveChanges();
            //         return true;
            //     }else{
            //         return false;
            //     }
            // }catch(Exception err){
            //     Console.WriteLine($">> Error al obtener las tarifas Err:{err.Message}");
            //     return false;
            // }
        }

        /// <summary>
        /// Realiza la consulta de la tarifa actual a la ruta que se pase por parametro.
        /// </summary>
        public Tarifa ObtenerTarifaOficina(IEnlace enlace, int idTarifa){
            var tarifa = new Tarifa(){
                IdOficina = enlace.Id,
                Oficina = enlace.Nombre,
            };
            try{
                using(var conexion = new SqlConnection(enlace.GetConnectionString())){
                    conexion.Open();

                    var _query = " Select id_tarifa, c.id_TipoUsuario, u.descripcion as tipoUsuario, desde, hasta, costo, costo_base, cuota_base, adicional_m3 " + 
                    " From Facturacion.Cat_Tarifas c With(nolock) " +
                    "Left Outer Join [Padron].[Cat_TiposUsuario] u on c.id_TipoUsuario = u.id_tipousuario " +
                    $"Where id_tarifa = {idTarifa}";

                    var command = new SqlCommand(_query, conexion);
                    using(SqlDataReader reader = command.ExecuteReader()){
                        if(reader.Read()){
                            var tmpInt = 0;
                            var tmpDec = 0m;
                            tarifa.Id_Tarifa = int.TryParse(reader["id_tarifa"].ToString(),out tmpInt)?tmpInt:0;
                            tarifa.Id_TipoUsuario = int.TryParse(reader["id_TipoUsuario"].ToString(),out tmpInt)?tmpInt:0;
                            tarifa.TipoUsuario = reader["tipoUsuario"].ToString();
                            tarifa.Desde = int.TryParse(reader["desde"].ToString(),out tmpInt)?tmpInt:0;
                            tarifa.Hasta = int.TryParse(reader["hasta"].ToString(),out tmpInt)?tmpInt:0;
                            tarifa.Costo = decimal.TryParse(reader["costo"].ToString(),out tmpDec)?tmpDec:0;
                            tarifa.CostoBase = decimal.TryParse(reader["costo_base"].ToString(),out tmpDec)?tmpDec:0;
                            tarifa.CuotaBase = decimal.TryParse(reader["cuota_base"].ToString(),out tmpDec)?tmpDec:0;
                            tarifa.AdicionalM3 = decimal.TryParse(reader["adicional_m3"].ToString(),out tmpDec)?tmpDec:0;
                        }                        
                    }
                    conexion.Close();
                }
                tarifa.Estatus = 1;
            }catch(Exception err){
                Console.WriteLine($">>Error al obtener la tarifa id:{idTarifa} de la oficina {enlace.Nombre}\n\tErr:{err.Message}\n\tStacktrace:{err.StackTrace}");
                tarifa.Estatus = 2;
            }
            return tarifa;
        }

        
        /// <summary>
        /// Realiza la consulta de catalogo de tarifas actual de la ruta que se pase por parametro y la almacena el la base de datos local.
        /// </summary>
        public bool GenerarHistorialTarifas(IEnlace enlace) {
            throw new NotImplementedException();

            // var _tarifas = new List<Models.Entities.Arquos.CatTarifa>();

            // //*** Obtener las tarifas de la oficina
            // try {
            //     using(var _arquosContext = new ArquosContext(enlace.GetConnectionString())) {
            //         _tarifas = _arquosContext.CatTarifas.ToList();
            //     }
            // } catch(Exception err) {
            //     Console.WriteLine($">> Error al obtener el catalogo de tarifas de la oficina;{enlace.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
            //     return false;
            // }

            // //*** Almacenar las tarifas en la base local
            // var tmpDate = DateTime.Now;
            // try {
            //     _tarifas.ForEach(tarifaItem => {

            //         HistTarifa _histTarifa = this.context.HistTarifas
            //             .Where(item => item.IdOficina == enlace.Id && item.IdTarifa == tarifaItem.IdTarifa && item.Af == tmpDate.Year && item.Mf == tmpDate.Month)
            //             .FirstOrDefault();

            //         //*** Crear nuevo historial item
            //         if(_histTarifa == null) {
            //             this.context.HistTarifas.Add(new HistTarifa() {
            //                 IdOficina = enlace.Id,
            //                 IdTarifa = (int) tarifaItem.IdTarifa,
            //                 Af = tmpDate.Year,
            //                 Mf = tmpDate.Month,
            //                 IdTipoUsuario = (int)tarifaItem.IdTipoUsuario,
            //                 Desde = (int)tarifaItem.Desde,
            //                 Hasta = (int)tarifaItem.Hasta,
            //                 Costo = tarifaItem.Costo,
            //                 CostoBase = tarifaItem.CostoBase,
            //                 IdEdito = tarifaItem.IdEdito,
            //                 CuotaBase = tarifaItem.CuotaBase,
            //                 AdicionalM3 = tarifaItem.AdicionalM3,
            //                 UltimAct = tmpDate,
            //             });
            //         }
            //         else {
            //             _histTarifa.IdTipoUsuario = (int)tarifaItem.IdTipoUsuario;
            //             _histTarifa.Desde = (int)tarifaItem.Desde;
            //             _histTarifa.Hasta = (int)tarifaItem.Hasta;
            //             _histTarifa.Costo = tarifaItem.Costo;
            //             _histTarifa.CostoBase = tarifaItem.CostoBase;
            //             _histTarifa.IdEdito = tarifaItem.IdEdito;
            //             _histTarifa.CuotaBase = tarifaItem.CuotaBase;
            //             _histTarifa.AdicionalM3 = tarifaItem.AdicionalM3;
            //             _histTarifa.UltimAct = tmpDate;
            //             this.context.HistTarifas.Update(_histTarifa);
            //         }                    
            //     });
            //     this.context.SaveChanges();
            //     return true;
            // } catch(Exception err) {
            //     Console.WriteLine($">> Error al almacer el historial de tarifas de la oficina;{enlace.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
            //     return false;
            // }
        }

        /// <summary>
        /// Obtiene el hisotrial de tarifas de la base de datos local.
        /// </summary>
        public IEnumerable<HistTarifa> ObtenerHistorialTarifa(IEnlace enlace, int af, int mf) {
            throw new NotImplementedException();

            // try {
            //     var tarifas = this.context.HistTarifas.Where(item => item.IdOficina == enlace.Id && item.Af == af && item.Mf == mf).ToList();
            //     return tarifas;
            // } catch(Exception err) {
            //     Console.WriteLine($">> Error al obtener el historial de tarifas de la oficina id: {enlace.Id}\n\tError:{err.Message}\n\tStcktrace:{err.StackTrace}");
            //     return null;
            // }
        }

        /// <summary>
        /// Obtiene todo el historial de tarifas de la oficina.
        /// </summary>
        public IEnumerable<HistTarifaItem> ObtenerHistorialTarifaComplete(IEnlace enlace) {
            throw new NotImplementedException();
            // try {
            //     var tarifas = this.context.HistTarifas.Where(item => item.IdOficina == enlace.Id)
            //         .Select(item => new HistTarifaItem(){
            //             Oficina = item.IdOficinaNavigation.Oficina,
            //             Af = item.Af??0,
            //             Mf = item.Mf??0,
            //             IdTarifa = item.IdTarifa??0,
            //             IdTipoUsuario = item.IdTipoUsuario??0,
            //             TipoUsuario = item.IdTipoUsuarioNavigation.Descripcion,
            //             Desde = item.Desde??0,
            //             Hasta = item.Hasta??0,
            //             Costo = item.Costo??0m,
            //             CostoBase = item.CostoBase??0m,
            //             CuotaBase = item.CuotaBase??0m,
            //             AdicionalM3 = item.AdicionalM3??0m,
            //             UltimAct = item.UltimAct
            //         })
            //         .ToList();
            //     return tarifas;
            // } catch(Exception err) {
            //     Console.WriteLine($">> Error al obtener el historial de tarifas de la oficina id: {enlace.Id}\n\tError:{err.Message}\n\tStcktrace:{err.StackTrace}");
            //     return null;
            // }
        }


        /// <summary>
        /// Procesar la simulacion de facturacion
        /// </sumary>
        public SimulacionResponse SimularFacturacion(IEnlace enlace, string tarifasXml, SimulacionParams simulacionParams, out string mensaje){
            var result = new SimulacionResponse();
            try{

            using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                sqlConnection.Open();
                
                var _query = string.Format("EXEC [SICEM].[SIMULADOR] 'RESUMEN', {0}, {1}, {2}, {3}, {4}, {5}, '{6}'", simulacionParams.Subsistema, simulacionParams.Sector, simulacionParams.Anio, simulacionParams.Mes, simulacionParams.FactorDren, simulacionParams.FactorSane, tarifasXml);
                var _command = new SqlCommand(_query, sqlConnection);

                var _dataset = new DataSet();
                new SqlDataAdapter(_command).Fill(_dataset);

                if(_dataset.Tables.Count == 9){

                    // Obtener datos globales
                    var _tmpTableGlobal = _dataset.Tables[0];
                    var _tmpListGlobal = new List<SimulacionResumen>();
                    foreach(DataRow row in _tmpTableGlobal.Rows){
                        var _newItem = new SimulacionResumen();
                        _newItem.TipoUsuario = row["tipo_usuario"].ToString();
                        _newItem.Agua = ConvertUtils.ParseDecimal(row["agua"].ToString());
                        _newItem.Drenaje = ConvertUtils.ParseDecimal(row["dren"].ToString());
                        _newItem.Saneamiento = ConvertUtils.ParseDecimal(row["sane"].ToString());
                        _newItem.Total = ConvertUtils.ParseDecimal(row["total"].ToString());
                        _newItem.AguaSim = ConvertUtils.ParseDecimal(row["agua_sim"].ToString());
                        _newItem.DrenajeSim = ConvertUtils.ParseDecimal(row["dren_sim"].ToString());
                        _newItem.SaneamientoSim = ConvertUtils.ParseDecimal(row["sane_sim"].ToString());

                        _newItem.TotalSimulado = ConvertUtils.ParseDecimal(row["total_sim"].ToString());
                        _newItem.Usuarios = ConvertUtils.ParseInteger(row["usu"].ToString());
                        _newItem.M3Facturados = ConvertUtils.ParseInteger(row["m3_fac"].ToString());
                        _newItem.M3Consumidos = ConvertUtils.ParseInteger(row["m3_con"].ToString());
                        _newItem.Porcentaje = ConvertUtils.ParseDouble(row["por"].ToString());
                        _tmpListGlobal.Add(_newItem);
                    }
                    result.DatosGlobal = _tmpListGlobal;

                    // Obtener datos medidos
                    var _tmpTableMedidos = _dataset.Tables[1];
                    var _tmpListMedidos = new List<SimulacionResumen>();
                    foreach(DataRow row in _tmpTableMedidos.Rows){
                        var _newItem = new SimulacionResumen();
                        _newItem.TipoUsuario = row["tipo_usuario"].ToString();
                        _newItem.Agua = ConvertUtils.ParseDecimal(row["agua"].ToString());
                        _newItem.Drenaje = ConvertUtils.ParseDecimal(row["dren"].ToString());
                        _newItem.Saneamiento = ConvertUtils.ParseDecimal(row["sane"].ToString());
                        _newItem.Total = ConvertUtils.ParseDecimal(row["total"].ToString());
                        _newItem.AguaSim = ConvertUtils.ParseDecimal(row["agua_sim"].ToString());
                        _newItem.DrenajeSim = ConvertUtils.ParseDecimal(row["dren_sim"].ToString());
                        _newItem.SaneamientoSim = ConvertUtils.ParseDecimal(row["sane_sim"].ToString());

                        _newItem.TotalSimulado = ConvertUtils.ParseDecimal(row["total_sim"].ToString());
                        _newItem.Usuarios = ConvertUtils.ParseInteger(row["usu"].ToString());
                        _newItem.M3Facturados = ConvertUtils.ParseInteger(row["m3_fac"].ToString());
                        _newItem.M3Consumidos = ConvertUtils.ParseInteger(row["m3_con"].ToString());
                        _newItem.Porcentaje = ConvertUtils.ParseDouble(row["por"].ToString());
                        _tmpListMedidos.Add(_newItem);
                    }
                    result.DatosMedidos = _tmpListMedidos;

                    // Obtener datos fijos
                    var _tmpTableFijos = _dataset.Tables[2];
                    var _tmpListFijos = new List<SimulacionResumen>();
                    foreach(DataRow row in _tmpTableFijos.Rows){
                        var _newItem = new SimulacionResumen();
                        _newItem.TipoUsuario = row["tipo_usuario"].ToString();
                        _newItem.Agua = ConvertUtils.ParseDecimal(row["agua"].ToString());
                        _newItem.Drenaje = ConvertUtils.ParseDecimal(row["dren"].ToString());
                        _newItem.Saneamiento = ConvertUtils.ParseDecimal(row["sane"].ToString());
                        _newItem.Total = ConvertUtils.ParseDecimal(row["total"].ToString());
                        _newItem.AguaSim = ConvertUtils.ParseDecimal(row["agua_sim"].ToString());
                        _newItem.DrenajeSim = ConvertUtils.ParseDecimal(row["dren_sim"].ToString());
                        _newItem.SaneamientoSim = ConvertUtils.ParseDecimal(row["sane_sim"].ToString());

                        _newItem.TotalSimulado = ConvertUtils.ParseDecimal(row["total_sim"].ToString());
                        _newItem.Usuarios = ConvertUtils.ParseInteger(row["usu"].ToString());
                        _newItem.M3Facturados = ConvertUtils.ParseInteger(row["m3_fac"].ToString());
                        _newItem.M3Consumidos = ConvertUtils.ParseInteger(row["m3_con"].ToString());
                        _newItem.Porcentaje = ConvertUtils.ParseDouble(row["por"].ToString());
                        _tmpListFijos.Add(_newItem);
                    }
                    result.DatosFijos = _tmpListFijos;


                    // Rangos Global
                    var _tmpTableRangos = _dataset.Tables[3];
                    var _tmpListRangos = new List<SimulacionRangos>();
                    foreach(DataRow row in _tmpTableRangos.Rows){
                        _tmpListRangos.Add( new SimulacionRangos(){
                            Rango = row["rango"].ToString(),
                            Importe = ConvertUtils.ParseDecimal(row["importe"].ToString()),
                            Metros = ConvertUtils.ParseInteger(row["m3"].ToString()),
                            Usuarios = ConvertUtils.ParseInteger(row["usuarios"].ToString()),
                            Porcentaje = ConvertUtils.ParseDouble(row["por"].ToString())
                        });

                    }
                    result.DatosRangos = _tmpListRangos;

                    // Rangos Domestico
                    _tmpTableRangos = _dataset.Tables[4];
                    _tmpListRangos = new List<SimulacionRangos>();
                    foreach(DataRow row in _tmpTableRangos.Rows){
                        _tmpListRangos.Add( new SimulacionRangos(){
                            Rango = row["rango"].ToString(),
                            Importe = ConvertUtils.ParseDecimal(row["importe"].ToString()),
                            Metros = ConvertUtils.ParseInteger(row["m3"].ToString()),
                            Usuarios = ConvertUtils.ParseInteger(row["usuarios"].ToString()),
                            Porcentaje = ConvertUtils.ParseDouble(row["por"].ToString())
                        });

                    }
                    result.DatosRangosDomestico = _tmpListRangos;

                    // Rangos Hotelero
                    _tmpTableRangos = _dataset.Tables[5];
                    _tmpListRangos = new List<SimulacionRangos>();
                    foreach(DataRow row in _tmpTableRangos.Rows){
                        _tmpListRangos.Add( new SimulacionRangos(){
                            Rango = row["rango"].ToString(),
                            Importe = ConvertUtils.ParseDecimal(row["importe"].ToString()),
                            Metros = ConvertUtils.ParseInteger(row["m3"].ToString()),
                            Usuarios = ConvertUtils.ParseInteger(row["usuarios"].ToString()),
                            Porcentaje = ConvertUtils.ParseDouble(row["por"].ToString())
                        });

                    }
                    result.DatosRangosHotelero = _tmpListRangos;

                    // Rangos Comercial
                    _tmpTableRangos = _dataset.Tables[6];
                    _tmpListRangos = new List<SimulacionRangos>();
                    foreach(DataRow row in _tmpTableRangos.Rows){
                        _tmpListRangos.Add( new SimulacionRangos(){
                            Rango = row["rango"].ToString(),
                            Importe = ConvertUtils.ParseDecimal(row["importe"].ToString()),
                            Metros = ConvertUtils.ParseInteger(row["m3"].ToString()),
                            Usuarios = ConvertUtils.ParseInteger(row["usuarios"].ToString()),
                            Porcentaje = ConvertUtils.ParseDouble(row["por"].ToString())
                        });

                    }
                    result.DatosRangosComercial = _tmpListRangos;

                    // Rangos Indsutrial
                    _tmpTableRangos = _dataset.Tables[7];
                    _tmpListRangos = new List<SimulacionRangos>();
                    foreach(DataRow row in _tmpTableRangos.Rows){
                        _tmpListRangos.Add( new SimulacionRangos(){
                            Rango = row["rango"].ToString(),
                            Importe = ConvertUtils.ParseDecimal(row["importe"].ToString()),
                            Metros = ConvertUtils.ParseInteger(row["m3"].ToString()),
                            Usuarios = ConvertUtils.ParseInteger(row["usuarios"].ToString()),
                            Porcentaje = ConvertUtils.ParseDouble(row["por"].ToString())
                        });

                    }
                    result.DatosRangosIndustrial = _tmpListRangos;

                    // Rangos Servicios generales
                    _tmpTableRangos = _dataset.Tables[8];
                    _tmpListRangos = new List<SimulacionRangos>();
                    foreach(DataRow row in _tmpTableRangos.Rows){
                        _tmpListRangos.Add( new SimulacionRangos(){
                            Rango = row["rango"].ToString(),
                            Importe = ConvertUtils.ParseDecimal(row["importe"].ToString()),
                            Metros = ConvertUtils.ParseInteger(row["m3"].ToString()),
                            Usuarios = ConvertUtils.ParseInteger(row["usuarios"].ToString()),
                            Porcentaje = ConvertUtils.ParseDouble(row["por"].ToString())
                        });

                    }
                    result.DatosRangosServiciosGen = _tmpListRangos;

                }else{
                    result.DatosGlobal = new SimulacionResumen[]{};
                    result.DatosMedidos = new SimulacionResumen[]{};
                    result.DatosFijos = new SimulacionResumen[]{};

                    result.DatosRangos = new SimulacionRangos[]{};
                    result.DatosRangosDomestico = new SimulacionRangos[]{} ;
                    result.DatosRangosHotelero = new SimulacionRangos[]{} ;
                    result.DatosRangosComercial = new SimulacionRangos[]{} ;
                    result.DatosRangosIndustrial = new SimulacionRangos[]{} ;
                    result.DatosRangosServiciosGen = new SimulacionRangos[]{} ;

                }
                sqlConnection.Close();
            }
            mensaje ="";
            return result;
            }catch(Exception err ){
                mensaje = err.Message;
                return null;
            }

        }


        public bool AlmacenarCustomCatalogoTarifas(StoTarifa stoTarifa, IEnumerable<StoDetTarifa> detTarifas ){
            throw new NotImplementedException();
            // var _result = true;
            // using( var transactionContext = context.Database.BeginTransaction()){
            //     try{
            //         context.StoTarifas.Add(stoTarifa);
            //         context.SaveChanges();

            //         foreach(var item in detTarifas){
            //             item.IdStoTarifa = stoTarifa.Id;
            //         }
            //         context.StoDetTarifas.AddRange(detTarifas.ToArray());
            //         context.SaveChanges();

            //         transactionContext.Commit();
            //     }catch(Exception){
            //         _result = false;
            //         transactionContext.Rollback();
            //     }
            // }
            // return _result;
        }

        /// <summary>
        /// Obtiene el listado de las cabeceras del catalogo de tarifas perzonalizadas
        /// Puede devolver null
        /// </summary>
        public IEnumerable<StoTarifa> ObtenerCustomCatalogoTarifas(){
            throw new NotImplementedException();
            // try{
            //     var _result = new List<StoTarifa>();
            //     _result = context.StoTarifas.ToList();
            //     return _result;
            // }catch(Exception err){
            //     logger.LogError(err, "Erro al tratar de obtener el listado de cabeceras de las tarifas perzonalizadas");
            //     return null;
            // }
        }

        /// <summary>
        /// Obtiene el listado de las tarifas perzonalizadas
        /// Puede devolver null
        /// </summary>
        public IEnumerable<ITarifa> ObtenerDetalleCustomCatalogoTarifas(int idStoTarifa){
            throw new NotImplementedException();
            // try{
            //     var _result = new List<StoDetTarifa>();
            //     _result = context.StoDetTarifas.Where(item => item.IdStoTarifa == idStoTarifa).ToList();

            //     // Actualizar el catalogo de usuarios
            //     var _catUsuario = ObtenerCatalogoTiposUsuarios();
            //     foreach(var item in _result){
            //         try{
            //             string _valor = _catUsuario.Where(c => c.Id == item.IdTipoUsuario).FirstOrDefault().Descripcion;
            //             item.SetTipoUsuario(_valor);
            //         }catch(Exception){}
            //     }
            //     _result = _result.OrderBy(item => item.Id_TipoUsuario).OrderBy(item => item.Desde).ToList();

            //     return _result;
            // }catch(Exception err){
            //     logger.LogError(err, "Error al obtener el listado de tarifas perzonalizadas del id:" + idStoTarifa);
            //     return null;
            // }
        }
        
        /// <summary>
        /// Actualiza los datos de la cabecera de la taria almacenada como el nombre o fecha de modificacion
        /// </summary>
        /// <param name="idStoTarifa">Id de la cebecera a actualizar</param>
        /// <param name="stoTarifa">Datos que se actualizaran</param>
        public void ActualizarCabeceraCustomCatalogoTarifas(int idStoTarifa, StoTarifa stoTarifa){
            throw new NotImplementedException();
            // try{
            //     var _item = context.StoTarifas.Where(item => item.Id == idStoTarifa).First();
            //     _item.Descripcion = stoTarifa.Descripcion;
            //     _item.FechaModificacion = DateTime.Now;
            //     context.StoTarifas.Update(_item);
            //     context.SaveChanges();
            // }catch(Exception err){
            //     logger.LogError(err, "Error al tratar de actualizar el catalogo de tarifas almacenados id:" + idStoTarifa);
            // }
        }
        /// <summary>
        /// Eliminar los datos del catalogo de tarias almacenado
        /// </summary>
        /// <param name="idStorTarifa">Id del catalogo de tarifas a eliminar</param>
        public void EliminarCustomCatalogoTarifas(int idStorTarifa){
            throw new NotImplementedException();
            // try{
            //     var _detalles = context.StoDetTarifas.Where(item => item.IdStoTarifa == idStorTarifa).ToArray();
            //     context.StoDetTarifas.RemoveRange(_detalles);

            //     var _cabecera = context.StoTarifas.Where(item => item.Id == idStorTarifa).FirstOrDefault();
            //     if(_cabecera != null){
            //         context.StoTarifas.Remove(_cabecera);
            //     }

            //     context.SaveChanges();
            // }catch(Exception err){
            //     logger.LogError(err, "Error al tratar de eliminar el catalogo de tarifas id:" + idStorTarifa);
            // }
        }

        public void ActualizarCustomCatalogoTarifas(int idStorTarifad, IEnumerable<StoDetTarifa> detTarifas ){
            throw new NotImplementedException();
            // var _header = context.StoTarifas.Find(idStorTarifad);
            // var _datosEliminar = context.StoDetTarifas.Where(item => item.IdStoTarifa == _header.Id).ToList();
            // context.StoDetTarifas.RemoveRange(_datosEliminar);
            // context.StoDetTarifas.AddRange(detTarifas);
            // context.SaveChanges();
        }

        #region Funciones para el calculo del nuevo catalogo de tarifas

        /// <summary>
        /// Obtiene el Historial de tarifas almacenados en la base central de sicem
        /// </summary>
        public IEnumerable<CatTarifa> ObtenerCatalogoTarifas(int anio, int mes)
        {
            throw new NotImplementedException();
            // try{
            //     var datos = context.CatTarifas.Where( item => item.Af == anio && item.Mf == mes).ToList();
                
            //     // Actualizar la descripcion de tipoUsuario
            //     var _catTipoUsuario = ObtenerCatalogoTiposUsuarios();
            //     foreach( var tarifa in datos){
            //         var _tipoUsu = _catTipoUsuario.Where( item => item.Id == tarifa.Id_TipoUsuario).FirstOrDefault();
            //         if(_tipoUsu != null){
            //             tarifa.TipoUsuario = $"{tarifa.Id_TipoUsuario} - {_tipoUsu.Descripcion}";
            //         }else{
            //             tarifa.TipoUsuario = "-INDEFINIDO-";
            //         }
            //     }
                
            //     datos = datos.OrderBy(item => item.Id_TipoUsuario).OrderBy(item => item.Desde).ToList();

            //     return datos;
            // }catch(Exception err){
            //     logger.LogError(err, "Error al tratr de obtner el catalogo de tarifas");
            //     return null;
            // }
        }

        public FactoresTarifarios ObtenerFactoresTarifarios(int anio, int mes)
        {
            throw new NotImplementedException();
            // try{
            //     var _result = new FactoresTarifarios();
            //     var _connectionString = configuration.GetConnectionString("SICEM").ToString();
            //     using(var sqlConnection = new SqlConnection(_connectionString)){
            //         sqlConnection.Open();

            //         var _query = "Begin \n" +
            //         " DECLARE @xCfe numeric(6,4) = 0; " +
            //         " Declare @xInpc numeric(6,4) = 0; " +
            //         " Declare @xUma numeric(6,4) = 0; " +
            //         $" Exec [Fact].[usp_Factores_Tarifarios] @nAf = {anio}, @nMf = {mes}, @cfe = @xCfe output, @inpc = @xInpc output, @uma = @xUma output " +
            //         " Select @xCfe as cfe, @xInpc as inpc, @xUma as uma \n" +
            //         "End ";

            //         var _command = new SqlCommand(_query, sqlConnection );
            //         using(var reader = _command.ExecuteReader()){
            //             if(reader.Read()){
            //                 _result.FactorCFE = ConvertUtils.ParseDecimal(reader["cfe"].ToString());
            //                 _result.FactorINPC = ConvertUtils.ParseDecimal(reader["inpc"].ToString());;
            //                 _result.FactorUMA = ConvertUtils.ParseDecimal(reader["uma"].ToString());;
            //             }
            //         }
            //         sqlConnection.Close();
            //     }
            //     return _result;
            // }catch(Exception err){
            //     logger.LogError(err, "Error al obtener los factores tarifarios");
            //     return null;
            // }
        }

        public IEnumerable<ITarifa> GenerarNuevoCatalogoTarifas(int anio, int mes, FactoresTarifarios factores)
        {
            var _result = new List<ITarifa>();
            var _catalogoAnt = ObtenerCatalogoTarifas(anio, mes);
            
            var _dateTime = new DateTime(anio, mes, 1);
            _dateTime = _dateTime.AddMonths(1);

            // Actualizar la descripcion de tipoUsuario
            var _catTipoUsuario = ObtenerCatalogoTiposUsuarios();
            foreach(var tarifa in _catalogoAnt){

                var _costoBase = tarifa.CostoBase;
                var _costo = tarifa.Costo;

                var _newCostoBase = GenerarNuevoValor(factores, tarifa.Id_TipoUsuario, _costoBase);
                var _newCosto = GenerarNuevoValor(factores, tarifa.Id_TipoUsuario, _costo);

                var _newItem = new CatTarifa(){
                    Af = _dateTime.Year,
                    Mf = _dateTime.Month,
                    IdTipousuario = tarifa.Id_TipoUsuario,
                    Desde = tarifa.Desde,
                    Hasta = tarifa.Hasta,
                    CostoBase = _newCostoBase,
                    Costo = _newCosto
                };

                var _tipoUsu = _catTipoUsuario.Where(item => item.Id == tarifa.Id_TipoUsuario).FirstOrDefault();
                if(_tipoUsu != null){
                    _newItem.TipoUsuario = $"{tarifa.Id_TipoUsuario} - {_tipoUsu.Descripcion}";
                }else{
                    _newItem.TipoUsuario = "--INDEFINIDO---";
                }

                _result.Add(_newItem);
            }
            
            _result = _result.OrderBy(item => item.Id_TipoUsuario).OrderBy(item => item.Desde).ToList();

            return _result;
        }

        public IEnumerable<Dictionary<string, object>> ObtenerCataloTarifasResumido()
        {
            throw new NotImplementedException();
            // var _result = new List<Dictionary<string,object>>();

            // var _datosAgrupados =  (from p in context.CatTarifas
            //     group p by new {p.Af, p.Mf} into g
            //     select new
            //     {
            //     Anio = g.Key.Af,
            //     Mes = g.Key.Mf
            //     }
            // ).ToList();

            // //var _datosAgrupados = context.CatTarifas.GroupBy(item => new {Anio = item.Af, Mes = item.Mf}).ToList();
            // foreach(var g in _datosAgrupados){
            //     int _anio = (int) g.Anio;
            //     int _mes = (int) g.Mes;
            //     var _dateTime = new DateTime(_anio, _mes, 1);
            //     var _dict = new Dictionary<string,object>{
            //         {"Anio", _anio},
            //         {"Mes", _mes},
            //         {"Descripcion", _dateTime.ToString("yyyy MMMM")}
            //     };

            //     _result.Add(_dict);
            // }
            // return _result.OrderByDescending(i => i["Anio"]).OrderByDescending(j => j["Mes"]);
        }

        private decimal GenerarNuevoValor(FactoresTarifarios fac, int id_tipousuario, decimal valor_ant){
            var _nuevoValor = 0m;
            if(id_tipousuario == 1){
                //Select convert(numeric(10,2), round((@valor*@xUma)+@valor+((@valor*@xUma)+@valor)*@xcfe/100,2)) as result
                _nuevoValor = Math.Round( (valor_ant * fac.FactorUMA) + valor_ant + ((valor_ant * fac.FactorUMA) + valor_ant) * fac.FactorCFE / 100 ,2);
            }else{
                //Select convert(numeric(10,2),round((@valor*@xinpc)+@valor+((@valor*@xinpc)+@valor)*@xcfe/100,2)) as result
                _nuevoValor = Math.Round( (valor_ant * fac.FactorINPC) + valor_ant + ((valor_ant * fac.FactorINPC) + valor_ant) * fac.FactorCFE / 100 ,2);
            }
            return _nuevoValor;
        }

        public bool ActualizarCatalogo(int anio, int mes, IEnumerable<ITarifa> catalogo){
            throw new NotImplementedException();
            // var result = true;
            // try{

            //     using(var tranx = context.Database.BeginTransaction()){
            //         try{
            //             bool _actualizar =  context.CatTarifas.Where(i => (int)i.Af == anio && (int)i.Mf == mes).Count() > 0?false:true;

            //             if(_actualizar){
            //                 var _query = $"Delete From [Fact].[Cat_TipoUsuario] Where Af = {anio} and Mf = {mes}";
                            
            //                 //context.Database.ExecuteSqlCommand("query para eliminar el catalogo anterior");

            //                 var _catAnt = context.CatTarifas.Where( item => (int)item.Af == anio && (int)item.Mf == mes ).ToList();
            //                 foreach(var i in _catAnt){
            //                     i.Af = anio;
            //                     i.Mf = mes;
            //                     i.IdTipousuario = i.Id_TipoUsuario;
            //                     i.Desde = i.Desde;
            //                     i.Hasta = i.Hasta;
            //                     i.Base = i.CostoBase;
            //                     i.Adicional = i.Costo;
            //                 }
            //                 context.CatTarifas.UpdateRange(_catAnt);
            //             }else{
            //                 var _nuevoCat = new List<CatTarifa>();
            //                 foreach(var i in catalogo){
            //                     var _newItem = new CatTarifa();
            //                     _newItem.Af = anio;
            //                     _newItem.Mf = mes;
            //                     _newItem.IdTipousuario = i.Id_TipoUsuario;
            //                     _newItem.Desde = i.Desde;
            //                     _newItem.Hasta = i.Hasta;
            //                     _newItem.Base = i.CostoBase;
            //                     _newItem.Adicional = i.Costo;
            //                     _nuevoCat.Add(_newItem);
            //                 }
            //                 context.CatTarifas.AddRange(_nuevoCat);
            //             }
                        
            //             context.SaveChanges();
            //             tranx.Commit();
                        
            //         }catch(Exception err){
            //             result = false;
            //             tranx.Rollback();
            //             logger.LogError(err, "Error al actualizar el catalogo de tarifas");
            //         }
            //     }
            //     return result;
            // }catch(Exception err){
            //     logger.LogError(err, "Error al actualizar el catalogo de tarifas");
            //     return false;
            // }
        }

        #endregion
    
        #region CatalogosFactores
        public IEnumerable<CatCfe> ObtenerCatalogoCFE(){
            throw new NotImplementedException();
            // return context.CatCves.ToList();
        }
        public void AgregarCatalogoCFE(CatCfe cat){
            throw new NotImplementedException();
            // using(var tranx = context.Database.BeginTransaction()){
            //     try {
            //         var _items = context.CatCves.Where(item => item.Af == cat.Af && item.Mf == cat.Mf).ToList();
            //         if(_items.Count() > 0){
            //             context.CatCves.RemoveRange(_items);
            //             context.SaveChanges();
            //         }
            //         context.CatCves.Add(cat);
            //         context.SaveChanges();
            //         tranx.Commit();
            //     }catch(Exception err){
            //         logger.LogError(err, "Error al almacenar el catalogo CFE");
            //         tranx.Rollback();
            //     }
            // }
        }
        public void EliminarCatalogoCFE(CatCfe cat){
            throw new NotImplementedException();
            // var _items = context.CatCves.Where(item => item.Af == cat.Af && item.Mf == cat.Mf).ToList();
            // context.CatCves.RemoveRange(_items);
            // context.SaveChanges();
        }
        public void ActualizarCatalogoCFE(CatCfe cat){
            throw new NotImplementedException();
            // var _item = context.CatCves.Where(item => item.Af == cat.Af && item.Mf == cat.Mf).FirstOrDefault();
            // if(_item != null){
            //     _item.CostoKw = cat.CostoKw;
            //     _item.FiKw = cat.FiKw;
            //     _item.PorcApli = cat.PorcApli;
            //     _item.PorcInc = cat.PorcInc;
            //     context.CatCves.Update(_item);
            //     context.SaveChanges();
            // }
        }
        
        public IEnumerable<CatUma> ObtenerCatalogoUMA(){
            throw new NotImplementedException();
            //return context.CatUmas.ToList();
        }
        public void AgregarCatalogoUMA(CatUma cat){
            throw new NotImplementedException();
            // context.CatUmas.Add(cat);
            // context.SaveChanges();
        }
        public void EliminarCatalogoUMA(CatUma cat){
            throw new NotImplementedException();
            // var _items = context.CatUmas.Where(i => i.Af == cat.Af).ToArray();
            // context.CatUmas.RemoveRange(_items);
            // context.SaveChanges();
        }
        public void ActualizarCatalogoUMA(CatUma cat){
            throw new NotImplementedException();
            // var _item = context.CatUmas.Where(i => i.Af == cat.Af).FirstOrDefault();
            // if(_item != null){
            //     _item.Uma = cat.Uma;
            //     _item.PorcVaria = cat.PorcVaria;
            //     context.CatUmas.Update(_item);
            //     context.SaveChanges();
            // }
        }
        

        public IEnumerable<CatInpc> ObtenerCatalogoINPC(){
            throw new NotImplementedException();
            // return context.CatInpcs.ToList();
        }
        public void AgregarCatalogoINPC(CatInpc cat){
            throw new NotImplementedException();
            // context.CatInpcs.Add(cat);
            // context.SaveChanges();
        }
        public void EliminarCatalogoINPC(CatInpc cat){
            throw new NotImplementedException();
            // var _items = context.CatInpcs.Where(i => i.Af == cat.Af && i.Mf == cat.Mf).ToArray();
            // context.CatInpcs.RemoveRange(_items);
            // context.SaveChanges();
        }
        public void ActualizarCatalogoINPC(CatInpc cat){
            throw new NotImplementedException();
            // var _item = context.CatInpcs.Where(i => i.Af == cat.Af && i.Mf == cat.Mf).FirstOrDefault();
            // if(_item != null){
            //     _item.Inpc = cat.Inpc;
            //     context.CatInpcs.Update(_item);
            //     context.SaveChanges();
            // }
        }
        #endregion
    }

}