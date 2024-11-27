namespace Sicem_Blazor.Tarifas.Data {
    public interface ITarifa {
        public int Id_Tarifa {get;}
        public string TipoUsuario {get;}
        public int Id_TipoUsuario {get;}
        public int Desde {get; set;}
        public int Hasta {get; set;}
        public decimal Costo {get; set;}
        public decimal CostoBase {get; set;}
        public decimal CuotaBase {get; set;}
        public decimal AdicionalM3 {get; set;}
    }
}