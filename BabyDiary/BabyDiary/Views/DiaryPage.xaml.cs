using BabyDiary.Models;
using Firebase.Database;
using Firebase.Database.Query;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
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

        
        public async Task<bool> synchronizeFromOnlineDB(FirebaseClient firebaseClient)
        {
            try
            {

                var diaryList = await
                    firebaseClient.Child("babyDiary")
                    .OnceAsync<DiaryEntry>();
                using (SQLiteConnection connection = new SQLiteConnection(App.DBFolder))
                {
                    diaryList.ForEach(diaryEntry =>
                    {
                        var updateObject = connection.Table<DiaryEntry>()
                        .Where(entry => entry.Sid == diaryEntry.Object.Sid).FirstOrDefault();
                        if(updateObject != null)
                        {
                            connection.Update(diaryEntry.Object);
                        } else
                        {
                            connection.Insert(diaryEntry.Object);
                        }
                    });
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
                return false;
            }
            return true;
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

            Refresh();
        }

        private void Refresh()
        {
            entriesRefresh.IsRefreshing = true;
            var firebaseClient = ((App)Application.Current).fireBaseClientObj;
            Task.Run(async () => {
                await synchronizeFromOnlineDB(firebaseClient);
                Device.BeginInvokeOnMainThread(() =>
                {
                    LoadDiaryEntries(entryDate.Date);
                });
            });
            entriesRefresh.IsRefreshing = false;
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            var entry = item.BindingContext as DiaryEntry;

            using (SQLiteConnection connection = new SQLiteConnection(App.DBFolder))
            {
                connection.CreateTable<DiaryEntry>();
                connection.Delete(entry);
                Task.Run(async () =>
                {
                    var firebaseClient = ((App)Application.Current).fireBaseClientObj;
                    var toUpdatePerson = (await
                          firebaseClient.Child("babyDiary")
                          .OnceAsync<DiaryEntry>()).Where(a => a.Object.Sid == entry.Sid).FirstOrDefault();
                    await firebaseClient.Child("babyDiary").Child(toUpdatePerson.Key).DeleteAsync();
                });
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

        private void entriesRefresh_Refreshing(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}