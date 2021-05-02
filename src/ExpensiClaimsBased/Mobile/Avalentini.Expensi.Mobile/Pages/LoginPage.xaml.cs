using System;
using System.Threading;
using Avalentini.Expensi.Mobile.Models;
using Avalentini.Expensi.Mobile.Services.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Avalentini.Expensi.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly IAuthenticationService _authenticationService;
        public static User CurrentUser { get; set; } = new User ();

        public LoginPage()
        {
            _authenticationService = App.Container.AuthenticationService;
            InitializeComponent();
            FormLayout.BindingContext = CurrentUser;
        }

        private async void LoginClicked (object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty (CurrentUser.Username) || string.IsNullOrEmpty (CurrentUser.Password))
            {
                await DisplayAlert ("WARNING", "Please insert username and password", "Back");
                return;
            }

            var isAuth = await _authenticationService.LoginAsync(CurrentUser.Username, CurrentUser.Password, new CancellationToken());
            if (!isAuth)
            {
                await DisplayAlert ("WARNING", "Login invalid", "Back");
                return;
            }
            Navigation.InsertPageBefore(new LogoutPage(), this);
            await Navigation.PopAsync();
        }
    }
}