using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GeoMapasUlatina.Model
{
    public class AlbumModel : RealmObject
    {
        public int imageId { get; set; }
        public string imageName { get; set; }
        public byte[] Image { get; set; }
    }
}