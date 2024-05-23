using BabyDiary.Services;
using BabyDiary.Views;
using Firebase.Database;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BabyDiary
{
    public partial class App : Application
    {
        public static string FirebaseClient = "https://babydiary-3a7ea-default-rtdb.europe-west1.firebasedatabase.app";
        public static string FrebaseSecret = "oG355Ev5gT6p4t9ShUw5gCZslDCAmIhrLNpaVUN5";
        public FirebaseClient fireBaseClientObj;
        public static string DBFolder;

        public App()
        {
            init();
        }
        public App(string dbFolder)
        {
            init();
            DBFolder = dbFolder;
        }

        private void init()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
            fireBaseClientObj = new FirebaseClient(FirebaseClient,
                           new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(FrebaseSecret) });

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
