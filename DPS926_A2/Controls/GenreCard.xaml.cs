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

        public static readonly BindableProperty GenreBackdropProperty = BindableProperty.Create("GenreBackdrop", typeof(string), typeof(GenreCard));
        public string GenreBackdrop
        {
            get { return (string)GetValue(GenreBackdropProperty); }
            set { SetValue(GenreBackdropProperty, value); }
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