using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.SeguimientoCobros.Data
{
    public class SeguimientoCobroMapJsInterop : IAsyncDisposable
    {

        /// <summary>
        /// reference of the js module
        /// </summary>
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public SeguimientoCobroMapJsInterop(IJSRuntime jSRuntime)
        {
            moduleTask = new Lazy<Task<IJSObjectReference>>(() => jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/sicem/seguimientoCobroMap.js").AsTask() );
        }

        public async Task InitializedMapAsync(object objRef, string elementId, IMapPoint point)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("initialize", objRef, elementId, point);
        }

        public async Task MoveMap(object objRef, IMapPoint point, double? zoom)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("moveMap", objRef, point, zoom);
        }

        public async Task UpdateMarks(object objRef, IEnumerable<MapMark> marks)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("updateMarks", objRef, marks);
        }

        public async ValueTask DisposeAsync()
        {
            if( moduleTask.IsValueCreated){
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }

}