<%@ Page Title="" Language="C#" MasterPageFile="~/Cemeteries.Master" AutoEventWireup="true" CodeBehind="Entry.aspx.cs" Inherits="Cemeteries.Administration.Sexton.Entry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .input-sm {
            padding: 3px 3px;
            font-size: 10px;
        }
    </style>
    <script type="text/javascript" src="<%: ResolveUrl("~/_Dependencies/Scripts/CascadingDropDown/jquery.cascadingdropdown.min.js")%>"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/Scripts/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/Scripts/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>

    <script type="text/javascript">
        var datamode = 'GIS';
        var mydialog;
        var newctr = 0;


        var transactiontypes = <%= jsobj_transactiontypes%>;

        $(document).ready(function () {

            var dialogwidth = $(window).width() * .9;
            if (dialogwidth > 810) {
                dialogwidth = 810;
            }
            var dialogheight = $(window).height() * .9;
          
            $('.btn_submit').click(function (e) {
                $('#tbl_transactions tr').each(function () {
                    if ($(this).attr("maint") == 'changed') {
                        id = $(this).attr("id");
                        val = $(this).attr("transactiontypeid") + '~' + $(this).attr("transactiondepth") + '~' + $(this).attr("transactiontakenby") + '~' + $(this).attr("transactionplot") + '~' + $(this).attr("transaction_gisplot") + '~' + $(this).find('td').eq(0).text() + '~' + $(this).find('td').eq(3).text();
                        $('<input>').attr({
                            type: 'hidden',
                            name: 'hf_' + id,
                            value: val
                        }).appendTo('form');
                    }
                });
                //e.preventDefault;
            });
             
                       
            $('#div_dateofbirth').datetimepicker({
                //$('#tb_dateofbirth').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: false,
                sideBySide: true,
                viewMode: 'years'
            });

            $("#div_dateofbirth").on("dp.change", function (e) {
                //$("#tb_dateofbirth").on("dp.change", function (e) {
                if (moment().diff(e.date, 'seconds') < 0) {
                    e.date = moment(e.date).subtract(100, 'years');
                    $("#tb_dateofbirth").val(moment(e.date).format('D MMM YYYY'));
                }
                var years = moment().diff(e.date, 'years');
                //alert(years);
                $("#tb_age").val(years);
                $("#dd_agetype").val('Years');


                //calculate_age(e.date);    
            });

            $('.dateselector').datetimepicker({
                //$('#tb_dateofbirth').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: false,
                sideBySide: true,
            });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                var target = $(e.target).attr("href") // activated tab
                //alert(target);
                if (target == '#div_map') {
                    $('#div_map').show();
                } else {
                    $('#div_map').hide();
                }
            });

            $("#form1").validate();

            $("#tb_internment_plot").click(function () {
                //mydialog = $("#dialog").load("plotselection.aspx").dialog({
                mydialog = $('#dialog').dialog({
                    width: 900,
                    height: 300
                });

            });

            //$(".transaction_edit").click(function () {
            $('body').on('click', '.transaction_edit', function () {
                var mode = $(this).text();
                $("#dialogtransaction").find(':input:not(:button)').val(''); // Clear all input fields
                $("#dialogplot").find(':input:not(:button)').val(''); // Clear all input fields
                $('#hf_cemetery').val('');
                $('#hf_area').val('');
                $('#hf_division').val('');
                $('#hf_GISplot').val('');

                if (mode == 'Edit') {
                    tr = $(this).closest('tr');
                    $('#tb_transactiondate').val($(tr).find('td').eq(0).text());
                    transactiontypesKey = $(tr).attr('transactiontypeid');
                    $('#dd_transactiontype').val(transactiontypesKey);
                    transaction_display(transactiontypesKey);

                    $('#tb_transactiondepth').val($(tr).attr('transactiondepth'));
                    $('#tb_transactiontakenby').val($(tr).attr('transactiontakenby'));
                    $('#tb_transactionremarks').val($(tr).find('td').eq(3).text());

                    $('#hf_cemetery').val($(tr).attr('transaction_cemetery'));
                    $('#hf_area').val($(tr).attr('transaction_area'));
                    $('#hf_division').val($(tr).attr('transaction_division'));
                    $('#hf_GISplot').val($(tr).attr('transaction_GISplot'));

                     //alert( $(tr).attr('transaction_cemetery') + ',' + $(tr).attr('transaction_area') + ',' + $(tr).attr('transaction_division') + ',' + $(tr).attr('transaction_GISplot'));

                } else {
                    $('#div_depth').hide();
                    $('#div_takenby').hide();
                    $('#div_selectplot').hide();
                }


                $('#plotlocation').cascadingDropdown({
                    selectBoxes: [
                        {
                            selector: '#plot_cemetery',
                            source: function (request, response) {
                                $.getJSON('../../_Dependencies/data.asmx/Cemeteries', function (data) {
                                    response($.map(data, function (item, index) {
                                        return {
                                            label: item.label,
                                            value: item.value
                                            , selected: item.value == $('#hf_cemetery').val() //selectOnlyOption
                                        };
                                    }));
                                });
                            }

                            /*
                            onChange: function (event, value, requiredValues, requirementsMet) {
                                alert('Cemetery changed');
                            }
                            */
                        },
                        {
                            selector: '#plot_area',
                            requires: ['#plot_cemetery'],
                            source: function (request, response) {
                                $.getJSON('../../_Dependencies/data.asmx/Areas', request, function (data) {
                                    //var selectOnlyOption = data.length <= 1;
                                    response($.map(data, function (item, index) {
                                        return {
                                            label: item.label,
                                            value: item.value//,
                                            , selected: item.value == $('#hf_area').val() //selectOnlyOption
                                            //selected: index == 0 //selectOnlyOption
                                        };
                                    }));
                                });
                            }
                        },
                        {
                            selector: '#plot_division',
                            requires: ['#plot_cemetery', '#plot_area'],
                            requireAll: true,
                            source: function (request, response) {
                                $.getJSON('../../_Dependencies/data.asmx/Divisions', request, function (data) {
                                    //var selectOnlyOption = data.length <= 1;
                                    response($.map(data, function (item, index) {
                                        return {
                                            label: item.label,
                                            value: item.value//,
                                            , selected: item.value == $('#hf_division').val() //selectOnlyOption
                                            //selected: index == 0 //selectOnlyOption
                                        };
                                    }));
                                });
                            }
                        },
                        {
                            selector: '#plot_plot',
                            requires: ['#plot_cemetery', '#plot_area', '#plot_division'],
                            requireAll: true,
                            source: function (request, response) {
                                $.getJSON('../../_Dependencies/data.asmx/Plots', request, function (data) {
                                    //var selectOnlyOption = data.length <= 1;
                                    response($.map(data, function (item, index) {
                                        return {
                                            label: item.label,
                                            value: item.value//,
                                            , selected: item.value == $('#hf_GISplot').val() //selectOnlyOption
                                            //selected: index == 0 //selectOnlyOption
                                        };
                                    }));
                                });
                            },
                            onChange: function (event, value, requiredValues, requirementsMet) {
                                if (!requirementsMet) {
                                    //$('#dialogplot').dialog('widget').find('.ui-dialog-buttonpane .ui-button:first').hide()
                                    //alert('To do: disable or hide Save button');
                                    return;
                                }
                                //plotlocation.loading(true);
                                //$('#dialogplot').dialog('widget').find('.ui-dialog-buttonpane .ui-button:first').show()
                                //alert('To do: enable or show Save button');
                                alert('To do: If used then show details, this value = ' + value);
                            }
                        }
                    ]
                });

                dialogtransaction = $('#dialogtransaction').dialog({
                    title: "Transaction Maintenance",
                    width: dialogwidth,
                    height: 600,
                    modal: true,
                    buttons: {
                        "Save": function () {
                            detailtext = 'Plot: ' + $('#plot_plot option:selected').text() + ', ' + $('#plot_cemetery option:selected').text() + ', ' + $('#plot_area option:selected').text() + ', ' + $('#plot_division option:selected').text();
                            if ($('#tb_transactiondepth').val() != '') {
                                detailtext += ' Depth: ' + $('#tb_transactiondepth').val();
                            }

                            if (mode == "Edit") {
                                transactionid = $(tr).attr('id');
                                transactionplot = $(tr).attr('transactionplot');
                            } else {
                                transactionid = "T_new_" + get_newctr();
                                transactionplot = "";
                            }

                            props = ' id="' + transactionid + '" maint="changed"' 
                                + ' transactiontypeid="' + $('#dd_transactiontype').val() + '"'
                                + ' transactiondepth="' + $('#tb_transactiondepth').val() + '"'
                                + ' transactiontakenby="' + $('#tb_transactiontakenby').val() + '"'
                                + ' transactionplot="' + transactionplot + '"'
                                + ' transaction_cemetery="' + $('#plot_cemetery').val() + '"'
                                + ' transaction_area="' + $('#plot_area').val() + '"'
                                + ' transaction_division="' + $('#plot_division').val() + '"'
                                + ' transaction_gisplot = "' + $('#plot_plot').val() + '"';

                            var $row = $('<tr' + props + '><td>' + $('#tb_transactiondate').val() + '</td><td >' + $('#dd_transactiontype option:selected').text() + '</td><td>' + detailtext + '</td><td>' + $('#tb_transactionremarks').val() + '</td><td class="transaction_edit">Edit</td></tr>');
                            if (mode == "Edit") {
                                $(tr).replaceWith($row);
                            } else {
                                $('#tbl_transactions > tbody:last').append($row);
                            }
                            $('#plotlocation').cascadingDropdown('destroy');
                            $(this).dialog("close");
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                            $('#plotlocation').cascadingDropdown('destroy');
                        }
                    }
                });

                $("#btn_selectplot").click(function () {
                    dialogplot = $('#dialogplot').dialog({
                        title: "Plot Selection",
                        width: dialogwidth,
                        height: 550,
                        modal: true,
                        buttons: {
                            "Save": function () {
                                alert('To do: validation and then populate transaction')
                                $(this).dialog("close");
                            },
                            Cancel: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                });

            });

            $("#dd_transactiontype").change(function () {
                transactiontypesKey = $(this).val();
                transaction_display(transactiontypesKey);
            });

            function get_newctr() {
                newctr++;
                return newctr;
            }

            function transaction_display(transactiontypesKey) {

                requirePlot = transactiontypes[transactiontypesKey]["RequirePlot"];
                RequireEntity = transactiontypes[transactiontypesKey]["RequireEntity"];
                if (requirePlot == 'Yes') {
                    $('#div_selectplot').show();
                    $('#div_depth').show();
                } else {
                    $('#div_selectplot').hide();
                    $('#div_depth').hide();
                }
                if (RequireEntity == 'Yes') {
                    $('#div_takenby').show();
                } else {
                    $('#div_takenby').hide();
                }
            }




            //There is a small issue, where a dropdown is changed by the user, the dependant dropdowns still use the parent values when they should really be set to -1
            //The problem is trying to identify that it was a human that changed it rather than the parent values



        }); //document ready
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <input type="hidden" value="<%= hf_id %>" id="hf_id" name="hf_id" />
    <input type="hidden" value="" id="hf_cemetery" />
    <input type="hidden" value="" id="hf_area" />
    <input type="hidden" value="" id="hf_division" />
    <input type="hidden" value="" id="hf_GISplot" />

    <a href="http://wdc.whanganui.govt.nz/cemeteries/business/home.aspx">http://wdc.whanganui.govt.nz/cemeteries/business/home.aspx</a>


    <ul class="nav nav-tabs">
        <li class="active"><a data-toggle="tab" data-target="#div_information">Information</a></li>
        <li><a data-toggle="tab" data-target="#div_transaction">Transactions</a></li>
        <li><a data-toggle="tab" data-target="#div_internment">Internment</a></li>
       <!-- <li><a data-toggle="tab" data-target="#div_map">Map</a></li>-->
        <li><a data-toggle="tab" data-target="#div_images">Images</a></li>
       
    </ul>
    <br />
    <div class="tab-content">
        <div id="div_information" class="tab-pane fade in active">

            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_warrant_no">Warrant number</label>
                <div class="col-sm-8">
                    <input id="tb_warrant_no" name="tb_warrant_no" type="text" class="form-control" maxlength="50" value="<%: warrant_no%>" />
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_register_no">Register number</label>
                <div class="col-sm-8">
                    <input id="tb_register_no" name="tb_register_no" type="text" class="form-control" maxlength="50" value="<%: register_no%>" />
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_surname">Surname</label>
                <div class="col-sm-8">
                    <input id="tb_surname" name="tb_surname" type="text" class="form-control" maxlength="50" value="<%: surname%>" />
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_givennames">Given names</label>
                <div class="col-sm-8">
                    <input id="tb_givennames" name="tb_givennames" type="text" class="form-control" maxlength="50" value="<%: givennames%>" />
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_lastpermanentaddress">Last Residence</label><div class="col-sm-8">
                    <textarea id="tb_lastpermanentaddress" name="tb_lastpermanentaddress" class="form-control" rows="4" maxlength="500" required><%: lastpermanentaddress%></textarea>
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_city">City</label>
                <div class="col-sm-8">
                    <input id="tb_city" name="tb_city" type="text" class="form-control" maxlength="50" value="<%: city%>" />
                </div>
            </div>

            <div class="form-group">
                <label for="tb_dateofdeath" class="control-label col-sm-4">Date of death</label>
                <div class="col-sm-8">
                    <div class="input-group date dateselector" id="div_dateofdeath">
                        <input id="tb_dateofdeath" name="tb_dateofdeath" required placeholder="eg: 23 Jun 1985" class="form-control" value="<%: dateofdeath%>" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <label for="tb_dateofbirth" class="control-label col-sm-4">Date of birth</label>
                <div class="col-sm-8">
                    <div class="input-group date" id="div_dateofbirth">
                        <input id="tb_dateofbirth" name="tb_dateofbirth" required placeholder="eg: 23 Jun 1985" class="form-control" value="<%: dateofbirth%>" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="dd_stillborn">Still born</label><div class="col-sm-8">
                    <select id="dd_stillborn" name="dd_stillborn" class="form-control" required>
                        <option></option>
                        <% 
                            Dictionary<string, string> yesnoOptions = new Dictionary<string, string>();
                            yesnoOptions["type"] = "select";
                            yesnoOptions["valuefield"] = "label";
                            Response.Write(Generic.Functions.buildselection(yesno_values, stillborn, yesnoOptions));
                        %>
                    </select>
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_age">Age</label>
                <div class="col-sm-4">
                    <input id="tb_age" name="tb_age" type="text" class="form-control" maxlength="50" value="<%: age%>" />
                </div>
                <div class="col-sm-4">
                    <select id="dd_ageperiod" name="dd_ageperiod" class="form-control" required>
                        <option></option>
                        <%
                            Dictionary<string, string> ageperiodOptions = new Dictionary<string, string>();
                            ageperiodOptions["type"] = "select";
                            ageperiodOptions["valuefield"] = "label";
                            string[] age_period_values = new string[4] { "Days", "Weeks", "Months", "Years" };
                            Response.Write(Generic.Functions.buildselection(age_period_values, age_period, ageperiodOptions));
                       %>
                    </select>
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="dd_gender">Gender</label><div class="col-sm-8">

                    <select id="dd_gender" name="dd_gender" class="form-control" required>
                        <option></option>
                        <%
                            Dictionary<string, string> genderOptions = new Dictionary<string, string>();
                            genderOptions["type"] = "select";
                            genderOptions["valuefield"] = "value";
                            string[,] gender_values = new string[3, 2] { { "Female", "F" }, { "Male", "M" }, { "Gender diverse", "D" } };
                            Response.Write(Generic.Functions.buildselection(gender_values, gender, genderOptions));
                        %>
                    </select>
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_rankoccupation">Rank / Occupation</label>
                <div class="col-sm-8">
                    <input id="tb_rankoccupation" name="tb_rankoccupation" type="text" class="form-control" maxlength="50" value="<%: rankoroccupation%>" />
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="dd_marital">Marital status</label><div class="col-sm-8">

                    <select id="dd_maritalstatus" name="dd_maritalstatus" class="form-control">
                        <option></option>
                        <%
                                                        Dictionary<string, string> maritalstatusOptions = new Dictionary<string, string>();
                            maritalstatusOptions["type"] = "select";
                            maritalstatusOptions["valuefield"] = "label";
                            string[] maritalstatus_values = new string[4] { "Widow", "Widowed", "Married", "Infant" };
                            Response.Write(Generic.Functions.buildselection(maritalstatus_values, maritalstatus, genderOptions));
                        %>
                    </select>
                </div>
            </div>



            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_placeofdeath">Place of death</label>
                <div class="col-sm-8">
                    <input id="tb_placeofdeath" name="tb_placeofdeath" type="text" class="form-control" maxlength="50" value="<%: placeofdeath%>" />
                </div>
            </div>

            <div class="form-group">
                <label for="tb_dateofmedcert" class="control-label col-sm-4">Date Of medical certificate</label>
                <div class="col-sm-8">
                    <div class="input-group date dateselector" id="div_dateofmedcert">
                        <input id="tb_dateofmedcert" name="tb_dateofmedcert" required placeholder="eg: 23 Jun 1985" class="form-control" value="<%: dateofmedcert%>" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_minister">Celebrant / Minister</label>
                <div class="col-sm-8">
                    <input id="tb_minister" name="tb_minister" type="text" class="form-control" maxlength="60" value="<%: minister%>" />
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_denomination">Religous affiliation</label>
                <div class="col-sm-8">
                    <input id="tb_denomination" name="tb_denomination" type="text" class="form-control" maxlength="50" value="<%: denomination%>" />
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_funeralcoordinator">Funeral co-ordinator</label>
                <div class="col-sm-8">
                    <input id="tb_funeralcoordinator" name="tb_funeralcoordinator" type="text" class="form-control" maxlength="50" value="<%: funeralcoordinator%>" />
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-4" for="tb_remarks">Remarks</label><div class="col-sm-8">
                    <textarea id="tb_remarks" name="tb_remarks" class="form-control" rows="4" maxlength="255"><%: remarks%></textarea>
                </div>
            </div>

            <div class="form-group">
                <div class="col-sm-4">
                </div>
                <div class="col-sm-8">
                    <asp:Button ID="btn_submit" runat="server" Text="Save" OnClick="btn_submit_Click" class="btn_submit btn btn-info submit"  />
                </div>
            </div>
        </div>

        <div id="div_transaction" class="tab-pane fade">
            <!-- Accordian header start -->
            <div class="panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <a data-toggle="collapse" href="#collapse_transaction">
                            <h4 class="panel-title">Transactions (trying to decide on whether to use tabs or an accordian)</h4>
                        </a>
                    </div>
                    <div id="collapse_transaction" class="panel-collapse collapse in">
                        <div class="panel-body">

                            <!-- Accordian header end -->

                            <table id="tbl_transactions" class="table table-striped">
                                <thead>
                                    <tr>
                                        <th>Date</th>
                                        <th>Type</th>
                                        <th>Detail</th>
                                        <th>Remarks</th>
                                        <th>Action | <span class="transaction_edit">Add</span></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%= transactions %>
                                </tbody>
                            </table>

                            <% if (1 == 2)
                                { %>
                            <div style="text-align: center">
                                <div class="form-group">
                                    <label class="control-label col-sm-1 input-sm" style="text-align: left" for="tb_transaction_date">Date</label>
                                    <label class="control-label col-sm-2 input-sm" style="text-align: left" for="dd_transaction_type">Type</label>
                                    <label class="control-label col-sm-3 input-sm" style="text-align: left" for="tb_transaction_remarks">Remarks</label>
                                    <label class="control-label col-sm-3 input-sm" style="text-align: left" for="tb_transaction_plot">Plot</label>
                                    <label class="control-label col-sm-2 input-sm" style="text-align: left" for="dd_transaction_other">Other</label>
                                </div>

                                <div>
                                    <div class="form-group">
                                        <div class="col-sm-3 input-sm">
                                            <input id="tb_transaction_plot" name="tb_transaction_plot" type="text" class="form-control input-sm" readonly="readonly" />
                                            <!--<button id="btn_findplot" type="button" class="btn btn-info">Find</button>-->
                                        </div>

                                        <div class="col-sm-1 input-sm">
                                            <input id="tb_transaction_date" name="tb_transaction_date" type="text" class="form-control input-sm dateselector" />
                                        </div>

                                        <div class="col-sm-2 input-sm">
                                            <select id="dd_transaction_type" name="dd_transaction_type" class="form-control input-sm">
                                                <option></option>
                                                <option value="Burialash">Burial of ashes</option>
                                                <option value="Memorial">Memorial (only)</option>
                                                <option value="Taken">Ashes taken</option>
                                                <option value="Cremation">Cremation</option>
                                                <option value="Scattered">Ashes scattered</option>
                                                <option value="Disinter">Distransaction</option>
                                                <option value="Burial">Burial</option>
                                            </select>
                                        </div>

                                        <div class="col-sm-3 input-sm">
                                            <input id="tb_transaction_remarks" name="tb_transaction_remarks" type="text" class="form-control input-sm" />
                                        </div>

                                        <!-- dependant on transaction_type: taken by or depth -->
                                        <div class="col-sm-3 input-sm">
                                            <input id="tb_transaction_other" name="tb_transaction_other" type="text" class="form-control input-sm" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <% } %>

                            <!-- Accordian footer start -->
                        </div>
                    </div>
                </div>
            </div>
            <!-- Accordian footer end -->
        </div>



        <div id="div_internment" class="tab-pane fade">

            <!-- Accordian header start -->

            <div class="panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <a data-toggle="collapse" href="#collapse_internment">
                            <h4 class="panel-title">Internment - Probably not going to use - See transactions instead</h4>
                        </a>
                    </div>
                    <div id="collapse_internment" class="panel-collapse collapse">
                        <div class="panel-body">

                            <!-- Accordian header end -->
                            <div style="text-align: center">

                                <div class="form-group">
                                    <label class="control-label col-sm-2 input-sm" style="text-align: left" for="dd_internment_modifier">Modifier</label>
                                    <label class="control-label col-sm-1 input-sm" style="text-align: left" for="tb_internment_date">Date</label>
                                    <label class="control-label col-sm-2 input-sm" style="text-align: left" for="dd_internment_type">Type</label>
                                    <label class="control-label col-sm-1 input-sm" style="text-align: left" for="dd_internment_status">Status</label>
                                    <label class="control-label col-sm-3 input-sm" style="text-align: left" for="tb_internment_remarks">Remarks</label>
                                </div>
                                <div>
                                    <div class="form-group">
                                        <div class="col-sm-3 input-sm">
                                            <input id="tb_internment_plot" name="tb_internment_plot" type="text" class="form-control input-sm" readonly="readonly" />
                                            <!--<button id="btn_findplot" type="button" class="btn btn-info">Find</button>-->
                                        </div>

                                        <div class="col-sm-2 input-sm">
                                            <select id="dd_internment_modifier" name="dd_internment_modifier" class="form-control input-sm">
                                                <option></option>
                                                <option>In space to the left</option>
                                                <option>In space to the right</option>
                                                <option>Scattered near</option>
                                            </select>
                                        </div>

                                        <div class="col-sm-1 input-sm">
                                            <input id="tb_internment_date" name="tb_internment_date" type="text" class="form-control input-sm dateselector" />
                                        </div>

                                        <div class="col-sm-2 input-sm">
                                            <select id="dd_internment_type" name="dd_internment_type" class="form-control input-sm">
                                                <option></option>
                                                <option value="Burialash">Burial of ashes</option>
                                                <option value="Memorial">Memorial (only)</option>
                                                <option value="Taken">Ashes taken</option>
                                                <option value="Cremation">Cremation</option>
                                                <option value="Scattered">Ashes scattered</option>
                                                <option value="Disinter">Disinternment</option>
                                                <option value="Burial">Burial</option>
                                            </select>
                                        </div>

                                        <div class="col-sm-1 input-sm">
                                            <select id="dd_internment_status" name="dd_internment_status" class="form-control input-sm">
                                                <option></option>
                                                <option>Current</option>
                                                <option>Past</option>
                                            </select>
                                        </div>

                                        <div class="col-sm-3 input-sm">
                                            <input id="tb_internment_remarks" name="tb_internment_remarks" type="text" class="form-control input-sm" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- Accordian footer start -->
                        </div>
                    </div>
                </div>
            </div>
            <!-- Accordian footer end -->
        </div>

        <div id="div_map" class="tab-pane fade">
            <div style="text-align: center">
                <asp:Literal ID="lit_map" runat="server"></asp:Literal>
            </div>
        </div>

        <div id="div_images" class="tab-pane fade">
            <%=html_images %>
        </div>
    </div>


    <div id="dialogtransaction" class="form-horizontal" style="display: none; background-color: #FCF7EA">
        <div class="form-group">
            <label for="tb_transactiondate" class="control-label col-sm-4">Date</label>
            <div class="col-sm-8">
                <div class="input-group date dateselector" id="div_transactiondate">
                    <input id="tb_transactiondate" name="tb_transactiondate" required placeholder="eg: 23 Jun 1985" class="form-control" value="1 Jan 2000" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="dd_transactiontype">Type</label>
            <div class="col-sm-8">
                <select id="dd_transactiontype" name="dd_transactiontype" class="form-control" required>
                    <option></option>
                    <%
                        Dictionary<string, string> transactiontypeOptions = new Dictionary<string, string>();
                        transactiontypeOptions["type"] = "select";
                        transactiontypeOptions["valuefield"] = "label";
                        Response.Write(Generic.Functions.buildselection(transactiontype_values, "", transactiontypeOptions));
                    %>
                </select>
            </div>
        </div>
        <div class="form-group" id="div_depth">
            <label class="control-label col-sm-4" for="tb_transactiondepth">Depth</label>
            <div class="col-sm-8">
                <input type="text" id="tb_transactiondepth" name="tb_transactiondepth" class="form-control" />
            </div>
        </div>
        <div class="form-group" id="div_takenby">
            <label class="control-label col-sm-4" for="tb_transactiontakenby">Taken by</label>
            <div class="col-sm-8">
                <input type="text" id="tb_transactiontakenby" name="tb_transactiontakenby" class="form-control" required />

            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="tb_transactionremarks">Remarks</label>
            <div class="col-sm-8">
                <textarea id="tb_transactionremarks" name="tb_transactionremarks" class="form-control" rows="4" maxlength="500"></textarea>
            </div>
        </div>

        <div class="form-group" id="div_selectplot">
            <div class="col-sm-4">
            </div>
            <div class="col-sm-8">
                <input id="btn_selectplot" type="button" value="Select Plot" class="btn btn-info" />
            </div>
        </div>

    </div>

    <div id="dialogplot" class="form-horizontal" style="display: none; background-color: #FCF7EA">
        <div id="plotlocation" class="table-responsive">
            <table class="table-bordered" style="width: 98%;">
                <tr>
                    <td>
                        <h2>Location Details</h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="plot_cemetery">Cemetery</label>
                            <div class="col-sm-8">
                                <select id="plot_cemetery" name="cemetery" class="form-control">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="plot_area">Area</label>
                            <div class="col-sm-8">
                                <select id="plot_area" name="area" class="form-control">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="plot_division">Division</label>
                            <div class="col-sm-8">
                                <select id="plot_division" name="division" class="form-control">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="plot_plot">Plot</label>
                            <div class="col-sm-8">
                                <select id="plot_plot" name="plot" class="form-control">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="transaction_located">Located</label>
                            <div class="col-sm-8">
                                <select id="transaction_located" name="transaction_located" class="form-control">
                                    <option>In</option>
                                    <option>To the left of</option>
                                    <option>To the right of</option>
                                    <option>Near</option>
                                    <option>Around</option>
                                </select>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="control-label col-sm-4" for="plot_remarks">Remarks</label>
                            <div class="col-sm-8">
                                <textarea id="plot_remarks" name="plot_remarks" class="form-control">To do</textarea>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
