using GeoMapasUlatina.Helpers;
using GeoMapasUlatina.Model;
using GeoMapasUlatina.View;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Realms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GeoMapasUlatina.ViewModel
{
    public class SignUpPageViewModel : INotifyPropertyChanged
    {
        public SignUpPageViewModel()
        {
            InitClass();
            InitCommands();
        }

        #region Atributos
        private string _usernameEntry;
        private string _fullNameEntry;
        private string _passwordEntry;
        private string _confirmPasswordEntry;
        private string _emailEntry;
        private string _message;
        ImageSource _imageSource;
        public Realm _realmUsers;
        MediaFile file;
        public ICommand GuardarCommand { get; set; }
        public ICommand ChangeImageCommand { get; set; }
        #endregion


        #region Propiedades

        public string usernameEntry
        {
            get
            {
                return _usernameEntry;
            }
            set
            {
                _usernameEntry = value;
                OnPropertyChanged("usernameEntry");
            }
        }
        public string fullNameEntry
        {
            get
            {
                return _fullNameEntry;
            }
            set
            {
                _fullNameEntry = value;
                OnPropertyChanged("fullNameEntry");
            }
        }
        public string passwordEntry
        {
            get
            {
                return _passwordEntry;
            }
            set
            {
                _passwordEntry = value;
                OnPropertyChanged("passwordEntry");
            }
        }
        public string confirmPasswordEntry
        {
            get
            {
                return _confirmPasswordEntry;
            }
            set
            {
                _confirmPasswordEntry = value;
                OnPropertyChanged("confirmPasswordEntry");
            }
        }
        public string emailEntry
        {
            get
            {
                return _emailEntry;
            }
            set
            {
                _emailEntry = value;
                OnPropertyChanged("emailEntry");
            }
        }
        public string message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged("message");
            }
        }
        public ImageSource ImageSource
        {
            set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                    PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(ImageSource)));
                }
            }
            get
            {
                return _imageSource;
            }
        }
        #endregion


        #region Metodos
        private void InitCommands()
        {
            GuardarCommand = new Command(guardar);
            ChangeImageCommand = new Command(changeImage);
        }

        private void InitClass()
        {
            _realmUsers = Helpers.UtilDB.GetInstanceRealm();
            ImageSource = "noimage.png";
        }

        private void guardar() {
            byte[] imageArray = null;

            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }
            else {
                Application.Current.MainPage.DisplayAlert("Aviso", "Debe se ingresar una imagen!!!", "Ok");
                return;
            }

            var NewUser = new User()
            {
                Username = usernameEntry,
                FullName = fullNameEntry,
                Password = passwordEntry,
                ConfirmPassword = confirmPasswordEntry,
                Email = emailEntry,
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
                catch (Exception ex)
                {
                    message = "Error, usuario ya existe!!!";
                    return;
                }

                /*var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    App.IsUserLoggedIn = true;
                    Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                    await Navigation.PopToRootAsync();
                }*/
                Application.Current.MainPage.DisplayAlert("Aviso", "Se creo el usuario correctamente!!!", "Ok");
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                message = "Fallo el registro";
                return;
            }
        }

        private async void changeImage()
        {
            {
                await CrossMedia.Current.Initialize();

                if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
                {
                    var source = await Application.Current.MainPage.DisplayActionSheet(
                    "De donde tomará la imagen?",
                    "Cancel",
                    null,
                    "From Gallery",
                    "From Camera");

                    //  var source = await dialogService.ShowImageOptions();

                    if (source == "Cancel")
                    {
                        file = null;
                        return;
                    }

                    if (source == "From Camera")
                    {
                        file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                        );
                    }
                    else
                    {
                        file = await CrossMedia.Current.PickPhotoAsync();
                    }
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                }

                if (file != null)
                {
                    ImageSource = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });
                }
            }
        }

        bool AreDetailsValid(User user)
        {
            return (!string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.Password) &&
                !string.IsNullOrWhiteSpace(user.ConfirmPassword) &&
                 user.Password.Equals(user.ConfirmPassword) &&
                !string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@"));
        }

        #endregion


        #region Eventos

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) // if there is any subscribers 
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}