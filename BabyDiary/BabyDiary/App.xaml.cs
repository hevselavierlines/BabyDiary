using BabyDiary.Services;
using BabyDiary.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BabyDiary
{
    public partial class App : Application
    {
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
