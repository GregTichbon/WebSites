<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StockReceived.aspx.cs" Inherits="CommonGoodCoffee.StockReceived" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <style>
             
    </style>

    <script type="text/javascript">
        var newctr = 0;

        $(document).ready(function () {

            $('.item').click(function () {
                thisitem = $(this).parent().next();
                thishidden = $(thisitem).is(":hidden");
                $('.item').each(function (index) {
                    $(this).parent().next().hide();
                });
                if (thishidden) {
                    $(thisitem).show();
                }
            })
            $(".numeric").keydown(function (event) {
                if (event.shiftKey == true) {
                    event.preventDefault();
                }
                if ((event.keyCode >= 48 && event.keyCode <= 57) ||
                    (event.keyCode >= 96 && event.keyCode <= 105) ||
                    event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 ||
                    event.keyCode == 39 || event.keyCode == 46 ||
                    (event.keyCode == 190 && !($(this).hasClass('nopoint')))) {

                } else {
                    event.preventDefault();
                }

                if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
                    event.preventDefault();
                //if a decimal has been added, disable the "."-button

            });

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

            

            $('.standarddate').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true
                //,maxDate: moment().add(-1, 'year')
            });

            $('.submit').click(function (e) {
                if ($('#itemstable > tbody > tr[maint="changed"]').length == 0) {
                    e.preventDefault();
                    alert('You need to add at least one item');
                } else {
                    update_item();
                }

            });  //.submit end

            /* ========================================= EDIT BATCH ===========================================*/
            $(document).on('click', '.itemedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    //$('#div_item_quantity').show();
                    $("#dialog_item").find(':input').val('');
                } else {
                    //$('#div_item_quantity').hide();
                    tr = $(this).closest('tr');
                    $('#fld_item_type').val($(tr).find('td').eq(1).data('id'));
                    $('#fld_item_quantity').val($(tr).find('td').eq(2).text());
                    //$('#fld_item_onorder').val($(tr).find('td').eq(3).text());
                    $('#fld_item_note').val($(tr).find('td').eq(3).text());
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_item").dialog({
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
                                tr = $('#div_items > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_items > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'item_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");
                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_item_type option:selected').text());
                            $(tr).find('td').eq(1).data('id', $('#fld_item_type').val());
                            $(tr).find('td').eq(2).text($('#fld_item_quantity').val());
                            $(tr).find('td').eq(3).text($('#fld_item_onorder').val());
                            $(tr).find('td').eq(4).text($('#fld_item_note').val());

                            $(this).dialog("close");
                        }
                    }
                }

                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this item?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }

                $("#dialog_item").dialog('option', 'buttons', myButtons);
            })

            function update_item() {
                delim = String.fromCharCode(254);
                $('#itemstable > tbody > tr[maint="changed"]').each(function () {

                    tr_id = $(this).attr('id');
                    tr_type = $(this).find('td:eq(1)').data('id');
                    tr_quantity = $(this).find('td:eq(2)').text();
                    //tr_onorder = $(this).find('td:eq(3)').text();
                    tr_note = $(this).find('td:eq(3)').text();

                    value = tr_type + delim + tr_quantity + delim + tr_note;
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
    <h1>Stock Received
    </h1>
    <div class="form-horizontal row">
        <div class="col-md-8">
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_date">Date</label>
                <div class="col-sm-6">
                    <div class="input-group standarddate">
                        <input id="fld_date" name="fld_date" type="text" class="form-control" required="required" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>
    
        <div class="form-group row">
            <label class="control-label col-md-6" for="fld_reference">Reference</label>
            <div class="col-md-6">
                <input id="fld_reference" name="fld_reference" type="text" class="form-control" maxlength="20" required="required" />
            </div>
        </div>
        
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_ordered">Ordered</label>
                <div class="col-sm-6">
                    <div class="input-group standarddate">
                        <input id="fld_ordered" name="fld_ordered" type="text" class="form-control" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>
       


        <div class="form-group row">
            <label class="control-label col-md-6" for="fld_note">Note</label>
            <div class="col-md-6">
                <textarea id="fld_note" name="fld_note" class="form-control"></textarea>
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
            <!-- ================================= ITEMS TAB ===================================  -->
            <div id="div_items" class="tab-pane fade in active">
                <!--<h3 class="tabheading">Transactions</h3>-->
                <table id="itemstable" class="table" style="width: 100%">
                    <%= html_items %>
                </table>
            </div>
            <!-- ================================= TRANSACTIONS DIALOG ===================================  -->

            <div id="dialog_item" title="Maintain Items" style="display: none" class="form-horizontal">

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_item_type">Type</label>
                    <div class="col-sm-8">
                     <select id="fld_item_type" name="fld_item_type" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                //string[] nooptions = { }; //temp
                                Dictionary<string, string> itemtypeoptions = new Dictionary<string, string>();
                                itemtypeoptions["type"] = "select";
                                itemtypeoptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(itemtypes, nooptions, itemtypeoptions));
                            %>
                        </select>
                        </div>
                </div>

                <div id="div_item_quantity" class="form-group">
                    <label class="control-label col-sm-4" for="fld_item_quantity">Quantity</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_item_quantity" name="fld_item_quantity" class="form-control numeric" required="required" />
                    </div>
                </div>

                
               
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_item_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_item_note" name="fld_item_note" class="form-control"></textarea>
                    </div>
                </div>
            </div>

            
            <!-- ================================= END OF TRANSACTIONS DIALOG ===================================  -->
            
            <!-- ================================= END OF TABS ===================================  -->
        </div>
    </div>
     
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <form id="form2">
    </form>
</asp:Content>

