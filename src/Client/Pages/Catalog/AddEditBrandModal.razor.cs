using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Brand;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class AddEditBrandModal
    {
        [Inject] private IBrandManager BrandManager { get; set; }

        [Parameter] public AddEditBrandCommand AddEditBrandModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await BrandManager.SaveAsync(AddEditBrandModel);
            if (response.Succeeded)
            {
                //_snackBar.Add(response.Messages[0], Severity.Success);
                _snackBar.Notify(new Radzen.NotificationMessage { Severity = Radzen.NotificationSeverity.Success, Detail = response.Messages[0], Duration = 4000 });
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    //_snackBar.Add(message, Severity.Error);
                    _snackBar.Notify(new Radzen.NotificationMessage { Severity = Radzen.NotificationSeverity.Error, Detail = message, Duration = 4000 });

                }
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }
    }
}