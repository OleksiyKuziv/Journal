using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace journal.Hubs
{
    public class LiveMessage : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}