using GeoMapasUlatina.Helpers;
using GeoMapasUlatina.Model;
using GeoMapasUlatina.View;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GeoMapasUlatina.ViewModel
{
    public class CameraViewModel : INotifyPropertyChanged
    {
        #region Singleton
        private static CameraViewModel instance = null;
        private CameraViewModel()
        {
            InitClass();
            InitCommands();
        }
        public static CameraViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new CameraViewModel();
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
        public ICommand GuardarCommand { get; set; }
        public ICommand ChangeImageCommand { get; set; }
        public ICommand VerImagenCommand { get; set; }
        private List<AlbumModel> _lstImagenes = new List<AlbumModel>();
        private AlbumModel _ImagenActual { get; set; }
        ImageSource _imageSource;
        ImageSource _imageSource1;
        MediaFile file;
        public Realm _realm;
        private bool _isRefreshing = false;

        #endregion

        #region Propiedades

        public ImageSource ImageSource {
            set {
                if (_imageSource != value) {
                    _imageSource = value;
                    PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(ImageSource)));
                }
            }
            get {
                return _imageSource;
            }
        }

        public ImageSource ImageSource1
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
        }

        public List<AlbumModel> lstImagenes
        {
            get
            {
                return _lstImagenes;
            }
            set
            {
                _lstImagenes = value;
                OnPropertyChanged("lstImagenes");
            }

        }


        public AlbumModel ImagenActual
        {
            get
            {
                return _ImagenActual;
            }
            set
            {
                _ImagenActual = value;
                OnPropertyChanged("ImagenActual");
            }

        }

        #endregion
        #region Metodos
        private void InitCommands()
        {
            GuardarCommand = new Command(guardarImagen);
            ChangeImageCommand = new Command(changeImage);
            VerImagenCommand = new Command<int>(VerImagen);
        }

        private void InitClass() {
            _realm = Helpers.UtilDB.GetInstanceRealm();
            ImageSource = "noimage.png";
            cargarImagenes();
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
                    cargarImagenes();

                    IsRefreshing = false;
                });
            }
        }

        private void cargarImagenes() {
            lstImagenes = _realm.All<AlbumModel>().ToList();
        }
        private void VerImagen(int imageId)
        {
            ImagenActual = lstImagenes.Where(x => x.imageId == imageId).FirstOrDefault();

            var stream1 = new MemoryStream(ImagenActual.Image);

            ImageSource1 = ImageSource.FromStream(() => stream1);

           // Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
            Application.Current.MainPage = new NavigationPage(new MyImagenPage());
        }

        private void guardarImagen()
        {
            byte[] imageArray = null;

            if (file != null) {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }

            var id = _realm.All<AlbumModel>().Count() + 1;

            var newImagen = new AlbumModel
            {
                imageId = id,
                imageName = "Foto",
                Image = imageArray
            };

            _realm.Write(() =>
            {
                newImagen = _realm.Add(newImagen); 
            }
            );

            var count = _realm.All<AlbumModel>().Count();


            if (count == id)
            {
                ImageSource = "noimage.png";
                Application.Current.MainPage.DisplayAlert("Alerta", "La imagen se ha guardado",
                    "Ok");
            }
            else {
                Application.Current.MainPage.DisplayAlert("Alerta", "La imagen no se ha guardado!!!",
                    "Ok");
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