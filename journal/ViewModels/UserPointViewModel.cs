using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace journal.ViewModels
{
    public class UserPointViewModel
    {
        public string subjectid { get; set; }
        public string pointLevel { get; set; }
        public string dataEdit { get; set; }
        public List<UserList> userList { get; set; }
    }

    public class UserList
    {
        public string user { get; set; }
        public string pointValue { get; set; }

    }
}