using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Data;
using Sicem_Blazor.Models;
using Sicem_Blazor.Models.Entities.Arquos;
using Sicem_Blazor.AnalisisInformacion.Models;

namespace Sicem_Blazor.Services
{
    public class AnalisisInformacionService
    {

        public readonly IConfiguration appSettings;
        private readonly SicemContext sicemContext;
        private readonly ILogger<AnalisisInformacionService> logger;

        public AnalisisInformacionService(SicemContext context, IConfiguration c, ILogger<AnalisisInformacionService> logger)
        {
            this.sicemContext = context;
            this.appSettings = c;
            this.logger = logger;
        }


        public async Task<List<CatPadron>> ObtenerAnalisisInfo(ICollection<Ruta> oficinas, AnalisysInfoFilter filtro)
        {
            logger.LogInformation("Begining analisis info search to offices:{offices} and filters [{filters}]", String.Join(",", oficinas), filtro);
            var tareas = oficinas.Select( ofi => Task.Run(() => AnalisisInfoOfi2(ofi, filtro))).ToList();
            var results = await Task.WhenAll(tareas);

            // * combine the results from all tasks
            var response = results.Where( r => r != null).SelectMany(r => r).ToList();
            return response;
        }

        private List<CatPadron> AnalisisInfoOfi(Ruta ofi, AnalisysInfoFilter filtro)
        {
            throw new NotImplementedException();

            // Console.WriteLine($">> Iniciando Analisis Info Oficina: {ofi.Oficina}");
            // var _result = new List<CatPadron>();
            // try{
            //     var _datos = new List<VwCatPadron>();
            //     using( var _arquosContext = new ArquosContext(ofi.StringConection)){
            //         _datos = _arquosContext.VwCatPadrons.OrderByDescending( item => item.FechaAlta ).ToList();
            //     }

            //     //****** Aplicar Filtro
            //     if(filtro.RazonSocial.Trim().Length > 1){
            //         _datos = _datos.Where( item => item.RazonSocial.ToLower().Contains(filtro.RazonSocial.ToLower())).ToList();
            //     }
            //     if(filtro.Direccion.Trim().Length > 1){
            //         _datos = _datos.Where( item => (item.Direccion + item.CallePpal + item.CalleLat1 + item.CalleLat2).ToLower().Contains(filtro.Direccion.ToLower())).ToList();
            //     }
            //     if(filtro.Colonia.Trim().Length > 1){
            //         _datos = _datos.Where( item => item.Colonia.ToLower().Contains(filtro.Colonia.ToLower())).ToList();
            //     }

            //     if(filtro.Id_Estatus.Count() > 0){
            //         _datos = _datos.Where( item => filtro.Id_Estatus.Contains((int)item.IdEstatus)).ToList();
            //     }
            //     if(filtro.Id_Servicios.Count() > 0){
            //         _datos = _datos.Where( item => filtro.Id_Servicios.Contains((int)item.IdServicio)).ToList();
            //     }
            //     if(filtro.Id_Tarifas.Count() > 0){
            //         _datos = _datos.Where( item => filtro.Id_Tarifas.Contains((int)item.IdTarifa)).ToList();
            //     }
            //     if(filtro.Id_TipoCalculo.Count() > 0){
            //         _datos = _datos.Where( item => filtro.Id_TipoCalculo.Contains((int)item.IdTipocalculo)).ToList();
            //     }
            //     if(filtro.Id_Anomalias.Count() > 0){
            //         _datos = _datos.Where( item => filtro.Id_Anomalias.Contains((int)item.IdAnomaliaAct)).ToList();
            //     }
            //     if(filtro.Id_Giro.Count() > 0){
            //         _datos = _datos.Where( item => filtro.Id_Giro.Contains((int)item.IdGiro)).ToList();
            //     }
            //     if(filtro.Id_ClaseUsuario.Count() > 0){
            //         _datos = _datos.Where( item => filtro.Id_ClaseUsuario.Contains((int)item.IdClaseusuario)).ToList();
            //     }

            //     if(filtro.ImporteTarifaAgua_Opcion > 0){
            //         switch (filtro.ImporteTarifaAgua_Opcion){
            //             case 1: //Mayor Que
            //                 _datos = _datos.Where( item => (item.ImporteFijo??0m) >= filtro.ImporteTarifaAgua_Valor1).ToList();
            //                 break;

            //             case 2: //Menor Que
            //                 _datos = _datos.Where( item => (item.ImporteFijo??0m) <= filtro.ImporteTarifaAgua_Valor1).ToList();
            //                 break;

            //             case 3: //Entre
            //                 _datos = _datos.Where( item => (item.ImporteFijo??0m) >= filtro.ImporteTarifaAgua_Valor1 && (item.ImporteFijo??0m) <= filtro.ImporteTarifaAgua_Valor2).ToList();
            //                 break;

            //             case 4: //Igual
            //                 _datos = _datos.Where( item => (item.ImporteFijo??0m) == filtro.ImporteTarifaAgua_Valor1).ToList();
            //                 break;
            //         }
            //     }
            //     if(filtro.ImporteTarifaDren_Opcion > 0){
            //         switch (filtro.ImporteTarifaDren_Opcion){
            //             case 1: //Mayor Que
            //                 _datos = _datos.Where( item => (item.ImporteFijoDren??0m) >= filtro.ImporteTarifaDren_Valor1).ToList();
            //                 break;

            //             case 2: //Menor Que
            //                 _datos = _datos.Where( item => (item.ImporteFijoDren??0m) <= filtro.ImporteTarifaDren_Valor1).ToList();
            //                 break;

            //             case 3: //Entre
            //                 _datos = _datos.Where( item => (item.ImporteFijoDren??0m) >= filtro.ImporteTarifaDren_Valor1 && (item.ImporteFijoDren??0m) <= filtro.ImporteTarifaDren_Valor2).ToList();
            //                 break;

            //             case 4: //Igual
            //                 _datos = _datos.Where( item => (item.ImporteFijoDren??0m) == filtro.ImporteTarifaDren_Valor1).ToList();
            //                 break;
            //         }
            //     }
            //     if(filtro.ImporteTarifaSane_Opcion > 0){
            //         switch (filtro.ImporteTarifaSane_Opcion){
            //             case 1: //Mayor Que
            //                 _datos = _datos.Where( item => (item.ImporteFijoSane??0m) >= filtro.ImporteTarifaSane_Valor1).ToList();
            //                 break;

            //             case 2: //Menor Que
            //                 _datos = _datos.Where( item => (item.ImporteFijoSane??0m) <= filtro.ImporteTarifaSane_Valor1).ToList();
            //                 break;

            //             case 3: //Entre
            //                 _datos = _datos.Where( item => (item.ImporteFijoSane??0m) >= filtro.ImporteTarifaSane_Valor1 && (item.ImporteFijoSane??0m) <= filtro.ImporteTarifaSane_Valor2).ToList();
            //                 break;

            //             case 4: //Igual
            //                 _datos = _datos.Where( item => (item.ImporteFijoSane??0m) == filtro.ImporteTarifaSane_Valor1).ToList();
            //                 break;
            //         }
            //     }
            //     if(filtro.Consumo_Opcion > 0){
            //         switch (filtro.Consumo_Opcion){
            //             case 1: //Mayor Que
            //                 _datos = _datos.Where( item => (item.ConsumoAct??0m) >= filtro.Consumo_Valor1).ToList();
            //                 break;

            //             case 2: //Menor Que
            //                 _datos = _datos.Where( item => (item.ConsumoAct??0m) <= filtro.Consumo_Valor1).ToList();
            //                 break;

            //             case 3: //Entre
            //                 _datos = _datos.Where( item => (item.ConsumoAct??0m) >= filtro.Consumo_Valor1 && (item.ConsumoAct??0m) <= filtro.Consumo_Valor2).ToList();
            //                 break;

            //             case 4: //Igual
            //                 _datos = _datos.Where( item => (item.ConsumoAct??0m) == filtro.Consumo_Valor1).ToList();
            //                 break;
            //         }
            //     }
            //     if(filtro.LPS_Opcion > 0){
            //         switch (filtro.LPS_Opcion){
            //             case 1: //Mayor Que
            //                 _datos = _datos.Where( item => (item.LpsPagados??0m) >= filtro.LPS_Valor1).ToList();
            //                 break;

            //             case 2: //Menor Que
            //                 _datos = _datos.Where( item => (item.LpsPagados??0m) <= filtro.LPS_Valor1).ToList();
            //                 break;

            //             case 3: //Entre
            //                 _datos = _datos.Where( item => (item.LpsPagados??0m) >= filtro.LPS_Valor1 && (item.LpsPagados??0m) <= filtro.LPS_Valor2).ToList();
            //                 break;

            //             case 4: //Igual
            //                 _datos = _datos.Where( item => (item.LpsPagados??0m) == filtro.LPS_Valor1).ToList();
            //                 break;
            //         }
            //     }
            //     if(filtro.MesesAdeudo_Opcion > 0){
            //         switch (filtro.MesesAdeudo_Opcion){
            //             case 1: //Mayor Que
            //                 _datos = _datos.Where( item => (item.MesAdeudoAct??0m) >= filtro.MesesAdeudo_Valor1).ToList();
            //                 break;

            //             case 2: //Menor Que
            //                 _datos = _datos.Where( item => (item.MesAdeudoAct??0m) <= filtro.MesesAdeudo_Valor1).ToList();
            //                 break;

            //             case 3: //Entre
            //                 _datos = _datos.Where( item => (item.MesAdeudoAct??0m) >= filtro.MesesAdeudo_Valor1 && (item.MesAdeudoAct??0m) <= filtro.MesesAdeudo_Valor2).ToList();
            //                 break;

            //             case 4: //Igual
            //                 _datos = _datos.Where( item => (item.MesAdeudoAct??0m) == filtro.MesesAdeudo_Valor1).ToList();
            //                 break;
            //         }
            //     }
            //     if(filtro.LectAct_Opcion > 0){
            //         switch (filtro.LectAct_Opcion){
            //             case 1: //Mayor Que
            //                 _datos = _datos.Where( item => (item.LecturaAct??0m) >= filtro.LectAct_Valor1).ToList();
            //                 break;

            //             case 2: //Menor Que
            //                 _datos = _datos.Where( item => (item.LecturaAct??0m) <= filtro.LectAct_Valor1).ToList();
            //                 break;

            //             case 3: //Entre
            //                 _datos = _datos.Where( item => (item.LecturaAct??0m) >= filtro.LectAct_Valor1 && (item.LecturaAct??0m) <= filtro.LectAct_Valor2).ToList();
            //                 break;

            //             case 4: //Igual
            //                 _datos = _datos.Where( item => (item.LecturaAct??0m) == filtro.LectAct_Valor1).ToList();
            //                 break;
            //         }
            //     }
            //     if(filtro.LectAnt_Opcion > 0){
            //         switch (filtro.LectAnt_Opcion){
            //             case 1: //Mayor Que
            //                 _datos = _datos.Where( item => (item.LecturaAnt??0m) >= filtro.LectAnt_Valor1).ToList();
            //                 break;

            //             case 2: //Menor Que
            //                 _datos = _datos.Where( item => (item.LecturaAnt??0m) <= filtro.LectAnt_Valor1).ToList();
            //                 break;

            //             case 3: //Entre
            //                 _datos = _datos.Where( item => (item.LecturaAnt??0m) >= filtro.LectAnt_Valor1 && (item.LecturaAnt??0m) <= filtro.LectAnt_Valor2).ToList();
            //                 break;

            //             case 4: //Igual
            //                 _datos = _datos.Where( item => (item.LecturaAnt??0m) == filtro.LectAnt_Valor1).ToList();
            //                 break;
            //         }
            //     }
            //     if(filtro.Promedio_Opcion > 0){
            //         switch (filtro.Promedio_Opcion){
            //             case 1: //Mayor Que
            //                 _datos = _datos.Where( item => (item.PromedioAct??0m) >= filtro.Promedio_Valor1).ToList();
            //                 break;

            //             case 2: //Menor Que
            //                 _datos = _datos.Where( item => (item.PromedioAct??0m) <= filtro.Promedio_Valor1).ToList();
            //                 break;

            //             case 3: //Entre
            //                 _datos = _datos.Where( item => (item.PromedioAct??0m) >= filtro.Promedio_Valor1 && (item.PromedioAct??0m) <= filtro.Promedio_Valor2).ToList();
            //                 break;

            //             case 4: //Igual
            //                 _datos = _datos.Where( item => (item.PromedioAct??0m) == filtro.Promedio_Valor1).ToList();
            //                 break;
            //         }
            //     }

            //     if(filtro.FechaLect_Opcion > 0){
            //         switch (filtro.FechaLect_Opcion){
            //             case 1: //Mayor Que
            //                 _datos = _datos.Where( item => (DateTime.TryParse(item.FechaLecturaAct, out DateTime tmpD)?tmpD:null) > filtro.FechaLect_Valor1).ToList();
            //                 break;

            //             case 2: //Menor Que 
            //                 _datos = _datos.Where( item => (DateTime.TryParse(item.FechaLecturaAct, out DateTime tmpD)?tmpD:null) < filtro.FechaLect_Valor1).ToList();
            //                 break;

            //             case 3: //Menor Que 
            //                 _datos = _datos.Where( item => (DateTime.TryParse(item.FechaLecturaAct, out DateTime tmpD)?tmpD:null) >= filtro.FechaLect_Valor1 && (DateTime.TryParse(item.FechaLecturaAct, out tmpD)?tmpD:null) <= filtro.FechaLect_Valor2).ToList();
            //                 break;
                            
            //             case 4: //Igual
            //                 _datos = _datos.Where( item => (DateTime.TryParse(item.FechaLecturaAct, out DateTime tmpD)?tmpD:null) == filtro.FechaLect_Valor1).ToList();
            //                 break;
            //         }
            //     }
            //     if(filtro.FechaVenci_Opcion > 0){
            //         switch (filtro.FechaLect_Opcion){
            //             case 1: //Mayor Que
            //                 _datos = _datos.Where( item => (DateTime.TryParse(item.FechaVencimientoAct, out DateTime tmpD)?tmpD:null) > filtro.FechaVenci_Valor1).ToList();
            //                 break;

            //             case 2: //Menor Que 
            //                 _datos = _datos.Where( item => (DateTime.TryParse(item.FechaVencimientoAct, out DateTime tmpD)?tmpD:null) < filtro.FechaVenci_Valor1).ToList();
            //                 break;

            //             case 3: //Menor Que 
            //                     _datos = _datos.Where( item => (DateTime.TryParse(item.FechaVencimientoAct, out DateTime tmpD)?tmpD:null) >= filtro.FechaVenci_Valor1 && (DateTime.TryParse(item.FechaVencimientoAct, out tmpD)?tmpD:null) <= filtro.FechaVenci_Valor2).ToList();
            //                 break;
                            
            //             case 4: //Igual
            //                 _datos = _datos.Where( item => (DateTime.TryParse(item.FechaVencimientoAct, out DateTime tmpD)?tmpD:null) == filtro.FechaVenci_Valor1).ToList();
            //                 break;
            //         }
            //     }

            //     if(filtro.EsDraef_Opcion > 0) {
            //         switch(filtro.EsDraef_Opcion) {
            //             case 1:
            //                 _datos = _datos.Where(item => item.EsDraef == true).ToList();
            //                 break;
            //             case 2:
            //                 _datos = _datos.Where(item => item.EsDraef == false).ToList();
            //                 break;
            //         }
            //     }
            //     if(filtro.AltoConsumidor_Opcion > 0) {
            //         switch(filtro.AltoConsumidor_Opcion) {
            //             case 1:
            //                 _datos = _datos.Where(item => item.EsAltoconsumidor == true).ToList();
            //                 break;
            //             case 2:
            //                 _datos = _datos.Where(item => item.EsAltoconsumidor == false).ToList();
            //                 break;
            //         }
            //     }
            //     if(filtro.TienePozo_Opcion> 0) {
            //         switch(filtro.TienePozo_Opcion) {
            //             case 1:
            //                 _datos = _datos.Where(item => item.TienePozo == true).ToList();
            //                 break;
            //             case 2:
            //                 _datos = _datos.Where(item => item.TienePozo == false).ToList();
            //                 break;
            //         }
            //     }


            //     //****** Convertir lista
            //     _result = _datos.Select( item => new CatPadron(ofi, item)).ToList();
            //     Console.WriteLine($">> Finalizando Analisis Info Oficina: {ofi.Oficina}");
            //     return _result;
            // }catch(Exception err){
            //     Console.WriteLine($">> Error analisis infomarcion oficina: {ofi.Oficina}\n\tError: {err.Message}\n\tStacktrace:{err.StackTrace}");
            //     return null;
            // }
        }
        private List<CatPadron> AnalisisInfoOfi2(Ruta ofi, AnalisysInfoFilter filtro)
        {
            logger.LogDebug("Beginning search on office {officeId}:{officeName}", ofi.Id, ofi.Oficina);
            var _result = new List<CatPadron>();
            try
            {
                var busquea = new BusquedaAvanzada();
                var _query = busquea.GenerarQuery(filtro);
                var _datos = busquea.EjecutarConsulta(ofi, _query).ToList();

                // * Convertir lista
                _result = _datos.Select( item => new CatPadron(ofi, item)).ToList();

                // * Si se paso como parametro un listado de cuentas, filtrarlas
                if(filtro.Cuentas.Count() > 0)
                {
                    _result = _result.Where( item => filtro.Cuentas.Contains((int)item.Id_Cuenta)).ToList();
                }

                logger.LogInformation("Finished search on office '{officeId}:{officeName}'", ofi.Id, ofi.Oficina);
                return _result;
            }
            catch(Exception err)
            {
                logger.LogError(err, "Fail at get the results of the search on the office '{officeId}:{officeName}': {message}", ofi.Id, ofi.Nombre, err.Message);
                return null;
            }
        }

    }

}