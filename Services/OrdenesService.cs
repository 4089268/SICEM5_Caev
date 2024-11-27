using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Services {
    public class OrdenesService {
        private readonly IConfiguration appSettings;
        private readonly SicemService sicemService;
        public OrdenesService(IConfiguration c, SicemService s) {
            this.appSettings = c;
            this.sicemService = s;
        }

        public Ordenes_Resumen ObtenerOrdenesResumen(int Id_Oficina, string Fecha1, string Fecha2, int Sb, int Sect) {
            var respuesta = new Ordenes_Resumen();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [Sicem].[OrdenesTrabajos] @cAlias='RESUMEN', @xfec1='{Fecha1}', @xfec2='{Fecha2}', @nSub={Sb}, @nSec={Sect}";
                    var xDataSet = new DataSet();
                    var xDataAdapter = new SqlDataAdapter(xCommand);
                    xDataAdapter.Fill(xDataSet);
                    if(xDataSet.Tables.Count != 3){
                        throw new Exception("Error al hacer la consulta a la base de datos.");
                    }
                    
                    // ****** Obtener Totales ******
                    DataRow xRow = xDataSet.Tables[0].Rows[0];
                    respuesta.Pendi = int.Parse(xRow["pendi"].ToString());
                    respuesta.Eneje = int.Parse(xRow["eneje"].ToString());
                    respuesta.Reali = int.Parse(xRow["reali"].ToString());
                    respuesta.Cance = int.Parse(xRow["Cance"].ToString());
                    respuesta.Eje = int.Parse(xRow["eje"].ToString());
                    respuesta.No_eje = int.Parse(xRow["no_eje"].ToString());
                    respuesta.Total = int.Parse(xRow["total"].ToString());

                    // ****** Obtener Trabajos ******
                    var tmpList = new List<Ordenes_Resumen_Trabajo>();
                    foreach(DataRow xRow1 in xDataSet.Tables[1].Rows) {
                        tmpList.Add(new Ordenes_Resumen_Trabajo {
                            Id = int.Parse(xRow1["id_trabajo"].ToString()),
                            Descripcion = xRow1["descripcion"].ToString(),
                            Pendi = int.Parse(xRow1["pendi"].ToString()),
                            Eneje = int.Parse(xRow1["eneje"].ToString()),
                            Reali = int.Parse(xRow1["reali"].ToString()),
                            Cance = int.Parse(xRow1["cance"].ToString()),
                            Eje = int.Parse(xRow1["eje"].ToString()),
                            No_eje = int.Parse(xRow1["no_eje"].ToString()),
                            Total = int.Parse(xRow1["total"].ToString())
                        });
                    }
                    respuesta.Trabajos = tmpList.ToArray<Ordenes_Resumen_Trabajo>();

                    // ****** Obtener Departamentos ******
                    var tmpList2 = new List<Ordenes_Resumen_Departamento>();
                    foreach(DataRow xRow1 in xDataSet.Tables[2].Rows) {
                        tmpList2.Add(new Ordenes_Resumen_Departamento {
                            Id = int.Parse(xRow1["id_departamento"].ToString()),
                            Descripcion = xRow1["descripcion"].ToString(),
                            Total = int.Parse(xRow1["total"].ToString())
                        });
                    }
                    respuesta.Departamentos = tmpList2.ToArray<Ordenes_Resumen_Departamento>();
                }
            }
            return respuesta;
        }
        public Ordenes_Oficina[] ObtenerOrdenesPorOficinas(Ruta[] oficinas, string Fecha1, string Fecha2, int sb, int sect, bool agregarTotal = true) {
            var response = new List<Ordenes_Oficina>();

            foreach(var ofi in oficinas) {
                using(var xConnecton = new SqlConnection(ofi.StringConection)) {
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = $"EXEC [Sicem].[OrdenesTrabajos] @cAlias='TOTALES', @xfec1='{Fecha1}', @xfec2='{Fecha2}' ";
                        using(var xReader = xCommand.ExecuteReader()){
                            if(xReader.Read()){
                                var newItem = new Ordenes_Oficina();
                                newItem.IdOficina = ofi.Id;
                                newItem.Oficina = ofi.Oficina;
                                var tmpInteger = 0;
                                newItem.Pendi = int.TryParse(xReader["pendi"].ToString(), out tmpInteger)?tmpInteger:0;
                                newItem.Eneje = int.TryParse(xReader["eneje"].ToString(), out tmpInteger)?tmpInteger:0;
                                newItem.Reali = int.TryParse(xReader["reali"].ToString(), out tmpInteger)?tmpInteger:0;
                                newItem.Cance = int.TryParse(xReader["cance"].ToString(), out tmpInteger)?tmpInteger:0;
                                newItem.Eje = int.TryParse(xReader["eje"].ToString(), out tmpInteger) ? tmpInteger:0;
                                newItem.No_eje = int.TryParse(xReader["no_eje"].ToString(), out tmpInteger)?tmpInteger:0;
                                newItem.Total = int.TryParse(xReader["total"].ToString(), out tmpInteger)?tmpInteger:0;
                                response.Add(newItem);
                            }
                        }
                    }
                }
            }

            if(agregarTotal && response.Count > 1) {
                var tmpItemTotal = new Ordenes_Oficina();
                tmpItemTotal.IdOficina = 0;
                tmpItemTotal.Oficina = " TOTAL";
                tmpItemTotal.Pendi = response.Sum(item => item.Pendi);
                tmpItemTotal.Eneje = response.Sum(item => item.Eneje);
                tmpItemTotal.Reali = response.Sum(item => item.Reali);
                tmpItemTotal.Cance = response.Sum(item => item.Cance);
                tmpItemTotal.Eje = response.Sum(item => item.Eje);
                tmpItemTotal.No_eje = response.Sum(item => item.No_eje);
                tmpItemTotal.Total = response.Sum(item => item.Total);
                response.Add(tmpItemTotal);
            }
            return response.ToArray();
        }
        public Ordenes_Oficina ObtenerOrdenesPorOficina(IEnlace oficina, string Fecha1, string Fecha2, int sb, int sect) {
            var response = new Ordenes_Oficina();
            response.IdOficina = oficina.Id;
            response.Oficina = oficina.Nombre;
            try {
                using(var xConnecton = new SqlConnection(oficina.GetConnectionString())) {
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = $"EXEC [Sicem].[OrdenesTrabajos] @cAlias='TOTALES', @xfec1='{Fecha1}', @xfec2='{Fecha2}' ";
                        using(var xReader = xCommand.ExecuteReader()) {
                            if(xReader.Read()) {
                                var tmpInteger = 0;
                                response.Pendi = int.TryParse(xReader["pendi"].ToString(), out tmpInteger) ? tmpInteger : 0;
                                response.Eneje = int.TryParse(xReader["eneje"].ToString(), out tmpInteger) ? tmpInteger : 0;
                                response.Reali = int.TryParse(xReader["reali"].ToString(), out tmpInteger) ? tmpInteger : 0;
                                response.Cance = int.TryParse(xReader["cance"].ToString(), out tmpInteger) ? tmpInteger : 0;
                                response.Eje = int.TryParse(xReader["eje"].ToString(), out tmpInteger) ? tmpInteger : 0;
                                response.No_eje = int.TryParse(xReader["no_eje"].ToString(), out tmpInteger) ? tmpInteger : 0;
                                response.Total = int.TryParse(xReader["total"].ToString(), out tmpInteger) ? tmpInteger : 0;
                            }
                        }
                    }
                }
                response.Estatus = 1;
            }
            catch(Exception err) {
                Console.WriteLine($">> Error al procesar la consulta oficina: {oficina.Nombre}\n\tErr:{err.Message}\n\tStack:{err.StackTrace}");
                response.Estatus = 2;
            }
            return response;
        }

        public Ordenes_Detalle[] ObtenerOrdenesDetalle(int Id_Oficina, string Fecha1, string Fecha2, int Sb, int Sect, string Filtro) {
            var respuesta = new List<Ordenes_Detalle>();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [Sicem].[OrdenesTrabajos] @cAlias='DETALLE_ORDENES', @xfec1='{Fecha1}', @xfec2='{Fecha2}', @nSub={Sb}, @nSec={Sect}, @cFiltro = '{Filtro}'";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            respuesta.Add(new Ordenes_Detalle {
                                Orden = xReader["id_orden"].ToString(),
                                Cuenta = long.Parse(xReader["id_cuenta"].ToString()),
                                Nombre = xReader["nombre"].ToString(),
                                Id_Trabajo = int.Parse(xReader["id_trabajo"].ToString()),
                                Trabajo = xReader["trabajo"].ToString(),
                                Fecha_Genero = DateTime.Parse(xReader["fecha_genero"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                                Fecha_Realizo = (xReader.IsDBNull("fecha_realizo") ? "" : DateTime.Parse(xReader["fecha_realizo"].ToString()).ToString("dd/MM/yyyy HH:mm:ss")),
                                Id_Estatus = int.Parse(xReader["id_estatus"].ToString()),
                                Estatus = xReader["estatus"].ToString(),
                                Colonia = xReader["colonia"].ToString(),
                                Id_Colonia = int.Parse(xReader["id_colonia"].ToString()),
                                Tarifa = xReader["tarifa"].ToString(),
                                Id_Tarifa = int.Parse(xReader["id_tarifa"].ToString()),
                                Id_Genero = xReader["id_genero"].ToString(),
                                Genero = xReader["genero"].ToString(),
                                Motivo = xReader["motivo"].ToString(),
                                Resultado = xReader["resultado"].ToString(),
                                Id_Departamento = 0,
                                Departamento = "indefinido",
                                Id_Realizo = int.Parse(xReader["id_realizo"].ToString()),
                                Realizo = xReader["realizo"].ToString()
                            });
                        }
                    }
                }
            }
            return respuesta.ToArray();
        }
        public Ordenes_Agrupado[] ObtenerOrdenesColonias(int Id_Oficina, string Fecha1, string Fecha2, int Sb, int Sect, string Filtro) {
            var respuesta = new List<Ordenes_Agrupado>();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [Sicem].[OrdenesTrabajos] @cAlias='POR_COLONIAS', @xfec1='{Fecha1}', @xfec2='{Fecha2}', @nSub={Sb}, @nSec={Sect}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            respuesta.Add(new Ordenes_Agrupado {
                                Id = int.Parse(xReader["id_colonia"].ToString()),
                                Descripcion = xReader["colonia"].ToString(),
                                Pendientes = int.Parse(xReader["pendi"].ToString()),
                                En_Ejecucion = int.Parse(xReader["eneje"].ToString()),
                                Realizadas = int.Parse(xReader["reali"].ToString()),
                                Canceladas = int.Parse(xReader["cance"].ToString()),
                                Ejecutadas = int.Parse(xReader["eje"].ToString()),
                                No_Ejecutadas = int.Parse(xReader["no_eje"].ToString()),
                                Total = int.Parse(xReader["total"].ToString())
                            });
                        }
                    }
                }
            }
            return respuesta.ToArray();
        }
        public Ordenes_Agrupado[] ObtenerOrdenesTrabajos(int Id_Oficina, string Fecha1, string Fecha2, int Sb, int Sect) {
            var respuesta = new List<Ordenes_Agrupado>();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();            
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [Sicem].[OrdenesTrabajos] @cAlias='POR_TRABAJOS', @xfec1='{Fecha1}', @xfec2='{Fecha2}', @nSub={Sb}, @nSec={Sect}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            respuesta.Add(new Ordenes_Agrupado {
                                Id = int.Parse(xReader["id_trabajo"].ToString()),
                                Descripcion = xReader["trabajo"].ToString(),
                                Pendientes = int.Parse(xReader["pendi"].ToString()),
                                En_Ejecucion = int.Parse(xReader["eneje"].ToString()),
                                Realizadas = int.Parse(xReader["reali"].ToString()),
                                Canceladas = int.Parse(xReader["cance"].ToString()),
                                Ejecutadas = int.Parse(xReader["eje"].ToString()),
                                No_Ejecutadas = int.Parse(xReader["no_eje"].ToString()),
                                Total = int.Parse(xReader["total"].ToString())
                            });
                        }
                    }
                }
            }
            return respuesta.ToArray();
        }
        public Ordenes_Realizacion[] ObtenerOrdenesRealizacion(int Id_Oficina, string Fecha1, string Fecha2, int Sb, int Sect, string Filtro) {
            var respuesta = new List<Ordenes_Realizacion>();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [Sicem].[OrdenesTrabajos] @cAlias='POR_REALIZACION', @xfec1='{Fecha1}', @xfec2='{Fecha2}', @nSub={Sb}, @nSec={Sect}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            respuesta.Add(new Ordenes_Realizacion {
                                Id = int.Parse(xReader["id_realizo"].ToString()),
                                Descripcion = xReader["realizo"].ToString(),
                                Ejecutadas = int.Parse(xReader["eje"].ToString()),
                                No_Ejecutadas = int.Parse(xReader["no_eje"].ToString()),
                                Eje_0_3 = int.Parse(xReader["eje_0_3"].ToString()),
                                Eje_4_6 = int.Parse(xReader["eje_4_6"].ToString()),
                                Eje_7_9 = int.Parse(xReader["eje_7_9"].ToString()),
                                Eje_10 = int.Parse(xReader["eje_10"].ToString()),
                                Total = int.Parse(xReader["total"].ToString())
                            });
                        }
                    }
                }
            }
            return respuesta.ToArray();
        }
        public Ordenes_Capturacion[] ObtenerOrdenesCapturacion(int Id_Oficina, string Fecha1, string Fecha2, int Sb, int Sect, string Filtro){
            var respuesta = new List<Ordenes_Capturacion>();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [Sicem].[OrdenesTrabajos] @cAlias='POR_CAPTURACION', @xfec1='{Fecha1}', @xfec2='{Fecha2}', @nSub={Sb}, @nSec={Sect}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            respuesta.Add(new Ordenes_Capturacion {
                                Id = xReader["id_capturo"].ToString(),
                                Descripcion = xReader["nombre"].ToString(),
                                Pendientes = int.Parse(xReader["pendi"].ToString()),
                                En_Ejecucion = int.Parse(xReader["eneje"].ToString()),
                                Realizadas = int.Parse(xReader["reali"].ToString()),
                                Canceladas = int.Parse(xReader["cance"].ToString()),
                                Ejecutadas = int.Parse(xReader["eje"].ToString()),
                                No_Ejecutadas = int.Parse(xReader["no_eje"].ToString()),
                                Total = int.Parse(xReader["total"].ToString())
                            });
                        }
                    }
                }
            }
            return respuesta.ToArray();
        }


        //****** Funciones Estaticas ******
        public static string GenerarFiltro_Detalle( int isEstatus = 0, int ejecutada = -1, int colonia = -1, int trabajo = -1, int realizo = -1, string capturo = ""){
            var xmlFiltro = new System.Text.StringBuilder();
            xmlFiltro.Append("<filtro>");
            xmlFiltro.Append($"<id_estatus>{isEstatus}</id_estatus>");
            xmlFiltro.Append($"<ejecutada>{ejecutada}</ejecutada>");
            xmlFiltro.Append($"<colonia>{colonia}</colonia>");
            xmlFiltro.Append($"<id_trabajo>{trabajo}</id_trabajo>");
            xmlFiltro.Append($"<id_realizo>{realizo}</id_realizo>");
            xmlFiltro.Append($"<id_capturo>{capturo}</id_capturo>");
            xmlFiltro.Append("</filtro>");
            return xmlFiltro.ToString();
        }

    }
}