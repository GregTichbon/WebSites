<%@ Page Title="TOHW Vehicle Booking" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VehicleBookings.aspx.cs" Inherits="TeOraHouWhanganui.Private.VehicleBookings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--<script src="https://cdn.jsdelivr.net/npm/fullcalendar@5/main.min.js"></script>-->
    <script src="https://cdn.jsdelivr.net/npm/fullcalendar-scheduler@5/main.min.js"></script>
    <!--<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/fullcalendar@5/main.min.css">-->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/fullcalendar-scheduler@5/main.min.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js"></script>
    <link href="/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <script src="/_Dependencies/Support.js"></script>
    <script>

        var gotodate;

        document.addEventListener('DOMContentLoaded', function () {
            var calendarEl = document.getElementById('calendar');
            var calendar = new FullCalendar.Calendar(calendarEl, {
                titleFormat: { // will produce something like "Tuesday, September 18, 2018"
                    month: 'long',
                    year: 'numeric',
                    day: 'numeric',
                    weekday: 'long'
                },
                height: "auto",
                initialView: 'resourceTimeGridDay',
                eventOverlap: false,
                resourceOrder: 'sequence',
                resources: "/_Dependencies/data.asmx/get_vehicles",
                refetchResourcesOnNavigate: true,
                events: "/_Dependencies/data.asmx/get_vehicle_bookings"/*
                 [
                    { id: '1', resourceId: '2', start: '2020-12-23T02:00:00', end: '2020-12-23T07:00:00', title: 'Greg - Mentoring' },
                    { id: '2', resourceId: '3', start: '2020-12-23T06:00:00', end: '2020-12-23T09:00:00', title: 'Keegan - Rally car racing' },
                ]*/,
                /*
                dateClick: function (info) {
                    $("#dialog_edit").dialog({
                        title: 'clicked ' + info.dateStr,
                        resizable: false,
                        height: 600,
                        width: 800,
                        modal: true
                    });
                },
                */
                buttonText: {
                    month: 'Month',
                    agendaDay: 'Day',
                    agendaWeek: 'Week',
                    today: 'Today',
                    resourceTimeline: 'Time Line',
                    resourceTimeGridDay: "Day Grid"
                },
                customButtons: {
                    gotodatebutton: {
                        text: 'Go to date',
                        click: function () {
                            $("#dialog_gotodate").dialog({
                                buttons: {
                                    "Cancel": function () {
                                        $(this).dialog("close");
                                    },
                                    "Use": function () {
                                        calendar.gotoDate(gotodate);
                                        $(this).dialog("close");
                                    }
                                }
                            });
                        }
                    }
                },

                headerToolbar: {
                    left: 'prev,next today gotodatebutton',
                    center: 'title',
                    //right: 'dayGridMonth,timeGridWeek,timeGridDay,resourceTimeline,resourceTimeGridDay'
                    right: 'dayGridMonth,timeGridWeek,resourceTimeline,resourceTimeGridDay'
                },


                select: function (info) {
                    if (!calendar.view.type.startsWith("resource")) {
                        alert('Working on .... To allow edit then the vehicle must be able to be selected in the dialog');
                    } else {
                        $('#fld_startdatetime').val(moment(info.startStr).format('D MMM YYYY HH:mm'));
                        $('#fld_enddatetime').val(moment(info.endStr).format('D MMM YYYY HH:mm'));
                        $('#fld_worker').val('');
                        $('#fld_worker').data('worker_ctr', '');
                        $('#fld_detail').val('');
                        $("#dialog_edit").dialog({
                            title: 'Create ' + moment(info.startStr).format('D MMM YYYY HH:mm') + ' to ' + moment(info.endStr).format('D MMM YYYY HH:mm'),
                            resizable: false,
                            height: 600,
                            width: 800,
                            modal: true,
                            buttons: {
                                "Cancel": function () {
                                    $(this).dialog("close");
                                },
                                "Confirm": function () {
                                    if ($('#fld_detail').val() == '' || $('#fld_worker').data('worker_ctr') == '') {
                                        alert('You must select a worker and provide some detail');
                                    } else {
                                        id = update_vehicle_booking({ vehicle_booking_ctr: "new", vehicle_ctr: info.resource.id, start: moment($('#fld_startdatetime').val()).format('D-MMM-YYYY HH:mm'), end: moment($('#fld_enddatetime').val()).format('D-MMM-YYYY HH:mm'), worker_ctr: $('#fld_worker').data('worker_ctr'), details: $('#fld_detail').val() });
                                        resourcename = calendar.getResourceById(info.resource.id).extendedProps.name;
                                        calendar.addEvent({
                                            id: id,
                                            title: "<b>" + resourcename + "</b><br />" + moment($('#fld_startdatetime').val()).format('D MMM YY HH:mm') + " -<br /> " + moment($('#fld_enddatetime').val()).format('D MMM YY HH:mm') + "<br />" + $('#fld_worker').val() + '<br />' + $('#fld_detail').val(),
                                            resourceId: info.resource.id,
                                            start: moment($('#fld_startdatetime').val()).format("YYYY-MM-DDTHH:mm:ssZ"), //info.startStr,
                                            end: moment($('#fld_enddatetime').val()).format("YYYY-MM-DDTHH:mm:ssZ"), //info.endStr,
                                            extendedProps: {
                                                worker: $('#fld_worker').val(),
                                                worker_ctr: $('#fld_worker').data('worker_ctr'),
                                                detail: $('#fld_detail').val()
                                            }
                                        });
                                        $(this).dialog("close");
                                    }
                                }
                            }
                        });
                    }
                },
                eventClick: function (info) {
                    if (!calendar.view.type.startsWith("resource")) {
                        alert('Working on .... To allow edit then the vehicle must be able to be selected in the dialog');
                    } else {
                        $('#fld_startdatetime').val(moment(info.event.startStr).format('D MMM YYYY HH:mm'));
                        $('#fld_enddatetime').val(moment(info.event.endStr).format('D MMM YYYY HH:mm'));
                        $('#fld_worker').val(info.event.extendedProps.worker);
                        $('#fld_worker').data('worker_ctr', info.event.extendedProps.worker_ctr);
                        $('#fld_detail').val(info.event.extendedProps.detail);
                        $("#dialog_edit").dialog({
                            title: 'Edit ' + moment(info.event.startStr).format('D MMM YYYY HH:mm') + ' to ' + moment(info.event.endStr).format('D MMM YYYY HH:mm'),
                            resizable: false,
                            height: 600,
                            width: 800,
                            modal: true,
                            buttons: {
                                "Cancel": function () {
                                    $(this).dialog("close");
                                },
                                "Remove": function () {
                                    info.event.remove();
                                    update_vehicle_booking({ vehicle_booking_ctr: -info.event.id });
                                    $(this).dialog("close");
                                },
                                "Confirm": function () {
                                    if ($('#fld_detail').val() == '' || $('#fld_worker').data('worker_ctr') == '') {
                                        alert('You must select a worker and provide some detail');
                                    } else {
                                        update_vehicle_booking({ vehicle_booking_ctr: info.event.id, vehicle_ctr: info.event._def.resourceIds[0], start: moment($('#fld_startdatetime').val()).format('D-MMM-YYYY HH:mm'), end: moment($('#fld_enddatetime').val()).format('D-MMM-YYYY HH:mm'), worker_ctr: $('#fld_worker').data('worker_ctr'), details: $('#fld_detail').val() });
                                        info.event.setStart(moment($('#fld_startdatetime').val()).format("YYYY-MM-DDTHH:mm:ssZ"));
                                        info.event.setEnd(moment($('#fld_enddatetime').val()).format("YYYY-MM-DDTHH:mm:ssZ"));
                                        var resources = info.event.getResources();
                                        info.event.setProp("title", "<b>" + resources[0].extendedProps.name + "</b><br />" + moment($('#fld_startdatetime').val()).format('D MMM YY HH:mm') + " -<br /> " + moment($('#fld_enddatetime').val()).format('D MMM YY HH:mm') + "<br />" + $('#fld_worker').val() + '<br />' + $('#fld_detail').val());
                                        info.event.setExtendedProp('worker', $('#fld_worker').val());
                                        info.event.setExtendedProp('worker_ctr', $('#fld_worker').data('worker_ctr'));
                                        info.event.setExtendedProp('detail', $('#fld_detail').val());

                                        $(this).dialog("close");
                                    }
                                }
                            }
                        });
                    }
                },
                eventContent: function (arg) {
                    /*
                    let arrayOfDomNodes = []
                    let titleEvent = document.createElement('div')
                    if (arg.event._def.title) {
                        titleEvent.innerHTML = arg.event._def.title;
                        titleEvent.classList = "fc-event-title fc-sticky";
                    }
                    arrayOfDomNodes = [titleEvent]
                    */

                    /*
                    // image event
                    let imgEventWrap = document.createElement('div')
                    if (arg.event.extendedProps.image_url) {
                        let imgEvent = '<img src="' + arg.event.extendedProps.image_url + '" >'
                        imgEventWrap.classList = "fc-event-img"
                        imgEventWrap.innerHTML = imgEvent;
                    }

                    arrayOfDomNodes = [titleEvent, imgEventWrap]
                    */

                    //return { domNodes: arrayOfDomNodes }

                    return { html: arg.event.title };
                },


                //eventDidMount: function (info) {
                /*
                var tooltip = new Tooltip(info.el, {
                    title: info.event.extendedProps.description,
                    placement: 'top',
                    trigger: 'hover',
                    container: 'body'
                });
                */
                //console.log(info.event);
                //info.event.title = "---- YOUR TEXT----"
                //info.event.setProp('title', 'xxxxxxxxxxxx<br />zzzzzzz')
                //},
                eventResize: function (info) {
                    update_vehicle_booking({ vehicle_booking_ctr: info.event.id, vehicle_ctr: info.event._def.resourceIds[0], start: moment(info.event.startStr).format('D-MMM-YYYY HH:mm'), end: moment(info.event.endStr).format('D-MMM-YYYY HH:mm'), worker_ctr: info.event.extendedProps.worker_ctr, details: info.event.extendedProps.detail });
                    alert('Need to rebuild title');
                    /*
                    if (!confirm("is this okay?")) {
                        info.revert();
                    }
                    */
                },
                eventDrop: function (info) {
                    /*
                    $.post("/_Dependencies/posts.asmx/update_vehicle_booking", createFormVars({ vehicle_ctr: info.event._def.resourceIds[0], start: moment(info.event.startStr).format('D-MMM-YYYY HH:mm'), end: moment(info.event.endStr).format('D-MMM-YYYY HH:mm'), worker_ctr: info.event.extendedProps.worker_ctr, details: info.event.extendedProps.detail })
                        , function (data) {
                            alert(data);
                        });
                        */

                    update_vehicle_booking({ vehicle_booking_ctr: info.event.id, vehicle_ctr: info.event._def.resourceIds[0], start: moment(info.event.startStr).format('D-MMM-YYYY HH:mm'), end: moment(info.event.endStr).format('D-MMM-YYYY HH:mm'), worker_ctr: info.event.extendedProps.worker_ctr, details: info.event.extendedProps.detail });
                    alert('Need to rebuild title');
                    /*
                    if (!confirm("is this okay?")) {
                        info.revert();
                    }
                    */
                },

                resourceLabelContent: function (arg) {
                    return { html: arg.resource.title };
                },
                /*
                resourcesSet: function (resource) {
                    console.log(resource);
                },
                */
                editable: true,
                selectable: true

            });
            calendar.render();
            window.scrollTo(0, document.body.scrollHeight);
        });



        $(document).ready(function () {


            $('#supporturl').val(window.location.href);
            $('body').append('<form id="frmDialog">');
            $('#support').click(function () {
                $("#dialog_support").dialog({
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true,
                    buttons: {
                        "Send": function () {
                            if ($("#frmDialog").valid()) {
                                $(this).dialog("close");
                                var arr = [];
                                arr.push({ 'name': 'supportusername', 'value': $('#supportusername').val() });
                                arr.push({ 'name': 'supporturl', 'value': $('#supporturl').val() });
                                arr.push({ 'name': 'supportmessage', 'value': $('#supportmessage').val() });
                                var formVars = JSON.stringify({ formVars: arr });
                                $.ajax({
                                    type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                                    url: '/_dependencies/posts.asmx/support', // the url where we want to POST
                                    contentType: "application/json; charset=utf-8",
                                    data: formVars,
                                    dataType: 'json', // what type of data do we expect back from the server
                                    async: false,
                                    success: function (result) {
                                    },
                                    error: function (xhr, status) {
                                        alert('error');
                                    }
                                });
                            }
                        }
                    }
                    , appendTo: "#frmDialog"
                });
            })
            $("#frmDialog").validate();

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
           $("#fld_worker").autocomplete({
               source: "<%: ResolveUrl("~/_Dependencies/data.asmx/Person_name_autocomplete?options=Workers")%>",
                minLength: 2,
                appendTo: "#dialog_edit",
                select: function (event, ui) {
                    event.preventDefault();
                    selected = ui.item;
                    $('#fld_worker').val(selected.name);
                    $('#fld_worker').data('worker_ctr', selected.value);
                },
                change: function (event, ui) {
                    if (!ui.item) {
                        $("#fld_worker").val("");
                    }
                }
            })

           $('.datetime').datetimepicker({
               format: 'DD MMM YYYY HH:mm',
               extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
               //daysOfWeekDisabled: [0, 6],
               showClear: true,
               viewDate: false,
               useCurrent: true,
               stepping: 30,
               sideBySide: true

               //,maxDate: moment().add(-1, 'year')
           });

           $('#gotodate').datetimepicker({
               format: 'DD MMM YYYY',
               extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
               //daysOfWeekDisabled: [0, 6],
               showClear: true,
               viewDate: false,
               useCurrent: true,
               inline: true
           });
           $("#gotodate").on("dp.change", function (e) {
               gotodate = moment(e.date).format('YYYY-MM-DD');
           });
       });





        function createFormVars(obj) {
            var arr = [];
            for (const [key, value] of Object.entries(obj)) {
                arr.push({ 'name': key, 'value': value });
            }
            var formVars = JSON.stringify({ formVars: arr });
            return formVars;
        }

        function update_vehicle_booking(data) {
            var id = '';
            var formData = createFormVars(data);
            $.ajax({
                type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                url: '/_dependencies/posts.asmx/update_vehicle_booking', // the url where we want to POST
                contentType: "application/json; charset=utf-8",
                data: formData,
                dataType: 'json', // what type of data do we expect back from the server
                async: false,
                success: function (result) {
                    id = result.d;
                },
                error: function (xhr, status) {
                    alert('error');
                }
            });
            return id;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- #include file = "/_dependencies/support.html" -->
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p>Click, hold, and drag down in empty space to create a new booking.</p>
        <p>Click, hold, and drag an existing booking to a new time and/or vehicle.</p>
        <p>Click on the top/bottom edge of an existing booking, hold, and drag up or down to change the start and/or end time.</p>
        <p>Click on an existing booking to make changes or to remove the booking.</p>
        <p>Click in a cell on the "All-Day" row to book a vehicle for the whole day.</p>
        <p>You can also change the start and/or end time in the dialog box.  This is useful if a booking will span multiple days.</p>
    </div>
    <div id="dialog_gotodate" style="display: none">
        <div id="gotodate"></div>
    </div>
    <div id="dialog_edit" title="Edit" style="display: none" class="form-horizontal">
        <div class="form-group">
            <label for="fld_vehicle_ctr" class="control-label col-sm-4">
                Vehicle
            </label>
            <div class="col-sm-8">
                <input id="fld_vehicle_ctr" name="fld_vehicle_ctr" type="text" class="form-control" value="working on" readonly="readonly" />
            </div>
        </div>


        <div class="form-group">
            <label for="fld_startdatetime" class="control-label col-sm-4">
                Start Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group datetime">
                    <input id="fld_startdatetime" name="fld_startdatetime" placeholder="eg: 23 Jun 1985 21:00" type="text" class="form-control" required="required" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label for="fld_enddatetime" class="control-label col-sm-4">
                End Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group datetime">
                    <input id="fld_enddatetime" name="fld_enddatetime" placeholder="eg: 23 Jun 1985 21:00" type="text" class="form-control" required="required" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_worker">Worker</label>
            <div class="col-sm-8">
                <input id="fld_worker" name="fld_worker" type="text" class="form-control" />
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_detail">Detail</label>
            <div class="col-sm-8">
                <textarea id="fld_detail" name="fld_detail" class="form-control"></textarea>
            </div>
        </div>

    </div>
    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <div id="calendar"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
