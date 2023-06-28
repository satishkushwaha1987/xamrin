using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHSBackOffice.Support
{
    public abstract class BaseMatcher<Key, Value>
    {
        #region Properties
        public abstract Dictionary<Key, Value> KeyValuePairs
        {
            get;
        }

        public abstract Value DefaultValue { get; }

        public virtual Key DefaultKey { get => default(Key); }
        #endregion

        #region Methods
        public Key FromValue(Value param)
        {
            if (param == null)
            {
                return DefaultKey;
            }
            return KeyValuePairs.ContainsValue(param) ?
                KeyValuePairs.FirstOrDefault(x => x.Value.Equals(param)).Key : DefaultKey;
        }

        public Value ToValue(Key param)
        {
            if (param == null)
            {
                return DefaultValue;
            }
            return KeyValuePairs.ContainsKey(param) ?
                KeyValuePairs[param] : DefaultValue;
        }
        #endregion
    }
}
