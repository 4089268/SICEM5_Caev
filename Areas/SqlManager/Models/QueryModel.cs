using System;
using System.Collections;
using System.Collections.Generic;
using Sicem_Blazor.Data;
using static Sicem_Blazor.SqlManager.Models.QueryResult;

namespace Sicem_Blazor.SqlManager.Models;

public class QueryModel
{
    public Guid Id {get;set;}
    public string Name {get;set;}
    public string Query {get;set;}
    public List<IEnlace> Enlaces {get;set;}
    public List<QueryResult> Results {get;set;}
    public QueryResultStatus Status {get;set;}

    public QueryModel(Guid id)
    {
        this.Id = id;
        Status = QueryResultStatus.Pending;
        Results = new();
        Enlaces = new();
    }

    public QueryModel()
    {
        this.Id = Guid.NewGuid();
        Status = QueryResultStatus.Pending;
        Results = new();
        Enlaces = new();
    }

}