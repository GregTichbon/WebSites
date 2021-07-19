<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Followup.aspx.cs" Inherits="VehicleService.Followup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>

    <style>
        .updatewof {
            width: 20px;
        }
    </style>


    <script>
        $(document).ready(function () {

            myheight = $(window).height() * .95;


            $('#assistance').click(function () {
                $("#dialog_assistance").dialog({
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true
                });
            })
            $('#menu').click(function () {
                window.location.href = "<%=ResolveUrl("~/default.aspx")%>";
            });
            $('.customer').click(function () {
                customer = $(this).closest('tr').data('customer');
                window.open("/main.aspx?customer=" + customer, 'main');
                //window.location.href = "/main.aspx?customer=" + customer;
            })
            $('.customer_vehicle').click(function () {
                customer = $(this).closest('tr').data('customer');
                customer_vehicle = $(this).closest('tr').data('customer_vehicle');
                window.open("/main.aspx?customer=" + customer + '&customer_vehicle=' + customer_vehicle, 'main');
                //window.location.href = "/main.aspx?customer=" + customer + '&customer_vehicle=' + customer_vehicle;
            })

            $('#form2').validate({
                rules: {
                    fld_activity: {
                        required: $("#fld_activitydate").val().length <= 0
                    },
                    fld_activitydate: {
                        required: $("#fld_activity").val().length <= 0
                    }
                },
                messages: {
                    fld_activity: {
                        required: "An activity date is required"
                    },
                    fld_activitydate: {
                        required: "A description of the activity is required"
                    },
                    fld_followupdetail: {
                        required: "An followup date is required"
                    },
                    fld_followupdate: {
                        required: "The detail of the followup is required"
                    }
                }
            });
            
            $('.updatewof').click(function () {
                id = $(this).closest('tr').data('vehicle');
                wofdate = $(this).closest('tr').find('td').eq(2).text();
                wof_cycle = $(this).closest('tr').data('wof_cycle');
                alert(wof_cycle);
                $('#fld_wofdate').val(wofdate);
                $('#fld_wofcycle').val(wof_cycle);

                $("#dialog_updatewof").dialog({
                    //autoOpen: false,
                    resizable: false,
                    height: myheight,
                    width: 800,
                    modal: true,
                    buttons: {
                        "Cancel": function () {
                            $(this).dialog('close');
                        },
                        "Save": function () {
                            if ($('#form2').valid()) {

                                var arForm = $("#form1")
                                    .find("input,textarea,select,hidden")
                                    .not("[id^='__']")
                                    .serializeArray();

                                //arForm.push({ name: 'vehicle_ctr', value: $('#vehicle_ctr').val() });
                                var formData = JSON.stringify({ formVars: arForm });
                                alert(formData);

                                $.ajax({
                                    type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                                    url: '/_dependencies/posts.asmx/update_followup', // the url where we want to POST
                                    contentType: "application/json; charset=utf-8",
                                    data: formData,
                                    dataType: 'json', // what type of data do we expect back from the server
                                    async: false,
                                    success: function (result) {
                                    },
                                    error: function (xhr, status) {
                                        alert('error');
                                    }
                                });
                                $(this).dialog('close');
                            }
                        }
                    }
                    , appendTo: "#form2"

                });
            });
            $('.datetime').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true,
                stepping: 15

                //,maxDate: moment().add(-1, 'year')
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p>To do</p>
    </div>
    <div id="dialog_details" title="Details" style="display: none">
        <p>To do</p>
    </div>
    <div id="dialog_updatewof" title="Update" style="display: none" class="form-horizontal">
        <div class="panel panel-default">
            <div class="panel-heading">Not currently active</div>
            <div class="panel-body">
                <p>
                    This will not currently update the records
                </p>
                <p>Work is underway to restructure followup and activity and then allow this functionality.</p>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_wofdate">WOF Date</label>
            <div class="col-sm-8">
                <div class="input-group datetime">
                    <input id="fld_wofdate" name="fld_wofdate" placeholder="eg: 23 Jun 1985 21:00" type="text" class="form-control" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_wofcycle">WOF Cycle</label>
            <div class="col-sm-8">
                <select id="fld_wofcycle" name="fld_wofcycle" class="form-control">
                    <option value="">--- Please select ---</option>
                    <option value="6">6 monthly</option>
                    <option value="12">Annualy</option>
                    <option value="0">N/A</option>
                </select>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_nextwofdate">Next WOF Date</label>
            <div class="col-sm-8">
                <div class="input-group datetime">
                    <input id="fld_nextwofdate" name="fld_nextwofdate" type="text" class="form-control" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_activitydate">Activity Date</label>
            <div class="col-sm-8">
                <div class="input-group datetime">
                    <input id="fld_activitydate" name="fld_activitydate" type="text" class="form-control" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_activity">Activity</label>
            <div class="col-sm-8">
                <textarea id="fld_activity" name="fld_activity" class="form-control"></textarea>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_followupdate">Followup Date</label>
            <div class="col-sm-8">
                <div class="input-group datetime">
                    <input id="fld_followupdate" name="fld_followupdate" type="text" class="form-control" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_followupdetail">Followup Detail</label>
            <div class="col-sm-8">
                <textarea id="fld_followupdetail" name="fld_followupdetail" class="form-control"></textarea>
            </div>
        </div>
    </div>

    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <h1>Customer Followup
    </h1>
    <%= html %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <form id="form2">
    </form>
</asp:Content>
