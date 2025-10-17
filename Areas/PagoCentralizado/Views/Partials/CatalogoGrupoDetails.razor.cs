using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Grids;
using MatBlazor;
using Sicem_Blazor.PagoCentralizado.Services;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.PagoCentralizado.Views;

public partial class CatalogoGrupoDetails
{
    [Inject]
    public PagosCentralizadosService PagosCentralizadosService1 { get; set; }

    [Inject]
    public IMatToaster MatToaster { get; set; }

    [Inject]
    public ILogger<CatalogoGrupoDetails> Logger { get; set; }

    [Parameter]
    public PagoCentCatGrupo Grupo {get; set;}

    SfGrid<PagoCentOprGrupo> Grid { get; set; }
    public List<PagoCentOprGrupo> Data
    {
        get
        {
            if(Grupo == null)
            {
                return [];
            }
            return Grupo.OprGrupos.ToList();
        }
        set {
            if(Grupo == null)
            {
                return;
            }

            if(Grupo.OprGrupos == null)
            {
                Grupo.OprGrupos = new List<PagoCentOprGrupo>();
            }

            Grupo.OprGrupos = value;
        }
    }
    public PagoCentOprGrupo cuentaSeleccionada = null;
    private bool ventanaSecundariaVisible = false;
    private string ventanaSecundariaTitle = "Nueva Cuenta";


    public async Task AgregarCuentaClick()
    {
        this.cuentaSeleccionada = null;
        this.ventanaSecundariaTitle = "Nueva Cuenta";
        this.ventanaSecundariaVisible = true;
        await Task.CompletedTask;
    }

    public async Task VentanSecundariaClosed(PagoCentOprGrupo grupoCuenta)
    {
        ventanaSecundariaVisible = false;

        if(grupoCuenta == null)
        {
            return;
        }

        try
        {
            var oprGrupo = this.PagosCentralizadosService1.AgregarGrupoCuenta(Grupo.Id, grupoCuenta);
            this.Grupo.OprGrupos.Add(oprGrupo);
            StateHasChanged();
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Error al guardar el nuevo elemento.");
            MatToaster.Add(ex.Message, MatToastType.Danger);
        }

    }

    public void RowSelectHandler(RowSelectEventArgs<PagoCentOprGrupo> args)
    {
        cuentaSeleccionada = args.Data;
    }
}
