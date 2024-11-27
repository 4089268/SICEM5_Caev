using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.Data;
using Sicem_Blazor.Services;
using Sicem_Blazor.Recaudacion.Models;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.Recaudacion.Data {

    public class RecaudacionService : IRecaudacionService {

        private readonly IConfiguration appSettings;
        private readonly SicemService sicemService;
        private readonly ILogger<IRecaudacionService> logger;
        
        public RecaudacionService(IConfiguration c, SicemService s, ILogger<IRecaudacionService> l) {
            this.appSettings = c;
            this.sicemService = s;
            this.logger = l;
        }
       
        // Todo: Implementar DateRange.cs in parameters 
        public Recaudacion_Resumen ObtenerResumen(IEnlace enlace, DateRange dateRange) {
            var response = new Recaudacion_Resumen(enlace){ Estatus = ResumenOficinaEstatus.Pendiente };
            try {
                logger.LogInformation("(-) Obteniendo ingresos resumen oficina:{Oficina} del {FechaInicial} al {FechaFin} ", enlace.Nombre, dateRange.Desde, dateRange.Hasta);
                using(var connection = new SqlConnection(enlace.GetConnectionString())) {
                    connection.Open();
                    var command = new SqlCommand(){
                        Connection = connection,
                        CommandText = string.Format( StoreProceduresRecaudacion.RECAUDACION_RESUMEN, dateRange.Desde_ISO, dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector)
                    };
                    using(var dataReader = command.ExecuteReader()) {
                        if(dataReader.Read()) {
                            response.LoadFromDataReader(dataReader);
                        }
                    }
                    connection.Close();
                }
            }
            catch(Exception err) {
                logger.LogError(0, err, "(-) Error al obtener resumen recaudacion enlace:{Enlace}", enlace.Nombre );
                response.Estatus = ResumenOficinaEstatus.Error;
            }
            return response;
        }
        public Recaudacion_Analitico ObtenerAnalisisIngresos(IEnlace enlace, DateRange dateRange) {
            try {

                var recaudacionAnaliticoFactory = new RecaudacionAnaliticoFactory();
                
                //*** Analitico 
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand(){
                        Connection = xConnecton,
                        CommandText = string.Format( StoreProceduresRecaudacion.RECAUDACION_ANALITICO, dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector ),
                        CommandTimeout = (int)TimeSpan.FromMinutes(3).TotalSeconds
                    };
                    var dataSetAnalitico = new DataSet();
                    new SqlDataAdapter(xCommand).Fill(dataSetAnalitico);
                    recaudacionAnaliticoFactory.SetDatosAnalitico(dataSetAnalitico);
                    xConnecton.Close();
                }

                //*** Analitico Rezago
                using(var xConnection = new SqlConnection(enlace.GetConnectionString())) {
                    xConnection.Open();
                    var xCommand = new SqlCommand(){
                        Connection = xConnection,
                        CommandText = string.Format( StoreProceduresRecaudacion.RECAUDACION_ANALITICO_REZAGO, dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector),
                        CommandTimeout = (int)TimeSpan.FromMinutes(3).TotalSeconds
                    };
                    var dataSetAnaliticoRezago = new DataSet();
                    new SqlDataAdapter(xCommand).Fill(dataSetAnaliticoRezago);
                    recaudacionAnaliticoFactory.SetDatosAnaliticoRezago(dataSetAnaliticoRezago);
                    xConnection.Close();
                }

                return recaudacionAnaliticoFactory.Build();
            }
            catch(Exception err) {
                logger.LogError(0, err, "(-) Error al obtener ingresos Analitico oficina:{Oficina}", enlace.Nombre);
                return null;
            }
        }
        public IEnumerable<Recaudacion_IngresosDias> ObtenerIngresosPorDias(IEnlace enlace, DateRange dateRange){
            var respuesta = new List<Recaudacion_IngresosDias>();
            try {
                logger.LogInformation("(-) Obteniendo ingresos por dias oficina:{Oficina} del {FechaInicial} al {FechaFin} ", enlace.Nombre, dateRange.Desde, dateRange.Hasta );
                using(var connection = new SqlConnection( enlace.GetConnectionString() )){ 
                    connection.Open();
                    var command = new SqlCommand(){
                        Connection = connection,
                        CommandText = string.Format(StoreProceduresRecaudacion.RECAUDACION_DIAS, dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector)
                    };
                    Console.WriteLine("(-) " + command.CommandText );
                    using(var dataReader = command.ExecuteReader()) {
                        while(dataReader.Read()) {
                            var tmpResRow = new Recaudacion_IngresosDias();
                            tmpResRow.LoadFromDataReader(dataReader);
                            respuesta.Add(tmpResRow);
                        }
                    }                
                }
            }catch(Exception err) {
                logger.LogError(err, "(-) Error al obtener los ingresos por dia enlace:{Oficina}", enlace.Nombre );
                return null;
            }
            return respuesta;
        } 
        public IEnumerable<Recaudacion_IngresosCajas> ObtenerIngresosPorCajas(IEnlace enlace, DateRange dateRange) {
            var respuesta = new List<Recaudacion_IngresosCajas>();
            try {
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = string.Format( StoreProceduresRecaudacion.RECAUDACION_CAJAS, dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector);
                    using(var xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            var tmpResRow = new Recaudacion_IngresosCajas();
                            tmpResRow.LoadFromDataReader(xReader);
                            respuesta.Add(tmpResRow);
                        }
                    }
                }
            }
            catch(Exception err){
                logger.LogError(err, "(-) Error al obtener ingresos por cajas enlace:{Oficina}", enlace.Nombre );
            }
            return respuesta.ToArray();
        }
        public IEnumerable<RecaudacionIngresosxPoblaciones> ObtenerRecaudacionLocalidades(IEnlace enlace, DateRange dateRange){
            var result = new List<RecaudacionIngresosxPoblaciones>();
            try{
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query = string.Format( StoreProceduresRecaudacion.RECAUDACION_LOCALIDADES, dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector);
                    var _command = new SqlCommand(_query, sqlConnection);
                    _command.CommandTimeout = (int)TimeSpan.FromMinutes(15).TotalSeconds;
                    using( var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            var item = new RecaudacionIngresosxPoblaciones();
                            item.LoadFromDataReader(reader);
                            result.Add(item);
                        }
                    }
                    sqlConnection.Close();
                }
                return result;

            }catch(Exception err){
                logger.LogError(err, "(-) Error al obtener ingresos por localidades oficina:{Oficina} desde {Inicio}, hasta {Fin}", enlace.Nombre, dateRange.Desde, dateRange.Hasta);
                return new List<RecaudacionIngresosxPoblaciones>();
            }
        }
        public IEnumerable<Recaudacion_Rezago> ObtenerRezago(IEnlace enlace, DateRange dateRange) {
            var respuesta = new List<Recaudacion_Rezago>();
            try {
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = string.Format( StoreProceduresRecaudacion.RECAUDACION_REZAGO, dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector );
                    using(var xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            var tmpResRow = new Recaudacion_Rezago();
                            tmpResRow.LoadFromDataReader(xReader);
                            respuesta.Add(tmpResRow);
                        }
                    }
                }
                return respuesta.ToArray();
            }
            catch(Exception err){
                logger.LogError(err, "(-) Error al obtener el ingresos rezago enlace:{Oficna}", enlace.Nombre);
                Console.WriteLine(err.StackTrace);
                return null;
            }
        }
        public IEnumerable<IngresosxConceptos> ObtenerIngresosPorConceptos(IEnlace enlace, DateRange dateRange){
            var respuesta = new List<IngresosxConceptos>();
            try {
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = string.Format( StoreProceduresRecaudacion.RECAUDACION_CONCEPTOS, dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector );
                    using(var xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            var tmpResRow = new IngresosxConceptos();
                            tmpResRow.LoadFromDataReader(xReader);
                            respuesta.Add(tmpResRow);
                        }
                    }
                }
                return respuesta.ToArray();
            }
            catch(Exception err){
                logger.LogError(err, "(-)Error al obtener ingresos por conceptos del enlace: {Oficina}", enlace.Nombre);
                Console.WriteLine(err.StackTrace);
                return null;
            }
        }
        public IEnumerable<Ingresos_Conceptos> ObtenerIngresosPorConceptosTipoUsuarios(IEnlace enlace, DateRange dateRange){
            var result = new List<Ingresos_Conceptos>();
            try {
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query = string.Format( StoreProceduresRecaudacion.RECAUDACION_CONCEPTOS_TARIFAS, dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector);
                    var _commad = new SqlCommand(_query, sqlConnection);
                    using(var reader = _commad.ExecuteReader()){
                        while(reader.Read()){
                            var item = new Ingresos_Conceptos();
                            item.LoadFromSqlDataReader(reader);
                            result.Add(item);
                        }
                    }
                    sqlConnection.Close();
                }
                return result;
            }catch(Exception err){
                logger.LogError(err, "(-) Error al tratar de obtener los ingresos de la oficina:{Oficina}", enlace.Nombre);
                return Array.Empty<Ingresos_Conceptos>();
            }
        }
        


        // Todo: Extraer procedures a StoreProceduresRecaudacion.cs 
        // Todo: Extraer poblacion de datos a un factory o constructor
        public IEnumerable<IngresosTipoUsuario> ObtenerIngresosPorTipoUsuarios(IEnlace enlace, DateRange dateRange){
            try {
                var respuesta = new List<IngresosTipoUsuario>();
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = string.Format("EXEC [SICEM].[Recaudacion] 'POR_CONCEPTOS_TARIFAS','{0}','{1}',{2}, {3}", dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector);
                    using(var xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            var tmpResRow = new IngresosTipoUsuario {
                                Id_TipoCalculo = 0,
                                Id_Concepto = xReader.GetFieldValue<int>("Id_Concepto"),
                                Descripcion = xReader.GetFieldValue<string>("Descripcion"),

                                Dom_Sbt = xReader.GetFieldValue<decimal>("Sbt1"),
                                Dom_IVA = xReader.GetFieldValue<decimal>("IVA1"),
                                Dom_Tot = xReader.GetFieldValue<decimal>("Tot1"),

                                Hot_Sbt = xReader.GetFieldValue<decimal>("Sbt2"),
                                Hot_IVA = xReader.GetFieldValue<decimal>("IVA2"),
                                Hot_Tot = xReader.GetFieldValue<decimal>("Tot2"),

                                Com_Sbt = xReader.GetFieldValue<decimal>("Sbt3"),
                                Com_IVA = xReader.GetFieldValue<decimal>("IVA3"),
                                Com_Tot = xReader.GetFieldValue<decimal>("Tot3"),

                                Ind_Sbt = xReader.GetFieldValue<decimal>("Sbt4"),
                                Ind_IVA = xReader.GetFieldValue<decimal>("IVA4"),
                                Ind_Tot = xReader.GetFieldValue<decimal>("Tot4"),

                                Pub_Sbt = xReader.GetFieldValue<decimal>("Sbt5"),
                                Pub_IVA = xReader.GetFieldValue<decimal>("IVA5"),
                                Pub_Tot = xReader.GetFieldValue<decimal>("Tot5"),

                                Subtotal = xReader.GetFieldValue<decimal>("Subtotal"),
                                IVA = xReader.GetFieldValue<decimal>("IVA"),
                                Total = xReader.GetFieldValue<decimal>("Total")
                            };
                            respuesta.Add(tmpResRow);
                        }
                    }
                    xConnecton.Close();
                }

                //*** Medido
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = string.Format("EXEC [SICEM].[Recaudacion] 'POR_CONCEPTOS_TARIFAS_MEDIDOS','{0}','{1}',{2}, {3}", dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector);
                    using(var xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            var tmpResRow = new IngresosTipoUsuario {
                                Id_TipoCalculo = 1,
                                Id_Concepto = xReader.GetFieldValue<int>("Id_Concepto"),
                                Descripcion = xReader.GetFieldValue<string>("Descripcion"),

                                Dom_Sbt = xReader.GetFieldValue<decimal>("Sbt1"),
                                Dom_IVA = xReader.GetFieldValue<decimal>("IVA1"),
                                Dom_Tot = xReader.GetFieldValue<decimal>("Tot1"),

                                Hot_Sbt = xReader.GetFieldValue<decimal>("Sbt2"),
                                Hot_IVA = xReader.GetFieldValue<decimal>("IVA2"),
                                Hot_Tot = xReader.GetFieldValue<decimal>("Tot2"),

                                Com_Sbt = xReader.GetFieldValue<decimal>("Sbt3"),
                                Com_IVA = xReader.GetFieldValue<decimal>("IVA3"),
                                Com_Tot = xReader.GetFieldValue<decimal>("Tot3"),

                                Ind_Sbt = xReader.GetFieldValue<decimal>("Sbt4"),
                                Ind_IVA = xReader.GetFieldValue<decimal>("IVA4"),
                                Ind_Tot = xReader.GetFieldValue<decimal>("Tot4"),

                                Pub_Sbt = xReader.GetFieldValue<decimal>("Sbt5"),
                                Pub_IVA = xReader.GetFieldValue<decimal>("IVA5"),
                                Pub_Tot = xReader.GetFieldValue<decimal>("Tot5"),

                                Subtotal = xReader.GetFieldValue<decimal>("Subtotal"),
                                IVA = xReader.GetFieldValue<decimal>("IVA"),
                                Total = xReader.GetFieldValue<decimal>("Total")
                            };
                            respuesta.Add(tmpResRow);
                        }
                    }

                    xConnecton.Close();
                }

                //*** Promedio
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = string.Format("EXEC [SICEM].[Recaudacion] 'POR_CONCEPTOS_TARIFAS_PROMEDIOS','{0}','{1}',{2}, {3}", dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector);
                    using(var xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            var tmpResRow = new IngresosTipoUsuario {
                                Id_TipoCalculo = 2,
                                Id_Concepto = xReader.GetFieldValue<int>("Id_Concepto"),
                                Descripcion = xReader.GetFieldValue<string>("Descripcion"),

                                Dom_Sbt = xReader.GetFieldValue<decimal>("Sbt1"),
                                Dom_IVA = xReader.GetFieldValue<decimal>("IVA1"),
                                Dom_Tot = xReader.GetFieldValue<decimal>("Tot1"),

                                Hot_Sbt = xReader.GetFieldValue<decimal>("Sbt2"),
                                Hot_IVA = xReader.GetFieldValue<decimal>("IVA2"),
                                Hot_Tot = xReader.GetFieldValue<decimal>("Tot2"),

                                Com_Sbt = xReader.GetFieldValue<decimal>("Sbt3"),
                                Com_IVA = xReader.GetFieldValue<decimal>("IVA3"),
                                Com_Tot = xReader.GetFieldValue<decimal>("Tot3"),

                                Ind_Sbt = xReader.GetFieldValue<decimal>("Sbt4"),
                                Ind_IVA = xReader.GetFieldValue<decimal>("IVA4"),
                                Ind_Tot = xReader.GetFieldValue<decimal>("Tot4"),

                                Pub_Sbt = xReader.GetFieldValue<decimal>("Sbt5"),
                                Pub_IVA = xReader.GetFieldValue<decimal>("IVA5"),
                                Pub_Tot = xReader.GetFieldValue<decimal>("Tot5"),

                                Subtotal = xReader.GetFieldValue<decimal>("Subtotal"),
                                IVA = xReader.GetFieldValue<decimal>("IVA"),
                                Total = xReader.GetFieldValue<decimal>("Total")
                            };
                            respuesta.Add(tmpResRow);
                        }
                    }

                    xConnecton.Close();
                }

                //*** Fijo
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = string.Format("EXEC [SICEM].[Recaudacion] 'POR_CONCEPTOS_TARIFAS_FIJOS','{0}','{1}',{2}, {3}", dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector);
                    using(var xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            var tmpResRow = new IngresosTipoUsuario {
                                Id_TipoCalculo = 3,
                                Id_Concepto = xReader.GetFieldValue<int>("Id_Concepto"),
                                Descripcion = xReader.GetFieldValue<string>("Descripcion"),

                                Dom_Sbt = xReader.GetFieldValue<decimal>("Sbt1"),
                                Dom_IVA = xReader.GetFieldValue<decimal>("IVA1"),
                                Dom_Tot = xReader.GetFieldValue<decimal>("Tot1"),

                                Hot_Sbt = xReader.GetFieldValue<decimal>("Sbt2"),
                                Hot_IVA = xReader.GetFieldValue<decimal>("IVA2"),
                                Hot_Tot = xReader.GetFieldValue<decimal>("Tot2"),

                                Com_Sbt = xReader.GetFieldValue<decimal>("Sbt3"),
                                Com_IVA = xReader.GetFieldValue<decimal>("IVA3"),
                                Com_Tot = xReader.GetFieldValue<decimal>("Tot3"),

                                Ind_Sbt = xReader.GetFieldValue<decimal>("Sbt4"),
                                Ind_IVA = xReader.GetFieldValue<decimal>("IVA4"),
                                Ind_Tot = xReader.GetFieldValue<decimal>("Tot4"),

                                Pub_Sbt = xReader.GetFieldValue<decimal>("Sbt5"),
                                Pub_IVA = xReader.GetFieldValue<decimal>("IVA5"),
                                Pub_Tot = xReader.GetFieldValue<decimal>("Tot5"),

                                Subtotal = xReader.GetFieldValue<decimal>("Subtotal"),
                                IVA = xReader.GetFieldValue<decimal>("IVA"),
                                Total = xReader.GetFieldValue<decimal>("Total")
                            };
                            respuesta.Add(tmpResRow);
                        }
                    }

                    xConnecton.Close();
                }

                return respuesta.ToArray();

            }catch(Exception err) {
                logger.LogError(err, "(-) Error al obtener los ingresos por tipo de usuario enlace:{Oficina}", enlace.Nombre);
                return null;
            }
        }
        
        // Todo: Extraer procedures a StoreProceduresRecaudacion.cs 
        // Todo: Extraer poblacion de datos a factory o constructor
        public IEnumerable<Ingresos_FormasPago> ObtenerIngresosPorFormasPago(IEnlace enlace, DateRange dateRange) {
            try {
                var _result = new List<Ingresos_FormasPago>();
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    var _query = string.Format("Exec [WEB].[usp_Recaudacion] @cAlias='FORMA_PAGO', @xano=0, @xmes=0, @xfec1='{0}', @xfec2='{1}', @sb={2}, @sector={3}", dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector);
                    using(var xCommand = new SqlCommand(_query, xConnecton)){
                        using(SqlDataReader reader = xCommand.ExecuteReader()) {
                            while(reader.Read()){
                                var _newItem = new Ingresos_FormasPago();
                                var tmpInt = 0;
                                var tmpDec = 0m;
                                _newItem.Orden = int.TryParse(reader["orden"].ToString(), out tmpInt) ? tmpInt : 0;
                                _newItem.Id = int.TryParse(reader["id"].ToString(), out tmpInt) ? tmpInt : 0;
                                _newItem.Forma_Pago = reader["forma_pago"].ToString().Trim();
                                _newItem.Cobrado = decimal.TryParse(reader["cobrado"].ToString(), out tmpDec) ? tmpDec : 0m;
                                _newItem.Arqueo = decimal.TryParse(reader["arqueo"].ToString(), out tmpDec) ? tmpDec : 0m;
                                _newItem.Diferencia = decimal.TryParse(reader["dif"].ToString(), out tmpDec) ? tmpDec : 0m;
                                _result.Add(_newItem);
                            }
                        }
                    }
                    xConnecton.Close();
                }
                return _result.ToArray();
            }catch(Exception err){
                logger.LogError(err, "(-) Error al obtener ingresos por formas de pago enlace:{Oficina}", enlace.Nombre );
                return null;
            }
        }
        
        // Todo: Extraer procedures a StoreProceduresRecaudacion.cs
        // Todo: Extrar poblacion de datos a factory class
        public Recaudacion_PagosMayores_Response ObtenerPagosMayores(IEnlace enlace, DateRange dateRange, int total){
            try {
                var respuesta = new Recaudacion_PagosMayores_Response();
                var tmp_listaPagosMayores = new List<Recaudacion_PagosMayores>();
                var tmp_listaPagosM_Items = new List<Recaudacion_PagosMayores_Items>();
                var tmpDataSet = new DataSet();
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = string.Format("Exec [Sicem].[Recaudacion] @cAlias = 'TOP_VENTAS', @cFecha1 = '{0}', @cFecha2 = '{1}', @xParam = {2}", dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector , total);
                    var xDataAdapter = new SqlDataAdapter(xCommand);
                    xDataAdapter.Fill(tmpDataSet);
                    xConnecton.Close();
                }

                if(tmpDataSet.Tables.Count < 2) {
                    logger.LogInformation( "(-) Error al obtener ObtenerPagosMayores \n\tError: El dataset no contiene 2 tablas.");
                    return null;
                }
                else {
                    foreach(DataRow xRow in tmpDataSet.Tables[0].Rows) {
                        var newItem = new Recaudacion_PagosMayores {
                            Id_Padron = int.Parse(xRow["id_padron"].ToString()),
                            Id_Cuenta = int.Parse(xRow["id_Cuenta"].ToString()),
                            Fecha = DateTime.Parse(xRow["fecha"].ToString()),
                            Subtotal = decimal.Parse(xRow["subtotal"].ToString()),
                            Iva = decimal.Parse(xRow["iva"].ToString()),
                            Total = decimal.Parse(xRow["total"].ToString()),
                            Sucursal = xRow["sucursal"].ToString(),
                            Id_Venta = xRow["id_venta"].ToString(),
                            Nombre = xRow["nombre"].ToString(),
                            Direccion = xRow["direccion"].ToString(),
                            Id_Publico = int.Parse(xRow["id_publico"].ToString())
                        };
                        tmp_listaPagosMayores.Add(newItem);
                    }
                    foreach(DataRow xRow in tmpDataSet.Tables[1].Rows) {
                        var newItem = new Recaudacion_PagosMayores_Items {
                            Id_Venta = xRow["id_venta"].ToString(),
                            Concepto = xRow["concepto"].ToString(),
                            Subtotal = decimal.Parse(xRow["subtotal"].ToString()),
                            Iva = decimal.Parse(xRow["iva"].ToString()),
                            Total = decimal.Parse(xRow["total"].ToString())
                        };
                        tmp_listaPagosM_Items.Add(newItem);
                    }
                    respuesta = new Recaudacion_PagosMayores_Response {
                        PagosMayores = tmp_listaPagosMayores.ToArray<Recaudacion_PagosMayores>(),
                        PagosMayores_Detalle = tmp_listaPagosM_Items.ToArray<Recaudacion_PagosMayores_Items>()
                    };
                }
                        
                return respuesta;

            }catch(Exception err){
                logger.LogError(err, "(-) Error al obtener ObtenerPagosMayores oficina:{Oficina}", enlace.Nombre);
                return null;
            }
        }
        public IEnumerable<Recaudacion_IngresosDetalle> ObtenerDetalleIngresos(IEnlace enlace, DateRange dateRange){
            try {
                var response = new List<Recaudacion_IngresosDetalle>();
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())) {
                    logger.LogInformation("(-) Obteniendo detalle ingresos Oficina:{Oficina} del {FechaInicial} al {FechaFin} ", enlace.Nombre, dateRange.Desde, dateRange.Hasta);
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand(){
                        Connection = sqlConnection,
                        CommandText = string.Format(StoreProceduresRecaudacion.RECAUDACION_DETALLE, dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector)
                    };
                    using(SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()){
                        while(sqlDataReader.Read()){
                            response.Add( Recaudacion_IngresosDetalle.FromSqlDataReader(sqlDataReader) );
                        }
                    }
                    sqlConnection.Close();
                }
                return response;
            }catch(Exception err){
                logger.LogError(err, "(-) Error al obtener detalle ingresos oficina:{Oficina} del {Inicio} al {Fin}", enlace.Nombre, dateRange.Desde,  dateRange.Hasta);
                return null;
            }
        }

        // Todo: Extraer procedure a StoredProcesuresRecaudacion.cs
        // Todo: Extraer poblacion de datos a factory class 
        public IEnumerable<Recaudacion_IngresosDetalleConceptos> ObtenerDetalleConceptos(IEnlace enlace, DateRange dateRange, int id_poblacion, int id_colonia){
            var respuesta = new List<Recaudacion_IngresosDetalleConceptos>();
            //*** Rezago Total, xTipo = 0
            using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                xConnecton.Open();
                var xCommand = new SqlCommand();
                xCommand.Connection = xConnecton;
                xCommand.CommandText = string.Format("EXEC [SICEM].[Recaudacion] 'POR_POBLACION_COLONIA_CONCEPTOS','{0}','{1}',{2}, {3}, {4}, {5}", dateRange.Desde_ISO,  dateRange.Hasta_ISO, dateRange.Subsistema, dateRange.Sector, id_poblacion, id_colonia);
                using(var xReader = xCommand.ExecuteReader()) {
                    while(xReader.Read()) {
                        var tmpResRow = new Recaudacion_IngresosDetalleConceptos();
                        tmpResRow.Descripcion = xReader.GetValue("Descripcion").ToString();
                        tmpResRow.Concepto_Con_Iva = decimal.Parse(xReader.GetValue("Conc_Con_Iva").ToString());
                        tmpResRow.Iva = decimal.Parse(xReader.GetValue("Iva").ToString());
                        tmpResRow.Aplicado_Con_Iva = decimal.Parse(xReader.GetValue("Aplicado_Con_Iva").ToString());
                        tmpResRow.Concepto_Sin_Iva = decimal.Parse(xReader.GetValue("Conc_Sin_Iva").ToString());
                        tmpResRow.Total_Aplicado = decimal.Parse(xReader.GetValue("Total_Aplicado").ToString());
                        tmpResRow.Usuarios = int.Parse(xReader.GetValue("Usuarios").ToString());
                        tmpResRow.Id_Concepto = int.Parse(xReader.GetValue("Id_Concepto").ToString());
                        respuesta.Add(tmpResRow);
                    }
                }
                xConnecton.Close();
            }
            return respuesta;
        }
        public IEnumerable<Recaudacion_Rezago_Detalle> ObtenerDetalleRezago(IEnlace enlace, DateRange dateRange, int mes, bool acumulativo){
            throw new NotImplementedException();
        }
        public IEnumerable<ConceptoTipoUsuario> ObtenerRecaudacionPorConceptosYTipoUsuario(IEnlace enlace, DateRange dateRange){
            throw new NotImplementedException();
        }
        // Todo: Extrare procedure a SoredProceduresRecaudacion.cs
        public IEnumerable<RecaudacionIngresosxColonias> ObtenerRecaudacionColonias(IEnlace enlace, DateRange dateRange, int idLocalidad){
            var result = new List<RecaudacionIngresosxColonias>();
            try{
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query =$"Exec [Sicem_QRoo].[Ingresos_05] @xAlias = 'COLONIAS', @xfec1 = '{dateRange.Desde_ISO}', @xfec2 ={dateRange.Hasta_ISO}, @xSb = {dateRange.Subsistema}, @xSec = {dateRange.Sector}, @nParam = {idLocalidad}";
                    var _command = new SqlCommand(_query, sqlConnection);
                    _command.CommandTimeout = (int)TimeSpan.FromMinutes(15).TotalSeconds;
                    using( var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            result.Add( RecaudacionIngresosxColonias.FromDataReader(reader));
                        }
                    }
                    sqlConnection.Close();
                }
                return result;

            }catch(Exception err){
                logger.LogError(err, "(-) Error al obtener la facturacion por localidades oficina:{Oficina}", enlace.Nombre);
                return new List<RecaudacionIngresosxColonias>();
            }
        }

        // Todo: Extraer procedure en StoreProceduresRecaudacion.cs
        public IEnumerable<Recaudacion_IngresosDetalleConceptos> ObtenerIngresosConceptosPorLocalidadColonia(IEnlace enlace, DateRange dateRange, int idLocalidad, int idColonia){
            var result = new List<Recaudacion_IngresosDetalleConceptos>();

            var tmpIdLocalidad = idLocalidad>=999?0:idLocalidad;
            var tmpIdColonia = idColonia>=999?0:idColonia;
            try{
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query =$"Exec [SICEM_QROO].[Ingresos_06] @xfec1 = '{dateRange.Desde_ISO}', @xfec2 = '{dateRange.Hasta_ISO}', @xLocalidad = {idLocalidad}, @xColonia = {tmpIdColonia}";
                    var _command = new SqlCommand(_query, sqlConnection);
                    _command.CommandTimeout = (int)TimeSpan.FromMinutes(15).TotalSeconds;
                    using( var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            result.Add( Recaudacion_IngresosDetalleConceptos.FromSqlDataReader(reader));
                        }
                    }
                    sqlConnection.Close();
                }
                return result;

            }catch(Exception err){
                logger.LogError(err, "(-) Error al obtener la facturacion por localidades oficina:{Oficina}", enlace.Nombre);
                return Array.Empty<Recaudacion_IngresosDetalleConceptos>();
            }
        }

        public IEnumerable<Ingreso_Gravamen> IngresosGravamen(IEnlace enlace, DateRange dateRange)
        {
            throw new NotImplementedException();
        }


        public static Dictionary<int, string> ObtenerCatalogoTiposConceptos(IEnlace enlace){
            var _result = new Dictionary<int,string>();
            _result.Add(0, "z--*--");
            using(var connection = new SqlConnection(enlace.GetConnectionString())){
                connection.Open();
                var _query = "Select id_TipoConcepto as id, descripcion From [Padron].[Cat_TiposConcepto] Where id_TipoConcepto > 0";
                var _command = new SqlCommand(_query, connection);
                using(var reader = _command.ExecuteReader()){
                    while(reader.Read()){
                        _result.Add(ConvertUtils.ParseInteger(reader["id"].ToString()), reader["descripcion"].ToString().ToUpper());
                    }
                }
                connection.Close();
            }
            return _result;
        }

    }

}
