<%@ Page Title="TOHW Person Maintenance" Language="C#" MasterPageFile="~/Private/Main.Master" AutoEventWireup="true" CodeBehind="PersonMaintenance.aspx.cs" Inherits="TeOraHouWhanganui.Private.PersonMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script src='//cdn.tinymce.com/4/tinymce.min.js'></script>

    <script type="text/javascript">
        var newctr = 0;

        $(document).ready(function () {
            //Generic.Functions.googleanalyticstracking()%>
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
                window.location.href = "<%=ResolveUrl("~/private/personsearch.aspx")%>";
            });

            $("#form1").validate();

            $(".nav-tabs a").click(function () {
                $(this).tab('show');
            });

            $('.nav-tabs a').on('shown.bs.tab', function (event) {
                var x = $(event.target).text();         // active tab
                var y = $(event.relatedTarget).text();  // previous tab
                $(".act span").text(x);
                $(".prev span").text(y);
            });

            //$(".uicheckbox").checkboxradio({
            //    icon: false
            //});

            $('.standarddate').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true
                //,maxDate: moment().add(-1, 'year')
            });

            $('.datetime').datetimepicker({
                format: 'D MMM YYYY HH:mm',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true,
                  
                //,maxDate: moment().add(-1, 'year')
            });

            $('.birthdate').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: false,
                sideBySide: true,
                viewMode: 'years'
                //,maxDate: moment().add(-1, 'year')
            });

            $(".birthdate").on("dp.change", function (e) {
                showage_id = $(this).data('showage');
                asat = $(this).data('asat');
                years = calculateage(e.date, asat);
                if (showage_id != "") {
                    $('#' + showage_id).text('Age: ' + years + ' years')
                }
            });

            var e = moment($(".birthdate").find("input").val());
            showage_id = $(this).data('showage');
            asat = $(this).data('asat');
            years = calculateage(e.date, asat);
            if (showage_id != "") {
                $('#' + showage_id).text('Age: ' + years + ' years')
            }

            function calculateage(e, asat) {   //MOVE THIS TO MAIN.JS
                if (moment().diff(e, 'seconds') < 0) {
                    e.date = moment(e).subtract(100, 'years');
                    //$("#tb_birthdate").val(moment(e).format('D MMM YYYY'));
                }
                var years = moment().diff(e, 'years');
                thisyear = moment().year();
                thismonth = moment().month() + 1;


                //$("#span_age").text('Age: ' + years + ' years, ' + jan1.diff(e, 'years') + ' years at 1 Jan ' + thisyear);
                return (years);
            }

            function get_newctr() {
                newctr++;
                return newctr;
            }

            $("#fld_assigned_person").autocomplete({
                source: "<%: ResolveUrl("~/_Dependencies/data.asmx/Person_name_autocomplete")%>",
                minLength: 2 //,
                , appendTo: "#dialog_assigned"
                , select: function (event, ui) {
                    event.preventDefault();
                    $('#fld_assigned_person').val(ui.item.label);
                    $('#fld_assigned_person').attr('person_ctr', ui.item.value);
                }
            });

            $('.submit').click(function () { //Started creating functions so that I can group code together - see update_enrolement() - not yet tested
                delim = String.fromCharCode(254);
                
                /*----------------------------------------------WORKER ROLE-----------------------------------------*/
                $('#workerroletable > tbody > tr[maint="changed"]').each(function () {
                    tr_id = $(this).attr('id');
                    tr_role = $(this).find('td:eq(1)').attr('role_ctr');
                    tr_startdate = $(this).find('td:eq(2)').text();
                    tr_enddate = $(this).find('td:eq(3)').text();
                    tr_note = $(this).find('td:eq(4)').text();

                    value = tr_role + delim + tr_startdate + delim + tr_enddate + delim + tr_note;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
                /*
                $('#workerroletable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });
                */
                /*----------------------------------------------ASSIGNMENT-----------------------------------------*/
                $('#assignedtable > tbody > tr[maint="changed"]').each(function () {
                    tr_id = $(this).attr('id');
                    tr_type = $(this).find('td:eq(1)').text();
                    tr_person = $(this).find('td:eq(2)').attr('person_ctr');
                    tr_startdate = $(this).find('td:eq(3)').text();
                    tr_enddate = $(this).find('td:eq(4)').text();
                    tr_note = $(this).find('td:eq(5)').text();
                    tr_level = $(this).find('td:eq(6)').attr('accesslevel');

                    value = tr_type + delim + tr_person + delim + tr_startdate + delim + tr_enddate + delim + tr_note + delim + tr_level;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
                /*
                $('#assignedtable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });
                */
                /*----------------------------------------------ENCOUNTERS-----------------------------------------*/
                $('#encountertable > tbody > tr[maint="changed"]').each(function () {

                    tr_id = $(this).attr('id');
                    tr_startdatetime = $(this).find('td:eq(1)').text();
                    tr_enddatetime = $(this).find('td:eq(2)').text();
                    tr_narrative = $(this).find('td:eq(3)').text();
                    tr_workers = $(this).find('td:eq(4)').attr('workerid');
                    tr_level = $(this).find('td:eq(5)').attr('encounteraccesslevel');

                    value = tr_startdatetime + delim + tr_enddatetime + delim + tr_narrative + delim + tr_workers + delim + tr_level;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
                /*
                $('#encountertable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });
                */

                update_enrolement();

            });  //.submit end

            /* ========================================= EDIT ENCOUNTERS ===========================================*/
            $(document).on('click', '.encounteredit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_encounter").find(':input').val('');
                    workers = '';
                } else {
                    tr = $(this).closest('tr');
                    $('#fld_encounter_startdatetime').val($(tr).find('td').eq(1).text());
                    $('#fld_encounter_enddatetime').val($(tr).find('td').eq(2).text());
                    $('#fld_encounter_narrative').val($(tr).find('td').eq(3).html());
                    workers = $(tr).find('td').eq(4).attr("workerid");
                    $('#fld_encounter_level').val($(tr).find('td').eq(5).attr('encounteraccesslevel'));
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_encounter").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true

                    , open: function (type, data) {
                        //$(this).appendTo($('form')); // reinsert the dialog to the form   
                       
                        var myarr = [];
                        var workersarray = workers.toString().split('|');
                        $.each(workersarray, function (index, value) {
                            myarr.push(value);
                        });
                         
                        $('#fld_encounter_worker').select2();
                        $('#fld_encounter_worker').val(myarr);
                        $("#fld_encounter_worker").trigger("change");

                    }
                    /*
                    , close: function (event, ui) {
                        //$('#fld_encounter_worker').select2('destroy');
                    }
                    */
                    , appendTo: "#form2"
                });

                tinymce.init({
                    selector: '.tinymce',
                    menubar: "tools edit format view",
                    paste_as_text: true,
                    plugins: [
                        "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
                        "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                        "save table contextmenu directionality emoticons template paste textcolor"
                    ]
                });

                var myButtons = {
                    "Cancel": function () {
                        tinymce.remove('.tinymce');;
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#div_encounter > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_encounter > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'encounter_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_encounter_startdatetime').val());
                            $(tr).find('td').eq(2).text($('#fld_encounter_enddatetime').val());
                            $(tr).find('td').eq(3).html(tinyMCE.activeEditor.getContent());
                            var workerid = "";
                            var workertext = "";
                            var delim1 = "";
                            var delim2 = "";
                            $('#fld_encounter_worker > option:selected').each(function () {
                                workerid += delim1 + $(this).val();
                                workertext += delim2 + $(this).text();
                                delim1 = '|';
                                delim2 = '<br />';
                            });
                            $(tr).find('td').eq(4).html(workertext);
                            $(tr).find('td').eq(4).attr("workerid", workerid);
                            $(tr).find('td').eq(5).text($('#fld_encounter_level option:selected').text());
                            $(tr).find('td').eq(5).attr('encounteraccesslevel', $('#fld_encounter_level').val());

                            tinymce.remove('.tinymce');;
                            $(this).dialog("close");
                        }
                    }
                }


                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this encounter?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            tinymce.remove('.tinymce');;
                            $(this).dialog("close");
                        }
                    }
                }

                $("#dialog_encounter").dialog('option', 'buttons', myButtons);
            })

            /* ========================================= EDIT WORKER ROLES ===========================================*/
            $(document).on('click', '.workerroleedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_workerrole").find(':input').val('');
                    $("#dialog_workerrole").find(':input').prop("disabled", false);
                } else {
                    tr = $(this).closest('tr');

                    $('#fld_workerrole_role').val($(tr).find('td').eq(1).attr('role_ctr'));
                    $('#fld_workerrole_startdate').val($(tr).find('td').eq(2).text());
                    $('#fld_workerrole_enddate').val($(tr).find('td').eq(3).text());
                    $('#fld_workerrole_note').val($(tr).find('td').eq(4).html());

                    $('#fld_workerrole_role').prop("disabled", true);
                    $('#fld_workerrole_startdate').prop("disabled", true);
                    if ($('#fld_workerrole_enddate').val() != "") {
                        $('#fld_workerrole_enddate').prop("disabled", true);
                        $('#fld_workerrole_note').prop("disabled", true);
                    }
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_workerrole").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true
                    /*
                    ,open: function (type, data) {
                        $(this).appendTo($('form')); // reinsert the dialog to the form       
                    }*/
                    , appendTo: "#form2"
                });


                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#div_workerrole > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_workerrole > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'workerrole_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');

                            $(tr).find('td').eq(1).text($('#fld_workerrole_role option:selected').text());
                            $(tr).find('td').eq(1).attr('role_ctr', $('#fld_workerrole_role').val());
                            $(tr).find('td').eq(2).text($('#fld_workerrole_startdate').val());
                            $(tr).find('td').eq(3).text($('#fld_workerrole_enddate').val());
                            $(tr).find('td').eq(4).text($('#fld_workerrole_note').val());
                            $(this).dialog("close");
                        }
                    }
                }

                /*
                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this workerrole?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }
                */

                $("#dialog_workerrole").dialog('option', 'buttons', myButtons);
            })

            /* ========================================= EDIT ASSIGNMENTS ===========================================*/
            $(document).on('click', '.assignededit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_assigned").find(':input').val('');
                    $("#dialog_assigned").find(':input').prop("disabled", false);
                } else {
                    tr = $(this).closest('tr');
                    $('#fld_assigned_type').val($(tr).find('td').eq(1).text());
                    $('#fld_assigned_person').val($(tr).find('td').eq(2).text());
                    $('#fld_assigned_person').attr('person_ctr', $(tr).find('td').eq(2).attr('person_ctr'));
                    $('#fld_assigned_startdate').val($(tr).find('td').eq(3).text());
                    $('#fld_assigned_enddate').val($(tr).find('td').eq(4).text());
                    $('#fld_assigned_note').val($(tr).find('td').eq(5).html());
                    $('#fld_assigned_level').val($(tr).find('td').eq(6).attr('accesslevel'));

                    $('#fld_assigned_type').prop("disabled", true);
                    $('#fld_assigned_person').prop("disabled", true);
                    $('#fld_assigned_startdate').prop("disabled", true);
                    if ($('#fld_assigned_enddate').val() != "") {
                        $('#fld_assigned_enddate').prop("disabled", true);
                        $('#fld_assigned_note').prop("disabled", true);
                    }
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_assigned").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true
                    /*
                    ,open: function (type, data) {
                        $(this).appendTo($('form')); // reinsert the dialog to the form       
                    }*/
                    , appendTo: "#form2"
                });


                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#div_assigned > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_assigned > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'assigned_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_assigned_type option:selected').text());

                            $(tr).find('td').eq(2).text($('#fld_assigned_person').val());
                            $(tr).find('td').eq(2).attr('person_ctr', $('#fld_assigned_person').attr('person_ctr'));

                            $(tr).find('td').eq(3).text($('#fld_assigned_startdate').val());
                            $(tr).find('td').eq(4).text($('#fld_assigned_enddate').val());
                            $(tr).find('td').eq(5).text($('#fld_assigned_note').val());

                            $(tr).find('td').eq(6).text($('#fld_assigned_level option:selected').text());
                            $(tr).find('td').eq(6).attr('accesslevel', $('#fld_assigned_level').val());

                            alert($('#fld_assigned_level').val());

                            $(this).dialog("close");
                        }
                    }
                }

                /*
                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this assigned?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            //$(tr).remove
                            totalassigned();
                            alert('To do: Delete in database');
                            $(this).dialog("close");
                        }
                    }
                }
                */

                $("#dialog_assigned").dialog('option', 'buttons', myButtons);
            })

            /* ========================================= EDIT ENROLMENTS ===========================================*/
            $(document).on('click', '.enrolmentedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_enrolment").find(':input').val('');
                } else {
                    tr = $(this).closest('tr');
                    $('#fld_enrolment_program').val($(tr).find('td').eq(1).text());
                    $('#fld_enrolment_status').val($(tr).find('td').eq(2).text());
                    $('#fld_enrolment_notes').val($(tr).find('td').eq(3).html());
                    $('#fld_enrolment_alwayspickup').val($(tr).find('td').eq(5).data('enrolmentaccesslevel'));
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_enrolment").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true,
                    appendTo: "#form2"
                });

                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#div_enrolment > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_enrolment > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'enrolment_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_enrolment_program').val());
                            $(tr).find('td').eq(2).text($('#fld_enrolment_status').val());
                            $(tr).find('td').eq(3).text($('#fld_enrolment_notes').val());
                            $(tr).find('td').eq(4).text($('#fld_enrolment_alwayspickup').val());

                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this enrolment?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }

                $("#dialog_enrolment").dialog('option', 'buttons', myButtons);
            })

            function update_enrolement() {
                /*----------------------------------------------ENROLMENT-----------------------------------------*/
                $('#enrolementtable > tbody > tr[maint="changed"]').each(function () {

                    tr_id = $(this).attr('id');
                    tr_program = $(this).find('td:eq(1)').text();
                    tr_status = $(this).find('td:eq(2)').text();
                    tr_notes = $(this).find('td:eq(3)').text();
                    tr_alwayspickup = $(this).find('td:eq(4)').attr('workerid');

                    value = tr_program + delim + tr_status + delim + tr_notes + delim + tr_alwayspickup;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
                /*
                $('#enrolementtable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });
                */
            }

        }); //document.ready
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%=username %>
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p>When adding a new record, enter the name fields and then click on the <span class="btn btn-info">Submit</span> button.  Tabs will then be shown to allow you to add more data.</p>
        <p>Making changes to the data under the various tabs will not be actioned until the record is submitted. </p>
        <p>Upload photo is still to be done.</p>
        <p><b>Encounters: </b> There are different levels of access given to Te Ora Hou workes.  Each worker is granted a level of between 1 and 4 for each person in the database.  They will only see the encounter notes where their access level is equal to or less than the level assigned to the encounter.  They may additionally see encounter notes where they have been recorded as a worker in that record.</p>
        <p>The levels are</p><ul><li>1. Confidential</li><li>2. Highly sensitive</li><li>3. Sensitive</li><li>4. General</li></ul>
        <p>Everyone has access to encounter notes at level 4.</p>
    </div>
    <div class="toprighticon">
        <input type="button" id="search" class="btn btn-info" value="Search" />
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <h1>Person Maintenance
    </h1>



    <div class="form-horizontal row">
        <div class="col-md-8">
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_firstname">First name</label>
                <div class="col-md-6">
                    <input id="fld_firstname" name="fld_firstname" type="text" class="form-control" value="<%:fld_firstname%>" maxlength="20" required="required" />
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_knownas">Known as</label>
                <div class="col-md-6">
                    <input id="fld_knownas" name="fld_knownas" type="text" class="form-control" value="<%:fld_knownas%>" maxlength="20" />
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_surname">Surname</label>
                <div class="col-md-6">
                    <input id="fld_surname" name="fld_surname" type="text" class="form-control" value="<%:fld_surname%>" maxlength="20" />
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <img id="img_photo" alt="" src="Images/<%: person_ctr %>.jpg" style="width: 200px" /><br />
            <a id="getphoto">Upload Photo</a>
        </div>
    </div>

    <div id="dialog_getphoto" title="Upload Photo" style="display: none">
        <div class="imagecontainer">
            <input type="file" id="fileInput" class="btn btn-info" accept="image/*" />
            <canvas id="canvas" style="display: none">Your browser does not support the HTML5 canvas element.
            </canvas>
            <br />
            <input type="button" id="btn_Crop" class="btn btn-info" value="Crop" style="display: none" />
            <div id="preview"></div>
            <div id="result"></div>
        </div>
    </div>
    <!------------------------------------------ TABS ------------------------------------------------------------>

    <div class="form-horizontal">
        <ul class="nav nav-tabs">
            <li class="active"><a data-target="#div_general">General</a></li>
            <%=html_tab %>
        </ul>

        <div class="tab-content">
            <!-- ================================= GENERAL TAB ===================================  -->
            <div id="div_general" class="tab-pane fade in active">
                <h3 class="tabheading">General</h3>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_gender">Gender</label>
                    <div class="col-sm-4">
                        <select id="fld_gender" name="fld_gender" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <% 
                                Dictionary<string, string> genderoptions = new Dictionary<string, string>();
                                genderoptions["type"] = "select";
                                genderoptions["valuefield"] = "label";
                                Response.Write(Generic.Functions.buildselection(genders, fld_gender, genderoptions));
                            %>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label for="fld_birthdate" class="control-label col-sm-4">
                        Date of birth
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group birthdate" data-showage="span_age">
                            <input id="fld_birthdate" name="fld_birthdate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" value="<%: fld_birthdate %>" />

                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>

                            <span id="span_age" class="input-group-addon"></span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_medical">Medical notes</label>
                    <div class="col-sm-8">
                        <textarea id="fld_medical" name="fld_medical" class="form-control"><%: fld_medical %></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_dietry">Dietary notes</label>
                    <div class="col-sm-8">
                        <textarea id="fld_dietary" name="fld_dietary" class="form-control"><%: fld_dietary %></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_notes">General notes</label>
                    <div class="col-sm-8">
                        <textarea id="fld_notes" name="fld_notes" class="form-control"><%: fld_notes %></textarea>
                    </div>
                </div>
            </div>

            <!-- ================================= ENCOUNTERS TAB ===================================  -->
            <div id="div_encounter" class="tab-pane fade in">
                <h3 class="tabheading">Encounters</h3>
                <table id="encountertable" class="table" style="width: 100%">
                    <%= html_encounter %>
                </table>
            </div>

            <!-- ================================= ENCOUNTERS DIALOG ===================================  -->
            <div id="dialog_encounter" title="Maintain encounters" style="display: none" class="form-horizontal">

                <div class="form-group">
                    <label for="fld_encounter_startdatetime" class="control-label col-sm-4">
                        Start Date/Time
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group datetime">
                            <input id="fld_encounter_startdatetime" name="fld_encounter_startdatetime" placeholder="eg: 23 Jun 1985 21:00" type="text" class="form-control" required="required" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label for="fld_encounter_enddatetime" class="control-label col-sm-4">
                        End Date/Time
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group datetime">
                            <input id="fld_encounter_enddatetime" name="fld_encounter_enddatetime" placeholder="eg: 23 Jun 1985 22:00" type="text" class="form-control" required="required" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_encounter_narrative">Narrative</label>
                    <div class="col-sm-8">
                        <textarea id="fld_encounter_narrative" name="fld_encounter_narrative" class="form-control tinymce"></textarea>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_encounter_worker">Worker(s)</label>
                    <div class="col-sm-8">
                        <select id="fld_encounter_worker" name="fld_encounter_worker" class="form-control" required="required" multiple="multiple">
                            <%                                                                                                                                            
                                string[] nooptions = { }; 
                                Dictionary<string, string> encounter_workeroptions = new Dictionary<string, string>();
                                encounter_workeroptions["type"] = "select";
                                encounter_workeroptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(workers, nooptions, encounter_workeroptions));
                            %>
                        </select>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_encounter_level">Access level</label>
                    <div class="col-sm-8">
                        <select id="fld_encounter_level" name="fld_encounter_level" class="form-control" required="required">
                            <%                                                                                                                                            
                                string[] noleveloptions = { };  
                                Dictionary<string, string> encounter_leveloptions = new Dictionary<string, string>();
                                encounter_leveloptions["type"] = "select";
                                encounter_leveloptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(encounterAccessLevels, noleveloptions, encounter_leveloptions));
                            %>
                        </select>
                    </div>
                </div>
            </div>

            <!-- ================================= WORKER ROLE TAB ===================================  -->
            <div id="div_workerrole" class="tab-pane fade in">
                <h3 class="tabheading">Worker Roles</h3>
                <span>Record roles and periods that this person was designated as a worker.</span>
                <table id="workerroletable" class="table" style="width: 100%">
                    <%= html_workerroles %>
                </table>
            </div>

            <!-- ================================= WORKER ROLE DIALOG ===================================  -->
            <div id="dialog_workerrole" title="Maintain worker roles" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_workerrole_role">Role</label>
                    <div class="col-sm-8">
                        <select id="fld_workerrole_role" name="fld_workerrole_role" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                //string[] nooptions = { }; //temp
                                Dictionary<string, string> workerroleoptions = new Dictionary<string, string>();
                                workerroleoptions["type"] = "select";
                                workerroleoptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(workerRoles, nooptions, workerroleoptions));
                            %>
                        </select>
                    </div>
                </div>

                <div class="form-group">
                    <label for="fld_workerrole_startdate" class="control-label col-sm-4">
                        Start Date 
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_workerrole_startdate" name="fld_workerrole_startdate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" required="required" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label for="fld_workerrole_enddate" class="control-label col-sm-4">
                        End Date 
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_workerrole_enddate" name="fld_workerrole_enddate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_workerrole_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_workerrole_note" name="fld_workerrole_note" class="form-control tinymce"></textarea>
                    </div>
                </div>
            </div>

            <!-- ================================= ASSIGNED TAB ===================================  -->
            <div id="div_assigned" class="tab-pane fade in">
                <h3 class="tabheading">Assigned Workers</h3>
                <span>Record workers that are assigned to this person.</span>
            <table id="assignedtable" class="table" style="width: 100%">
                <%= html_assigned %>
            </table>
            </div>

            <!-- ================================= ASSIGNED DIALOG ===================================  -->
            <div id="dialog_assigned" title="Maintain Assigned Workers" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_assigned_type">Type</label>
                    <div class="col-sm-8">
                        <select id="fld_assigned_type" name="fld_assigned_type" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                Dictionary<string, string> assignmenttypeoptions = new Dictionary<string, string>();
                                assignmenttypeoptions["type"] = "select";
                                Response.Write(Generic.Functions.buildselection(assignmenttypes, nooptions, assignmenttypeoptions));
                            %>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_assigned_person">Person</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_assigned_person" name="fld_assigned_person" class="form-control" required="required" />
                    </div>
                </div>


                <div class="form-group">
                    <label for="fld_assigned_startdate" class="control-label col-sm-4">
                        Start Date
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_assigned_startdate" name="fld_assigned_startdate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" required="required" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label for="fld_assigned_enddate" class="control-label col-sm-4">
                        End Date 
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_assigned_enddate" name="fld_assigned_enddate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_assigned_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_assigned_note" name="fld_assigned_note" class="form-control"></textarea>
                    </div>
                </div>

                <div class="form-group" <%=show_assigned_level %>>
                    <label class="control-label col-sm-4" for="fld_assigned_level">Level</label>
                    <div class="col-sm-8">
                        <select id="fld_assigned_level" name="fld_assigned_level" class="form-control" required="required">
                            <%                                                                                                                                            
                                //string[] noleveloptions = { };  
                                //Dictionary<string, string> encounter_leveloptions = new Dictionary<string, string>();
                                //encounter_leveloptions["type"] = "select";
                                //encounter_leveloptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(encounterAccessLevels, noleveloptions, encounter_leveloptions));
                            %>
                        </select>
                    </div>
                </div>
            </div>
        </div>
        <!-- ================================= END OF TABS ===================================  -->
    </div>
    <p></p>
        <p></p>
        <div class="form-group">
            <div class="col-sm-4">
            </div>
            <div class="col-sm-8">
                <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />
            </div>
        </div>
        <br />
        <br />
        <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <form id="form2">
    </form>
</asp:Content>

