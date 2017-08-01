using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace journal.ViewModels
{
    public class UserPointViewModel
    {
        public List<UserList> userList { get; set; }
        public string subjectid { get; set; }
        public string pointLevel { get; set; }
        public string chooseData { get; set; }
        public string dataEdit { get; set; }
        
    }

    public class UserList
    {
        public Guid? user { get; set; }
        public Guid? pointValue { get; set; }

    }
}