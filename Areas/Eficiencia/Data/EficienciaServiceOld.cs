using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.Models;
using Sicem_Blazor.Descuentos.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Eficiencia.Models;
using Sicem_Blazor.Services;

namespace Sicem_Blazor.Eficiencia.Data{
    public class EficienciaService2: IEficienciaService{
        private readonly IConfiguration appSettings;
        private readonly SicemService sicemService;
        public EficienciaService2(IConfiguration c, SicemService s){
            this.appSettings = c;
            this.sicemService = s;
        }

        public EficienciaResumen ObtenerEficienciaResumen(int Id_Oficina, int Ano, int Mes, int Sb, int Sect) {
            // var respuesta = new Eficiencia_Resumen();            
            // var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            // using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
            //     xConnecton.Open();

            //     using(var xCommand = new SqlCommand()) {
            //         xCommand.Connection = xConnecton;
            //         xCommand.CommandText = $"Exec [Sicem].[Eficiencia] 'EFI-MENSUAL',{Sb},{Sect},{Ano},{Mes}";
            //         var xDataSet = new DataSet();
            //         var xDataAdapter = new SqlDataAdapter(xCommand);
            //         xDataAdapter.Fill(xDataSet);

            //         if(xDataSet.Tables.Count != 3){
            //             throw new Exception("Error al hacer la consulta a la base de datos.");
            //         }

            //         //**** Importes
            //         var tmpImportes = new List<Eficiencia_Mensual>();
            //         foreach(DataRow xRow in xDataSet.Tables[0].Rows) {
            //             tmpImportes.Add(new Eficiencia_Mensual {
            //                 Tipo_Usuario = xRow["tipo_usuario"].ToString(),
            //                 Facturacion = decimal.Parse(xRow["facturacion"].ToString()),
            //                 Por_Fact = decimal.Parse(xRow["por_fac"].ToString()),
            //                 Cobrado = decimal.Parse(xRow["cobrado"].ToString()),
            //                 Por_Cob = decimal.Parse(xRow["por_cob"].ToString()),
            //                 Descontado = decimal.Parse(xRow["descontado"].ToString()),
            //                 Por_Desc = decimal.Parse(xRow["por_des"].ToString()),
            //                 Anticipado = decimal.Parse(xRow["anticipado"].ToString()),
            //                 Por_Ant = decimal.Parse(xRow["por_ant"].ToString()),
            //                 Por_Efi = decimal.Parse(xRow["por_efi"].ToString()),
            //                 Por_Inef = decimal.Parse(xRow["por_ineficiencia"].ToString())
            //             });
            //         }
            //         respuesta.Importes = tmpImportes.ToArray();


            //         //**** Metros Cubicos
            //         var tmpMetros = new List<Eficiencia_Mensual>();
            //         foreach(DataRow xRow in xDataSet.Tables[1].Rows) {
            //             tmpMetros.Add(new Eficiencia_Mensual {
            //                 Tipo_Usuario = xRow["tipo_usuario"].ToString(),
            //                 Facturacion = decimal.Parse(xRow["facturacion"].ToString()),
            //                 Por_Fact = decimal.Parse(xRow["por_fac"].ToString()),
            //                 Cobrado = decimal.Parse(xRow["cobrado"].ToString()),
            //                 Por_Cob = decimal.Parse(xRow["por_cob"].ToString()),
            //                 Descontado = decimal.Parse(xRow["descontado"].ToString()),
            //                 Por_Desc = decimal.Parse(xRow["por_des"].ToString()),
            //                 Anticipado = decimal.Parse(xRow["anticipado"].ToString()),
            //                 Por_Ant = decimal.Parse(xRow["por_ant"].ToString()),
            //                 Por_Efi = decimal.Parse(xRow["por_efi"].ToString()),
            //                 Por_Inef = decimal.Parse(xRow["por_ineficiencia"].ToString())
            //             });
            //         }
            //         respuesta.Metros = tmpMetros.ToArray();


            //         //**** Usuarios Cubicos
            //         var tmpUsuarios = new List<Eficiencia_Mensual>();
            //         foreach(DataRow xRow in xDataSet.Tables[2].Rows) {
            //             tmpUsuarios.Add(new Eficiencia_Mensual {
            //                 Tipo_Usuario = xRow["tipo_usuario"].ToString(),
            //                 Facturacion = decimal.Parse(xRow["facturacion"].ToString()),
            //                 Por_Fact = decimal.Parse(xRow["por_fac"].ToString()),
            //                 Cobrado = decimal.Parse(xRow["cobrado"].ToString()),
            //                 Por_Cob = decimal.Parse(xRow["por_cob"].ToString()),
            //                 Descontado = decimal.Parse(xRow["descontado"].ToString()),
            //                 Por_Desc = decimal.Parse(xRow["por_des"].ToString()),
            //                 Anticipado = decimal.Parse(xRow["anticipado"].ToString()),
            //                 Por_Ant = decimal.Parse(xRow["por_ant"].ToString()),
            //                 Por_Efi = decimal.Parse(xRow["por_efi"].ToString()),
            //                 Por_Inef = decimal.Parse(xRow["por_ineficiencia"].ToString())
            //             });
            //         }
            //         respuesta.Usuarios = tmpUsuarios.ToArray();
            //     }

            //     using(var xCommand = new SqlCommand()) {
            //         xCommand.Connection = xConnecton;
            //         xCommand.CommandText = $"Exec [Sicem].[Eficiencia] 'EFI-HIS-TOTALES',{Sb},{Sect},{Ano},{Mes}";
            //         var xDataSet = new DataSet();
            //         var xDataAdapter = new SqlDataAdapter(xCommand);
            //         xDataAdapter.Fill(xDataSet);

            //         if(xDataSet.Tables.Count != 3) {
            //             throw new Exception("Error al hacer la consulta a la base de datos.");
            //         }

            //         //**** Importes
            //         var tmpImportes = new List<string>();
            //         foreach(DataRow xRow in xDataSet.Tables[0].Rows) {
            //             tmpImportes.Add($"{xRow["periodo"].ToString().ToUpper()};{decimal.Parse(xRow["por_efi"].ToString())};{decimal.Parse(xRow["por_ine"].ToString())}");
            //         }
            //         respuesta.HistorialEfi_Importes = tmpImportes.ToArray();


            //         //**** Metros Cubicos
            //         var tmpMetros = new List<string>();
            //         foreach(DataRow xRow in xDataSet.Tables[1].Rows) {
            //             tmpMetros.Add($"{xRow["periodo"].ToString().ToUpper()};{decimal.Parse(xRow["por_efi"].ToString())};{decimal.Parse(xRow["por_ine"].ToString())}");
            //         }
            //         respuesta.HistorialEfi_Metros = tmpMetros.ToArray();


            //         //**** Usuarios Cubicos
            //         var tmpUsuarios = new List<string>();
            //         foreach(DataRow xRow in xDataSet.Tables[2].Rows) {
            //             tmpUsuarios.Add($"{xRow["periodo"].ToString().ToUpper()};{decimal.Parse(xRow["por_efi"].ToString())};{decimal.Parse(xRow["por_ine"].ToString())}");
            //         }
            //         respuesta.HistorialEfi_Usuarios = tmpUsuarios.ToArray();
            //     }
            // }
            // return respuesta;
            throw new NotImplementedException();
        }
        public dynamic[] ObtenerEficienciaPorOficinas(Ruta[] oficinas, DateTime fecha1, DateTime fecha2, bool agregarTotal = true){
            var response = new List<DescuentosTotal>();
            foreach(var ofi in oficinas) {
                var tmpItem = new DescuentosTotal();
                tmpItem.Enlace = ofi;
                try {
                    using(var xConnecton = new SqlConnection(ofi.StringConection)) {
                        xConnecton.Open();
                        var xCommand = new SqlCommand();
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = string.Format("EXEC [SICEM].[Descuentos] 'SOLO_TOTAL','{0}','{1}'", fecha1.ToString("yyyyMMdd"), fecha2.ToString("yyyyMMdd"));
                        using(var xReader = xCommand.ExecuteReader()) {
                            if(xReader.Read()) {
                                decimal tmpdata = 0m;
                                tmpItem.Conc_Con_Iva = decimal.TryParse(xReader["Conc_Con_Iva"].ToString(), out tmpdata) ? tmpdata : 0m;
                                tmpItem.Iva = decimal.TryParse(xReader["Iva"].ToString(), out tmpdata) ? tmpdata : 0m;
                                tmpItem.Aplicado_Con_Iva = decimal.TryParse(xReader["Aplicado_Con_Iva"].ToString(), out tmpdata) ? tmpdata : 0m;
                                tmpItem.Conc_Sin_Iva = decimal.TryParse(xReader["Conc_Sin_Iva"].ToString(), out tmpdata) ? tmpdata : 0m;
                                tmpItem.Total_Aplicado = decimal.TryParse(xReader["Total_Aplicado"].ToString(), out tmpdata) ? tmpdata : 0m;
                                tmpItem.Usuarios = int.TryParse(xReader["Usuarios"].ToString(), out int tmpData2) ? tmpData2 : 0;
                            }
                        }
                    }
                }
                catch(Exception err) {
                    Console.WriteLine($">> Error al obtener descuentos oficina {ofi.Id} {ofi.Oficina}\n\t{err.Message}");
                }
                response.Add(tmpItem);
            }
            if(agregarTotal && response.Count > 1) {
                var tmpItemTotal = new DescuentosTotal();
                tmpItemTotal.Conc_Con_Iva = response.Sum(item => item.Conc_Con_Iva);
                tmpItemTotal.Iva = response.Sum(item => item.Iva);
                tmpItemTotal.Aplicado_Con_Iva = response.Sum(item => item.Aplicado_Con_Iva);
                tmpItemTotal.Conc_Sin_Iva = response.Sum(item => item.Conc_Sin_Iva);
                tmpItemTotal.Total_Aplicado = response.Sum(item => item.Total_Aplicado);
                tmpItemTotal.Usuarios = response.Sum(item => item.Usuarios);
                response.Add(tmpItemTotal);
            }
            return response.ToArray();
        }
        public Eficiencia_Sectores_Resp ObtenerEficienciaSectores(int Id_Oficina, int Ano, int Mes, int Sb) {
            var respuesta = new Eficiencia_Sectores_Resp();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"Exec [Sicem].[Eficiencia] 'EFI-MENSUAL-TOTALES', {Sb}, 0, {Ano}, {Mes}";
                    var xDataSet = new DataSet();
                    var xDataAdapter = new SqlDataAdapter(xCommand);
                    xDataAdapter.Fill(xDataSet);

                    if(xDataSet.Tables.Count != 3) {
                        throw new Exception("Error al hacer la consulta a la base de datos.");
                    }

                    //**** Importes
                    var tmpImportes = new List<Eficiencia_Sectores>();
                    foreach(DataRow xRow in xDataSet.Tables[0].Rows) {
                        tmpImportes.Add(new Eficiencia_Sectores {
                            Sector = xRow["tipo_usuario"].ToString(),
                            Facturacion = decimal.Parse(xRow["facturacion"].ToString()),
                            Por_Fact = double.Parse(xRow["por_fac"].ToString()),
                            Cobrado = decimal.Parse(xRow["cobrado"].ToString()),
                            Por_Cobrado = double.Parse(xRow["por_cob"].ToString()),
                            Descontado = decimal.Parse(xRow["descontado"].ToString()),
                            Por_Desc = double.Parse(xRow["por_des"].ToString()),
                            Anticipado = decimal.Parse(xRow["anticipado"].ToString()),
                            Por_Ant = double.Parse(xRow["por_ant"].ToString()),
                            Por_Efi = double.Parse(xRow["por_efi"].ToString()),
                            Por_Inefi = double.Parse(xRow["por_ineficiencia"].ToString())
                        });
                    }
                    respuesta.Importes = tmpImportes.ToArray();


                    //**** Metros Cubicos
                    var tmpMetros = new List<Eficiencia_Sectores>();
                    foreach(DataRow xRow in xDataSet.Tables[1].Rows) {
                        tmpMetros.Add(new Eficiencia_Sectores {
                            Sector = xRow["tipo_usuario"].ToString(),
                            Facturacion = decimal.Parse(xRow["facturacion"].ToString()),
                            Por_Fact = double.Parse(xRow["por_fac"].ToString()),
                            Cobrado = decimal.Parse(xRow["cobrado"].ToString()),
                            Por_Cobrado = double.Parse(xRow["por_cob"].ToString()),
                            Descontado = decimal.Parse(xRow["descontado"].ToString()),
                            Por_Desc = double.Parse(xRow["por_des"].ToString()),
                            Anticipado = decimal.Parse(xRow["anticipado"].ToString()),
                            Por_Ant = double.Parse(xRow["por_ant"].ToString()),
                            Por_Efi = double.Parse(xRow["por_efi"].ToString()),
                            Por_Inefi = double.Parse(xRow["por_ineficiencia"].ToString())
                        });
                    }
                    respuesta.Metros = tmpMetros.ToArray();


                    //**** Usuarios Cubicos
                    var tmpUsuarios = new List<Eficiencia_Sectores>();
                    foreach(DataRow xRow in xDataSet.Tables[2].Rows) {
                        tmpUsuarios.Add(new Eficiencia_Sectores {
                            Sector = xRow["tipo_usuario"].ToString(),
                            Facturacion = decimal.Parse(xRow["facturacion"].ToString()),
                            Por_Fact = double.Parse(xRow["por_fac"].ToString()),
                            Cobrado = decimal.Parse(xRow["cobrado"].ToString()),
                            Por_Cobrado = double.Parse(xRow["por_cob"].ToString()),
                            Descontado = decimal.Parse(xRow["descontado"].ToString()),
                            Por_Desc = double.Parse(xRow["por_des"].ToString()),
                            Anticipado = decimal.Parse(xRow["anticipado"].ToString()),
                            Por_Ant = double.Parse(xRow["por_ant"].ToString()),
                            Por_Efi = double.Parse(xRow["por_efi"].ToString()),
                            Por_Inefi = double.Parse(xRow["por_ineficiencia"].ToString())
                        });
                    }
                    respuesta.Usuarios = tmpUsuarios.ToArray();
                }
            }
            
