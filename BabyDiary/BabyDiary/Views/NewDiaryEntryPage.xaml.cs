using BabyDiary.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace BabyDiary.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewDiaryEntryPage : ContentPage
	{
        enum Type { 
            DRINK, DIAPERS, SLEEP 
        };
        Type mType;
        DateTime mDay;
        DiaryEntry mEntry;
		public NewDiaryEntryPage (DateTime day)
		{
			InitializeComponent ();
            newEntryTime.Time = DateTime.Now.TimeOfDay;
            changeType(Type.DRINK);
            poopMode.SelectedIndex = 0;
            DrinkAmount.Text = "180";
            mDay = day;
        }

        public NewDiaryEntryPage(DateTime day, DiaryEntry entry)
        {
            InitializeComponent();
            mEntry = entry;
            mDay = day;
            specialText.Text = mEntry.SpecialInfo;
            newEntryTime.Time = new TimeSpan(mEntry.EntryTime.Hour, mEntry.EntryTime.Minute, 0);
            if (mEntry.Type == DiaryEntry.TYPE.DRINK) {
                changeType(Type.DRINK);
                DrinkAmount.Text = mEntry.DrinkAmount.ToString();
            }
            else if(mEntry.Type == DiaryEntry.TYPE.DIAPERS)
            {
                changeType(Type.DIAPERS);
                poopMode.SelectedItem = mEntry.PoopInfo;
            }
            else
            {
                changeType(Type.SLEEP);
                SleepAmount.Text = mEntry.SleepTime.ToString();
            }
            newEntryAddButton.Text = "Save";
            Title = "Edit Diary Entry";
        }

        private void changeType(Type type)
        {
            mType = type;
            if(type == Type.DRINK) 
            {
                ButtonTypeDrink.IsEnabled = false;
                ButtonTypeDiapers.IsEnabled = true;
                ButtonTypeSleep.IsEnabled = true;
                drinkGrid.IsVisible = true;
                diapersGrid.IsVisible = false;
                sleepGrid.IsVisible = false;
                entryIcon.Source = "babyflasche.png";
            } 
            else if(type == Type.DIAPERS)
            {
                ButtonTypeDrink.IsEnabled = true;
                ButtonTypeDiapers.IsEnabled = false;
                ButtonTypeSleep.IsEnabled = true;
                drinkGrid.IsVisible = false;
                diapersGrid.IsVisible = true;
                sleepGrid.IsVisible= false;
                entryIcon.Source = "windel.png";
            } 
            else
            {
                ButtonTypeDrink.IsEnabled = true;
                ButtonTypeDiapers.IsEnabled = true;
                ButtonTypeSleep.IsEnabled = false;
                drinkGrid.IsVisible = false;
                diapersGrid.IsVisible = false;
                sleepGrid.IsVisible = true;
                entryIcon.Source = "babyschlaf.png";
            }
        }

        private void ButtonTypeDrink_Clicked(object sender, EventArgs e)
        {
            changeType(Type.DRINK);
        }

        private void ButtonTypeDiapers_Clicked(object sender, EventArgs e)
        {
            changeType(Type.DIAPERS);
        }

        private void ButtonTypeSleep_Clicked(object sender, EventArgs e)
        {
            changeType(Type.SLEEP);
        }

        private void newEntryAddButton_Clicked(object sender, EventArgs e)
        {
            DiaryEntry entry;
            if (mEntry == null) {
                entry = new DiaryEntry();
            } 
            else
            {
                entry = mEntry;
            }
            entry.SpecialInfo = specialText.Text;
            switch (mType)
            {
                case Type.DRINK:
                    entry.Type = DiaryEntry.TYPE.DRINK;
                    entry.DrinkAmount = int.Parse(DrinkAmount.Text);
                    break;
                case Type.DIAPERS:
                    entry.Type = DiaryEntry.TYPE.DIAPERS;
                    entry.PoopInfo = poopMode.SelectedItem + "";
                    break;
                case Type.SLEEP:
                    entry.Type = DiaryEntry.TYPE.SLEEP;
                    entry.SleepTime = int.Parse(SleepAmount.Text);
                    break;
            } 
            DateTime dateTime = mDay;
            dateTime = dateTime.AddHours(newEntryTime.Time.Hours);
            dateTime = dateTime.AddMinutes(newEntryTime.Time.Minutes);
            entry.EntryTime = dateTime;
            using (SQLiteConnection connection = new SQLiteConnection(App.DBFolder))
            {

                connection.CreateTable<DiaryEntry>();
                if (mEntry == null)
                {
                    connection.Insert(entry);
                } else
                {
                    connection.Update(entry);
                }
            }
            Navigation.PopAsync();
        }
    }
}