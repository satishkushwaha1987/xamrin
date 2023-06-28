using CHBackOffice.ApiServices.ChsProxy;

namespace CHSBackOffice.Support
{
    public class FactoryMatcher
    {
        #region Methods

        public static BaseMatcher<string, string> GetTransactionStatusToColorMatcher()
        {
            return new TransactionStatusToColorMatcher();
        }

        public static BaseMatcher<SeverityType, string> GetSeverityTypeToColorMatcher()
        {
            return new SeverityTypeToColorMatcher();
        }

        public static BaseMatcher<string, string> GetActiveFloatToColorMatcher()
        {
            return new ActiveFloatStatusToColorMatcher();
        }
        #endregion
    }
}
