using System;
using System.Data;
using System.Collections.Generic;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.Recaudacion.Models
{
    public class AnaliticoResumenAno
    {
        public IEnlace Enlace {get;set;}
        public int Id { get => Enlace?.Id ?? 0; }
        public string Oficina {get => Enlace?.Nombre ?? String.Empty;}

        public int Ano {get;set;}
        public decimal Ene { get; set;}
        public decimal Feb { get; set;}
        public decimal Mar { get; set;}
        public decimal Abr { get; set;}
        public decimal May { get; set;}
        public decimal Jun { get; set;}
        public decimal Jul { get; set;}
        public decimal Ago { get; set;}
        public decimal Sep { get; set;}
        public decimal Oct { get; set;}
        public decimal Nov { get; set;}
        public decimal Dic { get; set;}
        public decimal Total { get; set;}

        public decimal GetValueByMonth(int i)
        {
            return i switch {
                1 => Ene,
                2 => Feb,
                3 => Mar,
                4 => Abr,
                5 => May,
                6 => Jun,
                7 => Jul,
                8 => Ago,
                9 => Sep,
                10 => Oct,
                11 => Nov,
                12 => Dic,
                _ => Total
            };
        }

        public AnaliticoResumenAno(IEnlace enlace)
        {
            this.Enlace = enlace;
            this.Ano = DateTime.Now.Year;
        }

        public AnaliticoResumenAno(IEnlace enlace, int year)
        {
            this.Enlace = enlace;
            this.Ano = year;
        }

        public static AnaliticoResumenAno FromDataReader(IEnlace enlace, IDataReader reader)
        {
            var newitem = new AnaliticoResumenAno(enlace)
            {
                Ano = ConvertUtils.ParseInteger(reader["Año"]),
                Ene = ConvertUtils.ParseDecimal(reader["ENE"]),
                Feb = ConvertUtils.ParseDecimal(reader["FEB"]),
                Mar = ConvertUtils.ParseDecimal(reader["MAR"]),
                Abr = ConvertUtils.ParseDecimal(reader["ABR"]),
                May = ConvertUtils.ParseDecimal(reader["MAY"]),
                Jun = ConvertUtils.ParseDecimal(reader["JUN"]),
                Jul = ConvertUtils.ParseDecimal(reader["JUL"]),
                Ago = ConvertUtils.ParseDecimal(reader["AGO"]),
                Sep = ConvertUtils.ParseDecimal(reader["SEP"]),
                Oct = ConvertUtils.ParseDecimal(reader["OCT"]),
                Nov = ConvertUtils.ParseDecimal(reader["NOV"]),
                Dic = ConvertUtils.ParseDecimal(reader["DIC"]),
                Total = ConvertUtils.ParseDecimal(reader["TOTAL"])
            };
            return newitem;
        }
    }
}