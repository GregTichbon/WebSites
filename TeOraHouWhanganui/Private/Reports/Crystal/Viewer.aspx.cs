using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace TeOraHouWhanganui.Private.Reports.Crystal
{
    public partial class Viewer : System.Web.UI.Page
    {
        ReportDocument rpt;
        public string html = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string r = Request.QueryString["id"] + "";

                switch (r)
                {
                    case "1":
                        doreport1();
                        break;

                    case "2":
                        doreport2();
                        break;
                    case "3":
                        doreport3();
                        break;
                    /*
                case "3":
                    report = Server.MapPath("~/people/reports/crystal/testNoData.rpt");
                    rpt.Load(report);

                    crv_report.ReportSource = rpt;

                    break;http://localhost:58611/Private/Reports/Crystal/Viewer.aspx.cs
                    */
                    case "4":
                        doreport4();



                        break;

                    default:
                        break;
                }
            }
            else
            {
                ReportDocument doc = (ReportDocument)Session["Report"];
                crv_report.ReportSource = doc;
            }
        }




        protected void Page_Load(object sender, EventArgs e)
        {
            if (1 == 2)
            {
                //if (!IsPostBack)
                if (Session["Report"] == null)
                {

                    string r = Request.QueryString["id"] + "X";

                    switch (r)
                    {
                        case "1":
                            doreport1();
                            break;

                        case "2":
                            doreport2();
                            break;
                        /*
                    case "3":
                        report = Server.MapPath("~/people/reports/crystal/testNoData.rpt");
                        rpt.Load(report);

                        crv_report.ReportSource = rpt;

                        break;
                        */
                        case "4":
                            doreport4();



                            break;

                        default:
                            break;
                    }
                }
                //crv_report.ReportSource = Session["Report"];
                //crv_report.RefreshReport();  //do i need this?
            }
        }

        protected void doreport1()
        {
            string strConnString = "Data Source=toh-app;Initial Catalog=TeOraHou;Integrated Security=False;user id=OnlineServices;password=Whanganui497";

            DataSet ds = new DataSet();
            rpt = new ReportDocument();
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                SqlDataAdapter da = new SqlDataAdapter("Report_Encounters", con);

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add("@fromdate", SqlDbType.VarChar).Value = Request.QueryString["fromdate"];
                da.SelectCommand.Parameters.Add("@todate", SqlDbType.VarChar).Value = Request.QueryString["todate"];

                da.Fill(ds);

                //int records = ds.Tables["Table"].Rows.Count;

            }

            string report = Server.MapPath("~/private/reports/crystal/EncountersByPersonDate.rpt");
            rpt.Load(report);
            rpt.SetDataSource(ds.Tables["Table"]);
            //rpt.SetParameterValue("fromdate", Request.QueryString["fromdate"]);
            //rpt.SetParameterValue("todate", Request.QueryString["todate"]);


            //crv_report.ReportSource = rpt;
            //int PageCount = rpt.FormatEngine.GetLastPageNumber(new ReportPageRequestContext());

            //Literal1.Text = PageCount.ToString();

            if (1 == 2)
            {

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = "C:\\temp\\test.pdf";
                CrExportOptions = rpt.ExportOptions;

                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrFormatTypeOptions.UsePageRange = true;

                //CrFormatTypeOptions.FirstPageNumber = 1;
                //CrFormatTypeOptions.LastPageNumber = 1;

                CrExportOptions.FormatOptions = CrFormatTypeOptions;

                rpt.Export();
            }
            else
            {
                crv_report.ReportSource = rpt;
                Session.Add("Report", rpt);

            }
        }

        protected void doreport2()
        {
            string strConnString = "Data Source=toh-app;Initial Catalog=TeOraHou;Integrated Security=False;user id=OnlineServices;password=Whanganui497";

            DataSet ds = new DataSet();
            rpt = new ReportDocument();
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                SqlDataAdapter da = new SqlDataAdapter("Report_Encounters", con);

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add("@fromdate", SqlDbType.VarChar).Value = Request.QueryString["fromdate"];
                da.SelectCommand.Parameters.Add("@todate", SqlDbType.VarChar).Value = Request.QueryString["todate"];

                da.Fill(ds);

                //int records = ds.Tables["Table"].Rows.Count;

            }

            string report = Server.MapPath("~/private/reports/crystal/EncounterWorkerYouth.rpt");
            rpt.Load(report);
            rpt.SetDataSource(ds.Tables["Table"]);
            //rpt.SetParameterValue("fromdate", Request.QueryString["fromdate"]);
            //rpt.SetParameterValue("todate", Request.QueryString["todate"]);

            crv_report.ReportSource = rpt;
            /*
            PdfFormatOptions formatOpt = new PdfFormatOptions();
            ExportOptions ex = new ExportOptions();
            formatOpt.CreateBookmarksFromGroupTree = true;
            */
            Session.Add("Report", rpt);

            //int PageCount = rpt.FormatEngine.GetLastPageNumber(new ReportPageRequestContext());

            //Literal1.Text = PageCount.ToString();

            //crv_report.ReportSource = rpt;
            //crv_report.RefreshReport();

            /*
            reportDocument.Load("TestReport.rpt");
            ExportOptions exportOptions = new ExportOptions();
            exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            exportOptions.ExportDestinationOptions = new DiskFileDestinationOptions()
            {
            DiskFileName = "Output.pdf"
            };
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            exportOptions.ExportFormatOptions = new PdfFormatOptions()
            {
            CreateBookmarksFromGroupTree = true
            };
            reportDocument.Export(exportOptions);
            */


        }

        protected void doreport3()
        {
            string strConnString = "Data Source=toh-app;Initial Catalog=TeOraHou;Integrated Security=False;user id=OnlineServices;password=Whanganui497";

            DataSet ds = new DataSet();
            rpt = new ReportDocument();
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                SqlDataAdapter da = new SqlDataAdapter("Report_Encounters", con);

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add("@fromdate", SqlDbType.VarChar).Value = Request.QueryString["fromdate"];
                da.SelectCommand.Parameters.Add("@todate", SqlDbType.VarChar).Value = Request.QueryString["todate"];

                da.Fill(ds);

                //int records = ds.Tables["Table"].Rows.Count;

            }

            string report = Server.MapPath("~/private/reports/crystal/EncounterYouthWorker.rpt");
            rpt.Load(report);
            rpt.SetDataSource(ds.Tables["Table"]);
            //rpt.SetParameterValue("fromdate", Request.QueryString["fromdate"]);
            //rpt.SetParameterValue("todate", Request.QueryString["todate"]);

            crv_report.ReportSource = rpt;
            /*
            PdfFormatOptions formatOpt = new PdfFormatOptions();
            ExportOptions ex = new ExportOptions();
            formatOpt.CreateBookmarksFromGroupTree = true;
            */
            Session.Add("Report", rpt);

            //int PageCount = rpt.FormatEngine.GetLastPageNumber(new ReportPageRequestContext());

            //Literal1.Text = PageCount.ToString();

            //crv_report.ReportSource = rpt;
            //crv_report.RefreshReport();

            /*
            reportDocument.Load("TestReport.rpt");
            ExportOptions exportOptions = new ExportOptions();
            exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            exportOptions.ExportDestinationOptions = new DiskFileDestinationOptions()
            {
            DiskFileName = "Output.pdf"
            };
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            exportOptions.ExportFormatOptions = new PdfFormatOptions()
            {
            CreateBookmarksFromGroupTree = true
            };
            reportDocument.Export(exportOptions);
            */


        }

        protected void doreport4()
        {

            string username = HttpContext.Current.User.Identity.Name.ToLower();
            string strConnString = "Data Source=toh-app;Initial Catalog=TeOraHou;Integrated Security=False;user id=OnlineServices;password=Whanganui497";

            DataSet ds = new DataSet();
            rpt = new ReportDocument();
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                SqlDataAdapter da = new SqlDataAdapter("Report_Maintenance_Encounters", con);

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                da.SelectCommand.Parameters.Add("@fromdate", SqlDbType.VarChar).Value = Request.QueryString["fromdate"];
                da.SelectCommand.Parameters.Add("@todate", SqlDbType.VarChar).Value = Request.QueryString["todate"];

                da.Fill(ds);

                //int records = ds.Tables["Table"].Rows.Count;

            }
            string report = Server.MapPath("~/private/reports/crystal/EncountersByPersonDate.rpt");
            rpt.Load(report);
            rpt.SetDataSource(ds.Tables["Table"]);

            crv_report.ReportSource = rpt;
            Session.Add("Report", rpt);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            //Not sure what I was doing here, exports pages seperatly?
            int PageCount = rpt.FormatEngine.GetLastPageNumber(new ReportPageRequestContext());

            /*
            //Option 1
            for (int f1 = 1; f1 <= 3; f1++)
            {
                Literal1.Text = f1.ToString();
                ExportOptions exportOpts = new ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = "C:\\inetpub\\ubc.org.nz\\people\\test" + f1.ToString() + ".pdf";

                PdfRtfWordFormatOptions pdfOpts = ExportOptions.CreatePdfRtfWordFormatOptions();
                pdfOpts.UsePageRange = true;

                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                exportOpts.DestinationOptions = CrDiskFileDestinationOptions;

                pdfOpts.FirstPageNumber = f1;
                pdfOpts.LastPageNumber = f1;
                exportOpts.ExportFormatOptions = pdfOpts;

                //rpt.ExportToHttpResponse(exportOpts, Response, true, "Crystal" + f1.ToString());
                //rpt.ExportToHttpResponse(exportOpts, Response, false, "");
                rpt.Export();

            }
            */


            /*
                        //Option 2

                        ExportFormatType formatType = ExportFormatType.NoFormat;
                        formatType = ExportFormatType.PortableDocFormat;
                        rpt.ExportToHttpResponse(formatType, Response, true, "Crystal");
            */


            //Option 3


            //string report = Server.MapPath("~/people/reports/crystal/RegattaStatements.rpt");
            //rpt.Load(report);


            for (int f1 = 1; f1 <= 3; f1++)
            {
                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = "C:\\inetpub\\ubc.org.nz\\people\\test" + f1.ToString() + ".pdf";
                CrExportOptions = rpt.ExportOptions;


                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrFormatTypeOptions.UsePageRange = true;


                CrFormatTypeOptions.FirstPageNumber = f1;
                CrFormatTypeOptions.LastPageNumber = f1;

                CrExportOptions.FormatOptions = CrFormatTypeOptions;

                rpt.Export();
            }

        }
    }
}