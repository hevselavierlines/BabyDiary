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
        public string text { get; set; }
        public string type { get; set; }
        public string specialInfo { get; set; }
        public DateTime entryTime { get; set; }
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
                    else
                    {
                        return "windel.png";
                    }
                }
            }
        }
    }
}
