using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace DPS926_A2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieDetailsPage : ContentPage
    {
        NetworkingManager manager = new NetworkingManager();

        Models.MovieDetails currMovie = new Models.MovieDetails();
        
        string[] Ratings = { "10★", "9★", "8★", "7★", "6★", "5★", "4★", "3★", "2★", "1★" };
        string[] Statuses = { "Watching", "Completed", "Plan to Watch" };

        public MovieDetailsPage(Models.MovieDetails movieDetails)
        {
            InitializeComponent();
            Title = movieDetails.title;

            currMovie = movieDetails;
            SetMovieDetails();
        }

        private void SetMovieDetails()
        {
            Models.MovieDetails SavedMovie = App.Database.moviesList.FirstOrDefault(movie => movie.id == currMovie.id);

            if(SavedMovie != null)
            {
                currMovie.userRating = SavedMovie.userRating;
                currMovie.userWatchStatus = SavedMovie.userWatchStatus;
            }
            RatingPicker.ItemsSource = Ratings;
            StatusPicker.ItemsSource = Statuses;

            backdrop_path.Source = "https://image.tmdb.org/t/p/w780" + currMovie.backdrop_path;
            poster_path.Source = "https://image.tmdb.org/t/p/w300" + currMovie.poster_path;
            title.Text = currMovie.title;
            audience_rating.Text = currMovie.vote_average.ToString() + "★ (" + currMovie.vote_count.ToString() + ")";
            overview.Text = currMovie.overview;
            GenreList.ItemsSource = currMovie.genres;
            release_date.Text = DateTime.Parse(currMovie.release_date).ToLongDateString();
            TimeSpan timespan = TimeSpan.FromMinutes(currMovie.runtime);
            runtime.Text = (timespan.Hours + "hr " + timespan.Minutes + "mins").ToString();
            language.Text = currMovie.original_language;
            budget.Text = currMovie.budget.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            revenue.Text = currMovie.revenue.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            popularity.Text = currMovie.popularity.ToString();
            homepage.Text = currMovie.homepage;

            UpdateUserMovieInfo();
        }

        private void UpdateUserMovieInfo()
        {
            StatusPicker.SelectedIndex = Array.IndexOf(Statuses, currMovie.userWatchStatus);
            if (!string.IsNullOrWhiteSpace(currMovie.userRating))
            {
                RatingPicker.SelectedIndex = Array.IndexOf(Ratings, currMovie.userRating);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            bool result = App.Database.moviesList.FirstOrDefault(movie => movie.id == currMovie.id) == null;
            if(!result)
            {
                UpdateUserMovieInfo();
            }
            AddToList.IsVisible = result;
            UserMovieInfo.IsVisible = !result;
            DelFromList.IsVisible = !result;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Navigation.PopAsync();
        }

        private async void addToListClicked(object sender, EventArgs e)
        {
            string choice = await DisplayActionSheet("Add to a List", "Cancel", null, Statuses);

            if(!(string.IsNullOrWhiteSpace(choice) || choice == "Cancel")) {
                App.Database.AddMovie(currMovie);

                currMovie.userWatchStatus = choice;
                UpdateUserMovieInfo();

                AddToList.IsVisible = false;
                UserMovieInfo.IsVisible = true;
                DelFromList.IsVisible = true;
            }
        }

        private async void remFromListClicked(object sender, EventArgs e)
        {
            bool confirmed = await DisplayAlert("Remove from List?", null, "Confirm", "Cancel");

            if (confirmed)
            {
                App.Database.DeleteMovie(currMovie);

                AddToList.IsVisible = true;
                UserMovieInfo.IsVisible = false;
                DelFromList.IsVisible = false;
            }
        }

        private void statusUpdated(object sender, EventArgs e)
        {
            currMovie.userWatchStatus = Statuses[StatusPicker.SelectedIndex];
            App.Database.UpdateMovie(currMovie);
        }

        private void ratingUpdated(object sender, EventArgs e)
        {
            currMovie.userRating = Ratings[RatingPicker.SelectedIndex]; 
            App.Database.UpdateMovie(currMovie);
        }

        public async Task ShareText(string text)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Share Movie"
            });
        }

        private async void ShareButton_Clicked(object sender, EventArgs e)
        {
            await ShareText("Hey! Check out this movie I found: " + currMovie.title + (!string.IsNullOrWhiteSpace(currMovie.homepage) ? (" [" + currMovie.homepage + "] ") : " "));
        }

        private async void PlayButton_Clicked(object sender, EventArgs e)
        {
            await Launcher.OpenAsync(new Uri("https://www.youtube.com/watch?v=" + await manager.GetTrailer(currMovie.id)));
        }
    }
}