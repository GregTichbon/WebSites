<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VehicleBookings.aspx.cs" Inherits="TeOraHouWhanganui.Private.VehicleBookings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--<script src="https://cdn.jsdelivr.net/npm/fullcalendar@5/main.min.js"></script>-->
    <script src="https://cdn.jsdelivr.net/npm/fullcalendar-scheduler@5/main.min.js"></script>
    <!--<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/fullcalendar@5/main.min.css">-->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/fullcalendar-scheduler@5/main.min.css">
    <script>

        document.addEventListener('DOMContentLoaded', function () {
            var calendarEl = document.getElementById('calendar');
            var calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'resourceTimeGridDay',
                resourceOrder: 'sequence',
                resources: "/_Dependencies/data.asmx/get_vehicles",
                events: "/_Dependencies/data.asmx/get_vehicle_bookings"/*
                 [
                    { id: '1', resourceId: '2', start: '2020-12-23T02:00:00', end: '2020-12-23T07:00:00', title: 'Greg - Mentoring' },
                    { id: '2', resourceId: '3', start: '2020-12-23T06:00:00', end: '2020-12-23T09:00:00', title: 'Keegan - Rally car racing' },
                ]*/,
                dateClick: function (info) {
                    alert('clicked ' + info.dateStr);
                },
                select: function (info) {
                    alert('selected ' + info.startStr + ' to ' + info.endStr);
                },
                editable: true

            });
            calendar.render();
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id="calendar"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
