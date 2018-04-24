using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GeoMapasUlatina.Model;
using GeoMapasUlatina.View;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Realms;
using Xamarin.Forms;

namespace GeoMapasUlatina.ViewModel
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        #region Singleton
        private static HomePageViewModel instance = null;

        private HomePageViewModel()
        {
            InitClass();
            InitCommands();
        }

        public static HomePageViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new HomePageViewModel();
            }

            return instance;
        }

        public static void DeleteInstance()
        {
            if (instance != null)
            {
                instance = null;
            }
        }
        #endregion


        #region Atributos
        private Realm _realm;
        private Realm _realmUsers;

        private ObservableCollection<Localizaciones> _lstUbicaciones = new ObservableCollection<Localizaciones>();
        private List<GeoLocaTions> _lstUbicacionesDB = new List<GeoLocaTions>();
        private ObservableCollection<GeoLocaTionsTmp> _lstUbicacionesTmp = new ObservableCollection<GeoLocaTionsTmp>();
        private List<User> _lstUsuariosDB = new List<User>();
        private ObservableCollection<UserTmp> _lstUsuarios = new ObservableCollection<UserTmp>();
        private Localizaciones _UbicacionActual { get; set; }
        public ICommand VerUbicacionCommand { get; set; }
        public ICommand VerUsuariosCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand ChangeImageCommand { get; set; }
        public ICommand NavegarWazeCommand { get; set; }
        private User _UsuarioActual { get; set; }
        ImageSource _imageSource;
        //ImageSource _imageSource1;
        MediaFile file { get; set; }
        private Image imagen { get;  set; }
        private bool _isRefreshing = false;
        #endregion

        #region Propiedades
        public MediaFile getFile
        {
            get { return file; }
        }
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    List<GeoLocaTions> listGeo = cargarRestaurantesBD();
                    cargarRestaurantes(listGeo);
                    await cargarUbicaciones();
                    //List<User> list = cargarUsuariosBD();
                    //cargarUsuarios(list);

                    IsRefreshing = false;
                });
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

        /*public ImageSource ImageSource1
        {
            set
            {
                if (_imageSource1 != value)
                {
                    _imageSource1 = value;
                    PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(ImageSource1)));
                }
            }
            get
            {
                return _imageSource1;
            }
        }*/

        public Localizaciones UbicacionActual
        {
            get
            {
                return _UbicacionActual;
            }
            set
            {
                _UbicacionActual = value;
                OnPropertyChanged("UbicacionActual");
            }
        }

        public List<GeoLocaTions> lstUbicacionesDB
        {
            get
            {
                return _lstUbicacionesDB;
            }
            set
            {
                _lstUbicacionesDB = value;
                OnPropertyChanged("lstUbicacionesDB");
            }

        }

        public List<User> lstUsuariosDB
        {
            get
            {
                return _lstUsuariosDB;
            }
            set
            {
                _lstUsuariosDB = value;
                OnPropertyChanged("lstUsuariosDB");
            }
        }

        public ObservableCollection<UserTmp> lstUsuarios
        {
            get
            {
                return _lstUsuarios;
            }
            set
            {
                _lstUsuarios = value;
                OnPropertyChanged("lstUsuarios");
            }

        }

        public ObservableCollection<GeoLocaTionsTmp> lstUbicacionesTmp
        {
            get
            {
                return _lstUbicacionesTmp;
            }
            set
            {
                _lstUbicacionesTmp = value;
                OnPropertyChanged("lstUbicacionesTmp");
            }

        }
        public ObservableCollection<Localizaciones> lstUbicaciones
        {
            get
            {
                return _lstUbicaciones;
            }
            set
            {
                _lstUbicaciones = value;
                OnPropertyChanged("lstUbicaciones");
            }

        }

        public User UsuarioActual
        {
            get
            {
                return _UsuarioActual;
            }
            set
            {
                _UsuarioActual = value;
                OnPropertyChanged("UsuarioActual");
            }
        }
        #endregion

        #region Metodos

        private void InitCommands()
        {
            VerUbicacionCommand = new Command<int>(VerUbicacion);
            VerUsuariosCommand = new Command<string>(VerUsuarios);
            NavegarWazeCommand = new Command<int>(NavegarWaze);
            GuardarCommand = new Command(guardar);
            ChangeImageCommand = new Command(changeImage);

        }
        private void guardar()
        {

        }
        private async Task cargarUbicaciones()
        {
            lstUbicaciones = await Ubicaciones.CargaUbicaciones();
        }
        private List<GeoLocaTions> cargarRestaurantesBD()
        {
           return lstUbicacionesDB = _realm.All<GeoLocaTions>().ToList();
        }
        //Metodo que carga la lista de usuarios de BD en la lista temporal, para poder mostrar la imagen
        private void cargarRestaurantes(List<GeoLocaTions> lista)
        {
            lstUbicacionesTmp = new ObservableCollection<GeoLocaTionsTmp>();
            foreach (var geo in lista)
            {
                var stream1 = new MemoryStream(geo.Image);
                var newGeo = new GeoLocaTionsTmp()
                {
                   LocalID = geo.LocalID,
                    Latitud = geo.Latitud,
                    Longitud = geo.Longitud,
                    Title = geo.Title,
                    Direccion = geo.Direccion,
                    ImageSource = ImageSource.FromStream(() => stream1)
                };
                lstUbicacionesTmp.Add(newGeo);
            }
        }
        private List<User> cargarUsuariosBD()
        {
             return lstUsuariosDB = _realmUsers.All<User>().ToList();
        }
        //Metodo que carga la lista de usuarios de BD en la lista temporal, para poder mostrar la imagen
        private void cargarUsuarios(List<User> lista)
        {
            lstUsuarios = new ObservableCollection<UserTmp>();
            foreach (var user in lista)
            {
                var stream1 = new MemoryStream(user.Image);
                var newUser = new UserTmp()
                {
                    Username = user.Username,
                    FullName = user.FullName,
                    Password = user.Password,
                    ConfirmPassword = user.ConfirmPassword,
                    Email = user.Email,
                    ImageSource = ImageSource.FromStream(() => stream1)
                 };
                lstUsuarios.Add(newUser);
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
 
        private void NavegarWaze(int LocalId)
        {
            switch (LocalId)
            {
                case 1:
                    Device.OpenUri(new Uri("https://waze.com/ul?ll=10.0775551,-84.31710379999998&navigate=yes"));
                    break;
                case 2:
                    Device.OpenUri(new Uri("https://waze.com/ul?ll=10.0712719,-84.31234319999999&navigate=yes"));
                    break;
                case 3:
                    Device.OpenUri(new Uri("https://waze.com/ul?ll=10.0749398,-84.31258389999999&navigate=yes"));
                    break;
                case 4:
                    Device.OpenUri(new Uri("https://waze.com/ul?ll=10.0775759,-84.3171749&navigate=yes"));
                    break;
                case 5:
                    Device.OpenUri(new Uri("https://waze.com/ul?ll=10.0740914,-84.31052361111111&navigate=yes"));
                    break;
                case 6:
                    Device.OpenUri(new Uri("https://waze.com/ul?ll=10.0721746,-84.31173590000003&navigate=yes"));
                    break;
                case 7:
                    Device.OpenUri(new Uri("https://waze.com/ul?ll=10.07220703722195,-84.31199356913567&navigate=yes"));
                    break;
                case 8:
                    Device.OpenUri(new Uri("https://waze.com/ul?ll=10.07209744105017,-84.31231141090393&navigate=yes"));
                    break;
                case 9:
                    Device.OpenUri(new Uri("https://waze.com/ul?ll=10.077467,-84.31721399999998&navigate=yes"));
                    break;
                case 10:
                    Device.OpenUri(new Uri("https://waze.com/ul?ll=10.0773237,-84.31692750000002&navigate=yes"));
                    break;
                default:
                    break;
            }
        }

        private void VerUbicacion(int LocalId)
        {
            //UbicacionActual = lstUbicaciones.Where(x => x.LocalID == LocalId).FirstOrDefault();

            /*((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(new GeoMapsPage());
            ((MainPage)Application.Current.MainPage).Detail.Navigation.PushAsync(new GeoMapsPage());*/

            //Application.Current.MainPage = new NavigationPage(new GeoMapsPage());

            GeoLocaTions geo = lstUbicacionesDB.Where(x => x.LocalID == LocalId).FirstOrDefault();
            var lat = geo.Latitud;
            var lon = geo.Longitud;
            var uri = "https://waze.com/ul?ll=" + lat + "," + lon + "&navigate=yes";
            Device.OpenUri(new Uri(uri));
            //Device.OpenUri(new Uri("https://waze.com/ul?ll=10.0775551,-84.31710379999998&navigate=yes"));

        }

        private void VerUsuarios(string Username)
        {
          
        }

        private async Task InitClass()
        {
            _realm = Helpers.UtilDB.GetInstanceRealm(); //Inicializo la base de datos
            _realmUsers = Helpers.UtilDB.GetInstanceRealm(); //Inicializo la base de datos
            ImageSource = "noimage.png";

            List<GeoLocaTions> listGeo = cargarRestaurantesBD();
            cargarRestaurantes(listGeo);
            await cargarUbicaciones();
            List<User> list = cargarUsuariosBD();
            cargarUsuarios(list);
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
