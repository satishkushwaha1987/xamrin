using System;
using System.Collections.Generic;
using System.Text;

namespace CHSBackOffice.Support
{
    public class ActiveFloatStatusToColorMatcher : BaseMatcher<string,string>
    {
        #region Public Properties
        public override Dictionary<string, string> KeyValuePairs =>
            new Dictionary<string, string>
            {
                { "Normal","ActiveFloatNormal" },
                { "InService","ActiveFloatInService"},
                { "Critical","ActiveFloatCritical"}
            };

        public override string DefaultValue => "ActiveFloatNormal";
        #endregion
    }
}
