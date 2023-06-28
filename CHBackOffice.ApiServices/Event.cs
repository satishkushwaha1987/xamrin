using System;
using System.Collections.Generic;
using System.Text;

namespace CHBackOffice.ApiServices
{
    public class Event
    {
        public KeyValuePair<string,string> Unit { get; set; }
        public KeyValuePair<string, string> ErrorCode { get; set; }
        public KeyValuePair<string, string> EventDescription { get; set; }
        public KeyValuePair<string, string> EventTimestamp { get; set; }
        public KeyValuePair<string, string> Severity { get; set; }
        public KeyValuePair<string, string> Group { get; set; }
        public KeyValuePair<string, string> CauseOfError { get; set; }
        public KeyValuePair<string, string> Service { get; set; }
        public KeyValuePair<string, string> EmailNotyfyStatus { get; set; }
    }
}
