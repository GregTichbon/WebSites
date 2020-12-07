<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RosterMaintenance.aspx.cs" Inherits="TeOraHouWhanganui.Private.Roster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <!--<script src='//cdn.tinymce.com/4/tinymce.min.js'></script>-->
    <script src="https://cdn.tiny.cloud/1/0rm57f61mkhtljml272kg6rniw3zjw84hmu9q5zoyebn7sb2/tinymce/5/tinymce.min.js" referrerpolicy="origin"></script>


    <script type="text/javascript">

        var roster_ctr = "<%=ViewState["roster_ctr"]%>";
        var newctr = 0;

        $(document).ready(function () {

            if (roster_ctr != 'new') {
                get_roster_workers(roster_ctr);
                get_roster_persons(roster_ctr);
            }

            //Generic.Functions.googleanalyticstracking()%>
            mywidth = $(window).width() * .95;
            if (mywidth > 800) {
                mywidth = 800;
            }

            $("#fld_person_person").autocomplete({
                source: "<%: ResolveUrl("~/_Dependencies/data.asmx/Person_name_autocomplete?options=")%>",
                appendTo: "#dialog_person",
                minLength: 2,
                select: function (event, ui) {
                    event.preventDefault();
                    selected = ui.item;
                    $('#fld_person_person').attr('person_ctr', selected.person_ctr);
                    $('#fld_person_person').val(selected.name);
                }
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
                window.location.href = "<%=ResolveUrl("~/private/rostersearch.aspx")%>";
            });
            $("#form1").validate();
            //$("#form_worker").validate();
            //$("#form_person").validate();



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

            $(document).on('click', '.workeredit', function () {
                mode = $(this).text();
                if (mode == "Add") {
                    roster_worker_ctr = 'new';
                    $("#dialog_worker").find(':input').val('');
                } else {
                    tr = $(this).closest('tr');
                    roster_worker_ctr = $(tr).data("id");
                    $.getJSON("/_Dependencies/data.asmx/get_roster_worker?id=" + roster_worker_ctr, function (data) {
                        $('#fld_worker_worker').val(data.Worker_CTR);
                        $('#fld_worker_datetimestart').val(data.DateTimeStart);
                        $('#fld_worker_datetimeend').val(data.DateTimeEnd);
                        $('#fld_worker_note').val(data.Note);
                        $('#fld_worker_datetimestartactual').val(data.DateTimeStartActual);
                        $('#fld_worker_datetimeendactual').val(data.DateTimeEndActual);
                        $('#fld_worker_status').val(data.Status);
                        $('#fld_worker_worknote').val(data.WorkNotes);
                    });
                }
                $("#dialog_worker").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true
                    /*
                    ,open: function (type, data) {
                        var myarr = [];
                        var workersarray = workers.toString().split('|');
                        $.each(workersarray, function (index, value) {
                            myarr.push(value);
                        });  
                        $('#fld_worker_worker').select2();
                    }
                    */
                    , appendTo: "#form_worker"
                });

                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form_worker").valid()) {
                            var arForm = $("#form_worker").serializeArray();

                            arForm.push({ name: 'roster_worker_ctr', value: roster_worker_ctr }, { name: 'roster_ctr', value: roster_ctr });
                            var formData = JSON.stringify({ formVars: arForm });

                            $.ajax({
                                type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                                url: '/_dependencies/posts.asmx/update_roster_worker', // the url where we want to POST
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

                            get_roster_workers(roster_ctr);

                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this worker?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'View') {
                    $("#dialog_worker").dialog('option', 'buttons', myButtons);
                }
            });

            $(document).on('click', '.personedit', function () {
                mode = $(this).text();
                if (mode == "Add") {
                    roster_person_ctr = 'new';
                    $("#dialog_person").find(':input').val('');
                } else {
                    tr = $(this).closest('tr');
                    roster_person_ctr = $(tr).data("id");
                    $.getJSON("/_Dependencies/data.asmx/get_roster_person?id=" + roster_person_ctr, function (data) {
                        $('#fld_person_person').attr("person_ctr",data.Person_CTR);
                        $('#fld_person_person').val(data.personname);
                        $('#fld_person_datetimestart').val(data.DateTimeStart);
                        $('#fld_person_datetimeend').val(data.DateTimeEnd);
                        $('#fld_person_note').val(data.Note);
                        $('#fld_person_datetimestartactual').val(data.DateTimeStartActual);
                        $('#fld_person_datetimeendactual').val(data.DateTimeEndActual);
                        $('#fld_person_status').val(data.Status);
                        $('#fld_person_worknote').val(data.WorkNotes);
                    });
                }
                $("#dialog_person").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true
                    /*
                    ,open: function (type, data) {
                        var myarr = [];
                        var personsarray = persons.toString().split('|');
                        $.each(personsarray, function (index, value) {
                            myarr.push(value);
                        });  
                        $('#fld_person_person').select2();
                    }
                    */
                    , appendTo: "#form_person"
                });

                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form_person").valid()) {
                            var arForm = $("#form_person").serializeArray();

                            arForm.push({ name: 'roster_person_ctr', value: roster_person_ctr }, { name: 'roster_ctr', value: roster_ctr }, { name: 'person_ctr', value: $('#fld_person_person').attr("person_ctr") });
                            var formData = JSON.stringify({ formVars: arForm });

                            $.ajax({
                                type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                                url: '/_dependencies/posts.asmx/update_roster_person', // the url where we want to POST
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

                            get_roster_persons(roster_ctr);
                            
                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this person?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'View') {
                    $("#dialog_person").dialog('option', 'buttons', myButtons);
                }
            });

        }); //document.ready

        function get_roster_workers(roster_ctr) {
            $.post("/_Dependencies/data.aspx", { mode: "get_roster_workers", id: roster_ctr })
                .done(function (data) {
                    $("#div_workers").html(data);
                });
        }
        function get_roster_persons(roster_ctr) {
            $.post("/_Dependencies/data.aspx", { mode: "get_roster_persons", id: roster_ctr })
                .done(function (data) {
                    $("#div_persons").html(data);
                });
        }

        function get_newctr() {
            newctr++;
            return newctr;
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
        <p>A roster item requires the start and end date/time, a link to at least one worker, and a link to at least one young person.</p>
        <p>Notes are not required but can be associated to the roster event, the associated worker(s), and associated young person/people.  This information will be combined on the schedule and other reports.</p>
        <p>The start and end date/time should not be entered against the associated worker(s), and associated young person/people unless they are different to those recorded in the roster event.</p>
        <p>The Status, Actual Start Date/Time, Actual End Date/Time, and Work Note are not currently used.</p>
        <p>The associated worker(s) and associated young person/people are saved immediatly to the database.</p>
    </div>

    <div class="toprighticon">
        <input type="button" id="search" class="btn btn-info" value="Search" />
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <div class="bottomrighticon">
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />
    </div>
    <h1>Roster Event Maintenance
    </h1>
    <div class="form-horizontal">
        <div class="form-group">
            <label for="fld_startdate" class="control-label col-sm-4">Start Date/Time</label>
            <div class="col-sm-4">
                <div class="input-group datetime">
                    <input id="fld_datetimestart" name="fld_datetimestart" placeholder="eg: 23 Jun 1985 21:00" type="text" class="form-control" required="required" value="<%: fld_datetimestart %>" />
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
                    <input id="fld_datetimeend" name="fld_datetimeend" placeholder="eg: 23 Jun 1985 21:00" type="text" class="form-control" required="required" value="<%: fld_datetimeend %>" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_detail">Detail</label>
            <div class="col-sm-8">
                <textarea id="fld_detail" name="fld_detail" class="form-control" required><%: fld_detail %></textarea>
            </div>
        </div>

        <div id="div_workers">
            <%=html_workers %>
        </div>

        <div id="div_persons">
            <%=html_persons %>
        </div>
    </div>


    <div id="dialog_worker" title="Maintain Worker" style="display: none" class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_worker_worker">Worker</label>
            <div class="col-sm-8">
                <select id="fld_worker_worker" name="fld_worker_worker" class="form-control" required="required">
                    <option value="">--- Please select ---</option>
                    <%                                                                                                                                            
                        //string[] nooptions = { }; 
                        Dictionary<string, string> worker_workeroptions = new Dictionary<string, string>();
                        worker_workeroptions["type"] = "select";
                        worker_workeroptions["valuefield"] = "value";
                        Response.Write(Generic.Functions.buildselection(workers, nooptions, worker_workeroptions));
                    %>
                </select>
            </div>
        </div>
        <div class="form-group">
            <label for="fld_worker_datetimestart" class="control-label col-sm-4">
                Start Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group date">
                    <input id="fld_worker_datetimestart" name="fld_worker_datetimestart" placeholder="eg: 23 Jun 1985" type="text" class="form-control datetime" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label for="fld_worker_datetimeend" class="control-label col-sm-4">
                End Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group date">
                    <input id="fld_worker_datetimeend" name="fld_worker_datetimeend" placeholder="eg: 23 Jun 1985" type="text" class="form-control datetime" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_worker_note">Note</label>
            <div class="col-sm-8">
                <textarea id="fld_worker_note" name="fld_worker_note" class="form-control"></textarea>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_worker_status">Status</label>
            <div class="col-sm-8">
                <select id="fld_worker_status" name="fld_worker_status" class="form-control" required="required">
                    <option value="">--- Please select ---</option>
                    <option value="Confirmed">Confirmed</option>

                </select>
            </div>
        </div>
        <div class="form-group">
            <label for="fld_worker_datetimestartactual" class="control-label col-sm-4">
                Actual Start Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group date">
                    <input id="fld_worker_datetimestartactual" name="fld_worker_datetimestartactual" placeholder="eg: 23 Jun 1985" type="text" class="form-control datetime" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label for="fld_worker_datetimeendactual" class="control-label col-sm-4">
                Actual End Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group date">
                    <input id="fld_worker_datetimeendactual" name="fld_worker_datetimeendactual" placeholder="eg: 23 Jun 1985" type="text" class="form-control datetime" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_worker_worknote">Work Note</label>
            <div class="col-sm-8">
                <textarea id="fld_worker_worknote" name="fld_worker_worknote" class="form-control"></textarea>
            </div>
        </div>
    </div>

    <div id="dialog_person" title="Maintain person" style="display: none" class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_person_person">Youth</label>
            <div class="col-sm-8">
                <input id="fld_person_person" name="fld_person_person" type="text" class="form-control" required />
            </div>
        </div>
        <div class="form-group">
            <label for="fld_person_datetimestart" class="control-label col-sm-4">
                Start Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group date">
                    <input id="fld_person_datetimestart" name="fld_person_datetimestart" placeholder="eg: 23 Jun 1985" type="text" class="form-control datetime" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label for="fld_person_datetimeend" class="control-label col-sm-4">
                End Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group date">
                    <input id="fld_person_datetimeend" name="fld_person_datetimeend" placeholder="eg: 23 Jun 1985" type="text" class="form-control datetime" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_person_note">Note</label>
            <div class="col-sm-8">
                <textarea id="fld_person_note" name="fld_person_note" class="form-control"></textarea>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_person_status">Status</label>
            <div class="col-sm-8">
                <select id="fld_person_status" name="fld_person_status" class="form-control" required="required">
                    <option value="">--- Please select ---</option>
                    <option value="Confirmed">Confirmed</option>

                </select>
            </div>
        </div>
        <div class="form-group">
            <label for="fld_person_datetimestartactual" class="control-label col-sm-4">
                Actual Start Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group date">
                    <input id="fld_person_datetimestartactual" name="fld_person_datetimestartactual" placeholder="eg: 23 Jun 1985" type="text" class="form-control datetime" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label for="fld_person_datetimeendactual" class="control-label col-sm-4">
                Actual End Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group date">
                    <input id="fld_person_datetimeendactual" name="fld_person_datetimeendactual" placeholder="eg: 23 Jun 1985" type="text" class="form-control datetime" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_person_worknote">Work Note</label>
            <div class="col-sm-8">
                <textarea id="fld_person_worknote" name="fld_person_worknote" class="form-control"></textarea>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <form id="form_worker"></form>
    <form id="form_person"></form>
</asp:Content>

