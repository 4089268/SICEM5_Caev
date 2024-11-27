using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.Services {
    public class ConsultaGralService {
        private readonly IConfiguration appSettings;
        private readonly SicemService sicemService;
        private readonly ILogger<ConsultaGralService> logger;
        public ConsultaGralService(IConfiguration c, SicemService s, ILogger<ConsultaGralService> log) {
            this.appSettings = c;
            this.sicemService = s;
            this.logger = log;
        }

        public ConsultaGralResponse ConsultaGeneral(int Id_Oficina, string IdCuenta) {
            var respuesta = new ConsultaGralResponse();
            Ruta xEnlace;
            
            logger.LogInformation($"(-) Obtener enlaces de oficina id: {Id_Oficina}" );
            xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            
            if(xEnlace == null){
                logger.LogError(0, null, $"(-) ConsultaGeneral; Error al obtener enlace de oficina id {Id_Oficina}; xEnlace == null.");
                return null;
            }

            try {

                /*********Datos Generales *********/
                logger.LogInformation("(-) Obteniendo datos generales vista cuenta:{Cuenta}", IdCuenta);
                using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    xConnecton.Open();
                    string xQuery1 = $"select top 1 * From [Padron].[vw_Cat_Padron] Where id_cuenta = ${IdCuenta}";
                    var xCommand = new SqlCommand(xQuery1, xConnecton);
                    using(var reader = xCommand.ExecuteReader()) {
                        if(reader.Read()){
                            respuesta.Id_Padron = int.TryParse(reader.GetValue("id_padron").ToString(), out int tmpId)?tmpId:0;                            
                            respuesta.Id_Cuenta = int.TryParse(reader.GetValue("id_cuenta").ToString(), out int tmpIdCuenta)?tmpIdCuenta:0;
                            respuesta.Nom_comercial = reader.GetValue("nom_comercial").ToString();
                            respuesta.Razon_social = reader.GetValue("razon_social").ToString();

                            //*** Cambara si el registro cuenta con calles laterales y en caso de tenerlo concatenarlo a la direccion
                            var _calleLat1 = reader["_calle_lat1"].ToString().Trim();
                            var _calleLat2 = reader["_calle_lat2"].ToString().Trim();
                            if(_calleLat1.Length > 1 || _calleLat2.Length > 1 ){
                                respuesta.Direccion = string.Format("{0} entre {1} y {2} ", reader.GetValue("direccion").ToString(), _calleLat1, _calleLat2).ToUpper();
                            }else{
                                respuesta.Direccion = reader.GetValue("direccion").ToString().ToUpper();
                            }

                            respuesta.Colonia = reader.GetValue("_colonia").ToString();
                            respuesta.Localizacion = reader.GetValue("_localizacion").ToString();
                            respuesta.Estatus = reader.GetValue("_estatus").ToString();
                            respuesta.RFC = reader.GetValue("rfc").ToString();
                            respuesta.Telefono = reader.GetValue("telefono1").ToString();
                            respuesta.Codigo_postal = reader.GetValue("codigo_postal").ToString();
                            respuesta.Giro = reader.GetValue("_giro").ToString();
                            respuesta.ClaseUsuario = reader.GetValue("_claseusuario").ToString();
                            respuesta.Grupo = reader.GetValue("_tipogrupo").ToString();            
                            respuesta.Servicio = reader.GetValue("_servicio").ToString();
                            respuesta.Poblacion = reader.GetValue("_poblacion").ToString();
                            respuesta.TarifaFija = reader.GetValue("_tarifafija").ToString();
                            respuesta.ConsumoFijo = reader.GetValue("consumo_fijo").ToString();
                            respuesta.Calculo = reader.GetValue("_calculo").ToString();
                            respuesta.MesesAdeudo = reader.GetValue("mes_adeudo_act").ToString();
                            respuesta.Situacion_Toma = reader.GetValue("_situacion").ToString();
                            respuesta.Promedio_Act = reader.GetValue("promedio_act").ToString();
                            respuesta.Lectura_Act = reader.GetValue("lectura_act").ToString();
                            respuesta.Lectura_Ant = reader.GetValue("lectura_ant").ToString();
                            respuesta.Consumo_Act = reader.GetValue("consumo_act").ToString();
                            respuesta.FechaLectura_Act = reader.GetValue("_fecha_lectura_act").ToString();
                            respuesta.FechaLectura_Ant = reader.GetValue("_fecha_lectura_ant").ToString();
                            respuesta.DiametroToma = reader.GetValue("_diametro").ToString();
                            respuesta.FechaVencimiento = reader.GetValue("_fecha_vencimiento").ToString();
                            respuesta.Tarifa = reader["_tipousuario"].ToString();
                            respuesta.CalculoAct = reader["_calculo"].ToString();
                            respuesta.ConsumoForzado = reader["consumo_forzado"].ToString();
                            try { respuesta.ConsumoReal = (int.Parse(respuesta.Lectura_Act) - int.Parse(respuesta.Lectura_Ant)).ToString(); }
                            catch(Exception) { respuesta.ConsumoReal = "0"; }
                            respuesta.Hidrocircuito = reader["_hidrocircuito"].ToString();
                            respuesta.TipoFactible = reader["_tipofactible"].ToString();
                            double tmpD = 0.0;
                            respuesta.ImporteFijoAgua = double.TryParse(reader["importe_fijo"].ToString(), out tmpD) ? tmpD : 0.0;
                            respuesta.ImporteFijoDren = double.TryParse(reader["importe_fijo_dren"].ToString(), out tmpD) ? tmpD : 0.0;
                            respuesta.ImporteFijoSane = double.TryParse(reader["importe_fijo_sane"].ToString(), out tmpD) ? tmpD : 0.0;
                            respuesta.NivelSocioeconomico = reader["_tipousuario"].ToString();
                            respuesta.Anomalia_Act = reader["_anomalia_act"].ToString();
                            respuesta.Ciclo = reader["_MesFacturado"].ToString();
                            respuesta.Viviendas = int.TryParse(reader["viviendas"].ToString(), out int tmpV) ? tmpV : 0;
                            respuesta.Descto60 = reader["por_descto"].ToString();
                        }
                    }
                     xConnecton.Close();
                }

                
                /*********Datos Generales2 *********/
                logger.LogInformation("(-) Obteniendo datos generales cuenta:{Cuenta}", IdCuenta);
                using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    xConnecton.Open();
                    string xQuery11 = String.Format(@"Select mf, af, email, recibo_mail, curp, es_draef, es_altoconsumidor, tiene_pozo, es_macromedidor,
                        [id_medidor] = 'indefinido', [lps_pagados] = '0', [_zona] = ''
                        From Padron.Cat_Padron Where id_cuenta = {0}", IdCuenta);
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = xQuery11;
                    using(var reader = xCommand.ExecuteReader()) {
                        if(reader.Read()) {
                            respuesta.CURP = reader["curp"].ToString();
                            respuesta.ProximaTomaLectura = "";
                            respuesta.Zona = reader["_zona"].ToString();
                            respuesta.LPSPagados = reader["lps_pagados"].ToString();
                            respuesta.AltoConsumidor = reader.GetBoolean("es_altoconsumidor");
                            respuesta.Correo = reader["email"].ToString();
                            respuesta.EnviarEstadoCuenta = reader.GetBoolean("recibo_mail");
                            respuesta.EsDraef = reader.GetBoolean("es_draef");
                            respuesta.TienePozo = reader.GetBoolean("tiene_pozo");
                            respuesta.EsMacromedidor = reader.GetBoolean("es_macromedidor");
                            respuesta.Mf = int.TryParse(reader.GetValue("mf").ToString(), out int tmpmf)?tmpmf:0;
                            respuesta.Af = int.TryParse(reader.GetValue("af").ToString(), out int tmpAf)?tmpAf:0;
                            respuesta.Medidor = reader.GetValue("id_medidor").ToString();
                        }
                    }
                    xConnecton.Close();
                }


                /********* Si el id_padron es cero, abortar *********/
                if(respuesta.Id_Padron <= 0) {
                    throw new Exception("Sin datos, id_padron es igual a 0.");
                }

                /********* Saldo a Favor *********/
                logger.LogInformation("(-) Obteniendo saldo a fovor cuenta:{Cuenta} padron:{Padron}", IdCuenta, respuesta.Id_Padron );
                using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    string xQuery2 = String.Format("Exec [Global].[usp_RegistroActual] @cAlias='OPR_SALDOAFAVOR', @cIdReferencia='{0}', @cOrder= '', @cWhere =''", respuesta.Id_Padron);
                    xConnecton.Open();
                    var xCommand = new SqlCommand(xQuery2, xConnecton);
                    using(var reader = xCommand.ExecuteReader()) {
                        if(reader.Read()) {
                            respuesta.SaldoFavor_importe = double.TryParse(reader.GetValue("importe").ToString(), out double tmpSf)?tmpSf:0;
                            respuesta.SaldoFavor_meses_x_aplicar = double.TryParse(reader.GetValue("meses_xaplicar").ToString(), out double tmpMA)? tmpMA:0;
                            respuesta.SaldoFavor_m3_x_aplicar = double.TryParse(reader.GetValue("m3_xaplicar").ToString(), out double m3a)?m3a:0;
                        }
                    }
                    xConnecton.Close();
                }


                /********* Documentos x Cobrar *********/
                logger.LogInformation("(-) Obteniendo documentos por cobrar cuenta:{Cuenta} padron:{Padron}", IdCuenta, respuesta.Id_Padron );
                using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    xConnecton.Open();
                    string xQuery3 = String.Format("Exec [Padron].[usp_ObtnAdeudo_Docs] @cAlias = 'ADEUDO_RESUMEN', @nId_Padron = {0}, @cAdeudoXML = ''", respuesta.Id_Padron);
                    var xCommand = new SqlCommand(xQuery3, xConnecton);
                    using(var reader = xCommand.ExecuteReader()) {
                        if(reader.Read()) {
                            respuesta.Documentos_imp_atiempo = double.TryParse(reader.GetValue("imp_atiempo").ToString(), out double tmpImpa)? tmpImpa:0;
                            respuesta.Documentos_imp_vencido = double.TryParse(reader.GetValue("imp_vencido").ToString(), out double tmpV)?tmpV:0;
                            respuesta.Documentos_imp_total = double.TryParse(reader.GetValue("imp_total").ToString(), out double impT)?impT:0;
                        }
                    }
                    xConnecton.Close();
                }


                /********* Saldo Actual *********/
                logger.LogInformation("(-) Obteniendo saldo actual cuenta:{Cuenta} padron:{Padron}", IdCuenta, respuesta.Id_Padron);
                using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    xConnecton.Open();
                    string xQuery4 = string.Format("Exec [Facturacion].[usp_ObtnAdeudo] @cAlias = 'ADE_FACTURAS+OTROS', @nId_Padron = {0}, @cAdeudoXML = ''", respuesta.Id_Padron);
                    var xCommand = new SqlCommand(xQuery4, xConnecton);
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = xQuery4;
                    var tmpListItems = new List<ConsultaGralResponse_saldoItem>();
                    using(var reader = xCommand.ExecuteReader()) {
                        while(reader.Read()) {
                            tmpListItems.Add(new ConsultaGralResponse_saldoItem {
                                Concepto = reader.GetValue("concepto").ToString(),
                                Subtotal = Double.Parse(reader.GetValue("s_subtotal").ToString()),
                                Iva = Double.Parse(reader.GetValue("s_iva").ToString()),
                                Total = Double.Parse(reader.GetValue("s_total").ToString()),
                            });
                        }
                    }
                    respuesta.SaldoActual = tmpListItems.ToArray<ConsultaGralResponse_saldoItem>();
                    xConnecton.Close();
                }


                /********* Calcular Totales Saldo Actual *********/
                double tmpSubTotal = respuesta.SaldoActual.Sum(item => item.Subtotal);
                double tmpIva = respuesta.SaldoActual.Sum(item => item.Iva);
                double tmpTotal = respuesta.SaldoActual.Sum(item => item.Total);

                respuesta.SaldoAct_subtotal = Math.Round(tmpSubTotal, 2);
                respuesta.SaldoAct_iva = Math.Round(tmpIva, 2);
                respuesta.SaldoAct_total = Math.Round(tmpTotal, 2);

                /********* Obtener total Imagenes *********/
                try {
                    logger.LogInformation("(-) Obteniendo imagenes cuenta:{Cuenta} padron:{Padron}", IdCuenta, respuesta.Id_Padron);
                    using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                        xConnecton.Open();
                        string xQuery5 = $"Select Count(id_imagen) as t From [{xEnlace.BaseDatos}Media].[Global].[Opr_Imagenes]  Where id_padron = {respuesta.Id_Padron} or id_tabla = '{respuesta.Id_Padron}' ";
                        var xCommand = new SqlCommand(xQuery5, xConnecton);
                        var tmpListItems = new List<ConsultaGralResponse_saldoItem>();
                        using(var reader = xCommand.ExecuteReader()) {
                            if(reader.Read()) {
                                respuesta.TotalImagenes = int.Parse(reader.GetValue("t").ToString());
                            }
                        }
                        xConnecton.Close();
                    }
                }
                catch(Exception){
                    logger.LogWarning("(-) No hay imagenes para la cuenta:{Cuenta}", IdCuenta);
                    respuesta.TotalImagenes = 0;
                }


                /********* Obtener coordenadas *********/
                try {
                    logger.LogInformation("(-) Obteniendo coordenadas cuenta:{Cuenta} padron:{Padron}", IdCuenta, respuesta.Id_Padron);
                    using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                        xConnecton.Open();
                        string xQuery6 = string.Format("Select latitud, longitud From [Padron].[Cat_Padron] Where id_padron = {0}", respuesta.Id_Padron);
                        var xCommand = new SqlCommand(xQuery6, xConnecton);
                        var tmpListItems = new List<ConsultaGralResponse_saldoItem>();
                        using(var reader = xCommand.ExecuteReader()) {
                            if(reader.Read()) {
                                respuesta.Latitud = reader.GetValue("latitud").ToString();
                                respuesta.Longitud = reader.GetValue("longitud").ToString();
                            }
                        }
                        xConnecton.Close();
                    }
                }catch(Exception err){
                    respuesta.Latitud = "0";
                    respuesta.Longitud = "0";
                    logger.LogError(0, err, "(-) NError al obtener las coordenadas de la consulta general cuenta:{Cuenta}", IdCuenta );
                }


                /********* Obtener Historial Consumos *********/
                logger.LogInformation("(-) Obteniendo historial consumos cuenta:{Cuenta} padron:{Padron}", IdCuenta, respuesta.Id_Padron);
                using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    xConnecton.Open();
                    string xQuery7 = string.Format("Select top 24 id, id_padron, id_cuenta, mf, af, fecha, consumo_act, lectura_act, lectura_ant From [Facturacion].[Opr_Facturas]  Where  id_padron = {0} and id_tipomovto <> 11 Order BY fecha Desc", respuesta.Id_Padron);
                    var xCommand = new SqlCommand(xQuery7, xConnecton);
                    var tmpListItems = new List<ConsumoItem>();
                    using(var reader = xCommand.ExecuteReader()) {
                        while(reader.Read()) {
                            tmpListItems.Add(new ConsumoItem {
                                Id = int.Parse(reader.GetValue("id").ToString()),
                                Id_padron = int.Parse(reader.GetValue("id_padron").ToString()),
                                Mf = int.Parse(reader.GetValue("mf").ToString()),
                                Af = int.Parse(reader.GetValue("af").ToString()),
                                Consumo_Act = double.Parse(reader.GetValue("consumo_act").ToString()),
                                Lectura_ant = double.Parse(reader.GetValue("lectura_act").ToString()),
                                Lectura_act = double.Parse(reader.GetValue("lectura_ant").ToString()),
                                Fecha = DateTime.Parse(reader.GetValue("fecha").ToString())
                            });
                        }
                    }
                    xConnecton.Close();
                    respuesta.HistorialConsumos = tmpListItems.ToArray<ConsumoItem>();
                }
                return respuesta;

            }catch(Exception err) {
                logger.LogError(0, err, "(-) ConsultaGeneral; Error al obtener datos de la oficina id {Oficina}", Id_Oficina);
                return null;
            }
        }
        public Cuenta_Arquos[] BusquedaCuentas(int Id_Oficina, string nombre, string medidor, string direccion, string colonia, int? soloActivos) {
            var respuesta = new List<Cuenta_Arquos>();

            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            if(xEnlace != null) {
                try {

                    /********* Generar Filto Xml  *********/
                    var xmlString = new System.Text.StringBuilder();
                    xmlString.Append("<filtro>");
                    if(nombre != null) { xmlString.Append(string.Format("<nombre>{0}</nombre>", nombre.ToString())); }
                    if(direccion != null) { xmlString.Append(string.Format("<direccion>{0}</direccion>", direccion.ToString())); }
                    if(colonia != null) { xmlString.Append(string.Format("<colonia>{0}</colonia>", colonia.ToString())); }
                    if(medidor != null) { xmlString.Append(string.Format("<medidor>{0}</medidor>", medidor.ToString())); }
                    if(soloActivos != null) { xmlString.Append(string.Format("<soloActivos>{0}</soloActivos>", (soloActivos == 1) ? "1" : "0")); }
                    xmlString.Append("</filtro>");
                                        
                    /********* Ejecutar Consulta *********/
                    string xQuery1 = String.Format("EXEC [SICEM].[BusquedaAvanzada] @cAlias = 'AVANZADA', @xml = '{0}'", xmlString.ToString());
                    using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                        xConnecton.Open();
                        var xCommand = new SqlCommand();
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = xQuery1;
                        using(var reader = xCommand.ExecuteReader()) {
                            while(reader.Read()) {
                                respuesta.Add(new Cuenta_Arquos {
                                    Id_Oficina = xEnlace.Id,
                                    Oficina = xEnlace.Oficina,
                                    Id_padron = long.Parse(reader.GetValue("id_padron").ToString()),
                                    Id_cuenta = long.Parse(reader.GetValue("id_cuenta").ToString()),
                                    Razon_social = reader.GetValue("razon_social").ToString(),
                                    Nombre_comercial = reader.GetValue("nom_comercial").ToString(),
                                    Direccion = reader.GetValue("direccion").ToString(),
                                    Servicio = reader.GetValue("__RO__SERVICIO").ToString(),
                                    Colonia = reader.GetValue("__RO__COLONIA").ToString(),
                                    Poblacion = reader.GetValue("__RO__POBLACION").ToString(),
                                    Medidor = reader.GetValue("medidor").ToString(),
                                    Estatus = reader.GetValue("estatus").ToString()
                                });
                            }
                        }
                    }
                
                }
                catch(Exception err){
                    Console.WriteLine($">> Error al buscar cuentas en oficina:{Id_Oficina} {xEnlace.Oficina} \n{err.Message}");
                }
            }

            return respuesta.ToArray();
        }

        public List<ConsultaGreal_MovimientosResponse> ConsultaGral_Movimientos(int IdOficina, int IdPadron) {
            try{
                logger.LogInformation("(-) Consultando movimientos del padron:{Padron} de la oficina:{Oficina}", IdPadron, IdOficina);
                var respuesta = new List<ConsultaGreal_MovimientosResponse>();
                var xEnlace = sicemService.ObtenerEnlaces(IdOficina).FirstOrDefault();
                using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    xConnecton.Open();
                    string xQuery1 = String.Format("Exec Global.usp_ConsultaGeneral @cAlias='OPR_MOVIMIENTOS', @cIdReferencia='{0}', @xmlTables= '<FILTROS/>'", IdPadron);
                    var xCommand = new SqlCommand(xQuery1, xConnecton);
                    using(var reader = xCommand.ExecuteReader()) {
                        while(reader.Read()) {
                            respuesta.Add( ConsultaGreal_MovimientosResponse.FromDataReaderV2(reader));
                        }
                    }
                }
                return respuesta;
            }catch(Exception err){
                logger.LogError(0, err, "(-) Error al consulta movimientos del padron:{Padron} de la oficina:{Oficina}", IdPadron, IdOficina );
                return null;
            }
            
        }
        public List<ConsultaGral_ModificacionesABCResponse> ConsultaGral_ModificacionesABC(int IdOficina, string IdPadron) {
            try{
                logger.LogInformation("(-) Consultando Modificaciones ABC del padron:{Padron} de la oficina:{Oficina}", IdPadron, IdOficina);
                var respuesta = new List<ConsultaGral_ModificacionesABCResponse>();
                var enlace = sicemService.ObtenerEnlaces(IdOficina).FirstOrDefault();
                
                using(var xConnection = new SqlConnection(enlace.StringConection)) {
                    xConnection.Open();
                    string xQuery = String.Format("Exec [Global].[usp_ConsultaGral] @cAlias='BITACORA_ABC', @nId_Padron = {0}, @nId_Cuenta=0, @cFolio=''", IdPadron);
                    var xCommand = new SqlCommand(xQuery, xConnection);
                    var xDataSet = new DataSet();
                    new SqlDataAdapter(xCommand).Fill(xDataSet);

                    foreach(DataRow xRow in xDataSet.Tables[0].Rows) {
                        var tmpItem = new ConsultaGral_ModificacionesABCResponse();
                        tmpItem.Id_abc = int.Parse(xRow.ItemArray[0].ToString());
                        tmpItem.Fecha = DateTime.Parse(xRow.ItemArray[1].ToString());
                        tmpItem.Id_padron = int.Parse(xRow.ItemArray[2].ToString());
                        tmpItem.Id_operador = xRow.ItemArray[3].ToString();
                        tmpItem.Id_sucursal = int.Parse(xRow.ItemArray[4].ToString());
                        tmpItem.Observacion = xRow.ItemArray[5].ToString();
                        tmpItem.Maquina = xRow.ItemArray[6].ToString();
                        tmpItem.Operador = xRow.ItemArray[7].ToString();
                        tmpItem.Sucursal = xRow.ItemArray[8].ToString();

                        tmpItem.Detalle = new List<ConsultaGral_ModificacionesABCResponse_Item>();
                        foreach(DataRow yRow in xDataSet.Tables[1].Rows) {
                            int tmpId = int.Parse(yRow.ItemArray[0].ToString());
                            if(tmpId == tmpItem.Id_abc) {
                                var tmpItem2 = new ConsultaGral_ModificacionesABCResponse_Item();
                                tmpItem2.Id_abc = int.Parse(yRow.ItemArray[0].ToString());
                                tmpItem2.Campo = yRow.ItemArray[1].ToString();
                                tmpItem2.Valor_Ant = yRow.ItemArray[2].ToString();
                                tmpItem2.Valor_Act = yRow.ItemArray[3].ToString();
                                tmpItem.Detalle.Add(tmpItem2);
                            }
                        }
                        respuesta.Add(tmpItem);
                    }
                }

                return respuesta;
            }catch(Exception err){
                logger.LogError(0, err, "(-) Error al consulta movimientos del padron:{Padron} de la oficina:{Oficina}", IdPadron, IdOficina );
                return null;
            }
        }
        public List<ConsultaGral_Ordenesresponse> ConsultaGral_Ordenes(int IdOficina, string IdPadron) {
            try{ 
                logger.LogInformation("(-) Obteniendo ordenes de trabajo del padron:{Padron} de la oficina:{Oficina}", IdPadron, IdOficina);

                var respuesta = new List<ConsultaGral_Ordenesresponse>();
                var _enlace = sicemService.ObtenerEnlaces(IdOficina).FirstOrDefault();
                string query = String.Format("Exec [Ordenes].[usp_OprOrdenes] 'OTS_USUARIO', {0}, '<FILTROS/>' ", IdPadron);
                using(var _connection = new SqlConnection(_enlace.StringConection)) {
                        _connection.Open();
                        var _command = new SqlCommand(query, _connection);
                        using(SqlDataReader sqlDataReader = _command.ExecuteReader()) {
                            while(sqlDataReader.Read()) {
                                var tmpItem = new ConsultaGral_Ordenesresponse();

                                tmpItem.Id_Orden = sqlDataReader.GetValue("id_orden").ToString();
                                try {
                                    tmpItem.Fecha_Genero = DateTime.Parse(sqlDataReader.GetValue("fecha_genero").ToString());
                                }
                                catch(Exception) {
                                    tmpItem.Fecha_Genero = null;
                                }
                                tmpItem.Ejecutada = (int.Parse(sqlDataReader.GetValue("id_estatus").ToString()) == 9) ? true : false;
                                tmpItem.Trabajo = sqlDataReader.GetValue("_trabajo").ToString();
                                tmpItem.Tipo_Orden = int.Parse(sqlDataReader.GetValue("id_tipoorden").ToString());
                                tmpItem.Tipo_OrdenDesc = sqlDataReader.GetValue("_tipoorden").ToString();


                                //Datos de la Orden
                                tmpItem.Estatus = sqlDataReader.GetValue("_estatus").ToString();
                                tmpItem.Trabajo = sqlDataReader.GetValue("_trabajo").ToString();
                                tmpItem.Genero = sqlDataReader.GetValue("_genero").ToString();
                                tmpItem.Imprimio = sqlDataReader.GetValue("_imprimio").ToString();
                                try {
                                    tmpItem.Fecha_Imprimio = DateTime.Parse(sqlDataReader.GetValue("_fecha_imprimio").ToString());
                                }
                                catch(Exception) {
                                    tmpItem.Fecha_Imprimio = null;
                                }
                                tmpItem.Observaciones_Orden = sqlDataReader.GetValue("observa_a").ToString();


                                //Resultados de la Orden
                                try {
                                    tmpItem.Fecha_Ini = DateTime.Parse(sqlDataReader.GetValue("fecha_ini").ToString());
                                }
                                catch(Exception) {
                                    tmpItem.Fecha_Ini = null;
                                }
                                try {
                                    tmpItem.Fecha_Fin = DateTime.Parse(sqlDataReader.GetValue("fecha_fin").ToString());
                                }
                                catch(Exception) {
                                    tmpItem.Fecha_Fin = null;
                                }
                                tmpItem.Realizo = sqlDataReader.GetValue("_realizo").ToString();
                                tmpItem.Capturo = sqlDataReader.GetValue("_capturo").ToString();
                                try {
                                    tmpItem.Fecha_Capturo = DateTime.Parse(sqlDataReader.GetValue("_fecha_capturo").ToString());
                                }
                                catch(Exception) {
                                    tmpItem.Fecha_Capturo = null;
                                }

                                tmpItem.Duracion = int.Parse(sqlDataReader.GetValue("duracion").ToString());
                                tmpItem.Respuesta_Orden = sqlDataReader.GetValue("observa_b").ToString();


                                // Datos Medidor 
                                tmpItem.Medidor_Reg = sqlDataReader.GetValue("medidor_reg").ToString();
                                tmpItem.Marca_Reg = sqlDataReader.GetValue("_marca_reg").ToString();
                                tmpItem.Modelo_Reg = sqlDataReader.GetValue("_modelo_reg").ToString();
                                tmpItem.Diametro_Reg = sqlDataReader.GetValue("_diametro_reg").ToString();

                                tmpItem.Medidor_Enc = sqlDataReader.GetValue("medidor_enc").ToString();
                                tmpItem.Marca_Enc = sqlDataReader.GetValue("_marca_enc").ToString();
                                tmpItem.Modelo_Enc = sqlDataReader.GetValue("_modelo_enc").ToString();
                                tmpItem.Diametro_Enc = sqlDataReader.GetValue("_diametro_enc").ToString();

                                tmpItem.Medidor_Ret = sqlDataReader.GetValue("medidor_ret").ToString();
                                tmpItem.Marca_Ret = sqlDataReader.GetValue("_marca_ret").ToString();
                                tmpItem.Modelo_Ret = sqlDataReader.GetValue("_modelo_ret").ToString();
                                tmpItem.Diametro_Ret = sqlDataReader.GetValue("_diametro_ret").ToString();

                                tmpItem.Medidor_Inst = sqlDataReader.GetValue("medidor_ins").ToString();
                                tmpItem.Marca_Inst = sqlDataReader.GetValue("_marca_ins").ToString();
                                tmpItem.Modelo_Inst = sqlDataReader.GetValue("_modelo_ins").ToString();
                                tmpItem.Diametro_Inst = sqlDataReader.GetValue("_diametro_ins").ToString();

                                tmpItem.Anomalia_Reg = sqlDataReader.GetValue("_anomalia_reg").ToString();
                                tmpItem.Lectura_Reg = int.Parse(sqlDataReader.GetValue("lectura_reg").ToString());

                                tmpItem.Anomalia_Act = sqlDataReader.GetValue("_anomalia_enc").ToString();
                                tmpItem.Lectura_Act = int.Parse(sqlDataReader.GetValue("lectura_enc").ToString());
                                tmpItem.ErrorLectura = false;


                                // Datos Inspeccion
                                tmpItem.Lecutar_Ini = double.Parse(sqlDataReader.GetValue("lectura_ini").ToString());
                                tmpItem.Lecutar_Fin = double.Parse(sqlDataReader.GetValue("lectura_fin").ToString());
                                tmpItem.Consumo_Estimado = int.Parse(sqlDataReader.GetValue("consumo_estimado").ToString());
                                tmpItem.PruebaMed_A = double.Parse(sqlDataReader.GetValue("prueba_med1").ToString());
                                tmpItem.PruebaMed_B = double.Parse(sqlDataReader.GetValue("prueba_med2").ToString());
                                tmpItem.Sanit_FV = int.Parse(sqlDataReader.GetValue("fuga_vertedero").ToString());
                                tmpItem.Sanit_FS = int.Parse(sqlDataReader.GetValue("fuga_sapito").ToString());
                                tmpItem.Sanit_HNA = int.Parse(sqlDataReader.GetValue("huella_nivelalto").ToString());
                                tmpItem.Sanit_Nor = int.Parse(sqlDataReader.GetValue("normales").ToString());
                                tmpItem.Llav_Coc = int.Parse(sqlDataReader.GetValue("cocina").ToString());
                                tmpItem.Llav_Arc = int.Parse(sqlDataReader.GetValue("arco").ToString());
                                tmpItem.Llav_Ban = int.Parse(sqlDataReader.GetValue("banos").ToString());
                                tmpItem.Llav_Tom = int.Parse(sqlDataReader.GetValue("tomas").ToString());
                                tmpItem.Llav_Tinacos = int.Parse(sqlDataReader.GetValue("tinacos").ToString());
                                tmpItem.Llav_Jard = int.Parse(sqlDataReader.GetValue("jardin").ToString());
                                tmpItem.Llav_Lavado = int.Parse(sqlDataReader.GetValue("lavado").ToString());
                                tmpItem.Llav_Pilas = int.Parse(sqlDataReader.GetValue("pilas").ToString());
                                tmpItem.Llav_Cisterna = int.Parse(sqlDataReader.GetValue("cisternas").ToString());
                                tmpItem.Llav_Otros = int.Parse(sqlDataReader.GetValue("otros").ToString());

                                tmpItem.Person_Adultos = int.Parse(sqlDataReader.GetValue("adultos").ToString());
                                tmpItem.Person_Menore = int.Parse(sqlDataReader.GetValue("menores").ToString());
                                tmpItem.Promedio = int.Parse(sqlDataReader.GetValue("promedio_sugerido").ToString());
                                tmpItem.Tarifa = sqlDataReader.GetValue("_tarifa").ToString();

                                respuesta.Add(tmpItem);

                            }
                        }
                    }
                return respuesta;
            }catch(Exception err){
                logger.LogError(0, err, "(-) Error al obtener ordenes de trabajo del padron:{Padron} de la oficina:{Oficina}", IdPadron, IdOficina );
                return null;
            }
        }
        public ConsultaGral_Aniticipos_v2 ConsultaGral_Anticipos(int Id_Oficina, string IdPadron) {
            var respuesta = new ConsultaGral_Aniticipos_v2();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            string xQuery1 = String.Format("Exec [Global].[usp_ConsultaGral] @cAlias='OPR_ANTICIPOS', @nId_Padron={0}, @nId_Cuenta=0, @cFolio=''", IdPadron);
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                var xCommand = new SqlCommand();
                xCommand.Connection = xConnecton;
                xCommand.CommandText = xQuery1;

                var xDataSet = new DataSet();
                new SqlDataAdapter(xCommand).Fill(xDataSet);

                if(xDataSet.Tables.Count < 3) {
                    throw new Exception("Error al hacer la consulta a la base de datos.");
                }

                //******* Anticipos *******
                respuesta.Anticipos = new List<AnticipoItem>();
                foreach(DataRow xRow in xDataSet.Tables[0].Rows) {                    

                    var tmpItem = new AnticipoItem();
                    tmpItem.Id_Abono = xRow["id_abono"].ToString();
                    tmpItem.Fecha = DateTime.Parse(xRow["fecha"].ToString());
                    tmpItem.Id_Estatus = int.Parse(xRow["id_estatus"].ToString());
                    tmpItem.Estatus = xRow["ESTATUS"].ToString();
                    tmpItem.Sucursal = xRow["SUCURSAL"].ToString();
                    tmpItem.Autorizo = xRow["AUTORIZO"].ToString();
                    tmpItem.Subtotal = decimal.Parse(xRow["subtotal"].ToString());
                    tmpItem.Iva = decimal.Parse(xRow["iva"].ToString());
                    tmpItem.Total = decimal.Parse(xRow["total"].ToString());
                    tmpItem.Mf = int.Parse(xRow["mf"].ToString());
                    tmpItem.Af = int.Parse(xRow["af"].ToString());

                    tmpItem.Ciclo = xRow["CICLO"].ToString();
                    tmpItem.Promedio = int.Parse(xRow["promedio"].ToString());
                    tmpItem.Observa_a = xRow["observa_a"].ToString();
                    tmpItem.Observa_c = xRow["observa_c"].ToString();
                    tmpItem.Tarifa = xRow["TARIFA"].ToString();
                    tmpItem.Servicio = xRow["SERVICIO"].ToString();
                    tmpItem.Situacion = xRow["SITUACION"].ToString();
                    tmpItem.Anomalia = xRow["ANOMALIA"].ToString();
                    tmpItem.Calculo = xRow["CALCULO"].ToString();
                    tmpItem.Tarifa_Fija = xRow["TARIFAFIJA"].ToString();
                    tmpItem.Movimiento = xRow["MOVIMIENTO"].ToString();
                    tmpItem.Cancelo = xRow["CANCELO"].ToString();

                    tmpItem.Fecha_Cancelacion = DateTime.Parse(xRow["fecha_cancelo"].ToString());
                    tmpItem.Id_Caja = xRow["id_caja"].ToString();
                    tmpItem.Id_Operacion = xRow["id_operador"].ToString();
                    tmpItem.Cobrado = decimal.Parse(xRow["cobrado"].ToString());
                    tmpItem.Tipo_Pago = xRow["TIPOPAGO"].ToString();
                    tmpItem.Cajero = xRow["CAJERO"].ToString();
                    tmpItem.Lectura_Actual = int.Parse(xRow["lectura_act"].ToString()); ;
                    tmpItem.Consumo_Actual = int.Parse(xRow["consumo_act"].ToString()); ;
                    tmpItem.Meses_Adeudo = int.Parse(xRow["meses_adeudo"].ToString()); ;
                    tmpItem.M3_Aplicados = int.Parse(xRow["m3_aplicados"].ToString()); ;
                    tmpItem.Recno = int.Parse(xRow["RECNO"].ToString()); ;
                    respuesta.Anticipos.Add(tmpItem);
                }

                //******* Conceptos *******
                respuesta.Conceptos = new List<Anticipo_Concepto>();
                foreach(DataRow xRow in xDataSet.Tables[1].Rows) {
                    var tmpItem = new Anticipo_Concepto();
                    tmpItem.Id_Abono = xRow["id_abono"].ToString();
                    tmpItem.Rezago = bool.Parse(xRow["es_rezago"].ToString());
                    tmpItem.Id_Concepto = int.Parse(xRow["id_concepto"].ToString());
                    tmpItem.Concepto = xRow["CONCEPTO"].ToString();
                    tmpItem.Sub_Total = double.Parse(xRow["subtotal"].ToString());
                    tmpItem.IVA = double.Parse(xRow["iva"].ToString());
                    tmpItem.Total = double.Parse(xRow["total"].ToString());
                    respuesta.Conceptos.Add(tmpItem);
                }
            }

            return respuesta;
        }
        public List<ConsultaGral_Documentos> ConsultaGral_Imagenes(int Id_Oficina, string IdPadron) {
            try {
                var respuesta = new List<ConsultaGral_Documentos>();
                var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
                string xQuery1 = String.Format("Select id_imagen, id_tabla, id_padron, descripcion, archivo, fecha_insert, filesize, id_insert From [{1}Media].[Global].[Opr_Imagenes] " +
                    "Where	id_tabla = '{0}' or id_padron = '{0}'", IdPadron, xEnlace.BaseDatos);
                using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = xQuery1;
                    using(SqlDataReader xReader = xCommand.ExecuteReader()){
                        while(xReader.Read()) {
                            var tmpItem = new ConsultaGral_Documentos();
                            tmpItem.Id_Imagen = xReader.GetValue("id_imagen").ToString();
                            tmpItem.Id_Padron = xReader.GetValue("id_padron").ToString();
                            tmpItem.Descripcion = xReader.GetValue("descripcion").ToString();
                            tmpItem.Archivo = xReader.GetValue("archivo").ToString();
                            tmpItem.Extencion = tmpItem.Archivo.Split(".").Last<string>().ToLower();
                            tmpItem.Fecha_Insert = DateTime.TryParse(xReader.GetValue("fecha_insert").ToString(), out DateTime tmpD)?tmpD:new DateTime();
                            tmpItem.Tamano = long.TryParse(xReader["filesize"].ToString(), out long tmpL) ? tmpL : 0;
                            tmpItem.Personal = xReader["id_insert"].ToString();
                            respuesta.Add(tmpItem);
                        }
                    }
                }
                return respuesta;
            }catch(Exception err){
                Console.WriteLine($">> Error al procesar la consutla\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return null;
            }
        }

    }
}
