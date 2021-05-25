<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="stockconsignments.aspx.cs" Inherits="CommonGoodCoffee.Reporting.stockconsignments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
        $(document).ready(function () {

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
    <h2>Stock Consignments</h2>
    <%=html%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
