using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CHSBackOffice.Support
{
    public class ValidationHelper
    {
        public static Dictionary<ErrorReason, string> Messages { get; set; } = new Dictionary<ErrorReason, string>
        {
            { ErrorReason.HasNotNumberUpperChar, Resources.Resource.HasNotNumberUpperChar },
            { ErrorReason.ArentDigits, Resources.Resource.ArentDigits},
            { ErrorReason.CharacterLongError,Resources.Resource.CharacterLongError}
            
        };

        private static ValidationObject _validationResult;
        public static ValidationObject ValidationResult
        {
            set { _validationResult = value; }
            get
            {
                if (_validationResult == null)
                {
                    _validationResult = new ValidationObject();
                    _validationResult.FailureList = new List<ValidationFailure>();
                }
                return _validationResult;
            }
        }

        public static ValidationFailure ValidateProperty(string propName, string propValue)
        {
            ValidationFailure res = null;
            var hasMinMaxChar = new Regex(@"^.{4,15}$");
            if ("NewPassword".Equals(propName) && !string.IsNullOrEmpty(propValue))
            {
                if (!string.IsNullOrEmpty(propValue))
                {
                    var hasNumber = new Regex(@"[0-9]+");
                    var hasUpperChar = new Regex(@"[A-Z]+");
                    if (!(hasNumber.IsMatch(propValue) &
                          hasUpperChar.IsMatch(propValue)))
                    {
                        var failure = new ValidationFailure
                        {
                            Reason = FailureCode.Invalid,
                            PropertyName = propName,
                            ErrorMessage = Messages[ErrorReason.HasNotNumberUpperChar]
                        };
                        res = failure;
                    }
                    if (!(hasMinMaxChar.IsMatch(propValue)))
                    {
                        var failure = new ValidationFailure
                        {
                            Reason = FailureCode.Invalid,
                            PropertyName = propName,
                            ErrorMessage = Messages[ErrorReason.CharacterLongError]
                        };
                        res = failure;
                    }
                }
                return res;
            }
            if ("NewSopOrEmplPassword".Equals(propName) && !string.IsNullOrEmpty(propValue))
            {
                if (!string.IsNullOrEmpty(propValue))
                {
                    var onlyDigits = new Regex(@"^\d+$");
                   
                    if (!onlyDigits.IsMatch(propValue))
                    {
                        var failure = new ValidationFailure
                        {
                            Reason = FailureCode.Invalid,
                            PropertyName = propName,
                            ErrorMessage = Messages[ErrorReason.ArentDigits]
                        };
                        res = failure;
                    }
                    if (!(hasMinMaxChar.IsMatch(propValue)))
                    {
                        var failure = new ValidationFailure
                        {
                            Reason = FailureCode.Invalid,
                            PropertyName = propName,
                            ErrorMessage = Messages[ErrorReason.CharacterLongError]
                        };
                        res = failure;
                    }
                }
                return res;
            }
            if (string.IsNullOrEmpty(propValue))
            {
                var failure = new ValidationFailure
                {
                    Reason = FailureCode.Empty,
                    PropertyName = propName
                };
                res = failure;
            }
            return res;
        }

    }

    public class ValidationObject
    {
        public List<ValidationFailure> FailureList { get; set; }
    }

    public class ValidationFailure
    {
        public FailureCode Reason { get; set; }
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum FailureCode
    {
        Empty,
        Invalid
    }

    public enum ErrorReason
    {
        HasNotNumberUpperChar,
        CharacterLongError,
        ArentDigits
    }
}
