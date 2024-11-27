using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Sicem_Blazor.Data;
using Sicem_Blazor.Tarifas.Data;

namespace Sicem_Blazor.Models
{
    public partial class CatTarifa :ITarifa
    {
        public decimal Af { get; set; }
        public decimal Mf { get; set; }
        public decimal? IdTipousuario { get; set; }
        public decimal? Desde { get; set; }
        public decimal? Hasta { get; set; }
        public decimal? Base { get; set; }
        public decimal? Adicional { get; set; }
        public int Id { get; set; }

        // Implementacion de Interfas
        private string _tipoUsuario = "";
        public int Id_Tarifa =>  -1;

        [NotMapped]
        public string TipoUsuario { get => _tipoUsuario; set => _tipoUsuario = value;}
        [NotMapped]
        public int Id_TipoUsuario => ConvertUtils.ParseInteger(IdTipousuario.ToString());
        [NotMapped]
        public decimal CostoBase { get => Base??0m; set => Base = value; }
        [NotMapped]
        public decimal Costo { get => Adicional??0m; set => Adicional = value; }
        [NotMapped]
        public decimal CuotaBase { get => Base??0m; set => Base = value; }
        [NotMapped]
        public decimal AdicionalM3 { get => Adicional??0m; set => Adicional = value; }
        [NotMapped]
        int ITarifa.Desde {
            get {
                return ConvertUtils.ParseInteger(Desde.ToString());
            }
            set {
                Desde = ConvertUtils.ParseDecimal(value.ToString());
            }
        }
        
        [NotMapped]
        int ITarifa.Hasta {
            get {
                return ConvertUtils.ParseInteger(Hasta.ToString());
            }
            set {
                Hasta = ConvertUtils.ParseDecimal(value.ToString());
            }
        }
        
    }
}
