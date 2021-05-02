using System;
using Avalentini.Expensi.Mobile.Services.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Avalentini.Expensi.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogoutPage : ContentPage
    {
        private readonly IAuthenticationService _authenticationService;
        public LogoutPage()
        {
            _authenticationService = App.Container.AuthenticationService;
            InitializeComponent();
        }

        private async void LogoutClicked (object sender, EventArgs e)
        {
            // TODO: _authenticationService.Logout();
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}