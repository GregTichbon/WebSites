using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeOraHouWhanganui.TestAndPlay
{
    public partial class Email1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string host = "smtp.office365.com";
            string emailfrom = "noreply@teorahou.org.nz"; // "noreply@teorahou.org.nz";
            //emailfrom = "gtichbon@teorahou.org.nz";
            string password = "WhanganuiInc1998"; // "Whanganui1998";
            //password = "Linus007t";
            int port = 587;
            Boolean enableSsl = true;
            string emailfromname = "Test Email";
            string emailBCC = "";
            string emailRecipient = "whanganui@teorahou.org.nz";
            string emailsubject = "Test Email";
            string emailhtml = "<html><head></head><body>Greg was here</body></html>";
            string[] attachments = new string[0];
            Dictionary<string, string> emailoptions = new Dictionary<string, string>();

            Generic.Functions gFunctions = new Generic.Functions();
            gFunctions.sendemailV5(host, port, enableSsl, emailfrom, emailfromname, password, emailsubject, emailhtml, emailRecipient, emailBCC, "", attachments, emailoptions);

        }
    }
}