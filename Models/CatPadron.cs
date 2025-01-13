using System;
using System.Collections.Generic;
using System.Linq;
using Sicem_Blazor.Models.Entities.Arquos;

namespace Sicem_Blazor.Models {
    public class CatPadron {
        public int Id_Oficina {get;set;} = 0;
        public string Oficina {get;set;} = "";
        public long Id_Padron { get; set; } = 0;
        public long Id_Cuenta { get; set; } = 0;
        public string NomComercial { get; set; } = "";
        public string NomPropietario { get; set; } = "";
        public string RazonSocial { get; set; } = "";
        public string Rfc { get; set; } = "";
        public string Curp { get; set; } = "";
        public string Direccion { get; set; } = "";
        public int IdLocalidad { get; set; } = 0;
        public string Poblacion { get; set; } = "";
        public int Id_Colonia { get; set; } = 0;
        public string Colonia { get; set; } = "";
        public string Ciudad { get; set; } = "";
        public string Estado { get; set; } = "";
        public string CodigoPostal { get; set; } = "";
        public string Telefono1 { get; set; } = "";
        public string Telefono2 { get; set; } = "";
        public bool ReciboMail { get; set; } = false;
        public string Email { get; set; } = "";
        public string CallePpal { get; set; } = "";
        public string NumExt { get; set; } = "";
        public string NumInt { get; set; } = "";
        public string CalleLat1 { get; set; } = "";
        public string CalleLat2 { get; set; } = "";
        public int Ruta { get; set; } = 0;
        public int Sb { get; set; } = 0;
        public int Sector { get; set; } = 0;
        public int Manzana { get; set; } = 0;
        public int Lote { get; set; } = 0;
        public int Nivel { get; set; } = 0;
        public int Fraccion { get; set; } = 0;
        public int Toma { get; set; } = 0;
        public string Localizacion { get; set; } = "";
        public int Id_Giro { get; set; } = 0;
        public string Giro { get; set; } = "";
        public int Id_Claseusuario { get; set; } = 0;
        public string Clase_Usuario { get; set; } = "";
        public int Id_Estatus { get; set; } = 0;
        public string Estatus { get; set; } = "";
        public int Mf { get; set; } = 0;
        public int Af { get; set; } = 0;
        public double PromedioAnt { get; set; } = 0;
        public double PromedioAct { get; set; } = 0;
        public int MesAdeudoAnt { get; set; } = 0;
        public int MesAdeudoAct { get; set; } = 0;
        public int Id_Servicio { get; set; } = 0;
        public string Servicio { get; set; } = "";
        public int Id_Tarifa { get; set; } = 0;
        public string Tipo_Usuario { get; set; } = "";
        public int Id_TarifaFija { get; set; } = 0;
        public string Tarifa_Fija { get; set; } = "";
        public int ConsumoFijo { get; set; } = 0;
        public decimal ImporteFijo { get; set; } = 0m;
        public decimal ImporteFijoDren { get; set; } = 0m;
        public decimal ImporteFijoSane { get; set; } = 0m;
        public int Id_Situacion { get; set; } = 0;
        public string Situacion { get; set; } = "";
        public int Id_AnomaliaAct { get; set; } = 0;
        public string AnomaliaAct { get; set; } = "";
        public int LecturaAnt { get; set; } = 0;
        public int LecturaAct { get; set; } = 0;
        public int ConsumoAnt { get; set; } = 0;
        public int ConsumoAct { get; set; } = 0;
        public int Consumo_RealAnt { get; set; } = 0;
        public int Consumo_RealAct { get; set; } = 0;
        public int Id_Tipocalculo { get; set; } = 0;
        public string Calculo { get; set; } = "";
        public string Calculo_Act { get; set; } = "";
        public decimal Subtotal { get; set; } = 0;
        public decimal Iva { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public bool EsMacromedidor { get; set; } = false;
        public bool EsDraef { get; set; } = false;
        public bool EsAltoconsumidor { get; set; } = false;
        public string Id_Medidor { get; set; } = "";
        public string Diametro { get; set; } = "";
        public string Tipotoma { get; set; } = "";
        public string MesFacturado { get; set; } = "";
        public DateTime? FechaAlta { get; set; }
        public DateTime? AltaFactura { get; set; }
        public DateTime? FechaLecturaAct { get; set; }
        public DateTime? FechaFacturaAct { get; set; }
        public DateTime? FechaVencimiento { get; set; }

        public decimal Latitude {get;set;} = 0m;
        public decimal Longitude {get;set;} = 0m;

        public bool TieneUbicacion {
            get {
                return Latitude != 0m && Longitude != 0m;
            }
        }

        public bool TieneTelefono {
            get {
                return !String.IsNullOrEmpty(Telefono1);
            }
        }

        public CatPadron()
        {
            //
        }

        public CatPadron(Ruta oficina, VwCatPadron pad){
            this.Id_Oficina = oficina.Id;
            this.Oficina = oficina.Oficina;
            this.Id_Padron = (long) pad.IdPadron;
            this.Id_Cuenta = (long) pad.IdCuenta;
            this.NomComercial = pad.NomComercial;
            this.NomPropietario = pad.NomPropietario;
            this.RazonSocial = pad.RazonSocial;
            this.Rfc = pad.RazonSocial;
            // this.Curp = pad.Curp;
            this.Direccion = pad.Direccion;
            // this.IdLocalidad = (int) (pad.IdLocalidad??0m);
            this.Poblacion = pad.Poblacion;
            // this.Id_Colonia = (int) pad.IdColonia;
            this.Colonia = pad.Colonia;
            this.Ciudad = pad.Ciudad;
            this.Estado = pad.Estado;
            // this.CodigoPostal = pad.CodigoPostal;
            this.Telefono1 = pad.Telefono1;
            this.Telefono2 = pad.Telefono2;
            // this.ReciboMail = pad.ReciboMail == true;
            this.Email = pad.Email;
            this.CallePpal = pad.CallePpal;
            this.NumExt = pad.NumExt;
            this.NumInt = pad.NumInt;
            this.CalleLat1 = pad.CalleLat1;
            this.CalleLat2 = pad.CalleLat2;
            this.Ruta = (int) (pad.Ruta??0m);
            // this.Sb = (int) (pad.Sb??0m);
            // this.Sector = (int) (pad.Sector??0m);
            // this.Manzana = (int) (pad.Manzana??0m);
            // this.Lote = (int) (pad.Lote??0m);
            // this.Nivel = (int) (pad.Nivel??0m);
            // this.Fraccion = (int) (pad.Fraccion??0m);
            // this.Toma = (int) (pad.Toma??0m);
            this.Localizacion = pad.Localizacion;
            // this.Id_Giro = (int) (pad.IdGiro??0m);
            this.Giro = pad.Giro;
            // this.Id_Claseusuario  = (int) (pad.IdClaseusuario??0m);
            this.Clase_Usuario = pad.Claseusuario;
            // this.Id_Estatus = (int) (pad.IdEstatus??0m);
            this.Estatus = pad.Estatus;
            // this.Mf = (int) (pad.Mf??0m);
            // this.Af = (int) (pad.Af??0m);
            this.PromedioAnt = (double) (pad.PromedioAnt??0m);
            this.PromedioAct = (double) (pad.PromedioAct??0m);
            this.MesAdeudoAnt = (int) (pad.MesAdeudoAnt??0m);
            this.MesAdeudoAct = (int) (pad.MesAdeudoAct??0m);
            // this.Id_Servicio = (int) (pad.IdServicio??0m);
            this.Servicio = pad.Servicio;
            // this.Id_Tarifa = (int) (pad.IdTarifa??0m);
            this.Tipo_Usuario = pad.Tipousuario;
            // this.Id_TarifaFija = (int) (pad.IdTarifafija??0m);
            this.Tarifa_Fija = pad.Tarifafija;
            this.ConsumoFijo = (int) (pad.ConsumoFijo??0m);
            this.ImporteFijo = pad.ImporteFijo??0m;
            this.ImporteFijoDren = pad.ImporteFijoDren??0m;
            this.ImporteFijoSane = pad.ImporteFijoSane??0m;
            // this.Id_Situacion = (int) (pad.IdSituacion??0m);
            this.Situacion = pad.Situacion;
            // this.Id_AnomaliaAct = (int) (pad.IdAnomaliaAct??0m);
            this.AnomaliaAct = pad.AnomaliaAct;
            this.LecturaAnt = (int) (pad.LecturaAnt??0m);
            this.LecturaAct = (int) (pad.LecturaAct??0m);
            this.ConsumoAnt = (int) (pad.ConsumoAnt??0m);
            this.ConsumoAct = (int) (pad.ConsumoAct??0m);
            // this.Consumo_RealAnt = (int) (pad.ConsumoRealAnt??0m);
            // this.Consumo_RealAct = (int) (pad.ConsumoRealAct??0m);
            // this.Id_Tipocalculo = (int) (pad.IdTipocalculo??0m);
            this.Calculo = pad.Calculo;
            // this.Calculo_Act = pad.CalculoAct;
            this.Subtotal = pad.Subtotal??0m;
            this.Iva = pad.Iva??0m;
            this.Total = pad.Total??0m;
            // this.EsMacromedidor = pad.EsMacromedidor==true;
            // this.EsDraef = pad.EsDraef==true;
            // this.EsAltoconsumidor = pad.EsAltoconsumidor==true;
            // this.Id_Medidor = pad.IdMedidor;
            this.Diametro = pad.Diametro;
            this.Tipotoma = pad.Tipotoma;
            this.MesFacturado = pad.MesFacturado;
            var tmpDate = new DateTime();
            // this.FechaAlta = DateTime.TryParse(pad.FechaAlta, out tmpDate)?tmpDate:null;
            this.AltaFactura =  DateTime.TryParse(pad.AltaFactura, out tmpDate)?tmpDate:null;
            this.FechaLecturaAct = DateTime.TryParse(pad.FechaLecturaAct, out tmpDate)?tmpDate:null;
            this.FechaFacturaAct = DateTime.TryParse(pad.FechaFacturaAct, out tmpDate)?tmpDate:null;
            this.FechaVencimiento = DateTime.TryParse(pad.FechaVencimiento, out tmpDate)?tmpDate:null;
            this.Latitude = pad.Latitud??0m;
            this.Longitude = pad.Longitud??0m;
        }

    }
}