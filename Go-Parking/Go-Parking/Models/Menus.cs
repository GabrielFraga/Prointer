using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace System.Web.Mvc
{
    public static class Menus
    {
        public static string GetUserRole(this HtmlHelper html)
        {
            string CurrentUserRole = "Admin";
            return CurrentUserRole;
        }
    }
}