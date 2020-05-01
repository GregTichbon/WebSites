<%@ Page Title="" Language="C#" MasterPageFile="~/Private/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TeOraHouWhanganui.Private.Reports.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">

        $(document).ready(function () {
            //Generic.Functions.googleanalyticstracking()%>
            $('#assistance').click(function () {
                $("#dialog_assistance").dialog({
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true
                });
            })

            $('#menu').click(function () {
                window.location.href = "<%=ResolveUrl("~/private/default.aspx")%>";
            });

        }); //document.ready
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p>Reports</p>
    </div>
    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <h1>Reports
    </h1>
    <p><a href="Report.aspx?id=AccessLevels">Access Levels</a></p>
    <p><a href="Report.aspx?id=WorkerAccessLevelsWorker">Worker Access Levels - by Worker</a></p>
    <p><a href="Report.aspx?id=WorkerAccessLevelsYouth">Worker Access Levels - by Youth</a></p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
