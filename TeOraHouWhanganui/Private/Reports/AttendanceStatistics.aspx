<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AttendanceStatistics.aspx.cs" Inherits="TeOraHouWhanganui.Private.Reports.AttendanceStatistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <style>
        tr.Header td {
            font-weight:bold;
}
    </style>

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
                window.location.href = "<%=ResolveUrl("default.aspx")%>";
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
        <p>Still working on this report ... showing duplicate records where enrolled in more than one program, also unsure of first and last event.</p>
    </div>

    <div class="toprighticon">
        <button id="assistance" type="button" class="btn btn-info">Assistance</button>
        <button id="menu" type="button" class="btn btn-info">MENU</button>
    </div>

    <h1>Attendance Statistics
    </h1>
    <div class="form-horizontal">
        <div class="row">
            <div class="col-sm-4 form-group">
                <label class="control-label">Date From</label>
                <div class="input-group date">
                    <asp:TextBox ID="fld_datefrom" runat="server" class="form-control" required="required"></asp:TextBox>
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>


            <div class="col-sm-4 form-group">
                <label class="control-label">Date To</label>
                <div class="input-group date">
                    <asp:TextBox ID="fld_dateto" runat="server" class="form-control" required="required"></asp:TextBox>
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>

            <div class="col-sm-4 form-group">
                <label class="control-label">Program</label>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:tohwConnectionString %>" SelectCommand="SELECT [ID], [ProgramName] FROM [Program] ORDER BY [ProgramName]"></asp:SqlDataSource>
                <asp:ListBox CssClass="form-control" runat="server" ID="fld_programs" SelectionMode="Multiple" ConnectionString="<%$ ConnectionStrings:tohwConnectionString %>" SelectCommand="SELECT [ID], [ProgramName] FROM [Program] ORDER BY [ProgramName]" DataSourceID="SqlDataSource1" DataTextField="ProgramName" DataValueField="ID"></asp:ListBox>
            </div>
            <div class="col-sm-4 form-group">
                <label class="control-label"></label>
                <div class="input-group">
                    <asp:Button ID="btn_submit" runat="server" Text="Run" OnClick="btn_submit_Click" class="submit btn btn-info" />
                </div>
            </div>

        </div>
    </div>



    <%=html %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
