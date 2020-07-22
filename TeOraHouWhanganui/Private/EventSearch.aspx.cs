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
using Generic;
using localfunctions = TeOraHouWhanganui._Dependencies;

namespace TeOraHouWhanganui.Private
{
    public partial class EventSearch : System.Web.UI.Page
    {
        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };
        public Dictionary<string, string> programs = new Dictionary<string, string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            options.Clear();
            options.Add("storedprocedure", "");
            options.Add("storedprocedurename", "");
            options.Add("usevalues", "");
            //options.Add("insertblank", "start");
            programs = Functions.buildselectionlist(connectionString, "get_programs", options);
        }
    }
}