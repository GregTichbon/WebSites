<%@ Page Title="TOHW Event Search" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EventSearch.aspx.cs" Inherits="TeOraHouWhanganui.Private.EventSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>

    <script>
        $(document).ready(function () {
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

            //$("#form1").validate();

            $('.standarddate').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true
                //,maxDate: moment().add(-1, 'year')
            });

            $('#btn_search').click(function (e) {
                e.preventDefault();
                $('#div_results').html('get events that match via Ajax and add a link to create new event');
            })

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p>Coming soon</p>


    </div>
    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <h1>Event Search
    </h1>
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="name">Program</label>
            <div class="col-sm-8">
                <select id="fld_gender" name="fld_gender" class="form-control">
                    <option value="">--- Please select ---</option>
                    <% 
                        Dictionary<string, string> programoptions = new Dictionary<string, string>();
                        programoptions["type"] = "select";
                        programoptions["valuefield"] = "label";
                        Response.Write(Generic.Functions.buildselection(programs, nooptions, programoptions));
                    %>
                </select>
            </div>
        </div>
    </div>
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="date">Date</label>
            <div class="col-sm-8">
                <div class="input-group standarddate">
                    <input id="date" name="date" placeholder="eg: 23 Jun 1985" type="text" class="form-control" required="required" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4">
        </div>
        <div class="col-sm-8">
            <input type="submit" id="btn_search" class="submit btn btn-info" value="Search" />
        </div>
    </div>
    <div id="div_results"></div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
