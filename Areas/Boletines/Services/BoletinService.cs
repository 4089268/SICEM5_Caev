using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Boletines.Models;
using Sicem_Blazor.Models;
using Sicem_Blazor.Services;

namespace Sicem_Blazor.Boletines.Services;

public class BoletinService : IBoletinService
{
    private readonly SicemContext sicemContext;
    private readonly SicemService sicemService;
    private readonly ILogger<BoletinService> logger;

    public BoletinService(SicemContext sicemContext, SicemService sicemService, ILogger<BoletinService> logger)
    {
        this.sicemContext = sicemContext;
        this.sicemService = sicemService;
        this.logger = logger;
    }

    public async Task ActualizarDestinatario(Guid destinatarioId, IBoletinDestinatario destinatario)
    {
        var boletinDest = this.sicemContext.Destinatarios.FirstOrDefault(item => item.Id == destinatarioId)
            ?? throw new KeyNotFoundException("BoletinDestinatario not found");
        
        boletinDest.Titulo = destinatario.Titulo;
        boletinDest.Telefono = destinatario.Telefono;
        boletinDest.Lada = destinatario.Lada;
        boletinDest.Error = destinatario.Error;
        boletinDest.Resultado = destinatario.Resultado;
        boletinDest.EnvioMetadata = destinatario.EnvioMetadata;
        boletinDest.FechaEnvio = destinatario.FechaEnvio;
        this.sicemContext.Destinatarios.Update(boletinDest);
        await this.sicemContext.SaveChangesAsync();
    }

    public async Task ActualizarMensaje(Guid mensajeId, IBoletinMensaje mensaje)
    {
        var boletinMensaje = this.sicemContext.BoletinMensajes.FirstOrDefault(item => item.Id == mensajeId)
            ?? throw new KeyNotFoundException("BoletinMesnaje not found");
        
        boletinMensaje.EsArchivo = mensaje.EsArchivo;
        boletinMensaje.FileSize = mensaje.FileSize;
        boletinMensaje.Mensaje = mensaje.Mensaje;
        boletinMensaje.MimmeType = mensaje.MimmeType;
        boletinMensaje.CreatedAt = mensaje.CreatedAt;
        this.sicemContext.BoletinMensajes.Update(boletinMensaje);
        await this.sicemContext.SaveChangesAsync();
    }

    public async Task<Guid> AlmacenarBoletin(BoletinDTO boletin)
    {
        var newBoletin = new OprBoletin
        {
            Id = boletin.Id,
            Titulo = boletin.Titulo,
            CreatedAt = boletin.CreatedAt,
            Proveedor = boletin.Proveedor,
            FinishedAt = null,
        };
        this.sicemContext.OprBoletins.Add(newBoletin);
        await this.sicemContext.SaveChangesAsync();
        return newBoletin.Id;
    }

    public async Task<ICollection<BoletinDTO>> GetBoletines()
    {
        await Task.CompletedTask;

        var boletines = this.sicemContext.OprBoletins.OrderBy(item => item.CreatedAt).ToList();

        var boletinesDTO = boletines.Select( b =>
        {
            var data = this.sicemContext.Destinatarios
                .Where(item => item.BoletinId == b.Id)
                .ToArray();

            var boletinDTO = BoletinDTO.FromEntity(b);

            boletinDTO.TotalDestinatarios = data.Count();
            boletinDTO.Exitos = data.Count(item => item.Error != true && item.FechaEnvio != null);
            boletinDTO.Fallidos = data.Count(item => item.Error == true && item.FechaEnvio != null);
            return boletinDTO;

        }).ToList();
        return boletinesDTO;
    }

    public async Task<ICollection<IBoletinDestinatario>> GetDestinatariosBoletin(Guid boletinId)
    {
        await Task.CompletedTask;
        return this.sicemContext.Destinatarios
            .Where( item => item.BoletinId == boletinId)
            .ToList<IBoletinDestinatario>();
    }

    public async Task<ICollection<IBoletinMensaje>> GetMensajesBoletin(Guid boletinId)
    {
        await Task.CompletedTask;
        return this.sicemContext.BoletinMensajes
            .Where( item => item.BoletinId == boletinId)
            .ToList<IBoletinMensaje>();
    }

    public async Task RemoverDestinatario(IBoletinDestinatario destinatario)
    {
        var dest = this.sicemContext.Destinatarios.FirstOrDefault( item => item.Id == destinatario.Id);
        if(dest != null)
        {
            this.sicemContext.Destinatarios.Remove(dest);
            await this.sicemContext.SaveChangesAsync();
        }
    }

    public async Task RemoverMensaje(IBoletinMensaje mensaje)
    {
        var mens = this.sicemContext.BoletinMensajes.FirstOrDefault( item => item.Id == mensaje.Id);
        if(mens != null)
        {
            this.sicemContext.BoletinMensajes.Remove(mens);
            await this.sicemContext.SaveChangesAsync();
        }
    }

    public async Task<Guid> StoreBoletinDestinatatio(Guid boletinId, IBoletinDestinatario destinatario)
    {
        var newDest = new Destinatario
        {
            Id = Guid.NewGuid(),
            BoletinId = boletinId,
            Titulo = destinatario.Titulo,
            Telefono = destinatario.Telefono,
            Lada = destinatario.Lada,
            Error = destinatario.Error,
            Resultado = destinatario.Resultado,
            EnvioMetadata = destinatario.EnvioMetadata,
            FechaEnvio = destinatario.FechaEnvio
        };
        this.sicemContext.Destinatarios.Add(newDest);
        await this.sicemContext.SaveChangesAsync();
        return newDest.Id;
    }
    public async Task<int> StoreBoletinDestinatatioRange(Guid boletinId, IEnumerable<IBoletinDestinatario> destinatarios)
    {
        var t = 0;
        var count = 0;
        this.sicemContext.ChangeTracker.AutoDetectChangesEnabled = false;
        foreach (var destinatario in destinatarios)
        {
            var newDestinatario = new Destinatario
            {
                Id = Guid.NewGuid(),
                BoletinId = boletinId,
                Titulo = destinatario.Titulo,
                Telefono = destinatario.Telefono,
                Lada = destinatario.Lada,
                Error = destinatario.Error,
                Resultado = destinatario.Resultado,
                EnvioMetadata = destinatario.EnvioMetadata,
                FechaEnvio = destinatario.FechaEnvio
            };
            this.sicemContext.Destinatarios.Add(newDestinatario);
            count++;
            
            // * save changes every 100 records
            if (count % 100 == 0)
            {
                t += await this.sicemContext.SaveChangesAsync();
            }
        }

        t += await this.sicemContext.SaveChangesAsync();

        this.sicemContext.ChangeTracker.AutoDetectChangesEnabled = true;
        return t;
    }

    public async Task<Guid> StoreBoletinMensaje(Guid boletinId, IBoletinMensaje mensaje)
    {
        var newMansaje = new BoletinMensaje
        {
            Id = Guid.NewGuid(),
            BoletinId = boletinId,
            EsArchivo = mensaje.EsArchivo,
            Mensaje = mensaje.Mensaje,
            MimmeType = mensaje.MimmeType,
            FileSize = mensaje.FileSize,
            CreatedAt = mensaje.CreatedAt,
            DeletedAt = mensaje.DeletedAt
        };

        this.sicemContext.BoletinMensajes.Add(newMansaje);
        await this.sicemContext.SaveChangesAsync();
        return newMansaje.Id;
    }
}