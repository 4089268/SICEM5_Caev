using System;
namespace Sicem_Blazor.Data.Contracts {
    public interface IResumOficina{
        public ResumenOficinaEstatus Estatus {get; set;}
        public int Id {get;}
        public string Oficina {get;}

    }
    public enum ResumenOficinaEstatus {
        Pendiente = 0,
        Completado = 1,
        Error = 2
    }
}