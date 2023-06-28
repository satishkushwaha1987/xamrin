using System;

namespace CHSBackOffice.Support.ApexChart
{
    public sealed class State
    {
        const string yesterday = "yesterday";
        public static string Yesterday => yesterday;

        const string days7 = "7days";
        public static string Days7 => days7;

        const string days31 = "31days";
        public static string Days31 => days31;

        const string days366 = "366days";
        public static string Days366 => days366;

        const string machine_group = "machine_group";
        public static string MachineGroup => machine_group;

        const string machine = "machine";
        public static string Machine => machine;

        public static string GetLabel(string state)
        {
            switch (state)
            {
                case yesterday:
                    return "Yesterday";
                case days7:
                    return "7 Days";
                case days31:
                    return "31 Days";
                case days366:
                    return "366 Days";
                case machine_group:
                    return "Machine Group";
                case machine:
                    return "Machine";
                default:
                    return String.Empty;
            }
        }
    }
}
