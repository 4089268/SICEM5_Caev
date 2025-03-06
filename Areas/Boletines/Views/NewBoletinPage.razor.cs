using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Charts;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;
using Sicem_Blazor.Data;
using Sicem_Blazor.Boletines.Services;
using Sicem_Blazor.Boletines.Models;
using Sicem_Blazor.Helpers;

namespace Sicem_Blazor.Boletines.Views
{
    public partial class NewBoletinPage
    {

        [Inject]
        public NavigationManager NavManager {get;set;} = default!;
        
        [Inject]
        public IMatToaster Toaster {get;set;} = default!;
        
        [Inject]
        public SicemService SicemService {get;set;} = default!;
        
        [Inject]
        public ILogger<NewBoletinPage> Logger {get;set;} = default!;

        [Inject]
        public IBoletinService BoletinService {get;set;} = default!;

        private bool busyDialog = false;
        private OprBoletin Boletin {get;set;}
        private string MensageString {get;set;} = "";
        public List<Destinatario> Destinatarios {get;set;}
        public int DestinatariosError {
            get => (Destinatarios?.Where(item => item.Error == true).Count() ?? 0);
        }
        private List<BoletinMensaje> AttachedFiles {get;set;}
        private CultureInfo currentCultueInfo = new("es-MX");
        private bool dialogIsOpen = false;
        private string newContactValue = null;


        protected override async Task OnInitializedAsync()
        {
            await Task.CompletedTask;

            Boletin = new OprBoletin
            {
                Titulo = "Boletin " + DateTime.Now.ToString("yyyyMMddHHmm")
            };

            AttachedFiles = new();
            Destinatarios = new();
        }

        private async Task GoBackClick()
        {
            await Task.CompletedTask;
            NavManager.NavigateTo("/Boletines");
        }

        public async Task ImportContacts(InputFileChangeEventArgs e)
        {
            busyDialog = true;
            await Task.Delay(100);
            
            // * proccesing the file
            IBrowserFile browserFile = e.File;
            this.Logger.LogDebug("Archivo seleccionado {name}", browserFile.Name);
            var _tmpPath = string.Format("{0}/tmp_{1}.{2}", System.IO.Path.GetTempPath(), Guid.NewGuid().ToString(), browserFile.Name.Split(".").Last());
            using (FileStream fs = new FileStream(_tmpPath, System.IO.FileMode.CreateNew))
            {
                using(var stream = browserFile.OpenReadStream())
                {
                    await stream.CopyToAsync(fs);
                }
            }
            this.Logger.LogDebug("Archivo generado: {path}", _tmpPath);
            var procesarExcel = new ProcesarExcel();
            var _datos = procesarExcel.ToDataTable(_tmpPath,2);
            this.Logger.LogDebug("Total filas: {total}", _datos.Rows.Count);

            // * get the phone numbers
            var tmpListaDestinatarios = new List<Destinatario>();
            var tmpDestinatariosFailure = new List<string>();

            for(int i = 0; i< _datos.Rows.Count; i++)
            {
                var row = _datos.Rows[i];
                this.Logger.LogDebug("Procesando {i} de {total}", i + 1, _datos.Rows.Count);
                
                if(Boletin.Proveedor == "EMAIL")
                {
                    ProcessContactRowEmail(row, tmpListaDestinatarios, tmpDestinatariosFailure);
                }
                else
                {
                    ProcessContactRowPhone(row, tmpListaDestinatarios, tmpDestinatariosFailure);
                }
            }

            // * append the new phone numbers
            if(Destinatarios == null)
            {
                Destinatarios = new List<Destinatario>();
            }
            Destinatarios.AddRange(tmpListaDestinatarios);

            busyDialog = false;

            // * show failures
            if(tmpDestinatariosFailure.Any())
            {
                this.Toaster.Add($"Se encontraron {tmpDestinatariosFailure.Count} contactos no validos", MatToastType.Warning);
                this.Logger.LogWarning("Se encontraron contactos no validos: {total}", tmpDestinatariosFailure.Count);
                foreach(var item in tmpDestinatariosFailure)
                {
                    this.Logger.LogWarning("Contacto no valido: {contacto}", item);
                }
            }
        }

        private void ProcessContactRowPhone(DataRow row, List<Destinatario> tmpListaDestinatarios, List<string> tmpDestinatariosFailure)
        {
            try
            {
                // * validate the contact phone number
                if(!ValidateContact.IsValidPhoneNumber(row[0].ToString().Trim()))
                {
                    tmpDestinatariosFailure.Add(row[0].ToString());
                    return;
                }

                // * check if the phone is already stored
                long telefono = Convert.ToInt64(row[0]);
                int lada = int.TryParse(row[1].ToString(), out int _lada) ? _lada : 52;
                if(Destinatarios.Select(item => item.Telefono).Contains(telefono))
                {
                    return;
                }

                // * save the phonenumber
                tmpListaDestinatarios.Add( new Destinatario
                {
                    BoletinId = Boletin.Id,
                    Lada = lada.ToString(),
                    Telefono = telefono,
                    Titulo = $"+{lada} {telefono}"
                });
            } catch(Exception ex)
            {
                this.Logger.LogError("No se puede cargar el telefono '{phone}': {message}", row[0], ex.Message);
            }
        }

