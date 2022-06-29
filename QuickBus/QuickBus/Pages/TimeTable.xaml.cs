using Xamarin.Forms;
using QuickBus.ViewModelFolder;
using QuickBus.Pages;
using System.Collections.Generic;
using System.Linq;

namespace QuickBus
{
    public partial class TimeTable : ContentPage
    {
        StopsClass StopsClass;
        List<ListCellClass> list = new List<ListCellClass>();

        public TimeTable()
        {
            InitializeComponent();
            BindingContext = StopsClass = new StopsClass();
            LoadTable();
        }
        public async void LoadTable()
        {
            var content = await StopsClass.LoadStops();
            {
                if (content != null)
                {

                    foreach (var item in content)
                    {
                        list.Add(new ListCellClass() { Name = item.Name, Id = item.ID, Url = item.URL});
                    }
                }
                listView.ItemsSource = list;
            }
        }


        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                listView.ItemsSource = list;
            }
            else
            {
                listView.ItemsSource = list.Where(i => i.Name.ToLower().StartsWith(e.NewTextValue.ToLower()));
            }

        }


        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            var butt = ((Button)sender).BindingContext as ListCellClass;
            if (butt == null)
                return;
            await Navigation.PushAsync(new StopPage(butt.Name, butt.Url));
        }
    }
}
