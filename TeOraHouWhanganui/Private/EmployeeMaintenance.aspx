<%@ Page Title="Employee Maintenance" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EmployeeMaintenance.aspx.cs" Inherits="TeOraHouWhanganui.Private.EmployeeMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>

    <style>
        .hours {
            width: 80px;
            direction: rtl;
        }
        fld_hourlyrate {
            direction: rtl;
        }
    </style>

    <script type="text/javascript">

        var person_ctr;
        //Boolean create = false;

        $(document).ready(function () {
            //Generic.Functions.googleanalyticstracking()%>
            //$("#form1").submit(function (e) {
            $('#btn_submit').click(function () {
                //e.preventDefault();
                alert('do validate');
                var arForm = $("#form1")
                    .find("input,textarea,select,hidden")
                    .not("[id^='__']")
                    .serializeArray();

                //arForm.push({ name: 'vehicle_ctr', value: $('#vehicle_ctr').val() });
                var formData = JSON.stringify({ formVars: arForm });

                $.ajax({
                    type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                    url: '/_dependencies/posts.asmx/update_employee', // the url where we want to POST
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
                $.post("/_Dependencies/data.aspx", { mode: "get_employee_periods", person_ctr: $('#fld_person_ctr').val() })
                    .done(function (data) {
                        $("#div_periods").html(data);
                    });

                $('#div_details').hide();
                $('#btn_submit').hide();
                $('#div_periods').show();
            });

            $('#btn_canceldetails').click(function () {
                $('#div_details').hide();
                $('#btn_canceldetails').hide();
                $('#btn_submit').hide();
                $('#btn_cancelperiods').show();
                $('#div_periods').show();
            });

            $('#btn_cancelperiods').click(function () {
                $('#div_periods').hide();
                $('#btn_cancelperiods').hide();
                $('#div_periods').hide();
                $('#fld_person').val('');
                $('#search').hide();
                $('#fld_person').prop('disabled', false);
            });

            $("#fld_person").autocomplete({
                source: "/_Dependencies/data.asmx/Person_name_autocomplete?options=",
                minLength: 2 //,
                //, appendTo: "#dialog_assigned"
                , select: function (event, ui) {
                    event.preventDefault();

                    $('#fld_person').val(ui.item.label);
                    $('#fld_person').prop('disabled', true);
                    $('#fld_person_ctr').val(ui.item.value);

                    $.post("/_Dependencies/data.aspx", { mode: "get_employee_periods", person_ctr: ui.item.value })
                        .done(function (data) {
                            $("#div_periods").html(data);
                        });

                    $('#btn_cancelperiods').show();
                    $('#div_periods').show();
                    $('#search').show();
                    //$('#fld_person').text(ui.item.label);
                }
            });

            $(document).on('click', '.employeeperiodedit', function () {

                $("#form1")
                    .find("input,textarea,select,hidden")
                    .not("[id^='__']")
                    .not('#fld_person')
                    .not('#fld_person_ctr')
                    .val('');

                employee_ctr = $(this).attr('employee_ctr');
                $('#fld_employee_ctr').val(employee_ctr);

                if (employee_ctr != 'new') {
                    $.getJSON("/_Dependencies/data.asmx/get_employee?id=" + employee_ctr, function (data) {
                        $('#fld_startdate').val(data.startdate);
                        $('#fld_enddate').val(data.enddate);
                        $('#fld_position').val(data.position);
                        $('#fld_hoursperweek').val(data.hoursperweek);
                        $('#fld_note').val(data.note);
                        $('#fld_hourlyrate').val(data.hourlyrate);
                        $('#fld_paytype').val(data.paytype);

                        $('#fld_hoursMon').val(data.hoursMon);
                        $('#fld_hoursTue').val(data.hoursTue);
                        $('#fld_hoursWed').val(data.hoursWed);
                        $('#fld_hoursThu').val(data.hoursThu);
                        $('#fld_hoursFri').val(data.hoursFri);
                        $('#fld_hoursSat').val(data.hoursSat);
                        $('#fld_hoursSun').val(data.hoursSun);
                        $('#fld_lunchhoursMon').val(data.lunchhoursMon);
                        $('#fld_lunchhoursTue').val(data.lunchhoursTue);
                        $('#fld_lunchhoursWed').val(data.lunchhoursWed);
                        $('#fld_lunchhoursThu').val(data.lunchhoursThu);
                        $('#fld_lunchhoursFri').val(data.lunchhoursFri);
                        $('#fld_lunchhoursSat').val(data.lunchhoursSat);
                        $('#fld_lunchhoursSun').val(data.lunchhoursSun);
                        $('#fld_dinnerhoursMon').val(data.dinnerhoursMon);
                        $('#fld_dinnerhoursTue').val(data.dinnerhoursTue);
                        $('#fld_dinnerhoursWed').val(data.dinnerhoursWed);
                        $('#fld_dinnerhoursThu').val(data.dinnerhoursThu);
                        $('#fld_dinnerhoursFri').val(data.dinnerhoursFri);
                        $('#fld_dinnerhoursSat').val(data.dinnerhoursSat);
                        $('#fld_dinnerhoursSun').val(data.dinnerhoursSun);
                        $('#fld_ordinarystartMon').val(data.ordinarystartMon);
                        $('#fld_ordinarystartTue').val(data.ordinarystartTue);
                        $('#fld_ordinarystartWed').val(data.ordinarystartWed);
                        $('#fld_ordinarystartThu').val(data.ordinarystartThu);
                        $('#fld_ordinarystartFri').val(data.ordinarystartFri);
                        $('#fld_ordinarystartSat').val(data.ordinarystartSat);
                        $('#fld_ordinarystartSun').val(data.ordinarystartSun);
                        $('#fld_ordinaryendMon').val(data.ordinaryendMon);
                        $('#fld_ordinaryendTue').val(data.ordinaryendTue);
                        $('#fld_ordinaryendWed').val(data.ordinaryendWed);
                        $('#fld_ordinaryendThu').val(data.ordinaryendThu);
                        $('#fld_ordinaryendFri').val(data.ordinaryendFri);
                        $('#fld_ordinaryendSat').val(data.ordinaryendSat);
                        $('#fld_ordinaryendSun').val(data.ordinaryendSun);
                    });
                }
                $('#btn_cancelperiods').hide();
                $('#div_periods').hide();
                calculate_hoursperweek();
                $('#div_details').show();
                $('#btn_canceldetails').show();
                $('#btn_submit').show();
            })

            $('.ordinary').change(function () {
                calculate_hoursperweek();
            })

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

            $('#search').click(function () {
                $('#div_periods').hide();
                $('#div_details').hide();
                $('#btn_submit').hide();
                $('#fld_person').val('');
                $('#search').hide();
                $('#fld_person').prop('disabled', false);

            });


            $("#form1").validate();


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


            $(".hours").keydown(function (event) {
                validatenumeric(this,event,true)
            });
            $("#fld_hourlyrate").keydown(function (event) {
                validatenumeric(this,event,true)
            });


            /*
            var attendancestring = "No,Yes,Partial";
            var attendance = attendancestring.split(',');

            $('body').on('change', '.attendance', function () {
                name = $(this).attr("name");
                //if (name.substring(0, 8) != "updated_") {
                //    id = name.split("_")[1];
                //    $(this).attr('name', "updated_attendance_" + id);
                //}
                if (name == "") {
                    id = $(this).attr('attendanceid');
                    $(this).attr('name', "update_attendance_" + id);
                }



            });

            $('body').on('change', '.capacity', function () {
                //name = $(this).attr("name");
                //if (name.substring(0, 8) != "updated_") {
                //    id = name.split("_")[1];
                //    $(this).attr('name', "updated_capacity_" + id);
                //}
                if (name == "") {
                    id = $(this).attr('capacityid');
                    $(this).attr('name', "update_capacity_" + id);
                }

            });

            $('tbody tr').each(function () {
                id = $(this).data('id');
                //var s = $('<select class="form-control attendance" name="attendance_' + id + '" />');
                var s = $('<select class="form-control attendance" name="" attendanceid="' + id + '" />');
                value = $(this).find('td').eq(2).text();
                attendance.forEach(function (item, index) {
                    if (value == item) {
                        selectedvalue = true;
                    } else {
                        selectedvalue = false;
                    }
                    $("<option />", { value: item, text: item, selected: selectedvalue }).appendTo(s);
                });
                $(this).find('td').eq(2).html(s);

                //var s = $('<select class="form-control capacity" name="capacity_' + id + '" />');
                var s = $('<select class="form-control capacity" name="" capacityid="' + id + '" />');
                value = $(this).find('td').eq(3).text();
                capacity.forEach(function (item, index) {
                    if (value == item) {
                        selectedvalue = true;
                    } else {
                        selectedvalue = false;
                    }
                    $("<option />", { value: item, text: item, selected: selectedvalue }).appendTo(s);
                });
                $(this).find('td').eq(3).html(s);

            });
            */


        }); //document.ready

        function calculate_hoursperweek() {
            hoursperweek = 0;
            $('.ordinary').each(function () {
                hoursperweek = hoursperweek + parseFloat($(this).val());
            });
            $('#fld_hoursperweek').val(hoursperweek);
        }

        function validatenumeric(obj,event,allowpoint) {
            if (event.shiftKey == true) {
                event.preventDefault();
            }
            //alert(event.keyCode);
            if ((event.keyCode >= 48 && event.keyCode <= 57) ||
                (event.keyCode >= 96 && event.keyCode <= 105) ||
                event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 ||
                event.keyCode == 39 || event.keyCode == 46 || ((event.keyCode == 190 || event.keyCode == 110)  && allowpoint)) {
            } else {
                event.preventDefault();
            }

            if ($(obj).val().indexOf('.') !== -1 && (event.keyCode == 190 || event.keyCode == 110))
                event.preventDefault();
                //if a decimal has been added, disable the "."-button
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="fld_employee_ctr" name="fld_employee_ctr" />
    <input type="hidden" id="fld_person_ctr" name="fld_person_ctr" />

    <%=username %>
    <!--
    <input type="button" value="test" id="btn_test" />
    <input type="text" id="hidden_dirty" />
    -->
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p></p>
    </div>

    <div class="toprighticon">
        <button id="search" type="button" class="btn btn-info" value="Search" style="display:none">Search</button>
        <button id="assistance" type="button" class="btn btn-info">Assistance</button>
        <button id="menu" type="button" class="btn btn-info">MENU</button>
    </div>
    <div class="bottomrighticon">
        <button id="btn_canceldetails" type="button" class="cancel btn btn-info" style="display: none">Cancel</button>
        <button id="btn_cancelperiods" type="button" class="cancel btn btn-info" style="display: none">Cancel</button>
        <button id="btn_submit" type="button" class="submit btn btn-info" style="display: none">Submit</button>
    </div>
    <h1>Employee Maintenance
    </h1>
    <div class="form-horizontal">

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_person">Name</label>
            <div class="col-sm-8">
                <input type="text" id="fld_person" name="fld_person" class="form-control" required />
            </div>
        </div>

        <div id="div_details" style="display: none">
            <div class="form-group">
                <label class="control-label col-sm-4" for="fld_position">Position</label>
                <div class="col-sm-8">
                    <input type="text" id="fld_position" name="fld_position" class="form-control" required />
                </div>
            </div>

            <div class="form-group">
                <label for="fld_startdate" class="control-label col-sm-4">Start Date</label>
                <div class="col-sm-4">
                    <div class="input-group date">
                        <input id="fld_startdate" name="fld_startdate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" required="required" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <label for="fld_enddate" class="control-label col-sm-4">End Date</label>
                <div class="col-sm-4">
                    <div class="input-group date">
                        <input id="fld_enddate" name="fld_enddate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="fld_hourlyrate">Hourly rate</label>
                <div class="col-sm-4">
                    <input type="text" id="fld_hourlyrate" name="fld_hourlyrate" maxlength="5" class="form-control" required />
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="fld_paytype">Pay Type</label>
                <div class="col-sm-4">
                    <select id="fld_paytype" name="fld_paytype" class="form-control">
                        <option value="">--- Please select ---</option>
                        <% 
                            Dictionary<string, string> paytypeoptions = new Dictionary<string, string>();
                            paytypeoptions["type"] = "select";
                            paytypeoptions["valuefield"] = "value";
                            Response.Write(Generic.Functions.buildselection(paytypes, "", paytypeoptions));
                        %>
                    </select>
                </div>
            </div>

             <div class="form-group">
                <label class="control-label col-sm-4" for="fld_note">Note</label>
                <div class="col-sm-4">
                    <textarea id="fld_note" name="fld_note" rows="5" class="form-control" required></textarea>
                </div>
            </div>

            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th></th>
                        <th>Weekly total</th>
                        <th>Monday</th>
                        <th>Tuesday</th>
                        <th>Wednesday</th>
                        <th>Thursday</th>
                        <th>Friday</th>
                        <th>Saturday</th>
                        <th>Sunday</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><b>Ordinary hours</b></td>
                        <td><input type="text" id="fld_hoursperweek" name="fld_hoursperweek" readonly="readonly" /></td>
                        <td>
                            <input id="fld_hoursMon" name="fld_hoursMon" maxlength="5" class="form-control ordinary hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_hoursTue" name="fld_hoursTue" maxlength="5" class="form-control ordinary hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_hoursWed" name="fld_hoursWed" maxlength="5" class="form-control ordinary hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_hoursThu" name="fld_hoursThu" maxlength="5" class="form-control ordinary hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_hoursFri" name="fld_hoursFri" maxlength="5" class="form-control ordinary hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_hoursSat" name="fld_hoursSat" maxlength="5" class="form-control ordinary hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_hoursSun" name="fld_hoursSun" maxlength="5" class="form-control ordinary hours" type="text" value="0" /></td>
                    </tr>
                    <tr>
                        <td><b>Lunch</b></td>
                        <td></td>
                        <td>
                            <input id="fld_lunchhoursMon" name="fld_lunchhoursMon" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_lunchhoursTue" name="fld_lunchhoursTue" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_lunchhoursWed" name="fld_lunchhoursWed" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_lunchhoursThu" name="fld_lunchhoursThu" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_lunchhoursFri" name="fld_lunchhoursFri" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_lunchhoursSat" name="fld_lunchhoursSat" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_lunchhoursSun" name="fld_lunchhoursSun" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                    </tr>
                    <tr>
                        <td><b>Dinner</b></td>
                        <td></td>
                        <td>
                            <input id="fld_dinnerhoursMon" name="fld_dinnerhoursMon" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_dinnerhoursTue" name="fld_dinnerhoursTue" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_dinnerhoursWed" name="fld_dinnerhoursWed" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_dinnerhoursThu" name="fld_dinnerhoursThu" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_dinnerhoursFri" name="fld_dinnerhoursFri" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_dinnerhoursSat" name="fld_dinnerhoursSat" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                        <td>
                            <input id="fld_dinnerhoursSun" name="fld_dinnerhoursSun" maxlength="5" class="form-control hours" type="text" value="0" /></td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div id="div_periods" style="display: none">
            Show periods that exist for this employee and allow selection
            <a class="select_period">New</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
