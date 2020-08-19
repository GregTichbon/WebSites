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
                $('#results').empty();

                //var formData = $('form').serialize();

                //var formValues = new FormData();
                //formValues.append("mode", "test");
                //formData = $(formValues).serialize();
                //var form = document.getElementById('form1');
                //console.log(form);
                //var formData = new FormData(form);

                var formData = new FormData();
                formData.append('mode', 'eventsearch');
                formData.append('fld_program', $('#fld_program').val());
                formData.append('fld_date', $('#fld_date').val());

                $.ajax({
                    type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                    url: '../_dependencies/data.aspx', // the url where we want to POST
                    processData: false,  // tell jQuery not to process the data
                    contentType: false,  // tell jQuery not to set contentType
                    data: formData,
                    //dataType: 'html', // what type of data do we expect back from the server
                    success: function (result) {
                        $('#div_results').html(result);
                    },
                    error: function (xhr, status) {
                        alert('error');

                    }
                });
            })

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input name="mode" value="test" type="hidden" />
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
            <label class="control-label col-sm-4" for="fld_program">Program</label>
            <div class="col-sm-8">
                <select id="fld_program" name="fld_program" class="form-control">
                    <option value="">--- Please select ---</option>
                    <% 
                        Dictionary<string, string> programoptions = new Dictionary<string, string>();
                        programoptions["type"] = "select";
                        programoptions["valuefield"] = "value";
                        Response.Write(Generic.Functions.buildselection(programs, nooptions, programoptions));
                    %>
                </select>
            </div>
        </div>
    </div>
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_date">Date</label>
            <div class="col-sm-8">
                <div class="input-group standarddate">
                    <input id="fld_date" name="fld_date" placeholder="eg: 23 Jun 1985" type="text" class="form-control" required="required" />
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
            <input type="button" id="btn_search" class="btn btn-info" value="Search" />
        </div>
    </div>

    <div id="div_results"></div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
