<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Program.aspx.cs" Inherits="TeOraHouWhanganui.Private.Administration.Program" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            $('.standarddate').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true
                //,maxDate: moment().add(-1, 'year')
            });

            /* ========================================= EDIT activityS ===========================================*/
            $(document).on('click', '.activityedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_activity").find(':input').val('');
                } else {
                    tr = $(this).closest('tr');
                    //$('#fld_activity_startdatetime').val($(tr).find('td').eq(1).text());
                    //$('#fld_activity_enddatetime').val($(tr).find('td').eq(2).text());
                    //$('#fld_activity_narrative').val($(tr).find('td').eq(3).html());

                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_activity").dialog({
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
                                tr = $('#div_activity > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_activity > table > tbody > tr:last').before(tr);
                                $(tr).attr('id', 'activity_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_activity_startdatetime').val());
                            $(tr).find('td').eq(2).text($('#fld_activity_enddatetime').val());
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
                            totalactivity();
                            alert('To do: Delete in database');
                            $(this).dialog("close");
                        }
                    }
                }


                $("#dialog_activity").dialog('option', 'buttons', myButtons);
            })



        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none"></div>
    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <h1>Program Maintenance
    </h1>

    <div class="form-group">
        <label class="control-label col-sm-4" for="fld_program">Program</label>
        <div class="col-sm-3">
            <select id="fld_program" name="fld_program" class="form-control">
                <%= fld_program %>
            </select>
        </div>
    </div>


    <!-- ================================= activity DIALOG ===================================  -->
    <div id="dialog_activity" title="Maintain activitys" style="display: none" class="form-horizontal">
        <div class="form-group">
            <label for="fld_activity_startdatetime" class="control-label col-sm-4">
                Start Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group datetime">
                    <input id="fld_activity_startdatetime" name="fld_activity_startdatetime" placeholder="eg: 23 Jun 1985" type="text" class="form-control" required="required" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <label for="fld_activity_enddatetime" class="control-label col-sm-4">
                End Date/Time
            </label>
            <div class="col-sm-8">
                <div class="input-group datetime">
                    <input id="fld_activity_enddatetime" name="fld_activity_enddatetime" placeholder="eg: 23 Jun 1985" type="text" class="form-control" required="required" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_activity_narrative">Narrative</label>
            <div class="col-sm-8">
                <textarea id="fld_activity_narrative" name="fld_activity_narrative" class="form-control"></textarea>
            </div>
        </div>
    </div>


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
</asp:Content>
