namespace Sicem_Blazor.Tarifas.Models {
    public class SimulacionResumen {
        public string TipoUsuario {get;set;}
        public decimal Agua {get;set;}
        public decimal Drenaje {get;set;}
        public decimal Saneamiento {get;set;}
        public decimal Total {get;set;}

        public decimal AguaSim {get;set;}
        public decimal DrenajeSim {get;set;}
        public decimal SaneamientoSim {get;set;}
        public decimal TotalSimulado {get;set;}

        public int Usuarios {get;set;}
        public int M3Facturados {get;set;}
        public int M3Consumidos {get;set;}
        public double Porcentaje{get;set;}
        
    }
}