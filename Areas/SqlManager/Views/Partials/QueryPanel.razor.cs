using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatBlazor;
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

    [Inject]
    public IMatToaster MatToaster {get;set;}

    [Parameter]
    public QueryModel Query {get;set;}

    [Parameter]
    public IEnumerable<IEnlace> Enlaces {get;set;}


    public async Task RunOnClick()
    {
        // * check if the query is not null
        if(string.IsNullOrEmpty(Query.Query))
        {
            MatToaster.Add("La consulta no puede estar vacía.", MatToastType.Warning);
            return;
        }

        // * check if any office is selected
        if(!Query.Enlaces.Any())
        {
            MatToaster.Add("Seleccione minimo una oficina.", MatToastType.Warning);
            return;
        }

        MatToaster.Add("Procesando consulta.", MatToastType.Success);
        await Task.CompletedTask;
    }

    public void CheckboxChanged(ChangeEventArgs e, IEnlace enlace)
    {
        var selected = (bool)e.Value;
        if(selected)
        {
            if(!Query.Enlaces.Contains(enlace))
            {
                Query.Enlaces.Add(enlace);
            }
        }
        else
        {
            Query.Enlaces = Query.Enlaces.Where(x => x.Id != enlace.Id).ToList();
        }
        // Console.WriteLine($"{enlace.Nombre} {selected}");
    }

}
