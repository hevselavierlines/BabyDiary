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
        [PrimaryKey, AutoIncrement]
        public long sid { get; set; }
        public int DrinkAmount { get; set; }
        public string PoopInfo { get; set; }
        public string type { get; set; }
        public string specialInfo { get; set; }
        public DateTime entryTime { get; set; }
        public int sleepTime { get; set; }
        public string dayString
        {
            get
            {
                return entryTime.Day + "-" + entryTime.Month + "-" + entryTime.Year;
            }
        }
        public string Icon
        {
            get
            {
                {
                    if (type.Equals("drink"))
                    {
                        return "babyflasche.png";
                    }
                    else if(type.Equals("diapers"))
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
                if (type.Equals("sleep"))
                {
                    return sleepTime + " Minutes";
                } else if(type.Equals("drink"))
                {
                    return DrinkAmount + " ml";
                } else
                {
                    return PoopInfo;
                }
            } 
        }
    }
}
