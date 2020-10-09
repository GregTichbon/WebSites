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

namespace TeOraHouWhanganui.Private.Reports
{
    public partial class AttendanceStatistics : System.Web.UI.Page
    {
        public string username = "";
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (fld_datefrom.Text != "" && fld_dateto.Text != "" && fld_programs.GetSelectedIndices().Length > 0)
            {
                string programs = "";
                string delim = "";

                foreach (ListItem item in fld_programs.Items)
                {
                    if (item.Selected)
                    {
                        programs = programs + delim + item.Value; ;
                        delim = ",";
                    }

                }

                List<ReportDefinition> ReportDefinitionList = new List<ReportDefinition>();
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "ID",
                    Display = false,
                    Type = ""
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Name",
                    Display = true,
                    Type = "String",
                    Link = "<a target=\"_blank\" href=\"../PersonMaintenance.aspx?id=||LinkID||\">||Value||</a>",
                    LinkID = "ID"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Age at today",
                    Display = true,
                    Type = "String"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Gender",
                    Display = true,
                    Type = "String"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "First Event",
                    Display = true,
                    Type = "Date"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Last Event",
                    Display = true,
                    Type = "Date"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Event Attendance",
                    Display = true,
                    Type = "Number"
                });
                
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "First Encounter",
                    Display = true,
                    Type = "Date"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Last Encounter",
                    Display = true,
                    Type = "Date"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Encounters",
                    Display = true,
                    Type = "Int"
                });

                string sql = String.Format(@"select En.ID, dbo.get_name(En.ID,'') as [Name], dbo.AgeAtDate(En.DateofBirth,null) as [Age at today], Gender, E.FirstEvent, E.LastEvent
                    ,(
	                    select count(*) from Enrolement E 
	                    inner join Attendance A on A.EnrolementID = E.id and isnull(A.Attendance,'No') <> 'No' 
	                    inner join Event Ev on Ev.id = A.EventID and dbo.betweendates(Ev.EventDate,Ev.EventEndDate,'{0}','{1}','') = 1
	                    where E.EntityID = En.ID
                    )  [Event attendance]

                    , (select cast(min(Enc.StartDateTime) as Date) from Encounter Enc where EntityID = En.ID) [First Encounter]
                    , (select cast(max(Enc.StartDateTime) as Date) from Encounter Enc where EntityID = En.ID) [Last Encounter]
                    ,(
	                    select count(*) from Encounter Enc where Enc.EntityID = En.ID and dbo.betweendates(Enc.StartDateTime,Enc.EndDateTime,'{0}','{1}','') = 1
                    ) [Encounters]
                    from Entity En
                    inner join Enrolement E on E.EntityID = En.ID and E.ProgramID in ({2}) and dbo.betweendates(E.firstEvent,E.LastEvent,'{0}','{1}','') = 1
                    order by dbo.get_name(En.ID,'')
                    ", fld_datefrom.Text, fld_dateto.Text,programs);

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.CommandType = CommandType.Text;


                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        html = "<table class=\"table\"><thead>";
                        html += "<tr>";
                        foreach (ReportDefinition column in ReportDefinitionList)
                        {
                            if (column.Display)
                            {
                                html += "<th>" + column.Name + "</th>";
                            }
                        }
                        html += "</tr>";
                        html += "</thead><tbody>";
                        if (dr.HasRows)
                        {
                            

                            while (dr.Read())
                            {
                                int col = 0;
                                string value = "";
                                html += "<tr>";

                                foreach (ReportDefinition column in ReportDefinitionList)
                                {
                                    if (column.Display)
                                    {
                                        value = dr[col].ToString();
                                        if(column.Type == "Date")
                                        {
                                            value = Functions.formatdate(value, "dd MMM yy");
                                        }
                                        /*Link = "<a target=\"_blank\" href=\"../PersonMaintenance.aspx?id=||LinkID||\">||Value||</a>",
                                           LinkID = "ID"*/
                                        if(column.Link != null)
                                        {
                                            value = column.Link.Replace("||Value||", value);
                                            value = value.Replace("||LinkID||", dr[column.LinkID].ToString());
                                        }
                                        html += "<td>" + value + "</td>";
                                    }
                                    col++;
                                }
                                html += "</tr>";
                            }

                        }
                        html += "</tbody></table>";

                    }
                }
            }

        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {

        }

        public class ReportDefinition
        {
            public string Name;
            public Boolean Display;
            public string Type;
            public string Link;
            public string LinkID;

        }
    }
}