using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Charts;
using MatBlazor;
using Sicem_Blazor.Services;
using Sicem_Blazor.Models;
using Sicem_Blazor.Boletines.Services;
using Sicem_Blazor.Boletines.Models;
using SixLabors.Fonts;

namespace Sicem_Blazor.Boletines.Views
{
    public partial class BoletinesIndexPage
    {

        [Inject]
        public NavigationManager NavManager {get;set;} = default!;
        
        [Inject]
        public IMatToaster Toaster {get;set;} = default!;
        
        [Inject]
        public SicemService SicemService {get;set;} = default!;
        
        [Inject]
        public ILogger<BoletinesIndexPage> Logger {get;set;} = default!;

        [Inject]
        public NavigationManager NavigationManager1 {get;set;} = default!;

        [Inject]
        public IBoletinService BoletinService {get;set;} = default!;


        private bool busyDialog = false;
        private List<BoletinDTO> boletinesList {get;set;}
        private CultureInfo currentCultueInfo = new("es-MX");
        private BoletinDTO boletinSelected {get;set;}
        private List<IBoletinMensaje> messagesList {get;set;}
        private List<IBoletinDestinatario> destinatarios {get;set;}

        protected override async Task OnInitializedAsync()
        {
            await LoadBoletines();
        }

        private async Task LoadBoletines()
        {
            // * get the data
            this.boletinesList = (await this.BoletinService.GetBoletines()).ToList<BoletinDTO>();
        }

        private async Task BoletinInfoClick(BoletinDTO boletin)
        {
            this.busyDialog = true;
            await Task.Delay(100);

            this.boletinSelected = boletin;

            // * Load messages
            this.messagesList = (await this.BoletinService.GetMensajesBoletin(boletin.Id)).ToList();

            // * Load contacts
            this.destinatarios = (await this.BoletinService.GetDestinatariosBoletin(boletin.Id)).ToList();


            this.busyDialog = false;
            await InvokeAsync(StateHasChanged);
        }

        private async Task NewMessageClick()
        {
            await Task.CompletedTask;
            NavigationManager1.NavigateTo("/Boletines/Nuevo");
        }

    }

}