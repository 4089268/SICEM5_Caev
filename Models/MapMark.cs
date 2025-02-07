using System;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.Models
{
    public class MapMark : IMapPoint
    {
        public double Latitude {get;set;} = 19.507345;
        public double Longitude {get;set;} = -96.644080;
        public string Descripcion {get;set;} = "";
        public string Subtitulo {get;set;} = "";
        public double Zoom {get;set;} = 8;

        public int IdOficina = 0;

        public CatPadron Padron {get;set;}
    }
}