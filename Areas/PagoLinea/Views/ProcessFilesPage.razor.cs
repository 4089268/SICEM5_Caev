using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
        private DateTime f1, f2;
        private int Subsistema, Sector;

        private List<RecordsFile> recordsFiles = new();
        private int tabIndex = 0;
        private MatTabBar mattabbar;

        private DateTime fecha1 = DateTime.Now;
        private DateTime fecha2 = DateTime.Now;

        private bool _chbOnlyPaided = false;
        public bool ChboOnlyPaided
        {
            get => _chbOnlyPaided;

            set {
                _chbOnlyPaided = value;
                DataGrid.Refresh();
            }
        }

        protected override void OnInitialized()
        {
            var _now = DateTime.Now;
            this.f1 = new DateTime(_now.Year, _now.Month, 1);
            this.f2 = new DateTime(_now.Year, _now.Month, DateTime.DaysInMonth(_now.Year, _now.Month));
            this.Subsistema = 0;
            this.Sector = 0;
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

            var recordsFile = new RecordsFile
            {
                Name = e.File.Name
            };

            //* get a temporary path to store the file
            var filePath = Path.Combine(Environment.ContentRootPath, "Temp", e.File.Name);

            // * save the file temporarily
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using(var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using var readStream = e.File.OpenReadStream();
                await readStream.CopyToAsync(fileStream);
            }
            Logger.LogDebug("File save at {tempPath}", filePath);

            // * process the CSV file
            var tmpRecords = await ProcessTheFileCsvAsync(filePath);

            // * get the satus of the payments
            Records.AddRange( await PagoLineaService.CalulateStatusPayment(tmpRecords));

            Logger.LogDebug("Total records {total}", tmpRecords.Count());

            // * save the file data
            recordsFile.TotalRecords = tmpRecords.Count();
            recordsFile.From = tmpRecords.OrderBy(item => item.Fecha).First().Fecha;
            recordsFile.To = tmpRecords.OrderByDescending(item => item.Fecha).First().Fecha;
            this.recordsFiles.Add(recordsFile);
            
            // * clean up the temporary filea
            try
            {
                File.Delete(filePath);
                Logger.LogDebug("Archivo eliminado");
            }
            catch (System.Exception err)
            {
                Logger.LogInformation("Cant delete the file {file}: {message}", filePath, err.Message);
            }

            // * update date ranges
            UpdateDateRanges();

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
            var csvDataLines = await File.ReadAllLinesAsync(filePath);
            foreach (var item in csvDataLines.Skip(1))
            {
                try
                {
                    var newRecord = TransactionRecordAdapter.Adapt(fileName, item.Replace("\"", "").Split(","));
                    records.Add(newRecord);
                }
                catch (Exception err)
                {
                    this.Logger.LogError(err, "Fail");
                }
            }
            Logger.LogDebug("Total records {total}", records.Count);
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

        private void UpdateDateRanges()
        {
            var from = this.recordsFiles.Select( item => item.From).OrderBy( item => item).First();
            var to = this.recordsFiles.Select( item => item.To).OrderByDescending( item => item).First().AddDays(1);
            this.fecha1 = from;
            this.fecha2 = to;
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
    }
}