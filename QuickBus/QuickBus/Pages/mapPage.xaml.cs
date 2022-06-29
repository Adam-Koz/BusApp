using Xamarin.Forms;
using QuickBus.ViewModelFolder;
using Xamarin.Forms.GoogleMaps;
using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using QuickBus.Pages;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Threading;

namespace QuickBus
{
    
    public partial class mapPage : ContentPage
    {
        
        StopsClass StopsClass;
        List<string> list = new List<string>(); // url list

        CancellationTokenSource cts; // location token 

        public mapPage()
        {
            InitializeComponent();
            BindingContext = StopsClass = new StopsClass();
            MapStyleLoad();  
            Load();
        }

        // LOAD CUSTOM STYLE OF MAP 
        private void MapStyleLoad()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("QuickBus.MapStyle.json");
            string text = string.Empty;
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            map.MapStyle = MapStyle.FromJson(text);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(50.89794465121954, 15.722194574649057), Distance.FromKilometers(.5)));
        }


        // LOAD ALL BUS STOPS PIN   
        [Obsolete]
        public async void Load()
        {
            var content = await StopsClass.LoadStops();
            if (content != null)
            {
                foreach (var item in content)
                {
                    Pin pin = new Pin()
                    {
                        Type = PinType.Place,
                        Position = new Position(item.X, item.Y),
                        Label = item.Name,
                        Address = item.URL,
                        Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("StopBusIcon.png") : BitmapDescriptorFactory.FromView(new Image() 
                        { Source = "StopBusIcon.png", HeightRequest = 35, WidthRequest = 35 })
                    };
                    pin.Clicked += Pin_Clicked;
                    map.Pins.Add(pin);
                    list.Add(item.URL);
                }
            }
           
        }
        #region
        // GET CURRENT LOCATION OF USER
        public async Task GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(1));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Position pos = new Position(location.Latitude, location.Longitude);
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromKilometers(0.5)));
                    Pin locationPin = new Pin()
                    {
                        Type = PinType.Place,
                        Label = "user",
                        Position = pos,
                        Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("User.png") : BitmapDescriptorFactory.FromView(new Image()
                        { Source = "User.png", HeightRequest = 35, WidthRequest = 35 })
                    };
                    map.Pins.Add(locationPin);
                }
            }

            catch (Exception ex)
            {
               
            }
        }
        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
        #endregion

        private async void Pin_Clicked(object sender, EventArgs e)
        {
            var Url = (sender as Pin).Address;
            var Name = (sender as Pin).Label;
            await Navigation.PushAsync(new StopPage(Name, Url));
        }

        private  void Location_Button_Clicked(object sender, EventArgs e)
        {
            GetLocation();         
        }
    } 
        
        
    
}