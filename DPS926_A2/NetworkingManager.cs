﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using SQLite;
using Xamarin.Forms;

namespace DPS926_A2
{
    public class NetworkingManager
    {
        private HttpClient client = new HttpClient();

        private const string key = "33f0b7dbdcaef3a551cf390bfec1c63c";

        private const string domainURL = "https://api.themoviedb.org/3/";

        private const string searchURL = domainURL + "search/movie";
        private const string detailsURL = domainURL + "movie/";

        private const string nowPlaying = domainURL + "movie/now_playing";
        private const string trendingURL = domainURL + "trending/movie/week";
        private const string topRatedURL = domainURL + "movie/top_rated";
        private const string upcomingURL = domainURL + "movie/upcoming";

        private const string queryParam = "&query=";
        private const string apiParam = "?api_key=";

        public NetworkingManager() { }

        private async Task<ObservableCollection<Models.Movie>> GetMovies(string type, bool highQualityImages = false, string query = null)
        {
            ObservableCollection<Models.Movie> movieCollection = new ObservableCollection<Models.Movie>();

            string completeURL;
            int elem = 1;

            switch (type)
            {
                case "nowPlaying":
                    completeURL = nowPlaying + apiParam + key;
                    elem = 2;
                    break;
                case "trending":
                    completeURL = trendingURL + apiParam + key;
                    break;
                case "topRated":
                    completeURL = topRatedURL + apiParam + key;
                    break;
                case "upcoming":
                    completeURL = upcomingURL + apiParam + key;
                    elem = 2;
                    break;
                case "search":
                    completeURL = searchURL + apiParam + key + queryParam + query;
                    break;
                default:
                    completeURL = nowPlaying + apiParam + key;
                    break;
            }

            List<Models.Movie> resultList = new List<Models.Movie>();
            var response = await client.GetAsync(completeURL);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) {
                return movieCollection;
            }
            else  {
                var jsonString = await response.Content.ReadAsStringAsync();
                var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

                var array = dic.ElementAt(elem).Value;
                resultList = JsonConvert.DeserializeObject<List<Models.Movie>>(array.ToString());
            }

            string imageResolution = highQualityImages ? "780" : "300";

            int cnt = 0, maxCnt = 10;
            foreach (Models.Movie i in resultList)
            {
                if (cnt == maxCnt)
                    break;
                movieCollection.Add(new Models.Movie(i.id, i.title, "https://image.tmdb.org/t/p/w" + imageResolution + i.poster_path,
                    "https://image.tmdb.org/t/p/w" + imageResolution + i.backdrop_path, i.vote_average, i.userWatchStatus, i.userRating));
                cnt++;
            }

            return movieCollection;
        }

        public async Task<ObservableCollection<Models.Movie>> GetNowPlayingMovies()
        {
            return await GetMovies("nowPlaying", true);
        }

        public async Task<ObservableCollection<Models.Movie>> GetTrendingMovies()
        {
            return await GetMovies("trending");
        }

        public async Task<ObservableCollection<Models.Movie>> GetTopRatedMovies()
        {
            return await GetMovies("topRated");
        }

        public async Task<ObservableCollection<Models.Movie>> GetUpcomingMovies()
        {
            return await GetMovies("upcoming");
        }

        public async Task<ObservableCollection<Models.Movie>> SearchMovie(string query)
        {
            return await GetMovies("search", false, query);
        }

        public async Task<Models.MovieDetails> GetMovieDetails(int id)
        {
            string completeURL = detailsURL + id + apiParam + key;
            var response = await client.GetAsync(completeURL);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new Models.MovieDetails();
            }
            else
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var movieDetails = JsonConvert.DeserializeObject<Models.MovieDetails>(jsonString.ToString());
                return movieDetails;
            }
        }
    }
}