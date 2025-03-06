using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sicem_Blazor.Boletines.Models;

namespace Sicem_Blazor.Boletines.Services;

public interface IBoletinService
{
    Task<ICollection<BoletinDTO>> GetBoletines();
    Task<ICollection<IBoletinMensaje>> GetMensajesBoletin(Guid boletinId);
    Task<ICollection<IBoletinDestinatario>> GetDestinatariosBoletin(Guid boletinId);
    Task<Guid> AlmacenarBoletin(BoletinDTO boletin);

    Task<int> EliminarBoletin(BoletinDTO boletin);
    
    Task<Guid> StoreBoletinMensaje(Guid boletinId, IBoletinMensaje mensaje);
    Task ActualizarMensaje(Guid mensajeId, IBoletinMensaje mensaje);
    Task RemoverMensaje(IBoletinMensaje mensaje);
    Task<IBoletinMensaje> ObtenerMensaje(Guid mensajeId);
    
    Task<Guid> StoreBoletinDestinatatio(Guid boletinId, IBoletinDestinatario destinatario);
    Task<int> StoreBoletinDestinatatioRange(Guid boletinId, IEnumerable<IBoletinDestinatario> destinatarios);
    Task ActualizarDestinatario(Guid destinatarioId, IBoletinDestinatario destinatario);
    Task RemoverDestinatario(IBoletinDestinatario destinatario);

}