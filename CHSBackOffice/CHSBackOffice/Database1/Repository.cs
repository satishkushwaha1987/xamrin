using CHSBackOffice.Database.Models;
using CHSBackOffice.Support;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
