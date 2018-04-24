using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GeoMapasUlatina.Model
{
    public class Ubicaciones
    {

        public static async Task<ObservableCollection<Localizaciones>> CargaUbicaciones()
        {

            ObservableCollection<Localizaciones> lstUbicas = new ObservableCollection<Localizaciones>();
            
            lstUbicas.Add(new Localizaciones { LocalID = 1, Imagen = "mcdonald.png", Title = "McDonalds", Direccion = "Provincia de Alajuela, Grecia", Latitud = 10.0775551, Longitud = -84.31710379999998 });
            lstUbicas.Add(new Localizaciones { LocalID = 2, Imagen="icon.png", Title = "Café Del Patio Restaurante Gourmet", Direccion = "Av 4, Provincia de Alajuela, Grecia, Costa Rica",Latitud = 10.0712719, Longitud = -84.31234319999999 });       
            lstUbicas.Add(new Localizaciones { LocalID = 3, Imagen = "icon.png", Title = "Restaurante Pizza Macho", Direccion = "Calle 2 Lucas Fernández, Alajuela Province, Grecia",Latitud = 10.0749398, Longitud = -84.31258389999999 });
            lstUbicas.Add(new Localizaciones { LocalID = 4, Imagen = "pizzahut.png", Title = "Pizza Hut", Direccion = "Alajuela Province, Grecia", Latitud = 10.0775759, Longitud = -84.3171749 });
            lstUbicas.Add(new Localizaciones { LocalID = 5, Imagen = "icon.png", Title = "Bar Rest Tarire", Direccion = "Calle 1, Provincia de Alajuela, Grecia, Costa Rica", Latitud = 10.0740914, Longitud = -84.31052361111111 });
            lstUbicas.Add(new Localizaciones { LocalID = 6, Imagen = "icon.png", Title = "Los Pira", Direccion = "Provincia de Alajuela, Grecia, Costa Rica", Latitud = 10.0721746, Longitud = -84.31173590000003 });
            lstUbicas.Add(new Localizaciones { LocalID = 7, Imagen = "subway.png", Title = "Subway", Direccion = "Av. 2 Ismael Valerio, Provincia de Alajuela, Grecia, Costa Rica", Latitud = 10.07220703722195, Longitud = -84.31199356913567 });
            lstUbicas.Add(new Localizaciones { LocalID = 8, Imagen = "icon.png", Title = "Delicias", Direccion = "Av. 2 Ismael Valerio, Provincia de Alajuela, Grecia, Costa Rica", Latitud = 10.07209744105017, Longitud = -84.31231141090393 });
            lstUbicas.Add(new Localizaciones { LocalID = 9, Imagen = "kfc.png", Title = "KFC", Direccion = "Costado oeste del Parque Central de Grecia, Calle 2 Lucas Fernández, Provincia de Alajuela, Grecia", Latitud = 10.077467, Longitud = -84.31721399999998 });
            lstUbicas.Add(new Localizaciones { LocalID = 10, Imagen = "tacobell.png", Title = "Taco Bell", Direccion = "Alajuela Province, Grecia, Costa Rica", Latitud = 10.0773237, Longitud = -84.31692750000002 });

            return lstUbicas;

        }



    }
}
