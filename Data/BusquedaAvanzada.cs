using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Sicem_Blazor.Models;
using Sicem_Blazor.Models.Entities.Arquos;

namespace Sicem_Blazor {
    public class BusquedaAvanzada {
        private Ruta oficina {get;set;}

        public string GenerarQuery(AnalisysInfoFilter filtro){
            var _queryBuilder = new System.Text.StringBuilder();
            _queryBuilder.Append("Select * From [Padron].[vw_Cat_Padron] Where 1=1 ");

            //****** Aplicar Filtro
                if(filtro.RazonSocial.Trim().Length > 1){
                    _queryBuilder.Append($" and razon_social like '%{filtro.RazonSocial}%'");
                }
                if(filtro.Direccion.Trim().Length > 1){
                    _queryBuilder.Append($" and Concat(direccion, calle_ppal, calle_lat1, calle_lat2) like '%{filtro.Direccion}%'");
                }
                if(filtro.Colonia.Trim().Length > 1){
                    _queryBuilder.Append($" and colonia like '%{filtro.Colonia}%'");
                }
                //Nombre Comercial
                if(filtro.Localidad.Trim().Length > 1 ){
                    _queryBuilder.Append( $" and _poblacion like '%{filtro.Localidad}%'");
                }
                
                if(filtro.Id_Estatus.Count() > 0){
                    _queryBuilder.Append($" and id_estatus in (");
                    filtro.Id_Estatus.ForEach( id => _queryBuilder.Append($" {id},"));
                    _queryBuilder.Remove(_queryBuilder.Length-1,1); // Quitar la ultima coma
                    _queryBuilder.Append(")");
                }
                if(filtro.Id_Servicios.Count() > 0){
                    _queryBuilder.Append($" and id_servicio in (");
                    filtro.Id_Servicios.ForEach( id => _queryBuilder.Append($" {id},"));
                    _queryBuilder.Remove(_queryBuilder.Length-1,1); // Quitar la ultima coma
                    _queryBuilder.Append(")");
                }
                if(filtro.Id_Tarifas.Count() > 0){
                    _queryBuilder.Append($" and id_tarifa in (");
                    filtro.Id_Tarifas.ForEach( id => _queryBuilder.Append($" {id},"));
                    _queryBuilder.Remove(_queryBuilder.Length-1,1); // Quitar la ultima coma
                    _queryBuilder.Append(")");
                }
                if(filtro.Id_TipoCalculo.Count() > 0){
                    _queryBuilder.Append($" and id_tipocalculo in (");
                    filtro.Id_TipoCalculo.ForEach( id => _queryBuilder.Append($" {id},"));
                    _queryBuilder.Remove(_queryBuilder.Length-1,1); // Quitar la ultima coma
                    _queryBuilder.Append(")");
                }
                if(filtro.Id_Anomalias.Count() > 0){
                    _queryBuilder.Append($" and id_anomalia_act in (");
                    filtro.Id_Anomalias.ForEach( id => _queryBuilder.Append($" {id},"));
                    _queryBuilder.Remove(_queryBuilder.Length-1,1); // Quitar la ultima coma
                    _queryBuilder.Append(")");
                }
                if(filtro.Id_Giro.Count() > 0){
                    _queryBuilder.Append($" and id_giro in (");
                    filtro.Id_Giro.ForEach( id => _queryBuilder.Append($" {id},"));
                    _queryBuilder.Remove(_queryBuilder.Length-1,1); // Quitar la ultima coma
                    _queryBuilder.Append(")");
                }
                if(filtro.Id_ClaseUsuario.Count() > 0){
                    _queryBuilder.Append($" and id_claseusuario in (");
                    filtro.Id_ClaseUsuario.ForEach( id => _queryBuilder.Append($" {id},"));
                    _queryBuilder.Remove(_queryBuilder.Length-1,1); // Quitar la ultima coma
                    _queryBuilder.Append(")");
                }

                if(filtro.Consumo_Opcion > 0){
                    switch (filtro.Consumo_Opcion){
                        case 1: //Mayor Que
                            _queryBuilder.Append($" and consumo_act > {filtro.Consumo_Valor1}");
                            break;
                        case 2: //Menor Que
                            _queryBuilder.Append($" and consumo_act < {filtro.Consumo_Valor1}");
                            break;
                        case 3: //Entre
                            _queryBuilder.Append($" and consumo_act between {filtro.Consumo_Valor1} and {filtro.Consumo_Valor2}");
                            break;
                        case 4: //Igual
                            _queryBuilder.Append($" and consumo_act = {filtro.Consumo_Valor1}");
                            break;
                    }
                }
                if(filtro.MesesAdeudo_Opcion > 0){
                    switch (filtro.MesesAdeudo_Opcion){
                        case 1: //Mayor Que
                            _queryBuilder.Append($" and mes_adeudo_act > {filtro.MesesAdeudo_Valor1}");
                            break;
                        case 2: //Menor Que
                            _queryBuilder.Append($" and mes_adeudo_act < {filtro.MesesAdeudo_Valor1}");
                            break;
                        case 3: //Entre
                            _queryBuilder.Append($" and mes_adeudo_ant between {filtro.MesesAdeudo_Valor1} and {filtro.MesesAdeudo_Valor2}");
                            break;
                        case 4: //Igual
                            _queryBuilder.Append($" and mes_adeudo_ant = {filtro.MesesAdeudo_Valor1}");
                            break;
                    }
                }
                if(filtro.Saldo_Opcion > 0){
                    switch (filtro.Saldo_Opcion){
                        case 1: //Mayor Que
                            _queryBuilder.Append($" and total > {filtro.Saldo_Valor1}");
                            break;
                        case 2: //Menor Que
                            _queryBuilder.Append($" and total < {filtro.Saldo_Valor1}");
                            break;
                        case 3: //Entre
                            _queryBuilder.Append($" and total between {filtro.Saldo_Valor1} and {filtro.Saldo_Valor2}");
                            break;
                        case 4: //Igual
                            _queryBuilder.Append($" and total = {filtro.Saldo_Valor1}");
                            break;
                    }
                }

                if(filtro.EsDraef_Opcion > 0) {
                    switch(filtro.EsDraef_Opcion) {
                        case 1:
                            _queryBuilder.Append($" and es_draef = 1");
                            break;
                        case 2:
                            _queryBuilder.Append($" and es_draef = 0");
                            break;
                    }
                }
                if(filtro.AltoConsumidor_Opcion > 0) {
                    switch(filtro.AltoConsumidor_Opcion) {
                        case 1:
                            _queryBuilder.Append($" and es_altoconsumidor = 1");
                            break;
                        case 2:
                            _queryBuilder.Append($" and es_altoconsumidor = 0");
                            
                            break;
                    }
                }
                if(filtro.TienePozo_Opcion> 0) {
                    switch(filtro.TienePozo_Opcion) {
                        case 1:
                            _queryBuilder.Append($" and tiene_pozo = 1");
                            break;
                        case 2:
                            _queryBuilder.Append($" and tiene_pozo = 0");
                            break;
                    }
                }
                if(filtro.TieneUbicacion> 0) {
                    switch(filtro.TieneUbicacion) {
                        case 1:
                            _queryBuilder.Append($" and (CONVERT(int,IsNull(latitud,'0')) <> 0 and Convert(int, IsNull(longitud,'0')) <> 0)");
                            break;
                        case 2:
                            _queryBuilder.Append($" and (CONVERT(int,IsNull(latitud,'0')) = 0 and Convert(int, IsNull(longitud,'0')) = 0)");
                            break;
                    }
                }

                if(filtro.Subsistema_Opcion > 0){
                    switch (filtro.Subsistema_Opcion){
                        case 1: //Mayor Que
                            _queryBuilder.Append($" and sb > {filtro.Subsistema_Valor1}");
                            break;
                        case 2: //Menor Que
                            _queryBuilder.Append($" and sb < {filtro.Subsistema_Valor1}");
                            break;
                        case 3: //Entre
                            _queryBuilder.Append($" and sb between {filtro.Subsistema_Valor1} and {filtro.Subsistema_Valor2}");
                            break;
                        case 4: //Igual
                            _queryBuilder.Append($" and sb = {filtro.Subsistema_Valor1}");
                            break;
                    }
                
                }
                if(filtro.Sector_Opcion > 0){
                    switch (filtro.Sector_Opcion){
                        case 1: //Mayor Que
                            _queryBuilder.Append($" and sector > {filtro.Sector_Valor1}");
                            break;
                        case 2: //Menor Que
                            _queryBuilder.Append($" and sector < {filtro.Sector_Valor1}");
                            break;
                        case 3: //Entre
                            _queryBuilder.Append($" and sector between {filtro.Sector_Valor1} and {filtro.Sector_Valor2}");
                            break;
                        case 4: //Igual
                            _queryBuilder.Append($" and sector = {filtro.Sector_Valor1}");
                            break;
                    }
                
                }

            return _queryBuilder.ToString();
        }

