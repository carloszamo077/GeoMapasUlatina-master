using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GeoMapasUlatina.Model
{
    public class GeoLocaTions : RealmObject
    {
        [PrimaryKey]
        public int LocalID { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Title { get; set; }
        public string Direccion { get; set; }
        public byte[] Image { get; set; }
    }
}