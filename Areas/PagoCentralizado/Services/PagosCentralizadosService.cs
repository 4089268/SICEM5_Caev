using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.PagoCentralizado.Services;

public class PagosCentralizadosService(ILogger<PagosCentralizadosService> lg, SicemContext c)
{
    private readonly ILogger<PagosCentralizadosService> logger = lg;
    private readonly SicemContext context = c;

    public IEnumerable<PagoCentCatGrupo> ObtenerGrupos()
    {
        return this.context.PagoCentCatGrupos
            .Where(e => e.FechaEliminacion == null)
            .ToList();
    }

    public PagoCentCatGrupo AgregarGrupo(string nombre)
    {
        PagoCentCatGrupo nuevoGrupo = new()
        {
            Descripcion = nombre
        };
        this.context.PagoCentCatGrupos.Add(nuevoGrupo);
        this.context.SaveChanges();
        this.logger.LogInformation("Pagos Centralizados nuevo grupo agregado id:{id} Nombre:{nombre}", nuevoGrupo.Id, nuevoGrupo.Descripcion);
        return nuevoGrupo;
    }

    public void EliminarGrupo(int grupoId)
    {
        var grupo = this.context.PagoCentCatGrupos
            .Where(e => e.FechaEliminacion == null)
            .FirstOrDefault();

        if(grupo == null)
        {
            this.logger.LogWarning("Se intento eliminar un grupo inexistente GrupoId:{id}", grupoId);
            return;
        }

        grupo.FechaEliminacion = DateTime.Now;
        this.context.PagoCentCatGrupos.Update(grupo);
        this.context.SaveChanges();
        this.logger.LogWarning("Grupo Id:{id} eliminado", grupoId);

        // TODO: Optional set his OprGrupo relations as deleted as well
    }

    public void ActualizarGrupo(PagoCentCatGrupo grupoArg)
    {
        var grupo = this.context.PagoCentCatGrupos
            .Where(e => e.FechaEliminacion == null)
            .Where(e => e.Id == grupoArg.Id)
            .FirstOrDefault();

        if(grupo == null)
        {
            this.logger.LogWarning("Se intento actualizar un grupo inexistente GrupoId:{id}", grupoArg.Id);
            return;
        }

        grupo.Descripcion = grupoArg.Descripcion;
        this.context.PagoCentCatGrupos.Update(grupo);
        this.context.SaveChanges();
        this.logger.LogWarning("Grupo Id:{id} actualiado", grupo.Id);
    }

}
