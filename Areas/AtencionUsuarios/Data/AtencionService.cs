using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Services;
using Microsoft.Extensions.Logging;

namespace Sicem_Blazor.Services {
    public class AtencionService : IAtencionService {
        private readonly IConfiguration appSetting;
        private readonly SicemService sicemService;
        private readonly ILogger<IAtencionService> logger;

        public AtencionService(IConfiguration c, SicemService s, ILogger<IAtencionService> l) {
            this.appSetting = c;
            this.sicemService = s;
            this.logger = l;
        }


        public Atencion_Resumen ObtenerResumenOficina(IEnlace enlace, DateTime fecha1, DateTime fecha2, int sb, int sect){

            var respuesta = new Atencion_Resumen();
            respuesta.Enlace = enlace;
            
            try{
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())){
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        
                        xCommand.CommandText = $"Exec [Sicem].[Atencion]  @cAlias = 'RESUMEN', @cFecha1 = '{fecha1.ToString("yyyyMMdd")}', @cFecha2 = '{fecha2.ToString("yyyyMMdd")}'";

                        var xDataSet = new DataSet();
                        var xDataAdapter = new SqlDataAdapter(xCommand);
                        xDataAdapter.Fill(xDataSet);

                        if(xDataSet.Tables.Count != 3) {
                            throw new Exception("Error al llamar el procedure");
                        }

                        // ****** Obtener Totales Atencion ******
                        DataRow xRow = xDataSet.Tables[0].Rows[0];
                        respuesta.Total = int.Parse(xRow["total"].ToString());
                        respuesta.Pendiente = int.Parse(xRow["pendiente"].ToString());
                        respuesta.Atendido = int.Parse(xRow["atendido"].ToString());
                        respuesta.Resuelto = int.Parse(xRow["resuelto"].ToString());
                        respuesta.Sin_resolver = int.Parse(xRow["sin_resolver"].ToString());

                        // ****** Turnos Por Personal Genero ******
                        var tmpList = new List<Atencion_Resumen_Genero>();
                        foreach(DataRow xRow1 in xDataSet.Tables[1].Rows) {
                            tmpList.Add(new Atencion_Resumen_Genero {
                                Id_Genero = xRow1["id_genero"].ToString(),
                                Genero = xRow1["genero"].ToString(),
                                Total = int.Parse(xRow1["total"].ToString()),
                                Pendiente = int.Parse(xRow1["pendiente"].ToString()),
                                Atendido = int.Parse(xRow1["atendido"].ToString()),
                                Resuelto = int.Parse(xRow1["resuelto"].ToString()),
                                Sin_resolver = int.Parse(xRow1["sin_resolver"].ToString()),
                            });
                        }
                        respuesta.Atencion_Generacion = tmpList.ToArray<Atencion_Resumen_Genero>();

                        // ****** Totales Descuentos ******
                        DataRow xRow2 = xDataSet.Tables[2].Rows[0];
                        respuesta.Descuentos = int.Parse(xRow2["descuentos"].ToString());
                        respuesta.Imp_Descuentos = decimal.Parse(xRow2["imp_desc"].ToString());
                        respuesta.Convenios = int.Parse(xRow2["convenios"].ToString());
                        respuesta.Imp_Convenios = decimal.Parse(xRow2["imp_conv"].ToString());
                    }                    
                    xConnecton.Close();
                }               
                respuesta.IdEstatus = 1;
            }catch(Exception err){
                logger.LogError(err, $"Error al tratar de obtener el resumen de atencion del enlace: {enlace.Nombre}");
                respuesta.IdEstatus = 2;
            }

            return respuesta;
        }
        public IEnumerable<Atencion_Detalle> ObtenerAtencionDetalle(IEnlace enlace, DateTime Fecha1, DateTime Fecha2, int Sb, int Sect, string Filtro) {
            var respuesta = new List<Atencion_Detalle>();
            try{
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = $"Exec [SICEM].[Atencion] @cAlias='DETALLE', @cFecha1='{Fecha1.ToString("yyyyMMdd")}', @cFecha2='{Fecha2.ToString("yyyyMMdd")}',@xSb={Sb}, @xSec={Sect}, @xFiltro = '{GenerarFiltro(Filtro)}'";
                        using(var xReader = xCommand.ExecuteReader()) {
                            while(xReader.Read()) {
                                respuesta.Add(new Atencion_Detalle {
                                    Folio = xReader["folio"].ToString(),
                                    Cuenta = long.Parse(xReader["cuenta"].ToString()),
                                    Nombre = xReader["nombre"].ToString(),
                                    Id_Colonia = int.Parse(xReader["id_colonia"].ToString()),
                                    Colonia = xReader["colonia"].ToString(),
                                    Id_Estatus = int.Parse(xReader["id_estatus"].ToString()),
                                    Estatus = xReader["estatus"].ToString(),
                                    Id_Asunto = int.Parse(xReader["id_asunto"].ToString()),
                                    Asunto = xReader["asunto"].ToString(),
                                    Descripcion = xReader["descripcion"].ToString(),
                                    Resultado = xReader["resultado"].ToString(),
                                    Fecha_Genero = DateTime.Parse(xReader["fecha_genero"].ToString()),
                                    Id_Genero = xReader["id_genero"].ToString(),
                                    Genero = xReader["genero"].ToString()
                                });
                            }
                        }
                    }
                    xConnecton.Close();
                }
            }catch(Exception err){
                logger.LogError(err, $"Error al tratar de obtener el detalle de atencion del enlace: {enlace.Nombre}");
                return new Atencion_Detalle[]{};
            }
            return respuesta;
        }
        public IEnumerable<Atencion_Descuento> ObtenerDescuentos(IEnlace enlace, DateTime Fecha1, DateTime Fecha2, int Sb, int Sect) {
            var respuesta = new List<Atencion_Descuento>();
            try{
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = $"Exec [SICEM].[Atencion] @cAlias='DESCTOS', @cFecha1='{Fecha1.ToString("yyyyMMdd")}', @cFecha2='{Fecha2.ToString("yyyyMMdd")}', @xSb={Sb}, @xSec={Sect}";
                        using(var xReader = xCommand.ExecuteReader()) {
                            while(xReader.Read()) {
                                respuesta.Add(new Atencion_Descuento {
                                    Folio = xReader["folio"].ToString(),
                                    Sb = int.Parse(xReader["sb"].ToString()),
                                    Sector = int.Parse(xReader["sector"].ToString()),
                                    Fecha = DateTime.Parse(xReader["fecha"].ToString()).ToString("dd/MM/yyyy"),
                                    Contrato = long.Parse(xReader["contrato"].ToString()),
                                    Nombre = xReader["a_nombre_de"].ToString(),
                                    Id_TipoUsuario = int.Parse(xReader["id_tipousuario"].ToString()),
                                    Tipo_Usuario = xReader["tipo_usuario"].ToString(),
                                    Id_Ajuste = xReader["id_ajuste"].ToString(),
                                    Tipo_ajuste = xReader["tipo_ajuste"].ToString(),
                                    MesesAdeudo = int.Parse(xReader["M.A."].ToString()),
                                    Adeudo_Inicial = decimal.Parse(xReader["adeudo_inicial"].ToString()),
                                    Importe_Ajustado = decimal.Parse(xReader["importe_ajustado"].ToString()),
                                    Saldo_Cuenta = decimal.Parse(xReader["saldo_cuenta"].ToString()),
                                    Id_Genero = xReader["id_genero"].ToString(),
                                    Genero = xReader["genero"].ToString()
                                });
                            }
                        }
                    }
                    xConnecton.Close();
                }
            }catch(Exception err){
                logger.LogError(err, $"Error al tratar de obtener los descuentos del enlace: {enlace.Nombre}");
                return new Atencion_Descuento[]{};
            }
            return respuesta;
        }
        public IEnumerable<Atencion_Convenios> ObtenerConvenios(IEnlace enlace, DateTime Fecha1, DateTime Fecha2, int Sb, int Sect) {
            var respuesta = new List<Atencion_Convenios>();      
            try{
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = $"Exec [SICEM].[Atencion] @cAlias='CONVENIOS', @cFecha1='{Fecha1.ToString("yyyyMMdd")}', @cFecha2='{Fecha2.ToString("yyyyMMdd")}', @xSb={Sb}, @xSec={Sect}";
                        using(var xReader = xCommand.ExecuteReader()) {
                            while(xReader.Read()) {
                                respuesta.Add(new Atencion_Convenios {
                                    Folio = xReader["folio"].ToString(),
                                    Fecha = DateTime.Parse(xReader["fecha"].ToString()),
                                    Concepto = xReader["concepto"].ToString(),
                                    Contrato = long.Parse(xReader["contrato"].ToString()),
                                    Nombre = xReader["a_nombre_de"].ToString(),
                                    Adeudo = decimal.Parse(xReader["adeudo"].ToString()),
                                    MesesAdeudo = int.Parse(xReader["M.A."].ToString()),
                                    Anticipo = decimal.Parse(xReader["anticipo"].ToString()),
                                    Convenio = decimal.Parse(xReader["convenio"].ToString()),
                                    Saldo = decimal.Parse(xReader["saldo"].ToString()),
                                    Parcialidades = int.Parse(xReader["parcialidades"].ToString()),
                                    Id_TipoUsuario = int.Parse(xReader["id_tipoUsuario"].ToString()),
                                    TipoUsuario = xReader["tipoUsuario"].ToString()
                                });
                            }
                        }
                    }
                    xConnecton.Close();
                }
            }
            catch(Exception err){
                logger.LogError(err, $"Error al tratar de obtener los convenios del enlace: {enlace.Nombre}");
                return new Atencion_Convenios[]{};
            }
            return respuesta;
        }
        public IEnumerable<Atencion_Group> ObtenerAtencionPorColonias(IEnlace enlace, DateTime Fecha1, DateTime Fecha2, int Sb, int Sect) {
            var respuesta = new List<Atencion_Group>();
            try{
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = $"Exec [Sicem].[Atencion] @cAlias='POR-COLONIAS', @cFecha1='{Fecha1.ToString("yyyyMMdd")}', @cFecha2='{Fecha2.ToString("yyyyMMdd")}', @xSb={Sb}, @xSec={Sect}";
                        using(var xReader = xCommand.ExecuteReader()) {
                            while(xReader.Read()) {
                                respuesta.Add(new Atencion_Group {
                                    Id = int.Parse(xReader["id_colonia"].ToString()),
                                    Descripcion = xReader["colonia"].ToString(),
                                    Pendiente = int.Parse(xReader["pendi"].ToString()),
                                    Atendido = int.Parse(xReader["atend"].ToString()),
                                    Resuelto = int.Parse(xReader["res"].ToString()),
                                    Sin_Resolver = int.Parse(xReader["sin_res"].ToString()),
                                    Total = int.Parse(xReader["total"].ToString())
                                });
                            }
                        }
                    }
                    xConnecton.Close();
                }
            }catch(Exception err){
                logger.LogError(err, $"Error al tratar de obtener el resumen de atencion por colonias del enlace: {enlace.Nombre}");
                return new Atencion_Group[]{};
            }
            return respuesta.ToArray();
        }
        public IEnumerable<Atencion_Group> ObtenerAtencionPorAsuntos(IEnlace enlace,  DateTime Fecha1, DateTime Fecha2, int Sb, int Sect) {
            var respuesta = new List<Atencion_Group>();            
            try{
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = $"Exec [Sicem].[Atencion] @cAlias='POR-ASUNTOS', @cFecha1='{Fecha1.ToString("yyyyMMdd")}', @cFecha2='{Fecha2.ToString("yyyyMMdd")}', @xSb={Sb}, @xSec={Sect}";
                        using(var xReader = xCommand.ExecuteReader()) {
                            while(xReader.Read()) {
                                respuesta.Add(new Atencion_Group {
                                    Id = int.Parse(xReader["id_asunto"].ToString()),
                                    Descripcion = xReader["asunto"].ToString(),
                                    Pendiente = int.Parse(xReader["pendi"].ToString()),
                                    Atendido = int.Parse(xReader["atend"].ToString()),
                                    Resuelto = int.Parse(xReader["res"].ToString()),
                                    Sin_Resolver = int.Parse(xReader["sin_res"].ToString()),
                                    Total = int.Parse(xReader["total"].ToString())
                                });
                            }
                        }
                    }
                    xConnecton.Close();
                }
            }catch(Exception err){
                logger.LogError(err, $"Error al tratar de obtener el resumen de atencion por asuntos del enlace: {enlace.Nombre}");
                return new Atencion_Group[]{};
            }
            return respuesta;
        }
        public IEnumerable<Atencion_Group> ObtenerAtencionPorOrigen(IEnlace enlace,  DateTime Fecha1, DateTime Fecha2, int Sb, int Sect) {
            var respuesta = new List<Atencion_Group>();            
            try{
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = $"Exec [Sicem].[Atencion] @cAlias='POR-ORIGEN', @cFecha1='{Fecha1.ToString("yyyyMMdd")}', @cFecha2='{Fecha2.ToString("yyyyMMdd")}', @xSb={Sb}, @xSec={Sect}";
                        Console.WriteLine("Connection:" + enlace.GetConnectionString());
                        using(var xReader = xCommand.ExecuteReader()) {
                            while(xReader.Read()) {
                                respuesta.Add(new Atencion_Group {
                                    Id = int.Parse(xReader["id_origen"].ToString()),
                                    Descripcion = xReader["origen"].ToString(),
                                    Pendiente = int.Parse(xReader["pendi"].ToString()),
                                    Atendido = int.Parse(xReader["atend"].ToString()),
                                    Resuelto = int.Parse(xReader["res"].ToString()),
                                    Sin_Resolver = int.Parse(xReader["sin_res"].ToString()),
                                    Total = int.Parse(xReader["total"].ToString())
                                });
                            }
                        }
                    }
                    xConnecton.Close();
                }
            }catch(Exception err){
                logger.LogError(err, $"Error al tratar de obtener el resumen de atencion por asuntos del enlace: {enlace.Nombre}");
                return new Atencion_Group[]{};
            }
            return respuesta;
        }


        public Atencion_Anual[] ObtenerAtencionAnual(int Id_Oficina, DateTime Fecha1, int Sb, int Sect, string Filtro) {
            var respuesta = new List<Atencion_Anual>();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"Exec [SICEM].[Atencion] @cAlias='ANUAL', @cFecha1='{Fecha1}', @xSb={Sb}, @xSec={Sect}";
                    using(var xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            respuesta.Add(new Atencion_Anual {
                                Id_asunto = int.Parse(xReader["id_asunto"].ToString()),
                                Asunto = xReader["asunto"].ToString(),
                                Ene = int.Parse(xReader["ene"].ToString()),
                                Feb = int.Parse(xReader["feb"].ToString()),
                                Mar = int.Parse(xReader["mar"].ToString()),
                                Abr = int.Parse(xReader["abr"].ToString()),
                                May = int.Parse(xReader["may"].ToString()),
                                Jun = int.Parse(xReader["jun"].ToString()),
                                Jul = int.Parse(xReader["jul"].ToString()),
                                Ago = int.Parse(xReader["ago"].ToString()),
                                Sep = int.Parse(xReader["sep"].ToString()),
                                Oct = int.Parse(xReader["oct"].ToString()),
                                Nov = int.Parse(xReader["nov"].ToString()),
                                Dic = int.Parse(xReader["dic"].ToString()),
                                Total = int.Parse(xReader["total"].ToString())
                            });
                        }
                    }
                }
            }
            return respuesta.ToArray();
        }
        private string GenerarFiltro(string param){
            var stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("<filtro>");
            var _data = param.Split(",");
            foreach(var x in _data){
                if(x.Contains("id_estatus")){
                    var val = x.Split(":").Last();
                    stringBuilder.Append($"<id_estatus>{val}</id_estatus>");
                }
                if(x.Contains("id_colonia")){
                    var val = x.Split(":").Last();
                    stringBuilder.Append($"<id_colonia>{val}</id_colonia>");
                }
                if(x.Contains("id_asunto")){
                    var val = x.Split(":").Last();
                    stringBuilder.Append($"<id_asunto>{val}</id_asunto>");
                }
                if(x.Contains("id_genero")){
                    var val = x.Split(":").Last();
                    stringBuilder.Append($"<id_genero>{val}</id_genero>");
                }
                if(x.Contains("id_origen")){
                    var val = x.Split(":").Last();
                    stringBuilder.Append($"<id_origen>{val}</id_origen>");
                }
            }
            stringBuilder.Append("</filtro>");
            return stringBuilder.ToString();
        }

    }
}
