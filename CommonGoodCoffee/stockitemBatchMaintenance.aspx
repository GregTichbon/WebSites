<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="stockitemBatchMaintenance.aspx.cs" Inherits="CommonGoodCoffee.stockitemBatchMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.AreYouSure/1.9.0/jquery.are-you-sure.min.js"></script>
    <style>
             
    </style>

    <script type="text/javascript">
        var newctr = 0;

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

            $(".numeric").keydown(function (event) {
                if (event.shiftKey == true) {
                    event.preventDefault();
                }
               
                if (
                    (event.keyCode >= 48 && event.keyCode <= 57) ||
                    (event.keyCode >= 96 && event.keyCode <= 105) ||
                    event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || 
                    (event.keyCode == 190 && !($(this).hasClass('nopoint'))) ||
                    (event.keyCode == 189 && !($(this).hasClass('nominus')))
                ) {

                } else {
                    event.preventDefault();
                }

                if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
                    event.preventDefault();
  
                if ($(this).val() !== '' && event.keyCode == 189)
                    event.preventDefault();
     
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

                update_transaction()

            });  //.submit end

            /* ========================================= EDIT TRANSACTIONS ===========================================*/
            $(document).on('click', '.transactionedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    //$('#div_transaction_quantity').show();
                    $("#dialog_transaction").find(':input').val('');
                } else {
                    //$('#div_transaction_quantity').hide();
                    tr = $(this).closest('tr');
                    $('#fld_transaction_date').val($(tr).find('td').eq(1).text());
                    $('#fld_transaction_type').val($(tr).find('td').eq(2).data('id'));
                    $('#fld_transaction_quantity').val($(tr).find('td').eq(3).text());
                    $('#fld_transaction_note').val($(tr).find('td').eq(4).text());
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_transaction").dialog({
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
                                tr = $('#div_transactions > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_transactions > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'transaction_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");
                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_transaction_date').val());
                            $(tr).find('td').eq(2).text($('#fld_transaction_type option:selected').text());
                            $(tr).find('td').eq(2).data('id', $('#fld_transaction_type').val());
                            $(tr).find('td').eq(3).text($('#fld_transaction_quantity').val());
                            $(tr).find('td').eq(4).text($('#fld_transaction_note').val());

                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this transaction?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }

                $("#dialog_transaction").dialog('option', 'buttons', myButtons);
            })

            function update_transaction() {
                delim = String.fromCharCode(254);
                $('#transactionstable > tbody > tr[maint="changed"]').each(function () {

                    tr_id = $(this).attr('id');
                    tr_date = $(this).find('td:eq(1)').text();
                    tr_type = $(this).find('td:eq(2)').data('id');
                    tr_quantity = $(this).find('td:eq(3)').text();
                    tr_note = $(this).find('td:eq(4)').text();

                    value = tr_date + delim + tr_type + delim + tr_quantity + delim + tr_note;

                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });

                $('#transactionstable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
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
    <h1>Stock Item Batch Maintenance - <%=stockItem %>
    </h1>
    <div class="form-horizontal row">
        <div class="col-md-8">
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_date">Date</label>
                <div class="col-sm-6">
                    <div class="input-group standarddate">
                        <input id="fld_date" name="fld_date" type="text" class="form-control" required="required" value="<%:fld_date %>" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_reference">Reference</label>
                <div class="col-md-6">
                    <input id="fld_reference" name="fld_reference" type="text" class="form-control" maxlength="20" value="<%:fld_reference%>" />
                </div>
            </div>
             
                <div class="form-group row">
                    <label class="control-label col-md-6" for="fld_ordered">Ordered</label>
                    <div class="col-sm-6">
                        <div class="input-group standarddate">
                            <input id="fld_ordered" name="fld_ordered" type="text" class="form-control" value="<%:fld_ordered%>" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
    

            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_note">Note</label>
                <div class="col-md-6">
                    <textarea id="fld_note" name="fld_note" class="form-control"><%:fld_note%></textarea>
                </div>
            </div>
            <div id="div_takeon_transaction" style="display:<%=showtransaction%>">
                <div class="col-md-6"></div>
                <div class="col-md-6"><h2>Takeon Transaction</h2></div>
             <div class="form-group row">
                 <label class="control-label col-md-6" for="fld_takeon_quantity">Quantity</label>
                 <div class="col-md-6">
                     <input id="fld_takeon_quantity" name="fld_takeon_quantity" type="text" class="form-control numeric" />
                 </div>
             </div>
                <div class="form-group row">
                    <label class="control-label col-md-6" for="fld_takeon_note">Transaction Note</label>
                    <div class="col-md-6">
                        <textarea id="fld_takeon_note" name="fld_takeon_note" class="form-control"></textarea>
                    </div>
                </div>
            </div>

        </div>
    </div>
  
    <!------------------------------------------ TABS ------------------------------------------------------------>

    <div class="form-horizontal">
        <ul class="nav nav-tabs">

            <%=html_tab %>
        </ul>
        <div class="tab-content">
            <!-- ================================= TRANSACTIONS TAB ===================================  -->
            <div id="div_transactions" class="tab-pane fade in active">
                <!--<h3 class="tabheading">Transactions</h3>-->
                <table id="transactionstable" class="table" style="width: 100%">
                    <%= html_transactions %>
                </table>
            </div>
            <!-- ================================= TRANSACTIONS DIALOG ===================================  -->

            <div id="dialog_transaction" title="Maintain Transactions" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label for="fld_transaction_date" class="control-label col-sm-4">
                        Date
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_transaction_date" name="fld_transaction_date" required="required" placeholder="eg: 23 Jun 1985" type="text" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
              
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_transaction_type">Type</label>
                    <div class="col-sm-8">
                     <select id="fld_transaction_type" name="fld_transaction_type" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                //string[] nooptions = { }; //temp
                                Dictionary<string, string> transactiontypeoptions = new Dictionary<string, string>();
                                transactiontypeoptions["type"] = "select";
                                transactiontypeoptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(transactiontypes, nooptions, transactiontypeoptions));
                            %>
                        </select>
                        </div>
                </div>


                <div id="div_transaction_quantity" class="form-group">
                    <label class="control-label col-sm-4" for="fld_transaction_quantity">Quantity</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_transaction_quantity" name="fld_transaction_quantity" class="form-control numeric" required="required" />
                    </div>
                </div>
                
               
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_transaction_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_transaction_note" name="fld_transaction_note" class="form-control"></textarea>
                    </div>
                </div>
            </div>

            <!-- ================================= ORDERS TAB ===================================  -->
            <div id="div_orders" class="tab-pane fade in">
                <!--<h3 class="tabheading">Transactions</h3>-->
                <table id="orderstable" class="table" style="width: 100%">
                    <%= html_orders %>
                </table>
            </div>
            <!-- ================================= TRANSACTIONS DIALOG ===================================  -->
            
            <!-- ================================= END OF TABS ===================================  -->
        </div>
    </div>
     
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <form id="form2">
    </form>
</asp:Content>

