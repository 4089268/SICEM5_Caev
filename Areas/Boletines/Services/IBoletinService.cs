using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sicem_Blazor.Boletines.Models;

namespace Sicem_Blazor.Boletines.Services;

public interface IBoletinService
{
    Task<Boletin> GetBoletines();

    Task<BoletinMensaje> GetMensajeBoletin(Guid boletinId);
    
    Task<ICollection<Destinatario>> GetDestinatariosBoletin(Guid boletinId);

    Task<Guid> StoreBoletin(Boletin newBoletin);

    Task<Guid> StoreBoletin(Boletin newBoletin);
}