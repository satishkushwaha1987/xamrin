using System;
using System.Collections.Generic;
using System.Text;

namespace CHBackOffice.ApiServices
{
    public class User
    {
        public KeyValuePair<string, string> UserId { get; set; }
        public KeyValuePair<string, string> Name { get; set; }
        public KeyValuePair<string, string> Password { get; set; }
        public KeyValuePair<string, string> ConfirmPassword { get; set; }
        public KeyValuePair<string, string> UserGroup { get; set; }
    }
}
