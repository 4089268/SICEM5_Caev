using System;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.Eficiencia.Models {

    public class EficienciaResumen : IResumOficina {
    
        public IEnlace Enlace {get;set;}
        public string Mes {get;set;}
        public decimal Facturado {get;set;}
        public decimal Refacturado {get; set;}
        public decimal Anticipado {get; set;}
        public decimal Descontado {get; set;}
        public decimal Cobrado {get; set;}
        public double Porcentaje {get; set;}

        public ResumenOficinaEstatus Estatus { get; set; }
        public int Id {get => Enlace==null?999:Enlace.Id; }
        public string Oficina {get => Enlace==null?"TOTAL":Enlace.Nombre; }
    }

}