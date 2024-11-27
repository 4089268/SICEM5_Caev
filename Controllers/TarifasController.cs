using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sicem_Blazor.Services;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Controllers {
    
    [ApiController]
    [Route("api/[controller]")]
    public class TarifasController : ControllerBase {

        private readonly SicemService sicemService;
        private readonly ITarifasService tarifasService;

        public TarifasController(SicemService s, ITarifasService t) {
            this.sicemService = s;
            this.tarifasService = t;
        }

        [HttpGet]
        [Route("/api/[controller]/generarHistorial")]
        public IActionResult GenerarHistorial() {

            var _response = new List<Dictionary<string, object>>();

            var _oficinas = sicemService.ObtenerEnlaces()
                    .Where(item => item.Inactivo == false)
                    .ToList();

            var _tareas = new Task[_oficinas.Count];

            _oficinas.ForEach(oficina => {
                bool _resultTask = false;
                var _tmpTask = Task.Run<bool>(() => tarifasService.GenerarHistorialTarifas(oficina));
                if(_tmpTask.Wait(TimeSpan.FromMinutes(5))) {
                    _resultTask = _tmpTask.Result;
                }
                else {
                    _resultTask = false;

                }
                _response.Add(new Dictionary<string, object>() {
                    { "id", oficina.Id },
                    { "oficina", oficina.Oficina },
                    { "estatus", _resultTask?"completado":"error"}
                });
            });
            
            return Ok(_response);
        }
        
    }
}
