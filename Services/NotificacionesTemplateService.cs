using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Sicem_Blazor.Data;
using Sicem_Blazor.Helpers;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.Services
{
    public class NotificacionesTemplateService
    {
        private readonly SicemContext sicemContext;
        private readonly ILogger<NotificacionesTemplateService> logger;
        private readonly MessageNotificationSettings messageNotificationSettings;
        private readonly IWebHostEnvironment webHostEnvironment;

        public NotificacionesTemplateService(SicemContext context, ILogger<NotificacionesTemplateService> l, IOptions<MessageNotificationSettings> options, IWebHostEnvironment webHostEnvironment)
        {
            this.sicemContext = context;
            this.logger = l;
            this.messageNotificationSettings = options.Value;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<NotificacionTemplate> ObtenerTemplateNotificaciones()
        {
            var _tmpNots = sicemContext.CatMessagesTemplates.ToList();
            return _tmpNots.Select(e => new NotificacionTemplate(e));
        }

        public Guid ActualizarTemplateNotificaciones(NotificacionTemplate template)
        {
            // * si existe, actualizar
            if(sicemContext.CatMessagesTemplates.ToList().Select(e => e.Id).Contains(template.UUID)){
                Console.WriteLine("Actualizar template");

                var tmpData = sicemContext.CatMessagesTemplates.FirstOrDefault(item => item.Id == template.UUID);
                tmpData.Titulo = template.Titulo;
                tmpData.Mensaje = template.Texto;
                tmpData.UltimaModificacion = DateTime.Now;
                tmpData.ImageId = template.ImageId;
                tmpData.ImagePropertiesJson = null;
                if(template.AppendTextSettings != null)
                {
                    tmpData.ImagePropertiesJson = Newtonsoft.Json.JsonConvert.SerializeObject(template.AppendTextSettings);
                }
                sicemContext.CatMessagesTemplates.Update(tmpData);
                sicemContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("Agregar template");
                sicemContext.CatMessagesTemplates.Add(template.ToCatMessageTemplate());
                sicemContext.SaveChanges();
            }
            return template.UUID;
        }

        public IEnumerable<CatImagenesTemplate> GetImagenesTemplates(bool withDeleted = false)
        {
            var data = this.sicemContext.CatImagenesTemplates.OrderBy( item => item.Titulo).ToList();
            return data.Where(item => item.DeletedAt == (withDeleted ?item.DeletedAt :null)).ToList();
        }

        /// <summary>
        /// attempt to updated or add the CatImageTemplate
        /// </summary>
        /// <param name="template"></param>
        /// <exception cref="Exception">Fail al update or insert the catImageTemplate</exception>
        /// <returns></returns>
        public Guid UpdateOrCreatedCatImage(CatImagenesTemplate template)
        {
            // * si existe, actualizar
            if(sicemContext.CatImagenesTemplates.Any( item => item.Id == template.Id))
            {
                this.logger.LogInformation("Attempt to update the CatImagenesTemplate with id '{id}'", template.Id);
                var tmpData = sicemContext.CatImagenesTemplates.Find(template.Id);
                tmpData.Titulo = template.Titulo;
                tmpData.Path = template.Path;
                tmpData.DeletedAt = template.DeletedAt;
                sicemContext.CatImagenesTemplates.Update(tmpData);
                try
                {
                    sicemContext.SaveChanges();
                    this.logger.LogInformation("Successfully to update the CatImagenesTemplate with id '{id}'", template.Id);
                }
                catch(Exception err)
                {
                    this.logger.LogError("Fail at update the CatImagenesTemplate with id '{id}': {message}", template.Id, err.Message);
                    throw;
                }
            }
            else
            {
                this.logger.LogInformation("Attempt to add new CatImagenesTemplate with title '{title}'", template.Titulo);
                sicemContext.CatImagenesTemplates.Add(template);
                try
                {
                    sicemContext.SaveChanges();
                    this.logger.LogInformation("Successfully added new CatImagenesTemplate with title '{title}' and id '{id}'", template.Titulo, template.Id);
                }
                catch(Exception err)
                {
                    this.logger.LogError("Fail at add the CatImagenesTemplate with title '{title}': {message}", template.Titulo, err.Message);
                    throw;
                }
            }
            return template.Id;
        }


        /// <summary>
        ///  upload the file image into the default path
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> UploadFileImage(Stream stream, string fileName)
        {
            // * get the path to the "wwwroot/notificationImages" folder
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, this.messageNotificationSettings.ImagesPath, fileName);
            
            // * ensures that the path exist
            Directory.CreateDirectory(Path.Combine(webHostEnvironment.WebRootPath, this.messageNotificationSettings.ImagesPath));

            using(FileStream fs = new(filePath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fs);
            }

            return Path.Combine(this.messageNotificationSettings.ImagesPath, fileName);
        }

    }
}