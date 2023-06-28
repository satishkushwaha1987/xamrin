using CHSBackOffice.Database.Models;
using CHSBackOffice.Support;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHSBackOffice.Database
{
    class Repository
    {
        private static Dictionary<Type, int> SettingTypes = new Dictionary<Type, int>
        {
            { typeof(bool), 0 },
            { typeof(int), 1 },
            { typeof(string), 2 }
        };

        internal static string DatabasePath;
        static SQLiteConnection _connection;

        internal static void Init()
        {
            try
            {
                _connection = new SQLiteConnection(DatabasePath);
                _connection.CreateTable<Setting>();
                _connection.CreateTable<User>();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }


        internal static bool SettingHasValue(string key)
        {
            try
            {
                if (_connection != null)
                {
                    var retVal = _connection.Table<Setting>().Where(s => s.SettingKey == key).FirstOrDefault();
                    return retVal != null;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }

            return false;
        }

        internal static Type GetSettingType(string key)
        {
            try
            {
                if (_connection != null)
                {
                    var retVal = _connection.Table<Setting>().Where(s => s.SettingKey == key).FirstOrDefault();
                    return SettingTypes.FirstOrDefault(t => t.Value == retVal.SettingType).Key;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }

            return typeof(int);
        }

        internal static T GetSetting<T>(string key)
        {
            try
            {
                if (_connection != null)
                {
                    var retVal = _connection.Table<Setting>().Where(s => s.SettingKey == key).FirstOrDefault();
                    if (retVal != null)
                    {
                        if (typeof(T) == typeof(bool))
                            return (T)(object)(retVal.SettingValue.ToLower() == "true");

                        if (typeof(T) == typeof(int))
                            return (T)(object)Int32.Parse((retVal.SettingValue));

                        if (typeof(T) == typeof(string))
                            return (T)(object)(retVal.SettingValue);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }

            return default(T);

        }

        internal static void SetSetting<T>(string key, T value)
        {
            try
            {
                if (_connection != null)
                {

                    int type = SettingTypes.ContainsKey(typeof(T)) ? SettingTypes[typeof(T)] : -1;

                    _connection.Table<Setting>().Delete(w => w.SettingKey == key);
                    _connection.Insert(new Setting
                    {
                        SettingKey = key,
                        SettingValue = value.ToString(),
                        SettingType = type
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        internal static User LastRememberUser()
        {
            try
            {
                return _connection.Table<User>().OrderByDescending(u => u.LastLoginDateTime).Where(u => u.DbPassword != string.Empty).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        internal static User UpdateUser(User currentUser)
        {
            try
            {
                string encryptedVal = Crypto.Encrypt(currentUser.UserName);
                var findedUser = _connection.Table<User>().Where(u => u.DbUserName == encryptedVal).FirstOrDefault();
                if (findedUser != null)
                {
                    findedUser.Id = currentUser.Id;
                    findedUser.UserName = currentUser.UserName;
                    findedUser.Password = currentUser.Password;
                    findedUser.StartPage = currentUser.StartPage;
                    findedUser.EnableRememberme = currentUser.EnableRememberme;
                    findedUser.LastLoginDateTime = currentUser.LastLoginDateTime;
                    _connection.Update(findedUser);
                }
                return findedUser;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        internal static User SaveUser(string userName, string password, bool rememberme = false)
        {
            try
            {
                string encryptedVal = Crypto.Encrypt(userName);
                var findedUser = _connection.Table<User>().Where(u => u.DbUserName == encryptedVal).FirstOrDefault();
                var findedId = findedUser?.Id ?? -1;
                var findedStartPage = findedUser?.StartPage ?? string.Empty;
                var userForAddOrUpdate = new User
                {
                    Id = findedId == -1 ? 0 : findedId,
                    UserName = userName,
                    Password = password,
                    StartPage = findedStartPage,
                    EnableRememberme = rememberme,
                    LastLoginDateTime = DateTime.Now
                };

                // Clean old users passwords if we try to remember new user
                if (!String.IsNullOrEmpty(password))
                {
                    var usersWithPass = _connection.Table<User>().Where(u => u.DbUserName != encryptedVal && u.DbPassword != String.Empty).ToList();
                    if (usersWithPass.Count > 0)
                    {
                        _connection.UpdateAll(usersWithPass.Select(u => new User()
                        {
                            Id = u.Id,
                            DbUserName = u.DbUserName,
                            DbPassword = String.Empty,
                            DbStartPage = u.DbStartPage,
                            EnableRememberme = false
                        }));
                    }
                }

                if (_connection.Update(userForAddOrUpdate) == 0)
                    _connection.Insert(userForAddOrUpdate);

                return  userForAddOrUpdate;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        internal static bool AnyUserWithRememberme()
        {
            try
            {
                return _connection.Table<User>().Where(u => u.EnableRememberme == true).Any();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return false;
            }
        }
    }
}
