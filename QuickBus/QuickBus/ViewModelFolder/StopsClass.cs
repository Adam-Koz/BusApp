using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;


namespace QuickBus.ViewModelFolder
{
    class StopsClass
    {
        public class StopsLoad
        {
            public int ID { get; set; }
            public string Name { get; set; }

            public string URL { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
        }     
        internal async Task<List<StopsLoad>> LoadStops()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("QuickBus.StopsBus.json");
            string text = string.Empty;
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            var json = JsonConvert.DeserializeObject<List<StopsLoad>>(text);

            List<StopsLoad> stop = json;
            
            return stop;
        }
    }

    
}