        private void ProcessContactRowEmail(DataRow row, List<Destinatario> tmpListaDestinatarios, List<string> tmpDestinatariosFailure)
        {
            try
            {
                // * validate the contact email
                if(!ValidateContact.IsValidEmail(row[0].ToString().Trim()))
                {
                    tmpDestinatariosFailure.Add(row[0].ToString());
                    return;
                }

                // * check if the phone is already stored
                string correo = row[0].ToString().Trim();
                if(Destinatarios.Select(item => item.Correo).Contains(correo))
                {
                    return;
                }

                // * save the contact
                tmpListaDestinatarios.Add(new Destinatario
                {
                    BoletinId = Boletin.Id,
                    Correo = correo,
                    Titulo = correo
                });
            } catch(Exception ex)
            {
                this.Logger.LogError("No se puede cargar el correo '{correo}': {message}", row[0], ex.Message);
            }
        }

        public async Task AddContact()
        {
            if(dialogIsOpen)
            {
                return;
            }
            this.newContactValue = string.Empty;
            this.dialogIsOpen = true;
            await Task.CompletedTask;
        }

        private async Task UploadAttachFile(InputFileChangeEventArgs e)
        {
            busyDialog = true;
            await Task.Delay(100);
            
            // * proccesing the file
            foreach (IBrowserFile file in e.GetMultipleFiles())
            {

                string base64String = string.Empty;

                // * Read file into memory
                using(var memoryStream = new MemoryStream())
                {
                    using(var stream = file.OpenReadStream(maxAllowedSize: 10_000_000))
                    {
                        await stream.CopyToAsync(memoryStream);
                    }

                    // * Convert to Base64
                    base64String = Convert.ToBase64String(memoryStream.ToArray());
                }

                // * create the messageFile model
                var messageFile = new BoletinMensaje
                {
                    BoletinId = Boletin.Id,
                    EsArchivo = true,
                    Mensaje = base64String,
                    MimmeType = file.ContentType,
                    FileSize = (int) file.Size,
                    FileName = file.Name,
                    CreatedAt = DateTime.Now
                };

                this.AttachedFiles.Add(messageFile);
            }

            busyDialog = false;
        }

        private async Task SaveClick()
        {
            busyDialog = true;
            await InvokeAsync(StateHasChanged);

            // * validate if the message is setted
            if(string.IsNullOrEmpty(MensageString) && !AttachedFiles.Any())
            {
                Toaster.Add("Agrege un mensaje o un archivo.", MatToastType.Warning);
                busyDialog = false;
                return;
            }

            // * Validate destinatarios
            if(!Destinatarios.Any())
            {
                Toaster.Add("Agregue minimo un contacto destinatario.", MatToastType.Warning);
                busyDialog = false;
                return;
            }

            var boletinID = await this.BoletinService.AlmacenarBoletin(BoletinDTO.FromEntity(this.Boletin));

            if(!string.IsNullOrEmpty(this.MensageString))
            {
                this.Logger.LogDebug("Generando mensaje de texto: {texto}", this.MensageString);
                await this.BoletinService.StoreBoletinMensaje(boletinID, new BoletinMensaje
                    {
                        BoletinId = boletinID,
                        EsArchivo = false,
                        Mensaje = this.MensageString,
                        CreatedAt = DateTime.Now
                    }
                );
            }

            foreach(var fileAttach in AttachedFiles)
            {
                await this.BoletinService.StoreBoletinMensaje(boletinID, fileAttach);
            }

            var t = await this.BoletinService.StoreBoletinDestinatatioRange(boletinID, Destinatarios);
            this.Logger.LogDebug("{t} Destinatarios almacenados", t);
            
            busyDialog = false;
            await InvokeAsync(StateHasChanged);

            this.Toaster.Add("Boletin Registrado!", MatToastType.Info);
            await GoBackClick();
        }

        private void HandleSelectionChange(ChangeEventArgs e)
        {
            Boletin.Proveedor = e.Value?.ToString();
            this.Destinatarios.Clear();
            StateHasChanged();
        }

        private void DialogContactSave()
        {
            // * validate the input value
            if(string.IsNullOrEmpty(newContactValue))
            {
                this.Toaster.Add("Ingrese un contacto valido.", MatToastType.Warning);
                return;
            }


            Destinatario newDestinatario = null;
            
            if(Boletin.Proveedor == "EMAIL")
            {
                if(!ValidateContact.IsValidEmail(newContactValue))
                {
                    this.Toaster.Add("Ingrese un correo electronico valido.", MatToastType.Warning);
                    return;
                }

                if(this.Destinatarios.Select(item => item.Correo).Contains(newContactValue.Trim()))
                {
                    this.Toaster.Add("El correo ya se encuentra registrado.", MatToastType.Warning);
                    return;
                }

                newDestinatario = new Destinatario
                {
                    BoletinId = Boletin.Id,
                    Correo = newContactValue.Trim(),
                    Titulo = newContactValue
                };

            }
            else
            {
                if(!ValidateContact.IsValidPhoneNumber(newContactValue))
                {
                    this.Toaster.Add("Ingrese un numero de telefono valido de 10 digitos.", MatToastType.Warning);
                    return;
                }

                if(this.Destinatarios.Select(item => item.Telefono).Contains(long.Parse(newContactValue)))
                {
                    this.Toaster.Add("El numero de telefono ya se encuentra registrado.", MatToastType.Warning);
                    return;
                }

                newDestinatario = new Destinatario
                {
                    BoletinId = Boletin.Id,
                    Lada = "52",
                    Telefono = long.Parse(newContactValue),
                    Titulo = $"+52 {newContactValue}"
                };
            }

            this.Destinatarios.Add(newDestinatario);
            this.dialogIsOpen = false;
            StateHasChanged();
        }
    }
}