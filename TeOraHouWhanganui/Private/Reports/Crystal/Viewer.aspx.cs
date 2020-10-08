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
        protected void Page_Load(object sender, EventArgs e)
        {
            string strConnString = "Data Source=toh-app;Initial Catalog=TeOraHou;Integrated Security=False;user id=OnlineServices;password=Whanganui497";

            string r = Request.QueryString["id"];
           
            string report;
            DataSet ds = new DataSet();
            rpt = new ReportDocument();


            switch (r)
            {
                case "1":
                    SqlConnection con1 = new SqlConnection(strConnString);
                    SqlDataAdapter adp1 = new SqlDataAdapter("Report_Encounters", con1);

                    adp1.Fill(ds);
                    report = Server.MapPath("~/private/reports/crystal/EncountersByPersonDate.rpt");
                    rpt.Load(report);
                    rpt.SetDataSource(ds.Tables["Table"]);

                    crv_report.ReportSource = rpt;
                    int PageCount = rpt.FormatEngine.GetLastPageNumber(new ReportPageRequestContext());

                    Literal1.Text = PageCount.ToString();

                    if (1 == 1)
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
                    } else
                    {
                        crv_report.RefreshReport();
                    }

                    break;
                case "2":
                    //SqlConnection con2 = new SqlConnection(strConnString);
                    //SqlDataAdapter adp2 = new SqlDataAdapter("select * from person", con2);
                    //adp2.Fill(ds);
                    report = Server.MapPath("~/private/reports/crystal/testNoData.rpt");
                    rpt.Load(report);
                    //rpt.SetDataSource(ds.Tables["Table"]);

                    crv_report.ReportSource = rpt;
                    crv_report.RefreshReport();

                    break;
                case "3":
                    report = Server.MapPath("~/people/reports/crystal/testNoData.rpt");
                    rpt.Load(report);

                    crv_report.ReportSource = rpt;

                    break;
                
               default:
                    break;
            }


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
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