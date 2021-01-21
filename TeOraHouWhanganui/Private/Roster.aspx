<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Roster.aspx.cs" Inherits="TeOraHouWhanganui.Private.Roster1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--<script src="https://cdn.jsdelivr.net/npm/fullcalendar@5/main.min.js"></script>-->
    <script src="https://cdn.jsdelivr.net/npm/fullcalendar-scheduler@5/main.min.js"></script>
    <!--<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/fullcalendar@5/main.min.css">-->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/fullcalendar-scheduler@5/main.min.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script>

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
                resources: "/_Dependencies/data.asmx/get_entitys_for_program?program=17&worker=1",
                refetchResourcesOnNavigate: true,
                events: "/_Dependencies/data.asmx/get_roster?program=17",
                /*
                events: function (info, callback) {
                    $.ajax({
                        url: "/_Dependencies/data.asmx/get_roster?program=17",
                        data: "start=" + info.start.valueOf() + "&end=" + info.end.valueOf(),
                        dataType: "json",
                        success: function (doc) {
                            var events = [];   //javascript event object created here
                            $(doc).each(function () {
                                events.push({
                                    id: $(this).attr('id'),
                                    resourceId: $(this).attr('resourceId'),
                                    title: $(this).attr('title'),  //your calevent object has identical parameters 'title', 'start', ect, so this will work
                                    start: $(this).attr('start'), // will be parsed into DateTime object    
                                    end: $(this).attr('end')
                                });
                            });
                            //refetchResources();
                            callback(events);
                        }
                    });
                },
                */
                /* 
                 [
                    { id: '1', resourceId: '2', start: '2020-12-23T02:00:00', end: '2020-12-23T07:00:00', title: 'Greg - Mentoring' },
                    { id: '2', resourceId: '3', start: '2020-12-23T06:00:00', end: '2020-12-23T09:00:00', title: 'Keegan - Rally car racing' },
                ],
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
                /*
                datesSet: function (info) {
                    alert(1);
                },
                */
                select: function (info) {
                    $('#fld_person').val('');
                    $('#fld_person').data('person_ctr', '');
                    $('#fld_detail').val('');
                    $("#dialog_edit").dialog({
                        title: 'Create ' + moment(info.startStr).format('D MMM YYYY H:mm') + ' to ' + moment(info.endStr).format('D MMM YYYY H:mm'),
                        resizable: false,
                        height: 600,
                        width: 800,
                        modal: true,
                        buttons: {
                            "Cancel": function () {
                                $(this).dialog("close");
                            },
                            "Confirm": function () {
                                id = update_roster({ roster_ctr: "new", vehicle_ctr: info.resource.id, start: moment(info.startStr).format('D-MMM-YYYY H:mm'), end: moment(info.endStr).format('D-MMM-YYYY H:mm'), person_ctr: $('#fld_person').data('person_ctr'), details: $('#fld_detail').val() });
                                calendar.addEvent({
                                    id: id,
                                    title: $('#fld_person').val() + ' - ' + $('#fld_detail').val(),
                                    resourceId: info.resource.id,
                                    start: info.startStr,
                                    end: info.endStr,
                                    extendedProps: {
                                        person: $('#fld_person').val(),
                                        person_ctr: $('#fld_person').data('person_ctr'),
                                        detail: $('#fld_detail').val()
                                    }
                                });
                                $(this).dialog("close");
                            }
                        }
                    });
                },
                eventClick: function (info) {
                    $('#fld_person').val(info.event.extendedProps.person);
                    $('#fld_person').data('person_ctr', info.event.extendedProps.person_ctr);
                    $('#fld_detail').val(info.event.extendedProps.detail)

                    $("#dialog_edit").dialog({
                        title: 'Edit ' + moment(info.event.startStr).format('D MMM YYYY H:mm') + ' to ' + moment(info.event.endStr).format('D MMM YYYY H:mm'),
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
                                $(this).dialog("close");
                            },
                            "Confirm": function () {
                                update_roster({ roster_ctr: info.event.id, vehicle_ctr: info.event._def.resourceIds[0], start: moment(info.event.startStr).format('D-MMM-YYYY H:mm'), end: moment(info.event.endStr).format('D-MMM-YYYY H:mm'), person_ctr: $('#fld_person').data('person_ctr'), details: $('#fld_detail').val() });

                                info.event.setProp("title", $('#fld_person').val() + ' - ' + $('#fld_detail').val());
                                info.event.extendedProps.person = $('#fld_person').val();
                                info.event.extendedProps.person_ctr = $('#fld_person').data('person_ctr');
                                info.event.extendedProps.detail = $('#fld_detail').val();

                                $(this).dialog("close");
                            }
                        }
                    });
                },
                eventResize: function (info) {
                    update_roster({ roster_ctr: info.event.id, vehicle_ctr: info.event._def.resourceIds[0], start: moment(info.event.startStr).format('D-MMM-YYYY H:mm'), end: moment(info.event.endStr).format('D-MMM-YYYY H:mm'), person_ctr: info.event.extendedProps.person_ctr, details: info.event.extendedProps.detail });
                    /*
                    if (!confirm("is this okay?")) {
                        info.revert();
                    }
                    */
                },
                eventDrop: function (info) {
                    /*
                    $.post("/_Dependencies/posts.asmx/update_roster", createFormVars({ vehicle_ctr: info.event._def.resourceIds[0], start: moment(info.event.startStr).format('D-MMM-YYYY H:mm'), end: moment(info.event.endStr).format('D-MMM-YYYY H:mm'), person_ctr: info.event.extendedProps.person_ctr, details: info.event.extendedProps.detail })
                        , function (data) {
                            alert(data);
                        });
                        */

                    update_roster({ roster_ctr: info.event.id, vehicle_ctr: info.event._def.resourceIds[0], start: moment(info.event.startStr).format('D-MMM-YYYY H:mm'), end: moment(info.event.endStr).format('D-MMM-YYYY H:mm'), person_ctr: info.event.extendedProps.person_ctr, details: info.event.extendedProps.detail });


                    /*
                    if (!confirm("is this okay?")) {
                        info.revert();
                    }
                    */
                },
                editable: true,
                selectable: true

            });
        calendar.render();
        });



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
            $("#fld_person").autocomplete({
                source: "<%: ResolveUrl("~/_Dependencies/data.asmx/Person_name_autocomplete?options=persons")%>",
                minLength: 2,
                appendTo: "#dialog_edit",
                select: function (event, ui) {
                    event.preventDefault();
                    selected = ui.item;
                    //console.log(selected.value);
                    $('#fld_person').val(selected.name);
                    $('#fld_person').data('person_ctr', selected.value);
                    //selected.person_ctr
                }
            })

        });

        function createFormVars(obj) {
            var arr = [];
            for (const [key, value] of Object.entries(obj)) {
                arr.push({ 'name': key, 'value': value });
            }
            var formVars = JSON.stringify({ formVars: arr });
            return formVars;
        }


        function update_roster(data) {
            var id = '';
            var formData = createFormVars(data);
            $.ajax({
                type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                url: '/_dependencies/posts.asmx/update_roster', // the url where we want to POST
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

    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p>Coming soon</p>
    </div>
    <div id="dialog_edit" title="Edit" style="display: none" class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_person">Person</label>
            <div class="col-sm-8">
                <input id="fld_person" name="fld_name" type="text" class="form-control" />
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

