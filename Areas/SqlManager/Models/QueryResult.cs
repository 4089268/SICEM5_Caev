using System;
using System.Data;
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
    public DataSet Result {get;set;}
    public QueryResultStatus Status {get;set;}

    public static QueryResult NewPending(IEnlace enlace)
    {
        var queryResult = new QueryResult();
        queryResult.Enlace = enlace;
        queryResult.Status = QueryResultStatus.Pending;
        return queryResult;
    }
}
