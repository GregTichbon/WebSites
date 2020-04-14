using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generic;

namespace TeOraHouWhanganui.Private.Administration
{
    public partial class Program : System.Web.UI.Page
    {
        public string fld_program = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

        Dictionary<string, string> programs = new Dictionary<string, string>();
            programs = Functions.buildselectionlist(connectionString, "select ID as [Value], ProgramName as [Label] from Program order by programName", options);
            string[] selectedoptions = { };
            fld_program = Functions.buildselection(programs, selectedoptions, options);

        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {

        }
    }
}