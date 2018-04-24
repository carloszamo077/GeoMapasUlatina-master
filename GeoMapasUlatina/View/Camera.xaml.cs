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
	public partial class Camera : ContentPage
	{
		public Camera ()
		{
			InitializeComponent ();
            BindingContext = CameraViewModel.GetInstance();

            Title = "Camara";
        }
	}
}