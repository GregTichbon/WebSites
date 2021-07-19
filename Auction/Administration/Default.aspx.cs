using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Auction.Administration
{
   
    public partial class Default : System.Web.UI.Page
    {
        public Dictionary<string, string> parameters;
        protected void Page_Load(object sender, EventArgs e)
        {
            parameters = _Dependencies.Functions.functions.get_Auction_Parameters(Request.Url.AbsoluteUri);

        }
    }
}