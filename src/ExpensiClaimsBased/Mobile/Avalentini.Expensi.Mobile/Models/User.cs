using Xamarin.Forms;

namespace Avalentini.Expensi.Mobile.Models
{
    public class User
    {
        private const string UsernameKey = "username";
        private const string PasswordKey = "password";

        public string Username
        {
            get =>
                Application.Current.Properties.ContainsKey (UsernameKey) 
                    ? Application.Current.Properties[UsernameKey].ToString () 
                    : "";
            set
            {
                Application.Current.Properties[UsernameKey] = value;
                Application.Current.SavePropertiesAsync ();
            }
        }

        public string Password
        {
            get => Application.Current.Properties.ContainsKey (PasswordKey) 
                ? Application.Current.Properties[PasswordKey].ToString () 
                : "";
            set
            {
                Application.Current.Properties[PasswordKey] = value;
                Application.Current.SavePropertiesAsync ();
            }
        }
    }
}
