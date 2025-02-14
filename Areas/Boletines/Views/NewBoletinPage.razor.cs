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
        private string MensajeString {get;set;} = "";
        private List<Destinatario> Destinatarios {get;set;}
        private List<BoletinMensaje> AttachedFiles {get;set;}

        private CultureInfo currentCultueInfo = new("es-MX");


        protected override async Task OnInitializedAsync()
        {
            await Task.CompletedTask;

            Boletin = new OprBoletin
            {
                Titulo = "Boletin " + DateTime.Now.ToString("yyyyMMddHHmm")
            };

            AttachedFiles = new();
        }

        private async Task GoBackClick()
        {
            await Task.CompletedTask;
            NavManager.NavigateTo("/Boletines");
        }

        private async Task LoadFilePhones(InputFileChangeEventArgs e)
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
            foreach(DataRow row in _datos.Rows)
            {
                var telefono = Convert.ToInt64(row[0]);
                var lada = 52;
                try
                {
                    lada = Convert.ToInt32(row[1]);
                }catch(Exception)
                {
                    //
                }

                if(tmpListaDestinatarios.Select(item=> item.Telefono).Contains(telefono))
                {
                    continue;
                }
                tmpListaDestinatarios.Add( new Destinatario
                {
                    BoletinId = Boletin.Id,
                    Lada = lada.ToString(),
                    Telefono = telefono,
                    Titulo = $"+{lada} {telefono}"
                });
            }

            // * append the new phone numbers
            if(Destinatarios == null)
            {
                Destinatarios = new List<Destinatario>();
            }
            Destinatarios.AddRange(tmpListaDestinatarios);

            busyDialog = false;
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
            var boletinID = await this.BoletinService.AlmacenarBoletin(BoletinDTO.FromEntity(this.Boletin));

            if(string.IsNullOrEmpty(this.MensajeString))
            {
                await this.BoletinService.StoreBoletinMensaje(boletinID, new BoletinMensaje
                    {
                        BoletinId = boletinID,
                        EsArchivo = false,
                        Mensaje = this.MensajeString,
                        CreatedAt = DateTime.Now
                    }
                );
            }
            foreach(var fileAttach in AttachedFiles)
            {
                await this.BoletinService.StoreBoletinMensaje(boletinID, fileAttach);
            }


            foreach(var dest in Destinatarios)
            {
                await this.BoletinService.StoreBoletinDestinatatio(boletinID, dest);
            }

            this.Toaster.Add("Boletin Registrado!", MatToastType.Info);
            await GoBackClick();
        }
    }
}