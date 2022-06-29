using Xamarin.Forms.GoogleMaps;
using System.Collections.Generic;
namespace QuickBus
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }
}
