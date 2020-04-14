<%@ Page Title="" Language="C#" MasterPageFile="~/Community/Community.Master" AutoEventWireup="true" CodeBehind="Person.aspx.cs" Inherits="TeOraHouWhanganui.Community.Person" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <style>
        .tabheading {
            display: none
        }

        .tab-content {
            background-color: bisque
        }
    </style>
    <script type="text/javascript">
        var newctr = 0;

        $(document).ready(function () {
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

            $('#search').click(function () {
                window.location.href = 'search.aspx';
            })

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

            $("#fld_location").autocomplete({
                source: "<%: ResolveUrl("~/Community/_Dependencies/data.asmx/postaladdress_autocomplete?option=localphysical")%>",
                minLength: 3,
                select: function (event, ui) {
                    $('#fld_location').val(ui.item.label);
                    $('#fld_PAF').val(ui.item.delivery_point_id);
                    //event.preventDefault();
                },
                change: function (event, ui) {
                    if (!ui.item) {
                        $(event.target).val("");
                        $('#fld_PAF').val("");
                    }
                },
                open: function (event, ui) {
                    if (navigator.userAgent.match(/iPad/)) {
                        // alert(1);
                        $('.autocomplete').off('menufocus hover mouseover');
                    }
                }
            })

            function get_newctr() {
                newctr++;
                return newctr;
            }

            $('.submit').click(function () {
                delim = String.fromCharCode(254);
                /*----------------------------------------------update-----------------------------------------*/
                $('#updatetable > tbody > tr[maint="changed"]').each(function () {
                    tr_id = $(this).attr('id');
                    tr_datetime = $(this).find('td:eq(1)').text();
                    tr_contact = $(this).find('td:eq(2)').attr('contact_ctr');
                    tr_note = $(this).find('td:eq(3)').text();
                    tr_followupaction = $(this).find('td:eq(4)').text();
                    tr_followupdate = $(this).find('td:eq(5)').text();
                    tr_followupdone = $(this).find('td:eq(6)').text();

                    value = tr_datetime + delim + tr_contact + delim + tr_note + delim + tr_followupaction + delim + tr_followupdate + delim + tr_followupdone;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
                $('#updatetable > tbody > tr[maint="deleted"]').each(function () {
                    //don't do if new
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });
            });  //.submit end

            /* ========================================= EDIT UPDATES ===========================================*/
            $(document).on('click', '.updateedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_update").find(':input').val('');
                } else {
                    tr = $(this).closest('tr');
                    $('#fld_update_datetime').val($(tr).find('td').eq(1).text());
                    $('#fld_update_contact').val($(tr).find('td').eq(2).attr('contact_ctr'));
                    $('#fld_update_note').val($(tr).find('td').eq(3).text());
                    $('#fld_update_followupaction').val($(tr).find('td').eq(4).text());
                    $('#fld_update_followupdate').val($(tr).find('td').eq(5).text());
                    $('#fld_update_followupdone').val($(tr).find('td').eq(6).text());
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_update").dialog({
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
                                tr = $('#div_update > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_update > table > tbody > tr:last').before(tr);
                                $(tr).attr('id', 'update_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_update_datetime').val());
                            $(tr).find('td').eq(2).text($('#fld_update_contact option:selected').text());
                            $(tr).find('td').eq(2).attr('contact_ctr', $('#fld_update_contact').val());
                            $(tr).find('td').eq(3).text($('#fld_update_note').val());
                            $(tr).find('td').eq(4).text($('#fld_update_followupaction').val());
                            $(tr).find('td').eq(5).text($('#fld_update_followupdate').val());
                            $(tr).find('td').eq(6).text($('#fld_update_followupdone').val());
                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this update?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'View') {
                    $("#dialog_update").dialog('option', 'buttons', myButtons);
                }
            })
            /* ========================================= EDIT WORKER ROLES ===========================================*/
            $(document).on('click', '.updateedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_update").find(':input').val('');
                } else {
                    tr = $(this).closest('tr');
                    $('#fld_update_datetime').val($(tr).find('td').eq(1).text());
                    $('#fld_update_contact').val($(tr).find('td').eq(2).attr('contact_ctr'));
                    $('#fld_update_note').val($(tr).find('td').eq(3).text());
                    $('#fld_update_followupaction').val($(tr).find('td').eq(4).text());
                    $('#fld_update_followupdate').val($(tr).find('td').eq(5).text());
                    $('#fld_update_followupdone').val($(tr).find('td').eq(6).text());
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_update").dialog({
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
                                tr = $('#div_update > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_update > table > tbody > tr:last').before(tr);
                                $(tr).attr('id', 'update_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_update_datetime').val());
                            $(tr).find('td').eq(2).text($('#fld_update_contact option:selected').text());
                            $(tr).find('td').eq(2).attr('contact_ctr', $('#fld_update_contact').val());
                            $(tr).find('td').eq(3).text($('#fld_update_note').val());
                            $(tr).find('td').eq(4).text($('#fld_update_followupaction').val());
                            $(tr).find('td').eq(5).text($('#fld_update_followupdate').val());
                            $(tr).find('td').eq(6).text($('#fld_update_followupdone').val());
                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this update?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'View') {
                    $("#dialog_update").dialog('option', 'buttons', myButtons);
                }
            })

        }); //document.ready
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="fld_PAF" name="fld_PAF" value="<% =fld_PAF %>" />
    <h1>Person 
    </h1>
    (<%=Username%>)
    <div class="toprighticon">
        <input type="button" id="search" class="btn btn-info" value="Search" />
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Save" />
    </div>
    <div class="form-row">
        <div class="form-group col-md-4">
            <label class="control-label" for="fld_firstname">First name</label>

            <input id="fld_firstname" name="fld_firstname" type="text" class="form-control" value="<%= fld_firstname %>" required="required" />
        </div>
        <div class="form-group col-md-4">
            <label class="control-label" for="fld_lastname">Last name</label>

            <input id="fld_lastname" name="fld_lastname" type="text" class="form-control" value="<%= fld_lastname %>" />

        </div>
        <div class="form-group col-md-4">
            <label class="control-label" for="fld_knownas">Known as</label>

            <input id="fld_knownas" name="fld_knownas" type="text" class="form-control" value="<%= fld_knownas %>" />

        </div>
    </div>

    <div class="form-horizontal">
        <!------------------------------------------ TABS ------------------------------------------------------------>

        <ul class="nav nav-tabs">
            <li class="active"><a data-target="#div_general">General</a></li>
            <%=html_tab %>
        </ul>


        <div class="tab-content">
            <!-- ================================= GENERAL TAB ===================================  -->
            <div id="div_general" class="tab-pane fade in active">
                <p>&nbsp;</p>
                <h3 class="tabheading">General</h3>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="fld_gender">Gender</label>
                    <div class="col-sm-2">
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
                    <label class="control-label col-sm-2" for="fld_age">Age</label>
                    <div class="col-sm-2">
                        <input id="pb-5 fld_age" name="fld_age" type="text" class="form-control" value="<%= fld_age %>" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="fld_address">Address</label>
                    <div class="col-sm-6">
                        <input id="fld_location" name="fld_location" type="text" class="form-control" value="<%= fld_location %>" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="fld_phone">Phone</label>
                    <div class="col-sm-2">
                        <input id="fld_phone" name="fld_phone" type="text" class="form-control" value="<%= fld_phone %>" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="fld_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_note" name="fld_note" class="form-control"><%= fld_note %></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="fld_contact">Contact</label>
                    <div class="col-sm-2">
                        <select id="fld_contact" name="fld_contact" class="form-control">
                            <option value="">--- Please select ---</option>
                            <option value=""></option>
                            <% 
                                Dictionary<string, string> contactoptions = new Dictionary<string, string>();
                                contactoptions["type"] = "select";
                                contactoptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(contacts, fld_contact_ctr, contactoptions));
                            %>
                        </select>
                    </div>
                </div>
                <p>&nbsp;</p>
            </div>

            <!-- ================================= UPDATES TAB ===================================  -->
            <div id="div_update" class="tab-pane fade in">
                <h3 class="tabheading">Updates</h3>
                <table id="updatetable" class="table" style="width: 100%">
                    <%= html_updates %>
                </table>
            </div>

            <!-- ================================= UPDATES DIALOG ===================================  -->
            <div id="dialog_update" title="Maintain Updates" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_update_contact">Done by</label>
                    <div class="col-sm-8">
                        <select id="fld_update_contact" name="fld_update_contact" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <option value=""></option>
                            <% 
                                string[] nooptions = { };
                                contactoptions["type"] = "select";
                                contactoptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(contacts, nooptions, contactoptions));
                            %>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label for="fld_update_datetime" class="control-label col-sm-4">
                        Start Date/Time
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group date">
                            <input id="fld_update_datetime" name="fld_update_datetime" placeholder="eg: 23 Jun 1985" type="text" class="form-control datetime" required="required" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_update_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_update_note" name="fld_update_note" class="form-control" required="required"></textarea>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_update_followupaction">Followup Action</label>
                    <div class="col-sm-8">
                        <textarea id="fld_update_followupaction" name="fld_update_followupaction" class="form-control"></textarea>
                    </div>
                </div>

                <div class="form-group">
                    <label for="fld_update_followupdate" class="control-label col-sm-4">
                        Followup Date
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group date">
                            <input id="fld_update_followupdate" name="fld_update_followupdate" placeholder="eg: 23 Jun 1985" type="text" class="form-control standarddate" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label for="fld_update_followupdone" class="control-label col-sm-4">
                        Followup Done
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group date">
                            <input id="fld_update_followupdone" name="fld_update_followupdone" placeholder="eg: 23 Jun 1985" type="text" class="form-control datetime" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- ================================= END OF TABS ===================================  -->
    </div>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <form id="form2"></form>
</asp:Content>
