﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleAccounts
{
    public partial class Main : System.Web.UI.MasterPage
    {
        public string headerimage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            headerimage = ResolveUrl("~/_Dependencies/Images/CampbellAutoRepairs.png");

        }
    }
}