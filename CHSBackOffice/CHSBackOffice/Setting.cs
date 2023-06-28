using System;
using System.Collections.Generic;
using System.Text;

namespace CHSBackOffice.Database.Models
{
    class Setting : SQLiteParent
    {
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public int SettingType { get; set; }
    }
}
