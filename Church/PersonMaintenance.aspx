<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PersonMaintenance.aspx.cs" Inherits="Church.PersonMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/cropper/4.0.0/cropper.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/cropper/4.0.0/cropper.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.AreYouSure/1.9.0/jquery.are-you-sure.min.js"></script>


    <style>
        .imagecontainer {
            max-width: 800px;
            max-height: 800px;
            margin: 20px auto;
        }

        #preview {
            overflow: hidden;
            width: 200px;
            height: 200px;
        }

        img {
            max-width: 100%;
        }

        #canvas {
            background-color: #ffffff;
            cursor: default;
            border: 1px solid black;
            width: 1200px;
        }
    </style>

    <script type="text/javascript">
        var newctr = 0;
        var mode = "<%=ViewState["person_ctr"]%>";
        var last_order_date = "";
        var last_order_reference = "";

        $(document).ready(function () {

            $('#form1').areYouSure();

            $('#menu').click(function () {
                window.location.href = "/default.aspx";
            });

            $('#search').click(function () {
                window.location.href = "/personsearch.aspx";
            });

            $("#form1").validate();

            $('#fld_persontype').change(function () {
                if ($(this).val() == 1) {
                    $('#div_businessfields').show();
                } else {
                    $('#div_businessfields').hide();
                    $('#fld_businessname').val('');
                    $('#fld_businesstype').val('');
                }
            });

            var canvas = $("#canvas")
            var context = canvas.get(0).getContext("2d")

            $('#getphoto').click(function () {
                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }
                $("#dialog_getphoto").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true,
                    buttons: {
                        "Restore": function () {
                            canvas.cropper('reset');
                            //$result.empty();
                        },
                        "Cancel": function () {
                            canvas.cropper("destroy");
                            $(this).dialog("close");
                        },
                        "Upload": function () {
                            var image = canvas.cropper('getCroppedCanvas').toDataURL("image/jpg");
                            image = image.replace('data:image/png;base64,', '');
                            //console.log(image);
                            $.ajax({
                                type: "POST",
                                //async: false,
                                url: "_dependencies/posts.asmx/SaveImage",
                                data: '{"imageData": "' + image + '", "id": "<%: person_ctr %>"}',
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (result) {
                                    //alert(result);
                                    d = new Date();
                                    $("#img_photo").attr("src", "images/<%:person_ctr %>.jpg?" + d.getTime());
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert("Status: " + textStatus); alert("Error: " + errorThrown);
                                }
                            });
                            canvas.cropper("destroy");
                            $(this).dialog("close");
                        }
                    }
                });
            })

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

            $('#fileInput').on('change', function () {
                if (this.files && this.files[0]) {
                    if (this.files[0].type.match(/^image\//)) {
                        var reader = new FileReader();
                        reader.onload = function (evt) {
                            var img = new Image();
                            img.onload = function () {
                                context.canvas.height = img.height;
                                context.canvas.width = img.width;
                                context.drawImage(img, 0, 0);
                                var cropper = canvas.cropper({
                                    preview: '#preview',
                                    dragMode: 'crop',
                                    autoCropArea: 0.65,
                                    rotatable: true,
                                    cropBoxMovable: true,
                                    cropBoxResizable: true
                                });
                            };
                            img.src = evt.target.result;
                        };
                        reader.readAsDataURL(this.files[0]);
                    }
                    else {
                        alert("Invalid file type! Please select an image file.");
                    }
                }
                else {
                    alert('No file(s) selected.');
                }
            });

            $('#img_photo').click(function () {
                $("#fullphoto").attr('src', $('#img_photo').attr('src'));
                $("#dialog_fullphoto").dialog({
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true
                });
            });



            $('.geocode').click(function () {
                address = $('#fld_address_address').val();
                if (address.toLowerCase().indexOf("whanganui") == -1 && address.toLowerCase().indexOf("wanganui") == -1) {
                    address += ", Whanganui";
                }
                $.ajax({
                    type: "POST",
                    //async: false,
                    url: "posts.asmx/googlegeocode",
                    data: '{"address": "' + address + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        $('#fld_address_address').val(result.d.address.replace(/, /g, '\n'));
                        $('#fld_address_coordinates').val(result.d.lat + "," + result.d.lng);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Status: " + textStatus); alert("Error: " + errorThrown);
                    }
                });
            })

            $(document).on('click', '.send_email_local', function () {
                email = $('td:eq(1)', $(this).parents('tr')).text();
                firstname = $('#tb_firstname').val();
                knownas = $('#tb_knownas').val();
                if (knownas == "") {
                    knownas = firstname;
                }
                $(this).attr("href", "mailto:" + email + "?subject=Te Ora Hou Whanganui&body=Hi " + knownas);
            })

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

            $('.fld_order_delivereddate').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true
                //,maxDate: moment().add(-1, 'year')
            });

            $('.submit').click(function (e) { //Started creating functions so that I can group code together - see update_order() - not yet tested
                //e.preventDefault();
                delim = String.fromCharCode(254);

                update_order();
                update_subscription();

            });  //.submit end


            /* ========================================= EDIT ORDERS ===========================================*/
            $(document).on('click', '.orderedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $('#fld_order_ctr').val(0);
                    $("#dialog_order").find(':input').val('');
                    $('#fld_order_date').val(last_order_date);
                    $('#fld_order_reference').val(last_order_reference);
                    $('#div_order_stockitembatch').hide();
                } else {
                    tr = $(this).closest('tr');
                    $('#fld_order_date').val($(tr).find('td').eq(1).text());
                    $('#fld_order_reference').val($(tr).find('td').eq(2).text());
                    $('#fld_order_stockitem').val($(tr).find('td').eq(3).attr('stockitem_ctr'));
                    $('#fld_order_grind').val($(tr).find('td').eq(4).attr('grind_ctr'));
                    $('#fld_order_quantity').val($(tr).find('td').eq(5).text());
                    $('#fld_order_amount').val($(tr).find('td').eq(6).text());
                    $('#fld_order_delivereddate').val($(tr).find('td').eq(7).text());
                    //$('#fld_order_stockitembatch').val($(tr).find('td').eq(8).data('id'));
                    $('#fld_order_invoicereference').val($(tr).find('td').eq(9).text());
                    $('#fld_order_note').val($(tr).find('td').eq(11).text());

                    show_div_order_stockitembatch();
                    /*
                    if ($('#fld_order_delivereddate').val() != '') {
                        $('#div_order_stockitembatch').show();
                    } else {
                        $('#div_order_stockitembatch').hide();
                    }
                    */


                    stockitem_ctr = $('#fld_order_stockitem').val();
                    stockitembatch_ctr = $(tr).find('td').eq(8).data('id');
                    order_ctr = $(tr).attr('id').substring(6);
                    quantity = $('#fld_order_quantity').val();

                    get_stockitembatches(stockitem_ctr, stockitembatch_ctr, order_ctr, quantity);
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_order").dialog({
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
                                tr = $('#div_orders > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_orders > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'order_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                                last_order_date = $('#fld_order_date').val();
                                last_order_reference = $('#fld_order_reference').val();
                            } else {
                                $(tr).find('td:first').attr("class", "changed");
                                last_order_date = "";
                                last_order_reference = "";
                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_order_date').val());
                            $(tr).find('td').eq(2).text($('#fld_order_reference').val());
                            $(tr).find('td').eq(3).text($('#fld_order_stockitem option:selected').text());
                            $(tr).find('td').eq(3).attr('stockitem_ctr', $('#fld_order_stockitem').val());
                            $(tr).find('td').eq(4).text($('#fld_order_grind option:selected').text());
                            $(tr).find('td').eq(4).attr('grind_ctr', $('#fld_order_grind').val());
                            $(tr).find('td').eq(5).text($('#fld_order_quantity').val());
                            $(tr).find('td').eq(6).text($('#fld_order_amount').val());
                            $(tr).find('td').eq(7).text($('#fld_order_delivereddate').val());
                            $(tr).find('td').eq(8).text($('#fld_order_stockitembatch option:selected').text());
                            $(tr).find('td').eq(8).data('id', $('#fld_order_stockitembatch').val());
                            $(tr).find('td').eq(9).text($('#fld_order_invoicereference').val());
                            $(tr).find('td').eq(11).text($('#fld_order_note').val());

                            $(this).dialog("close");
                            $('#form1').areYouSure({ 'addRemoveFieldsMarksDirty': true });
                        }
                    }
                }

                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this order?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                            last_order_date = "";
                            last_order_reference = "";
                            $('#form1').areYouSure({ 'addRemoveFieldsMarksDirty': true });
                        }
                    }
                }

                $("#dialog_order").dialog('option', 'buttons', myButtons);
            })


            $('#fld_order_stockitem').focusin(function () {
                $(this).data('val', $(this).val());
            }).change(function () {
                if ($('#fld_order_delivereddate').val() != "") {
                    var response = confirm("Are you sure you want to change this?\nThere is already a delivery date.");
                    if (response) {
                        stockitem_ctr = $('#fld_order_stockitem').val();
                        stockitembatch_ctr = $(tr).find('td').eq(8).data('id');
                        order_ctr = $(tr).attr('id').substring(6);
                        quantity = $('#fld_order_quantity').val();
                        get_stockitembatches(stockitem_ctr, stockitembatch_ctr, order_ctr, quantity);
                    } else {
                        $('#fld_order_stockitem').val($(this).data('val'));
                    }
                }
                show_div_order_stockitembatch();
            });


            $('#fld_order_quantity').focusin(function () {
                $(this).data('val', $(this).val());
            }).change(function () {
                if ($('#fld_order_delivereddate').val() != "") {
                    var response = confirm("Are you sure you want to change this?\nThere is already a delivery date.");
                    if (response) {
                        stockitem_ctr = $('#fld_order_stockitem').val();
                        stockitembatch_ctr = $(tr).find('td').eq(8).data('id');
                        order_ctr = $(tr).attr('id').substring(6);
                        quantity = $('#fld_order_quantity').val();
                        get_stockitembatches(stockitem_ctr, stockitembatch_ctr, order_ctr, quantity);
                    } else {
                        $('#fld_order_quantity').val($(this).data('val'));
                    }
                }
                show_div_order_stockitembatch();

            });

            $('#fld_order_amount').focusin(function () {
                $(this).data('val', $(this).val());
            }).change(function () {
                if ($('#fld_order_invoicereference').val() != "") {
                    var response = confirm("Are you sure you want to change this?\nThere is already an invoice.");
                    if (response) {
                        stockitem_ctr = $('#fld_order_stockitem').val();
                        stockitembatch_ctr = $(tr).find('td').eq(8).data('id');
                        order_ctr = $(tr).attr('id').substring(6);
                        quantity = $('#fld_order_quantity').val();
                        get_stockitembatches(stockitem_ctr, stockitembatch_ctr, order_ctr, quantity);
                    } else {
                        $('#fld_order_amount').val($(this).data('val'));
                    }
                }
                show_div_order_stockitembatch();
            });


            $('.fld_order_delivereddate').on('dp.change', function (e) {
                //   IF THE VERY FIRST CHANGE IS TO MAKE IT NULL THIS CODE DOES NOT FIRE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //
                //alert(e.date);
                if (!e.date) {
                    //alert("Test");
                    warning = "";
                    if ($('#fld_order_invoicereference').val() != "") {
                        warning = "\nThere is an Invoice Reference!";
                    }
                    var response = confirm("Are you sure you want to remove the delivery date?" + warning);
                    /*
                    if (response) {
                        $('#div_order_stockitembatch').hide();
                        $("#fld_order_stockitembatch").empty();
                    } else {
                        $('#fld_order_delivereddate').val(e.oldDate);
                    }
                    */
                    /*
                    alert('Delivery date has been removed, need to confirm this and then take into account that it has no longer been delivered when compiling stock batches.');
                    stockitem_ctr = $('#fld_order_stockitem').val();
                    stockitembatch_ctr = $(tr).find('td').eq(8).data('id');
                    order_ctr = $(tr).attr('id').substring(6);
                    quantity = $('#fld_order_quantity').val();

                    get_stockitembatches(stockitem_ctr, stockitembatch_ctr, order_ctr, quantity);
                    */
                } else {
                    show_div_order_stockitembatch();
                    //$('#div_order_stockitembatch').show();
                }

            })

            function show_div_order_stockitembatch() {
                if ($('#fld_order_stockitem').val() != '' && $('#fld_order_quantity').val() != '' && $('#fld_order_amount').val() != '') { // && $('#fld_order_delivereddate').val() != '') {
                    stockitem_ctr = $('#fld_order_stockitem').val();
                    stockitembatch_ctr = 0;
                    order_ctr = 0;
                    quantity = $('#fld_order_quantity').val();
                    get_stockitembatches(stockitem_ctr, stockitembatch_ctr, order_ctr, quantity);
                    $('#div_order_stockitembatch').show();
                } else {
                    $('#fld_order_stockitembatch').val('');
                    $('#div_order_stockitembatch').hide();
                }


            }



            function update_order() {

                delim = String.fromCharCode(254);
                $('#orderstable > tbody > tr[maint="changed"]').each(function () {
                    tr_id = $(this).attr('id');
                    tr_date = $(this).find('td:eq(1)').text();
                    tr_reference = $(this).find('td:eq(2)').text();
                    tr_stockitem = $(this).find('td:eq(3)').attr('stockitem_ctr');
                    tr_grind = $(this).find('td:eq(4)').attr('grind_ctr');
                    tr_quantity = $(this).find('td:eq(5)').text();
                    tr_amount = $(this).find('td:eq(6)').text();
                    tr_delivereddate = $(this).find('td:eq(7)').text();
                    tr_stockitembatch = $(this).find('td:eq(8)').data('id');
                    tr_invoicereference = $(this).find('td:eq(9)').text();
                    tr_note = $(this).find('td:eq(11)').text();

                    value = tr_date + delim + tr_reference + delim + tr_stockitem + delim + tr_grind + delim + tr_quantity + delim + tr_amount + delim + tr_delivereddate + delim + tr_stockitembatch + delim + tr_invoicereference + delim + tr_note;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });

                $('#orderstable > tbody > tr[maint="deleted"]').each(function () {
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


            /* ========================================= EDIT SUBSCRIPTIONS ===========================================*/
            $(document).on('click', '.subscriptionedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_subscription").find(':input').val('');
                } else {
                    tr = $(this).closest('tr');
                    $('#fld_subscription_frequency').val($(tr).find('td').eq(1).text());
                    $('#fld_subscription_period').val($(tr).find('td').eq(2).text());
                    $('#fld_subscription_startdate').val($(tr).find('td').eq(3).text());
                    $('#fld_subscription_lastorder').val($(tr).find('td').eq(4).text());
                    //$('#fld_subscription_nextorder').val($(tr).find('td').eq(5).text());
                    $('#fld_subscription_stockitem').val($(tr).find('td').eq(6).attr('stockitem_ctr'));
                    $('#fld_subscription_grind').val($(tr).find('td').eq(7).attr('grind_ctr'));
                    $('#fld_subscription_quantity').val($(tr).find('td').eq(8).text());
                    $('#fld_subscription_amount').val($(tr).find('td').eq(9).text());
                    $('#fld_subscription_dropship').val($(tr).find('td').eq(10).text());
                    $('#fld_subscription_note').val($(tr).find('td').eq(11).text());
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_subscription").dialog({
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
                                tr = $('#div_subscriptions > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_subscriptions > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'subscription_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_subscription_frequency').val());
                            $(tr).find('td').eq(2).text($('#fld_subscription_period').val());
                            $(tr).find('td').eq(3).text($('#fld_subscription_startdate').val());
                            $(tr).find('td').eq(4).text($('#fld_subscription_lastorder').val());
                            //$(tr).find('td').eq(5).text($('#fld_subscription_nextorder').val());
                            $(tr).find('td').eq(5).text("Updated?");
                            $(tr).find('td').eq(6).text($('#fld_subscription_stockitem option:selected').text());
                            $(tr).find('td').eq(6).attr('stockitem_ctr', $('#fld_subscription_stockitem').val());
                            $(tr).find('td').eq(7).text($('#fld_subscription_grind option:selected').text());
                            $(tr).find('td').eq(7).attr('grind_ctr', $('#fld_subscription_grind').val());
                            $(tr).find('td').eq(8).text($('#fld_subscription_quantity').val());
                            $(tr).find('td').eq(9).text($('#fld_subscription_amount').val());
                            $(tr).find('td').eq(10).text($('#fld_subscription_dropship option:selected').text());
                            $(tr).find('td').eq(11).text($('#fld_subscription_note').val());

                            $(this).dialog("close");
                            $('#form1').areYouSure({ 'addRemoveFieldsMarksDirty': true });
                        }
                    }
                }

                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this subscription?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                            $('#form1').areYouSure({ 'addRemoveFieldsMarksDirty': true });
                        }
                    }
                }

                $("#dialog_subscription").dialog('option', 'buttons', myButtons);
            })

            function update_subscription() {
                /*----------------------------------------------SUBSCRIPTIONS-----------------------------------------*/
                delim = String.fromCharCode(254);
                $('#subscriptionstable > tbody > tr[maint="changed"]').each(function () {
                    tr_id = $(this).attr('id');
                    tr_frequency = $(this).find('td:eq(1)').text();
                    tr_period = $(this).find('td:eq(2)').text();
                    tr_startdate = $(this).find('td:eq(3)').text();
                    tr_lastorder = $(this).find('td:eq(4)').text();
                    //tr_nextorder = $(this).find('td:eq(5)').text();
                    tr_stockitem = $(this).find('td:eq(6)').attr('stockitem_ctr');
                    tr_grind = $(this).find('td:eq(7)').attr('grind_ctr');
                    tr_quantity = $(this).find('td:eq(8)').text();
                    tr_amount = $(this).find('td:eq(9)').text();
                    tr_dropship = $(this).find('td:eq(10)').text();
                    tr_note = $(this).find('td:eq(11)').text();
                    value = tr_frequency + delim + tr_period + delim + tr_startdate + delim + tr_lastorder + delim + tr_stockitem + delim + tr_grind + delim + tr_quantity + delim + tr_amount + delim + tr_dropship + delim + tr_note;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });

                $('#subscriptionstable > tbody > tr[maint="deleted"]').each(function () {
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

            function get_stockitembatches(stockitem_ctr, stockitembatch_ctr, order_ctr) {
                $.post("/_Dependencies/data.aspx", { mode: "get_stockitembatches", stockitem_ctr: stockitem_ctr, stockitembatch_ctr: stockitembatch_ctr, order_ctr: order_ctr, quantity: quantity })
                    .done(function (data) {
                        $("#fld_order_stockitembatch").empty().append(data);
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

    <div id="dialog_fullphoto" title="Full Photo" style="display: none">
        <img id="fullphoto" />
    </div>
    <div class="toprighticon">
        <input type="button" id="search" class="btn btn-info" value="Search" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <div class="bottomrighticon">
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />
    </div>
    <h1>Person Maintenance
    </h1>
    <div class="form-horizontal row">
        <div class="col-md-8">
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_firstname">First name</label>
                <div class="col-md-6">
                    <input id="fld_firstname" name="fld_firstname" type="text" class="form-control" value="<%:fld_firstname%>" maxlength="50" />
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_surname">Surname</label>
                <div class="col-md-6">
                    <input id="fld_surname" name="fld_surname" type="text" class="form-control" value="<%:fld_surname%>" maxlength="50" />
                </div>
            </div>

        </div>
        <div class="col-md-4">
            <img id="img_photo" alt="" src="Images/<%: person_ctr %>.jpg" style="height: 200px" /><br />
            <a id="getphoto">Upload Photo</a> <%=fld_xxxxxx %>
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
                <!--<h3 class="tabheading">General</h3>-->

                <div class="form-group row">
                    <label class="control-label col-sm-4" for="fld_persontype">Type</label>
                    <div class="col-sm-8">
                        <select id="fld_persontype" name="fld_persontype" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <% 
                                Dictionary<string, string> persontypeoptions = new Dictionary<string, string>();
                                persontypeoptions["type"] = "select";
                                persontypeoptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(persontypes, fld_persontype, persontypeoptions));
                            %>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_gender">Gender</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_gender" name="fld_gender" class="form-control" value="<%: fld_gender %>" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_dateofbirth">Date of Birth</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_dateofbirth" name="fld_dateofbirth" class="form-control" value="<%: fld_dateofbirth %>" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_greeting">Greeting</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_greeting" name="fld_greeting" class="form-control" value="<%: fld_greeting %>" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_emailaddress">Email Address</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_emailaddress" name="fld_emailaddress" class="form-control" value="<%: fld_emailaddress %>" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_deliveryaddress">Address</label>
                    <div class="col-sm-8">
                        <textarea id="fld_deliveryaddress" name="fld_deliveryaddress" rows="6" class="form-control"><%: fld_address %></textarea>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_note" name="fld_note" rows="6" class="form-control"><%: fld_note %></textarea>
                    </div>
                </div>
            </div>





            <!-- ================================= ATTENDANCE TAB ===================================  -->
            <div id="div_attendance" class="tab-pane fade in">
                <!--<h3 class="tabheading">Attendance</h3>-->
                <table id="attendancetable" class="table" style="width: 100%">
                    <%= html_attendance %>
                </table>
            </div>

            <!-- ================================= ATTENDANCE DIALOG ===================================  -->

            <div id="dialog_attendance" title="Maintain Attendance" style="display: none" class="form-horizontal">
                <input type="hidden" id="fld_order_ctr" name="fld_order_ctr" />
                <div class="form-group">
                    <label for="fld_order_date" class="control-label col-sm-4">
                        Date
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_order_date" name="fld_order_date" required="required" placeholder="eg: 23 Jun 1985" type="text" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_order_reference">Reference</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_order_reference" name="fld_order_reference" maxlength="20" class="form-control" />
                    </div>
                </div>



                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_order_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_order_note" name="fld_order_note" class="form-control tinymce"></textarea>
                    </div>
                </div>
            </div>

            <!-- ================================= NOTES TAB ===================================  -->
            <div id="div_notes" class="tab-pane fade in">
                <!--<h3 class="tabheading">Notes</h3>-->
                <table id="notestable" class="table" style="width: 100%">
                    <%= html_notes %>
                </table>
            </div>

            <!-- ================================= NOTES DIALOG ===================================  -->

            <div id="dialog_notes" title="Maintain Notes" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label for="fld_note_date" class="control-label col-sm-4">
                        Start Date
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_note_date" name="fld_note_date" required="required" placeholder="eg: 23 Jun 1985" type="text" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>



                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_note_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_note_note" name="fld_note_note" class="form-control tinymce"></textarea>
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
