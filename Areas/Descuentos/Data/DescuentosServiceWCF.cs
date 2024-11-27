using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.Data;
using Sicem_Blazor.Models;
using Sicem_Blazor.Recaudacion.Models;
using Sicem_Blazor.Descuentos.Models;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data.Contracts;
using ServiceReference;
using System.Threading;

namespace Sicem_Blazor.Services {
    public class DescuentosServiceWCF {
        private readonly IConfiguration appSettings;
        private readonly SicemService sicemService;
        private readonly ILogger<DescuentosService> logger;

        private ServiceClient serviceClient;

        public DescuentosServiceWCF(IConfiguration c, SicemService s, ILogger<DescuentosService> l) {
            this.appSettings = c;
            this.sicemService = s;
            this.logger = l;

            this.serviceClient = new ServiceReference.ServiceClient();
        }

        public DescuentosTotal ObtenerDescuentosOficina(IEnlace enlace, DateTime fecha1, DateTime fecha2, int sb, int sect) {
            
            Console.WriteLine($">> Obtener descuentos {enlace.Nombre} desde WCF");
            
            // Generar cad Oficina
            var cadConexion = CadOficinasGenerator.Genearar(enlace.Id);

            try{

                var cancelationToken = new CancellationTokenSource();
                cancelationToken.CancelAfter(TimeSpan.FromMinutes(15));

                // Realizar consulta
                Recaudacion_1[] response = Task.Run( 
                    async () => await serviceClient.Fun_Descuentos_1Async(cadConexion, fecha1, fecha2, TiposDescuento.TODOS),
                    cancelationToken.Token
                ).GetAwaiter().GetResult();

                //Procesar respuesta
                var firstItem = response.FirstOrDefault<Recaudacion_1>();
                if(firstItem == null){
                    throw new Exception("No hay datos de respuesta");
                }

                // Procesar datos
                var totales = new DescuentosTotal(){
                    Estatus = ResumenOficinaEstatus.Completado,
                    Conc_Con_Iva = (decimal) firstItem.Con_Iva,
                    Iva = (decimal) firstItem.IVA,
                    Aplicado_Con_Iva = (decimal) firstItem.Con_IVA_Total,
                    Conc_Sin_Iva = (decimal) firstItem.Sin_Iva,
                    Total_Aplicado = (decimal) firstItem.Total,
                    Usuarios = firstItem.Usuarios
                };
                return totales;
            }catch(Exception err){
                Console.WriteLine($"Error al obtener descuentos de {enlace.Nombre} \n{err.Message}");
                return new DescuentosTotal(){
                    Estatus = ResumenOficinaEstatus.Error,
                };
            }
        }

