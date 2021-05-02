using System;
using System.Threading;
using Avalentini.Expensi.Mobile.Services.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Avalentini.Expensi.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartingPage : ContentPage
    {
        private readonly IAuthenticationService _authenticationService;
        public StartingPage()
        {
            _authenticationService = App.Container.AuthenticationService;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                var isAuth = await _authenticationService.TrySilentLoginAsync(new CancellationToken());
                if (isAuth)
                {
                    Navigation.InsertPageBefore(new LogoutPage(), this);
                    await Navigation.PopAsync ();
                }
            }
            catch
            {
                // Do nothing - the user isn't logged in
            }
            base.OnAppearing();
        }

        private async void LoginClicked (object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}