using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Charts;
using MatBlazor;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;
using Sicem_Blazor.Models;
using Sicem_Blazor.Services;
using Sicem_Blazor.Helpers;
using Sicem_Blazor.PagoLinea.Models;
using Sicem_Blazor.PagoLinea.Data;
using Sicem_Blazor.Services.PagoLinea;
using Sicem_Blazor.Models.PagoLinea;

namespace Sicem_Blazor.PagoLinea.Views
{
    public partial class ProcessFilesPage
    {

        [Inject]
        public NavigationManager NavManager {get;set;} = default!;
        
        [Inject]
        public IMatToaster Toaster {get;set;} = default!;
        
        [Inject]
        public PagoLineaService PagoLineaService {get;set;} = default!;
        
        [Inject]
        public HttpPagoLineaService HPagoLineaService {get;set;} = default!;
        
        [Inject]
        public SicemService SicemService {get;set;} = default!;
        
        [Inject]
        public IWebHostEnvironment Environment {get;set;} = default!;

        [Inject]
        public ILogger<PagoLineaPage> Logger {get;set;} = default!;


        private SfGrid<RecordsFile> DataGridFiles {get;set;}
        private SfGrid<TransactionRecord> DataGrid {get;set;}
        private List<TransactionRecord> Records {get;set;} = new();

        private List<TransactionRecord> RecordsFiltered {
            get {
                if(_chbOnlyPaided)
                {
                    return Records.Where(item => item.Status == TransactionRecord.ProcessFileStatues.NoPaided).ToList();
                }
                return Records;
            }
        }

        private bool busyDialog = false;
        
        private List<RecordsFile> recordsFiles = new();
        private int tabIndex = 0;
        private MatTabBar mattabbar;

        private bool _chbOnlyPaided = false;
        public bool ChboOnlyPaided
        {
            get => _chbOnlyPaided;

            set {
                _chbOnlyPaided = value;
                DataGrid.Refresh();
            }
        }


        private async Task HandleUploadFile(InputFileChangeEventArgs e)
        {
            Logger.LogDebug("Attemp to upload the File");
            
            if(e.File == null || !BrowserFileValidator.IsCsvFile(e.File))
            {
                Toaster.Add("El archivo debe ser un archivo CSV válido.", MatToastType.Warning);
                return;
            }

            // * Validate if the file is not already loaded
            if(recordsFiles.Select(item => item.Name.ToLower()).Contains(e.File.Name.ToLower()))
            {
                Toaster.Add("Este archivo ya se encuentra cargado", MatToastType.Info);
                return;
            }


            this.busyDialog = true;
            await Task.Delay(100);


            // * save the file temporarily
            var filePath = await SaveFileTemporally(e.File);


            // * process the CSV file
            var tmpRecords = await ProcessTheFileCsvAsync(filePath);
            if(!tmpRecords.Any())
            {
                Toaster.Add("Error al procesar el archivo, no contiene datos validos.", MatToastType.Danger);
                this.busyDialog = false;
                StateHasChanged();
            }


            // * upload the records
            IEnumerable<StorePaymentRequest> uploadRequest = tmpRecords.Select(item => TransactionRecordAdapter.AdaptToStorePaymentRequest(item)).ToArray();
            var storedRecordsId = await HPagoLineaService.StorePaymentsRecords(uploadRequest);


            // * process the response and update the records stored successfully
            foreach(var record in tmpRecords)
            {
                if(storedRecordsId.Contains(record.ID))
                {
                    record.Status = TransactionRecord.ProcessFileStatues.Paided;
                }
            }
            Records.AddRange(tmpRecords);


            // * save the file data
            this.recordsFiles.Add(new RecordsFile
                {
                    Name = e.File.Name,
                    TotalRecords = tmpRecords.Count(),
                    From = tmpRecords.OrderBy(item => item.Fecha).First().Fecha,
                    To = tmpRecords.OrderByDescending(item => item.Fecha).First().Fecha
                }
            );


            // * clean up the temporary filea
            ClearTemporallyFile(filePath);


            // * refresh UI
            this.busyDialog = false;
            StateHasChanged();
            DataGridFiles.Refresh();
            Toaster.Add("Archivo cargado", MatToastType.Success);
        }

        private async Task<IEnumerable<TransactionRecord>> ProcessTheFileCsvAsync(string filePath)
        {
            Logger.LogDebug("Attempt ot process the file {tempPath}", filePath);
            
            var records = new List<TransactionRecord>();
            var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

            try
            {
                // * process the file and return the lines
                IEnumerable<string[]> csvData = Helpers.ProcessCSV.LoadFile(filePath);
                foreach (var data in csvData.Skip(1)) // * skip the first row because has the headers
                {
                    if(string.IsNullOrWhiteSpace(data[0])) continue;
                    var newRecord = TransactionRecordAdapter.Adapt(fileName, data);
                    records.Add(newRecord);
                }
            }
            catch (System.Exception ex)
            {
                this.Logger.LogError(ex, "Fail at process the csv: {message}", ex.Message);
                return [];
            }
            
            Logger.LogDebug("Total records {total}", records.Count);
            await Task.CompletedTask;
            return records;
        }

        private async Task RemoveFile(RecordsFile file)
        {
            this.busyDialog = true;
            await Task.Delay(100);

            this.Records = Records.Where( item => item.FileName != file.Name).ToList();
            this.recordsFiles = this.recordsFiles.Where( item => item.Name != file.Name).ToList();
            DataGridFiles.Refresh();

            await Task.Delay(100);
            this.busyDialog = false;
        }

        private async Task MatTabBarActiveChanged(BaseMatTabLabel label)
        {
            tabIndex = label.Id switch {
                "archivos" => 0,
                "cargados" => 1,
                "noaplicados" => 2,
                _ => 0
            };
            await Task.CompletedTask;
        }

        private async Task DownloadExcel()
        {
            await DataGrid.ExportToExcelAsync();
        }
    
        private async Task<string> SaveFileTemporally(IBrowserFile file)
        {
            //* get a temporary path to store the file
            var filePath = Path.Combine(Environment.ContentRootPath, "Temp", file.Name);

            // * save the file temporarily
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using(var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using var readStream = file.OpenReadStream();
                await readStream.CopyToAsync(fileStream);
            }
            Logger.LogDebug("File save at {tempPath}", filePath);
            return filePath;
        }

        private void ClearTemporallyFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                Logger.LogDebug("Archivo eliminado");
            }
            catch (System.Exception err)
            {
                Logger.LogInformation("Cant delete the file {file}: {message}", filePath, err.Message);
            }
        }
    }
}