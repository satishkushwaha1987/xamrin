using CHBackOffice.ApiServices.ChsProxy;
using CHSBackOffice.Support;

namespace CHSBackOffice.Database
{
    public static class StateInfoService
    {
        #region ".CTOR"

        static StateInfoService()
        {
            CurrentUser = Repository.LastRememberUser();
        }

        #endregion

        internal static Models.User CurrentUser { get; set; }
        internal static string SessionId { get; set; }
        internal static bool HasActiveSession => !string.IsNullOrEmpty(SessionId);

        #region "Curent User Profile"

        private static string _userFirstName;
        internal static string UserFirstName
        {
            get => _userFirstName;
            set
            {
                _userFirstName = value;
                CommonViewObjects.Instance.UserFirstName = value;
            }
        }

        private static string _userLastName;
        internal static string UserLastName
        {
            get => _userLastName;
            set
            {
                _userLastName = value;
                CommonViewObjects.Instance.UserLastName = value;
            }
        }

        internal static bool UserIsLocked { get; set; }
        internal static Status UserStatus { get; set; }
        internal static bool UserCanStartApp => !UserIsLocked && UserStatus == Status.Enabled;

        internal static string CurentLocationId { get; set; }
        internal static Property[] AllowedLocations { get; set; }
        internal static bool ThereAreMultipleLocations => AllowedLocations != null && AllowedLocations.Length > 1;

        internal static Permissions UserPermissions { get; set; }

        #endregion


        internal static bool HasBackOfficeHostAddress => !string.IsNullOrEmpty(Settings.ServerAddress.Value);
        internal static bool RememberMeEnabled => Settings.RememberMe.Value;

        internal static bool HasUserWithRememberme
        {
            get
            {
                return Repository.AnyUserWithRememberme();
            }
        }

        internal static void ChangeStartPage(string startPage)
        {
            CurrentUser.DbStartPage = startPage;
            CurrentUser = Repository.UpdateUser(CurrentUser);
        }

        private static T GetSettingValue<T>(string key)
        {
            return Repository.GetSetting<T>(key);
        }
       
        internal static void SetUserProfileData(UserSession userSession)
        {
            if (userSession?.User == null)
                return;

            UserFirstName = userSession.User.FirstName;
            UserLastName = userSession.User.LastName;
            UserIsLocked = userSession.User.Locked;
            UserStatus = userSession.User.Status;
            CurentLocationId = userSession.Property.ToString();
            AllowedLocations = userSession.User.Properties;
            UserPermissions = userSession.User.Permissions;
        }

        internal static void CleanCurrentSession()
        {
            SessionId = string.Empty;
        }
    }
}
