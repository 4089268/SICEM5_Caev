using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Boletines.Models;

public class BoletinMensajeFile
{
    public Guid UUID {get;set;}
    public string Titulo {get;set;}
    public string Ruta {get;set;}
    public string MimeType {get;set;}
    public bool Size {get;set;}
    public DateTime CreatedAt {get;set;}

}