        public IEnumerable<VwCatPadron> EjecutarConsulta(Ruta oficina, String query)
        {
            try {
                var results = new List<VwCatPadron>();
                using(var connection = new SqlConnection(oficina.StringConection)){
                    connection.Open();
                    var command = new SqlCommand(query, connection);
                    using(SqlDataReader reader = command.ExecuteReader()){
                        while(reader.Read()){
                            var _newItem = new VwCatPadron();
                            
                            var tmpDec = 0m;
                            var tmpBol = false;

                            _newItem.IdPadron = decimal.TryParse(reader["id_padron"].ToString(), out tmpDec)?tmpDec:0m;;
                            _newItem.IdCuenta = decimal.TryParse(reader["id_cuenta"].ToString(), out tmpDec)?tmpDec:0m;;
                            _newItem.NomComercial = reader["nom_comercial"].ToString();
                            _newItem.NomPropietario = reader["nom_propietario"].ToString();
                            _newItem.RazonSocial = reader["razon_social"].ToString();
                            _newItem.Rfc = reader["rfc"].ToString();
                            // _newItem.Curp = reader["curp"].ToString();
                            _newItem.Direccion = reader["direccion"].ToString();
                            _newItem.Colonia = reader["colonia"].ToString();
                            _newItem.Ciudad = reader["ciudad"].ToString();
                            _newItem.Estado = reader["estado"].ToString();
                            _newItem.CodigoPostal = decimal.TryParse(reader["codigo_postal"].ToString(), out tmpDec) ?tmpDec :0m;
                            _newItem.Telefono1 = reader["telefono1"].ToString();
                            _newItem.Telefono2 = reader["telefono2"].ToString();
                            _newItem.Telefono3 = reader["telefono3"].ToString();
                            // _newItem.ReciboMail = bool.TryParse(reader["recibo_mail"].ToString(), out tmpBol)?tmpBol:false;;
                            _newItem.Email = reader["email"].ToString();
                            _newItem.PaginaInternet = reader["pagina_internet"].ToString();
                            _newItem.CallePpal = reader["calle_ppal"].ToString();
                            _newItem.NumExt = reader["num_ext"].ToString();
                            _newItem.NumInt = reader["num_int"].ToString();
                            _newItem.CalleLat1 = reader["calle_lat1"].ToString();
                            _newItem.CalleLat2 = reader["calle_lat2"].ToString();
                            _newItem.Ruta = decimal.TryParse(reader["ruta"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.Sb = decimal.TryParse(reader["sb"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.Sector= decimal.TryParse(reader["sector"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.Manzana= decimal.TryParse(reader["manzana"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.Lote= decimal.TryParse(reader["lote"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.Nivel= decimal.TryParse(reader["nivel"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.Fraccion= decimal.TryParse(reader["fraccion"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.Toma= decimal.TryParse(reader["toma"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.Localizacion= reader["_localizacion"].ToString();
                            // _newItem.IdLocalidad= decimal.TryParse(reader["id_localidad"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.IdColonia= decimal.TryParse(reader["id_colonia"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.IdGiro= decimal.TryParse(reader["id_giro"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.AreaLote= decimal.TryParse(reader["area_lote"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.AreaConstruida= decimal.TryParse(reader["area_construida"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.AreaJardin= decimal.TryParse(reader["area_jardin"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.IdClaseusuario= decimal.TryParse(reader["id_claseusuario"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.Viviendas= decimal.TryParse(reader["viviendas"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.SalidasHidraulicas= decimal.TryParse(reader["salidas_hidraulicas"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.Frente= decimal.TryParse(reader["frente"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.IdEstatus= decimal.TryParse(reader["id_estatus"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.LpsPagados= decimal.TryParse(reader["lps_pagados"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.Mf= decimal.TryParse(reader["mf"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.Af= decimal.TryParse(reader["af"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.Desviacion= decimal.TryParse(reader["desviacion"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.PromedioAnt= decimal.TryParse(reader["promedio_ant"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.PromedioAct= decimal.TryParse(reader["promedio_act"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.MesAdeudoAnt= decimal.TryParse(reader["mes_adeudo_ant"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.MesAdeudoAct = decimal.TryParse(reader["mes_adeudo_act"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.IdServicio = decimal.TryParse(reader["id_servicio"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.IdTarifa = decimal.TryParse(reader["id_tarifa"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.IdTarifafija = decimal.TryParse(reader["id_tarifafija"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.ConsumoFijo = decimal.TryParse(reader["consumo_fijo"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.ImporteFijo = decimal.TryParse(reader["importe_fijo"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.ImporteFijoDren = decimal.TryParse(reader["importe_fijo_dren"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.ImporteFijoSane = decimal.TryParse(reader["importe_fijo_sane"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.IdSituacion = decimal.TryParse(reader["id_situacion"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.IdAnomaliaAct = decimal.TryParse(reader["id_anomalia_act"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.LecturaAnt = decimal.TryParse(reader["lectura_ant"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.LecturaAct = decimal.TryParse(reader["lectura_act"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.ConsumoAnt = decimal.TryParse(reader["consumo_ant"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.ConsumoAct = decimal.TryParse(reader["consumo_act"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.ConsumoRealAnt = decimal.TryParse(reader["consumo_real_ant"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.ConsumoRealAct = decimal.TryParse(reader["consumo_real_act"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.IdTipocalculo = decimal.TryParse(reader["id_tipocalculo"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.Subtotal = decimal.TryParse(reader["subtotal"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.Iva = decimal.TryParse(reader["iva"].ToString(), out tmpDec)?tmpDec:0m;
                            _newItem.Total = decimal.TryParse(reader["total"].ToString(), out tmpDec)?tmpDec:0m;
                            // _newItem.Prefacturado = bool.TryParse(reader["prefacturado"].ToString(), out tmpBol)?tmpBol:false;
                            // _newItem.Anual= bool.TryParse(reader["anual"].ToString(), out tmpBol)?tmpBol:false;
                            // _newItem.Recibo= decimal.TryParse(reader["recibo"].ToString(),out tmpDec)?tmpDec:0m;
                            _newItem.ConsumoForzado= decimal.TryParse(reader["consumo_forzado"].ToString(),out tmpDec)?tmpDec:0m;
                            _newItem.EsFiscal= bool.TryParse(reader["es_fiscal"].ToString(), out tmpBol)?tmpBol:false;
                            _newItem.PorDescto= decimal.TryParse(reader["por_descto"].ToString(),out tmpDec)?tmpDec:0m;
                            _newItem.Visitar= bool.TryParse(reader["visitar"].ToString(), out tmpBol)?tmpBol:false;
                            _newItem.Longitud= decimal.TryParse(reader["longitud"].ToString(),out tmpDec)?tmpDec:0m;
                            _newItem.Latitud= decimal.TryParse(reader["latitud"].ToString(),out tmpDec)?tmpDec:0m;
                            // _newItem.LastIdAbono = reader["last_idAbono"].ToString();
                            // _newItem.LastIdVenta = reader["last_idVenta"].ToString();
                            // _newItem.EsMacromedidor = bool.TryParse(reader["es_macromedidor"].ToString(), out tmpBol)?tmpBol:false;
                            // _newItem.TienePozo = bool.TryParse(reader["tiene_pozo"].ToString(), out tmpBol)?tmpBol:false;
                            // _newItem.EsDraef = bool.TryParse(reader["es_draef"].ToString(), out tmpBol)?tmpBol:false;
                            // _newItem.IdMedidor= reader["id_medidor"].ToString();
                            // _newItem.EsAltoconsumidor = bool.TryParse(reader["es_altoconsumidor"].ToString(), out tmpBol)?tmpBol:false;
                            _newItem.Colonia1 = reader["_colonia"].ToString();
                            _newItem.Poblacion = reader["_poblacion"].ToString();
                            _newItem.Estatus = reader["_estatus"].ToString();
                            _newItem.Calculo = reader["_calculo"].ToString();
                            _newItem.Servicio = reader["_servicio"].ToString();
                            _newItem.Tipousuario = reader["_tipousuario"].ToString();
                            // _newItem.Nivelsocial = reader["_nivelsocial"].ToString();
                            _newItem.Tarifafija = reader["_tarifafija"].ToString();
                            _newItem.Claseusuario = reader["_claseusuario"].ToString();
                            _newItem.Tipogrupo = reader["_tipogrupo"].ToString();
                            _newItem.Giro = reader["_giro"].ToString();
                            _newItem.Situacion = reader["_situacion"].ToString();
                            _newItem.AnomaliaAct = reader["_anomalia_act"].ToString();
                            _newItem.AnomaliaAnt = reader["_anomalia_ant"].ToString();
                            _newItem.Diametro = reader["_diametro"].ToString();
                            _newItem.Hidrocircuito = reader["_hidrocircuito"].ToString();
                            _newItem.CallePpal1 = reader["_calle_ppal"].ToString();
                            _newItem.CalleLat11 = reader["_calle_lat1"].ToString();
                            _newItem.CalleLat21 = reader["_calle_lat2"].ToString();
                            _newItem.Materialmedidor = reader["_materialmedidor"].ToString();
                            _newItem.Tipoinstalacion = reader["_tipoinstalacion"].ToString();
                            _newItem.Ubicacionmedidor = reader["_ubicacionmedidor"].ToString();
                            // _newItem.CalculoAct = reader["_calculo_act"].ToString();
                            _newItem.CalculoAnt = reader["_calculo_ant"].ToString();
                            // _newItem.Zona = reader["_zona"].ToString();
                            _newItem.Tipotoma = reader["_tipotoma"].ToString();
                            _newItem.MaterialToma = reader["_material_toma"].ToString();
                            _newItem.MaterialCalle = reader["_material_calle"].ToString();
                            _newItem.MaterialBanqueta = reader["_material_banqueta"].ToString();
                            _newItem.Tipotuberia = reader["_tipotuberia"].ToString();
                            _newItem.Tipofactible = reader["_tipofactible"].ToString();
                            _newItem.MesFacturado = reader["_MesFacturado"].ToString();
                            _newItem.FechaAlta = Convert.ToDateTime(reader["_fecha_alta"].ToString());
                            _newItem.AltaFactura = reader["_alta_factura"].ToString();
                            _newItem.FechaLecturaAnt = reader["_fecha_lectura_ant"].ToString();
                            _newItem.FechaLecturaAct = reader["_fecha_lectura_act"].ToString();
                            _newItem.FechaFacturaAnt = reader["_fecha_factura_ant"].ToString();
                            _newItem.FechaFacturaAct = reader["_fecha_factura_act"].ToString();
                            _newItem.FechaVencimiento = reader["_fecha_vencimiento"].ToString();
                            // _newItem.FechaVencimientoAct = reader["_fecha_vencimiento_act"].ToString();
                            _newItem.FechaInsert = reader["_fecha_insert"].ToString();
                            results.Add(_newItem);
                        }
                    }
                    connection.Close();
                }
                return results;
            }
            catch(Exception err)
            {
                Console.WriteLine($">>Error al realizar la consulta oficina {oficina.Oficina}\n\tError:{err.Message}\n\tStacktrace:{err.StackTrace}");
                return new VwCatPadron[]{};
            }
        }

    }
}