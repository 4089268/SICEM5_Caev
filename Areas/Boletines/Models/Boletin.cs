using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Boletines.Models;

public class Boletin
{
    public Guid UUID {get;set;}
    public string Titulo {get;set;}
    public BoletinMensaje Mensaje {get;set;}
    public ICollection<Destinatario> Destinatarios {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime FinishedAt {get;set;}

}