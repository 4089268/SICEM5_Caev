using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Boletines.Models;

public interface IBoletinMensaje
{
    public Guid Id {get;set;}
    public Guid BoletinId {get;set;}
    public bool EsArchivo {get;set;}
    public string Mensaje {get;set;}
    public string MimmeType {get;set;}
    public int FileSize {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime DeletedAt {get;set;}
}