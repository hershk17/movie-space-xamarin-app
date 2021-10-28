using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DPS926_A2
{
    public class MovieListManager
    {
        public string currentlyActiveParam = "Best Match ⥮";
        private List<Models.SortParameter> sortParameters = new List<Models.SortParameter>
        {
            new Models.SortParameter { name="Latest", active=false},
            new Models.SortParameter { name="Popularity", active=false },
            new Models.SortParameter { name="Rating", active=false },
            new Models.SortParameter { name="Title", active=false }
        };
        
        public ObservableCollection<Models.Movie> MovieResults = new ObservableCollection<Models.Movie>();
        public List<Models.Movie> MovieResultsSorted = new List<Models.Movie>();

        public List<string> GetSortParameters()
        {
            List<string> sortParameterNames = new List<string>();
            sortParameterNames.Add("Best Match");
            sortParameters.ForEach(param => sortParameterNames.Add(param.name));
            return sortParameterNames;
        }

        public void UpdateSortParameters(int id, bool activeState)
        {
            currentlyActiveParam = "Best Match ⥮";

            if (id > -1)
            {
                sortParameters.ForEach(param => param.active = param.name == sortParameters[id].name ? param.active : false);
                currentlyActiveParam = sortParameters[id].name;
                sortParameters[id].active = activeState;
            }
            else
            {
                sortParameters.ForEach(param => param.active = false);
            }
        }

        public void SortMovies(string choice)
        {
            switch (choice)
            {
                case "Latest ↑":
                    MovieResultsSorted = MovieResults.OrderByDescending(movie => DateTime.Parse(movie.release_date)).ToList();
                    UpdateSortParameters(0, true);
                    break;
                case "Latest ↓":
                    MovieResultsSorted = MovieResults.OrderBy(movie => DateTime.Parse(movie.release_date)).ToList();
                    UpdateSortParameters(0, false);
                    break;
                case "Popularity ↑":
                    MovieResultsSorted = MovieResults.OrderByDescending(movie => movie.popularity).ToList();
                    UpdateSortParameters(1, true);
                    break;
                case "Popularity ↓":
                    MovieResultsSorted = MovieResults.OrderBy(movie => movie.popularity).ToList();
                    UpdateSortParameters(1, false);
                    break;
                case "Rating ↑":
                    MovieResultsSorted = MovieResults.OrderByDescending(movie => movie.vote_average).ToList();
                    UpdateSortParameters(2, true);
                    break;
                case "Rating ↓":
                    MovieResultsSorted = MovieResults.OrderBy(movie => movie.vote_average).ToList();
                    UpdateSortParameters(2, false);
                    break;
                case "Title ↑":
                    MovieResultsSorted = MovieResults.OrderBy(movie => movie.title).ToList();
                    UpdateSortParameters(3, true);
                    break;
                case "Title ↓":
                    MovieResultsSorted = MovieResults.OrderByDescending(movie => movie.title).ToList();
                    UpdateSortParameters(3, false);
                    break;
                case "Best Match":
                default:
                    MovieResultsSorted = MovieResults.ToList();
                    UpdateSortParameters(-1, false);
                    break;
            }
        }
    }
}