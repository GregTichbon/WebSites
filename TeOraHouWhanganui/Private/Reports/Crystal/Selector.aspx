<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Selector.aspx.cs" Inherits="TeOraHouWhanganui.Private.Reports.Crystal.Selector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <script type="text/javascript">

        var person_ctr;
        //Boolean create = false;

        $(document).ready(function () {

            $('.date').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false//,
                //useCurrent: true,
                //stepping: 15

                //,maxDate: moment().add(-1, 'year')
            });

            $('#menu').click(function () {
                window.location.href = "<%=ResolveUrl("../default.aspx")%>";
            });
            $('#reportmenu').click(function () {
                window.location.href = "<%=ResolveUrl("~/private/reports/default.aspx")%>";
            });
            $('#assistance').click(function () {
                $("#dialog_assistance").dialog({
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true
                });
            })

            $("#form1").validate();

        }); //document.ready

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%=username %>
    <!--
    <input type="button" value="test" id="btn_test" />
    <input type="text" id="hidden_dirty" />
    -->
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p></p>
    </div>

    <div class="toprighticon">
        <button id="assistance" type="button" class="btn btn-info">Assistance</button>
        <button id="reportmenu" class="btn btn-info">REPORTS MENU</button>
        <button id="menu" type="button" class="btn btn-info">MENU</button>
    </div>

    <h1>Report Selector - <%=reporttitle %>
    </h1><br />
    <div class="form-horizontal">
        <%=html %>
        <div class="form-group">
            <div class="col-sm-4"></div>
            <div class="col-sm-8">
                <asp:Button ID="btn_submit" runat="server" Text="Run" OnClick="btn_submit_Click" class="submit btn btn-info" />
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
