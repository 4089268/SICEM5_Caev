using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sicem_Blazor.Boletines.Models;
using Sicem_Blazor.Boletines.Services;

namespace Sicem_Blazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoletinController : ControllerBase
    {
        private ILogger<BoletinController> logger;
        private IBoletinService boletinService;

        public BoletinController(ILogger<BoletinController> log, IBoletinService boletinService)
        {
            this.logger = log;
            this.boletinService = boletinService;
        }


        [HttpGet("attachment/{messageId}")]
        public async Task<IActionResult> GetAttachmentFile([FromRoute] string messageId)
        {
            Guid messageUUID;
            try
            {
                messageUUID = Guid.Parse(messageId);
            }catch(Exception)
            {
                return BadRequest(new {
                    Message = "The message id is not a valid UUID."
                });
            }

            // * get the file
            IBoletinMensaje message;
            try
            {
                message = await boletinService.ObtenerMensaje(messageUUID);
            }
            catch(KeyNotFoundException)
            {
                return NotFound( new {
                    Message = "The message id is not registered on the system."
                });
            }

            // * returna plain text if is not a file
            if(!message.EsArchivo)
            {
                return Ok(message.Mensaje);
            }

            var bytes = Convert.FromBase64String(message.Mensaje);
            var stream = new MemoryStream(bytes);
            return File(stream, message.MimmeType, message.FileName);

        }

    }

}
