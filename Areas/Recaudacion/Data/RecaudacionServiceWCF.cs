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
using ServiceReference;
using System.Threading;
using Sicem_Blazor.Data.Contracts;
using System.IO;

namespace Sicem_Blazor.Recaudacion.Data {
    public class RecaudacionServiceWCF // : IRecaudacionService
    {
        private readonly SicemService sicemService;
        private readonly IConfiguration appSettings;
        private readonly ILogger<IRecaudacionService> logger;
        private ServiceClient serviceClient;
        private readonly TimeSpan timeoutWCFConnection;


        public RecaudacionServiceWCF(IConfiguration c, SicemService s, ILogger<IRecaudacionService> l) {
            this.appSettings = c;
            this.sicemService = s;
            this.logger = l;
            serviceClient = new ServiceClient();
            timeoutWCFConnection = TimeSpan.FromMinutes(15);
        }
       

        public Recaudacion_Resumen ObtenerResumen(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect) {
            var cadOficina = CadOficinasGenerator.Genearar(enlace.Id);
            
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(timeoutWCFConnection);

            try {

                var resultService = Task.Run( 
                    async () => await serviceClient.Fun_RecaudacionAsync(cadOficina, desde, hasta, (short) sb, (short) sect),
                    cancellationSource.Token
                ).GetAwaiter().GetResult();

                var firstItem = resultService.FirstOrDefault();
                if( firstItem == null){
                    throw new Exception("No hay datos disponibles");
                }

                return new Recaudacion_Resumen(enlace){
                    Estatus = ResumenOficinaEstatus.Completado,
                    Usuarios_Propios = firstItem.u1,
                    Ingresos_Propios = Convert.ToDecimal( firstItem.i1) ,
                    Usuarios_Otros = firstItem.u2,
                    Ingresos_Otros = Convert.ToDecimal( firstItem.i2) ,
                    Importe_Total = Convert.ToDecimal( firstItem.Total),
                    Cobrado = Convert.ToDecimal( firstItem.Cobrado) ,
                    Usuarios_Total = firstItem.Usuarios,
                }; 

            }catch(Exception err){
                Console.WriteLine($"Error al obtener el resumen de ingresos de {enlace.Nombre} \n\t{err.Message}");
                return new Recaudacion_Resumen(enlace){
                    Estatus = ResumenOficinaEstatus.Error,
                };
            }

        }
        public Recaudacion_Analitico ObtenerAnalisisIngresos(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect) {
            try {
            
                // Ingresos analitico
                var analiticoIngAnual = new List<Recaudacion_AnaliticoMensual>();
                var analiticoIngQuincenal = new List<Recaudacion_AnaliticoQuincenal>();
                var analiticoIngSemanal = new List<Recaudacion_AnaliticoSemanal>();
                
                var dsString = Task.Run( async () => await serviceClient.Fun_Recaudacion_15Async( (short) enlace.Id, desde, hasta, (short) sb, (short) sect)).GetAwaiter().GetResult();
                var dsIngresos = new DataSet();
                StringReader txtReader = new StringReader(dsString);
                dsIngresos.ReadXml(txtReader);
                txtReader.Close();
                

                var tabAnual = dsIngresos.Tables[0];
                foreach(DataRow row in tabAnual.Rows){
                    analiticoIngAnual.Add( new Recaudacion_AnaliticoMensual {
                        Oficina = row["Oficina"].ToString(),
                        Año = ConvertUtils.ParseInteger(row["Año"].ToString()),
                        Ene = ConvertUtils.ParseDecimal(row["Ene"].ToString()),
                        Feb = ConvertUtils.ParseDecimal(row["Feb"].ToString()),
                        Mar = ConvertUtils.ParseDecimal(row["Mar"].ToString()),
                        Abr = ConvertUtils.ParseDecimal(row["Abr"].ToString()),
                        May = ConvertUtils.ParseDecimal(row["May"].ToString()),
                        Jun = ConvertUtils.ParseDecimal(row["Jun"].ToString()),
                        Jul = ConvertUtils.ParseDecimal(row["Jul"].ToString()),
                        Ago = ConvertUtils.ParseDecimal(row["Ago"].ToString()),
                        Sep = ConvertUtils.ParseDecimal(row["Sep"].ToString()),
                        Oct = ConvertUtils.ParseDecimal(row["Oct"].ToString()),
                        Nov = ConvertUtils.ParseDecimal(row["Nov"].ToString()),
                        Dic = ConvertUtils.ParseDecimal(row["Dic"].ToString()),
                        Total = ConvertUtils.ParseDecimal(row["Total"].ToString()),
                    });
                }
                
                var tabQuin = dsIngresos.Tables[1];
                foreach(DataRow row in tabQuin.Rows){
                    analiticoIngQuincenal.Add( new Recaudacion_AnaliticoQuincenal{
                        Oficina = row["Oficina"].ToString(),
                        Año = ConvertUtils.ParseInteger(row["Año"].ToString()),
                        Ene_1 = ConvertUtils.ParseDecimal(row["Ene_1"].ToString()),
                        Feb_1 = ConvertUtils.ParseDecimal(row["Feb_1"].ToString()),
                        Mar_1 = ConvertUtils.ParseDecimal(row["Mar_1"].ToString()),
                        Abr_1 = ConvertUtils.ParseDecimal(row["Abr_1"].ToString()),
                        May_1 = ConvertUtils.ParseDecimal(row["May_1"].ToString()),
                        Jun_1 = ConvertUtils.ParseDecimal(row["Jun_1"].ToString()),
                        Jul_1 = ConvertUtils.ParseDecimal(row["Jul_1"].ToString()),
                        Ago_1 = ConvertUtils.ParseDecimal(row["Ago_1"].ToString()),
                        Sep_1 = ConvertUtils.ParseDecimal(row["Sep_1"].ToString()),
                        Oct_1 = ConvertUtils.ParseDecimal(row["Oct_1"].ToString()),
                        Nov_1 = ConvertUtils.ParseDecimal(row["Nov_1"].ToString()),
                        Dic_1 = ConvertUtils.ParseDecimal(row["Dic_1"].ToString()),
                        Ene_2 = ConvertUtils.ParseDecimal(row["Ene_2"].ToString()),
                        Feb_2 = ConvertUtils.ParseDecimal(row["Feb_2"].ToString()),
                        Mar_2 = ConvertUtils.ParseDecimal(row["Mar_2"].ToString()),
                        Abr_2 = ConvertUtils.ParseDecimal(row["Abr_2"].ToString()),
                        May_2 = ConvertUtils.ParseDecimal(row["May_2"].ToString()),
                        Jun_2 = ConvertUtils.ParseDecimal(row["Jun_2"].ToString()),
                        Jul_2 = ConvertUtils.ParseDecimal(row["Jul_2"].ToString()),
                        Ago_2 = ConvertUtils.ParseDecimal(row["Ago_2"].ToString()),
                        Sep_2 = ConvertUtils.ParseDecimal(row["Sep_2"].ToString()),
                        Oct_2 = ConvertUtils.ParseDecimal(row["Oct_2"].ToString()),
                        Nov_2 = ConvertUtils.ParseDecimal(row["Nov_2"].ToString()),
                        Dic_2 = ConvertUtils.ParseDecimal(row["Dic_2"].ToString()),
                        Total = ConvertUtils.ParseDecimal(row["Total"].ToString()),
                    });
                }

                var tabSem = dsIngresos.Tables[2];
                foreach(DataRow row in tabSem.Rows){
                    analiticoIngSemanal.Add(new Recaudacion_AnaliticoSemanal {
                        Oficina = row["Oficina"].ToString(),
                        Año = ConvertUtils.ParseInteger(row["Año"].ToString()),
                        Ene_1 = ConvertUtils.ParseDecimal(row["Ene_1"].ToString()),
                        Feb_1 = ConvertUtils.ParseDecimal(row["Feb_1"].ToString()),
                        Mar_1 = ConvertUtils.ParseDecimal(row["Mar_1"].ToString()),
                        Abr_1 = ConvertUtils.ParseDecimal(row["Abr_1"].ToString()),
                        May_1 = ConvertUtils.ParseDecimal(row["May_1"].ToString()),
                        Jun_1 = ConvertUtils.ParseDecimal(row["Jun_1"].ToString()),
                        Jul_1 = ConvertUtils.ParseDecimal(row["Jul_1"].ToString()),
                        Ago_1 = ConvertUtils.ParseDecimal(row["Ago_1"].ToString()),
                        Sep_1 = ConvertUtils.ParseDecimal(row["Sep_1"].ToString()),
                        Oct_1 = ConvertUtils.ParseDecimal(row["Oct_1"].ToString()),
                        Nov_1 = ConvertUtils.ParseDecimal(row["Nov_1"].ToString()),
                        Dic_1 = ConvertUtils.ParseDecimal(row["Dic_1"].ToString()),
                        Ene_2 = ConvertUtils.ParseDecimal(row["Ene_2"].ToString()),
                        Feb_2 = ConvertUtils.ParseDecimal(row["Feb_2"].ToString()),
                        Mar_2 = ConvertUtils.ParseDecimal(row["Mar_2"].ToString()),
                        Abr_2 = ConvertUtils.ParseDecimal(row["Abr_2"].ToString()),
                        May_2 = ConvertUtils.ParseDecimal(row["May_2"].ToString()),
                        Jun_2 = ConvertUtils.ParseDecimal(row["Jun_2"].ToString()),
                        Jul_2 = ConvertUtils.ParseDecimal(row["Jul_2"].ToString()),
                        Ago_2 = ConvertUtils.ParseDecimal(row["Ago_2"].ToString()),
                        Sep_2 = ConvertUtils.ParseDecimal(row["Sep_2"].ToString()),
                        Oct_2 = ConvertUtils.ParseDecimal(row["Oct_2"].ToString()),
                        Nov_2 = ConvertUtils.ParseDecimal(row["Nov_2"].ToString()),
                        Dic_2 = ConvertUtils.ParseDecimal(row["Dic_2"].ToString()),
                        Ene_3 = ConvertUtils.ParseDecimal(row["Ene_3"].ToString()),
                        Feb_3 = ConvertUtils.ParseDecimal(row["Feb_3"].ToString()),
                        Mar_3 = ConvertUtils.ParseDecimal(row["Mar_3"].ToString()),
                        Abr_3 = ConvertUtils.ParseDecimal(row["Abr_3"].ToString()),
                        May_3 = ConvertUtils.ParseDecimal(row["May_3"].ToString()),
                        Jun_3 = ConvertUtils.ParseDecimal(row["Jun_3"].ToString()),
                        Jul_3 = ConvertUtils.ParseDecimal(row["Jul_3"].ToString()),
                        Ago_3 = ConvertUtils.ParseDecimal(row["Ago_3"].ToString()),
                        Sep_3 = ConvertUtils.ParseDecimal(row["Sep_3"].ToString()),
                        Oct_3 = ConvertUtils.ParseDecimal(row["Oct_3"].ToString()),
                        Nov_3 = ConvertUtils.ParseDecimal(row["Nov_3"].ToString()),
                        Dic_3 = ConvertUtils.ParseDecimal(row["Dic_3"].ToString()),
                        Ene_4 = ConvertUtils.ParseDecimal(row["Ene_4"].ToString()),
                        Feb_4 = ConvertUtils.ParseDecimal(row["Feb_4"].ToString()),
                        Mar_4 = ConvertUtils.ParseDecimal(row["Mar_4"].ToString()),
                        Abr_4 = ConvertUtils.ParseDecimal(row["Abr_4"].ToString()),
                        May_4 = ConvertUtils.ParseDecimal(row["May_4"].ToString()),
                        Jun_4 = ConvertUtils.ParseDecimal(row["Jun_4"].ToString()),
                        Jul_4 = ConvertUtils.ParseDecimal(row["Jul_4"].ToString()),
                        Ago_4 = ConvertUtils.ParseDecimal(row["Ago_4"].ToString()),
                        Sep_4 = ConvertUtils.ParseDecimal(row["Sep_4"].ToString()),
                        Oct_4 = ConvertUtils.ParseDecimal(row["Oct_4"].ToString()),
                        Nov_4 = ConvertUtils.ParseDecimal(row["Nov_4"].ToString()),
                        Dic_4 = ConvertUtils.ParseDecimal(row["Dic_4"].ToString()),
                        Total = ConvertUtils.ParseDecimal(row["Total"].ToString()),
                    });
                    }
        
                // var task1 = Task.Run( async() => {
                //     var dsString = await serviceClient.Fun_Recaudacion_15Async( (short) enlace.Id, desde, hasta, (short) sb, (short) sect);
                //     lock(dsIngresos){
                //         using(StringReader txtReader = new StringReader(dsString)){
                //             dsIngresos.ReadXml(txtReader);
                //         }

                //         var tabAnual = dsIngresos.Tables[0];
                //         foreach(DataRow row in tabAnual.Rows){
                //             analiticoIngAnual.Add( new Recaudacion_AnaliticoMensual {
                //                 Oficina = row["Oficina"].ToString(),
                //                 Año = ConvertUtils.ParseInteger(row["Año"].ToString()),
                //                 Ene = ConvertUtils.ParseDecimal(row["Ene"].ToString()),
                //                 Feb = ConvertUtils.ParseDecimal(row["Feb"].ToString()),
                //                 Mar = ConvertUtils.ParseDecimal(row["Mar"].ToString()),
                //                 Abr = ConvertUtils.ParseDecimal(row["Abr"].ToString()),
                //                 May = ConvertUtils.ParseDecimal(row["May"].ToString()),
                //                 Jun = ConvertUtils.ParseDecimal(row["Jun"].ToString()),
                //                 Jul = ConvertUtils.ParseDecimal(row["Jul"].ToString()),
                //                 Ago = ConvertUtils.ParseDecimal(row["Ago"].ToString()),
                //                 Sep = ConvertUtils.ParseDecimal(row["Sep"].ToString()),
                //                 Oct = ConvertUtils.ParseDecimal(row["Oct"].ToString()),
                //                 Nov = ConvertUtils.ParseDecimal(row["Nov"].ToString()),
                //                 Dic = ConvertUtils.ParseDecimal(row["Dic"].ToString()),
                //                 Total = ConvertUtils.ParseDecimal(row["Total"].ToString()),
                //             });
                //         }
                        
                //         var tabQuin = dsIngresos.Tables[1];
                //         foreach(DataRow row in tabQuin.Rows){
                //             analiticoIngQuincenal.Add( new Recaudacion_AnaliticoQuincenal{
                //                 Oficina = row["Oficina"].ToString(),
                //                 Año = ConvertUtils.ParseInteger(row["Año"].ToString()),
                //                 Ene_1 = ConvertUtils.ParseDecimal(row["Ene_1"].ToString()),
                //                 Feb_1 = ConvertUtils.ParseDecimal(row["Feb_1"].ToString()),
                //                 Mar_1 = ConvertUtils.ParseDecimal(row["Mar_1"].ToString()),
                //                 Abr_1 = ConvertUtils.ParseDecimal(row["Abr_1"].ToString()),
                //                 May_1 = ConvertUtils.ParseDecimal(row["May_1"].ToString()),
                //                 Jun_1 = ConvertUtils.ParseDecimal(row["Jun_1"].ToString()),
                //                 Jul_1 = ConvertUtils.ParseDecimal(row["Jul_1"].ToString()),
                //                 Ago_1 = ConvertUtils.ParseDecimal(row["Ago_1"].ToString()),
                //                 Sep_1 = ConvertUtils.ParseDecimal(row["Sep_1"].ToString()),
                //                 Oct_1 = ConvertUtils.ParseDecimal(row["Oct_1"].ToString()),
                //                 Nov_1 = ConvertUtils.ParseDecimal(row["Nov_1"].ToString()),
                //                 Dic_1 = ConvertUtils.ParseDecimal(row["Dic_1"].ToString()),
                //                 Ene_2 = ConvertUtils.ParseDecimal(row["Ene_2"].ToString()),
                //                 Feb_2 = ConvertUtils.ParseDecimal(row["Feb_2"].ToString()),
                //                 Mar_2 = ConvertUtils.ParseDecimal(row["Mar_2"].ToString()),
                //                 Abr_2 = ConvertUtils.ParseDecimal(row["Abr_2"].ToString()),
                //                 May_2 = ConvertUtils.ParseDecimal(row["May_2"].ToString()),
                //                 Jun_2 = ConvertUtils.ParseDecimal(row["Jun_2"].ToString()),
                //                 Jul_2 = ConvertUtils.ParseDecimal(row["Jul_2"].ToString()),
                //                 Ago_2 = ConvertUtils.ParseDecimal(row["Ago_2"].ToString()),
                //                 Sep_2 = ConvertUtils.ParseDecimal(row["Sep_2"].ToString()),
                //                 Oct_2 = ConvertUtils.ParseDecimal(row["Oct_2"].ToString()),
                //                 Nov_2 = ConvertUtils.ParseDecimal(row["Nov_2"].ToString()),
                //                 Dic_2 = ConvertUtils.ParseDecimal(row["Dic_2"].ToString()),
                //                 Total = ConvertUtils.ParseDecimal(row["Total"].ToString()),
                //             });
                //         }

                //         var tabSem = dsIngresos.Tables[2];
                //         foreach(DataRow row in tabSem.Rows){
                //          analiticoIngSemanal.Add(new Recaudacion_AnaliticoSemanal {
                    //             Oficina = row["Oficina"].ToString(),
                    //             Año = ConvertUtils.ParseInteger(row["Año"].ToString()),
                    //             Ene_1 = ConvertUtils.ParseDecimal(row["Ene_1"].ToString()),
                    //             Feb_1 = ConvertUtils.ParseDecimal(row["Feb_1"].ToString()),
                    //             Mar_1 = ConvertUtils.ParseDecimal(row["Mar_1"].ToString()),
                    //             Abr_1 = ConvertUtils.ParseDecimal(row["Abr_1"].ToString()),
                    //             May_1 = ConvertUtils.ParseDecimal(row["May_1"].ToString()),
                    //             Jun_1 = ConvertUtils.ParseDecimal(row["Jun_1"].ToString()),
                    //             Jul_1 = ConvertUtils.ParseDecimal(row["Jul_1"].ToString()),
                    //             Ago_1 = ConvertUtils.ParseDecimal(row["Ago_1"].ToString()),
                    //             Sep_1 = ConvertUtils.ParseDecimal(row["Sep_1"].ToString()),
                    //             Oct_1 = ConvertUtils.ParseDecimal(row["Oct_1"].ToString()),
                    //             Nov_1 = ConvertUtils.ParseDecimal(row["Nov_1"].ToString()),
                    //             Dic_1 = ConvertUtils.ParseDecimal(row["Dic_1"].ToString()),
                    //             Ene_2 = ConvertUtils.ParseDecimal(row["Ene_2"].ToString()),
                    //             Feb_2 = ConvertUtils.ParseDecimal(row["Feb_2"].ToString()),
                    //             Mar_2 = ConvertUtils.ParseDecimal(row["Mar_2"].ToString()),
                    //             Abr_2 = ConvertUtils.ParseDecimal(row["Abr_2"].ToString()),
                    //             May_2 = ConvertUtils.ParseDecimal(row["May_2"].ToString()),
                    //             Jun_2 = ConvertUtils.ParseDecimal(row["Jun_2"].ToString()),
                    //             Jul_2 = ConvertUtils.ParseDecimal(row["Jul_2"].ToString()),
                    //             Ago_2 = ConvertUtils.ParseDecimal(row["Ago_2"].ToString()),
                    //             Sep_2 = ConvertUtils.ParseDecimal(row["Sep_2"].ToString()),
                    //             Oct_2 = ConvertUtils.ParseDecimal(row["Oct_2"].ToString()),
                    //             Nov_2 = ConvertUtils.ParseDecimal(row["Nov_2"].ToString()),
                    //             Dic_2 = ConvertUtils.ParseDecimal(row["Dic_2"].ToString()),
                    //             Ene_3 = ConvertUtils.ParseDecimal(row["Ene_3"].ToString()),
                    //             Feb_3 = ConvertUtils.ParseDecimal(row["Feb_3"].ToString()),
                    //             Mar_3 = ConvertUtils.ParseDecimal(row["Mar_3"].ToString()),
                    //             Abr_3 = ConvertUtils.ParseDecimal(row["Abr_3"].ToString()),
                    //             May_3 = ConvertUtils.ParseDecimal(row["May_3"].ToString()),
                    //             Jun_3 = ConvertUtils.ParseDecimal(row["Jun_3"].ToString()),
                    //             Jul_3 = ConvertUtils.ParseDecimal(row["Jul_3"].ToString()),
                    //             Ago_3 = ConvertUtils.ParseDecimal(row["Ago_3"].ToString()),
                    //             Sep_3 = ConvertUtils.ParseDecimal(row["Sep_3"].ToString()),
                    //             Oct_3 = ConvertUtils.ParseDecimal(row["Oct_3"].ToString()),
                    //             Nov_3 = ConvertUtils.ParseDecimal(row["Nov_3"].ToString()),
                    //             Dic_3 = ConvertUtils.ParseDecimal(row["Dic_3"].ToString()),
                    //             Ene_4 = ConvertUtils.ParseDecimal(row["Ene_4"].ToString()),
                    //             Feb_4 = ConvertUtils.ParseDecimal(row["Feb_4"].ToString()),
                    //             Mar_4 = ConvertUtils.ParseDecimal(row["Mar_4"].ToString()),
                    //             Abr_4 = ConvertUtils.ParseDecimal(row["Abr_4"].ToString()),
                    //             May_4 = ConvertUtils.ParseDecimal(row["May_4"].ToString()),
                    //             Jun_4 = ConvertUtils.ParseDecimal(row["Jun_4"].ToString()),
                    //             Jul_4 = ConvertUtils.ParseDecimal(row["Jul_4"].ToString()),
                    //             Ago_4 = ConvertUtils.ParseDecimal(row["Ago_4"].ToString()),
                    //             Sep_4 = ConvertUtils.ParseDecimal(row["Sep_4"].ToString()),
                    //             Oct_4 = ConvertUtils.ParseDecimal(row["Oct_4"].ToString()),
                    //             Nov_4 = ConvertUtils.ParseDecimal(row["Nov_4"].ToString()),
                    //             Dic_4 = ConvertUtils.ParseDecimal(row["Dic_4"].ToString()),
                    //             Total = ConvertUtils.ParseDecimal(row["Total"].ToString()),
                    //         });
                //          }
                //     }

                // });

                // Ingresos analitico rezago
                var analiticoRezAnual = new List<Recaudacion_AnaliticoMensual>();
                var analiticoRezQuincenal = new List<Recaudacion_AnaliticoQuincenal>();
                var analiticoRezSemanal = new List<Recaudacion_AnaliticoSemanal>();

                // var dsRezago = new DataSet();
                // var task2 = Task.Run( async() => {
                //     var dsString = await serviceClient.Fun_Recaudacion_16Async( (short) enlace.Id, desde, hasta, (short) sb, (short) sect);

                //     lock(dsRezago){
                //         using(StringReader txtReader = new StringReader(dsString)){
                //             dsRezago.ReadXml(txtReader);
                //         }

                //         var tabAnual = dsRezago.Tables[0];
                //         foreach(DataRow row in tabAnual.Rows){
                //             analiticoRezAnual.Add( new Recaudacion_AnaliticoMensual {
                //                 Oficina = row["Oficina"].ToString(),
                //                 Año = ConvertUtils.ParseInteger(row["Año"].ToString()),
                //                 Ene = ConvertUtils.ParseDecimal(row["Ene"].ToString()),
                //                 Feb = ConvertUtils.ParseDecimal(row["Feb"].ToString()),
                //                 Mar = ConvertUtils.ParseDecimal(row["Mar"].ToString()),
                //                 Abr = ConvertUtils.ParseDecimal(row["Abr"].ToString()),
                //                 May = ConvertUtils.ParseDecimal(row["May"].ToString()),
                //                 Jun = ConvertUtils.ParseDecimal(row["Jun"].ToString()),
                //                 Jul = ConvertUtils.ParseDecimal(row["Jul"].ToString()),
                //                 Ago = ConvertUtils.ParseDecimal(row["Ago"].ToString()),
                //                 Sep = ConvertUtils.ParseDecimal(row["Sep"].ToString()),
                //                 Oct = ConvertUtils.ParseDecimal(row["Oct"].ToString()),
                //                 Nov = ConvertUtils.ParseDecimal(row["Nov"].ToString()),
                //                 Dic = ConvertUtils.ParseDecimal(row["Dic"].ToString()),
                //                 Total = ConvertUtils.ParseDecimal(row["Total"].ToString()),
                //             });
                //         }
                        
                //         var tabQuin = dsRezago.Tables[1];
                //         foreach(DataRow row in tabQuin.Rows){
                //             analiticoRezQuincenal.Add( new Recaudacion_AnaliticoQuincenal{
                //                 Oficina = row["Oficina"].ToString(),
                //                 Año = ConvertUtils.ParseInteger(row["Año"].ToString()),
                //                 Ene_1 = ConvertUtils.ParseDecimal(row["Ene_1"].ToString()),
                //                 Feb_1 = ConvertUtils.ParseDecimal(row["Feb_1"].ToString()),
                //                 Mar_1 = ConvertUtils.ParseDecimal(row["Mar_1"].ToString()),
                //                 Abr_1 = ConvertUtils.ParseDecimal(row["Abr_1"].ToString()),
                //                 May_1 = ConvertUtils.ParseDecimal(row["May_1"].ToString()),
                //                 Jun_1 = ConvertUtils.ParseDecimal(row["Jun_1"].ToString()),
                //                 Jul_1 = ConvertUtils.ParseDecimal(row["Jul_1"].ToString()),
                //                 Ago_1 = ConvertUtils.ParseDecimal(row["Ago_1"].ToString()),
                //                 Sep_1 = ConvertUtils.ParseDecimal(row["Sep_1"].ToString()),
                //                 Oct_1 = ConvertUtils.ParseDecimal(row["Oct_1"].ToString()),
                //                 Nov_1 = ConvertUtils.ParseDecimal(row["Nov_1"].ToString()),
                //                 Dic_1 = ConvertUtils.ParseDecimal(row["Dic_1"].ToString()),
                //                 Ene_2 = ConvertUtils.ParseDecimal(row["Ene_2"].ToString()),
                //                 Feb_2 = ConvertUtils.ParseDecimal(row["Feb_2"].ToString()),
                //                 Mar_2 = ConvertUtils.ParseDecimal(row["Mar_2"].ToString()),
                //                 Abr_2 = ConvertUtils.ParseDecimal(row["Abr_2"].ToString()),
                //                 May_2 = ConvertUtils.ParseDecimal(row["May_2"].ToString()),
                //                 Jun_2 = ConvertUtils.ParseDecimal(row["Jun_2"].ToString()),
                //                 Jul_2 = ConvertUtils.ParseDecimal(row["Jul_2"].ToString()),
                //                 Ago_2 = ConvertUtils.ParseDecimal(row["Ago_2"].ToString()),
                //                 Sep_2 = ConvertUtils.ParseDecimal(row["Sep_2"].ToString()),
                //                 Oct_2 = ConvertUtils.ParseDecimal(row["Oct_2"].ToString()),
                //                 Nov_2 = ConvertUtils.ParseDecimal(row["Nov_2"].ToString()),
                //                 Dic_2 = ConvertUtils.ParseDecimal(row["Dic_2"].ToString()),
                //                 Total = ConvertUtils.ParseDecimal(row["Total"].ToString()),
                //             });
                //         }

                //         var tabSem = dsRezago.Tables[2];
                //         foreach(DataRow row in tabSem.Rows){
                //             analiticoRezSemanal.Add(new Recaudacion_AnaliticoSemanal {
                //                 Oficina = row["Oficina"].ToString(),
                //                 Año = ConvertUtils.ParseInteger(row["Año"].ToString()),
                //                 Ene_1 = ConvertUtils.ParseDecimal(row["Ene_1"].ToString()),
                //                 Feb_1 = ConvertUtils.ParseDecimal(row["Feb_1"].ToString()),
                //                 Mar_1 = ConvertUtils.ParseDecimal(row["Mar_1"].ToString()),
                //                 Abr_1 = ConvertUtils.ParseDecimal(row["Abr_1"].ToString()),
                //                 May_1 = ConvertUtils.ParseDecimal(row["May_1"].ToString()),
                //                 Jun_1 = ConvertUtils.ParseDecimal(row["Jun_1"].ToString()),
                //                 Jul_1 = ConvertUtils.ParseDecimal(row["Jul_1"].ToString()),
                //                 Ago_1 = ConvertUtils.ParseDecimal(row["Ago_1"].ToString()),
                //                 Sep_1 = ConvertUtils.ParseDecimal(row["Sep_1"].ToString()),
                //                 Oct_1 = ConvertUtils.ParseDecimal(row["Oct_1"].ToString()),
                //                 Nov_1 = ConvertUtils.ParseDecimal(row["Nov_1"].ToString()),
                //                 Dic_1 = ConvertUtils.ParseDecimal(row["Dic_1"].ToString()),
                //                 Ene_2 = ConvertUtils.ParseDecimal(row["Ene_2"].ToString()),
                //                 Feb_2 = ConvertUtils.ParseDecimal(row["Feb_2"].ToString()),
                //                 Mar_2 = ConvertUtils.ParseDecimal(row["Mar_2"].ToString()),
                //                 Abr_2 = ConvertUtils.ParseDecimal(row["Abr_2"].ToString()),
                //                 May_2 = ConvertUtils.ParseDecimal(row["May_2"].ToString()),
                //                 Jun_2 = ConvertUtils.ParseDecimal(row["Jun_2"].ToString()),
                //                 Jul_2 = ConvertUtils.ParseDecimal(row["Jul_2"].ToString()),
                //                 Ago_2 = ConvertUtils.ParseDecimal(row["Ago_2"].ToString()),
                //                 Sep_2 = ConvertUtils.ParseDecimal(row["Sep_2"].ToString()),
                //                 Oct_2 = ConvertUtils.ParseDecimal(row["Oct_2"].ToString()),
                //                 Nov_2 = ConvertUtils.ParseDecimal(row["Nov_2"].ToString()),
                //                 Dic_2 = ConvertUtils.ParseDecimal(row["Dic_2"].ToString()),
                //                 Ene_3 = ConvertUtils.ParseDecimal(row["Ene_3"].ToString()),
                //                 Feb_3 = ConvertUtils.ParseDecimal(row["Feb_3"].ToString()),
                //                 Mar_3 = ConvertUtils.ParseDecimal(row["Mar_3"].ToString()),
                //                 Abr_3 = ConvertUtils.ParseDecimal(row["Abr_3"].ToString()),
                //                 May_3 = ConvertUtils.ParseDecimal(row["May_3"].ToString()),
                //                 Jun_3 = ConvertUtils.ParseDecimal(row["Jun_3"].ToString()),
                //                 Jul_3 = ConvertUtils.ParseDecimal(row["Jul_3"].ToString()),
                //                 Ago_3 = ConvertUtils.ParseDecimal(row["Ago_3"].ToString()),
                //                 Sep_3 = ConvertUtils.ParseDecimal(row["Sep_3"].ToString()),
                //                 Oct_3 = ConvertUtils.ParseDecimal(row["Oct_3"].ToString()),
                //                 Nov_3 = ConvertUtils.ParseDecimal(row["Nov_3"].ToString()),
                //                 Dic_3 = ConvertUtils.ParseDecimal(row["Dic_3"].ToString()),
                //                 Ene_4 = ConvertUtils.ParseDecimal(row["Ene_4"].ToString()),
                //                 Feb_4 = ConvertUtils.ParseDecimal(row["Feb_4"].ToString()),
                //                 Mar_4 = ConvertUtils.ParseDecimal(row["Mar_4"].ToString()),
                //                 Abr_4 = ConvertUtils.ParseDecimal(row["Abr_4"].ToString()),
                //                 May_4 = ConvertUtils.ParseDecimal(row["May_4"].ToString()),
                //                 Jun_4 = ConvertUtils.ParseDecimal(row["Jun_4"].ToString()),
                //                 Jul_4 = ConvertUtils.ParseDecimal(row["Jul_4"].ToString()),
                //                 Ago_4 = ConvertUtils.ParseDecimal(row["Ago_4"].ToString()),
                //                 Sep_4 = ConvertUtils.ParseDecimal(row["Sep_4"].ToString()),
                //                 Oct_4 = ConvertUtils.ParseDecimal(row["Oct_4"].ToString()),
                //                 Nov_4 = ConvertUtils.ParseDecimal(row["Nov_4"].ToString()),
                //                 Dic_4 = ConvertUtils.ParseDecimal(row["Dic_4"].ToString()),
                //                 Total = ConvertUtils.ParseDecimal(row["Total"].ToString()),
                //             });
                //         }
                //     }
                // });

                //Task.WaitAll( task1, task2);

                return new Recaudacion_Analitico(){
                    Analitico_Mensual = analiticoIngAnual.ToArray(),
                    Analitico_Quincenal = analiticoIngQuincenal.ToArray(),
                    Analitico_Semanal = analiticoIngSemanal.ToArray(),
                    AnaliticoRez_Mensual = analiticoRezAnual.ToArray(),
                    AnaliticoRez_Quincenal = analiticoRezQuincenal.ToArray(),
                    AnaliticoRez_Semanal = analiticoRezSemanal.ToArray(),
                };

            }catch(Exception err){
                Console.WriteLine($"Error al obtener analisis de ingresos de {enlace.Nombre}\n\t {err.Message}\n\t{err.StackTrace}");
                return null;
            }

        }
        public IEnumerable<Recaudacion_Rezago> ObtenerRezago(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect) {
            
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(timeoutWCFConnection);

            try{

                // Realizar consulta
                var resultService = Task.Run( 
                    async () => await serviceClient.Fun_Recaudacion_8Async((short) enlace.Id, desde, hasta, (short) sb, (short) sect),
                    cancellationSource.Token
                ).GetAwaiter().GetResult();

                // Procesar respuesta
                var response = new List<Recaudacion_Rezago>();
                foreach(var item in resultService){
                    var newItem = new Recaudacion_Rezago(){
                        Mes = item.mes,
                        Usuarios = (int) item.usuarios,
                        Rez_agua = (decimal) item.r_agua,
                        Rez_dren = (decimal) item.r_dren,
                        Rez_sane = (decimal) item.r_dren,
                        Rez_otros = (decimal) item.r_otros,
                        Rez_recargos = (decimal) item.r_recargos,
                        Subtotal = (decimal) item.subtotal,
                        Iva = (decimal) item.iva,
                        Total = (decimal) item.total
                    };
                    response.Add(newItem);
                }
                return response;

            }catch(Exception err){
                Console.WriteLine($"Error al obtener el rezago de {enlace.Nombre}\n\t{err.Message}");
                return null;
            }

        }
        public IEnumerable<Recaudacion_IngresosDias> ObtenerIngresosPorDias(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect) {
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(timeoutWCFConnection);

            try{

                // Realizar consulta
                var resultService = Task.Run( 
                    async () => await serviceClient.Fun_Recaudacion_0Async((short) enlace.Id, desde, hasta, (short) sb, (short) sect),
                    cancellationSource.Token
                ).GetAwaiter().GetResult();

                // Procesar respuesta
                var response = new List<Recaudacion_IngresosDias>();
                foreach(var item in resultService){
                    var newItem = new Recaudacion_IngresosDias(){
                        Fecha112 = item.dia,
                        Fecha_Letras = item.fecha,
                        Usuarios_Propios = (int) item.u1,
                        Usuarios_Otros = (int) item.u2,
                        Usuarios_Total = (int) item.Total,
                        Ingresos_Propios = (decimal) item.i1,
                        Ingresos_Otros = (decimal) item.i2,
                        Importe_Total = (decimal) item.Total,
                        Cobrado = (decimal) item.Cobrado
                    };
                    response.Add(newItem);
                }
                return response;

            }catch(Exception err){
                Console.WriteLine($"Error al obtener Ingresos por Dias de {enlace.Nombre}\n\t{err.Message}");
                return null;
            }
        }
        public IEnumerable<Recaudacion_IngresosCajas> ObtenerIngresosPorCajas(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect) {
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(timeoutWCFConnection);

            try{

                // Realizar consulta
                var resultService = Task.Run( 
                    async () => await serviceClient.Fun_Recaudacion_9Async((short) enlace.Id, desde, hasta, (short) sb, (short) sect),
                    cancellationSource.Token
                ).GetAwaiter().GetResult();

                // Procesar respuesta
                var dataSet = new DataSet();
                var stringReader = new StringReader(resultService);

                Console.WriteLine(resultService);
                dataSet.ReadXmlSchema(stringReader);
                // dataSet.ReadXml(stringReader, XmlReadMode.ReadSchema);

                Console.WriteLine("Total tabla: " + dataSet.Tables.Count);
                stringReader.Dispose();

                var response = new List<Recaudacion_IngresosCajas>();
                foreach( DataRow row in dataSet.Tables[0].Rows){
                    var newItem = new Recaudacion_IngresosCajas(){
                        Caja = row["caja"].ToString(),
                        Facturado = ConvertUtils.ParseDecimal( row["facturado"].ToString()),
                        Cobrado = ConvertUtils.ParseDecimal( row["cobrado"].ToString()),
                        Recibos = ConvertUtils.ParseInteger( row["recibos"].ToString())
                    };
                    response.Add(newItem);
                }
                return response;

            }catch(Exception err){
                Console.WriteLine($"Error al obtener el Ingresos por Cajas de {enlace.Nombre}\n\t{err.Message}");
                return null;
            }
        }
        public IEnumerable<IngresosxConceptos> ObtenerIngresosPorConceptos(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect) {
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(timeoutWCFConnection);

            try{

                // Realizar consulta
                var resultService = Task.Run( 
                    async () => await serviceClient.Fun_Recaudacion_2Async((short) enlace.Id, desde, hasta, (short) sb, (short) sect),
                    cancellationSource.Token
                ).GetAwaiter().GetResult();

                // Procesar respuesta
                var response = new List<IngresosxConceptos>();
                foreach(var item in resultService){
                    var newItem = new IngresosxConceptos(){
                        Id_Concepto = (int) item.Concepto,
                        Descripcion = item.Descripcion,
                        Conc_Con_Iva = (decimal) item.Con_Iva,
                        Iva = (decimal) item.IVA,
                        Aplicado_Con_Iva = (decimal) item.Con_IVA_Total,
                        Conc_Sin_Iva = (decimal) item.Sin_Iva,
                        Total_Aplicado = (decimal) item.Total,
                        Usuarios = (int) item.Usuarios,
                    };
                    response.Add(newItem);
                }
                return response;

            }catch(Exception err){
                Console.WriteLine($"Error al obtener Ingresos por Conceptos de {enlace.Nombre}\n\t{err.Message}");
                return null;
            }
        }
        
