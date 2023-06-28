using CHBackOffice.ApiServices.ChsProxy;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Support;
using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CHBackOffice.Service
{
    public sealed class CHSServiceAgent : ICHSServiceAgent
    {
        public string UniqId { get; set; } = Guid.NewGuid().ToString();

        #region "PRIVATE VARIABLES"

        static CHSSoapClient SoapClient;

        #endregion

        #region ".CTOR"

        public static void Init()
        {
            if (StateInfoService.HasBackOfficeHostAddress)
            {
                SoapClient = new CHSSoapClient(new BasicHttpBinding(), new EndpointAddress(Settings.ServerAddress.Value));
                SoapClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, Constants.SoapTimeoutSec);
            }
        }

        #endregion

        #region "PUBLIC METHODS"

        public void SetSoapClient(string soapUrl)
        {
            try
            {
                SoapClient = null;
                SoapClient = new CHSSoapClient(new BasicHttpBinding(), new EndpointAddress(soapUrl));
                SoapClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, Constants.SoapTimeoutSec);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
            
        }

        public async Task<bool> SwitchProperty(string sessionId, int property)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.SwitchProperty(sessionId, property));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<UserSession> GetUserProfile(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetUserProfile(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task EndSession(string sessionId)
        {
            try
            {
                await Task.Run(() => SoapClient.EndSession(sessionId));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<SessionInfo> GetSession(string username, string password)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetSession(username, password));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public bool TestConnectionSync()
        {
            var result = SoapClient.TestConnection();
            return result;
        }

        public async Task<bool> TestConnection()
        {
            try
            {
                var res = await Task.Run(() => SoapClient.TestConnection());
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<Kiosk[]> GetKioskStatus(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetKioskStatus(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<CHBackOffice.ApiServices.ChsProxy.Event[]> GetAllEvents(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetAllEvents(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<GetKioskDetailsResult> GetKioskDetails(string sessionId, string unitId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetKioskDetails(sessionId, unitId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<ParameterGroup[]> GetParameters(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetParameters(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<string> AddMachineParameter(string sessionId, string unitId, string paramId, string value)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.AddMachineParameter(sessionId, unitId, paramId, value));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<bool> UpdateMachineParameter(string sessionId, string unitId, string id, string value)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.UpdateMachineParameter(sessionId, unitId, id, value));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
            
        }

        public async Task<string[]> GetParameterNames(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetParameterNames(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<string[]> GetParameterGroupNames(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetParameterGroupNames(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<bool> AddParameter(string sessionId, string id, string group, string value, string comment)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.AddParameter(sessionId, id, group, value, comment));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<bool> UpdateParameter(string sessionId, string id, string group, string value, string comment)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.UpdateParameter(sessionId, id, group, value, comment));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<KioskGroup[]> GetMachineGroups(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetMachineGroups(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<Kiosk[]> GetMachineList(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetMachineList(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<Kiosk[]> GetMachines(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetMachines(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<bool> AddRemoteCommand(string sessionId, Command command)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.AddRemoteCommand(sessionId, command));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<bool> RemoteControl(string sessionId, CommandType cmdType, string unitIds, string comment)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.RemoteControl(sessionId, cmdType, unitIds, comment));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<Device[]> GetDevices(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetDevices(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<DeviceType[]> GetDeviceTypes(string sessionId, bool detail)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetDeviceTypes(sessionId, detail));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<UserGroup[]> GetBOGroups(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetBOGroups(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<UserGroup[]> AddBOGroups(string sessionId, UserGroup group)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.AddBOGroups(sessionId, group));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<UserGroup[]> UpdateBOGroups(string sessionId, UserGroup group)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.UpdateBOGroups(sessionId, group));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<UserGroup[]> DeleteBOGroup(string sessionId, string groupId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.DeleteBOGroup(sessionId, groupId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<CHBackOffice.ApiServices.ChsProxy.User[]> GetBOUsers(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetBOUsers(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<UserGroup[]> GetSOPGroups(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetSOPGroups(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<UserGroup[]> AddSOPGroup(string sessionId, UserGroup group)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.AddSOPGroup(sessionId, group));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<UserGroup[]> UpdateSOPGroup(string sessionId, UserGroup group)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.UpdateSOPGroup(sessionId, group));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<UserGroup[]> DeleteSOPGroup(string sessionId, string groupId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.DeleteBOGroup(sessionId, groupId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<SOPUser[]> GetSOPUsers(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetSOPUsers(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<EmployeeGroup[]> GetEmployeeGroups(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetEmployeeGroups(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<EmployeeGroup[]> AddEmployeeGroup(string sessionId, EmployeeGroup group)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.AddEmployeeGroup(sessionId, group));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<EmployeeGroup[]> UpdateEmployeeGroup(string sessionId, EmployeeGroup group)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.UpdateEmployeeGroup(sessionId, group));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<Employee[]> GetEmployees(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetEmployees(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<Employee[]> AddEmployee(string sessionId, string id, string firstName, string lastName, string group, string password, string cardNumber)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.AddEmployee(sessionId, id, firstName, lastName, group, password, cardNumber));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<Employee[]> UpdateEmployee(string sessionId, string id, string firstName, string lastName, string group, string password, string cardNumber)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.UpdateEmployee(sessionId, id, firstName, lastName, group, password, cardNumber));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<bool> Unlock(string sessionId, UserType type, string employeeId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.Unlock(sessionId, type, employeeId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<PatronTransaction[]> GetMachineTransactions(string sessionId, string unitId, int offset, int max)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetMachineTransactions(sessionId, unitId, offset, max));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<string> GetNumberOfMachineTransactions(string sessionId, string unitId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetNumberOfMachineTransactions(sessionId, unitId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<PatronTransaction[]> GetTransactions(string sessionId, bool dispute, int offset, int max)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetTransactions(sessionId, dispute, offset, max));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<PatronTransaction> ResolveDispute(string sessionId, string transId, string comment)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.ResolveDispute(sessionId, transId, comment));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<PatronTransaction[]> FindTransactions(string sessionId, bool isDispute, System.DateTime startDate, System.DateTime endDate, string criteria, int offset, int max)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.FindTransactions(sessionId, isDispute, startDate, endDate, criteria, offset, max));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<TransactionType[]> GetTransactionTypes()
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetTransactionTypes());
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<TransactionStatus[]> GetTransactionStatus()
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetTransactionStatus());
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<string> GetNumberOfFilteredTransactions(string sessionId, bool isDispute, System.DateTime startDate, System.DateTime endDate, string criteria)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetNumberOfFilteredTransactions(sessionId, isDispute, startDate, endDate, criteria));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<string> GetNumberOfTransactions(string sessionId, string criteria)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetNumberOfTransactions(sessionId, criteria));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<string> GetNumberOfDisputeTransactions(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetNumberOfDisputeTransactions(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<PatronTransaction> GetFirstTransaction(string sessionId, bool dispute)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetFirstTransaction(sessionId, dispute));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }

        }

        public async Task<PatronTransaction> GetLastTransaction(string sessionId, bool dispute)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetLastTransaction(sessionId, dispute));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<PatronTransaction> GetNextTransaction(string sessionId, string currentId, bool dispute)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetNextTransaction(sessionId, currentId, dispute));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<PatronTransaction> GetPreviousTransaction(string sessionId, string currentId, bool dispute)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetPreviousTransaction(sessionId, currentId, dispute));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<string> ChangePassword(string sessionId, UserType type, string employeeId, string currentPassword, string newPassword, bool checkCurrentPassword)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.ChangePassword(sessionId, type, employeeId, currentPassword, newPassword, checkCurrentPassword));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<DashboardTransactions> GetDashboardTransactions(string sessionId, GraphCriteria criteria)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetDashboardTransactions(sessionId, criteria));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<Transaction[][]> GetDashboardTransactionTypes(string sessionId, GraphCriteria criteria)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetDashboardTransactionTypes(sessionId, criteria));
                Transaction[][] result = res
                    .Cast<XmlNode[]>()
                    .Select(na => na
                        .Where(n => n.NodeType == XmlNodeType.Element)
                        .AsParallel()
                        .Select(n => XmlNodeToObject<Transaction>(n))
                        .ToArray()
                    ).ToArray();

                return result;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<string[][]> GetDashboardAvailability(string sessionId, GraphCriteria criteria)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetDashboardAvailability(sessionId,criteria));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<DashboardUtilization> GetDashboardUtilization(string sessionId, GraphCriteria criteria)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetDashboardUtilization(sessionId, criteria));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<DashboardCashSummary> GetDashboardCashSummary(string sessionId, int criteria)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetDashboardCashSummary(sessionId, criteria));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<DoorOpenEvent[]> GetAllOpenDoorEvents(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetAllOpenDoorEvents(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<Kiosk[]> GetDashboardMachineStatus(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetDashboardMachineStatus(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<ActiveFloat[]> GetActiveFloats(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetActiveFloats(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<string> CheckServiceURL()
        {
            try
            {
                var res = await Task.Run(() => SoapClient.CheckServiceURL());
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        public async Task<object[]> GetMachineIDs(string sessionId)
        {
            try
            {
                var res = await Task.Run(() => SoapClient.GetMachineIDs(sessionId));
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                throw;
            }
        }

        #endregion

        #region "PRIVATE METHODS"

        /// <summary>
        /// Convert XmlNode to object T
        /// </summary>
        private static T XmlNodeToObject<T>(XmlNode node) where T : class
        {
            MemoryStream stm = new MemoryStream();
            StreamWriter stw = new StreamWriter(stm);
            stw.Write(node.OuterXml);
            stw.Flush();
            stm.Position = 0;

            XmlRootAttribute xRoot = new XmlRootAttribute
            {
                ElementName = node.Name,
                Namespace = node.NamespaceURI,
                IsNullable = true
            };

            XmlSerializer ser = new XmlSerializer(typeof(T), xRoot);
            T result = (ser.Deserialize(stm) as T);

            return result;
        }

        #endregion
    }

    public enum OperationResult
    {
        Failure,
        Success,
        Invalid
        
    }
}
