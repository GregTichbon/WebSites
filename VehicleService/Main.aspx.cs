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
namespace VehicleService
{
    public partial class Main1 : System.Web.UI.Page
    {
        public string username = "";
        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };
        public Dictionary<string, string> customertypes = new Dictionary<string, string>();
        public Dictionary<string, string> wofcycles = new Dictionary<string, string>();
        public Dictionary<string, string> vehicletypes = new Dictionary<string, string>();
        public Dictionary<string, string> vehiclemodels = new Dictionary<string, string>();
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

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            options.Clear();
            options.Add("storedprocedure", "");
            options.Add("storedprocedurename", "");
            //options.Add("parameters", "|countcustomers|");
            options.Add("usevalues", "");
            //options.Add("insertblank", "start");
            //customertypes = Functions.buildselectionlist(connectionString, "get_customertypes", options);

            options.Clear();
            options.Add("storedprocedure", "");
            options.Add("storedprocedurename", "");
            //options.Add("parameters", "|countcustomers|");
            options.Add("usevalues", "");
            //options.Add("insertblank", "start");
            vehicletypes = Functions.buildselectionlist(connectionString, "get_vehicletypes", options);

            options.Clear();
            options.Add("storedprocedure", "");
            options.Add("storedprocedurename", "");
            //options.Add("parameters", "|countcustomers|");
            options.Add("usevalues", "");
            //options.Add("insertblank", "start");
            vehiclemodels = Functions.buildselectionlist(connectionString, "get_vehiclemodels", options);


        }
    }
}