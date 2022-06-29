using Xamarin.Forms;
using QuickBus.Pages;

namespace QuickBus.Pages
{
    public partial class StopPage : ContentPage
    {
        
        public StopPage(string Name, string Url)
        {
            InitializeComponent();
            Title = Name;
            webView.Source = Url;
        }
    }
}