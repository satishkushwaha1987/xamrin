using CHSBackOffice.Database;
using CHSBackOffice.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CHSBackOffice.Support
{
    public static class Settings
    {
        internal static void InitSettings()
        {
            RememberMe = new Settings.SettingInfo<bool>(nameof(RememberMe), false, Resources.Resource.RememberMe);
            ServerAddress = new SettingInfo<string>(nameof(ServerAddress), "", "");

            AutoRefresh = new SettingInfo<bool>(nameof(AutoRefresh), false, "", isAutoSave: true);
            
            IgnoreCertificate = new SettingInfo<bool>(nameof(IgnoreCertificate), false, "", isAutoSave: true);
            IgnoreCertificate.ValueChangedSyncAction = new Action<bool>((newVal)
                 =>
                {
                    if (newVal)
                        ServicePointManager.ServerCertificateValidationCallback += CheckCert;
                    else
                    {
                        ServicePointManager.ServerCertificateValidationCallback -= CheckCert;
                        ServicePointManager.MaxServicePoints = 1;
                        ServicePointManager.MaxServicePointIdleTime = 1;
                    }
                });

        }

        static bool CheckCert(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static SettingInfo<bool> RememberMe;
        public static SettingInfo<bool> IgnoreCertificate;
        public static SettingInfo<bool> AutoRefresh;
        public static SettingInfo<string> ServerAddress;

        #region Work with settings impl


        public abstract class EnabledNotifyPropertyChanged : INotifyPropertyChanged
        {
            internal void NotifyPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public event PropertyChangedEventHandler PropertyChanged;

        }

        public class SettingInfo<T> : EnabledNotifyPropertyChanged
        {
            private List<EnabledNotifyPropertyChanged> _objectsToNotify;
            private SettingInfo<bool> _dependentOf { get; set; }
            private IEventAggregator _sharedEventAggregator;
            public IEventAggregator SharedEventAggregator
            {
                get { return _sharedEventAggregator; }
                set
                {
                    _sharedEventAggregator = value;
                    _sharedEventAggregator?.GetEvent<SettingsStateChanged<T>>().Subscribe(OnSettingsStateChanged);
                }
            }
            internal void RegisterObjectForChangeNotify(EnabledNotifyPropertyChanged value)
            {
                if (value != null)
                    _objectsToNotify.Add(value);

            }

            public SettingInfo(string key, T defaultValue, string category, SettingInfo<bool> dependentOf = null, bool isAutoSave= true)
            {
                _objectsToNotify = new List<EnabledNotifyPropertyChanged>();
                Key = key;
                _dependentOf = dependentOf;
                if (_dependentOf != null)
                    _dependentOf.RegisterObjectForChangeNotify(this);

                Category = category;
                IsAutoSave = isAutoSave;
                DefaultValue = defaultValue;
                if (!Repository.SettingHasValue(Key))
                    Value = DefaultValue;

            }

            private bool _userInteraction = true;
            
            public Action<T> ValueChangingSyncAction;

            public Action<T> ValueChangedSyncAction { get; set; }
            public Action<T> ValueChangedAsyncAction { get; set; }

            public string Key { get; private set; }
            public T DefaultValue { get; private set; }
            public int? MinValue { get; set; }
            public int? MaxValue { get; set; }

            public string Category { get; set; }
            public bool IsAutoSave { get; set; }
            public T Value
            {
                get
                {
                    LastValue = Repository.GetSetting<T>(Key);

                    if (typeof(T) == typeof(int))
                    {
                        if (MinValue != null)
                            if (LastValue as int? < MinValue)
                                LastValue = (T)(object)MinValue;

                        if (MaxValue != null)
                            if (LastValue as int? > MaxValue)
                                LastValue = (T)(object)MaxValue;
                    }

                    return LastValue;
                }
                set
                {
                    try
                    {
                        T oldValue = Value;
                        T newValue = value;


                        if (typeof(T) == typeof(int))
                        {
                            if (MinValue != null)
                                if (newValue as int? < MinValue)
                                    newValue = (T)(object)MinValue;

                            if (MaxValue != null)
                                if (newValue as int? > MaxValue)
                                    newValue = (T)(object)MaxValue;
                        }

                        if (oldValue != null && newValue != null && !oldValue.Equals(newValue))
                        {
                            if (ValueChangingSyncAction != null && _userInteraction)
                            {
                                ValueChangingSyncAction.Invoke(Value);
                            }
                        }

                        if(IsAutoSave)
                            Repository.SetSetting(Key, newValue);

                        LastValue = newValue;

                        NotifyPropertyChanged(nameof(Value));
                        NotifyPropertyChanged(nameof(BoolValue));
                        NotifyPropertyChanged(nameof(StringValue));
                        foreach (var ob in _objectsToNotify)
                            ob.NotifyPropertyChanged(nameof(EnabledToChange));

                        if (oldValue != null && newValue != null && !oldValue.Equals(newValue))
                        {
                            if (ValueChangedAsyncAction != null)
                                ValueChangedAsyncAction.BeginInvoke(Value, null, this);

                            if (ValueChangedSyncAction != null)
                                ValueChangedSyncAction.Invoke(Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcessor.ProcessException(ex);
                    }
                }

            }

            public List<T> AllowedValues { get; set; }

            private void OnSettingsStateChanged(T obj)
            {
                _userInteraction = false;
                Value = obj;
                _userInteraction = true;

            }
            private bool _lastValueReaded = false;
            private T _lastValue;
            public T LastValue
            {
                get
                {
                    if (!_lastValueReaded)
                        return Value;
                    return _lastValue;
                }
                set
                {
                    _lastValue = value;
                    _lastValueReaded = true;
                }
            }

            public T BoolValue
            {
                get => Value;
                set
                {
                    if (SettingType == typeof(bool))
                        Value = value;
                }
            }

            public T StringValue
            {
                get => Value;
                set
                {
                    if (SettingType != typeof(bool))
                        Value = value;
                }
            }

            public Type SettingType => typeof(T);
            public string Description
            {
                get
                {
                    string result = null;
                    try
                    {
                        result = Resources.Resource.ResourceManager.GetString(Key);
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcessor.ProcessException(ex);
                    }
                    return result ?? "Not found";
                }
            }

            private bool _enabledToChange = true;
            public bool EnabledToChange
            {
                get => (_dependentOf == null ? true : _dependentOf.Value) && _enabledToChange;
                set => _enabledToChange = value;
            }

        }

        #endregion
    }
}
