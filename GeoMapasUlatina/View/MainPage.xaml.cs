using System;
using System.Collections.Generic;
using GeoMapasUlatina.Model;

using Xamarin.Forms;

namespace GeoMapasUlatina.View
{
    public partial class MainPage : MasterDetailPage
    {
        

        public MainPage()
        {
            InitializeComponent();
            Title = "Menu";

            MasterPage.ListView.ItemSelected += OnItemSelected;
        }
        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as HomeMenuItem;
            if (item != null)
            {
                if (item.Title.Equals("Desconectarse"))
                {
                    App.IsUserLoggedIn = false;
                    Navigation.InsertPageBefore(new LoginPage(), this);
                    Navigation.PopAsync();
                }
                else
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                    MasterPage.ListView.SelectedItem = null;
                    IsPresented = false;
                }
            }
        }
    }
}