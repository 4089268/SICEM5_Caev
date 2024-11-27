namespace Sicem_Blazor.Models{
    public class DatosUsuario {
        public int Id_Padron {get;set;}
        public int Id_Cuenta {get;set;}
        public string Razon_Social {get; set;}
        public string RFC {get; set;}
        public string Direccion {get; set;}
        public string Colonia {get; set;}
        public string Ciudad {get; set;}
        public string Estado {get; set;}
        public string Telefono1 {get; set;}
        public string Giro {get;set;}
        public string Estatus {get;set;}
        public string Tarifa {get;set;}
        public string Servicios {get;set;}
        public string Medidor {get;set;}
        public int Meses_Adeudo {get;set;}

        public DatosUsuario(){
            Id_Padron = 0;
            Id_Cuenta = 0;
            Razon_Social ="";
            RFC ="";
            Direccion ="";
            Colonia ="";
            Ciudad ="";
            Estado ="";
            Telefono1 ="";
            Giro="";
            Estatus ="";
            Tarifa ="";
            Servicios ="";
            Medidor ="";
            Meses_Adeudo = 0;
        }

    }
}