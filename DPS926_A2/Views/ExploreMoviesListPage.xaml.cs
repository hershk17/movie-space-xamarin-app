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
    public partial class ExploreMoviesListPage : ContentPage
    {
        NetworkingManager manager = new NetworkingManager();
        MovieListManager movieListManager = new MovieListManager();

        public ExploreMoviesListPage(ObservableCollection<Models.Movie> movies, string category = "Recommendations")
        {
            InitializeComponent();
            Title = category;

            ResultsLabel.Text = movies.Count.ToString() + " results shown.";
            SortButton.Text = movieListManager.currentlyActiveParam;

            movieListManager.MovieResults = movies;
            movieList.ItemsSource = movieListManager.MovieResults;
        }

        private async void movieList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (movieList.SelectedItem != null)
            {
                await Navigation.PushAsync(new MovieDetailsPage(await manager.GetMovieDetails(((Models.Movie)e.SelectedItem).id)));
                movieList.SelectedItem = null;
            }
        }

        private async void SortButton_Clicked(object sender, EventArgs e)
        {
            string choice = await DisplayActionSheet("Sort by", "Cancel", null, movieListManager.GetSortParameters().ToArray());

            if (!string.IsNullOrWhiteSpace(choice) && choice != "Cancel")
            {
                movieListManager.SortMovies(choice);
                movieList.ItemsSource = movieListManager.MovieResultsSorted;
                SortButton.Text = movieListManager.currentlyActiveParam;
            }
        }
    }
}