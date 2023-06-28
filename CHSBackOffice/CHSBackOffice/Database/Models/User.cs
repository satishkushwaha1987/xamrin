using CHSBackOffice.Support;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHSBackOffice.Database.Models
{
    class User : SQLiteParent
    {
        public string DbUserName { get; set; }
        public string DbPassword { get; set; }
        public string DbStartPage { get; set; }
        public DateTime LastLoginDateTime { get; set; }
        public bool EnableRememberme { get; set; }

        [Ignore]
        public string UserName
        {
            get => String.IsNullOrEmpty(DbUserName) ? String.Empty : Crypto.Decrypt(DbUserName);
            set => DbUserName = Crypto.Encrypt(value);
        }

        [Ignore]
        public string Password
        {
            get => String.IsNullOrEmpty(DbPassword) ? String.Empty : Crypto.Decrypt(DbPassword);
            set => DbPassword = String.IsNullOrEmpty(value) ? String.Empty : Crypto.Encrypt(value);
        }

        [Ignore]
        public string StartPage
        {
            get => String.IsNullOrEmpty(DbStartPage) ? String.Empty : DbStartPage;
            set => DbStartPage = value;
        }

    }
}
