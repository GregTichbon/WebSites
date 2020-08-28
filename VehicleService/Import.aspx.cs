using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace VehicleService
{
    public partial class Import : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("C:\\Users\\gtichbon\\Source\\Repos\\WebSites\\VehicleService\\Imports\\CampbellContacts.xml");

            XmlNodeList nodeListContact = xmlDoc.SelectNodes("//Contact");
            foreach (XmlNode NodeContact in nodeListContact)
            {
                //Response.Write(NodeContact.Name + "<br />");
                if (NodeContact["IsCustomer"].InnerText == "true")
                {
                    string XeroXML = NodeContact.OuterXml;
                    string XeroID = NodeContact["ContactID"].InnerText;
                    string Name = NodeContact["Name"].InnerText;
                    string EmailAddress = "";
                    if(NodeContact["EmailAddress"] != null) {
                        EmailAddress = NodeContact["EmailAddress"].InnerText;
                    }

                    string FirstName = "";
                    if (NodeContact["FirstName"] != null)
                    {
                        FirstName = NodeContact["FirstName"].InnerText;
                    }
                    string LastName = "";
                    if (NodeContact["LastName"] != null)
                    {
                        LastName = NodeContact["LastName"].InnerText;
                    }



                    string Phone = "";
                    String Mobile = "";
                    string Address = "";
                    Response.Write(XeroID + "<br />");  //guid
                    Response.Write(Name + "<br />");
                    Response.Write(NodeContact["UpdatedDateUTC"].InnerText + "<br />");

                    

                    int DefaultCount = 0;
                    int MobileCount = 0;
                    int DDICount = 0;
                    XmlNodeList nodeListContactPhones = NodeContact.SelectNodes("Phones");
                    foreach (XmlNode NodeContactPhones in nodeListContactPhones)
                    {
                        XmlNodeList nodeListContactPhone = NodeContactPhones.SelectNodes("Phone");
                        foreach (XmlNode NodeContactPhone in nodeListContactPhone)
                        {
                            string phonetype = NodeContactPhone.SelectSingleNode("PhoneType").InnerText;

                            string phoneNumber = "";
                            if (NodeContactPhone.SelectSingleNode("PhoneAreaCode") != null)
                            {
                                phoneNumber = NodeContactPhone["PhoneAreaCode"].InnerText;
                            }

                            if (NodeContactPhone.SelectSingleNode("PhoneNumber") != null)
                            {
                                phoneNumber += NodeContactPhone["PhoneNumber"].InnerText;
                                Response.Write(" - " + phoneNumber + "<br />");
                                switch (phonetype)
                                {
                                    case "DEFAULT":
                                        DefaultCount++;
                                        Phone = phoneNumber;
                                        break;
                                    case "DDI":
                                        DDICount++;
                                        break;
                                    case "MOBILE":
                                        MobileCount++;
                                        Mobile = phoneNumber;
                                        break;
                                }
                                
                            }
                        }
                    }
                    if(DefaultCount > 1 || MobileCount > 1 || DDICount > 0)
                    {
                        string x = "x";
                    }

                    XmlNodeList nodeListContactAddresses = NodeContact.SelectNodes("Addresses");
                    foreach (XmlNode NodeContactAddresses in nodeListContactAddresses)
                    {
                        XmlNodeList nodeListContactAddress = NodeContactAddresses.SelectNodes("Address");
                        foreach (XmlNode NodeContactAddress in nodeListContactAddress)
                        {
                            string Addresstype = NodeContactAddress.SelectSingleNode("AddressType").InnerText;

                            if (Addresstype == "STREET")
                            {
                                string delim = "";
                                if (NodeContactAddress.SelectSingleNode("AddressLine1") != null)
                                {
                                    Address = NodeContactAddress["AddressLine1"].InnerText;
                                    delim = "\r\n";
                                }
                                if (NodeContactAddress.SelectSingleNode("AddressLine2") != null)
                                {
                                    Address += delim + NodeContactAddress["AddressLine2"].InnerText;
                                    delim = "\r\n";
                                }
                                if (NodeContactAddress.SelectSingleNode("City") != null)
                                {
                                    Address += delim + NodeContactAddress["City"].InnerText.Replace("Wanganui", "Whanganui");
                                }
                                if (NodeContactAddress.SelectSingleNode("PostalCode") != null)
                                {
                                    Address += " " + NodeContactAddress["PostalCode"].InnerText;
                                }
                            }
                        }
                    }
                    string id;
                    if (1 == 1)
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        using (SqlCommand cmd = new SqlCommand("Update_Customer", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@Customer_CTR", SqlDbType.VarChar).Value = "new";
                            cmd.Parameters.Add("@XeroID", SqlDbType.VarChar).Value = XeroID;
                            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
                            cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
                            cmd.Parameters.Add("@Surname", SqlDbType.VarChar).Value = LastName;
                            cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = Address;
                            cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar).Value = EmailAddress;
                            cmd.Parameters.Add("@MobilePhone", SqlDbType.VarChar).Value = Mobile;
                            cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar).Value = Phone;
                            cmd.Parameters.Add("@XeroXML", SqlDbType.Xml).Value = XeroXML;
                            con.Open();
                            id = cmd.ExecuteScalar().ToString();

                            con.Close();
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
