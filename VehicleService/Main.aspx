<%@ Page Title="" Language="C#" MasterPageFile="~/onePage.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="VehicleService.Main1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <script src='//cdn.tinymce.com/4/tinymce.min.js'></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.AreYouSure/1.9.0/jquery.are-you-sure.min.js"></script>

    <style>
        h1, h2, h3, h4 {
            display: inline;
        }

        input[required], select[required], textarea[required] {
            border: 3px solid
        }

        .customer_name, vehicle_description {
            background-color:lightskyblue;
            color: white;
        }
    </style>


    <script type="text/javascript">
        var newctr = 0;

        $(document).ready(function () {
            //Generic.Functions.googleanalyticstracking()%>

            //function makedirty() {
            ////$('#hidden_dirty').addClass('dirty');
            //$('#form1').addClass('dirty');
            //}
            


            customer_ctr = "<%=customer_ctr%>";
            if (customer_ctr != "") {
                load_customer(customer_ctr);
                customer_vehicle_ctr = "<%=customer_vehicle_ctr%>";
                if (customer_vehicle_ctr != "") {
                    load_vehicle(customer_vehicle_ctr);
                    $('#section_vehicle').show();
                } else {
                    $('#section_customer').show();
                }
            }

            $('#assistance').click(function () {
                $("#dialog_assistance").dialog({
                resizable: false,
                    height: 600,
                    width: 800,
                    modal: true
                });
        })

            $('#nzta').click(function () {
            copytoClipboard($('#vehicle_registration').val());
            myWindow = window.open("https://transact.nzta.govt.nz/transactions/CheckExpiry/entry", "NZTA", "width=800,height=1000");
        })

            $('#makemodelrefresh').click(function () {
            alert('to do');
        })

            $('#vehicle_followup_actioneddate').change(function () {
            if ($(this).val() != "") {
                    $('#vehicle_followup_actioneddetail').prop('required', true);
            } else {
                    $('#vehicle_followup_actioneddetail').prop('required', false);
            }
        })

            $('#vehicle_followup_actioneddetail').change(function () {
            if ($(this).val() != "") {
                    $('#vehicle_followup_actioneddate').prop('required', true);
            } else {
                    $('#vehicle_followup_actioneddate').prop('required', false);
            }
        })

            $('#menu').click(function () {
            window.location.href = "<%=ResolveUrl("~/default.aspx")%>";
            });

            $('.key').click(function () {
                lockable = $(this).parent().parent().find('.lockable');
                disabled = $(lockable).prop('disabled');
                if (disabled == true) {
                    $(lockable).prop('disabled', false);
                } else {
                    $(lockable).prop('disabled', true);
                }
            })

            $('#customersearch').click(function () {
                $('#section_customer').hide();
                $('#section_customersearch').show();

                //window.location.href = "<%=ResolveUrl("~/customersearch.aspx")%>";
            });

            $("#customersearch_name").autocomplete({
                source: "<%: ResolveUrl("~/_Dependencies/data.asmx/Customer_name_autocomplete?options=Allow Create")%>",
                minLength: 2,
                select: function (event, ui) {
                    event.preventDefault();
                    load_customer(ui.item.customer_ctr);
                    $('#section_customer').show();

                }
            })

            $('#vehicle_registration').change(function () {
                $('#vehicle_registration').val($('#vehicle_registration').val().toUpperCase());
            });

            $(document).on('click', '.vehicleedit', function () {
                customer_vehicle_ctr = $(this).attr('link');
                $('#customer_vehicle_ctr').val(customer_vehicle_ctr);
                $('#section_customer').hide();
                if (customer_vehicle_ctr == 'new') {
                    $('#vehicle_registration').prop('disabled', false);
                    $('#vehicle_ctr').val('new');
                    $('#vehicle_registration').val('');
                    $('#vehicle_description').val('');
                    $('#vehicle_wof_cycle').val('');
                    $('#vehicle_wof_due').val('');
                    $('#vehicle_registration_due').val('');
                    $('#vehicle_year').val('');
                    $('#vehicle_odometer').val('');
                    $('#vehicle_vehiclemodel').val('');
                    $('#vehicle_note').val('');
                } else {
                    load_vehicle(customer_vehicle_ctr); //NEW
                    /*
                    $('#vehicle_registration').prop('disabled', true);
                    $.getJSON("/_Dependencies/data.asmx/get_customer_vehicle?id=" + customer_vehicle_ctr, function (data) {
                        $('#vehicle_ctr').val(data.vehicle_ctr);
                        $('#vehicle_registration').val(data.registration);
                        $('#vehicle_description').val(data.description);
                        $('#vehicle_wof_cycle').val(data.wof_cycle);
                        $('#vehicle_wof_due').val(data.wof_due);
                        $('#vehicle_registration_due').val(data.registration_due);
                        $('#vehicle_year').val(data.year);
                        $('#vehicle_odometer').val(data.odometer);
                        $('#vehicle_vehiclemodel').val(data.vehiclemodel);
                        $('#vehicle_vehicletype').val(data.vehicletype);
                        $('#vehicle_note').val(data.vehiclenote);
                        get_vehicle_activities($('#customer_ctr').val(), $('#vehicle_ctr').val());
                        get_vehicle_followups($('#customer_ctr').val(), $('#vehicle_ctr').val());
                    });
                    */
                }
                $('#section_vehicle').show();
            });

            $(document).on('click', '.vehicle_activityedit', function () {
                vehicle_activity_ctr = $(this).attr('link');
                $('#vehicle_activity_ctr').val(vehicle_activity_ctr);
                $('#section_vehicle').hide();
                if (vehicle_activity_ctr == 'new') {
                    $('#vehicle_activity_ctr').val('new');
                    $('#vehicle_activity_date').val('');
                    $('#vehicle_activity_detail').val('');
                } else {
                    $.getJSON("/_Dependencies/data.asmx/get_vehicle_activity?id=" + vehicle_activity_ctr, function (data) {
                        //$('#vehicle_activity_ctr').val(data.vehicle_activity_ctr);
                        $('#vehicle_activity_date').val(data.date);
                        $('#vehicle_activity_detail').val(data.detail);
                    });
                }

                $('.vehicle_description').html($('#vehicle_registration').val() + ' - ' + $('#vehicle_vehiclemodel option:selected').text() + ' - ' + $('#vehicle_vehicletype option:selected').text());
                $('#section_vehicle_activity').show();
            });

            $(document).on('click', '.vehicle_followupedit', function () {
                vehicle_followup_ctr = $(this).attr('link');
                $('#vehicle_followup_ctr').val(vehicle_followup_ctr);
                $('#section_vehicle').hide();
                if (vehicle_followup_ctr == 'new') {
                    $('#vehicle_followup_ctr').val('new');
                    $('#vehicle_followup_entrydate').val('');
                    $('#vehicle_followup_detail').val('');
                    $('#vehicle_followup_followupdate').val('');
                    $('#vehicle_followup_actioneddate').val('');
                    $('#vehicle_followup_actioneddetail').val('');
                } else {
                    $.getJSON("/_Dependencies/data.asmx/get_vehicle_followup?id=" + vehicle_followup_ctr, function (data) {
                        //$('#vehicle_followup_ctr').val(data.vehicle_followup_ctr);
                        $('#vehicle_followup_entrydate').val(data.entrydate);
                        $('#vehicle_followup_detail').val(data.detail);
                        $('#vehicle_followup_followupdate').val(data.followupdate);
                        $('#vehicle_followup_actioneddate').val(data.actioneddate);
                        $('#vehicle_followup_actioneddetail').val(data.actioneddetail);
                    });
                }

                $('.vehicle_description').html($('#vehicle_registration').val() + ' - ' + $('#vehicle_vehiclemodel option:selected').text() + ' - ' + $('#vehicle_vehicletype option:selected').text());
                $('#section_vehicle_followup').show();
            });


            form_customer = $("#form_customer").validate();
            form_vehicle = $("#form_vehicle").validate();
            form_vehicle_activity = $("#form_vehicle_activity").validate();
            form_vehicle_followup = $("#form_vehicle_followup").validate();

            $('.date').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true
                //,maxDate: moment().add(-1, 'year')
            });

            $(document).on('click', '.send_text', function () {
                phonenumber = $(this).closest('tr').find('td').eq(1).text();
                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }
                $("#dialog-sendtext").dialog({
                    resizable: false,
                    height: 250,
                    width: mywidth,
                    modal: true,
                    buttons: {
                        "Cancel": function () {
                            $(this).dialog("close");
                        },
                        "Send": function () {
                            $.post("posts.asmx/send_text", { PhoneNumber: phonenumber, Message: $('#tb_textmessage').val() }, function (data) {
                                alert(data);
                            },
                                'html'
                            );
                            $(this).dialog("close");
                        }
                    }
                });
            })

            $(document).on('click', '.send_email_system', function () {
                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }
                $("#dialog-sendemail").dialog({
                    resizable: false,
                    height: 400,
                    width: mywidth,
                    modal: true,
                    buttons: {
                        "Cancel": function () {
                            $(this).dialog("close");
                        },
                        "Send": function () {
                            alert("to do - send to: " + $('td:eq(1)', $(this).parents('tr')).text());
                            $(this).dialog("close");
                        }
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

            $('#btn_customersubmit').click(function () {
                if ($("#form_customer").valid()) {
                //if (form_customer.valid()) {
                    //$('#section_vehicle_activity').hide();
                    var arForm = $("#form_customer")
                        .find("input,textarea,select,hidden")
                        .not("[id^='__']")
                        .serializeArray();

                    //arForm.push({ name: 'vehicle_ctr', value: $('#vehicle_ctr').val() });
                    var formData = JSON.stringify({ formVars: arForm });

                    $.ajax({
                        type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                        url: '/_dependencies/posts.asmx/update_customer', // the url where we want to POST
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
                    $('#section_customer').hide();
                    $('#section_customersearch').show();
                    //get_vehicle_activities($('#customer_ctr').val(), $('#vehicle_ctr').val());
                    //$('#section_vehicle').show();
                }
            });

            $('#btn_customercancel').click(function () {
                form_customer.resetForm();
                $('#section_customer').hide();
                $('#section_customersearch').show();
            });

            $('#btn_vehiclecancel').click(function () {
                form_vehicle.resetForm();
                $('#section_vehicle').hide();
                $('#section_customer').show();
            });

            $('#btn_vehicle_activitysubmit').click(function () {
                if ($("#form_vehicle_activity").valid()) {
                    $('#section_vehicle_activity').hide();
                    var arForm = $("#form_vehicle_activity")
                        .find("input,textarea,select,hidden")
                        .not("[id^='__']")
                        .serializeArray();

                    arForm.push({ name: 'vehicle_ctr', value: $('#vehicle_ctr').val() });
                    var formData = JSON.stringify({ formVars: arForm });

                    $.ajax({
                        type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                        url: '/_dependencies/posts.asmx/update_vehicle_activity', // the url where we want to POST
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
                    get_vehicle_activities($('#customer_ctr').val(), $('#vehicle_ctr').val());
                    $('#section_vehicle').show();
                }
            });

            $('#btn_vehicle_activitycancel').click(function () {
                form_vehicle_activity.resetForm();
                $('#section_vehicle_activity').hide();
                $('#section_vehicle').show();
            });



            $('#btn_vehicle_followupsubmit').click(function () {
                if ($("#form_vehicle_followup").valid()) {
                    $('#section_vehicle_followup').hide();
                    var arForm = $("#form_vehicle_followup")
                        .find("input,textarea,select,hidden")
                        .not("[id^='__']")
                        .serializeArray();

                    arForm.push({ name: 'vehicle_ctr', value: $('#vehicle_ctr').val() });
                    var formData = JSON.stringify({ formVars: arForm });

                    $.ajax({
                        type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                        url: '/_dependencies/posts.asmx/update_vehicle_followup', // the url where we want to POST
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
                    get_vehicle_followups($('#customer_ctr').val(), $('#vehicle_ctr').val());
                    $('#section_vehicle').show();
                }
            });

            $('#btn_vehicle_followupdelete').click(function () {
                if (confirm('Are you sure you would like to delete this followup record?')) {
                    $('#section_vehicle_followup').hide();
                    var arForm = [{ name: 'vehicle_followup_ctr', value: $('#vehicle_followup_ctr').val() }];
                    var formData = JSON.stringify({ formVars: arForm });
                    $.ajax({
                        type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                        url: '/_dependencies/posts.asmx/delete_vehicle_followup', // the url where we want to POST
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

                    get_vehicle_followups($('#customer_ctr').val(), $('#vehicle_ctr').val());
                    $('#section_vehicle').show();
                }
            });


            $('#btn_vehicle_followupcancel').click(function () {
                form_vehicle_followup.resetForm();
                $('#section_vehicle_followup').hide();
                $('#section_vehicle').show();
            });

            $('#btn_vehiclesubmit').click(function () {
                $('#vehicle_registration').prop('disabled', false);
                if ($("#form_vehicle").valid()) {
                    $('#section_vehicle').hide();
                    var arForm = $("#form_vehicle")
                        .find("input,textarea,select,hidden")
                        .not("[id^='__']")
                        .serializeArray();

                    arForm.push({ name: 'customer_ctr', value: $('#customer_ctr').val() });
                    var formData = JSON.stringify({ formVars: arForm });
                    $.ajax({
                        type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                        url: '/_dependencies/posts.asmx/update_vehicle', // the url where we want to POST
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
                    customer_ctr = $('#customer_ctr').val();
                    get_customer_vehicles(customer_ctr);

                    $('#section_customer').show();
                }
            });
        }); //document.ready

        function load_customer(customer_ctr) {
            $('#customersearch_name').val("");
            $('#section_customersearch').hide();
            $('#customer_ctr').val(customer_ctr);
            $.getJSON("/_Dependencies/data.asmx/get_customer?id=" + customer_ctr, function (data) {
                $('#customer_name').val(data.name);
                $('.customer_name').html(data.displayname);
                $('#customer_firstname').val(data.firstname);
                $('#customer_surname').val(data.surname);
                $('#customer_knownas').val(data.knownas);
                $('#customer_address').val(data.address);
                $('#customer_customertype').val(data.customertype_ctr);
                $('#customer_emailaddress').val(data.emailaddress);
                $('#customer_mobilephone').val(data.mobilephone);
                $('#customer_homephone').val(data.homephone);
                $('#customer_workphone').val(data.workphone);
                $('#customer_note').val(data.note);
                $('#customer_guid').val(data.guid);
            });
            get_customer_vehicles(customer_ctr);
        }

        function load_vehicle(customer_vehicle_ctr) {
            $('#vehicle_registration').prop('disabled', true);
            $.getJSON("/_Dependencies/data.asmx/get_customer_vehicle?id=" + customer_vehicle_ctr, function (data) {
                $('#vehicle_ctr').val(data.vehicle_ctr);
                $('#vehicle_registration').val(data.registration);
                $('#vehicle_description').val(data.description);
                $('#vehicle_wof_cycle').val(data.wof_cycle);
                $('#vehicle_wof_due').val(data.wof_due);
                $('#vehicle_registration_due').val(data.registration_due);
                $('#vehicle_year').val(data.year);
                $('#vehicle_odometer').val(data.odometer);
                $('#vehicle_vehiclemodel').val(data.vehiclemodel);
                $('#vehicle_vehicletype').val(data.vehicletype);
                $('#vehicle_note').val(data.vehiclenote);
                get_vehicle_activities($('#customer_ctr').val(), $('#vehicle_ctr').val());
                get_vehicle_followups($('#customer_ctr').val(), $('#vehicle_ctr').val());
            });
        }

        function get_customer_vehicles(customer_ctr) {
            $.post("/_Dependencies/data.aspx", { mode: "get_customer_vehicles", customer_ctr: customer_ctr })
                .done(function (data) {
                    $("#div_vehicles").html(data);
                });
        }

        function get_vehicle_activities(customer_ctr, vehicle_ctr) {
            $.post("/_Dependencies/data.aspx", { mode: "get_vehicle_activities", customer_ctr: customer_ctr, vehicle_ctr: vehicle_ctr })
                .done(function (data) {
                    $("#div_vehicleactivities").html(data);
                });
        }
        function get_vehicle_followups(customer_ctr, vehicle_ctr) {
            $.post("/_Dependencies/data.aspx", { mode: "get_vehicle_followups", customer_ctr: customer_ctr, vehicle_ctr: vehicle_ctr })
                .done(function (data) {
                    $("#div_vehiclefollowups").html(data);
                });
        }

   

        function copytoClipboard(textToCopy) {

            navigator.clipboard.writeText(textToCopy)
                .then(() => {  })
                .catch((error) => { alert('Copy failed! ${error}') });


        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="section_customersearch">
        <h2>Customer Search
        </h2>
        <hr />
        <div class="form-horizontal">
            <div class="form-group">
                <label class="control-label col-sm-4" for="customersearch_name">Name / Registration</label>
                <div class="col-sm-8">
                    <input id="customersearch_name" name="customersearch_name" type="text" class="form-control" />
                </div>
            </div>
        </div>
    </div>

    <div id="section_customer" style="display: none">
        <form id="form_customer">
            <input type="hidden" id="customer_ctr" name="customer_ctr" />
            <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
                <p></p>
            </div>

            <div class="toprighticon">
                <input type="button" id="customersearch" class="btn btn-info" value="Search" />
                <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
                <input type="button" id="menu" class="btn btn-info" value="MENU" />
            </div>
            <div class="bottomrighticon">
                <input id="btn_customercancel" type="button" class="btn btn-info" value="Cancel" />
                <input id="btn_customersubmit" type="button" class="btn btn-info" value="Submit" />
            </div>
            <h2>Customer Maintenance : </h2>
            <h3 class="customer_name"></h3>
            <hr />


            <div class="form-horizontal">
                <!--
                 <h3 class="customer_name"></h3>
               
                <div class="form-group">
                    <label class="control-label col-sm-4" for="customer_name">Name</label>
                    <div class="col-sm-8">
                        <input type="text" id="customer_name" name="customer_name" class="form-control" required />
                    </div>

                </div>
                    -->
                <!------------------------------------------ TABS ------------------------------------------------------------>

                <div class="form-horizontal">
                    <ul class="nav nav-tabs">
                        <li class="active"><a data-target="#div_general">General</a></li>
                        <li><a data-target="#div_note">Note</a></li>
                        <li><a data-target="#div_vehicles">Vehicles</a></li>
                    </ul>
                    <div class="tab-content">
                        <!-- ================================= GENERAL TAB ===================================  -->
                        <div id="div_general" class="tab-pane fade in active">
                            <!--<h3 class="navbar"><span class="navbar-brand">General</span></h3>-->
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_name">Organisation Name</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_name" name="customer_name" class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_firstname">First name</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_firstname" name="customer_firstname" class="form-control" required />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_surname">Surname</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_surname" name="customer_surname" class="form-control" required />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_knownas">Known as</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_knownas" name="customer_knownas" class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_customertype">Customer type</label>
                                <div class="col-sm-4">
                                    <select id="customer_customertype" name="customer_customertype" class="form-control" required="required">
                                        <option value="">--- Please select ---</option>
                                        <% 
                                            Dictionary<string, string> customertypesoptions = new Dictionary<string, string>();
                                            customertypesoptions["type"] = "select";
                                            customertypesoptions["valuefield"] = "value";
                                            Response.Write(Generic.Functions.buildselection(customertypes, nooptions, customertypesoptions));
                                        %>
                                    </select>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_address">Address</label>
                                <div class="col-sm-8">
                                    <textarea id="customer_address" name="customer_address" rows="5" class="form-control"></textarea>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_emailaddress">Email address</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_emailaddress" name="customer_emailaddress" class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_mobilephone">Mobile phone</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_mobilephone" name="customer_mobilephone" class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_homephone">Home phone</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_homephone" name="customer_homephone" class="form-control" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_workphone">Work phone</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_workphone" name="customer_workphone" class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_guid">GUID</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_guid" disabled="disabled" class="form-control" />
                                    
                                </div>
                            </div>
                        </div>
                        <!-- ================================= NOTE TAB ===================================  -->
                        <div id="div_note" class="tab-pane fade in">
                            <!--<h3 class="tabheading">Note</h3>-->
                            <div class="form-group">
                                <!--<label class="control-label col-sm-4" for="customer_note">Note</label>-->
                                <div class="col-sm-12">
                                    <textarea id="customer_note" name="customer_note" rows="10" class="form-control"></textarea>
                                </div>
                            </div>
                        </div>
                        <!-- ================================= VEHICLES TAB ===================================  -->
                        <div id="div_vehicles" class="tab-pane fade in">
                        </div>


                        <!-- ================================= END OF TABS ===================================  -->
                    </div>
                </div>
            </div>
        </form>
    </div>

    <div id="section_vehicle" style="display: none">
        <form id="form_vehicle">
            <input type="hidden" id="vehicle_ctr" name="vehicle_ctr" />
            <input type="hidden" id="customer_vehicle_ctr" name="customer_vehicle_ctr" />
            <div class="toprighticon">
                <input type="button" id="vehicleassistance" class="btn btn-info" value="Assistance" />
            </div>
            <div class="bottomrighticon">
                <input type="button" id="btn_vehiclecancel" class="btn btn-info" value="Cancel" />
                <input type="button" id="btn_vehiclesubmit" class="btn btn-info" value="Submit" />
            </div>
            <h2>Vehicle Maintenance
            </h2>

            <h3 class="customer_name"></h3>
            <hr />
            <div class="row">
                <div class="col-sm-4 form-group">
                    <label>Registration</label>
                    <div class="input-group">
                        <input type="text" id="vehicle_registration" name="vehicle_registration" class="form-control lockable" disabled="disabled" maxlength="6" required /><span class="input-group-addon"><img src="_Dependencies/Images/key.png" class="key locked" /></span>
                    </div>
                </div>
                <div class="col-sm-4 form-group">
                    <label>Make/Model</label>
                    <select id="vehicle_vehiclemodel" name="vehicle_vehiclemodel" class="form-control" required="required">
                        <option value="">--- Please select ---</option>
                        <% 
                            Dictionary<string, string> vehiclemakemodeloptions = new Dictionary<string, string>();
                            vehiclemakemodeloptions["type"] = "select";
                            vehiclemakemodeloptions["valuefield"] = "value";
                            Response.Write(Generic.Functions.buildselection(vehicleakemodels, nooptions, vehiclemakemodeloptions));
                        %>
                    </select><a href="MakeModelMaintenance.aspx" target="makemodel">Maintain</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a id="makemodelrefresh">Refresh</a>
                </div>
                <div class="col-sm-4 form-group">
                    <label>Type</label>

                    <select id="vehicle_vehicletype" name="vehicle_vehicletype" class="form-control" required="required">
                        <option value="">--- Please select ---</option>
                        <% 
                            Dictionary<string, string> vehicletypeoptions = new Dictionary<string, string>();
                            vehicletypeoptions["type"] = "select";
                            vehicletypeoptions["valuefield"] = "value";
                            Response.Write(Generic.Functions.buildselection(vehicletypes, nooptions, vehicletypeoptions));
                        %>
                    </select>
                </div>
            </div>

            <!------------------------------------------ TABS ------------------------------------------------------------>

            <div class="form-horizontal">
                <ul class="nav nav-tabs">
                    <li class="active"><a data-target="#div_vehiclegeneral">General</a></li>
                    <li><a data-target="#div_vehiclenote">Note</a></li>
                    <li><a data-target="#div_vehicleactivities">Activity</a></li>
                    <li><a data-target="#div_vehiclefollowups">Followup</a></li>
                </ul>
                <div class="tab-content">
                    <!-- ================================= GENERAL TAB ===================================  -->
                    <div id="div_vehiclegeneral" class="tab-pane fade in active">
                        <!--<h3 class="navbar"><span class="navbar-brand">General</span></h3>-->
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="vehicle_description">Description</label>
                            <div class="col-sm-8">
                                <textarea id="vehicle_description" rows="6" name="vehicle_description" class="form-control"></textarea>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="vehicle_wof_cycle">WOF Cycle</label>
                            <div class="col-sm-8">
                                <select id="vehicle_wof_cycle" name="vehicle_wof_cycle" class="form-control">
                                    <option value="">--- Please select ---</option>
                                    <option value="6">6 monthly</option>
                                    <option value="12">Annualy</option>
                                    <option value="0">N/A</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="vehicle_wof_due" class="control-label col-sm-4">WOF Due</label>
                            <div class="col-sm-8">
                                <div class="input-group date" id="div_vehicle_wof_due">
                                    <input id="vehicle_wof_due" name="vehicle_wof_due" class="form-control">
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                   
                                </div>
                                 <a id="nzta">NZTA</a>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="vehicle_registration_due" class="control-label col-sm-4">Registration Due</label>
                            <div class="col-sm-8">
                                <div class="input-group date" id="div_vehicle_registration_due">
                                    <input id="vehicle_registration_due" name="vehicle_registration_due" class="form-control">
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="vehicle_odometer">Odometer</label>
                            <div class="col-sm-8">
                                <input type="text" id="vehicle_odometer" name="vehicle_odometer" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="vehicle_year">Year</label>
                            <div class="col-sm-8">
                                <input type="text" id="vehicle_year" name="vehicle_year" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <!-- ================================= NOTE TAB ===================================  -->

                    <div id="div_vehiclenote" class="tab-pane fade in">
                        <!--<h3 class="tabheading">Note</h3>-->
                        <div class="form-group">
                            <!--<label class="control-label col-sm-4" for="vehicle_note">Note</label>-->
                            <div class="col-sm-12">
                                <textarea id="vehicle_note" name="vehicle_note" rows="10" class="form-control"></textarea>
                            </div>
                        </div>
                    </div>
                    <!-- ================================= ACTIVITY TAB ===================================  -->
                    <div id="div_vehicleactivities" class="tab-pane fade in">
                    </div>
                    <!-- ================================= FOLLOWUP TAB ===================================  -->
                    <div id="div_vehiclefollowups" class="tab-pane fade in">
                    </div>
                    <!-- ================================= END OF TABS ===================================  -->
                </div>
            </div>
        </form>
    </div>
    <!--section vehicle maintenance-->

    <div id="section_vehicle_activity" style="display: none">
        <form id="form_vehicle_activity">
            <input type="hidden" id="vehicle_activity_ctr" name="vehicle_activity_ctr" />
            <div class="toprighticon">
                <input type="button" id="vehicle_activityassistance" class="btn btn-info" value="Assistance" />
            </div>
            <div class="bottomrighticon">
                <input type="button" id="btn_vehicle_activitycancel" class="btn btn-info" value="Cancel" />
                <input type="button" id="btn_vehicle_activitysubmit" class="btn btn-info" value="Submit" />
            </div>
            <h2>Vehicle Activity
            </h2>
            <h3 class="customer_name"></h3>
            <h4 class="vehicle_description"></h4>
            <hr />
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="vehicle_activity_date" class="control-label col-sm-4">Date</label>
                    <div class="col-sm-8">
                        <div class="input-group date" id="div_vehicle_activity_date">
                            <input id="vehicle_activity_date" name="vehicle_activity_date" required class="form-control">
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="vehicle_activity_detail">Detail</label>
                    <div class="col-sm-8">
                        <textarea id="vehicle_activity_detail" rows="10" name="vehicle_activity_detail" class="form-control" required></textarea>
                    </div>
                </div>
            </div>
        </form>
    </div>
    <!--section vehicle_activity maintenance-->

    <div id="section_vehicle_followup" style="display: none">
        <form id="form_vehicle_followup">
            <input type="hidden" id="vehicle_followup_ctr" name="vehicle_followup_ctr" />
            <div class="toprighticon">
                <input type="button" id="vehicle_followupassistance" class="btn btn-info" value="Assistance" />
            </div>
            <div class="bottomrighticon">
                <input type="button" id="btn_vehicle_followupcancel" class="btn btn-info" value="Cancel" />
                <input type="button" id="btn_vehicle_followupdelete" class="btn btn-info" value="Delete" />
                <input type="button" id="btn_vehicle_followupsubmit" class="btn btn-info" value="Submit" />
            </div>
            <h2>Vehicle followup
            </h2>
            <h3 class="customer_name"></h3>
            <h4 class="vehicle_description"></h4>
            <hr />
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="vehicle_followup_entrydate" class="control-label col-sm-4">Entry Date</label>
                    <div class="col-sm-8">
                        <div class="input-group date" id="div_vehicle_followup_entrydate">
                            <input id="vehicle_followup_entrydate" name="vehicle_followup_entrydate" required class="form-control">
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label for="vehicle_followup_followupdate" class="control-label col-sm-4">Followup Date</label>
                    <div class="col-sm-8">
                        <div class="input-group date" id="div_vehicle_followup_followupdate">
                            <input id="vehicle_followup_followupdate" name="vehicle_followup_followupdate" required class="form-control">
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="vehicle_followup_detail">Detail</label>
                    <div class="col-sm-8">
                        <textarea id="vehicle_followup_detail" rows="10" name="vehicle_followup_detail" class="form-control" required></textarea>
                    </div>
                </div>

                <div class="form-group">
                    <label for="vehicle_followup_actioneddate" class="control-label col-sm-4">Actioned Date</label>
                    <div class="col-sm-8">
                        <div class="input-group date" id="div_vehicle_followup_actioneddate">
                            <input id="vehicle_followup_actioneddate" name="vehicle_followup_actioneddate" class="form-control">
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="vehicle_followup_actioneddetail">Actioned Detail</label>
                    <div class="col-sm-8">
                        <textarea id="vehicle_followup_actioneddetail" rows="10" name="vehicle_followup_actioneddetail" class="form-control"></textarea>
                    </div>
                </div>


            </div>
        </form>
    </div>
    <!--section vehicle_followup maintenance-->


</asp:Content>

