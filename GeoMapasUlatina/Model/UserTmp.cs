using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GeoMapasUlatina.Model
{
    public class UserTmp 
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}