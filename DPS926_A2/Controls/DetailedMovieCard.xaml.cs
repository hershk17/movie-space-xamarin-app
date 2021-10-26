using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DPS926_A2.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedMovieCard : ViewCell
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", returnType: typeof(string), declaringType: typeof(DetailedMovieCard), defaultValue: "", defaultBindingMode: BindingMode.TwoWay, propertyChanged: TitlePropertyChanged);
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DetailedMovieCard)bindable;
            control.title.Text = newValue.ToString();
        }

        public static readonly BindableProperty PosterProperty = BindableProperty.Create("Poster", returnType: typeof(string), declaringType: typeof(DetailedMovieCard), defaultValue: "", defaultBindingMode: BindingMode.TwoWay, propertyChanged: PosterPropertyChanged);
        public string Poster
        {
            get { return (string)GetValue(PosterProperty); }
            set { SetValue(PosterProperty, value); }
        }

        private static void PosterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DetailedMovieCard)bindable;
            control.poster.Source = newValue.ToString();
        }

        public static readonly BindableProperty VoteAverageProperty = BindableProperty.Create("VoteAverage", returnType: typeof(string), declaringType: typeof(DetailedMovieCard), defaultValue: "", defaultBindingMode: BindingMode.TwoWay, propertyChanged: VoteAveragePropertyChanged);
        public string VoteAverage
        {
            get { return (string)GetValue(VoteAverageProperty); }
            set { SetValue(VoteAverageProperty, value); }
        }

        private static void VoteAveragePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DetailedMovieCard)bindable;
            control.vote_average.Text = newValue.ToString();
        }

        public static readonly BindableProperty VoteCountProperty = BindableProperty.Create("VoteCount", returnType: typeof(string), declaringType: typeof(DetailedMovieCard), defaultValue: "", defaultBindingMode: BindingMode.TwoWay, propertyChanged: VoteCountPropertyChanged);
        public string VoteCount
        {
            get { return (string)GetValue(VoteCountProperty); }
            set { SetValue(VoteCountProperty, value); }
        }

        private static void VoteCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DetailedMovieCard)bindable;
            control.vote_count.Text = newValue.ToString();
        }

        public static readonly BindableProperty ReleaseDateProperty = BindableProperty.Create("ReleaseDate", returnType: typeof(string), declaringType: typeof(DetailedMovieCard), defaultValue: "", defaultBindingMode: BindingMode.TwoWay, propertyChanged: ReleaseDatePropertyChanged);
        public string ReleaseDate
        {
            get { return (string)GetValue(ReleaseDateProperty); }
            set { SetValue(ReleaseDateProperty, value); }
        }

        private static void ReleaseDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DetailedMovieCard)bindable;
            control.release_date.Text = newValue.ToString();
        }

        public DetailedMovieCard()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}