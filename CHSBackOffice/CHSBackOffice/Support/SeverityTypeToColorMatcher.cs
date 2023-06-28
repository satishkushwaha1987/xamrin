using System.Collections.Generic;
using CHBackOffice.ApiServices.ChsProxy;

namespace CHSBackOffice.Support
{
    public class SeverityTypeToColorMatcher : BaseMatcher<SeverityType, string>
    {
        #region Public Properties
        public override Dictionary<SeverityType, string> KeyValuePairs =>
            new Dictionary<SeverityType, string>
            {
                { SeverityType.Information,"SeverityInformation"},
                { SeverityType.CriticalO,"SeverityCritical0"},
                { SeverityType.CriticalB,"SeverityCriticalB"},
                { SeverityType.Warning,"SeverityWarning"}
            };

        public override string DefaultValue => "SeverityInformation";
        #endregion
    }
}
