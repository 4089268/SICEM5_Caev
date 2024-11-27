namespace Sicem_Blazor.Padron.Models {
    public class Padron_DetallePadron {
        
        public long Cuenta {get;set;}
        public string Estatus {get;set;}
        public string Localizacion {get;set;}
        public string Nombre {get;set;}
        public string Direccion {get;set;}
        public string Tarifa {get;set;}
        public string Colonia {get;set;}
        public double Lec_Ant {get;set;}
        public double Lec_Act {get;set;}
        public double Consumo {get;set;}
        public string Calculo {get;set;}
        public double Promedio {get;set;}
        public string Medidor {get;set;}
        public decimal Agua {get;set;}
        public decimal Dren {get;set;}
        public decimal Sane {get;set;}
        public decimal Act {get;set;}
        public decimal Otros {get;set;}
        public decimal Ragua {get;set;}
        public decimal Rdren {get;set;}
        public decimal Rtrat {get;set;}
        public decimal Subtotal {get;set;}
        public decimal Iva {get;set;}
        public decimal Total {get;set;}

         public string Sector {
            get {
                if(Localizacion.Length > 0){
                    var arrs = Localizacion.Split("-");
                    return $"{arrs[0]} - {arrs[1]}";
                }else{
                    return "";
                }
            }
        }
    }
}