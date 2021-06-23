<%@ Page Title="TOHW Main Menu" Language="C#" MasterPageFile="~/Private/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TeOraHouWhanganui.Private.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/_Dependencies/Support.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#assistance').click(function () {
                $("#dialog_assistance").dialog({
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true
                });
            })



            //Generic.Functions.googleanalyticstracking()%>

        }); //document.ready



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- #include file = "/_dependencies/support.html" -->
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none"></div>
    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
    </div>

    <h1>Main Menu
    </h1>

    <p><a href="PersonSearch.aspx" class="btn btn-info">Person Maintenance</a></p>

    <%  if (TeOraHouWhanganui._Dependencies.functions.AccessStringTest(username, "1"))
        {
            Response.Write("<p><a href=\"administration/access.aspx\" class=\"btn btn-info\">Security Access</a></p>");
        }
    %>
    <% 
        Response.Write("<p><a href=\"EventSearch.aspx\" class=\"btn btn-info\">Event Maintenance</a></p>");
        Response.Write("<p><a href=\"ProgramSearch.aspx\" class=\"btn btn-info\">Program Maintenance</a></p>");
    %>
    <%    if (TeOraHouWhanganui._Dependencies.functions.AccessStringTest(username, "1001"))
        {
            Response.Write("<p><a href=\"Vehicles.aspx\" class=\"btn btn-info\">Vehicle Maintenance</a></p>");
        }
    %>
    <% 
        Response.Write("<p><a href=\"VehicleBookings.aspx\" class=\"btn btn-info\">Vehicle Bookings</a></p>");
        Response.Write("<p><a href=\"http://whanganui.teorahou.org.nz/pickups\" class=\"btn btn-info\">Senior Club Pickups</a></p>");
        Response.Write("<p><a href=\"http://whanganui.teorahou.org.nz/pickups/admin.aspx\" class=\"btn btn-info\">Senior Club Pickups - Set Date</a></p>");
    %>
    <p><a href="UploadMedia.aspx" class="btn btn-info">Upload Media</a></p>
    <p><a href="reports" class="btn btn-info">Reports</a></p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <form id="form2">
    </form>
</asp:Content>
