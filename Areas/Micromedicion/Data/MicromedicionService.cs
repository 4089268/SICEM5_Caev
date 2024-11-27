using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Micromedicion.Models;
using Microsoft.Extensions.Logging;

namespace Sicem_Blazor.Services {
    public class MicromedicionService {
        private readonly IConfiguration appSettings;
        private readonly SicemService sicemService;
        private readonly ILogger<MicromedicionService> logger;
        public MicromedicionService(IConfiguration c, SicemService s, ILogger<MicromedicionService> l) {
            this.appSettings = c;
            this.sicemService = s;
            this.logger = l;
        }

        public IEnumerable<Micromedicion_Oficina> ObtenerMicromedicionPorOficinas(IEnlace[] oficinas, int Ano, int Mes, int Sb, int Sect) {
            var _results = new List<Micromedicion_Oficina>();
            foreach(var _ofi in oficinas){
                var _newItem = new Micromedicion_Oficina();
                _newItem.Enlace = _ofi;
                try {
                    using(var _connection = new SqlConnection(_ofi.GetConnectionString())) {
                        _connection.Open();
                        var _query = $"Exec [Sicem].[Micromedicion_cierre] @Alias = 'ANALISIS-MENSUAL', @Af = {Ano}, @Mf = {Mes}, @Subsistema = {Sb}, @Sector = {Sect}";
                        using(var _commadn = new SqlCommand(_query, _connection)){
                            using(SqlDataReader _reader = _commadn.ExecuteReader()) {
                                if(_reader.Read()) {
                                    int tmpInt = 0; double tmpDouble = 0;
                                    _newItem.Reales = int.TryParse(_reader["reales"].ToString(),  out tmpInt)?tmpInt:0;
                                    _newItem.Reales_Porc = double.TryParse(_reader["rea_porc"].ToString(), out tmpDouble)? tmpDouble : 0;
                                    _newItem.Promedios = int.TryParse(_reader["promedios"].ToString(), out tmpInt) ? tmpInt : 0;
                                    _newItem.Promedios_Porc = double.TryParse(_reader["pro_porc"].ToString(), out tmpDouble) ? tmpDouble : 0;
                                    _newItem.Medidos = int.TryParse(_reader["medidos"].ToString(), out tmpInt) ? tmpInt : 0;
                                    _newItem.Medidos_Porc = double.TryParse(_reader["med_porc"].ToString(), out tmpDouble) ? tmpDouble : 0;
                                    _newItem.Fijos = int.TryParse(_reader["fijos"].ToString(), out tmpInt) ? tmpInt : 0;
                                    _newItem.Fijos_Porc = double.TryParse(_reader["fij_porc"].ToString(), out tmpDouble) ? tmpDouble : 0;
                                    _newItem.Total = int.TryParse(_reader["total"].ToString(), out tmpInt) ? tmpInt : 0;
                                }
                            }
                        }
                        _connection.Close();
                    }
                }
                catch(Exception e){
                    Console.WriteLine($">> Error al obtener la micromedicion de la oficina {_ofi.Nombre} \n{e.Message}");
                }
                finally {
                    _results.Add(_newItem);
                }
            }
            return _results.ToArray();
        }
        public Micromedicion_Oficina ObtenerMicromedicionPorOficina(IEnlace enlace, int Ano, int Mes, int Sb, int Sect) {
            var response = new Micromedicion_Oficina();
            response.Enlace = enlace;

            try {
                using(var _connection = new SqlConnection(enlace.GetConnectionString() )) {
                    _connection.Open();
                    var _query = $"Exec [Sicem].[Micromedicion_cierre] @Alias = 'ANALISIS-MENSUAL', @Af = {Ano}, @Mf = {Mes}, @Subsistema = {Sb}, @Sector = {Sect}";
                    using(var _commadn = new SqlCommand(_query, _connection)) {
                        using(SqlDataReader _reader = _commadn.ExecuteReader()) {
                            if(_reader.Read()) {
                                int tmpInt = 0; double tmpDouble = 0;
                                response.Reales = int.TryParse(_reader["reales"].ToString(), out tmpInt) ? tmpInt : 0;
                                response.Reales_Porc = double.TryParse(_reader["rea_porc"].ToString(), out tmpDouble) ? tmpDouble : 0;
                                response.Promedios = int.TryParse(_reader["promedios"].ToString(), out tmpInt) ? tmpInt : 0;
                                response.Promedios_Porc = double.TryParse(_reader["pro_porc"].ToString(), out tmpDouble) ? tmpDouble : 0;
                                response.Medidos = int.TryParse(_reader["medidos"].ToString(), out tmpInt) ? tmpInt : 0;
                                response.Medidos_Porc = double.TryParse(_reader["med_porc"].ToString(), out tmpDouble) ? tmpDouble : 0;
                                response.Fijos = int.TryParse(_reader["fijos"].ToString(), out tmpInt) ? tmpInt : 0;
                                response.Fijos_Porc = double.TryParse(_reader["fij_porc"].ToString(), out tmpDouble) ? tmpDouble : 0;
                                response.Total = int.TryParse(_reader["total"].ToString(), out tmpInt) ? tmpInt : 0;
                            }
                        }
                    }
                    _connection.Close();
                }
                response.Estatus = Data.Contracts.ResumenOficinaEstatus.Completado;
            }
            catch(Exception e) {
                response.Estatus = Data.Contracts.ResumenOficinaEstatus.Error;
                Console.WriteLine($">> Error al obtener la micromedicion de la oficina {enlace.Nombre} \n{e.Message}");
            }

            return response;
        }

