using System.Collections.Generic;
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
        private const string popularURL = domainURL + "movie/popular";

        private const string genresURL = domainURL + "genre/movie/list";
        private const string genreMoviesURL = domainURL + "discover/movie";

        private const string queryParam = "&query=";
        private const string apiParam = "?api_key=";
        private const string videosParam = "/videos";
        private const string genresParam = "&with_genres=";
        private const string recommendationsParam = "/recommendations";
        private const string languageParam = "&language=en-US";

        public NetworkingManager() { }

        private async Task<ObservableCollection<Models.Movie>> GetMovies(string type, bool highQualityImages = false, string query = null, int genre = -1, int id = -1, int limit = 10)
        {
            ObservableCollection<Models.Movie> movieCollection = new ObservableCollection<Models.Movie>();

            string completeURL;
            int elem = 1;
            int cnt = 0, maxCnt = limit;

            switch (type)
            {
                case "nowPlaying":
                    completeURL = nowPlaying + apiParam + key + languageParam;
                    elem = 2;
                    maxCnt = 5;
                    break;
                case "trending":
                    completeURL = trendingURL + apiParam + key + languageParam;
                    break;
                case "topRated":
                    completeURL = topRatedURL + apiParam + key + languageParam;
                    break;
                case "upcoming":
                    completeURL = upcomingURL + apiParam + key + languageParam;
                    elem = 2;
                    break;
                case "search":
                    completeURL = searchURL + apiParam + key + queryParam + query + languageParam;
                    maxCnt = 20;
                    break;
                case "genre":
                    completeURL = genreMoviesURL + apiParam + key + genresParam + genre + languageParam;
                    maxCnt = 20;
                    break;
                case "recommendations":
                    completeURL = detailsURL + id + recommendationsParam + apiParam + key + languageParam;
                    break;
                case "popular":
                    completeURL = popularURL + apiParam + key + languageParam;
                    maxCnt = 20;
                    break;
                default:
                    completeURL = nowPlaying + apiParam + key + languageParam;
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

            foreach (Models.Movie i in resultList)
            {
                if (cnt == maxCnt)
                    break;

                if(i.vote_count > 0 && i.popularity > 10)
                    movieCollection.Add(new Models.Movie(i.id, i.title, "https://image.tmdb.org/t/p/w" + imageResolution + i.poster_path,
                        "https://image.tmdb.org/t/p/w" + imageResolution + i.backdrop_path, i.vote_average, i.vote_count, i.popularity,
                        i.release_date, i.userWatchStatus, i.userRating));
                
                cnt++;
            }

            return movieCollection;
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

        public async Task<List<Models.Genre>> GetGenres()
        {
            string completeURL = genresURL + apiParam + key;
            var response = await client.GetAsync(completeURL);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<Models.Genre>();
            }
            else
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

                var array = dic.ElementAt(0).Value;
                var result = JsonConvert.DeserializeObject<List<Models.Genre>>(array.ToString());

                return result;
            }
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

        public async Task<string> GetTrailer(int id)
        {
            string completeURL = detailsURL + id + videosParam + apiParam + key;
            var response = await client.GetAsync(completeURL);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return "";
            }
            else
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

                var array = dic.ElementAt(1).Value;
                List<Models.Video> result = JsonConvert.DeserializeObject<List<Models.Video>>(array.ToString());

                return (result.FirstOrDefault(video => (video.official && video.site == "YouTube" && video.type == "Trailer"))).key;
            }
        }

        public async Task<ObservableCollection<Models.Movie>> GetMoviesOfGenre(int genreId)
        {
            return await GetMovies("genre", false, null, genreId);
        }

        public async Task<ObservableCollection<Models.Movie>> GetRecommendations()
        {
            List<int> completedMovies = new List<int>(App.Database.moviesList.ToList().FindAll(movie => movie.userWatchStatus == "Completed").Select(movie => movie.id).ToList());
            HashSet<Models.Movie> recommendations = new HashSet<Models.Movie>();

            ObservableCollection<Models.Movie> result = new ObservableCollection<Models.Movie>();

            const int MaxDepth = 4;
            int n = completedMovies.Count;

            if (n == 0)
            {
                result = await GetMovies("popular");

                for (int i = 0; i < result.Count; i++)
                {
                    recommendations.Add(result[i]);
                }
            }
            else if (n < 5)
            {
                for (int i = 0; i < n; i++)
                {
                    result = await GetMovies("recommendations", false, null, -1, completedMovies[i], 40 / n);

                    for (int j = 0; j < result.Count; j++)
                    {
                        recommendations.Add(result[j]);
                    }
                }
            }
            else
            {
                Random rng = new Random();
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    int tmp = completedMovies[k];
                    completedMovies[k] = completedMovies[n];
                    completedMovies[n] = tmp;
                }

                completedMovies = completedMovies.GetRange(0, MaxDepth);

                for (int i = 0; i < MaxDepth; i++)
                {
                    result = await GetMovies("recommendations", false, null, -1, completedMovies[i], 10);

                    for (int j = 0; j < result.Count; j++)
                    {
                        recommendations.Add(result[j]);
                    }
                }
            }

            List<Models.Movie> RandomRecommendations = new List<Models.Movie>(recommendations);
            
            Random rand = new Random();
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Models.Movie tmp = RandomRecommendations[k];
                RandomRecommendations[k] = RandomRecommendations[n];
                RandomRecommendations[n] = tmp;
            }

            recommendations.Clear();

            for(int i = 0; i < RandomRecommendations.Count; i++)
            {
                recommendations.Add(RandomRecommendations[i]);
            }

            return new ObservableCollection<Models.Movie>(recommendations);
        }
    }
}
