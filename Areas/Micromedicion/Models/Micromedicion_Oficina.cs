using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.Micromedicion.Models {
    public class Micromedicion_Oficina: IResumOficina{
        public ResumenOficinaEstatus Estatus { get; set; }

        public int Id {get => Enlace==null?999:Enlace.Id; }
        public string Oficina {get => Enlace==null?"TOTAL":Enlace.Nombre; }
        public IEnlace Enlace {get;set;}    
        
        public int Reales { get; set; }
        public double Reales_Porc { get; set; }
        public int Promedios { get; set; }
        public double Promedios_Porc { get; set; }
        public int Medidos { get; set; }
        public double Medidos_Porc { get; set; }
        public int Fijos { get; set; }
        public double Fijos_Porc { get; set; }
        public int Total { get; set; }
        
        public Micromedicion_Oficina() {
            Estatus = 0;
            Reales = 0;
            Reales_Porc = 0;
            Promedios = 0;
            Promedios_Porc = 0;
            Medidos = 0;
            Medidos_Porc = 0;
            Fijos = 0;
            Fijos_Porc = 0;
            Total = 0;
        }

    }
}
