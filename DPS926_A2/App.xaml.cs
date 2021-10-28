using DPS926_A2.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using Xamarin.Essentials;
using System.IO;

[assembly: ExportFont("Helvetica-Bold.ttf", Alias = "Helvetica Bold")]
[assembly: ExportFont("Helvetica-BoldOblique.ttf", Alias = "Helvetica Bold Oblique")]
[assembly: ExportFont("helvetica-compressed-5871d14b6903a.otf", Alias = "Helvetica Compressed")]
[assembly: ExportFont("helvetica-light-587ebe5a59211.ttf", Alias = "Helvetica Light")]
[assembly: ExportFont("Helvetica-Oblique.ttf", Alias = "Helvetica Oblique")]
[assembly: ExportFont("helvetica-rounded-bold-5871d05ead8de.otf", Alias = "Helvetica Rounded Bold")]
[assembly: ExportFont("Helvetica.ttf", Alias = "Helvetica")]

namespace DPS926_A2
{
    public partial class App : Application
    {
        static DatabaseManager database;

        public static DatabaseManager Database
        {
            get
            {
                if (database == null)
                {
                    database = new DatabaseManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "moviesList.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Xamarin.Forms.PlatformConfiguration.AndroidSpecific.Application.SetWindowSoftInputModeAdjust(this, Xamarin.Forms.PlatformConfiguration.AndroidSpecific.WindowSoftInputModeAdjust.Resize);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
