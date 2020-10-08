<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Viewer.aspx.cs" Inherits="TeOraHouWhanganui.Private.Reports.Crystal.Viewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />

        <CR:CrystalReportViewer ID="crv_report" runat="server" ToolPanelWidth="200px" Width="350px" GroupTreeImagesFolderUrl="" Height="50px" ReportSourceID="CrystalReportSource1" ToolbarImagesFolderUrl="" />

    </form>
</body>
</html>
