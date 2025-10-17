using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Models;
using Sicem_Blazor.PagoCentralizado.Services;

namespace Sicem_Blazor.PagoCentralizado.Views;

public partial class CatalogoPage : ComponentBase
{
    [Inject]
    public ILogger<CatalogoPage> Logger { get; set; }

    [Inject]
    public PagosCentralizadosService PagosCentralizadosService1 { get; set; }

    [Inject]
    public IMatToaster Toaster { get; set; }

    [Inject]
    public IMatDialogService DialogService { get; set; }

    private List<PagoCentCatGrupo> ListaGrupos { get; set; } = new();

    private PagoCentCatGrupo elementoSeleccionado = null;

    public bool IsBusy { get; set; } = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            await ObtenerGrupos();
        }
    }

    public async Task ObtenerGrupos()
    {
        IsBusy = true;
        await Task.Delay(100);

        try
        {
            ListaGrupos = this.PagosCentralizadosService1.ObtenerGrupos().ToList();
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Erro al obtener el catalogo de grupos");
            this.Toaster.Add("Erro al obtener el catalogo de grupos, comuníquese con el administrador.", MatToastType.Danger);
            ListaGrupos = new();
        }
        finally
        {
            IsBusy = false;
            StateHasChanged();
        }

    }

    public async Task AgregarGrupoClick()
    {
        var nombreGrupo = await DialogService.PromptAsync("Capture el nombre del nuevo grupo.");

        if(string.IsNullOrWhiteSpace(nombreGrupo))
        {
            return;
        }

        try
        {
            var grupo = this.PagosCentralizadosService1.AgregarGrupo(nombreGrupo);
            ListaGrupos.Add(grupo);
            StateHasChanged();
        }
        catch(System.Exception ex)
        {
            Logger.LogError(ex, "Erro al agregar el nuevo grupo");
            this.Toaster.Add("Error al agregar el nuevo grupo, comuníquese con el administrador.", MatToastType.Danger);
        }
    }

    public async Task ListaGrupoClick(PagoCentCatGrupo grupo)
    {
        this.elementoSeleccionado = grupo;
        await Task.CompletedTask;
        StateHasChanged();
    }
}
