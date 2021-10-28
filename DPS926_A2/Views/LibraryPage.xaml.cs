using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DPS926_A2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibraryPage : ContentPage
    {
        static NetworkingManager manager = new NetworkingManager();

        public LibraryPage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            App.Database.SetUserLibraryData();

            myList.ItemsSource = App.Database.moviesList;
        }

        async private void MovieSelected(object sender, SelectionChangedEventArgs e)
        {
            await Navigation.PushAsync(new MovieDetailsPage((Models.MovieDetails)myList.SelectedItem));
        }
    }
}