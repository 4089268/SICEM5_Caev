using System;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.Models
{
    public class MapMark : IMapPoint
    {
        public double Latitude {get;set;} = 20.213382;
        public double Longitude {get;set;} = -87.436819;
        public string Descripcion {get;set;} = "";
        public string Subtitulo {get;set;} = "";
        public double Zoom {get;set;} = 5;

        public int IdOficina = 0;

        public CatPadron Padron {get;set;}
    }
}