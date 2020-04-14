<%@ Page Title="" Language="C#" MasterPageFile="~/Community/Community.Master" AutoEventWireup="true" CodeBehind="ListAll_Updates.aspx.cs" Inherits="TeOraHouWhanganui.Community.Reports.ListAll_Updates" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="table">
        <tr><th>Name</th><th>Contact</th><th>Update Date/Time</th><th>Updated By</th><th style="width:30%">Note</th><th>Followup</th><th>Followup Date/Time</th><th>Followup Done</th></tr>
    <%=html %>
        </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
