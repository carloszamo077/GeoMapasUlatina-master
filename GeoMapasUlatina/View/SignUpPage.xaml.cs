using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GeoMapasUlatina.Model;
using Realms;
using Plugin.Media.Abstractions;
using GeoMapasUlatina.Helpers;
using System.Windows.Input;
using GeoMapasUlatina.ViewModel;

namespace GeoMapasUlatina.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        /*public Realm _realmUsers;
        MediaFile file;
        public ICommand ChangeImageCommand { get; set; }
        public ICommand VerImagenCommand { get; set; }
        private ImageSource _imageSource;*/

        public SignUpPage()
        {
            InitializeComponent();
            BindingContext = new SignUpPageViewModel();

            Title = "Registro de Usuario";
            // _realmUsers = Helpers.UtilDB.GetInstanceRealm();
        }
        /*
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            byte[] imageArray = null;

            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }
            var NewUser = new User()
            {
                Username = usernameEntry.Text,
                FullName = fullNameEntry.Text,
                Password = passwordEntry.Text,
                ConfirmPassword = confirmPasswordEntry.Text,
                Email = emailEntry.Text,
                Image = imageArray
            };

            // Sign up logic goes here

            var signUpSucceeded = AreDetailsValid(NewUser);
            if (signUpSucceeded)
            {
                try
                {
                    _realmUsers.Write(() =>
                    {
                        NewUser = _realmUsers.Add(NewUser);
                    });
                }
                catch (Exception ex) {
                    messageLabel.Text = "Error, usuario ya existe!!!";
                    return;
                }

                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    App.IsUserLoggedIn = true;
                    Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                    await Navigation.PopToRootAsync();
                }
            }
            else
            {
                messageLabel.Text = "Fallo el registro";
            }
        }

        bool AreDetailsValid(User user)
        {
            return (!string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.Password) &&
                !string.IsNullOrWhiteSpace(user.ConfirmPassword) &&
                 user.Password.Equals(user.ConfirmPassword) &&
                !string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@"));
        }*/
    }
}