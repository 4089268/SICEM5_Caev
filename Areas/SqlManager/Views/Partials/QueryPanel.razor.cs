using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Sicem_Blazor.Data;
using Sicem_Blazor.Services;
using Sicem_Blazor.SqlManager.Models;

namespace Sicem_Blazor.SqlManager.Views.Partials;

public partial class QueryPanel
{

    [Inject]
    public IJSRuntime JSRuntime {get;set;}

    [Parameter]
    public QueryModel Query {get;set;}

    [Parameter]
    public IEnumerable<IEnlace> Enlaces {get;set;}


    public async Task RunOnClick()
    {
        await Task.CompletedTask;
    }

}
