<%@ Page Title="" Language="C#" MasterPageFile="~/Private/Main.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="TeOraHouWhanganui.Private.Reports.Report" %>
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

            $('#reportmenu').click(function () {
                window.location.href = "<%=ResolveUrl("~/private/reports/default.aspx")%>";
            });


        }); //document.ready
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
     <p>A report</p>


 </div>
    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="reportmenu" class="btn btn-info" value="REPORTS MENU" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <h1><%= reportname %>
    </h1>
    <table class="table" style="width: 100%">
        <%=html %>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
