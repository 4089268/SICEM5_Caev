using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models.Request;
using Sicem_Blazor.Data.Filtros;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class LogearseController : Controller {

        private readonly SicemService sicemService;
        
        public LogearseController(SicemService s) {
            sicemService = s;
        }
        

        [HttpPost]
        [Route("/api/logearse")]
        public IActionResult Logearse(LogearseRequest request){
            var ipAddress = HttpContext.Connection.RemoteIpAddress;
            
            var r =  sicemService.Logearse(request.usuario, request.password, ipAddress.ToString());
            if(r == null){
                var usuario = sicemService.Usuario;
                
                //****** Obtener oficinas 
                int _idUsuario = ConvertUtils.ParseInteger(usuario.Id);
                var resultOficinas = sicemService.ObtenerOficinasDelUsuario(_idUsuario).Select(item => new {id = item.Id, oficina = item.Nombre });
                
                //****** Regresar datos
                return Ok( new {
                    session = sicemService.IdSession,
                    nombre = usuario.Nombre,
                    admin = (usuario.Administrador == true?1:0),
                    oficinas = resultOficinas
                    }
                );
            }else{
                return Unauthorized( new {message = r});
            }
        }

        [HttpPost]
        [Route("/api/logearse/cerrarSesion")]
        public IActionResult CerrarSesion(){
            if( HttpContext.Request.Headers.ContainsKey("session") ){
                var token = HttpContext.Request.Headers["session"].ToString();
                sicemService.CerrarSesionDb(token, HttpContext.Connection.RemoteIpAddress.ToString());
                return Ok();
            }else{
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("/api/oficinas")]
        [ServiceFilter(typeof(LogeadoFilter))]
        public IActionResult ObtenerOficina(){
            var usuario = (Usuario) HttpContext.Items["usuario"];

            var oficinas = sicemService.ObtenerOficinasDelUsuario(usuario.Id);

            var result = oficinas.Select(item => new {id = item.Id, oficina = item.Nombre });

            return Ok(result);
        }


    }
}
