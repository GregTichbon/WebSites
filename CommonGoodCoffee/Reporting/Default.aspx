<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CommonGoodCoffee.Reports.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Reports Menu</h1>
    <p><a href="report.aspx?id=1" class="btn btn-info">Customers</a></p>

    <p><a href="report.aspx?id=2" class="btn btn-info">Undelivered or Uninvoiced Orders</a></p>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
