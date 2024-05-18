using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;

namespace BabyDiary.Models
{
    public class DiaryEntry
    {
        public enum TYPE
        {
            DRINK, DIAPERS, SLEEP
        }
        [PrimaryKey, AutoIncrement]
        public long Sid { get; set; }
        public int DrinkAmount { get; set; }
        public string PoopInfo { get; set; }
        public TYPE Type { get; set; }
        public string SpecialInfo { get; set; }
        public DateTime EntryTime { get; set; }
        public int SleepTime { get; set; }
        public string DayString
        {
            get
            {
                return EntryTime.Day + "-" + EntryTime.Month + "-" + EntryTime.Year;
            }
        }
        public string Icon
        {
            get
            {
                {
                    if (Type == TYPE.DRINK)
                    {
                        return "babyflasche.png";
                    }
                    else if(Type == TYPE.DIAPERS)
                    {
                        return "windel.png";
                    } else
                    {
                        return "babyschlaf.png";
                    }
                }
            }
        }

        public string StatusText { get
            {
                switch(Type)
                { 
                    case TYPE.DRINK:
                        return DrinkAmount + " ml";
                    case TYPE.DIAPERS:
                        return PoopInfo;
                    case TYPE.SLEEP:
                        return SleepTime + " Minutes";
                    default:
                        return string.Empty;
                }
            } 
        }
    }
}