        public IEnumerable<DescuentosConcepto> ObtenerDescuentosConceptos(IEnlace enlace, DateTime fecha1, DateTime fecha2) {
           Console.WriteLine($">> Obtener descuentos por conceptos {enlace.Nombre} desde WCF");
            
            try{
                
                // Realizar consulta
                Recaudacion_1[] wcfResponse = Task.Run( 
                    async () => await serviceClient.Fun_Descuentos_2Async((short)enlace.Id, fecha1, fecha2, TiposDescuento.TODOS)
                ).GetAwaiter().GetResult();

                // Procesar consulta
                var response = new List<DescuentosConcepto>();
                foreach(var item in wcfResponse){
                    var newItem = new DescuentosConcepto(){
                        Id_Concepto = item.Concepto,
                        Descripcion = item.Descripcion,
                        Conc_Con_Iva = (decimal) item.Con_Iva,
                        Iva = (decimal) item.IVA,
                        Aplicado_Con_Iva = (decimal) item.Con_IVA_Total,
                        Conc_Sin_Iva = (decimal) item.Sin_Iva,
                        Total_Aplicado = (decimal) item.Total,
                        Usuarios = (int) item.Usuarios
                    };
                    response.Add( newItem);
                }

                return response;

            }catch(Exception err){
                Console.WriteLine($"Error al obtener descuentos por conceptos de {enlace.Nombre} \n{err.Message}");
                return null;
            }

        }
        public IEnumerable<DescuentosUsuarioAutorizo> ObtenerDescuentosAutorizo(IEnlace enlace, DateTime fecha1, DateTime fecha2) {
            Console.WriteLine($">> Obtener descuentos por autorizo de {enlace.Nombre} desde WCF");
            
            try{
                
                // Realizar consulta
                Descuentos_3[] wcfResponse = Task.Run( 
                    async () => await serviceClient.Fun_Descuentos_3Async((short)enlace.Id, fecha1, fecha2, TiposDescuento.TODOS)
                ).GetAwaiter().GetResult();

                // Procesar consulta
                var response = new List<DescuentosUsuarioAutorizo>();
                foreach(var item in wcfResponse){
                    var newItem = new DescuentosUsuarioAutorizo(){
                        CVE = item.CVE,
                        Autorizo = item.AUTORIZO,
                        Subtotal = (decimal) item.SUBTOTAL,
                        Iva = (decimal) item.IVA,
                        Total = (decimal) item.TOTAL,
                        Usuarios = (int) item.USUARIOS
                    };
                    response.Add( newItem);
                }
                
                return response;

            }catch(Exception err){
                Console.WriteLine($"Error al obtener descuentos por autorizo de {enlace.Nombre} \n{err.Message}");
                return null;
            }
        }
        public IEnumerable<DescuentosUsuarioAutorizoDetalle> ObtenerDescuentosAutorizoDetalle(IEnlace enlace, DateTime fecha1, DateTime fecha2, string cveAutorizo) {
            Console.WriteLine($">> Obtener descuentos por autorizo de {enlace.Nombre} desde WCF");
            
            try{
                
                // Realizar consulta
                Descuentos_4[] wcfResponse = Task.Run( 
                    async () => await serviceClient.Fun_Descuentos_4Async((short)enlace.Id, fecha1, fecha2, cveAutorizo, TiposDescuento.TODOS)
                ).GetAwaiter().GetResult();

                // Procesar consulta
                var response = new List<DescuentosUsuarioAutorizoDetalle>();
                foreach(var item in wcfResponse){
                    var newItem = new DescuentosUsuarioAutorizoDetalle(){
                        Id_Abono = item.ID_ABONO,
                        Cuenta = (long) item.CUENTA,
                        Usuarios = item.USUARIO,
                        Colonia = item.COLONIA,
                        Tipo_Usuario = item.TIPO_USUARIO,
                        Cve = item.CVE,
                        Autorizo = item.AUTORIZO,
                        Justifica = item.JUSTIFICA,
                        Agua = (decimal) item.agua,
                        Drenaje = (decimal) item.drenaje,
                        Saneamiento = (decimal) item.sanea,
                        Rez_Agua = (decimal) item.rezagua,
                        Rez_Drenaje = (decimal) item.rezdrenaje,
                        Rez_Saneamiento = (decimal) item.rezsanea,
                        Otros = (decimal) item.otros,
                        Recargos = (decimal) item.recargos,
                        Subtotal = (decimal) item.SUBTOTAL,
                        Iva = (decimal) item.IVA,
                        Total = (decimal) item.TOTAL,
                        Fecha = item.FECHA
                    };
                    response.Add( newItem);
                }
                
                return response;

            }catch(Exception err){
                Console.WriteLine($"Error al obtener descuentos por autorizo de {enlace.Nombre} \n{err.Message}");
                return null;
            }
        }
        public IEnumerable<DescuentosConcepto> ObtenerDescuentosAutorizoDetalle_Conceptos(IEnlace enlace, string cveAbono){
            Console.WriteLine($">> Obtener descuentos detalle conceptos de {enlace.Nombre} desde WCF");
            
            try{
                
                // Realizar consulta
                Descuentos_5[] wcfResponse = Task.Run( 
                    async () => await serviceClient.Fun_Descuentos_5Async((short)enlace.Id, cveAbono)
                ).GetAwaiter().GetResult();

                // Procesar consulta
                var response = new List<DescuentosConcepto>();
                foreach(var item in wcfResponse){
                    var newItem = new DescuentosConcepto(){
                        Id_Concepto = (int) item.CVE,
                        Descripcion = item.CONCEPTO,
                        Conc_Sin_Iva = (decimal) item.SUBTOTAL,
                        Iva = (decimal) item.IVA,
                        Total_Aplicado = (decimal) item.TOTAL
                    };
                    response.Add( newItem);
                }
                
                return response;

            }catch(Exception err){
                Console.WriteLine($"Error al obtener descuentos detalle conceptos de {enlace.Nombre} \n{err.Message}");
                return null;
            }
       
        }
        public IEnumerable<DescuentosClave> ObtenerDescuentosAutorizoDetalle_Claves(IEnlace enlace, string cveAbono){
            Console.WriteLine($">> Obtener descuentos detalle claves de {enlace.Nombre} desde WCF");
            
            try{
                
                // Realizar consulta
                Descuentos_6[] wcfResponse = Task.Run( 
                    async () => await serviceClient.Fun_Descuentos_6Async((short)enlace.Id, cveAbono)
                ).GetAwaiter().GetResult();

                // Procesar consulta
                var response = new List<DescuentosClave>();
                foreach(var item in wcfResponse){
                    var newItem = new DescuentosClave(){
                        Cve_Descuento = item.CVE_DESCTO,
                        Conc_Sin_Iva = (decimal) item.SUBTOTAL,
                        Iva = (decimal) item.IVA,
                        Total = (decimal) item.TOTAL,
                    };
                    response.Add( newItem);
                }
                
                return response;

            }catch(Exception err){
                Console.WriteLine($"Error al obtener descuentos detalle claves de {enlace.Nombre} \n{err.Message}");
                return null;
            }
       
        }
    }

    public class TiposDescuento {
        public static readonly int TODOS = 3;
        public static readonly int DESCUENTOS_USUARIOS = 1;
        public static readonly int DESCUENTOS_ESCUELAS = 2;
    }
}