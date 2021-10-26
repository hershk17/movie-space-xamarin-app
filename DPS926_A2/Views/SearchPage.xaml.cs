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

        List<Models.SortParameter> sortParameters = new List<Models.SortParameter>
        {
            new Models.SortParameter { name="Latest", active=false},
            new Models.SortParameter { name="Popularity", active=false },
            new Models.SortParameter { name="Rating", active=false },
            new Models.SortParameter { name="Title", active=false }
        };

        ObservableCollection<Models.Movie> MovieSearchResults = new ObservableCollection<Models.Movie>();

        public SearchPage()
        {
            InitializeComponent();

            Title = "Search";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(600);

            if(!movieList.IsVisible)
            {
                mySearchBar.Focus();
            }
        }

        private async void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            MovieSearchResults = await manager.SearchMovie(query);

            bool ResultsFound = MovieSearchResults.Count > 0;
            ResultsLabel.Text = (ResultsFound ? MovieSearchResults.Count.ToString() : "No") + " result(s) found.";

            SortButton.Text = "Best Match ⥮";
            SortInfo.IsVisible = ResultsFound;
            movieList.ItemsSource = ResultsFound ? MovieSearchResults : null;
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

        private void SetSortParameterState(int id, bool activeState)
        {
            if (id > -1)
            {
                sortParameters.ForEach(param => param.active = param.name == sortParameters[id].name ? param.active : false);
                SortButton.Text = sortParameters[id].name;
                sortParameters[id].active = activeState;
            }
            else
            {
                sortParameters.ForEach(param => param.active = false);
                SortButton.Text = "Best Match ⥮";
            }
        }

        private async void SortButton_Clicked(object sender, EventArgs e)
        {
            string choice = await DisplayActionSheet("Sort by", "Cancel", null, "Best Match", sortParameters[0].name, sortParameters[1].name, sortParameters[2].name, sortParameters[3].name);

            if (!(string.IsNullOrWhiteSpace(choice) || choice == "Cancel"))
            {
                switch (choice) {
                    case "Latest ↑":
                        movieList.ItemsSource = MovieSearchResults.OrderByDescending(movie => DateTime.Parse(movie.release_date)).ToList();
                        SetSortParameterState(0, true);
                        break;
                    case "Latest ↓":
                        movieList.ItemsSource = MovieSearchResults.OrderBy(movie => DateTime.Parse(movie.release_date)).ToList();
                        SetSortParameterState(0, false);
                        break;
                    case "Popularity ↑":
                        movieList.ItemsSource = MovieSearchResults.OrderByDescending(movie => movie.popularity).ToList();
                        SetSortParameterState(1, true);
                        break;
                    case "Popularity ↓":
                        movieList.ItemsSource = MovieSearchResults.OrderBy(movie => movie.popularity).ToList();
                        SetSortParameterState(1, false);
                        break;
                    case "Rating ↑":
                        movieList.ItemsSource = MovieSearchResults.OrderByDescending(movie => movie.vote_average).ToList();
                        SetSortParameterState(2, true);
                        break;
                    case "Rating ↓":
                        movieList.ItemsSource = MovieSearchResults.OrderBy(movie => movie.vote_average).ToList();
                        SetSortParameterState(2, false);
                        break;
                    case "Title ↑":
                        movieList.ItemsSource = MovieSearchResults.OrderBy(movie => movie.title).ToList();
                        SetSortParameterState(3, true);
                        break;
                    case "Title ↓":
                        movieList.ItemsSource = MovieSearchResults.OrderByDescending(movie => movie.title).ToList();
                        SetSortParameterState(3, false);
                        break;
                    case "Best Match":
                    default:
                        movieList.ItemsSource = MovieSearchResults;
                        SetSortParameterState(-1, false);
                        break;
                }
            }
        }
    }
}