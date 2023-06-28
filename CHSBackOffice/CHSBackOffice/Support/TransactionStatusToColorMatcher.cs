using System;
using System.Collections.Generic;
using System.Text;

namespace CHSBackOffice.Support
{
    public class TransactionStatusToColorMatcher : BaseMatcher<string, string>
    {
        #region Public Properties
        public override Dictionary<string, string> KeyValuePairs =>
            new Dictionary<string, string>
            {
                { "Completed","StatusCompleted"},
                { "Failed","StatusFailed"},
                { "Partial Dispense","StatusPartialDispense"}
            };

        public override string DefaultValue => "StatusCompleted";
        #endregion
    }
}
