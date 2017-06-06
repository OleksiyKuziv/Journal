using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using journal.Models;
using System.Security.Principal;

namespace journal.Filters
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
    public class MyAuthorizeAttribute:AuthorizeAttribute
    {
        public UserRole[] RoleList { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }
            if((RoleList.Length>0)&&!RoleList.Select(p=>p.ToString()).Any<string>(new Func<string, bool>(user.IsInRole)))
            {
                return false;
            }
            return true;
        }
    }
}