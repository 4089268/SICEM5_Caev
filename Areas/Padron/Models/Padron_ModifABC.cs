using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Padron.Models{
    
    public class Padron_ModifABC_Response {
        public Padron_ModifABC[] Modificaciones {get;set;}
        public Padron_ModifABC_Campo[] CamposModificados {get;set;}
    }

    public class Padron_ModifABC {
        public long Id_Abc {get;set;}
        public DateTime Fecha {get;set;}
        public long Id_Padron {get;set;}
        public long Id_Cuenta {get;set;}
        public string Razon_Social {get;set;}
        public string Direccion {get;set;}
        public string Observacion {get;set;}
        public string Maquina {get;set;}
        public string Operador {get;set;}
        public string Id_Operador {get;set;}
        public string Sucursal {get;set;}
        public int Id_Sucursal {get;set;}
        public string Localizacion {get;set;}
        public string Colonia {get;set;}
        public int Id_Colonia {get;set;}
        public string Departamento {get;set;}
        public int Id_Departamento {get;set;}
    }

    public class Padron_ModifABC_Campo {
        public long Id_Abc {get;set;}
        public string Campo {get;set;}
        public string valor_ant {get;set;}
        public string Valor_act {get;set;}
    }
}