using Sicem_Blazor.Data;

namespace Sicem_Blazor.ModsUbitoma.Models {
    public class ResumenOficina
    {
        public IEnlace Enlace {get;}
        public int IdEnlace { get => Enlace.Id; }
        public string Oficina { get => Enlace.Nombre;}

        public int TotalModificaciones { get; set; }
        public int Geolocalizados { get; set; }
        public int ModificacionTarifa { get; set; }
        public int ModificacionSituacionToma { get; set; }
        public int ModificacionGiro { get; set; }
        public int ModificacionEstatus { get; set; }
        public int ModificacionSituacionPredio { get; set; }
        public int Observaciones { get; set; }
        public int TotalImagenes { get; set; }
        

        public ResumenOficina(IEnlace e){
            this.Enlace = e;
        }

        public void SetValuesFromResponse(ResumenOficina_Response values){
            this.TotalModificaciones = values.totalModificaciones;
            this.Geolocalizados = values.geolocalizados;
            this.ModificacionTarifa = values.modificacionTarifa;
            this.ModificacionSituacionToma = values.modificacionSituacionToma;
            this.ModificacionGiro = values.modificacionGiro;
            this.ModificacionEstatus = values.modificacionEstatus;
            this.ModificacionSituacionPredio =values.modificacionSituacionPredio;
            this.Observaciones = values.observaciones;
            this.TotalImagenes = values.totalImagenes;
        }
        
    }

    public class ResumenOficina_Response
    {
        public int idOficina { get; set; }
        public string oficina { get; set; }
        public int totalModificaciones { get; set; }
        public int geolocalizados { get; set; }
        public int modificacionTarifa { get; set; }
        public int modificacionSituacionToma { get; set; }
        public int modificacionGiro { get; set; }
        public int modificacionEstatus { get; set; }
        public int modificacionSituacionPredio { get; set; }
        public int observaciones { get; set; }
        public int totalImagenes {get;set;}
    }
}