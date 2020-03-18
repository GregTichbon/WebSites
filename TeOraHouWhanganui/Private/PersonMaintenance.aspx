<%@ Page Title="TOHW Person Maintenance" Language="C#" MasterPageFile="~/Private/Main.Master" AutoEventWireup="true" CodeBehind="PersonMaintenance.aspx.cs" Inherits="TeOraHouWhanganui.Private.PersonMaintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>

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

            $(".uicheckbox").checkboxradio({
                icon: false
            });

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
                format: 'D MMM YYYY hh:mm',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true
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
                /*
                if (thismonth >= 9) {
                    thisyear = thisyear + 1;
                }
                var jan1 = moment([thisyear, 0, 1]);
                */

                //$("#span_age").text('Age: ' + years + ' years, ' + jan1.diff(e, 'years') + ' years at 1 Jan ' + thisyear);
                return (years);
            }

            function get_newctr() {
                newctr++;
                return newctr;
            }

            /* ========================================= EDIT ENCOUNTERS ===========================================*/
            $(document).on('click', '.encounteredit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_encounter").find(':input').val('');
                } else {
                    tr = $(this).closest('tr');
                    $('#fld_encounter_startdatetime').val($(tr).find('td').eq(1).text());
                    $('#fld_encounter_enddatetime').val($(tr).find('td').eq(2).text());
                    $('#fld_encounter_narrative').val($(tr).find('td').eq(3).html());
                    //$('#fld_encounter_event').val($(tr).find('td').eq(4).attr('event_id'));
                    //$('#fld_encounter_amount').val($(tr).find('td').eq(5).text());
                    //$('#fld_encounter_note').val($(tr).find('td').eq(6).text());
                    //$('#fld_encounter_banked').val($(tr).find('td').eq(7).text());
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
                    /*
                    ,open: function (type, data) {
                        $(this).appendTo($('form')); // reinsert the dialog to the form       
                    }*/
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
                                $('#div_encounter > table > tbody > tr:last').before(tr);
                                $(tr).attr('id', 'encounter_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_encounter_startdatetime').val());
                            $(tr).find('td').eq(2).text($('#fld_encounter_enddatetime').val());
                            $(tr).find('td').eq(3).html(tinyMCE.activeEditor.getContent());
                           // $(tr).find('td').eq(4).text($('#fld_encounter_event option:selected').text());
                            //$(tr).find('td').eq(4).attr('event_id', $('#fld_encounter_event').val());
                            //$(tr).find('td').eq(5).text(formatcurrency($('#fld_encounter_amount').val()));
                           // $(tr).find('td').eq(6).text($('#fld_encounter_note').val());
                            //totalencounter();
                            //$(tr).find('td').eq(7).text($('#fld_encounter_banked').val());
                            //alert("Database will be updated when record submited");
                            tinymce.remove('.tinymce');;
                           $(this).dialog("close");
                        }
                    }
                }


                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this transaction?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            //$(tr).remove
                            totalencounter();
                            alert('To do: Delete in database');
                            tinymce.remove('.tinymce');;
                           $(this).dialog("close");
                        }
                    }
                }


                $("#dialog_encounter").dialog('option', 'buttons', myButtons);
            })





        }); //document.ready
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none"></div>
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
                <label class="control-label col-md-6" for="firstname">First name</label>
                <div class="col-md-6">
                    <input id="firstname" name="firstname" type="text" class="form-control" value="<%:firstname%>" maxlength="20" required="required" />
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-md-6" for="knownas">Known as</label>
                <div class="col-md-6">
                    <input id="knownas" name="knownas" type="text" class="form-control" value="<%:knownas%>" maxlength="20" />
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-md-6" for="lastname">Surname</label>
                <div class="col-md-6">
                    <input id="surname" name="surname" type="text" class="form-control" value="<%:surname%>" maxlength="20" />
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

    <ul class="nav nav-tabs">
        <li class="active"><a data-target="#div_general">General</a></li>
        <%=html_tab %>
    </ul>


    <div class="tab-content">
        <!-- ================================= GENERAL TAB ===================================  -->
        <div id="div_general" class="tab-pane fade in active">
            <h3 class="tabheading">General</h3>
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="birthdate" class="control-label col-sm-4">
                        Date of birth
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group date birthdate" data-showage="span_age">
                            <input id="birthdate" name="birthdate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" value="<%: birthdate %>" />

                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>

                            <span id="span_age" class="input-group-addon"></span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="medical">Medical notes</label>
                    <div class="col-sm-8">
                        <textarea id="medical" name="medical" class="form-control"><%: medical %></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="dietry">Dietary notes</label>
                    <div class="col-sm-8">
                        <textarea id="dietary" name="dietary" class="form-control"><%: dietary %></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="notes">General notes</label>
                    <div class="col-sm-8">
                        <textarea id="notes" name="notes" class="form-control"><%: notes %></textarea>
                    </div>
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

        <p></p>
        <p></p>
        <!-- ================================= ENCOUNTERS DIALOG ===================================  -->
        <div id="dialog_encounter" title="Maintain encounters" style="display: none" class="form-horizontal">

            <div class="form-group">
                <label for="fld_encounter_startdatetime" class="control-label col-sm-4">
                    Start Date/Time
                </label>
                <div class="col-sm-8">
                    <div class="input-group datetime">
                        <input id="fld_encounter_startdatetime" name="fld_encounter_startdatetime" placeholder="eg: 23 Jun 1985" type="text" class="form-control" required="required" />
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
                        <input id="fld_encounter_enddatetime" name="fld_encounter_enddatetime" placeholder="eg: 23 Jun 1985" type="text" class="form-control" required="required" />
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
                    <fieldset>
                        <label for="checkbox-1">Greg T</label>
                        <input class="uicheckbox" type="checkbox" name="fld_encounter_worker" id="checkbox-1">
                        <label for="checkbox-2">Keegan E</label>
                        <input class="uicheckbox" type="checkbox" checked name="fld_encounter_worker" id="checkbox-2">
                        <label for="checkbox-3">Mary T</label>
                        <input class="uicheckbox" type="checkbox" name="fld_encounter_worker" id="checkbox-3">
                        <label for="checkbox-4">Jordie H</label>
                        <input class="uicheckbox" type="checkbox" name="fld_encounter_worker" id="checkbox-4">
                        <label for="checkbox-5">Person</label>
                        <input class="uicheckbox" type="checkbox" name="fld_encounter_worker" id="checkbox-5">
                        <label for="checkbox-6">Person</label>
                        <input class="uicheckbox" type="checkbox" name="fld_encounter_worker" id="checkbox-6">
                        <label for="checkbox-7">Person</label>
                        <input class="uicheckbox" type="checkbox" name="fld_encounter_worker" id="checkbox-7">
                        <label for="checkbox-8">Person</label>
                        <input class="uicheckbox" type="checkbox" name="fld_encounter_worker" id="checkbox-8">
                        <label for="checkbox-9">Person</label>
                        <input class="uicheckbox" type="checkbox" name="fld_encounter_worker" id="checkbox-9">
                        <label for="checkbox-10">Person</label>
                        <input class="uicheckbox" type="checkbox" name="fld_encounter_worker" id="checkbox-10">
                        <label for="checkbox-11">PersonH</label>
                        <input class="uicheckbox" type="checkbox" name="fld_encounter_worker" id="checkbox-11">
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <!-- ================================= END OF TABS ===================================  -->

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
    <form id="form2"></form>
</asp:Content>

