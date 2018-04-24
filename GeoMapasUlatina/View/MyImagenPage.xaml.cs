using GeoMapasUlatina.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoMapasUlatina.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyImagenPage : ContentPage
    {
        public MyImagenPage()
        {
            InitializeComponent();
            BindingContext = CameraViewModel.GetInstance();

            Title = "Imagen";

            ToolbarItems.Add(new ToolbarItem
            {
                Text = "<",
                Order = ToolbarItemOrder.Default,
                Command = new Command(() =>
                {
                    Application.Current.MainPage = new MainPage();
                    //Navigation.InsertPageBefore(new HomePage(), this);
                    //Navigation.PopAsync();
                })
            });
        }
    }
}