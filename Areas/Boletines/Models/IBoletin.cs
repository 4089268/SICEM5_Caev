using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Boletines.Models;

public interface IBoletin
{
    public Guid Id {get;set;}
    public string Titulo {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime FinishedAt {get;set;}
}