
using BabyDiary.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BabyDiary
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DiaryPage), typeof(NewDiaryEntryPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
