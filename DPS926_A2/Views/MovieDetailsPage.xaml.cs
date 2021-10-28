using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Windows.Input;

namespace DPS926_A2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieDetailsPage : ContentPage
    {
        NetworkingManager manager = new NetworkingManager();

        Models.MovieDetails currMovie = new Models.MovieDetails();
        
        string[] Ratings = { "10★", "9★", "8★", "7★", "6★", "5★", "4★", "3★", "2★", "1★" };
        string[] Statuses = { "Watching", "Completed", "Plan to Watch" };

        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        public MovieDetailsPage(Models.MovieDetails movieDetails)
        {
            InitializeComponent();
            Title = movieDetails.title;

            currMovie = movieDetails;
            SetMovieDetails();

            BindingContext = this;
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
            BindableLayout.SetItemsSource(GenreList, currMovie.genres);
            release_date.Text = DateTime.Parse(currMovie.release_date).ToLongDateString();
            TimeSpan timespan = TimeSpan.FromMinutes(currMovie.runtime);
            runtime.Text = (timespan.Hours + "hr " + timespan.Minutes + "mins").ToString();
            language.Text = currMovie.original_language;
            budget.Text = currMovie.budget.ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
            revenue.Text = currMovie.revenue.ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
            homepage.CommandParameter = currMovie.homepage;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Models.Movie result = App.Database.moviesList.FirstOrDefault(movie => movie.id == currMovie.id);

            if (result != null)
            {
                if (!string.IsNullOrWhiteSpace(result.userWatchStatus))
                {
                    StatusPicker.SelectedIndex = Array.IndexOf(Statuses, result.userWatchStatus);
                }
                if (!string.IsNullOrWhiteSpace(result.userRating))
                {
                    RatingPicker.SelectedIndex = Array.IndexOf(Ratings, result.userRating);
                }
            }

            AddToList.IsVisible = result == null;
            UserMovieInfo.IsVisible = result != null;
            DelFromList.IsVisible = result != null;
        }

        private void addToListClicked(object sender, EventArgs e)
        {
            StatusPicker.SelectedIndex = -1;
            RatingPicker.SelectedIndex = -1;

            AddToList.IsVisible = false;
            UserMovieInfo.IsVisible = true;
            DelFromList.IsVisible = true;

            App.Database.AddMovie(currMovie);
        }

        private async void remFromListClicked(object sender, EventArgs e)
        {
            bool confirmed = await DisplayAlert("Remove from List?", null, "Confirm", "Cancel");

            if (confirmed)
            {
                StatusPicker.SelectedIndex = -1;
                RatingPicker.SelectedIndex = -1;

                AddToList.IsVisible = true;
                UserMovieInfo.IsVisible = false;
                DelFromList.IsVisible = false;

                App.Database.DeleteMovie(currMovie);
            }
        }

        private void statusUpdated(object sender, EventArgs e)
        {
            if (StatusPicker.SelectedIndex != -1) currMovie.userWatchStatus = Statuses[StatusPicker.SelectedIndex];
            else currMovie.userWatchStatus = "";

            Models.Movie MovieToUpdate = App.Database.moviesList.FirstOrDefault(mov => mov.id == currMovie.id);
            if (MovieToUpdate != null) MovieToUpdate.userWatchStatus = currMovie.userWatchStatus;

            App.Database.UpdateMovie(currMovie);
        }

        private void ratingUpdated(object sender, EventArgs e)
        {
            if (RatingPicker.SelectedIndex != -1)
                currMovie.userRating = Ratings[RatingPicker.SelectedIndex];
            else
                currMovie.userRating = "";

            Models.Movie MovieToUpdate = App.Database.moviesList.FirstOrDefault(mov => mov.id == currMovie.id);
            if (MovieToUpdate != null) MovieToUpdate.userRating = currMovie.userRating;

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

        private async void MovieGenre_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExploreMoviesListPage(await manager.GetMoviesOfGenre(int.Parse((sender as StackLayout).ClassId)), (sender as StackLayout).AutomationId));
        }
    }
}