<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CommonGoodCoffee.Reports.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript">
        $(document).ready(function () {

                      $('#menu').click(function () {
                window.location.href = "/default.aspx";
            });

          
         });
      </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="toprighticon">
         <input type="button" id="menu" class="btn btn-info" value="MENU" />
     </div>
    <h1>Reports Menu</h1>
    <p><a href="report.aspx?id=1" class="btn btn-info">Customers</a></p>
    <p><a href="report.aspx?id=4" class="btn btn-info">Orders</a></p>
    <p><a href="report.aspx?id=6" class="btn btn-info">Orders not allocated</a></p>
    <p><a href="report.aspx?id=2" class="btn btn-info">Orders not dispatched</a></p>
    <p><a href="report.aspx?id=5" class="btn btn-info">Orders not invoiced</a></p>
    <p><a href="stockconsignments.aspx" class="btn btn-info">Stock Consignments</a></p>
    <p><a href="subscriptions.aspx" class="btn btn-info">Subscriptions</a></p>
    <p><a href="report.aspx?id=3" class="btn btn-info">Table Statistics</a></p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
