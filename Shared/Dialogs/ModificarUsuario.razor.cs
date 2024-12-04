using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using MatBlazor;
using Syncfusion.Blazor.Grids;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;

namespace Sicem_Blazor.Shared.Dialogs
{
    public partial class ModificarUsuario
    {
        [Inject]
        private IMatToaster Toaster {get;set;}
        
        [Inject]
        private IJSRuntime JSRuntime {get;set;}
        
        [Inject]
        private SicemService SicemService {get;set;}
        
        [Inject]
        private UsersSicemService UsersSicemService {get;set;}
        
        [Inject]
        private ILogger<ModificarUsuario> Logger {get;set;}


        [Parameter]
        public bool Visible { get; set; } = false;
        
        [Parameter]
        public EventCallback CerrarModal { get; set; }

        [Parameter]
        public string Titulo { get; set; } = "CREAR NUEVO USUARIO";


        private List<CatOpcione> CatalogoOpciones { get; set; }
        private List<Ruta> CatalogoOficinas { get; set; }
        public Usuario usuario;
        public bool modificado = false;
        public string password = "";
        public string confirm_password  = "";
        public List<int> opcionesSeleccionadas = new List<int>();
        public List<int> oficinasSeleccionadas = new List<int>();

        public void ShowDialog(){
            this.Titulo = "AGREGAR NUEVO USUARIO";
            this.modificado = false;
            this.usuario = new Usuario() {
                Nombre = "",
                Usuario1 = "",
                Administrador = false,
                Inactivo = true,
                CfgOpc = false,
                UltimaModif = DateTime.Now
            };
            this.password = "";
            this.confirm_password = "";
            LoadCatalogs();
        }

        public void ShowDialog(Usuario usuario){
            this.Titulo = $"modificado USUARIO {usuario.Nombre.ToUpper()}";
            this.modificado = true;
            this.usuario = usuario;
            this.password = "";
            this.confirm_password = "";
            LoadCatalogs();
        }
        
        private void LoadCatalogs(){

            var _idsOpciones = SicemService.ObtenerListaOpcionesDelUsuario(idUsuario:usuario.Id).Select(item => (int) item.IdOpcion).ToList<int>();
            var _idsOficinas = SicemService.ObtenerOficinasDelUsuario(idUsuario: usuario.Id).Select(item => (int) item.Id).ToList<int>();

            this.opcionesSeleccionadas = _idsOpciones;
            this.oficinasSeleccionadas = _idsOficinas;

            this.CatalogoOpciones = SicemService.ObtenerCatalogoOpciones();
            this.CatalogoOficinas = SicemService.ObtenerEnlaces().ToList();
        }

        private async Task HandleGuardarCambiosClick(){

            if(usuario == null){
                return;
            }
            if(usuario.Nombre.Length <= 1){
                Toaster.Add("Escriba un nombre valido e intente de nuevo.", MatToastType.Warning);
                try {
                    await JSRuntime.InvokeVoidAsync("shake", "#cf_user-nombre");
                    await JSRuntime.InvokeVoidAsync("FocusElement", "cf_user-nombre");
                }
                catch (Exception) { }
                return;
            }
            if (usuario.Usuario1.Length <= 1) {
                Toaster.Add("Escriba un usuario valido e intente de nuevo.", MatToastType.Warning);
                try {
                    await JSRuntime.InvokeVoidAsync("shake", "#cf_user-usuario");
                    await JSRuntime.InvokeVoidAsync("FocusElement", "cf_user-usuario");
                }
                catch (Exception) { }
                return;
            }
            if( (!modificado && password.Length < 8 ) || (modificado && password.Trim().Length > 0 && password.Trim().Length < 8 )){
                Toaster.Add("Escriba una contraseña con mínimo de 8 caracteres.", MatToastType.Warning);
                try {
                    await JSRuntime.InvokeVoidAsync("shake", "#cf_user-pass");
                    await JSRuntime.InvokeVoidAsync("FocusElement", "cf_user-pass");
                }
                catch (Exception) { }
                return;
            }
            if(password != confirm_password ) {
                Toaster.Add("Las contraseñas no coinciden, verifique e intente de nuevo.", MatToastType.Warning);
                try {
                    await JSRuntime.InvokeVoidAsync("shake", "#cf_user-pass");
                    await JSRuntime.InvokeVoidAsync("FocusElement", "cf_user-pass");
                }
                catch (Exception) { }
                return;
            }
            if(opcionesSeleccionadas.Count <= 0) {
                Toaster.Add("Seleccione mínimo una opción para el usuario.  ", MatToastType.Warning);
                try {
                    await JSRuntime.InvokeVoidAsync("shake", "#cf-tit-opc");
                }
                catch (Exception) { }
                return;
            }
            if (oficinasSeleccionadas.Count <= 0) {
                Toaster.Add("Seleccione mínimo una oficina para el usuario.  ", MatToastType.Warning);
                try {
                    await JSRuntime.InvokeVoidAsync("shake", "#cf-tit-ofi");
                }
                catch (Exception) { }
                return;
            }


            //*** Comprobar si usuario existe ***
            if (modificado) {
                var _user = UsersSicemService.ActualizarUsuario(usuario, password, opcionesSeleccionadas, oficinasSeleccionadas);
            }else {
                var _user = UsersSicemService.GenerarUsuarioNuevo(usuario, password, opcionesSeleccionadas, oficinasSeleccionadas);
            }

            Toaster.Add((this.modificado)?$"Usuario {usuario.Nombre.ToUpper()} actualizado!!": $"Usuario {usuario.Nombre.ToUpper()} generado!!", MatToastType.Success);
            await this.CerrarModal.InvokeAsync(null);
            
        }
        
        private void CheckBox_Changed(string opcion, object val) {
            switch (opcion) {
                case "Admin":
                    usuario.Administrador = (bool)val;
                    break;

                case "Ofi":
                    usuario.CfgOfi = (bool)val;
                    break;

                case "Ops":
                    usuario.CfgOpc = (bool)val;
                    break;
            }
        }

        public bool ComprobarOpcionSeleccionada(int idOpcion) {
            if (opcionesSeleccionadas.Count <= 0) {
                return false;
            }
            else {
                return opcionesSeleccionadas.Contains(idOpcion);
            }
        }
        
        public bool ComprobarOficinaSeleccionada(int idOficina){
            if(oficinasSeleccionadas.Count <= 0) {
                return false;
            }
            else {
                return oficinasSeleccionadas.Contains(idOficina);
            }
        }
        
        private async Task ChechBoxGrid_Changed(string grid, int idOpcion, bool isCheked) {
            if(grid == "catOpciones"){
                if (isCheked) {
                    if (!opcionesSeleccionadas.Contains(idOpcion)) {
                        opcionesSeleccionadas.Add(idOpcion);
                    }
                }
                else {
                    opcionesSeleccionadas.Remove(idOpcion);
                }
            }
            if(grid == "catOficinas"){
                if (isCheked) {
                    if (!oficinasSeleccionadas.Contains(idOpcion)) {
                        oficinasSeleccionadas.Add(idOpcion);
                    }
                }
                else {
                    oficinasSeleccionadas.Remove(idOpcion);
                }
            }
            await Task.Delay(100);
        }
    }
}
