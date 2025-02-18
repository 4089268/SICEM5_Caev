using System;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.Boletines.Models;

public class BoletinDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public DateTime CreatedAt { get; set;}
    public DateTime? FinishedAt { get; set;}

    public int TotalDestinatarios {get;set;} = 0;
    public int Exitos {get;set;} = 0;
    public int Fallidos {get;set;} = 0;

    public static BoletinDTO FromEntity(OprBoletin b)
    {
        var newItem = new BoletinDTO();
        newItem.Id = b.Id;
        newItem.Titulo = b.Titulo;
        newItem.CreatedAt = b.CreatedAt;
        newItem.FinishedAt = b.FinishedAt;
        return newItem;
    }

}