        public IEnumerable<Micromedicion_Item> ObtenerMicromedicionResumen(IEnlace enlace, int Ano, int Sb, int Sect) {
            Micromedicion_Item[] respuesta;
            using(var xConnecton = new SqlConnection(enlace.GetConnectionString() )){
                xConnecton.Open();
                var xItems = new List<Micromedicion_Item>();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"Exec [Sicem].[Micromedicion_cierre] @Alias = 'ANALISIS', @Af = {Ano}, @Subsistema = {Sb}, @Sector = {Sect}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            xItems.Add(new Micromedicion_Item {
                                Mes = int.Parse(xReader.GetValue("mes").ToString()),
                                Descripcion_Mes = xReader.GetValue("nombre_mes").ToString(),
                                Reales = int.Parse(xReader.GetValue("reales").ToString()),
                                Reales_Porc = double.Parse(xReader.GetValue("rea_porc").ToString()),
                                Promedios = int.Parse(xReader.GetValue("promedios").ToString()),
                                Promedios_Porc = double.Parse(xReader.GetValue("pro_porc").ToString()),
                                Medidos = int.Parse(xReader.GetValue("medidos").ToString()),
                                Medidos_Porc = double.Parse(xReader.GetValue("med_porc").ToString()),
                                Fijos = int.Parse(xReader.GetValue("fijos").ToString()),
                                Fijos_Porc = double.Parse(xReader.GetValue("fij_porc").ToString()),
                                Total = int.Parse(xReader.GetValue("total").ToString())
                            });
                        }
                    }
                }
                respuesta = xItems.ToArray<Micromedicion_Item>();
            }
            return respuesta;
        }
        public IEnumerable<Micromedicion_Anomalia> ObtenerMicromedicionAnomalias(IEnlace enlace, int Ano, int Sb, int Sect) {
            Micromedicion_Anomalia[]  respuesta;
            using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                xConnecton.Open();
                var xItems = new List<Micromedicion_Anomalia>();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"Exec [Sicem].[Micromedicion_cierre] @Alias = 'ANOMALIAS', @Af = {Ano}, @Subsistema = {Sb}, @Sector = {Sect}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            xItems.Add(new Micromedicion_Anomalia {
                                Id_Anomalia = int.Parse(xReader.GetValue("id_anomalia").ToString()),
                                Anomalia = xReader.GetValue("anomalia").ToString(),
                                Ene = int.Parse(xReader.GetValue("ene").ToString()),
                                Feb = int.Parse(xReader.GetValue("feb").ToString()),
                                Mar = int.Parse(xReader.GetValue("mar").ToString()),
                                Abr = int.Parse(xReader.GetValue("abr").ToString()),
                                May = int.Parse(xReader.GetValue("may").ToString()),
                                Jun = int.Parse(xReader.GetValue("jun").ToString()),
                                Jul = int.Parse(xReader.GetValue("jul").ToString()),
                                Ago = int.Parse(xReader.GetValue("ago").ToString()),
                                Sep = int.Parse(xReader.GetValue("sep").ToString()),
                                Oct = int.Parse(xReader.GetValue("oct").ToString()),
                                Nov = int.Parse(xReader.GetValue("nov").ToString()),
                                Dic = int.Parse(xReader.GetValue("dic").ToString())
                            });
                        }
                    }
                }
                respuesta = xItems.ToArray<Micromedicion_Anomalia>();
            }
            return respuesta;
        }
        public IEnumerable<Micromedicion_Padron> ObtenerMicromedicionResumenPadron(IEnlace enlace, int Ano, int Mes) {
            Micromedicion_Padron[] respuesta;
            using(var xConnecton = new SqlConnection(enlace.GetConnectionString())){
                xConnecton.Open();
                var xItems = new List<Micromedicion_Padron>();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"Exec [Sicem].[Micromedicion_cierre] @Alias = 'RESUMEN-PADRON', @Mf = {Mes}, @Af = {Ano}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            xItems.Add(new Micromedicion_Padron {
                                Estatus = xReader.GetValue("estatus").ToString(),
                                Contrato = long.Parse(xReader.GetValue("contrato").ToString()),
                                Usuario = xReader.GetValue("usuario").ToString(),
                                Sb = int.Parse(xReader.GetValue("sb").ToString()),
                                Sector = int.Parse(xReader.GetValue("sector").ToString()),
                                Localidad = xReader.GetValue("localidad").ToString(),
                                Colonia = xReader.GetValue("colonia").ToString(),
                                Tipo_usuario = xReader.GetValue("tipo_usuario").ToString(),
                                Servicio = xReader.GetValue("servicio").ToString(),
                                Medidor = xReader.GetValue("medidor").ToString(),
                                Diametro = xReader.GetValue("diametro").ToString(),
                                Marca = xReader.GetValue("marca").ToString(),
                                Modelo = xReader.GetValue("modelo").ToString(),
                                Fecha_inst = DateTime.Parse(xReader.GetValue("fecha_inst").ToString()),
                                Mes = int.Parse(xReader.GetValue("mes").ToString()),
                                Año = int.Parse(xReader.GetValue("año").ToString()),
                                Trabajo = xReader.GetValue("trabajo").ToString(),
                                Fecha_realizo = xReader.GetValue("fecha_realizo").ToString(),
                                Funcionando = xReader.GetValue("estatus_medidor").ToString()
                            });
                        }
                    }
                }
                respuesta = xItems.ToArray<Micromedicion_Padron>();
            }
            return respuesta;
        }

        public IEnumerable<Resumen_Tarifa> ObtenerResumenPorTarifas(IEnlace enlace, int anio, int mes, int sb, int sect){
            var _result = new List<Resumen_Tarifa>();
            try{
                using(var _sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    _sqlConnection.Open();
                    var _query = $"Exec [WEB].[usp_Medicion] 'TOMAS', 0, {anio}, {mes}, 0, 0, ''";
                    var _command = new SqlCommand(_query, _sqlConnection);
                    using(var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            var _newItem = new Resumen_Tarifa();
                            _newItem.Enlace = enlace;	
                            _newItem.Descripcion = reader["descripcion"].ToString();
                            _newItem.IdTarifa = ConvertUtils.ParseInteger(reader["id_tarifa"].ToString());
                            _newItem.UsuMedidorFun = ConvertUtils.ParseInteger(reader["con_med_fun"].ToString());
                            _newItem.UsuMedidorNoFun = ConvertUtils.ParseInteger(reader["con_med_nofun"].ToString());
                            _newItem.UsuConMedidor = ConvertUtils.ParseInteger(reader["con_medidor"].ToString());
                            _newItem.UsuSinMedidor = ConvertUtils.ParseInteger(reader["sin_medidor"].ToString());
                            _newItem.UsuDren = ConvertUtils.ParseInteger(reader["dren"].ToString());
                            _newItem.UsuTomas = ConvertUtils.ParseInteger(reader["tomas"].ToString());
                            _result.Add(_newItem);
                        }
                    }
                    _sqlConnection.Close();
                }
                return _result;
            }catch(Exception err){
                logger.LogError(err, "Error al obtener el Resumen por tarifa de " + enlace.Nombre);
                return new Resumen_Tarifa[]{};
            }
        }
        public IEnumerable<Resumen_Tarifa> ObtenerResumenPorTarifasLocalidad(IEnlace enlace, int anio, int mes, int sb, int sect){
            var _result = new List<Resumen_Tarifa>();
            try{
                using(var _sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    _sqlConnection.Open();
                    var _query = $"Exec [WEB].Usp_Medicion @cAlias = 'TOMAS-LOCALIDADTARIFA', @nAf = {anio}, @nMf = {mes}";
                    var _command = new SqlCommand(_query, _sqlConnection);
                    using(var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            var _newItem = new Resumen_Tarifa();
                            _newItem.Enlace = enlace;	
                            _newItem.Descripcion = reader["descripcion"].ToString();
                            _newItem.IdTarifa = ConvertUtils.ParseInteger(reader["id_tarifa"].ToString());
                            _newItem.UsuMedidorFun = ConvertUtils.ParseInteger(reader["con_med_fun"].ToString());
                            _newItem.UsuMedidorNoFun = ConvertUtils.ParseInteger(reader["con_med_nofun"].ToString());
                            _newItem.UsuConMedidor = ConvertUtils.ParseInteger(reader["con_medidor"].ToString());
                            _newItem.UsuSinMedidor = ConvertUtils.ParseInteger(reader["sin_medidor"].ToString());
                            _newItem.UsuDren = ConvertUtils.ParseInteger(reader["dren"].ToString());
                            _newItem.UsuTomas = ConvertUtils.ParseInteger(reader["tomas"].ToString());
                            _newItem.IdLocalidad = ConvertUtils.ParseInteger(reader["id_localidad"].ToString());
                            _newItem.Localidad = reader["localidad"].ToString();
                            if(string.IsNullOrEmpty(_newItem.Localidad)){
                                _newItem.Localidad = "z--*--";
                            }
                            _result.Add(_newItem);
                        }
                    }
                    _sqlConnection.Close();
                }

                // Agregar totales por localidad
                if(_result.Count() > 1){
                   var _localidades =  _result
                        .Where(item => !item.Localidad.Contains("--*--"))
                        .GroupBy( item => item.Localidad )
                        .ToList();

                    foreach(var group in _localidades){
                        var _newItem = new Resumen_Tarifa();
                        _newItem.Enlace = enlace;
                        _newItem.Descripcion = " TOTAL";
                        _newItem.IdTarifa = 9999;
                        _newItem.UsuMedidorFun = group.Sum(item => item.UsuMedidorFun);
                        _newItem.UsuMedidorNoFun = group.Sum(item => item.UsuMedidorNoFun);
                        _newItem.UsuConMedidor = group.Sum(item => item.UsuConMedidor);
                        _newItem.UsuSinMedidor = group.Sum(item => item.UsuSinMedidor);
                        _newItem.UsuDren =  group.Sum(item => item.UsuDren);
                        _newItem.UsuTomas = group.Sum(item => item.UsuTomas);
                        _newItem.IdLocalidad = group.First().IdTarifa;
                        _newItem.Localidad = group.Key;

                        _result.Add(_newItem);
                    }
                        
                }

                return _result;
            }catch(Exception err){
                logger.LogError(err, "Error al obtener el Resumen por tarifa de " + enlace.Nombre);
                return new Resumen_Tarifa[]{};
            }
        }


        public IEnumerable<Resumen_Sectores> ObtenerResumenPorSector(IEnlace enlace, int anio, int mes, int sb, int sect){
            var _result = new List<Resumen_Sectores>();
            try{
                using(var _sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    _sqlConnection.Open();
                    var _query = $"Exec [WEB].[usp_Medicion] 'TOMAS_X_SECTOR', 0, {anio}, {mes}, 0, 0, ''";
                    var _command = new SqlCommand(_query, _sqlConnection);
                    using(var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            var _newItem = new Resumen_Sectores();
                            _newItem.Enlace = enlace;	
                            _newItem.Sectores = reader["sb_sec"].ToString();
                            _newItem.UsuMedidorFun = ConvertUtils.ParseInteger(reader["con_med_fun"].ToString());
                            _newItem.UsuMedidorNoFun = ConvertUtils.ParseInteger(reader["con_med_nofun"].ToString());
                            _newItem.UsuConMedidor = ConvertUtils.ParseInteger(reader["con_medidor"].ToString());
                            _newItem.UsuSinMedidor = ConvertUtils.ParseInteger(reader["sin_medidor"].ToString());
                            _newItem.UsuDren = ConvertUtils.ParseInteger(reader["dren"].ToString());
                            _newItem.UsuTomas = ConvertUtils.ParseInteger(reader["tomas"].ToString());
                            _result.Add(_newItem);
                        }
                    }
                    _sqlConnection.Close();
                }
                return _result;
            }catch(Exception err){
                logger.LogError(err, "Error al obtener el Resumen por sectores de " + enlace.Nombre);
                return new Resumen_Sectores[]{};
            }
            throw new NotImplementedException();
        }

    }
}
