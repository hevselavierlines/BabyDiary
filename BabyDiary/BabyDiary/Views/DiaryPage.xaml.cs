using BabyDiary.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<SwipeView> _swipeViews;
        private ObservableCollection<DiaryEntry> _entries;
        public DiaryPage()
        {
            InitializeComponent();
            _swipeViews = new List<SwipeView>();
        }
        
        void LoadDiaryEntries(DateTime dateTime)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.DBFolder))
            {
                connection.CreateTable<DiaryEntry>();
                var minimum = dateTime;
                var maximum = dateTime.AddHours(23).AddMinutes(59).AddSeconds(59);
                _entries = new ObservableCollection<DiaryEntry>(connection
                    .Table<DiaryEntry>()
                    .Where(entry => entry.EntryTime >= minimum && entry.EntryTime <= maximum)
                    .OrderBy(entry => entry.EntryTime).ToList());
                entriesCV.ItemsSource = _entries;
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
            _entries.Remove(entry);
        }

        private void SwipeItem_Edit(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            var entry = item.BindingContext as DiaryEntry;
            Navigation.PushAsync(new NewDiaryEntryPage(entryDate.Date, entry));
        }

        private void entriesCV_ChildAdded(object sender, ElementEventArgs e)
        {
            SwipeView sw = e.Element.FindByName("SwipeViewRef") as SwipeView;
            if(sw != null)
            {
                _swipeViews.Add(sw);
            }
        }

        private void entriesCV_ChildRemoved(object sender, ElementEventArgs e)
        {
            SwipeView sw = e.Element.FindByName("SwipeViewRef") as SwipeView;
            if (sw != null)
            {
                _swipeViews.Remove(sw);
            }
        }

        private void ToolbarItemEdit_Clicked(object sender, EventArgs e)
        {
            foreach (var item in _swipeViews)
            {
                item.Open(OpenSwipeItem.LeftItems);
            }
        }
    }
}