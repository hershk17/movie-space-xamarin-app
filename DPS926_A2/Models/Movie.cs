using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.ComponentModel;
using System.Linq;

namespace DPS926_A2.Models
{
    public class Movie : INotifyPropertyChanged
    {
        [PrimaryKey]
        public int id { set; get; }
        public string title { set; get; }
        public string poster_path { get; set; }
        public string backdrop_path { set; get; }
        public double vote_average { set; get; }

        public string _userWatchStatus;
        public string userWatchStatus
        {
            get { return _userWatchStatus; }
            set
            {
                if (_userWatchStatus != value)
                {
                    _userWatchStatus = value;
                    OnPropertyChanged("userWatchStatus");
                }
            }
        }

        public string _userRating;
        public string userRating
        {
            get { return _userRating; }
            set
            {
                if (_userRating != value)
                {
                    _userRating = value;
                    OnPropertyChanged("userRating");
                }
            }
        }

        public Movie() { }

        public Movie(int id, string title, string poster_path, string backdrop_path, double vote_average, string userWatchStatus, string userRating)
        {
            this.id = id;
            this.title = title;
            this.poster_path = poster_path;
            this.backdrop_path = backdrop_path;
            this.vote_average = vote_average;
            this.userWatchStatus = userWatchStatus;
            this.userRating = userRating;
        }

        public Movie(Models.Movie movie) : this(movie.id, movie.title, movie.poster_path, movie.backdrop_path, movie.vote_average, movie.userWatchStatus, movie.userRating) { }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}