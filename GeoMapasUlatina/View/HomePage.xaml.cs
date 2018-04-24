using System;
using System.Collections.Generic;
using GeoMapasUlatina.ViewModel;
using GeoMapasUlatina.Model;
using Xamarin.Forms;

namespace GeoMapasUlatina.View
{
    public partial class HomePage : TabbedPage
    {
       // public List<Ubicaciones> tempdata;
        public HomePage()
        {
            InitializeComponent();

            BindingContext = HomePageViewModel.GetInstance();

            Title = "Restaurantes";
            
            //lstUbic.ItemsSource = tempdata;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //thats all you need to make a search  

            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                //lstUbic.ItemsSource = lstUbic.ItemsSource;
            }

            else
            {
                //lstUbic.ItemsSource = lstUbic.ItemsSource.Where(x => x.Name.StartsWith(e.NewTextValue));
            }
        }

     
    }
}
