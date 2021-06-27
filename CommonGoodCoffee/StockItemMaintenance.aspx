<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StockItemMaintenance.aspx.cs" Inherits="CommonGoodCoffee.StockItemMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.AreYouSure/1.9.0/jquery.are-you-sure.min.js"></script>
    <style>
        transaction_date {
            width: 200px;
        }

        transaction_quantity {
            width: 200px;
        }

        transaction_type {
            width: 300px;
        }
    </style>

    <script type="text/javascript">
        var newctr = 0;
        var mode = "<%=ViewState["customer_ctr"]%>";

        $(document).ready(function () {
            $('#form1').areYouSure();
            $('.transaction').click(function () {
                thistransaction = $(this).parent().next();
                thishidden = $(thistransaction).is(":hidden");
                $('.transaction').each(function (index) {
                    $(this).parent().next().hide();
                });
                if (thishidden) {
                    $(thistransaction).show();
                }
            })

            $('#menu').click(function () {
                window.location.href = "/default.aspx";
            });

            $('#list').click(function () {
                window.location.href = "/stockitemList.aspx";
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

            $('.standarddate').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true
                //,maxDate: moment().add(-1, 'year')
            });

            $('.submit').click(function () { //Started creating functions so that I can group code together - see update_transaction() - not yet tested
                delim = String.fromCharCode(254);

                update_batch()

            });  //.submit end

            /* ========================================= EDIT BATCHES ===========================================*/
            $(document).on('click', '.batchedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    //$('#div_batch_quantity').show();
                    $("#dialog_batch").find(':input').val('');
                } else {
                    //$('#div_batch_quantity').hide();
                    tr = $(this).closest('tr');
                    $('#fld_batch_date').val($(tr).find('td').eq(1).text());
                    //$('#fld_batch_quantity').val($(tr).find('td').eq(2).text());
                    $('#fld_batch_note').val($(tr).find('td').eq(3).text());
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_batch").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true
                    , open: function (type, data) {
                        //$(this).appendTo($('form')); // reinsert the dialog to the form   
                        $("#form1 :button").prop("disabled", true);
                        $('#form1 :input[type="submit"]').prop('disabled', true);
                    }
                    , close: function (event, ui) {
                        //$('#fld_encounter_worker').select2('destroy');
                        $("#form1 :button").prop("disabled", false);
                        $('#form1 :input[type="submit"]').prop('disabled', false);
                    }
                    , appendTo: "#form2"
                });

                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#div_batches > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');

                                $('#div_batches > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'batch_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");

                                $(tr).attr('id', 'transaction_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");


                            } else {
                                $(tr).find('td:first').attr("class", "changed");
                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_batch_date').val());
                            $(tr).find('td').eq(2).text($('#fld_batch_quantity').val());
                            $(tr).find('td').eq(3).text($('#fld_batch_note').val());

                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this batch and any transactions against it?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }

                $("#dialog_batch").dialog('option', 'buttons', myButtons);
            })

            function update_batch() {
                delim = String.fromCharCode(254);
                $('#batchestable > tbody > tr[maint="changed"]').each(function () {

                    tr_id = $(this).attr('id');
                    tr_date = $(this).find('td:eq(1)').text();
                    //tr_quantity = $(this).find('td:eq(2)').text();
                    tr_note = $(this).find('td:eq(3)').text();

                    //value = tr_date + delim + tr_quantity + delim + tr_note;
                    value = tr_date + delim + delim + tr_note;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
            }



        }); //document.ready

        function get_newctr() {
            newctr++;
            return newctr;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="toprighticon">
        <input type="button" id="list" class="btn btn-info" value="Stock List" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <div class="bottomrighticon">
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />
    </div>
    <h1>Stock Item Maintenance
    </h1>
    <div class="form-horizontal row">
        <div class="col-md-8">

            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_stockitem">Item</label>
                <div class="col-md-6">
                    <input id="fld_stockitem" name="fld_stockitem" type="text" class="form-control confirm" value="<%:fld_stockitem%>" maxlength="20" required="required" />
                </div>
            </div>


            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_description">Description</label>
                <div class="col-md-6">
                    <textarea id="fld_description" name="fld_description" class="form-control"><%:fld_description%></textarea>
                </div>
            </div>

            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_note">Note</label>
                <div class="col-md-6">
                    <textarea id="fld_note" name="fld_note" class="form-control"><%:fld_note%></textarea>
                </div>
            </div>

        </div>

    </div>


    <!------------------------------------------ TABS ------------------------------------------------------------>

    <div class="form-horizontal">
        <ul class="nav nav-tabs">
            <li class="active"><a data-target="#div_batches">Batches</a></li>
        </ul>
        <div class="tab-content">
            <!-- ================================= BATCHES TAB ===================================  -->
            <div id="div_batches" class="tab-pane fade in active">
                <!--<h3 class="tabheading">Transactions</h3>-->
                <table id="batchestable" class="table" style="width: 100%">
                    <%= html_batches %>
                </table>
            </div>
            <!-- ================================= BATCHES DIALOG ===================================  -->

            <div id="dialog_batch" title="Maintain Batch" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label for="fld_batch_date" class="control-label col-sm-4">
                        Date
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_batch_date" name="fld_batch_date" required="required" placeholder="eg: 23 Jun 1985" type="text" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <!--
                <div id="div_batch_quantity" class="form-group">
                    <label class="control-label col-sm-4" for="fld_batch_quantity">Quantity</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_batch_quantity" name="fld_batch_quantity" class="form-control" required="required" />
                    </div>
                </div>
                -->

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_batch_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_batch_note" name="fld_batch_note" class="form-control"></textarea>
                    </div>
                </div>
            </div>



            <!-- ================================= END OF TABS ===================================  -->
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <form id="form2">
    </form>
</asp:Content>
