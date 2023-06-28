using CHSBackOffice.Database;
using CHSBackOffice.Views;
using System;
using System.Collections.Generic;

namespace CHSBackOffice.Support
{
    class PagesPermissions
    {
        static Dictionary<Type, Func<bool>> Data = new Dictionary<Type, Func<bool>>
        {
            { typeof(MachineStatusSkiaPage), () => { return StateInfoService.UserPermissions.Dashboard.Status; } },
            { typeof(EventsPage), () => { return StateInfoService.UserPermissions.Dashboard.Events; } },
            { typeof(DoorOpenEventsPage), () => { return StateInfoService.UserPermissions.Dashboard.Events; } },
            { typeof(CashOnHandPage), () => { return StateInfoService.UserPermissions.Dashboard.Cash; } },
            { typeof(CashUtilizationPage), () => { return StateInfoService.UserPermissions.Dashboard.Cash; } },
            { typeof(TransactionsPage), () => { return StateInfoService.UserPermissions.Dashboard.Transactions; } },
            { typeof(TransactionByTypePage), () => { return StateInfoService.UserPermissions.Dashboard.Transactions; } },
            { typeof(MachineAvallabilityPage), () => { return StateInfoService.UserPermissions.Dashboard.Availability; } },
            { typeof(DashboardPage), () => { return StateInfoService.UserPermissions.Dashboard.View; } },
            { typeof(RemoteControlPage), () => { return StateInfoService.UserPermissions.RemoteControl.View; } },
            { typeof(ActiveFloatsPage), () => { return StateInfoService.UserPermissions.ActiveFloat.View; } },
            { typeof(SystemParametersPage), () => { return StateInfoService.UserPermissions.Parameters.View; } },
            { typeof(AllTransactionsPage), () => { return StateInfoService.UserPermissions.Transaction.View; } },
            { typeof(UsersPage), () => { return StateInfoService.UserPermissions.Employee.View; } },
            { typeof(SOPUsersPage), () => { return StateInfoService.UserPermissions.Employee.View; } },
            { typeof(EmployeesPage), () => { return StateInfoService.UserPermissions.Employee.View; } },
            { typeof(EmployeeManagmentPage), () => { return StateInfoService.UserPermissions.Employee.View; } },
            { typeof(SettingsPage), () => { return true; } },
            { typeof(LoginPage), () => { return true; } },
        };

        internal static bool CheckRights(Type pageType)
        {
            try
            {
                return PagesPermissions.Data.ContainsKey(pageType) && ((bool)PagesPermissions.Data[pageType].Invoke());
            }
            catch(Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return false;
            }
        }

        internal static Type GetFirstAllowedPage()
        {
            foreach (var t in Data)
                if (CheckRights(t.Key))
                    return t.Key;
            return null;
        }
    }
}
