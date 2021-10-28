using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Newtonsoft.Json;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using System.Linq;
using System.Text;
using Xamarin.Forms.Xaml;

namespace DPS926_A2
{
    public class DatabaseManager
    {
        readonly SQLiteAsyncConnection _database;

        public ObservableCollection<Models.MovieDetails> moviesList = new ObservableCollection<Models.MovieDetails>();

        public DatabaseManager(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Models.MovieDetails>().Wait();
        }

        public async void SetUserLibraryData()
        {
            var response = await _database.Table<Models.MovieDetails>().ToListAsync();

            moviesList.Clear();
            foreach (Models.MovieDetails i in response)
            {
                moviesList.Add(new Models.MovieDetails(i.id, i.title, i.poster_path, 
                    i.backdrop_path, i.vote_average, i.overview, i.original_language, 
                    i.popularity, i.release_date, i.vote_count, i.budget, i.homepage, i.revenue, i.runtime, i.genresString, 
                    i.userWatchStatus, i.userRating));
            }
        }

        public async void AddMovie(Models.MovieDetails movie)
        {
            await _database.InsertAsync(movie);
            moviesList.Add(movie);
        }

        public async void UpdateMovie(Models.MovieDetails updatedMovie)
        {
            await _database.UpdateAsync(updatedMovie);
        }

        public async void DeleteMovie(Models.MovieDetails movie)
        {
            await _database.DeleteAsync(movie);
            moviesList.Remove(movie);
            moviesList.Remove(moviesList.SingleOrDefault(i => i.id == movie.id));
        }
    }
}
