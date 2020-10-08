using Generic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeOraHouWhanganui.Private
{
    public partial class EmployeeMaintenance : System.Web.UI.Page
    {
        public string username = "";


        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };
        //public Dictionary<string, string> paytypes = new Dictionary<string, string>();
        public string[] paytypes = {"Casual - Timesheet", "Permanent - Timesheet", "Permanent - Salaried" };

        protected void Page_Load(object sender, EventArgs e)
        {
            // username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString().ToLower();
            username = HttpContext.Current.User.Identity.Name.ToLower();

            if (username == "")
            {
                username = "toh\\gtichbon";   //localhost
            }
            //username = HttpContext.Current.User.Identity.Name.ToLower();
            //Username += "<br />" + Environment.UserName;

        }


        
    }
}