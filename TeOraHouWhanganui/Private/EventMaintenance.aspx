<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EventMaintenance.aspx.cs" Inherits="TeOraHouWhanganui.Private.EventMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <!--<script src='//cdn.tinymce.com/4/tinymce.min.js'></script>-->
    <script src="https://cdn.tiny.cloud/1/0rm57f61mkhtljml272kg6rniw3zjw84hmu9q5zoyebn7sb2/tinymce/5/tinymce.min.js" referrerpolicy="origin"></script>


    <script type="text/javascript">

        var event_ctr = "<%=ViewState["event_ctr"]%>";

        $(document).ready(function () {
            //Generic.Functions.googleanalyticstracking()%>

            if (event_ctr != 'new') {
                $('#fld_program option:not(:selected)').prop('disabled', true);
                $('#div_control').show();
            } 


            function makedirty() {
                //$('#hidden_dirty').addClass('dirty');
                $('#form1').addClass('dirty');
            }
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
                window.location.href = "<%=ResolveUrl("~/private/eventsearch.aspx")%>";
            });
            $("#form1").validate();

            $('.Finished, .Deceased, .Casual').hide();
            
            $('.datetime').datetimepicker({
                format: 'D MMM YYYY HH:mm',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true,
                stepping: 15

                //,maxDate: moment().add(-1, 'year')
            });


            var attendancestring = "No,Yes,Partial";
            var attendance = attendancestring.split(',');

            var capacitystring = ",Worker";
            var capacity = capacitystring.split(',');

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
                counter();
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
                counter();
            });

            $('body').on('change', '.notes', function () {
                //name = $(this).attr("name");
                //if (name.substring(0, 8) != "updated_") {
                //    id = name.split("_")[1];
                //    $(this).attr('name', "updated_capacity_" + id);
                //}
                if (name == "") {
                    id = $(this).attr('notesid');
                    $(this).attr('name', "update_notes_" + id);
                }
                counter();
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

                value = $(this).find('td').eq(4).text();
                var s = $('<textarea class="form-control notes" maxlength=\"900\" name="" notesid="' + id + '" />').text(value);
                $(this).find('td').eq(4).html(s);



            });

            $('.statusfilter').change(function () {
                if (this.checked) {
                    $('.attendancefilter').prop('checked', false);
                    $('.notesfilter').prop('checked', false);
                }
                filter();
            });

            $('.attendancefilter').change(function () {
                if (this.checked) {
                    $('.statusfilter').prop('checked', false);
                    $('.notesfilter').prop('checked', false);
                }
                filter();
            });

            $('.notesfilter').change(function () {
                if (this.checked) {
                    $('.statusfilter').prop('checked', false);
                    $('.attendancefilter').prop('checked', false);
                }
                filter();
            });

            filter();

            counter();


        }); //document.ready

        function filter() {
            var selected = [];
            $('.statusfilter:checked').each(function () {
                selected.push($(this).val());
            });
            attendaceselected = $('.attendancefilter').prop('checked');
            notesselected = $('.notesfilter').prop('checked');
            $('tbody tr').each(function () {
                thisstatus = $(this).attr("status");
                thisattendance = $(this).find('.attendance').val();
                thisnotes = $(this).find('.notes').text();
                if (selected.indexOf(thisstatus) != -1 || (attendaceselected && (thisattendance == 'Yes' || thisattendance == 'Partial')) || (notesselected && thisnotes != "")) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            });
        }

        function counter() {
            var cnt = 0;
            $('.attendance').each(function () {
                status = $(this).val();
                if (status == 'Yes' || status == 'Partial') {
                    cnt++;
                }
            });
            $('#counter').text('Counter: ' + cnt + ' attended');
        }

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
        <input type="button" id="search" class="btn btn-info" value="Search" />
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <div class="bottomrighticon">
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />
    </div>
    <h1>Event Maintenance
    </h1>
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_program">Program</label>
            <div class="col-sm-4">
                <select id="fld_program" name="fld_program" class="form-control" required="required">
                    <option value="">--- Please select ---</option>
                    <% 
                        Dictionary<string, string> programoptions = new Dictionary<string, string>();
                        programoptions["type"] = "select";
                        programoptions["valuefield"] = "value";
                        Response.Write(Generic.Functions.buildselection(programs, fld_programid, programoptions));
                    %>
                </select>
            </div>
        </div>

        <div class="form-group">
            <label for="fld_startdate" class="control-label col-sm-4">Start Date/Time</label>
            <div class="col-sm-4">
                <div class="input-group datetime">
                    <input id="fld_startdate" name="fld_startdate" placeholder="eg: 23 Jun 1985 21:00" type="text" class="form-control" required="required" value="<%: fld_startdate %>" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <label for="fld_enddate" class="control-label col-sm-4">End Date/Time</label>
            <div class="col-sm-4">
                <div class="input-group datetime">
                    <input id="fld_enddate" name="fld_enddate" placeholder="eg: 23 Jun 1985 21:00" type="text" class="form-control" value="<%: fld_enddate %>" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>


        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_description">Description</label>
            <div class="col-sm-8">
                <input type="text" id="fld_description" name="fld_description" class="form-control" value="<%: fld_description %>" required />
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_note">Note</label>
            <div class="col-sm-8">
                <textarea id="fld_note" name="fld_note" class="form-control"><%: fld_note %></textarea>
            </div>
        </div>
    </div>
    <div id="div_control" style="display:none">
        Show ==> &nbsp;&nbsp;&nbsp;&nbsp;<input class="statusfilter" checked type="checkbox" value="Current" />
        Current&nbsp;&nbsp;&nbsp;&nbsp;<input class="statusfilter" type="checkbox" value="Casual" />
        Casual&nbsp;&nbsp;&nbsp;&nbsp;<input class="statusfilter" type="checkbox" value="Finished" />
        Finished&nbsp;&nbsp;&nbsp;&nbsp;<input class="statusfilter" type="checkbox" value="Deceased" />
        Deceased<br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;or ==> &nbsp;&nbsp;&nbsp;&nbsp;<input class="attendancefilter" type="checkbox" value="Attended" /> Attended<br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;or ==> &nbsp;&nbsp;&nbsp;&nbsp;<input class="notesfilter" type="checkbox" value="Notes" />
        Has Notes
    <p id="counter"></p>
    </div>
    <table class="table table-striped">
        <%= html_attendance %>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
