using System;
using System.Data;
using System.Collections.Generic;
using Sicem_Blazor.Recaudacion.Models;
using System.Linq;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Recaudacion.Data {

    public class RecaudacionAnaliticoFactory {

        private Recaudacion_Analitico recaudacionAnalitico;

        public RecaudacionAnaliticoFactory(){
            this.recaudacionAnalitico = new Recaudacion_Analitico();
        }

        public void SetDatosAnalitico(DataSet ds){
            var tmpTableMensual = ds.Tables[0];
            this.recaudacionAnalitico.Analitico_Mensual = GetAnaliticoMensualFromTable(tmpTableMensual).ToArray();

            var tmpTableQuincenal = ds.Tables[1];
            this.recaudacionAnalitico.Analitico_Quincenal = GetAnaliticoQuincenalFromTable(tmpTableQuincenal).ToArray();

            var tmpTableSemanal = ds.Tables[2];
            this.recaudacionAnalitico.Analitico_Semanal = GetAnaliticoSemanalFromTable(tmpTableSemanal).ToArray();
            
        }
        public void SetDatosAnaliticoRezago(DataSet ds){
            var tmpTableMensual = ds.Tables[0];
            this.recaudacionAnalitico.AnaliticoRez_Mensual = GetAnaliticoMensualFromTable(tmpTableMensual).ToArray();

            var tmpTableQuincenal = ds.Tables[1];
            this.recaudacionAnalitico.AnaliticoRez_Quincenal = GetAnaliticoQuincenalFromTable(tmpTableQuincenal).ToArray();

            var tmpTableSemanal = ds.Tables[2];
            this.recaudacionAnalitico.AnaliticoRez_Semanal = GetAnaliticoSemanalFromTable(tmpTableSemanal).ToArray();
            
        }

        public Recaudacion_Analitico Build(){
            return recaudacionAnalitico;
        }


        private IEnumerable<Recaudacion_AnaliticoMensual> GetAnaliticoMensualFromTable(DataTable table){
            var results = new List<Recaudacion_AnaliticoMensual>();
            foreach(DataRow row in table.Rows){
                var newItem = new Recaudacion_AnaliticoMensual();
                newItem.Oficina = row.Field<string>("Oficina");
                newItem.Año = row.Field<int>("Año");
                newItem.Ene = ConvertUtils.ParseDecimal(row["Ene"]);
                newItem.Feb = ConvertUtils.ParseDecimal(row["Feb"]);
                newItem.Mar = ConvertUtils.ParseDecimal(row["Mar"]);
                newItem.Abr = ConvertUtils.ParseDecimal(row["Abr"]);
                newItem.May = ConvertUtils.ParseDecimal(row["May"]);
                newItem.Jun = ConvertUtils.ParseDecimal(row["Jun"]);
                newItem.Jul = ConvertUtils.ParseDecimal(row["Jul"]);
                newItem.Ago = ConvertUtils.ParseDecimal(row["Ago"]);
                newItem.Sep = ConvertUtils.ParseDecimal(row["Sep"]);
                newItem.Oct = ConvertUtils.ParseDecimal(row["Oct"]);
                newItem.Nov = ConvertUtils.ParseDecimal(row["Nov"]);
                newItem.Dic = ConvertUtils.ParseDecimal(row["Dic"]);
                newItem.Total = ConvertUtils.ParseDecimal(row["Total"]);
                results.Add(newItem);
            }
            return results;
        }
        private IEnumerable<Recaudacion_AnaliticoQuincenal> GetAnaliticoQuincenalFromTable(DataTable table){
            var results = new List<Recaudacion_AnaliticoQuincenal>();
            foreach(DataRow dataRow in table.Rows){
                var newItem = new Recaudacion_AnaliticoQuincenal();
                newItem.Oficina = dataRow.Field<string>("Oficina");
                newItem.Año = dataRow.Field<int>("Año");
                newItem.Ene_1 = ConvertUtils.ParseDecimal(dataRow["Ene_1"]);
                newItem.Ene_2 = ConvertUtils.ParseDecimal(dataRow["Ene_2"]);
                newItem.Feb_1 = ConvertUtils.ParseDecimal(dataRow["Feb_1"]);
                newItem.Feb_2 = ConvertUtils.ParseDecimal(dataRow["Feb_2"]);
                newItem.Mar_1 = ConvertUtils.ParseDecimal(dataRow["Mar_1"]);
                newItem.Mar_2 = ConvertUtils.ParseDecimal(dataRow["Mar_2"]);
                newItem.Abr_1 = ConvertUtils.ParseDecimal(dataRow["Abr_1"]);
                newItem.Abr_2 = ConvertUtils.ParseDecimal(dataRow["Abr_2"]);
                newItem.May_1 = ConvertUtils.ParseDecimal(dataRow["May_1"]);
                newItem.May_2 = ConvertUtils.ParseDecimal(dataRow["May_2"]);
                newItem.Jun_1 = ConvertUtils.ParseDecimal(dataRow["Jun_1"]);
                newItem.Jun_2 = ConvertUtils.ParseDecimal(dataRow["Jun_2"]);
                newItem.Jul_1 = ConvertUtils.ParseDecimal(dataRow["Jul_1"]);
                newItem.Jul_2 = ConvertUtils.ParseDecimal(dataRow["Jul_2"]);
                newItem.Ago_1 = ConvertUtils.ParseDecimal(dataRow["Ago_1"]);
                newItem.Ago_2 = ConvertUtils.ParseDecimal(dataRow["Ago_2"]);
                newItem.Sep_1 = ConvertUtils.ParseDecimal(dataRow["Sep_1"]);
                newItem.Sep_2 = ConvertUtils.ParseDecimal(dataRow["Sep_2"]);
                newItem.Oct_1 = ConvertUtils.ParseDecimal(dataRow["Oct_1"]);
                newItem.Oct_2 = ConvertUtils.ParseDecimal(dataRow["Oct_2"]);
                newItem.Nov_1 = ConvertUtils.ParseDecimal(dataRow["Nov_1"]);
                newItem.Nov_2 = ConvertUtils.ParseDecimal(dataRow["Nov_2"]);
                newItem.Dic_1 = ConvertUtils.ParseDecimal(dataRow["Dic_1"]);
                newItem.Dic_2 = ConvertUtils.ParseDecimal(dataRow["Dic_2"]);
                newItem.Total = ConvertUtils.ParseDecimal(dataRow["Total"]);
                results.Add(newItem);
            }
            return results;
        }
        private IEnumerable<Recaudacion_AnaliticoSemanal> GetAnaliticoSemanalFromTable(DataTable table){
            var results = new List<Recaudacion_AnaliticoSemanal>();
            foreach(DataRow dataRow in table.Rows){
                var newItem = new Recaudacion_AnaliticoSemanal();
                newItem.Oficina = dataRow.Field<string>("Oficina");
                newItem.Año = dataRow.Field<int>("Año");
                newItem.Ene_1 = ConvertUtils.ParseDecimal( dataRow["Ene_1"] );
                newItem.Ene_2 = ConvertUtils.ParseDecimal( dataRow["Ene_2"] );
                newItem.Ene_3 = ConvertUtils.ParseDecimal( dataRow["Ene_3"] );
                newItem.Ene_4 = ConvertUtils.ParseDecimal( dataRow["Ene_4"] );
                newItem.Feb_1 = ConvertUtils.ParseDecimal( dataRow["Feb_1"] );
                newItem.Feb_2 = ConvertUtils.ParseDecimal( dataRow["Feb_2"] );
                newItem.Feb_3 = ConvertUtils.ParseDecimal( dataRow["Feb_3"] );
                newItem.Feb_4 = ConvertUtils.ParseDecimal( dataRow["Feb_4"] );
                newItem.Mar_1 = ConvertUtils.ParseDecimal( dataRow["Mar_1"] );
                newItem.Mar_2 = ConvertUtils.ParseDecimal( dataRow["Mar_2"] );
                newItem.Mar_3 = ConvertUtils.ParseDecimal( dataRow["Mar_3"] );
                newItem.Mar_4 = ConvertUtils.ParseDecimal( dataRow["Mar_4"] );
                newItem.Abr_1 = ConvertUtils.ParseDecimal( dataRow["Abr_1"] );
                newItem.Abr_2 = ConvertUtils.ParseDecimal( dataRow["Abr_2"] );
                newItem.Abr_3 = ConvertUtils.ParseDecimal( dataRow["Abr_3"] );
                newItem.Abr_4 = ConvertUtils.ParseDecimal( dataRow["Abr_4"] );
                newItem.May_1 = ConvertUtils.ParseDecimal( dataRow["May_1"] );
                newItem.May_2 = ConvertUtils.ParseDecimal( dataRow["May_2"] );
                newItem.May_3 = ConvertUtils.ParseDecimal( dataRow["May_3"] );
                newItem.May_4 = ConvertUtils.ParseDecimal( dataRow["May_4"] );
                newItem.Jun_1 = ConvertUtils.ParseDecimal( dataRow["Jun_1"] );
                newItem.Jun_2 = ConvertUtils.ParseDecimal( dataRow["Jun_2"] );
                newItem.Jun_3 = ConvertUtils.ParseDecimal( dataRow["Jun_3"] );
                newItem.Jun_4 = ConvertUtils.ParseDecimal( dataRow["Jun_4"] );
                newItem.Jul_1 = ConvertUtils.ParseDecimal( dataRow["Jul_1"] );
                newItem.Jul_2 = ConvertUtils.ParseDecimal( dataRow["Jul_2"] );
                newItem.Jul_3 = ConvertUtils.ParseDecimal( dataRow["Jul_3"] );
                newItem.Jul_4 = ConvertUtils.ParseDecimal( dataRow["Jul_4"] );
                newItem.Ago_1 = ConvertUtils.ParseDecimal( dataRow["Ago_1"] );
                newItem.Ago_2 = ConvertUtils.ParseDecimal( dataRow["Ago_2"] );
                newItem.Ago_3 = ConvertUtils.ParseDecimal( dataRow["Ago_3"] );
                newItem.Ago_4 = ConvertUtils.ParseDecimal( dataRow["Ago_4"] );
                newItem.Sep_1 = ConvertUtils.ParseDecimal( dataRow["Sep_1"] );
                newItem.Sep_2 = ConvertUtils.ParseDecimal( dataRow["Sep_2"] );
                newItem.Sep_3 = ConvertUtils.ParseDecimal( dataRow["Sep_3"] );
                newItem.Sep_4 = ConvertUtils.ParseDecimal( dataRow["Sep_4"] );
                newItem.Oct_1 = ConvertUtils.ParseDecimal( dataRow["Oct_1"] );
                newItem.Oct_2 = ConvertUtils.ParseDecimal( dataRow["Oct_2"] );
                newItem.Oct_3 = ConvertUtils.ParseDecimal( dataRow["Oct_3"] );
                newItem.Oct_4 = ConvertUtils.ParseDecimal( dataRow["Oct_4"] );
                newItem.Nov_1 = ConvertUtils.ParseDecimal( dataRow["Nov_1"] );
                newItem.Nov_2 = ConvertUtils.ParseDecimal( dataRow["Nov_2"] );
                newItem.Nov_3 = ConvertUtils.ParseDecimal( dataRow["Nov_3"] );
                newItem.Nov_4 = ConvertUtils.ParseDecimal( dataRow["Nov_4"] );
                newItem.Dic_1 = ConvertUtils.ParseDecimal( dataRow["Dic_1"] );
                newItem.Dic_2 = ConvertUtils.ParseDecimal( dataRow["Dic_2"] );
                newItem.Dic_3 = ConvertUtils.ParseDecimal( dataRow["Dic_3"] );
                newItem.Dic_4 = ConvertUtils.ParseDecimal( dataRow["Dic_4"] );
                newItem.Total = ConvertUtils.ParseDecimal( dataRow["Total"] );
                results.Add(newItem);
            }
            return results;
        }


    }

}
