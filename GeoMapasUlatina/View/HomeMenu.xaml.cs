using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GeoMapasUlatina.Model;
using Realms;
using System.IO;

namespace GeoMapasUlatina.View
{
  
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeMenu : ContentPage
    {
        private Realm _realmUser;
        public ListView ListView { get { return listView; } }
        //public Frame Frame { get { return frame; } }
        public Image Image { get { return image;  } }
        public Label Label { get { return fullname; } }
        private string _fullname;
        private string _username;
        ImageSource _imageSource;
        public HomeMenu()
        {
            InitializeComponent();
            _realmUser = Helpers.UtilDB.GetInstanceRealm(); //Inicializo la base de datos
            _fullname = (string)GetApplicationCurrentProperty("fullname");
            _username = (string)GetApplicationCurrentProperty("username");
            GeoMapasUlatina.Model.User userN = (GeoMapasUlatina.Model.User)_realmUser.Find("User", _username);

            var stream = new MemoryStream(userN.Image);
            _imageSource = ImageSource.FromStream(() => stream);
            image.Source = _imageSource;
            image.HeightRequest = 240;
            image.WidthRequest = 250;

            //image.Source = 
            var masterPageItems = new List<HomeMenuItem>();
            masterPageItems.Add(new HomeMenuItem
            {
                Title = "Home",
                IconSource = "home.png",
                TargetType = typeof(HomePage)
            });
            masterPageItems.Add(new HomeMenuItem
            {
                Title = "Agregar Restaurante",
                IconSource = "address.png",
                TargetType = typeof(GeoMapsPage)
            });
            masterPageItems.Add(new HomeMenuItem
            {
                Title = "Fotos",
                IconSource = "camera.png",
                TargetType = typeof(Camera)
            });
            masterPageItems.Add(new HomeMenuItem
            {
                IconSource = "logout.png",
                Title = "Desconectarse"
            });

            listView = new ListView
            {
                ItemsSource = masterPageItems,
                //Footer = { },
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid { Padding = new Thickness(5, 10) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    var image = new Image();
                    image.SetBinding(Image.SourceProperty, "IconSource");
                    var label = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    label.SetBinding(Label.TextProperty, "Title");

                    grid.Children.Add(image);
                    grid.Children.Add(label, 1, 0);

                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };

            fullname.Text = _fullname;
            fullname.TextColor = Color.Navy;
            fullname.FontSize = 20;
            fullname.HorizontalOptions = LayoutOptions.CenterAndExpand;

            Title = "Menú";
            Padding = new Thickness(0, 40, 0, 0);
            Content = new StackLayout
            {
                Children = { image, fullname, listView }
            };
        }
        public static object GetApplicationCurrentProperty(string propertyKey)
        {
            object retValue = null;
            IDictionary<string, object> properties = Application.Current.Properties;
            if (properties.ContainsKey(propertyKey))
            {
                retValue = properties[propertyKey];
            }
            return retValue;
        }
    }
       
    }
