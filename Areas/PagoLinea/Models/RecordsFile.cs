using System;
using System.Data;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.PagoLinea.Models;

public class RecordsFile
{
    public string Name {get; set;}
    public DateTime From {get;set;}
    public DateTime To {get;set;}
    public int TotalRecords {get;set;}
}
