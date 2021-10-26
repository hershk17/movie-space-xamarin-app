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
    public partial class SimpleMovieCard : StackLayout
    {
        public static readonly BindableProperty MoviePosterProperty = BindableProperty.Create(propertyName: "MoviePoster", returnType: typeof(string), declaringType: typeof(SimpleMovieCard), defaultValue: "", defaultBindingMode: BindingMode.TwoWay, propertyChanged: MoviePosterPropertyChanged);
        public string MoviePoster
        {
            get { return (string)GetValue(MoviePosterProperty); }
            set { SetValue(MoviePosterProperty, value); }
        }

        private static void MoviePosterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SimpleMovieCard)bindable;
            control.poster.Source = newValue.ToString();
        }

        public SimpleMovieCard()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}