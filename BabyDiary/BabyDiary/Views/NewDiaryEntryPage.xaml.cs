﻿using BabyDiary.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            drinkAmount.SelectedIndex = 4;
            mDay = day;
        }

        public NewDiaryEntryPage(DateTime day, DiaryEntry entry)
        {
            InitializeComponent();
            mEntry = entry;
            mDay = day;
            specialText.Text = mEntry.specialInfo;
            newEntryTime.Time = new TimeSpan(mEntry.entryTime.Hour, mEntry.entryTime.Minute, 0);
            if (mEntry.type.Equals("drink")) {
                changeType(Type.DRINK);
                drinkAmount.SelectedItem = mEntry.text;
            }
            else if(mEntry.type.Equals("diapers"))
            {
                changeType(Type.DIAPERS);
                poopMode.SelectedItem = mEntry.text;
            }
            else
            {
                changeType(Type.SLEEP);
            }
            newEntryAddButton.Text = "Save";
            Title = "Edit Diary Entry";
        }

        private void changeType(Type type)
        {
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
                entryIcon.Source = "none";
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
                entry = new DiaryEntry() {
                    specialInfo = specialText.Text,
                    type = !ButtonTypeDrink.IsEnabled ? "drink" : "diapers",
                    text = !ButtonTypeDrink.IsEnabled ? drinkAmount.SelectedItem.ToString() : poopMode.SelectedItem.ToString()
                };
            } 
            else
            {
                entry = mEntry;
                entry.specialInfo = specialText.Text;
                entry.type = !ButtonTypeDrink.IsEnabled ? "drink" : "diapers";
                entry.text = !ButtonTypeDrink.IsEnabled ? drinkAmount.SelectedItem.ToString() : poopMode.SelectedItem.ToString();
            }
            DateTime dateTime = mDay;
            dateTime = dateTime.AddHours(newEntryTime.Time.Hours);
            dateTime = dateTime.AddMinutes(newEntryTime.Time.Minutes);
            entry.entryTime = dateTime;
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