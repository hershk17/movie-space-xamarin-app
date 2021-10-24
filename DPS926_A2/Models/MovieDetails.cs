using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensionsAsync;
using Newtonsoft.Json;

namespace DPS926_A2.Models
{
    public class MovieDetails : Movie
    {
        public string overview { set; get; }
        public string original_language { set; get; }
        public double popularity { set; get; }
        public string release_date { set; get; }
        public int vote_count { set; get; }
        public long budget { set; get; }
        public string homepage { set; get; }
        public long revenue { set; get; }
        public int runtime { set; get; }

        public List<Genre> Genres;
        [Ignore]
        public List<Genre> genres { 
            set {
                Genres = value;
                genresString = JsonConvert.SerializeObject(Genres);
            }
            get { return Genres; }
        }
        public string genresString { set; get; }

        public MovieDetails() { }

        public MovieDetails(int id, string title, string poster_path, string backdrop_path, double vote_average, string overview, string original_language, double popularity, string release_date, int vote_count, long budget, string homepage, long revenue, int runtime, string genresString, string userWatchStatus, string userRating) : base(id, title, poster_path, backdrop_path, vote_average, userWatchStatus, userRating)
        {
            this.overview = overview;
            this.original_language = original_language;
            this.popularity = popularity;
            this.release_date = release_date;
            this.vote_count = vote_count;
            this.budget = budget;
            this.homepage = homepage;
            this.revenue = revenue;
            this.runtime = runtime;
            this.genres = JsonConvert.DeserializeObject<List<Genre>>(genresString);
        }
    }
}