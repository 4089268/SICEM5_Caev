using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using Sicem_Blazor.Services;

namespace Sicem_Blazor.Data.Filtros {

    public class LogeadoFilter : ActionFilterAttribute {
        private readonly SicemService service;
        
        public LogeadoFilter(SicemService s){
            service = s;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context){
            
            if(context.HttpContext.Request.Headers.ContainsKey("session")){
                var token = context.HttpContext.Request.Headers["session"];
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
                var tmpUser = service.ObtenerUsuarioToken(token, ipAddress );

                if(tmpUser == null){
                    context.Result = new StatusCodeResult(403);
                }else{
                    var oficinas = ((Sicem_Blazor.Models.Ruta[]) service.ObtenerOficinasDelUsuario(tmpUser.Id)).Select(item => item.Id).ToArray();
                    context.HttpContext.Items.Add("usuario", tmpUser);
                    context.HttpContext.Items.Add("oficinas", oficinas);
                    base.OnActionExecuting(context);
                }

            }else{
                context.Result = new  StatusCodeResult(403);
            }
        }
    }

}