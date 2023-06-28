using System;
using System.Collections.Generic;
using System.Text;

namespace CHBackOffice.ApiServices
{
    public enum MachineState
    {
        Inservice_Online_Normal,
        Inservice_Online_Warning,
        Inservice_Online,
        Offline,
        SOP_OOS_By_Command,
        Others
    }
}
