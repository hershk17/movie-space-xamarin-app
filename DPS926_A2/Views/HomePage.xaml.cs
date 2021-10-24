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
    public partial class HomePage : ContentPage
    {
        NetworkingManager manager = new NetworkingManager();
        int centerMovieIdx = 0;

        public HomePage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);

            App.Database.SetUserLibraryData();
            LoadMovies();
        }

        async private void LoadMovies()
        {
            NowPlayingMoviesPosters.ItemsSource = await manager.GetNowPlayingMovies();
            NowPlayingMoviesBackdrops.ItemsSource = NowPlayingMoviesPosters.ItemsSource;
            TrendingMovies.ItemsSource = await manager.GetTrendingMovies();
            TopRatedMovies.ItemsSource = await manager.GetTopRatedMovies();
            UpcomingMovies.ItemsSource = await manager.GetUpcomingMovies();
        }

        async private Task<bool> MovieSelected(Models.Movie selectedMovie)
        {
            await Navigation.PushAsync(new MovieDetailsPage(await manager.GetMovieDetails(selectedMovie.id)));
            return true;
        }

        async private void NowPlayingMovies_MovieSelected(object sender, SelectionChangedEventArgs e)
        {
            if (NowPlayingMoviesPosters.SelectedItem != null)
            {
                await MovieSelected((Models.Movie)NowPlayingMoviesPosters.SelectedItem);
                NowPlayingMoviesPosters.SelectedItem = null;
            }
        }

        async private void TrendingMovies_MovieSelected(object sender, SelectionChangedEventArgs e)
        {
            if (TrendingMovies.SelectedItem != null)
            {
                await MovieSelected((Models.Movie)TrendingMovies.SelectedItem);
                TrendingMovies.SelectedItem = null;
            }
        }

        async private void TopRatedMovies_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (TopRatedMovies.SelectedItem != null)
            {
                await MovieSelected((Models.Movie)TopRatedMovies.SelectedItem);
                TopRatedMovies.SelectedItem = null;
            }
        }

        async private void UpcomingMovies_MovieSelected(object sender, SelectionChangedEventArgs e)
        {
            if (UpcomingMovies.SelectedItem != null)
            {
                await MovieSelected((Models.Movie)UpcomingMovies.SelectedItem);
                UpcomingMovies.SelectedItem = null;
            }
        }

        private async void NowPlayingMoviesPosters_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if(e.CenterItemIndex != centerMovieIdx)
            {
                centerMovieIdx = e.CenterItemIndex;
                await Task.Delay(100);
                NowPlayingMoviesBackdrops.ScrollTo(centerMovieIdx);
            }
        }
    }
}