using System;
using System.Collections.Generic;
using System.Linq;
using GeoMapasUlatina.Model;
using GeoMapasUlatina.ViewModel;
using Plugin.Geolocator;
using Realms;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using System.ComponentModel;
using System.Windows.Input;
using UIKit;
using Foundation;
using System.IO;
using System.Text;
using GeoMapasUlatina.Helpers;

namespace GeoMapasUlatina.View
{
    public partial class GeoMapsPage : ContentPage
    {
        #region Atributos
        public Realm _realm;
        public ImageSource _imagenUbicacion;
        #endregion


        public GeoMapsPage()
        {
            InitializeComponent();

           // CameraButton.Clicked += CameraButton_Clicked;

            BindingContext = HomePageViewModel.GetInstance();

            _realm = Helpers.UtilDB.GetInstanceRealm();

            Title = "Agregar Restaurante";
            _imagenUbicacion = "noimage.jpeg";

        }


        private async void Cargar_PIN(object sender, EventArgs e)
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync();

            var lat = position.Latitude;

            var lon = position.Longitude;

            var pin = new Pin
            {

                Position = new Position(lat, lon),
                Label = "Mall San Pedro",
                Address = "San Pedro, Costa Rica"
            };

            MyMap.Pins.Add(pin);
        }

        private async void Handle_Clicked_Ubicame(object sender, System.EventArgs e)
        {
            var locator = CrossGeolocator.Current;

            if (locator.IsGeolocationEnabled) {
               /* await Xamarin.Forms.PlatformConfiguration.Android.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
                var url = new NSUrl("prefs:root=Settings");
                UIApplication.SharedApplication.OpenUrl(url);
                var root = new UINavigationController();
                root.PushViewController(new Uri("prefs:root=General"), true); */
            }

            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync();

            var lat = position.Latitude;

            var lon = position.Longitude;

            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lon), Distance.FromKilometers(.5)));

            var pin = new Pin
            {
                Position = new Position(lat, lon),
                Label = "Mi Ubicación",
                Type = PinType.Place
            };

            MyMap.Pins.Add(pin);

            Lat.Text = Convert.ToString(lat);
            Lon.Text = Convert.ToString(lon);
        }

        void Handle_Clicked_Guardar(object sender, System.EventArgs e)
        {
            var detailsValid = AreDetailsValid(Titulo.Text, Direccion.Text);
            if (detailsValid)
            {
                var id = _realm.All<GeoLocaTions>().Count() + 1;

                byte[] imageArray = null;

                if (HomePageViewModel.GetInstance().getFile != null)
                {
                    imageArray = FilesHelper.ReadFully(HomePageViewModel.GetInstance().getFile.GetStream());
                    HomePageViewModel.GetInstance().getFile.Dispose();
                }
                else {
                    Application.Current.MainPage.DisplayAlert("Aviso", "Debe se ingresar una imagen!!!", "Ok");
                    return;
                }

                var NewUbicacion = new GeoLocaTions()
                {
                    LocalID = id,
                    Latitud = Convert.ToDouble(Lat.Text.Replace('.', ',')),
                    Longitud = Convert.ToDouble(Lon.Text.Replace('.', ',')),
                    Title = Titulo.Text,
                    Direccion = Direccion.Text,
                    Image = imageArray
                };

                _realm.Write(() =>
                {
                    NewUbicacion = _realm.Add(NewUbicacion);
                });
                Application.Current.MainPage.DisplayAlert("Aviso", "Se guardo la ubicación correctamente!!!", "Ok");

                Navigation.InsertPageBefore(new HomePage(), this);
                Navigation.PopAsync();
            }
            else {
                messageLabel.Text = "Error guardando la ubicación";
            }
        }

        bool AreDetailsValid(string tit, string dir)
        {
            return (!string.IsNullOrWhiteSpace(tit) && !string.IsNullOrWhiteSpace(dir));
        }

        private async void Handle_Clicked_Camera(object sender, EventArgs e)
        {
            

        }
        private void MapWithPushpins_MouseDoubleClick(object sender, EventArgs e)
        {
            // Disables the default mouse double-click action.
            string n = e.ToString();

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
           // Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            //Location pinLocation = MyMap.(mousePosition);

            // The pushpin to add to the map.
            //Pushpin pin = new Pushpin();
            //pin.Location = pinLocation;

            // Adds the pushpin to the map.
            //myMap.Children.Add(pin);
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var locator = CrossGeolocator.Current;
            if (!locator.IsGeolocationEnabled)
            {
               await Application.Current.MainPage.DisplayAlert("Error", "No esta activa la ubicación!!!","Ok");
            }
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();
            var lat = position.Latitude;
            var lon = position.Longitude;

            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lon), Distance.FromKilometers(.5)));

            var pin = new Pin
            {
                Position = new Position(lat, lon),
                Label = "Mi Ubicación",
                Type = PinType.Generic
            };

            pin.Clicked += async (sender, e) => { await DisplayAlert("Pin", "Mi Ubicación", "Ok"); };
            MyMap.Pins.Add(pin);

            Lat.Text = Convert.ToString(lat);
            Lon.Text = Convert.ToString(lon);

            /* var latN = Convert.ToDouble(Lat.Text.Replace('.', ','));
             var lonN = Convert.ToDouble(Lon.Text.Replace('.', ','));
           
             MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latN, lonN), Distance.FromKilometers(.5)));
             var pin = new Pin
             {

                 Position = new Position(latN, lonN),
                 Label = Tit.Text,
                 Address = Dir.Text
             };
             MyMap.Pins.Add(pin);*/
        }
    }
}
