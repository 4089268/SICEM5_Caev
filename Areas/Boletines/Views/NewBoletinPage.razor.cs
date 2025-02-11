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
using Sicem_Blazor.Services.PagoLinea;
using Sicem_Blazor.Models;
using Sicem_Blazor.Models.PagoLinea;
using Sicem_Blazor.Data;

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
        public NavigationManager NavigationManager1 {get;set;} = default!;


        private SfGrid<ResumeOffice> DataGrid {get;set;}
        private bool busyDialog = false;
        private OprBoletin Boletin {get;set;}
        private string MensajeString {get;set;} = "Hola mundo";
        private List<Destinatario> Destinatarios {get;set;}
        private List<BoletinMensaje> AttachedFiles {get;set;}

        private CultureInfo currentCultueInfo = new("es-MX");


        protected override async Task OnInitializedAsync()
        {
            await Task.CompletedTask;

            Boletin = new OprBoletin
            {
                Titulo = "Nuevo Boletin " + DateTime.Now.ToShortDateString()
            };

            AttachedFiles = new();
        }

        private async Task GoBackClick()
        {
            await Task.CompletedTask;
            NavigationManager1.NavigateTo("/Boletines");
        }

        private async Task LoadFilePhones(InputFileChangeEventArgs e)
        {
            busyDialog = true;
            await Task.Delay(100);
            
            // * proccesing the file
            IBrowserFile browserFile = e.File;
            Console.WriteLine($"Archivo seleccionado {browserFile.Name}");
            var _tmpPath = string.Format("{0}/tmp_{1}.{2}", System.IO.Path.GetTempPath(), Guid.NewGuid().ToString(), browserFile.Name.Split(".").Last());
            using (FileStream fs = new FileStream(_tmpPath, System.IO.FileMode.CreateNew)){
                using(var stream = browserFile.OpenReadStream()){
                    await stream.CopyToAsync(fs);
                }
            }
            Console.WriteLine($"Archivo generado: {_tmpPath}");
            var procesarExcel = new ProcesarExcel();
            var _datos = procesarExcel.ToDataTableFirstCol(_tmpPath);

            // * get the phone numbers
            var tmpListaTelefonos = new List<long>();
            foreach(DataRow row in _datos.Rows)
            {
                var telefono = Convert.ToInt64(row[0]);
                if(!tmpListaTelefonos.Contains(telefono))
                {
                    tmpListaTelefonos.Add(telefono);
                }
            }

            // * append the new phone numbers
            if(Destinatarios == null)
            {
                Destinatarios = new List<Destinatario>();
            }

            // * prevent add duplicated phones
            var phonesToAdd = tmpListaTelefonos.Select(phone =>
                new Destinatario {
                    BoletinId = Boletin.Id,
                    Titulo = $"+52 {phone}",
                    Telefono = phone,
                    Lada = "52"
                }
            );
            Destinatarios.AddRange(phonesToAdd);

            busyDialog = false;
        }

        private async Task UploadAttacFile(InputFileChangeEventArgs e)
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

    }

}