<%@ Page Title="" Language="C#" MasterPageFile="~/onePage.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="VehicleService.Main1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <script src='//cdn.tinymce.com/4/tinymce.min.js'></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.AreYouSure/1.9.0/jquery.are-you-sure.min.js"></script>


    <script type="text/javascript">
        var newctr = 0;

        $(document).ready(function () {
            //Generic.Functions.googleanalyticstracking()%>

            //function makedirty() {
                ////$('#hidden_dirty').addClass('dirty');
                //$('#form1').addClass('dirty');
            //}
            $('#assistance').click(function () {
                $("#dialog_assistance").dialog({
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true
                });
            })

            $('#menu').click(function () {
                window.location.href = "<%=ResolveUrl("~/default.aspx")%>";
            });

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
                    $('#customersearch_name').val("");
                    customer_ctr = ui.item.customer_ctr;
                    $('#customer_ctr').val(customer_ctr);
                    $('#section_customersearch').hide();
                    $.getJSON("/_Dependencies/data.asmx/get_customer?id=" + customer_ctr, function (data) {
                        $('#customer_name').val(data.name);
                        $('.customer_name').html(data.name);
                    });
                    get_customer_vehicles(customer_ctr);
                    $('#section_customer').show();
                }
            })


            $(document).on('click', '.vehicleedit', function () {
                customer_vehicle_ctr = $(this).attr('link');
                $('#customer_vehicle_ctr').val(customer_vehicle_ctr);
                $('#section_customer').hide();
                if (customer_vehicle_ctr == 'new') {
                    $('#vehicle_ctr').val('new');
                    $('#vehicle_registration').val('');
                    $('#vehicle_description').val('');
                    $('#vehicle_note').val('');
                } else {
                    $.getJSON("/_Dependencies/data.asmx/get_customer_vehicle?id=" + customer_vehicle_ctr, function (data) {
                        $('#vehicle_ctr').val(data.vehicle_ctr);
                        $('#vehicle_registration').val(data.registration);
                        $('#vehicle_description').val(data.description);
                        $('#vehicle_note').val(data.vehiclenote);
                    });
                    get_customer_vehicle_activity(customer_vehicle_ctr);
                }
                $('#section_vehicle').show();
            });

            $(document).on('click', '.vehicleactivityedit', function () {
                customer_vehicleactivity_ctr = $(this).attr('link');
                $('#customer_vehicle_ctr').val(customer_vehicle_ctr);
                $('#section_vehicle').hide();

                /*
                if (customer_vehicle_ctr == 'new') {
                    $('#vehicle_ctr').val('new');
                    $('#vehicle_registration').val('');
                    $('#vehicle_description').val('');
                    $('#vehicle_note').val('');
                } else {
                    $.getJSON("/_Dependencies/data.asmx/get_customer_vehicle?id=" + customer_vehicle_ctr, function (data) {
                        $('#vehicle_ctr').val(data.vehicle_ctr);
                        $('#vehicle_registration').val(data.registration);
                        $('#vehicle_description').val(data.description);
                        $('#vehicle_note').val(data.vehiclenote);
                    });
                    get_customer_vehicle_activity(customer_vehicle_ctr);
                }
                */
                $('#section_vehicleactivity').show();
            });



            $("#form1").validate();

            $('.datetime').datetimepicker({
                format: 'D MMM YYYY HH:mm',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true,
                stepping: 15

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

            $('#btn_vehiclesubmit').click(function () {

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
            });
        }); //document.ready

        function get_customer_vehicles(customer_ctr) {
            $.post("/_Dependencies/data.aspx", { mode: "get_customer_vehicles", customer_ctr: customer_ctr })
                .done(function (data) {
                    $("#div_vehicles").html(data);
                });
        }

        function get_customer_vehicle_activity(customer_vehicle_ctr) {
            $.post("/_Dependencies/data.aspx", { mode: "get_customer_vehicle_activity", customer_vehicle_ctr: customer_vehicle_ctr })
                .done(function (data) {
                    $("#div_vehicleactivity").html(data);
                });
        }

        function test() {
            alert(1);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="section_customersearch">
        <h1>Customer Search
        </h1>
        <div class="form-horizontal">
            <div class="form-group">
                <label class="control-label col-sm-4" for="customersearch_name">Name</label>
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
                <input id="btn_customersubmit" type="button" class="btn btn-info" value="Submit" />
            </div>
            <h1>Customer Maintenance
            </h1>


            <div class="form-horizontal">

                <div class="form-group">
                    <label class="control-label col-sm-4" for="customer_name">Name</label>
                    <div class="col-sm-8">
                        <input type="text" id="customer_name" name="customer_name" class="form-control" required />
                    </div>
                </div>

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
                                    <input type="text" id="customer_knownas" name="customer_knownas" class="form-control" required />
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
                                    <input type="text" id="customer_emailaddress" name="customer_emailaddress" class="form-control" required />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_mobilephone">Mobile phone</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_mobilephone" name="customer_mobilephone" class="form-control" required />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_homephone">Home phone</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_homephone" name="customer_homephone" class="form-control" required />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="control-label col-sm-4" for="customer_workphone">Work phone</label>
                                <div class="col-sm-8">
                                    <input type="text" id="customer_workphone" name="customer_workphone" class="form-control" required />
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
                <input type="button" id="btn_vehiclesubmit" class="btn btn-info" value="Submit" />
            </div>
            <h1>Vehicle Maintenance
            </h1>

           <span class="customer_name"></span>
            <div class="row">
                <div class="col-sm-4 form-group">
                    <label>Registration</label>
                    <input type="text" id="vehicle_registration" name="vehicle_registration" class="form-control" maxlength="6" required />
                </div>
                <div class="col-sm-4 form-group">
                    <label>Make/Model</label>
                    <select id="vehicle_model" name="vehicle_model" class="form-control" required="required">
                        <option value="">--- Please select ---</option>
                        <% 
                            Dictionary<string, string> vehiclemodeloptions = new Dictionary<string, string>();
                            vehiclemodeloptions["type"] = "select";
                            vehiclemodeloptions["valuefield"] = "value";
                            Response.Write(Generic.Functions.buildselection(vehiclemodels, nooptions, vehiclemodeloptions));
                        %>
                    </select>
                </div>
                <div class="col-sm-4 form-group">
                    <label>Type</label>

                    <select id="vehicle_type" name="vehicle_type" class="form-control" required="required">
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
                    <li><a data-target="#div_vehicleactivity">Activity</a></li>
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
                    <div id="div_vehicleactivity" class="tab-pane fade in">
                    </div>
                    <!-- ================================= END OF TABS ===================================  -->
                </div>
            </div>
        </form>
    </div>
    <!--section vehicle maintenance-->

    <div id="section_vehicleactivity" style="display: none">
        <form id="form_vehicleactivity">
              <input type="hidden" id="vehicleactivity_ctr" name="vehicle_ctr" />
            <div class="toprighticon">
                <input type="button" id="vehicleactivityassistance" class="btn btn-info" value="Assistance" />
            </div>
            <div class="bottomrighticon">
                <input type="button" id="btn_vehicleactivitysubmit" class="btn btn-info" value="Submit" />
            </div>
            <h1>Vehicle Maintenance
            </h1>

           <span class="customer_name"></span>
            <div class="row">
                <div class="col-sm-4 form-group">
                    <label>Date</label>
                    <input type="text" id="vehicleactivity_date" name="vehicleactivity_date" class="form-control" required />
                </div>
            </div>

        </form>
    </div>
    <!--section vehicleactivity maintenance-->




</asp:Content>

