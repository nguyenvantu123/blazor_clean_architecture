using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using MudBlazor;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using BlazorHero.CleanArchitecture.Application.Models.Chat;

namespace BlazorHero.CleanArchitecture.Client.Pages.Authentication
{
    public partial class Register
    {
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private RegisterRequest _registerUserModel = new();

        private async Task SubmitAsync()
        {
            var response = await _userManager.RegisterUserAsync(_registerUserModel);
            if (response.Succeeded)
            {
                //_snackBar(response.Messages[0], Severity.Success);
                _snackBar.Notify(new Radzen.NotificationMessage { Severity = Radzen.NotificationSeverity.Success, Detail = response.Messages[0], Duration = 4000 });
                _navigationManager.NavigateTo("/login");
                _registerUserModel = new RegisterRequest();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    //_snackBar.Add(message, Severity.Error);
                    _snackBar.Notify(new Radzen.NotificationMessage { Severity = Radzen.NotificationSeverity.Error, Detail = message, Duration = 4000 });
                }
            }
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }
    }
}