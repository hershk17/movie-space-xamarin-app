using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using SQLite;

namespace DPS926_A2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExplorePage : ContentPage
    {
        NetworkingManager manager = new NetworkingManager();

        public ExplorePage()
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false);
        }

        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

        private async void Recommendations_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExploreMoviesListPage(await manager.GetRecommendations()));
        }
    }
}