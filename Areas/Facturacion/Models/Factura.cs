using System;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Facturacion.Models {
    public class Factura {
        public string IdFactura {get;set;} = "";
        public DateTime? Fecha {get;set;}
        public long IdPadron {get;set;} = 0;
        public long IdCuenta {get;set;} = 0;
        public decimal Subtotal {get;set;} = 0m;
        public decimal Iva {get;set;} = 0m;
        public decimal Total {get;set;} = 0m;
        public int IdSucursal {get;set;} = 0;
        public string Sucursal {get;set;} = "";
        public int IdGenero {get;set;} = 0;
        public string Genero {get;set;} = "";
        public int IdEstatus {get;set;} = 0;
        public string Estatus {get;set;} = "";
        public int Mf {get;set;} = 0;
        public int Af {get;set;} = 0;
        public int IdServicio {get;set;} = 0;
        public string Servicio {get;set;} = "";
        public int IdTarifa {get;set;} = 0;
        public string Tarifa {get;set;} = "";
        public int Consumo {get;set;} = 0;
        public int IdTipoCalculado {get;set;} = 0;
        public string TipoCalculado {get;set;} = "";
        public int IdLecturista {get;set;} = 0;
        public string Lecturista {get;set;} = "";
        public int IdCapturo {get;set;} = 0;
        public string Capturo {get;set;} = "";
        public decimal AguaSb {get;set;} = 0m;
        public decimal AguaIva {get;set;} = 0m;
        public decimal AguaTotal {get;set;} = 0m;
        public decimal DrenajeSb {get;set;} = 0m;
        public decimal DrenajeIva {get;set;} = 0m;
        public decimal DrenajeTotal {get;set;} = 0m;
        public decimal SaneamientoSb {get;set;} = 0m;
        public decimal SaneamientoIva {get;set;} = 0m;
        public decimal SaneamientoTotal {get;set;} = 0m;
        public decimal ActualizacionSb {get;set;} = 0m;
        public decimal ActualizacionIva {get;set;} = 0m;
        public decimal ActualizacionTotal {get;set;} = 0m;
        public decimal OtrosSb {get;set;} = 0m;
        public decimal OtrosIva {get;set;} = 0m;
        public decimal OtrosTotal {get;set;} = 0m;
        public int IdLocalidad {get;set;} = 0;
        public string Localidad {get;set;} = "";
        public int Subsistema {get;set;} = 0;
        public int Sector {get;set;} = 0;

        public int ConsumoFacturado {
            get => ConvertUtils.ParseInteger(Consumo) < 10 ? 10 : ConvertUtils.ParseInteger(Consumo);
        }

    }
}