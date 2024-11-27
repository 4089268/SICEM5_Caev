using System;

namespace Sicem_Blazor.ModsUbitoma.Models {
    public class HistActualizacion {
        public int IdOficina { get{ return ValorNuevo.IdOficina; } }
        public string Oficina { get{ return ValorNuevo.Oficina; } }
        public int IdCuenta { get{ return ValorNuevo.IdCuenta; } }
        public int IdPadron { get{ return ValorNuevo.IdPadron; } }

        public string Fecha {
            get {
                if(ValorNuevo != null){
                    if(ValorNuevo.Fecha != null){
                        return ValorNuevo.Fecha?.ToString("dd MMMM yyyy HH:mm").ToUpper();
                    }
                }
                return "";
            }
        }

        public bool Geolocalizado {
            get {
                var lat = double.TryParse(ValorNuevo.Latitud, out double tmpLa)?tmpLa:0;
                var lon = double.TryParse(ValorNuevo.Longitud, out double tmpLo)?tmpLo:0;
                return lat != 0 && lon != 0;
            }
        }
        public bool ModEstatus {
            get {
                return ValorNuevo.IdEstatus != ValorAnterior.IdEstatus;
            }
        }
        public bool ModSituacion {
            get {
                return ValorNuevo.IdSituacion != ValorAnterior.IdSituacion;
            }
        }
        public bool ModGiro {
            get {
                return ValorNuevo.IdGiro != ValorAnterior.IdGiro;
            }
        }
        public bool ModTarifa {
            get {return ValorNuevo.IdTarifa != ValorAnterior.IdTarifa;
            }
        }
        public bool ModAnomaliaToma {
            get {return ValorNuevo.IdAnomaliaPredio != ValorAnterior.IdAnomaliaPredio; }
        }

        public ActualizacionItem ValorNuevo {get; set;}
        public ActualizacionItem ValorAnterior {get; set;}


        public string LocalizacionAnt {
            get => ValorAnterior.Latitud + " " + ValorAnterior.Longitud;
        }
        public string LocalizacionAct {
            get => ValorNuevo.Latitud + " " + ValorNuevo.Longitud;
        }

        public string AnomaliaAnt {
            get => ValorAnterior.AnomaliaPredio;
        }
        public string AnomaliaAct {
            get => ValorNuevo.AnomaliaPredio;
        }
        public string GiroAnt {
            get => ValorAnterior.Giro;
        }
        public string GiroAct {
            get => ValorNuevo.Giro;
        }
        public string TarifaAnt {
            get => ValorAnterior.Tarifa;
        }
        public string TarifaAct {
            get => ValorNuevo.Tarifa;
        }
        public string Observaciones {
            get => ValorNuevo.Observaciones;
        }

    }
    
    public class ActualizacionItem {
        public int Id { get; set; }
        public int IdOficina { get; set; }
        public string Oficina {get;set;}
        public int IdPadron { get; set; }
        public int IdCuenta { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Observaciones { get; set; }
        public int IdEstatus { get; set; }
        public string Estatus { get; set; }
        public int IdSituacion { get; set; }
        public string Situacion { get; set; }
        public int IdTarifa { get; set; }
        public string Tarifa { get; set; }
        public int IdGiro { get; set; }
        public string Giro { get; set; }
        public int IdAnomaliaPredio { get; set; }
        public string AnomaliaPredio { get; set; }
        public DateTime? Fecha { get; set; }
        
        public ActualizacionItem(){
            Id = 0;
            IdOficina = 0;
            Oficina = "";
            IdPadron = 0;
            IdCuenta = 0;
            Latitud = "";
            Longitud = "";
            Observaciones = "";
            IdEstatus = 0;
            Estatus = "";
            IdSituacion = 0;
            Situacion = "";
            IdTarifa = 0;
            Tarifa = "";
            IdGiro = 0;
            Giro = "";
            IdAnomaliaPredio = 0;
            AnomaliaPredio = "";
        }
    }
}