        public IEnumerable<Ingresos_Conceptos> ObtenerIngresosPorConceptosTipoUsuarios(IEnlace enlace, DateTime desde , DateTime hasta, int sb , int sect) {
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(timeoutWCFConnection);

            try{

                // Realizar consulta
                var resultService = Task.Run( 
                    async () => await serviceClient.Fun_Recaudacion_3Async((short) enlace.Id, desde, hasta, (short) sb, (short) sect),
                    cancellationSource.Token
                ).GetAwaiter().GetResult();

                // Procesar respuesta
                var dataSet = new DataSet();
                var stringReader = new StringReader(resultService);
                dataSet.ReadXml(resultService);
                stringReader.Dispose();

                var response = new List<Ingresos_Conceptos>();
                foreach( DataRow row in dataSet.Tables[0].Rows){
                    var newItem = new Ingresos_Conceptos(){
                        Id_Concepto = ConvertUtils.ParseInteger(row["Id_Concepto"].ToString()), 
                        Concepto =  row["Descripcion"].ToString(), 
                        Subtotal = ConvertUtils.ParseDecimal(row["Subtotal"].ToString()), 
                        Iva = ConvertUtils.ParseDecimal(row["IVA"].ToString()), 
                        Total = ConvertUtils.ParseDecimal(row["Total"].ToString()), 
                        Domestico_Sub = ConvertUtils.ParseDecimal(row["Sb1"].ToString()), 
                        Domestico_Iva = ConvertUtils.ParseDecimal(row["IVA1"].ToString()), 
                        Domestico_Total = ConvertUtils.ParseDecimal(row["Tot1"].ToString()), 
                        Comercial_Sub = ConvertUtils.ParseDecimal(row["Sb2"].ToString()), 
                        Comercial_Iva = ConvertUtils.ParseDecimal(row["IVA2"].ToString()), 
                        Comercial_Total = ConvertUtils.ParseDecimal(row["Tot2"].ToString()),                         
                        Industrial_Sub = ConvertUtils.ParseDecimal(row["Sb3"].ToString()), 
                        Industrial_Iva = ConvertUtils.ParseDecimal(row["IVA3"].ToString()), 
                        Industrial_Total = ConvertUtils.ParseDecimal(row["Tot3"].ToString()),                        
                        Publico_Sub = ConvertUtils.ParseDecimal(row["Sb4"].ToString()), 
                        Publico_Iva = ConvertUtils.ParseDecimal(row["IVA4"].ToString()), 
                        Publico_Total = ConvertUtils.ParseDecimal(row["Tot4"].ToString())
                    };
                    response.Add(newItem);
                }
                return response;


            }catch(Exception err){
                Console.WriteLine($"Error al obtener Ingresos por Conceptos de {enlace.Nombre}\n\t{err.Message}");
                return null;
            }
        }
        public IEnumerable<IngresosTipoUsuario> ObtenerIngresosPorTipoUsuarios(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect) {
            throw new NotImplementedException();
        }
        public IEnumerable<Ingresos_FormasPago> ObtenerIngresosPorFormasPago(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect) {
            throw new NotImplementedException();
        }
        public Recaudacion_PagosMayores_Response ObtenerPagosMayores(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect, int total) {
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(timeoutWCFConnection);

            try{

                // Realizar consulta
                var resultService = Task.Run( 
                    async () => await serviceClient.fn_Recaudacion_TopVentasAsync((short) enlace.Id, total, desde.ToString("yyyyMMdd"), hasta.ToString("yyyyMMdd")),
                    cancellationSource.Token
                ).GetAwaiter().GetResult();

                // Procesar respuesta
                var dataSet = new DataSet();
                dataSet.ReadXml(resultService);

                // Obtener registros tabla 1
                var tmpData1 = new List<Recaudacion_PagosMayores>();
                var task1 = Task.Run( () => {
                    var table1 = dataSet.Tables[0];
                    foreach( DataRow row in table1.Rows){
                        var newItem = new Recaudacion_PagosMayores(){
                            Id_Padron = ConvertUtils.ParseInteger( row["id_padron"].ToString()),
                            Id_Cuenta = ConvertUtils.ParseInteger( row["id_cuenta"].ToString()),
                            Fecha = ConvertUtils.ParseDateTime( row["fecha"].ToString())?? new DateTime(),
                            Subtotal = ConvertUtils.ParseDecimal( row["subtotal"].ToString()),
                            Iva = ConvertUtils.ParseDecimal( row["iva"].ToString()),
                            Total = ConvertUtils.ParseDecimal( row["total"].ToString()),
                            Sucursal = row["sucursal"].ToString(),
                            Id_Venta = row["id_venta"].ToString(),
                            Nombre = row["nombre"].ToString(),
                            Direccion = row["direccion"].ToString(),
                            Id_Publico = ConvertUtils.ParseInteger( row["id_publico"].ToString()),
                        };
                        lock(tmpData1){
                            tmpData1.Add(newItem);
                        }
                    }
                });

                // Obtener registros tabla 2
                var tmpData2 = new List<Recaudacion_PagosMayores_Items>();
                var task2 = Task.Run( () => {
                    var table2 = dataSet.Tables[1];
                    foreach( DataRow row in table2.Rows){
                        var newItem = new Recaudacion_PagosMayores_Items(){
                            Id_Venta = row["id_venta"].ToString(),
                            Concepto = row["concepto"].ToString(),
                            Subtotal = ConvertUtils.ParseDecimal( row["subtotal"].ToString()),
                            Iva = ConvertUtils.ParseDecimal( row["iva"].ToString()),
                            Total = ConvertUtils.ParseDecimal( row["total"].ToString())
                        };
                        lock(tmpData2){
                            tmpData2.Add(newItem);
                        }
                    }
                });

                Task.WaitAll( task1, task2);
                
                var response = new Recaudacion_PagosMayores_Response(){
                    PagosMayores = tmpData1.ToArray(),
                    PagosMayores_Detalle = tmpData2.ToArray()
                };
                return response;


            }catch(Exception err){
                Console.WriteLine($"Error al obtener Pagos mayores de {enlace.Nombre}\n\t{err.Message}");
                return null;
            }
        }
        public IEnumerable<Recaudacion_IngresosDetalle> ObtenerDetalleIngresos(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect) {
            throw new NotImplementedException();
        }
        public IEnumerable<Recaudacion_IngresosDetalleConceptos> ObtenerDetalleConceptos(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect, int id_poblacion, int id_colonia) {
            throw new NotImplementedException();
        }
        public IEnumerable<Recaudacion_Rezago_Detalle> ObtenerDetalleRezago(IEnlace enlace, DateTime desde, DateTime hasta, int sub, int sect, int mes, bool acumulativo) {
            throw new NotImplementedException();
        }
        public IEnumerable<ConceptoTipoUsuario> ObtenerRecaudacionPorConceptosYTipoUsuario(IEnlace enlace, DateTime desde, DateTime hasta, int sub, int sect ) {
            throw new NotImplementedException();
        }
        public IEnumerable<RecaudacionIngresosxPoblaciones> ObtenerRecaudacionLocalidades(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sec) {
            throw new NotImplementedException();

            // var dsString = Task.Run( async () => await serviceClient.Fun_Recaudacion_10Async( (short) enlace.Id, desde, hasta, (short) sb, (short) sec)).GetAwaiter().GetResult();
            // var dsIngresos = new DataSet();
            // StringReader txtReader = new StringReader(dsString);
            // dsIngresos.ReadXml(txtReader);
            // txtReader.Close();

            // Procesar dataSet

        }
        public IEnumerable<RecaudacionIngresosxColonias> ObtenerRecaudacionColonias(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sec, int idLocalidad) {
            throw new NotImplementedException();
        }
        public IEnumerable<Recaudacion_IngresosDetalleConceptos> ObtenerIngresosConceptosPorLocalidadColonia(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect, int idLocalidad, int idColonia) {
            throw new NotImplementedException();
        }