            return respuesta;
        }
        public Eficiencia_Colonia[] ObtenerEficienciaColonias(int Id_Oficina, int Ano, int Mes, int Sb, int Sect, int Tipo) {
            var respuesta = new List<Eficiencia_Colonia>();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"Exec [Sicem].[Eficiencia] @cAlias='EFI-COLONIAS', @nSub={Sb}, @nSec={Sect}, @xAf={Ano}, @xXmf={Mes}, @xTipo={Tipo}";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            respuesta.Add(new Eficiencia_Colonia {
                                Colonia = xReader.GetValue("colonia").ToString(),
                                Id_Colonia = int.Parse(xReader.GetValue("id_colonia").ToString()),
                                Id_Localidad = int.Parse(xReader.GetValue("id_localidad").ToString()),
                                Facturado = decimal.Parse(xReader.GetValue("facturado").ToString()),
                                Por_Fact = double.Parse(xReader.GetValue("por_fact").ToString()),
                                Cobrado = decimal.Parse(xReader.GetValue("cobrado").ToString()),
                                Por_Cobrado = double.Parse(xReader.GetValue("por_cobr").ToString()),
                                Descontado = decimal.Parse(xReader.GetValue("descontado").ToString()),
                                Por_Desc = double.Parse(xReader.GetValue("por_desc").ToString()),
                                Anticipado = decimal.Parse(xReader.GetValue("Anticipado").ToString()),
                                Por_Ant = double.Parse(xReader.GetValue("por_ant").ToString()),
                                Por_Efi = double.Parse(xReader.GetValue("por_efi").ToString()),
                                Por_Inef = double.Parse(xReader.GetValue("por_inefi").ToString())
                            });
                        }
                    }
                }
            }
            return respuesta.ToArray();
        }
        public Eficiencia_Conceptos[] ObtenerEficienciaConceptos( int Id_Oficina, int Ano, int Mes, int Sb, int Sect) {
            var respuesta = new List<Eficiencia_Conceptos>();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();                
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"Exec [Sicem].[Eficiencia] @cAlias = 'EFICIENCIA-CONCEPTOS', @nSub = {Sb}, @nSec = {Sect}, @xAf = {Ano}, @xXmf = {Mes} ";
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            respuesta.Add(new Eficiencia_Conceptos {
                                Tipo_Usuario = xReader.GetValue("tipo_usuario").ToString(),
                                Facturacion = decimal.Parse(xReader.GetValue("facturacion").ToString()),
                                Cobrado = decimal.Parse(xReader.GetValue("cobrado").ToString()),
                                Por_Cob = double.Parse(xReader.GetValue("por_cob").ToString()),
                                Descontado = decimal.Parse(xReader.GetValue("descontado").ToString()),
                                Por_Des = double.Parse(xReader.GetValue("por_des").ToString()),
                                Anticipado = decimal.Parse(xReader.GetValue("anticipado").ToString()),
                                Por_Ant = double.Parse(xReader.GetValue("por_ant").ToString()),
                                Por_Efi = double.Parse(xReader.GetValue("por_efi").ToString()),
                                Por_Ineficiencia = double.Parse(xReader.GetValue("por_ineficiencia").ToString()),
                                Fac_Agua = decimal.Parse(xReader.GetValue("fac_agua").ToString()),
                                Fac_Dren = decimal.Parse(xReader.GetValue("fac_dren").ToString()),
                                Fac_Sane = decimal.Parse(xReader.GetValue("fac_sane").ToString()),
                                Cob_Agua = decimal.Parse(xReader.GetValue("cob_agua").ToString()),
                                Cob_Dren = decimal.Parse(xReader.GetValue("cob_dren").ToString()),
                                Cob_Sane = decimal.Parse(xReader.GetValue("cob_sane").ToString()),
                                Ant_Agua = decimal.Parse(xReader.GetValue("ant_agua").ToString()),
                                Ant_Dren = decimal.Parse(xReader.GetValue("ant_dren").ToString()),
                                Ant_Sane = decimal.Parse(xReader.GetValue("ant_sane").ToString()),
                                Des_Agua = decimal.Parse(xReader.GetValue("des_agua").ToString()),
                                Des_Dren = decimal.Parse(xReader.GetValue("des_dren").ToString()),
                                Des_Sane = decimal.Parse(xReader.GetValue("des_sane").ToString())
                            });
                        }
                    }
                }
            }
            return respuesta.ToArray();
        }
        public Eficiencia_Detalle[] ObtenerEficienciaDetalleSector(int Id_Oficina, int Ano, int Mes, int Sb, int Sect, int Tipo) {
            Eficiencia_Detalle[] respuesta;            
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"Exec [Sicem].[Eficiencia] 'EFI-MENSUAL-TIPO',{Sb}, {Sect}, {Ano}, {Mes}, {Tipo} ";
                    var tmpImportes = new List<Eficiencia_Detalle>();
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            tmpImportes.Add(new Eficiencia_Detalle {
                                Tipo_Usuario = xReader.GetValue("tipo_usuario").ToString(),
                                Facturacion = decimal.Parse(xReader.GetValue("facturacion").ToString()),
                                Por_Fac = 100,
                                Cobrado = decimal.Parse(xReader.GetValue("cobrado").ToString()),
                                Por_Cob = double.Parse(xReader.GetValue("por_cob").ToString()),
                                Descontado = decimal.Parse(xReader.GetValue("descontado").ToString()),
                                Por_Des = double.Parse(xReader.GetValue("por_des").ToString()),
                                Anticipado = decimal.Parse(xReader.GetValue("anticipado").ToString()),
                                Por_Ant = double.Parse(xReader.GetValue("por_ant").ToString()),
                                Por_Efi = double.Parse(xReader.GetValue("por_efi").ToString())
                            });
                        }
                    }
                    respuesta = tmpImportes.ToArray();
                }
            }
            return respuesta;
        }
        public Eficiencia_Detalle[] ObtenerEficienciaDetalleColonia(int Id_Oficina, int Ano, int Mes, int Sb, int Sect, int Tipo, int IdColonia, int IdLocalidad) {
            Eficiencia_Detalle[] respuesta;
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)){
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"Exec [Sicem].[Eficiencia] @cAlias = 'EFI-COLONIAS-DETALLE', @nSub = {Sb}, @nSec = {Sect}, @xTipo = {Tipo}, @xAf = {Ano}, @xXmf = {Mes}, @IdColonia = {IdColonia}, @IdLocalidad = {IdLocalidad}";
                    var tmpImportes = new List<Eficiencia_Detalle>();
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            tmpImportes.Add(new Eficiencia_Detalle {
                                Tipo_Usuario = xReader.GetValue("tipo_usuario").ToString(),
                                Facturacion = decimal.Parse(xReader.GetValue("facturacion").ToString()),
                                Por_Fac = 100,
                                Cobrado = decimal.Parse(xReader.GetValue("cobrado").ToString()),
                                Por_Cob = double.Parse(xReader.GetValue("por_cob").ToString()),
                                Descontado = decimal.Parse(xReader.GetValue("descontado").ToString()),
                                Por_Des = double.Parse(xReader.GetValue("por_des").ToString()),
                                Anticipado = decimal.Parse(xReader.GetValue("anticipado").ToString()),
                                Por_Ant = double.Parse(xReader.GetValue("por_ant").ToString()),
                                Por_Efi = double.Parse(xReader.GetValue("por_efi").ToString())
                            });
                        }
                    }
                    respuesta = tmpImportes.ToArray();
                }
            }
            return respuesta;
        }



        //****** Reportes ******
        public FileInfo Obtener_ReporteEficiencia(int Id_Oficina, int Ano, int Mes, int Sb, int Sect){

            //**** Obtener Cadena de Conexion
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            
            //**** Generar Query
            var xQueryReporte = $"Exec[Sicem].[Eficiencia] @cAlias = 'DATOS_REPORTE1', @xAf = {Ano}, @xXmf = {Mes}, @nSub = {Sb}, @nSec = {Sect} ";

            ////**** Generar DataSet
            //try {
            //    using (var xConection = new SqlConnection(xCad_Conexion)) {
            //        xConection.Open();
            //        var xDataSet = new DataSet();
            //        new SqlDataAdapter(xQueryReporte, xConection).Fill(xDataSet);
            //        xDataSet.WriteXml(appSettings.GetValue<string>("Rutas_Locales:Ruta_Reportes") + "\\Esquemas\\RptEficiencia_TipoCalculo.xsd");
            //        Console.WriteLine("DataSet generado");
            //    }
            //}
            //catch (Exception){}


            //**** Obtener Ruta Reporte
            var nombreReporte = appSettings.GetValue<string>("Rutas_Locales:Ruta_Reportes") + "RptEficiencia_TipoCalculo.rpt";

            //**** Generar Nombre Archivo
            var nombreArchivo = Guid.NewGuid().ToString().Replace("-", "") + ".pdf";
            var rutaArchivoGenerado = appSettings.GetValue<string>("Rutas_Locales:Archivos_Temporales") + nombreArchivo;

            //**** Generar Titulo Reporte
            var tmpData = new DateTime(Ano, Mes, 1);
            var titulo = $"Reporte de Eficiencia de {tmpData.ToString("MMMM")} del {tmpData.ToString("yyyy")}".ToUpper();


            //**** Generar Cadena de Parametros
            var paramBuilder = new System.Text.StringBuilder();
            paramBuilder.Append($"\"{xEnlace.StringConection}\" ");
            paramBuilder.Append($"\"{xQueryReporte}\" ");
            paramBuilder.Append($"\"{nombreReporte}\" ");
            paramBuilder.Append($"\"{rutaArchivoGenerado}\" ");
            paramBuilder.Append($"\"{titulo}\" ");


            //**** Executar Generardor de reporte
            var generadorRep = appSettings.GetValue<string>("Rutas_Locales:Generador_Reporte");
            var xProc = new System.Diagnostics.Process();
            xProc.StartInfo.FileName = generadorRep;
            xProc.StartInfo.Arguments = paramBuilder.ToString();
            xProc.Start();
            xProc.WaitForExit();

            if(xProc.ExitCode == 1){
                return new FileInfo(rutaArchivoGenerado);
                //return new FileInfo(stream, mimeType);
                throw new NotImplementedException();
            }
            else {
                throw new Exception($"Error al generar el reporte");
            }
        }
        public FileInfo Obtener_ReporteEficienciaConceptos(int Id_Oficina, int Ano, int Mes, int Sb, int Sect) {

            //**** Obtener Cadena de Conexion
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            
            //**** Generar Query
            var xQueryReporte = $"Exec[Sicem].[Eficiencia] @cAlias = 'DATOS_REPORTE1', @xAf = {Ano}, @xXmf = {Mes}, @nSub = {Sb}, @nSec = {Sect} ";

            ////**** Generar DataSet
            //try {
            //    using (var xConection = new SqlConnection(xCad_Conexion)) {
            //        xConection.Open();
            //        var xDataSet = new DataSet();
            //        new SqlDataAdapter(xQueryReporte, xConection).Fill(xDataSet);
            //        xDataSet.WriteXml(appSettings.GetValue<string>("Rutas_Locales:Ruta_Reportes") + "\\Esquemas\\RptEficiencia_TipoCalculo.xsd");
            //        Console.WriteLine("DataSet generado");
            //    }
            //}
            //catch (Exception){}


            //**** Obtener Ruta Reporte
            var nombreReporte = appSettings.GetValue<string>("Rutas_Locales:Ruta_Reportes") + "RptEficiencia_Conceptos.rpt";

            //**** Generar Nombre Archivo
            var nombreArchivo = Guid.NewGuid().ToString().Replace("-", "") + ".pdf";
            var rutaArchivoGenerado = appSettings.GetValue<string>("Rutas_Locales:Archivos_Temporales") + nombreArchivo;

            //**** Generar Titulo Reporte
            var tmpData = new DateTime(Ano, Mes, 1);
            var titulo = $"Reporte de Eficiencia Por Conceptos de {tmpData.ToString("MMMM")} del {tmpData.ToString("yyyy")}".ToUpper();


            //**** Generar Cadena de Parametros
            var paramBuilder = new System.Text.StringBuilder();
            paramBuilder.Append($"\"{xEnlace.StringConection}\" ");
            paramBuilder.Append($"\"{xQueryReporte}\" ");
            paramBuilder.Append($"\"{nombreReporte}\" ");
            paramBuilder.Append($"\"{rutaArchivoGenerado}\" ");
            paramBuilder.Append($"\"{titulo}\" ");


            //**** Executar Generardor de reporte
            var generadorRep = appSettings.GetValue<string>("Rutas_Locales:Generador_Reporte");
            var xProc = new System.Diagnostics.Process();
            xProc.StartInfo.FileName = generadorRep;
            xProc.StartInfo.Arguments = paramBuilder.ToString();
            xProc.Start();
            xProc.WaitForExit();

            if(xProc.ExitCode == 1){
                return new FileInfo(rutaArchivoGenerado);
                //return new FileStreamResult(stream, mimeType);
            }
            else {
                throw new Exception($"Error al generar el reporte");
            }

        }
        public FileInfo Obtener_ReporteRangoM3(int Id_Oficina, int Ano, int Mes, int Sb, int Sect) {

            //**** Obtener Cadena de Conexion
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            
            //**** Generar Query
            var xQueryReporte = $"Exec [Sicem].[Eficiencia] @cAlias = 'DATOS_REPORTE2', @xAf={Ano}, @xXmf={Mes}, @nSub={Sb}, @nSec={Sect}";

            //**** Generar DataSet
            // try {
            //    using (var xConection = new SqlConnection(xCad_Conexion)) {
            //        xConection.Open();
            //        var xDataSet = new DataSet();
            //        new SqlDataAdapter(xQueryReporte, xConection).Fill(xDataSet);
            //        xDataSet.WriteXml(appSettings.GetValue<string>("Rutas_Locales:Ruta_Reportes") + "\\Esquemas\\RptEficiencia_RangoConsumoM3.xsd");
            //        Console.WriteLine("DataSet generado");
            //    }
            // }
            // catch (Exception){}


            //**** Obtener Ruta Reporte
            var nombreReporte = appSettings.GetValue<string>("Rutas_Locales:Ruta_Reportes") + "RptEficiencia_RangoConsumoM3.rpt";

            //**** Generar Nombre Archivo
            var nombreArchivo = Guid.NewGuid().ToString().Replace("-", "") + ".pdf";
            var rutaArchivoGenerado = appSettings.GetValue<string>("Rutas_Locales:Archivos_Temporales") + nombreArchivo;

            //**** Generar Titulo Reporte
            var tmpData = new DateTime(Ano, Mes, 1);
            var titulo = $"ingresos cobrados por rango de consumos en m3 de {tmpData.ToString("MMMM")} del {tmpData.ToString("yyyy")}".ToUpper();


            //**** Generar Cadena de Parametros
            var paramBuilder = new System.Text.StringBuilder();
            paramBuilder.Append($"\"{xEnlace.StringConection}\" ");
            paramBuilder.Append($"\"{xQueryReporte}\" ");
            paramBuilder.Append($"\"{nombreReporte}\" ");
            paramBuilder.Append($"\"{rutaArchivoGenerado}\" ");
            paramBuilder.Append($"\"{titulo}\" ");


            //**** Executar Generardor de reporte
            var generadorRep = appSettings.GetValue<string>("Rutas_Locales:Generador_Reporte");
            var xProc = new System.Diagnostics.Process();
            xProc.StartInfo.FileName = generadorRep;
            xProc.StartInfo.Arguments = paramBuilder.ToString();
            xProc.Start();
            xProc.WaitForExit();

            if(xProc.ExitCode == 1){
                return new FileInfo(rutaArchivoGenerado);
                //return new FileStreamResult(stream, mimeType);
            }
            else {
                throw new Exception($"Error al generar el reporte");
            }

        }

        public EficienciaResumen ObtenerResumenEnlace(IEnlace enlace, int anio, int mes, int sb, int sec)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EficienciaResumen> ObtenerResumenAnual(IEnlace enlace, int anio, int sb, int sec)
        {
            throw new NotImplementedException();
        }
    }
}
