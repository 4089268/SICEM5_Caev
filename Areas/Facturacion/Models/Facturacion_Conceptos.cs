namespace Sicem_Blazor.Facturacion.Models
{
    public class Facturacion_Conceptos {

        public int Id_Oficina { get; set; }
        public string Oficina { get; set; }

        public int Es_Rezago { get; set; }
        public int Id_Concepto { get; set; }
        public string Concepto { get; set; }
        
        public decimal Domestico_Sub{ get; set; }
        public decimal Domestico_Iva{ get; set; }
        public decimal Domestico_Total{ get; set; }

        public decimal Hotelero_Sub{ get; set; }
        public decimal Hotelero_Iva{ get; set; }
        public decimal Hotelero_Total{ get; set; }

        public decimal Comercial_Sub{ get; set; }
        public decimal Comercial_Iva{ get; set; }
        public decimal Comercial_Total{ get; set; }

        public decimal Industrial_Sub{ get; set; }
        public decimal Industrial_Iva{ get; set; }
        public decimal Industrial_Total{ get; set; }

        public decimal ServGen_Sub{ get; set; }
        public decimal ServGen_Iva{ get; set; }
        public decimal ServGen_Total{ get; set; }

        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }

        public Facturacion_Conceptos(){
            Id_Oficina = 0;
            Oficina = "";
            Es_Rezago = 0;
            Id_Concepto = 0;
            Concepto = "";
            Domestico_Sub = 0;
            Domestico_Iva = 0;
            Domestico_Total = 0;
            Hotelero_Sub = 0;
            Hotelero_Iva = 0;
            Hotelero_Total = 0;
            Comercial_Sub = 0;
            Comercial_Iva = 0;
            Comercial_Total = 0;
            Industrial_Sub = 0;
            Industrial_Iva = 0;
            Industrial_Total = 0;
            ServGen_Sub = 0;
            ServGen_Iva = 0;
            ServGen_Total = 0;
            Subtotal = 0;
            Iva = 0;
            Total = 0;
        }
    }
}