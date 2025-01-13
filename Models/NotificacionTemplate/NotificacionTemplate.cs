using System;
using Aspose.Pdf;


namespace Sicem_Blazor.Models;

public class NotificacionTemplate
{
    public Guid UUID {get;}
    public string Titulo {get;set;}
    public string Texto {get;set;}
    public DateTime FCreacion {get;set;}
    public DateTime UltimaMod {get;set;}
    public DateTime? DeletedAt {get;set;}
    public Guid? ImageId {get;set;}
    public AppendTextSettings AppendTextSettings {get;set;}
    
    public NotificacionTemplate()
    {
        UUID = Guid.NewGuid();
    }
    public NotificacionTemplate(CatMessagesTemplate e)
    {
        UUID = e.Id;
        Titulo = e.Titulo;
        Texto = e.Mensaje;
        FCreacion = e.FechaCreacion;
        UltimaMod = e.UltimaModificacion;
        DeletedAt = e.DeletedAt;
        ImageId = e.ImageId;
        if(!string.IsNullOrEmpty(e.ImagePropertiesJson))
        {
            this.AppendTextSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<AppendTextSettings>(e.ImagePropertiesJson);
        }
    }

    public CatMessagesTemplate ToCatMessageTemplate()
    {
        var item = new CatMessagesTemplate();
        item.Id = UUID;
        item.Titulo = Titulo;
        item.Mensaje = Texto;
        item.FechaCreacion = FCreacion;
        item.UltimaModificacion = DateTime.Now;
        item.DeletedAt = DeletedAt;
        item.ImageId = ImageId;
        item.ImagePropertiesJson = Newtonsoft.Json.JsonConvert.SerializeObject(AppendTextSettings);
        return item;
    }
}