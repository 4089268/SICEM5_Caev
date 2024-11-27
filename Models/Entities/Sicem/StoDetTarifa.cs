using System;
using System.Collections.Generic;
using Sicem_Blazor.Tarifas.Data;

namespace Sicem_Blazor.Models
{
    public partial class StoDetTarifa : ITarifa
    {
        public int Id { get; set; }
        public int IdStoTarifa { get; set; }
        public int IdTarifa { get; set; }
        public int IdTipoUsuario { get; set; }
        public int Desde { get; set; }
        public int Hasta { get; set; }
        public decimal Costo { get; set; }
        public decimal CostoBase { get; set; }
        public decimal CuotaBase { get; set; }
        public decimal AdicionalM3 { get; set; }

        public virtual StoTarifa IdStoTarifaNavigation { get; set; }

        // Implementaciones 
        private string _tipoUsuario = "";
        public int Id_Tarifa { 
            get {
                return IdTarifa;
            }
        } 
        public int Id_TipoUsuario { 
            get {
                return IdTipoUsuario;
            }
        }
        public string TipoUsuario {
            get {
                return _tipoUsuario;
            }
        }

        public void SetTipoUsuario(string val){
            this._tipoUsuario = val;
        }
    
    }
}
