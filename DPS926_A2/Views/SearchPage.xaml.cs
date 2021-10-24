using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DPS926_A2.Views
{
    public partial class SearchPage : ContentPage
    {
        NetworkingManager manager = new NetworkingManager();
        string query;

        public SearchPage()
        {
            InitializeComponent();

            Title = "Search";

            Shell.SetNavBarIsVisible(this, false);
        }

        async void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            movieList.ItemsSource = await manager.SearchMovie(query);
        }

        void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            query = e.NewTextValue;
        }

        async void movieList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new MovieDetailsPage(await manager.GetMovieDetails(((Models.Movie)e.SelectedItem).id)));
        }
    }
}