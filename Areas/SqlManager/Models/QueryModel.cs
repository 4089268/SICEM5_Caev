using System;
using System.Collections;
using System.Collections.Generic;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.SqlManager.Models;

public class QueryModel
{
    public Guid Id {get;set;}
    public string Name {get;set;}
    public string Query {get;set;}
    public IEnumerable<IEnlace> Enlaces {get;set;}
    public IEnumerable<QueryResult> Results {get;set;}
    public string Status {get;set;}

}