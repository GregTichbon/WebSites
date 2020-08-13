using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace VehicleService
{
    public partial class Import : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("C:\\Users\\gtichbon\\Source\\Repos\\WebSites\\VehicleService\\Imports\\CampbellContacts.xml");

            XmlNodeList nodeListContact = xmlDoc.SelectNodes("//Contact");
            foreach (XmlNode NodeContact in nodeListContact)
            {
                //Response.Write(NodeContact.Name + "<br />");
                if (NodeContact["IsCustomer"].InnerText == "true")
                {
                    Response.Write(NodeContact["ContactID"].InnerText + "<br />");  //guid
                    Response.Write(NodeContact["Name"].InnerText + "<br />");
                    Response.Write(NodeContact["UpdatedDateUTC"].InnerText + "<br />");

                    XmlNodeList nodeListContactPhones = NodeContact.SelectNodes("Phones");
                    foreach (XmlNode NodeContactPhones in nodeListContactPhones)
                    {
                        XmlNodeList nodeListContactPhone = NodeContactPhones.SelectNodes("Phone");
                        foreach (XmlNode NodeContactPhone in nodeListContactPhone)
                        {
                            string phoneNumber = "";
                            if (NodeContactPhone.SelectSingleNode("PhoneAreaCode") != null)
                            {
                                phoneNumber = NodeContactPhone["PhoneAreaCode"].InnerText;
                            }

                            if (NodeContactPhone.SelectSingleNode("PhoneNumber") != null)
                            {
                                phoneNumber += NodeContactPhone["PhoneNumber"].InnerText;
                                Response.Write(" - " + phoneNumber + "<br />");  
                            }
                        }
                    }
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {

        }
    }
}
