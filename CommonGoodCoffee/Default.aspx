<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CommonGoodCoffee.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Main Menu</h1>
    <p><a href="CustomerSearch.aspx" class="btn btn-info">Customer Maintenance</a></p>
    <p><a href="stockitemList.aspx" class="btn btn-info">Stock Items</a></p>
    <p><a href="stockreceived.aspx" class="btn btn-info">Stock Received</a></p>

    <p><a href="reporting" class="btn btn-info">Reporting Menu</a></p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
