using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Padron.Models;

namespace Sicem_Blazor.Services {
    public class PadronService{
        private readonly IConfiguration appSettings;
        private readonly SicemService sicemService;
        public PadronService(IConfiguration c, SicemService s) {
            this.appSettings = c;
            this.sicemService = s;
        }

        public Padron_Resumen ObtenerPadronResumen(int Id_Oficina, int Ano, int Mes, int Sb, int Sect) {
            var respuesta = new Padron_Resumen();
            var xEnlace = sicemService.ObtenerEnlaces(Id_Oficina).FirstOrDefault();
            using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [SICEM].[Padron] @cAlias='SOLO_TOTAL', @nAf={Ano}, @nMf={Mes}, @nSub={Sb}, @nSec={Sect} ";
                    using(var xReader = xCommand.ExecuteReader()) {
                        if(xReader.Read()) {
                            respuesta.Activos_Usuarios = int.Parse(xReader["USU_1"].ToString());
                            respuesta.Activos_Adeudos = decimal.Parse(xReader["IMP_1"].ToString());
                            respuesta.Espera_Usuarios = int.Parse(xReader["USU_2"].ToString());
                            respuesta.Espera_Adeudos = decimal.Parse(xReader["IMP_2"].ToString());
                            respuesta.BajaTemp_Usuarios = int.Parse(xReader["USU_3"].ToString());
                            respuesta.BajaTemp_Adeudos = decimal.Parse(xReader["IMP_3"].ToString());
                            respuesta.BajaDef_Usuarios = int.Parse(xReader["USU_4"].ToString());
                            respuesta.BajaDef_Adeudos = decimal.Parse(xReader["IMP_4"].ToString());
                            respuesta.Conge_Usuarios = int.Parse(xReader["USU_5"].ToString());
                            respuesta.Conge_Adeudos = decimal.Parse(xReader["IMP_5"].ToString());
                            respuesta.Total_Usuarios = int.Parse(xReader["USU_TOT"].ToString());
                            respuesta.Total_Adeudos = decimal.Parse(xReader["IMP_TOT"].ToString());
                        }
                    }
                }
            }
            return respuesta;
        }
        public Padron_Resumen ObtenerPadronResumen(IEnlace enlace, int Ano, int Mes, int Sb, int Sect) {
            var respuesta = new Padron_Resumen();
            respuesta.Enlace = enlace;
            try {
                using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                    xConnecton.Open();
                    using(var xCommand = new SqlCommand()) {
                        xCommand.Connection = xConnecton;
                        xCommand.CommandText = $"EXEC [SICEM].[Padron] @cAlias='SOLO_TOTAL', @nAf={Ano}, @nMf={Mes}";
                        using(var xReader = xCommand.ExecuteReader()) {
                            if(xReader.Read()) {
                                respuesta.Activos_Usuarios = int.Parse(xReader["USU_1"].ToString());
                                respuesta.Activos_Adeudos = decimal.Parse(xReader["IMP_1"].ToString());
                                respuesta.Espera_Usuarios = int.Parse(xReader["USU_2"].ToString());
                                respuesta.Espera_Adeudos = decimal.Parse(xReader["IMP_2"].ToString());
                                respuesta.BajaTemp_Usuarios = int.Parse(xReader["USU_3"].ToString());
                                respuesta.BajaTemp_Adeudos = decimal.Parse(xReader["IMP_3"].ToString());
                                respuesta.BajaDef_Usuarios = int.Parse(xReader["USU_4"].ToString());
                                respuesta.BajaDef_Adeudos = decimal.Parse(xReader["IMP_4"].ToString());
                                respuesta.Conge_Usuarios = int.Parse(xReader["USU_5"].ToString());
                                respuesta.Conge_Adeudos = decimal.Parse(xReader["IMP_5"].ToString());
                                respuesta.Total_Usuarios = int.Parse(xReader["USU_TOT"].ToString());
                                respuesta.Total_Adeudos = decimal.Parse(xReader["IMP_TOT"].ToString());
                                try{
                                    respuesta.FechaModificacion = xReader["fecha_padron"].ToString();
                                    
                                }catch(Exception){}
                            }
                        }
                    }
                }
                respuesta.Estatus = 1;
            }
            catch(Exception err) {
                Console.WriteLine($"Error al procesar la consulta oficina:{enlace.Nombre}\n\tErr:{err.Message}\n\tStack:{err.StackTrace}");
                respuesta.Estatus = 2;
            }
            return respuesta;
        }
        public Padron_Resumen[] ObtenerPadronPorOficinas(IEnumerable<IEnlace> enlaces, int Ano, int Mes, int sb, int sect, bool agregarTotal = true){
            throw new NotImplementedException();
            // var response = new List<Padron_Resumen>();
            // foreach(var ofi in enlaces) {
            //     Padron_Resumen tmpItem = ObtenerPadronResumen(ofi.Id, Ano, Mes, sb, sect);
            //     tmpItem.Enlace = ofi;
            //     response.Add(tmpItem);
            // }
            // if(agregarTotal && response.Count > 1) {
            //     var tmpItemTotal = new Padron_Resumen();
            //     tmpItemTotal.Enlace = null;
            //     tmpItemTotal.Activos_Usuarios = response.Sum(item => item.Activos_Usuarios);
            //     tmpItemTotal.Activos_Adeudos = response.Sum(item => item.Activos_Adeudos);
            //     tmpItemTotal.Espera_Usuarios = response.Sum(item => item.Espera_Usuarios);
            //     tmpItemTotal.Espera_Adeudos = response.Sum(item => item.Espera_Adeudos);
            //     tmpItemTotal.BajaTemp_Usuarios = response.Sum(item => item.BajaTemp_Usuarios);
            //     tmpItemTotal.BajaTemp_Adeudos = response.Sum(item => item.BajaTemp_Adeudos);
            //     tmpItemTotal.BajaDef_Usuarios = response.Sum(item => item.BajaDef_Usuarios);
            //     tmpItemTotal.BajaDef_Adeudos = response.Sum(item => item.BajaDef_Adeudos);
            //     tmpItemTotal.Conge_Usuarios = response.Sum(item => item.Conge_Usuarios);
            //     tmpItemTotal.Conge_Adeudos = response.Sum(item => item.Conge_Adeudos);
            //     tmpItemTotal.Total_Usuarios = response.Sum(item => item.Total_Usuarios);
            //     tmpItemTotal.Total_Adeudos = response.Sum(item => item.Total_Adeudos);
                
            //     response.Add(tmpItemTotal);
            // }
            // return response.ToArray();
        }
        public IEnumerable<Padron_DetallePadron> ObtenerDetallePadron(IEnlace enlace, int Ano, int Mes, int Sb, int Sect, int Id_Estatus) {
            Padron_DetallePadron[] respuesta;
            using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [SICEM].[Padron] @cAlias='POR_ESTATUS', @nAf={Ano}, @nMf={Mes}, @nSub={Sb}, @nSec={Sect}, @nEsta = {Id_Estatus}";
                    var tmpList = new List<Padron_DetallePadron>();
                    using(var xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            tmpList.Add(new Padron_DetallePadron {
                                Estatus = xReader["estatus"].ToString(),
                                Cuenta = long.Parse(xReader["cuenta"].ToString()),
                                Localizacion = xReader["localizacion"].ToString(),
                                Nombre = xReader["nombre"].ToString(),
                                Direccion = xReader["direccion"].ToString(),
                                Tarifa = xReader["tarifa"].ToString(),
                                Colonia = xReader["colonia"].ToString(),
                                Lec_Ant = double.Parse(xReader["lec_ant"].ToString()),
                                Lec_Act = double.Parse(xReader["lec_act"].ToString()),
                                Consumo = double.Parse(xReader["consumo"].ToString()),
                                Calculo = xReader["calculo"].ToString(),
                                Promedio = double.Parse(xReader["promedio"].ToString()),
                                Medidor = xReader["medidor"].ToString(),
                                Agua = decimal.Parse(xReader["agua"].ToString()),
                                Dren = decimal.Parse(xReader["dren"].ToString()),
                                Sane = decimal.Parse(xReader["sane"].ToString()),
                                Act = decimal.Parse(xReader["actu"].ToString()),
                                Otros = decimal.Parse(xReader["otros"].ToString()),
                                Ragua = decimal.Parse(xReader["ragua"].ToString()),
                                Rdren = decimal.Parse(xReader["rdren"].ToString()),
                                Rtrat = decimal.Parse(xReader["rtrat"].ToString()),
                                Subtotal = decimal.Parse(xReader["total"].ToString()),
                                Iva = decimal.Parse(xReader["total"].ToString()),
                                Total = decimal.Parse(xReader["total"].ToString())
                            });
                        }
                    }
                    respuesta = tmpList.ToArray<Padron_DetallePadron>();
                }
            }            
            return respuesta;
        }
        public Padron_Contratos[] ObtenerContratosRealizados(IEnlace enlace, int Ano, int Mes) {
            Padron_Contratos[] respuesta;
            using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [SICEM].[Padron] @cAlias = 'CONTRATOS-REALIZADOS', @nAf={Ano}, @nMf={Mes}";
                    var tmpImportes = new List<Padron_Contratos>();
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            tmpImportes.Add(new Padron_Contratos {
                                Id_Contrato = xReader["id_contrato"].ToString(),
                                Fecha_Contratado = DateTime.Parse(xReader["fecha_contratado"].ToString()),
                                Cuenta = long.Parse(xReader["cuenta"].ToString()),
                                Localizacion = xReader["localizacion"].ToString(),
                                Usuario = xReader["usuario"].ToString(),
                                Tarifa_Contratada = xReader["tarifa_contratada"].ToString(),
                                Tarifa_Actual = xReader["tarifa_actual"].ToString(),
                                Subtotal = decimal.Parse(xReader["subtotal"].ToString()),
                                Iva = decimal.Parse(xReader["iva"].ToString()),
                                Total = decimal.Parse(xReader["total"].ToString()),
                                Id_Tarifa_Actual = int.TryParse(xReader["id_tarifa_contratada"].ToString(), out int tmpid1)?tmpid1:0,
                                Id_Tarifa_Contratada = int.TryParse(xReader["id_tarifa_actual"].ToString(), out int tmpid2)?tmpid2:0,
                            });
                        }
                    }
                    respuesta = tmpImportes.ToArray();
                }
            }
            return respuesta;
        }
        public IEnumerable<Padron_ModifTarifa> ObtenerModifiTarifas(IEnlace enlace, int Ano,  int Mes) {
            Padron_ModifTarifa[] respuesta;
            using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                xConnecton.Open();
                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [SICEM].[Padron] @cAlias = 'MODIFICACIONES-TARIFAS', @nAf={Ano}, @nMf={Mes}";
                    var tmpData = new List<Padron_ModifTarifa>();
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            tmpData.Add(new Padron_ModifTarifa {
                                Cuenta = long.Parse(xReader["cuenta"].ToString()),
                                Localizacion = xReader["localizacion"].ToString(),
                                Usuario = xReader["usuario"].ToString(),
                                Fecha = DateTime.Parse(xReader["fecha"].ToString()),
                                Valor_Ant = xReader["valor_ant"].ToString(),
                                Valor_Act = xReader["valor_act"].ToString(),
                                Realizo = xReader["realizo"].ToString()
                            });
                        }
                    }
                    respuesta = tmpData.ToArray();
                }
            }
            return respuesta;
        }
        public Padron_ModifABC_Response ObtenerModificacionesABC(IEnlace enlace, int Ano, int Mes) {
            var respuesta = new Padron_ModifABC_Response();            
            using(var xConnecton = new SqlConnection(enlace.GetConnectionString())) {
                xConnecton.Open();

                using(var xCommand = new SqlCommand()) {
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = $"EXEC [SICEM].[Padron] @cAlias='MODIFICACIONES-ABC', @nAf={Ano}, @nMf={Mes}";

                    var xDataSet = new DataSet();
                    var xDataAdapter = new SqlDataAdapter(xCommand);
                    xDataAdapter.Fill(xDataSet);

                    if(xDataSet.Tables.Count != 2) {
                        throw new Exception("Error al hacer la consulta a la base de datos.");
                    }

                    var tmpDatos = new List<Padron_ModifABC>();
                    foreach(DataRow xRow in xDataSet.Tables[0].Rows) {
                        var newItem = new Padron_ModifABC();
                        newItem.Id_Abc = long.TryParse(xRow["id_abc"].ToString(), out long tAbc)?tAbc:0;
                        newItem.Fecha = DateTime.Parse(xRow["fecha"].ToString());
                        newItem.Id_Padron = long.TryParse(xRow["id_padron"].ToString(), out long tPad)?tPad:0;
                        newItem.Id_Cuenta = long.TryParse(xRow["id_cuenta"].ToString(), out long tCu)?tCu:0;
                        newItem.Razon_Social = xRow["razon_social"].ToString();
                        newItem.Direccion = xRow["direccion"].ToString();
                        newItem.Observacion = xRow["observacion"].ToString();
                        newItem.Maquina = xRow["maquina"].ToString();
                        newItem.Operador = xRow["operador"].ToString();
                        newItem.Id_Operador = xRow["id_operador"].ToString();
                        newItem.Sucursal = xRow["sucursal"].ToString();
                        newItem.Id_Sucursal = int.TryParse(xRow["id_sucursal"].ToString(), out int tSuc)?tSuc:0;
                        newItem.Localizacion = xRow["localizacion"].ToString();
                        newItem.Colonia = xRow["colonia"].ToString();
                        newItem.Id_Colonia = int.TryParse(xRow["id_colonia"].ToString(), out int tidCol)?tidCol:0;
                        newItem.Departamento = xRow["departamento"].ToString();
                        newItem.Id_Departamento = int.TryParse(xRow["id_departamento"].ToString(), out int tDep)?tDep:0;

                        tmpDatos.Add(newItem);
                    }
                    respuesta.Modificaciones = tmpDatos.ToArray();

                    var tmpList_Campos = new List<Padron_ModifABC_Campo>();
                    foreach(DataRow xRow in xDataSet.Tables[1].Rows) {
                        tmpList_Campos.Add(new Padron_ModifABC_Campo {
                            Id_Abc = long.Parse(xRow["id_abc"].ToString()),
                            Campo = xRow["campo"].ToString(),
                            valor_ant = xRow["valor_ant"].ToString(),
                            Valor_act = xRow["valor_act"].ToString()
                        });
                    }
                    respuesta.CamposModificados = tmpList_Campos.ToArray();
                }
            }
            return respuesta;
        }

        public IEnumerable<PadronColonia> ObtenerPadronPorColonia( IEnlace enlace, int ano, int mes){
            var _result = new List<PadronColonia>();
            try{
                using( var conexion = new SqlConnection(enlace.GetConnectionString())){
                    conexion.Open();
                    var _query = $"Exec [Sicem].[Padron] @cAlias = 'POR-COLONIAS', @nAf = {ano}, @nMf = {mes}";
                    var _command = new SqlCommand(_query, conexion);
                    using(var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            var tmpInt = 0;
                            _result.Add( new PadronColonia(){
                                IdColonia = int.TryParse(reader["id_colonia"].ToString(), out tmpInt)?tmpInt:0,
                                Colonia = reader["colonia"].ToString(),
                                Activo = int.TryParse(reader["activo"].ToString(), out tmpInt)?tmpInt:0,
                                Espera = int.TryParse(reader["espera"].ToString(), out tmpInt)?tmpInt:0,
                                BajaTemporal = int.TryParse(reader["baja_temporal"].ToString(), out tmpInt)?tmpInt:0,
                                BajaDefinitiva = int.TryParse(reader["baja_definitiva"].ToString(), out tmpInt)?tmpInt:0,
                                Congelado = int.TryParse(reader["congelado"].ToString(), out tmpInt)?tmpInt:0,
                                TotalUsuarios = int.TryParse(reader["total_usuarios"].ToString(), out tmpInt)?tmpInt:0,
                            });
                        }
                    }
                    conexion.Close();
                }
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el padron por colonias oficina:{enlace.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return new PadronColonia[]{};
            }
            return _result;
        }
        public IEnumerable<PadronSector> ObtenerPadronPorSectores( IEnlace enlace, int ano, int mes){
            var _result = new List<PadronSector>();
            try {
                using( var conexion = new SqlConnection(enlace.GetConnectionString())){
                    conexion.Open();
                    var _query = $"Exec [Sicem].[Padron] @cAlias = 'POR-SECTORES', @nAf = {ano}, @nMf = {mes}";
                    var _command = new SqlCommand(_query, conexion);
                    using(var reader = _command.ExecuteReader()){
                        while(reader.Read()){
                            var tmpInt = 0;
                            _result.Add( new PadronSector(){
                                Sector = int.TryParse(reader["sector"].ToString(), out tmpInt)?tmpInt:0,
                                Activo = int.TryParse(reader["activo"].ToString(), out tmpInt)?tmpInt:0,
                                Espera = int.TryParse(reader["espera"].ToString(), out tmpInt)?tmpInt:0,
                                BajaTemporal = int.TryParse(reader["baja_temporal"].ToString(), out tmpInt)?tmpInt:0,
                                BajaDefinitiva = int.TryParse(reader["baja_definitiva"].ToString(), out tmpInt)?tmpInt:0,
                                Congelado = int.TryParse(reader["congelado"].ToString(), out tmpInt)?tmpInt:0,
                                TotalUsuarios = int.TryParse(reader["total_usuarios"].ToString(), out tmpInt)?tmpInt:0,
                            });
                        }
                    }
                    conexion.Close();
                }
            }catch(Exception err){
                Console.WriteLine($">> Error al obtener el padron por sectores oficina:{enlace.Nombre}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return new PadronSector[]{};
            }
            return _result;
        }
    }
}
