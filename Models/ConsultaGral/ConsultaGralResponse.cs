using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Models{
    public class ConsultaGralResponse {

        // *** Datos Generales ***
        public int Id_Padron { get; set; }
        public int Id_Cuenta { get; set; }
        public string Nom_comercial { get; set; }
        public string Razon_social { get; set; }
        public string Localizacion { get; set; }
        public string Direccion { get; set; }
        public string Colonia { get; set; }
        public string Codigo_postal { get; set; }
        public string Poblacion { get; set; }
        public string Estatus { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public bool EnviarEstadoCuenta { get; set; }

        // *** Datos Facturacion ***
        public string Servicio { get; set; }
        public string Tarifa { get; set; }
        public string TarifaFija{ get; set; }
        public double ImporteFijoAgua{ get; set; }
        public double ImporteFijoDren { get; set; }
        public double ImporteFijoSane { get; set; }
        public string ConsumoFijo { get; set; }
        public string Calculo { get; set; }
        public string CalculoAct { get; set; }
        public string LPSPagados { get; set; }
        public int Viviendas { get; set; }
        public string Descto60{ get; set; }
        public string NivelSocioeconomico { get; set; }
        public string Medidor { get; set; }
        public string Ciclo { get; set; }
        public string FechaLectura_Ant { get; set; }
        public string FechaLectura_Act { get; set; }
        public string FechaVencimiento { get; set; }
        public string FechaFactura_Act { get; set; }
        public string MesesAdeudo { get; set; }
        public string ConsumoForzado { get; set; }
        public string Lectura_Act { get; set; }
        public string Lectura_Ant { get; set; }
        public string ConsumoReal { get; set; }
        public string Consumo_Act { get; set; }
        public string Anomalia_Act { get; set; }
        public string Promedio_Act { get; set; }
        public bool EsDraef { get; set; }
        public string ProximaTomaLectura { get; set; }

        // *** Clasificacion Usuario ***
        public string Giro { get; set; }
        public string ClaseUsuario { get; set; }
        public string Grupo { get; set; }
        public string Zona { get; set; }
        public string Hidrocircuito { get; set; }
        public string TipoFactible { get; set; }
        public int Mf { get; set; }
        public int Af { get; set; }
        public string Situacion_Toma { get; set; }
        public string DiametroToma { get; set; }
        public bool AltoConsumidor { get; set; }
        public bool EsMacromedidor { get; set; }
        public bool TienePozo { get; set; }


        public double SaldoFavor_importe { get; set; }=0;
        public double SaldoFavor_meses_x_aplicar { get; set; }=0;
        public double SaldoFavor_m3_x_aplicar { get; set; }=0;

        public double Documentos_imp_atiempo { get; set; }=0;
        public double Documentos_imp_vencido { get; set; }=0;
        public double Documentos_imp_total { get; set; }=0;

        public double SaldoAct_subtotal { get; set; } =0;
        public double SaldoAct_iva { get; set; }=0;
        public double SaldoAct_total{ get; set; }=0;

        public int TotalImagenes { get; set; }

        public string Latitud { get; set; }
        public string Longitud { get; set; }

        public ConsultaGralResponse_saldoItem[] SaldoActual { get; set; }

        public ConsumoItem[] HistorialConsumos { get; set; }
        
        public bool TieneUbicacion {
            get {
                decimal tmpDec = 0m;
                var lat = decimal.TryParse(this.Latitud, out tmpDec)?tmpDec:0m;
                var lon = decimal.TryParse(this.Latitud, out tmpDec)?tmpDec:0m;
                return (lat != 0 && lon != 0);
            }
        }

    }
    public class ConsultaGralResponse_saldoItem {
        public string Concepto { get; set; }
        public double Subtotal { get; set; }
        public double Iva { get; set; }
        public double Total { get; set; }

    }

}
