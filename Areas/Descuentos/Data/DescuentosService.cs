using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data;
using Sicem_Blazor.Descuentos.Models;
using Sicem_Blazor.Descuentos.Data;
using Sicem_Blazor.Recaudacion.Models;

namespace Sicem_Blazor.Services {
    public class DescuentosService : IDescuentosService  {
        private readonly IConfiguration appSettings;
        private readonly SicemService sicemService;
        private readonly ILogger<IDescuentosService> logger;
        public DescuentosService(IConfiguration configuration, SicemService sicemService, ILogger<IDescuentosService> logger) {
            this.appSettings = configuration;
            this.sicemService = sicemService;
            this.logger = logger;
        }

        
        // Todo: Extraer poblacion de la clase a un Factory
        public DescuentosResumen ObtenerDescuentosResumen(IEnlace enlace, DateRange dateRange) {
            var response = new DescuentosResumen();
            logger.LogInformation("(-) Obteniendo resumen descuentos oficina:{Oficina} del {Desde} al {Hasta}", enlace.Nombre, dateRange.Desde_ISO, dateRange.Hasta_ISO);
            try {
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())) {
                    sqlConnection.Open();
                    using(var sqlCommand = new SqlCommand()) {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = string.Format( StoreProceduresDescuentos.DESCUENTOS_RESUMEN, dateRange.Desde_ISO, dateRange.Hasta_ISO );
                        // Todo: Genear clase DescuentosDia en base a la clase Recaudacion_IngresosDias
                        var tmpRes = new List<Recaudacion_IngresosDias>();
                        var tmpDataSet = new DataSet();
                        new SqlDataAdapter(sqlCommand).Fill(tmpDataSet);
                        
                        //****** Resumen 
                        var xRow = tmpDataSet.Tables[0].Rows[0];
                        response.Conc_Con_Iva = decimal.Parse(xRow["Conc_Con_Iva"].ToString());
                        response.Iva = decimal.Parse(xRow["Iva"].ToString());
                        response.Apli_Con_Iva = decimal.Parse(xRow["Aplicado_Con_Iva"].ToString());
                        response.Conc_Sin_Iva = decimal.Parse(xRow["Conc_Sin_Iva"].ToString());
                        response.Total = decimal.Parse(xRow["Total_Aplicado"].ToString());
                        response.Usuarios = int.Parse(xRow["Usuarios"].ToString());

                        // Obtener listado de conceptos
                        var tmpList = new List<string>();
                        tmpList.Add($"Agua;{decimal.Parse(xRow["Agua"].ToString())}");
                        tmpList.Add($"Drenaje;{decimal.Parse(xRow["Dren"].ToString())}");
                        tmpList.Add($"Saneamiento;{decimal.Parse(xRow["Sane"].ToString())}");
                        tmpList.Add($"Rezago Agua;{decimal.Parse(xRow["Rez_Agua"].ToString())}");
                        tmpList.Add($"Rezago Drenaje;{decimal.Parse(xRow["Rez_Dren"].ToString())}");
                        tmpList.Add($"Rezago Saneamiento;{decimal.Parse(xRow["Rez_Sane"].ToString())}");
                        tmpList.Add($"Recargos;{decimal.Parse(xRow["Recargos"].ToString())}");
                        tmpList.Add($"Conexion;{decimal.Parse(xRow["Conexion"].ToString())}");
                        tmpList.Add($"Reconexion;{decimal.Parse(xRow["Reconec"].ToString())}");
                        tmpList.Add($"Otros;{decimal.Parse(xRow["Otros"].ToString())}");
                        response.Conceptos = tmpList.ToArray<string>();

                        //****** Obtener descuentos por tarifas
                        var xList1 = new List<DescuentosResumenItem>();
                        foreach(DataRow dataRow in tmpDataSet.Tables[1].Rows) {
                            xList1.Add(
                                new DescuentosResumenItem {
                                    Id = int.Parse(dataRow["id_tarifa"].ToString()),
                                    Descripcion = dataRow["descrip"].ToString(),
                                    Total = decimal.Parse(dataRow["total"].ToString()),
                                    NTotal = int.Parse(dataRow["c"].ToString())
                                }
                            );
                        }
                        response.Tarifas = xList1.ToArray<DescuentosResumenItem>();

                        //****** Obtener descuentos por calculos
                        var xList2 = new List<DescuentosResumenItem>();
                        foreach(DataRow dataRow in tmpDataSet.Tables[2].Rows) {
                            xList2.Add(
                                new DescuentosResumenItem {
                                    Id = int.Parse(dataRow["id_calculo"].ToString()),
                                    Descripcion = dataRow["descripcion"].ToString(),
                                    Total = decimal.Parse(dataRow["total"].ToString()),
                                    NTotal = int.Parse(dataRow["c"].ToString())
                                }
                            );
                        }
                        response.Calculos = xList2.ToArray<DescuentosResumenItem>();

                    }
                }
                return response;
            }catch(Exception err){
                logger.LogError(err, "(-) Error al obtener resumen de descuentos de  {Oficina} del {Desde} al {Hasta}", enlace.Nombre, dateRange.Desde_ISO, dateRange.Hasta_ISO );
                return null;
            }
        }
        
        // Todo: Extraer poblacion de la clase a un Factory
        public DescuentosTotal ObtenerDescuentosTotal(IEnlace enlace, DateRange dateRange) {
            var response = new DescuentosTotal(){
                Enlace = enlace 
            };
            logger.LogInformation("(-) Obteniendo descuentos totales oficina:{Oficina} del {Desde} al {Hasta}", enlace.Nombre, dateRange.Desde_ISO, dateRange.Hasta_ISO);
            try {
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString())) {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand(){
                        Connection = sqlConnection,
                        CommandText = string.Format( StoreProceduresDescuentos.DESCUENTOS_TOTAL, dateRange.Desde_ISO, dateRange.Hasta_ISO )
                    };
                    using var sqlDataReader = sqlCommand.ExecuteReader();
                    if(sqlDataReader.Read()) {
                        response.Conc_Con_Iva = ConvertUtils.ParseDecimal( sqlDataReader["Conc_Con_Iva"].ToString() );
                        response.Iva = ConvertUtils.ParseDecimal( sqlDataReader["Iva"].ToString() );
                        response.Aplicado_Con_Iva = ConvertUtils.ParseDecimal( sqlDataReader["Aplicado_Con_Iva"].ToString() );
                        response.Conc_Sin_Iva = ConvertUtils.ParseDecimal( sqlDataReader["Conc_Sin_Iva"].ToString() );
                        response.Total_Aplicado = ConvertUtils.ParseDecimal( sqlDataReader["Total_Aplicado"].ToString() );
                        response.Usuarios = ConvertUtils.ParseInteger( sqlDataReader["Usuarios"].ToString() );
                    }
                    
                }
                response.Estatus = Data.Contracts.ResumenOficinaEstatus.Completado;
            }
            catch(Exception err) {
                response.Estatus = Data.Contracts.ResumenOficinaEstatus.Error;
                logger.LogError(err, "(-) Error al obtner descuentos totales oficina:{Oficina} del {Desde} al {Hasta}", enlace.Nombre, dateRange.Desde_ISO, dateRange.Hasta_ISO);
            }
            return response;
        }

        // Todo: Extrar poblacion de la clase a un Factory
        public IEnumerable<DescuentosConcepto> ObtenerDescuentosPorConceptos(IEnlace enlace, DateRange dateRange) {
            var respuesta = new List<DescuentosConcepto>();
            logger.LogInformation("(-) Obteniendo descuentos por conceptos oficina:{Oficina} del {Desde} al {Hasta}", enlace.Nombre, dateRange.Desde_ISO, dateRange.Hasta_ISO);
            try{
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString() )) {
                    sqlConnection.Open();

                    using var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = string.Format(StoreProceduresDescuentos.DESCUENTOS_POR_CONCEPTOS, dateRange.Desde_ISO, dateRange.Hasta_ISO);

                    using var sqlDataReader = sqlCommand.ExecuteReader(); 
                    while( sqlDataReader.Read() ) {
                        var descuentoItem = new DescuentosConcepto();
                        descuentoItem.Id_Concepto = int.Parse(sqlDataReader.GetValue("Id_Concepto").ToString());
                        descuentoItem.Descripcion = sqlDataReader.GetValue("Descripcion").ToString();
                        descuentoItem.Conc_Con_Iva = decimal.Parse(sqlDataReader.GetValue("Conc_Con_Iva").ToString());
                        descuentoItem.Iva = decimal.Parse(sqlDataReader.GetValue("Iva").ToString());
                        descuentoItem.Aplicado_Con_Iva = decimal.Parse(sqlDataReader.GetValue("Aplicado_Con_Iva").ToString());
                        descuentoItem.Conc_Sin_Iva = decimal.Parse(sqlDataReader.GetValue("Conc_Sin_Iva").ToString());
                        descuentoItem.Total_Aplicado = decimal.Parse(sqlDataReader.GetValue("Total_Aplicado").ToString());
                        descuentoItem.Usuarios = int.Parse(sqlDataReader.GetValue("Usuarios").ToString());
                        respuesta.Add(descuentoItem);
                    }

                }
                return respuesta.ToArray();
            }catch(Exception err){
                logger.LogError(err, "(-) Error al obtner descuentos por conceptos oficina:{Oficina} del {Desde} al {Hasta}", enlace.Nombre, dateRange.Desde_ISO, dateRange.Hasta_ISO);
                return Array.Empty<DescuentosConcepto>();
            }
        }
        
        // Todo: Extraer poblacion de la clase a un Factory
        public IEnumerable<DescuentosUsuarioAutorizo> ObtenerDescuentosPorUsuarioAutorizo(IEnlace enalce, DateRange dateRange) {
            var respuesta = new List<DescuentosUsuarioAutorizo>();
            using var sqlConnection = new SqlConnection(enalce.GetConnectionString() );
            sqlConnection.Open();

            using var sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = string.Format( StoreProceduresDescuentos.DESCUENTOS_POR_USUARIO_AUTORIZO, dateRange.Desde_ISO, dateRange.Hasta_ISO );

            using var xReader = sqlCommand.ExecuteReader();
            while(xReader.Read()) {
                respuesta.Add(new DescuentosUsuarioAutorizo {
                    CVE = xReader.GetString("CVE"),
                    Autorizo = xReader.GetString("AUTORIZO").ToString(),
                    Subtotal = decimal.Parse(xReader.GetValue("SUBTOTAL").ToString()),
                    Iva = decimal.Parse(xReader.GetValue("IVA").ToString()),
                    Total = decimal.Parse(xReader.GetValue("TOTAL").ToString()),
                    Usuarios = int.Parse(xReader.GetValue("USUARIOS").ToString())
                });
            }

            return respuesta.ToArray();
        }
        
        // Todo: Extraer poblacion de la clase a un Factory
        public IEnumerable<DescuentosUsuarioAutorizoDetalle> ObtenerDetalleDescuentosPorUsuarioAutorizo(IEnlace enlace, DateRange dateRange, string usuarioId) {
            var response = new List<DescuentosUsuarioAutorizoDetalle>();
            using(var sqlConnection = new SqlConnection(enlace.GetConnectionString() )) {
                sqlConnection.Open();
                using(var sqlCommand = new SqlCommand()) {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = string.Format( StoreProceduresDescuentos.DESCUENTOS_POR_USUARIO_AUTORIZO_DETALLE, dateRange.Desde_ISO, dateRange.Hasta_ISO, usuarioId );
                    using(var xReader = sqlCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            response.Add(new DescuentosUsuarioAutorizoDetalle {
                                Id_Abono = xReader.GetValue("id_abono").ToString(),
                                Cuenta = long.Parse(xReader.GetValue("CUENTA").ToString()),
                                Usuarios = xReader.GetValue("USUARIO").ToString(),
                                Colonia = "",
                                Tipo_Usuario = "",
                                Fecha = DateTime.Parse(xReader.GetValue("FECHA").ToString()),
                                Cve = xReader.GetValue("CVE").ToString(),
                                Autorizo = xReader.GetValue("AUTORIZO").ToString(),
                                Justifica = xReader.GetValue("Justifica").ToString(),
                                Agua = 0m,
                                Drenaje = 0m,
                                Saneamiento = 0m,
                                Rez_Agua = 0m,
                                Rez_Drenaje = 0m,
                                Rez_Saneamiento = 0m,
                                Otros = 0m,
                                Recargos = 0m,
                                Subtotal = decimal.Parse(xReader.GetValue("SUBTOTAL").ToString()),
                                Iva = decimal.Parse(xReader.GetValue("IVA").ToString()),
                                Total = decimal.Parse(xReader.GetValue("TOTAL").ToString())
                            });
                        }
                    }
                }
            }
            return response.ToArray();
        }

        public IEnumerable<DescuentosConcepto> ObtenerDescuentosConceptosDetalle(IEnlace enlace, string cveAbono){
            var response = new List<DescuentosConcepto>();
            try {
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString() )) {
                    sqlConnection.Open();

                    using var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = string.Format( StoreProceduresDescuentos.DESCUENTOS_DETALLE_CONCEPTOS, cveAbono );
                    
                    using var sqlDataReader = sqlCommand.ExecuteReader();
                    while(sqlDataReader.Read()) {
                        response.Add(new DescuentosConcepto {
                            Id_Concepto = ConvertUtils.ParseInteger( sqlDataReader.GetValue("id_concepto").ToString() ),
                            Descripcion = sqlDataReader.GetValue("descripcion").ToString(),
                            Conc_Sin_Iva = ConvertUtils.ParseDecimal( sqlDataReader.GetValue("subtotal").ToString() ),
                            Iva = ConvertUtils.ParseDecimal(  sqlDataReader.GetValue("Iva").ToString() ),
                            Total_Aplicado = ConvertUtils.ParseDecimal(  sqlDataReader.GetValue("total").ToString() )
                        });
                    }
                }
                return response;
            }
            catch(Exception err) {
                logger.LogError(err, "(-) Error al obtener detalle conceptos del abono:{Abono} oficina:{Oficina}", cveAbono, enlace.Nombre );
                return Array.Empty<DescuentosConcepto>();
            }
        }
        public IEnumerable<DescuentosClave> ObtenerDescuentosClavesDetalle(IEnlace enlace, string cveAbono){
            var response = new List<DescuentosClave>();
            try {
                using(var sqlConnection = new SqlConnection(enlace.GetConnectionString() )) {
                    sqlConnection.Open();
                    using var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = string.Format( StoreProceduresDescuentos.DESCUENTOS_DETALLE_CLAVES, cveAbono );
                    
                    using var sqlDataReader = sqlCommand.ExecuteReader();
                    while(sqlDataReader.Read()) {
                        response.Add(new DescuentosClave {
                            Cve_Descuento = sqlDataReader.GetValue("Cve_Descuento").ToString(),
                            Conc_Sin_Iva = ConvertUtils.ParseDecimal( sqlDataReader["Subtotal"].ToString() ),
                            Iva = ConvertUtils.ParseDecimal( sqlDataReader["Iva"].ToString() ),
                            Total = ConvertUtils.ParseDecimal( sqlDataReader["Total"].ToString() )
                        });
                    }

                }
                return response;
            }
            catch(Exception err) {
                logger.LogError(err, "(-) Error al obtener detalle claves del abono:{Abono} oficina:{Oficina}", cveAbono, enlace.Nombre);
                Console.WriteLine(err.StackTrace);
                return Array.Empty<DescuentosClave>();
            }
        }
        
    }
}
