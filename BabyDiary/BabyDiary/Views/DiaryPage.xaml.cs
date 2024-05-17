using BabyDiary.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace BabyDiary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryPage : ContentPage
    {
        public DiaryPage()
        {
            InitializeComponent();
            
        }
        
        void LoadDiaryEntries(DateTime dateTime)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.DBFolder))
            {
                connection.CreateTable<DiaryEntry>();
                var minimum = dateTime;
                var maximum = dateTime.AddHours(23).AddMinutes(59).AddSeconds(59);
                var entries = connection
                    .Table<DiaryEntry>()
                    .Where(entry => entry.entryTime >= minimum && entry.entryTime <= maximum)
                    .OrderBy(entry => entry.entryTime).ToList();
                entriesCV.ItemsSource = entries;
            }
        }

        private void entryDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            LoadDiaryEntries(entryDate.Date);
        }

        private void prevDateButton_Clicked(object sender, EventArgs e)
        {
            entryDate.Date = entryDate.Date.AddDays(-1);
            LoadDiaryEntries(entryDate.Date);
        }

        private void nextDateButton_Clicked(object sender, EventArgs e)
        {
            entryDate.Date = entryDate.Date.AddDays(1);
            LoadDiaryEntries(entryDate.Date);
        }

        private void addButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewDiaryEntryPage(entryDate.Date));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadDiaryEntries(entryDate.Date);
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            var entry = item.BindingContext as DiaryEntry;

            using (SQLiteConnection connection = new SQLiteConnection(App.DBFolder))
            {
                connection.CreateTable<DiaryEntry>();
                connection.Delete(entry);
            }
            LoadDiaryEntries(entryDate.Date);
        }

        private void SwipeItem_Edit(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            var entry = item.BindingContext as DiaryEntry;
            Navigation.PushAsync(new NewDiaryEntryPage(entryDate.Date, entry));
        }
    }
}