<%@ Page Title="" Language="C#" MasterPageFile="~/Auction.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Auction.Administration.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p><a href="itemlist.aspx">Items</a></p>
    <!--<p><a href="item.aspx?all=true">All Items</a></p>-->
    <p><a href="resequence.aspx">Resequence Items</a></p>
    <% if (parameters["DoDonors"] == "Yes")
        { %>
    <p><a href="donorlist.aspx">Donors</a></p>
    <%} %>
    <p><a href="categorylist.aspx">Categories</a></p>
    <p><a href="userlist.aspx">Users</a></p>

    <p></p>

    <p><a href="reportmenu.aspx">Reports</a></p>
    <p><a href="setup.aspx">Setup</a></p>
    <p><a href="/default.aspx" target="_blank">Auction</a></p>

    <p><a href="logoff.aspx">Log Off</a></p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
