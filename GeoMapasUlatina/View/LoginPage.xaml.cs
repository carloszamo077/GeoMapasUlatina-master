using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoMapasUlatina.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoMapasUlatina.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        private Realm _realm;
        public LoginPage ()
		{
			InitializeComponent ();
            Title = "Restaurantes";
            _realm = Helpers.UtilDB.GetInstanceRealm(); //Inicializo la base de datos
        }
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var user = new GeoMapasUlatina.Model.User
            {
                Username = usernameEntry.Text,
                Password = passwordEntry.Text
            };

            var isValid = AreCredentialsCorrect(user);
            if (isValid)
            {
                App.IsUserLoggedIn = true;

                /*var mainPage = new MainPage();
                mainPage.BindingContext = usernameEntry.Text;
                await Navigation.PushAsync(mainPage);*/

                Navigation.InsertPageBefore(new MainPage(),this);
                await Navigation.PopAsync();
                //await Navigation.PushAsync(new MainPage());
            }
            else
            {
                messageLabel.Text = "Usuario o contraseña incorrecta!!!";
                passwordEntry.Text = string.Empty;
            }
        }

        bool AreCredentialsCorrect(GeoMapasUlatina.Model.User user)
        {
            GeoMapasUlatina.Model.User userN = (GeoMapasUlatina.Model.User) _realm.Find("User", user.Username);

            if (userN == null)
            {
                /*if (user.Username == GeoMapasUlatina.Model.Constants.Username && user.Password == GeoMapasUlatina.Model.Constants.Password)
                {
                    return true;
                }*/
                return false;
            }
            else if(user.Username == userN.Username && user.Password == userN.Password) {
                SetApplicationCurrentProperty("username", userN.Username);
                SetApplicationCurrentProperty("fullname", userN.FullName);
                return true;
            }
            return false;
        }
        public static void SetApplicationCurrentProperty(string propertyKey, object obj)
        {
            IDictionary<string, object> properties = Application.Current.Properties;
            if (properties.ContainsKey(propertyKey))
            {
                properties[propertyKey] = obj;
            }
            else
            {
                properties.Add(propertyKey, obj);
            }
        }
    }
}