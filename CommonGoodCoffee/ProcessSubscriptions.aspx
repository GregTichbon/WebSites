<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ProcessSubscriptions.aspx.cs" Inherits="CommonGoodCoffee.ProcessSubscriptions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            var UptoDate = "<%= ViewState["id"] %>";
            if (UptoDate != "") {

            }
            $('.standarddate').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true
                //,maxDate: moment().add(-1, 'year')
            });
            $('#form1').validate();

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
    <div class="bottomrighticon">
        <asp:Button ID="btn_cancel" runat="server" OnClick="btn_cancel_Click" class="btn btn-info" Text="Cancel" UseSubmitBehavior="False" />
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />

    </div>
    <h1>Subscription Processing
    </h1>
    <div class="form-horizontal row">
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_uptodate">Process up to:</label>
            <div class="col-sm-4">
                <div class="input-group standarddate">
                    <asp:TextBox ID="fld_uptodate" CssClass="form-control standarddate" runat="server" ClientIDMode="Static" required="required"></asp:TextBox>
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
    <%=html %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