        public IEnumerable<Ingreso_Gravamen> IngresosGravamen(IEnlace enlace, DateTime desde, DateTime hasta, int sb, int sect){
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(timeoutWCFConnection);

            try{

                // Realizar consulta
                Recaudacion_3[] resultService = Task.Run( 
                    async () => await serviceClient.Fun_Recaudacion_7Async((short) enlace.Id, desde, hasta, (short) sb, (short) sect),
                    cancellationSource.Token
                ).GetAwaiter().GetResult();

                // Procesar respuesta
                var response = new List<Ingreso_Gravamen>();
                foreach(var item in resultService){
                    var newItem = new Ingreso_Gravamen(){
                        Id_Concepto = (int) item.Concepto,
                        Concepto = item.Descripcion,
                        Importe1 = (decimal) item.Imp_1,
                        Importe2 = (decimal) item.Imp_2,
                        Importe3 = (decimal) item.Imp_3,
                        Importe4 = (decimal) item.Imp_4,
                        Total = (decimal) item.Total,
                        Usuarios1 = item.Usu_1,
                        Usuarios2 = item.Usu_2,
                        Usuarios3 = item.Usu_3,
                        Usuarios4 = item.Usu_4,
                        Usuarios = item.Usuarios,
                    };
                    response.Add(newItem);
                }
                return response;

            }catch(Exception err){
                Console.WriteLine($"Error al obtener el rezago de {enlace.Nombre}\n\t{err.Message}");
                return null;
            }

        }


    }
}