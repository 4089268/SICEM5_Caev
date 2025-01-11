using System;
using System.Collections.Generic;
using System.Linq;

namespace Sicem_Blazor.AnalisisInformacion.Models {
    public class AnalisysInfoFilter {
        public List<int> Id_Oficinas {get;set;} = new List<int>();

        public List<int> Cuentas {get;set;} = new List<int>();


        //**** Datos Generales
        public string RazonSocial {get;set;} = "";
        public string NombreComercial {get;set;} = "";
        public string Direccion {get;set;} = "";
        public string Colonia {get;set;} = "";
        public string Localidad {get;set;} = "";
        public List<int> Id_Estatus {get;set;} = new List<int>();

        public List<int> Id_Servicios {get;set;} = new List<int>();
        public List<int> Id_Tarifas {get;set;} = new List<int>();

        public int ImporteTarifaAgua_Opcion {get;set;} = 0;
        public decimal ImporteTarifaAgua_Valor1 {get;set;} = 0m;
        public decimal ImporteTarifaAgua_Valor2 {get;set;} = 0m;

        public int ImporteTarifaDren_Opcion {get;set;} = 0;
        public decimal ImporteTarifaDren_Valor1 {get;set;} = 0m;
        public decimal ImporteTarifaDren_Valor2 {get;set;} = 0m;

        public int ImporteTarifaSane_Opcion {get;set;} = 0;
        public decimal ImporteTarifaSane_Valor1 {get;set;} = 0m;
        public decimal ImporteTarifaSane_Valor2 {get;set;} = 0m;

        public int Consumo_Opcion {get;set;} = 0;
        public decimal Consumo_Valor1 {get;set;} = 0m;
        public decimal Consumo_Valor2 {get;set;} = 0m;

        public List<int> Id_TipoCalculo {get;set;} = new List<int>();

        public int LPS_Opcion {get;set;} = 0;
        public decimal LPS_Valor1 {get;set;} = 0m;
        public decimal LPS_Valor2 {get;set;} = 0m;

        public List<int> Id_NivelSocioEco {get;set;} = new List<int>();

        public int FechaLect_Opcion {get;set;} = 0;
        public DateTime FechaLect_Valor1 {get;set;}
        public DateTime FechaLect_Valor2 {get;set;}

        public int FechaVenci_Opcion {get;set;} = 0;
        public DateTime FechaVenci_Valor1 {get;set;}
        public DateTime FechaVenci_Valor2 {get;set;}

        public int MesesAdeudo_Opcion {get;set;} = 0;
        public decimal MesesAdeudo_Valor1 {get;set;} = 0m;
        public decimal MesesAdeudo_Valor2 {get;set;} = 0m;

        public int LectAct_Opcion {get;set;} = 0;
        public decimal LectAct_Valor1 {get;set;} = 0m;
        public decimal LectAct_Valor2 {get;set;} = 0m;

        public int LectAnt_Opcion {get;set;} = 0;
        public decimal LectAnt_Valor1 {get;set;} = 0m;
        public decimal LectAnt_Valor2 {get;set;} = 0m;

        public List<int> Id_Anomalias {get;set;} = new List<int>();


        public int Promedio_Opcion {get;set;} = 0;
        public decimal Promedio_Valor1 {get;set;} = 0m;
        public decimal Promedio_Valor2 {get;set;} = 0m;

        public List<int> Id_Giro {get;set;} = new List<int>();
        public List<int> Id_ClaseUsuario {get;set;} = new List<int>();


        public int AltoConsumidor_Opcion { get; set; } = 0;
        public int EsDraef_Opcion { get; set; } = 0;
        public int TienePozo_Opcion { get; set; } = 0;

        public int Saldo_Opcion { get; set; } = 0;
        public decimal Saldo_Valor1 { get; set; } = 0m;
        public decimal Saldo_Valor2 { get; set; } = 0m;
        
        public int TieneUbicacion { get; set; } = 0;

        public int TelefonoRegistrado { get; set; } = 0;

        public int Subsistema_Opcion {get;set;} = 0;
        public decimal Subsistema_Valor1 {get;set;} = 0m;
        public decimal Subsistema_Valor2 {get;set;} = 0m;

        public int Sector_Opcion {get;set;} = 0;
        public decimal Sector_Valor1 {get;set;} = 0m;
        public decimal Sector_Valor2 {get;set;} = 0m;


        public override string ToString(){
            var _result = new System.Text.StringBuilder();
            _result.Append($"Razon Social: {this.RazonSocial}\n");
            _result.Append($"NombreComercial: {this.NombreComercial}\n");
            _result.Append($"Direccion: {this.Direccion}\n");
            _result.Append($"Colonia: {this.Colonia}\n");
            _result.Append($"Id Localidad: {this.Localidad}\n");

            _result.Append($"Id_Estatus: ");            
            this.Id_Estatus.ForEach(item => _result.Append($"{item} "));
            _result.Append("\n");

            _result.Append($"Id Servicios: ");
            this.Id_Servicios.ForEach(item => _result.Append($"{item} "));
            _result.Append("\n");

            _result.Append($"Id Tarifas: ");
            this.Id_Tarifas.ForEach(item => _result.Append($"{item} "));
            _result.Append("\n");

            _result.Append($"Id TipoCalculo: ");
            this.Id_TipoCalculo.ForEach(item => _result.Append($"{item} "));
            _result.Append("\n");

            _result.Append($"Id_NivelSocioEco: ");
            this.Id_NivelSocioEco.ForEach(item => _result.Append($"{item} "));
            _result.Append("\n");

            _result.Append($"Id_Anomalias: ");
            this.Id_Anomalias.ForEach(item => _result.Append($"{item} "));
            _result.Append("\n");

            _result.Append($"Id_Giro: ");
            this.Id_Giro.ForEach(item => _result.Append($"{item} "));
            _result.Append("\n");

            _result.Append($"Id_ClaseUsuario: ");
            this.Id_ClaseUsuario.ForEach(item => _result.Append($"{item} "));
            _result.Append("\n");

            _result.Append($"Consumo : {this.Consumo_Opcion} {this.Consumo_Valor1} {this.Consumo_Valor2} \n");
            _result.Append($"Lectura Act : {this.LectAct_Opcion} {this.LectAct_Valor1} {this.LectAct_Valor2} \n");

            return _result.ToString();
        }

    }

}