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
                    Name = "Type",
                    Action = "Class",
                    Type = ""
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "EntityID",
                    Action = "Hide",
                    Type = ""
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Name",
                    Action = "Display",
                    Type = "String",
                    Link = "<a target=\"_blank\" href=\"../PersonMaintenance.aspx?id=||LinkID||\">||Value||</a>",
                    LinkID = "EntityID"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "SortName",
                    Action = "Hide",
                    Type = ""
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Age",
                    Action = "Display",
                    Type = "String"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Gender",
                    Action = "Display",
                    Type = "String"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "First Encounter",
                    Action = "Display",
                    Type = "Date"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Last Encounter",
                    Action = "Display",
                    Type = "Date"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Encounters",
                    Action = "Display",
                    Type = "Int"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Program",
                    Action = "Display",
                    Type = "String"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "First Event",
                    Action = "Display",
                    Type = "Date"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Last Event",
                    Action = "Display",
                    Type = "Date"
                });
                ReportDefinitionList.Add(new ReportDefinition
                {
                    Name = "Event Attendance",
                    Action = "Display",
                    Type = "Number"
                });
                
                
                /*
                string sql = String.Format(@"select En.ID, dbo.get_name(En.ID,'') as [Name], cast(dbo.AgeAtDate(En.DateofBirth,@startdate) as varchar(10)) + ' - ' + cast(dbo.AgeAtDate(En.DateofBirth,@enddate) as varchar(10)) as [Age], Gender, P.ProgramName, E.FirstEvent, E.LastEvent--, Ev.EventDate
                    , (select cast(min(Enc.StartDateTime) as Date) from Encounter Enc where EntityID = En.ID) [First Encounter]
                    , (select cast(max(Enc.StartDateTime) as Date) from Encounter Enc where EntityID = En.ID) [Last Encounter]
                    ,(
                    select count(*) from Encounter Enc where Enc.EntityID = En.ID and dbo.betweendates(Enc.StartDateTime,Enc.EndDateTime,@startdate,@enddate,'') = 1
                    ) [Encounters]
                    ,(
                    select count(*) from Enrolement E 
                    inner join Program P on P.id = E.ProgramID
                    inner join Attendance A on A.EnrolementID = E.id and isnull(A.Attendance,'No') <> 'No' 
                    inner join Event Ev on Ev.ID = A.EventID and dbo.betweendates(Ev.EventDate,Ev.EventEndDate,@startdate,@enddate,'') = 1
                    where E.EntityID = En.ID and E.ProgramID in (select * from dbo.fnSplit(@programs,',')) 
                    ) [EventAttendance]
                    into #tmp
                    from Entity En
                    inner join Enrolement E on E.EntityID = En.ID and E.ProgramID in (select * from dbo.fnSplit(@programs,',')) 
                    inner join Program P on P.id = E.ProgramID     ", fld_datefrom.Text, fld_dateto.Text,programs);
                */
                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("reports", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@reportname", SqlDbType.VarChar).Value = "Attendance Statistics";
                        cmd.Parameters.Add("@startdate", SqlDbType.VarChar).Value = fld_datefrom.Text;
                        cmd.Parameters.Add("@enddate", SqlDbType.VarChar).Value = fld_dateto.Text;

                        cmd.Parameters.Add("@programs", SqlDbType.VarChar).Value = programs;
             
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        html = "<table class=\"table\"><thead>";
                        html += "<tr>";
                        foreach (ReportDefinition column in ReportDefinitionList)
                        {
                            if (column.Action == "Display")
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
                                html += "<tr class=\"" + dr["type"] + "\">";

                                foreach (ReportDefinition column in ReportDefinitionList)
                                {

                                    if (column.Action == "Display")
                                    {
                                        value = dr[col].ToString();
                                        if (column.Type == "Date")
                                        {
                                            value = Functions.formatdate(value, "dd MMM yy");
                                        }
                                        /*Link = "<a target=\"_blank\" href=\"../PersonMaintenance.aspx?id=||LinkID||\">||Value||</a>",
                                           LinkID = "ID"*/
                                        if (column.Link != null)
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
            public string Action;
            public string Type;
            public string Link;
            public string LinkID;
        }
    }
}