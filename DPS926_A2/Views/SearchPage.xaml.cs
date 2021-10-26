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
        MovieListManager movieListManager = new MovieListManager();

        string query;

        public SearchPage()
        {
            InitializeComponent();
            Title = "Search";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(600);

            if(!movieList.IsVisible) mySearchBar.Focus();
        }

        private async void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            movieListManager.MovieResults = await manager.SearchMovie(query);

            bool ResultsFound = movieListManager.MovieResults.Count > 0;
            ResultsLabel.Text = (ResultsFound ? movieListManager.MovieResults.Count.ToString() : "No") + " result(s) found.";

            SortButton.Text = movieListManager.currentlyActiveParam;
            movieList.ItemsSource = ResultsFound ? movieListManager.MovieResults : null;

            SortInfo.IsVisible = ResultsFound;
            movieList.IsVisible = ResultsFound;
        }

        private void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            query = e.NewTextValue;
        }

        private async void movieList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(movieList.SelectedItem != null)
            {
                await Navigation.PushAsync(new MovieDetailsPage(await manager.GetMovieDetails(((Models.Movie)e.SelectedItem).id)));
                movieList.SelectedItem = null;
            }
        }

        private async void SortButton_Clicked(object sender, EventArgs e)
        {
            string choice = await DisplayActionSheet("Sort by", "Cancel", null, movieListManager.GetSortParameters().ToArray());
            movieListManager.SortMovies(choice);
            movieList.ItemsSource = movieListManager.MovieResultsSorted;
            SortButton.Text = movieListManager.currentlyActiveParam;
        }
    }
}