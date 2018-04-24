namespace GeoMapasUlatina.Model
{
    using System;
    using Realms;

    public class Localizaciones : RealmObject
    {
        [PrimaryKey]
        public int LocalID { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Title { get; set; }
        public string Direccion { get; set; }
        public string Imagen { get; set; }
    }
}
