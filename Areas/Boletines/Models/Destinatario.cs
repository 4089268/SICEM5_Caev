using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Boletines.Models;

public class Destinatario
{
    public Guid UUID {get;set;}
    public string Label {get;set;}
    public int Telefono {get;set;}
    public int Lada {get;set;}
    public bool Enviado {get;set;}
    public string? Mensaje {get;set;}
    public string EnvioMetada {get;set;} // "key1=value;key2=value;"
}