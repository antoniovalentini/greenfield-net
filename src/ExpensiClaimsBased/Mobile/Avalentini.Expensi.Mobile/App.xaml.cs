using System;
using System.Threading;
using System.Threading.Tasks;
using Avalentini.Expensi.Mobile.Models;
using Avalentini.Expensi.Mobile.Pages;
using Avalentini.Expensi.Mobile.Services.Auth;
using Xamarin.Forms;

namespace Avalentini.Expensi.Mobile
{
    public partial class App : Application
    {
        //public static User UserProp { get; set; } = new User ();
        public static IoCContainer Container { get; set; }

        public App()
        {
            InitializeComponent();

            IAuthenticationService authService;
            //try
            //{
            //    authService = new AuthenticationService(new OfflineAuthenticationService());
            //    using (var source = new CancellationTokenSource(TimeSpan.FromSeconds(15)))
            //    {
            //        authService.Init(source.Token).GetAwaiter().GetResult();
            //    }
            //}
            //catch (TaskCanceledException)
            //{
            //    Console.WriteLine("Task was cancelled");
            //    authService = new OfflineAuthenticationService();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    authService = new OfflineAuthenticationService();
            //}
            authService = new AuthenticationService(new OfflineAuthenticationService());
            Container = new IoCContainer(authService);
            MainPage = new NavigationPage(new StartingPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    public class IoCContainer
    {
        public IoCContainer(IAuthenticationService authService)
        {
            AuthenticationService = authService;
        }

        public IAuthenticationService AuthenticationService { get; }
    }
}
