<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="CommonGoodCoffee.Reports.Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

            $('#menu').click(function () {
                window.location.href = "/default.aspx";
            });

            $('#reportmenu').click(function () {
                window.location.href = "/reporting";
             });
         });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="toprighticon">
        <input type="button" id="reportmenu" class="btn btn-info" value="REPORT MENU" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <%=html%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
