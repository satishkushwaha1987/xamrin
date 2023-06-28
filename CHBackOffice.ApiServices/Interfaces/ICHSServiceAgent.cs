
using CHBackOffice.ApiServices.ChsProxy;
using System.Threading.Tasks;

namespace CHBackOffice.ApiServices.Interfaces
{
    public interface ICHSServiceAgent
    {
        string UniqId { get; set; }
        void SetSoapClient(string soapUrl);
        Task<bool> SwitchProperty(string sessionId, int property);
        Task<UserSession> GetUserProfile(string sessionId);
        Task EndSession(string sessionId);
        Task<SessionInfo> GetSession(string username, string password);
        Task<bool> TestConnection();
        Task<Kiosk[]> GetKioskStatus(string sessionId);
        Task<CHBackOffice.ApiServices.ChsProxy.Event[]> GetAllEvents(string sessionId);
        Task<GetKioskDetailsResult> GetKioskDetails(string sessionId, string unitId);
        Task<ParameterGroup[]> GetParameters(string sessionId);
        Task<string> AddMachineParameter(string sessionId, string unitId, string paramId, string value);
        Task<bool> UpdateMachineParameter(string sessionId, string unitId, string id, string value);
        Task<string[]> GetParameterNames(string sessionId);
        Task<string[]> GetParameterGroupNames(string sessionId);
        Task<bool> AddParameter(string sessionId, string id, string group, string value, string comment);
        Task<bool> UpdateParameter(string sessionId, string id, string group, string value, string comment);
        Task<KioskGroup[]> GetMachineGroups(string sessionId);
        Task<Kiosk[]> GetMachineList(string sessionId);
        Task<Kiosk[]> GetMachines(string sessionId);
        Task<bool> AddRemoteCommand(string sessionId, Command command);
        Task<bool> RemoteControl(string sessionId, CommandType cmdType, string unitIds, string comment);
        Task<Device[]> GetDevices(string sessionId);
        Task<DeviceType[]> GetDeviceTypes(string sessionId, bool detail);
        Task<UserGroup[]> GetBOGroups(string sessionId);
        Task<UserGroup[]> AddBOGroups(string sessionId, UserGroup group);
        Task<UserGroup[]> UpdateBOGroups(string sessionId, UserGroup group);
        Task<UserGroup[]> DeleteBOGroup(string sessionId, string groupId);
        Task<CHBackOffice.ApiServices.ChsProxy.User[]> GetBOUsers(string sessionId);
        Task<UserGroup[]> GetSOPGroups(string sessionId);
        Task<UserGroup[]> AddSOPGroup(string sessionId, UserGroup group);
        Task<UserGroup[]> UpdateSOPGroup(string sessionId, UserGroup group);
        Task<UserGroup[]> DeleteSOPGroup(string sessionId, string groupId);
        Task<SOPUser[]> GetSOPUsers(string sessionId);
        Task<EmployeeGroup[]> GetEmployeeGroups(string sessionId);
        Task<EmployeeGroup[]> AddEmployeeGroup(string sessionId, EmployeeGroup group);
        Task<EmployeeGroup[]> UpdateEmployeeGroup(string sessionId, EmployeeGroup group);
        Task<Employee[]> GetEmployees(string sessionId);
        Task<Employee[]> AddEmployee(string sessionId, string id, string firstName, string lastName, string group, string password, string cardNumber);
        Task<Employee[]> UpdateEmployee(string sessionId, string id, string firstName, string lastName, string group, string password, string cardNumber);
        Task<bool> Unlock(string sessionId, UserType type, string employeeId);
        Task<PatronTransaction[]> GetMachineTransactions(string sessionId, string unitId, int offset, int max);
        Task<string> GetNumberOfMachineTransactions(string sessionId, string unitId);
        Task<PatronTransaction[]> GetTransactions(string sessionId, bool dispute, int offset, int max);
        Task<PatronTransaction> ResolveDispute(string sessionId, string transId, string comment);
        Task<PatronTransaction[]> FindTransactions(string sessionId, bool isDispute, System.DateTime startDate, System.DateTime endDate, string criteria, int offset, int max);
        Task<TransactionType[]> GetTransactionTypes();
        Task<TransactionStatus[]> GetTransactionStatus();
        Task<string> GetNumberOfFilteredTransactions(string sessionId, bool isDispute, System.DateTime startDate, System.DateTime endDate, string criteria);
        Task<string> GetNumberOfTransactions(string sessionId, string criteria);
        Task<string> GetNumberOfDisputeTransactions(string sessionId);
        Task<PatronTransaction> GetFirstTransaction(string sessionId, bool dispute);
        Task<PatronTransaction> GetLastTransaction(string sessionId, bool dispute);
        Task<PatronTransaction> GetNextTransaction(string sessionId, string currentId, bool dispute);
        Task<PatronTransaction> GetPreviousTransaction(string sessionId, string currentId, bool dispute);
        Task<string> ChangePassword(string sessionId, UserType type, string employeeId, string currentPassword, string newPassword, bool checkCurrentPassword);
        Task<DashboardTransactions> GetDashboardTransactions(string sessionId, GraphCriteria criteria);
        Task<Transaction[][]> GetDashboardTransactionTypes(string sessionId, GraphCriteria criteria);
        Task<string[][]> GetDashboardAvailability(string sessionId, GraphCriteria criteria);
        Task<DashboardUtilization> GetDashboardUtilization(string sessionId, GraphCriteria criteria);
        Task<DashboardCashSummary> GetDashboardCashSummary(string sessionId, int criteria);
        Task<DoorOpenEvent[]> GetAllOpenDoorEvents(string sessionId);
        Task<Kiosk[]> GetDashboardMachineStatus(string sessionId);
        Task<ActiveFloat[]> GetActiveFloats(string sessionId);
        Task<string> CheckServiceURL();
        Task<object[]> GetMachineIDs(string sessionId);
    }
}
