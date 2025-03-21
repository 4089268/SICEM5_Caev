using System;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.SqlManager.Models;

public class QueryResult
{
    public enum QueryResultStatus
    {
        Success,
        Failure,
        Pending
    }
    public IEnlace Enlace {get;set;}
    public int Id {get => Enlace.Id;}
    public string Name {get => Enlace.Nombre;}
    public dynamic Result {get;set;}
    public QueryResultStatus Status {get;set;}
}
