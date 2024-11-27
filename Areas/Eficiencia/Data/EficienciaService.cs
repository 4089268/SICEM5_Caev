using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Eficiencia.Models;
using Sicem_Blazor.Eficiencia.Data;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.Services {
    public class EficienciaService : IEficienciaService {

        private readonly ILogger<EficienciaService> logger;

        public EficienciaService(ILogger<EficienciaService> l){
            this.logger = l;
        }

        public EficienciaResumen ObtenerResumenEnlace(IEnlace enlace, int anio, int mes, int sb, int sec){
            var result = new EficienciaResumen(){
                Enlace = enlace,
                Estatus = ResumenOficinaEstatus.Completado
            };

            try{
                using( var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query = string.Format("Exec sicem.eficiencia @cAlias = 'MENSUAL', @nSub = {0}, @nSec = {1}, @xAf = {2}, @xMf = {3}", sb, sec, anio, mes);

                    var command = new SqlCommand(_query, sqlConnection);
                    using(SqlDataReader reader = command.ExecuteReader()){
                        if(reader.Read()){
                            result.Mes = reader["mes"].ToString();
                            result.Facturado = ConvertUtils.ParseDecimal( reader["facturado"].ToString());
                            result.Refacturado = ConvertUtils.ParseDecimal( reader["refacturado"].ToString());
                            result.Anticipado = ConvertUtils.ParseDecimal( reader["anticipado"].ToString());
                            result.Descontado = ConvertUtils.ParseDecimal( reader["descontado"].ToString());
                            result.Cobrado = ConvertUtils.ParseDecimal( reader["cobrado"].ToString());
                            result.Porcentaje = ConvertUtils.ParseDouble( reader["porc"].ToString());
                        }
                    }				
                    sqlConnection.Close();
                }
            }catch(Exception err){
                logger.LogError(err, $"Error al obtener la eficiencia comercial del enlace {enlace.Nombre}");
                result.Estatus = ResumenOficinaEstatus.Error;
            }
            return result;
        }

        public IEnumerable<EficienciaResumen> ObtenerResumenAnual(IEnlace enlace, int anio, int sb, int sec){
            var result = new List<EficienciaResumen>();
            try{
                using( var sqlConnection = new SqlConnection(enlace.GetConnectionString())){
                    sqlConnection.Open();
                    var _query = string.Format("Exec sicem.eficiencia @cAlias = 'MENSUAL', @nSub = {0}, @nSec = {1}, @xAf = {2}, @xMf = 0", sb, sec, anio);

                    var command = new SqlCommand(_query, sqlConnection);
                    using(SqlDataReader reader = command.ExecuteReader()){
                        while(reader.Read()){
                            var item = new EficienciaResumen(){
                                Enlace = enlace
                            };
                            item.Mes = reader["mes"].ToString();
                            item.Facturado = ConvertUtils.ParseDecimal( reader["facturado"].ToString());
                            item.Refacturado = ConvertUtils.ParseDecimal( reader["refacturado"].ToString());
                            item.Anticipado = ConvertUtils.ParseDecimal( reader["anticipado"].ToString());
                            item.Descontado = ConvertUtils.ParseDecimal( reader["descontado"].ToString());
                            item.Cobrado = ConvertUtils.ParseDecimal( reader["cobrado"].ToString());
                            item.Porcentaje = ConvertUtils.ParseDouble( reader["porc"].ToString());
                            result.Add(item);
                        }
                    }				
                    sqlConnection.Close();
                }
                return result;

            }catch(Exception err){
                logger.LogError(err, $"Error al obtener la eficiencia comercial anual del enlace {enlace.Nombre}");
                return new List<EficienciaResumen>();
            }
        }

        public Eficiencia_Colonia[] ObtenerEficienciaColonias(int Id_Oficina, int Ano, int Mes, int Sb, int Sect, int Tipo)
        {
            throw new NotImplementedException();
        }

        public Eficiencia_Conceptos[] ObtenerEficienciaConceptos(int Id_Oficina, int Ano, int Mes, int Sb, int Sect)
        {
            throw new NotImplementedException();
        }

        public Eficiencia_Detalle[] ObtenerEficienciaDetalleColonia(int Id_Oficina, int Ano, int Mes, int Sb, int Sect, int Tipo, int IdColonia, int IdLocalidad)
        {
            throw new NotImplementedException();
        }

        public Eficiencia_Detalle[] ObtenerEficienciaDetalleSector(int Id_Oficina, int Ano, int Mes, int Sb, int Sect, int Tipo)
        {
            throw new NotImplementedException();
        }

        public dynamic[] ObtenerEficienciaPorOficinas(Ruta[] oficinas, DateTime fecha1, DateTime fecha2, bool agregarTotal = true)
        {
            throw new NotImplementedException();
        }

        public Eficiencia_Sectores_Resp ObtenerEficienciaSectores(int Id_Oficina, int Ano, int Mes, int Sb)
        {
            throw new NotImplementedException();
        }

        
    }
}