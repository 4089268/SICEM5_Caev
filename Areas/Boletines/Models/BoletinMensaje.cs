using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Boletines.Models;

public class BoletinMensaje
{
    public string Message {get;set;}
    public ICollection<BoletinMensajeFile> Archivos {get;set;}
}