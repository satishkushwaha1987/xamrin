using System;
using Prism.Events;

namespace CHSBackOffice.Events
{
    public class SettingsStateChanged<T> : PubSubEvent<T>
    {
        internal void Subscribe()
        {
            throw new NotImplementedException();
        }
    }
}
