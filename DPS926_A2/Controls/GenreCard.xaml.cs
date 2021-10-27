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
    public partial class GenreCard : StackLayout
    {
        NetworkingManager manager = new NetworkingManager();

        public static readonly BindableProperty GenreIdProperty = BindableProperty.Create("GenreId", typeof(int), typeof(GenreCard));
        public int GenreId
        {
            get { return (int)GetValue(GenreIdProperty); }
            set { SetValue(GenreIdProperty, value); }
        }

        public static readonly BindableProperty GenreNameProperty = BindableProperty.Create("GenreName", typeof(string), typeof(GenreCard));
        public string GenreName
        {
            get { return (string)GetValue(GenreNameProperty); }
            set { SetValue(GenreNameProperty, value); }
        }

        public static readonly BindableProperty GenreBackgroundProperty = BindableProperty.Create("GenreBackground", typeof(string), typeof(GenreCard));
        public string GenreBackground
        {
            get { return (string)GetValue(GenreBackgroundProperty); }
            set { SetValue(GenreBackgroundProperty, value); }
        }

        public GenreCard()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void Genre_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.ExploreMoviesListPage(await manager.GetMoviesOfGenre(GenreId), GenreName));
        }
    }
}