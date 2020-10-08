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
    public partial class Attendance : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string fromdate = "1-sep-20";
            string todate = "30-sep-20";
            string sql = String.Format(@"select 
            dbo.daterange(Ev.EventDate,Ev.EventEndDate,'Yes','') as [Date], P.ProgramName, Ev.Description, dbo.get_name(E.EntityID,'') as [Name], A.Capacity, A.Attendance
             from Attendance A
            left outer join Event Ev on Ev.ID = A.EventID
            left outer join Enrolement E on E.ID = A.EnrolementID
            --left outer join Entity En on En.id = E.EntityID
            left outer join Program P on P.ID = E.ProgramID
            where dbo.betweendates(Ev.EventDate,Ev.EventEndDate,'{0}','{1}','') = 1 and A.attendance <> 'No'   
            order by P.ProgramName, Ev.EventDate,Ev.EventEndDate, A.Capacity, dbo.get_name(E.EntityID,'')", fromdate, todate);

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.Text;
                   

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    html = "<table class=\"table\"><thead><tr><th>Date</th><th>Program</th><th>Event</th><th>Name</th><th>Capacity</th><th>Attendance</th></tr></thead><tbody>";
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string Date = dr["Date"].ToString();
                            string ProgramName = dr["ProgramName"].ToString();
                            string description = dr["description"].ToString();
                            string name = dr["name"].ToString();
                            string capacity = dr["capacity"].ToString();
                            string attendance = dr["attendance"].ToString();

                            html += "<tr><td>" + Date + "</td><td>" + ProgramName + "</td><td>" + description + "</td><td>" + name + "</td><td>" + capacity + "</td><td>" + attendance + "</td></tr>";

                        }
                    }
                    html += "</tbody></table>";
                }

                
            }

        }
    }
}