using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabyDiary.Models
{
    public class UpdateStackEntry
    {
        [PrimaryKey, AutoIncrement]
        public long Sid { get; set; }
        public enum MODE
        {
            INSERT, UPDATE, DELETE
        }
        public MODE Mode { get; set; }
        public DiaryEntry DiaryEntry { get; set; } 
    }
}
