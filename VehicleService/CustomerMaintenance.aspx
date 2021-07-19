<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CustomerMaintenance.aspx.cs" Inherits="VehicleService.CustomerMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <script src='//cdn.tinymce.com/4/tinymce.min.js'></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.AreYouSure/1.9.0/jquery.are-you-sure.min.js"></script>
      <script src="/_Dependencies/Support.js"></script>

    <script type="text/javascript">
        var newctr = 0;

        $(document).ready(function () {
            //Generic.Functions.googleanalyticstracking()%>

            function makedirty() {
                //$('#hidden_dirty').addClass('dirty');
                $('#form1').addClass('dirty');
            }
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

            $('#search').click(function () {
                window.location.href = "<%=ResolveUrl("~/customersearch.aspx")%>";
            });
            $(document).on('click', '.vehicleedit', function () {
                id = $(this).attr('link');
                customerid = $(this).attr('customer');
                if (customerid != "") {
                    customerid = "&customerid=" + customerid;
                }
                window.open("VehicleMaintenance.aspx?id=" + id + customerid,"VehicleMaintenance");
                //alert(id + "," + customerid);
            });



            $("#form1").validate();

            $('.datetime').datetimepicker({
                format: 'D MMM YYYY',
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

            $('.submit').click(function () {  

            });

            

        }); //document.ready

        function test() {
            alert(1);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!-- #include file = "/_dependencies/support.html" -->
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p></p>
    </div>

    <div class="toprighticon">
        <input type="button" id="search" class="btn btn-info" value="Search" />
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <div class="bottomrighticon">
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />
    </div>
    <h1>Customer Maintenance
    </h1>


    <div class="form-horizontal">

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_name">Name</label>
            <div class="col-sm-8">
                <input type="text" id="fld_name" name="fld_name" class="form-control" value="<%: fld_name %>" required />
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
                        <label class="control-label col-sm-4" for="fld_firstname">First name</label>
                        <div class="col-sm-8">
                            <input type="text" id="fld_firstname" name="fld_firstname" class="form-control" value="<%: fld_firstname %>" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="fld_surname">Surname</label>
                        <div class="col-sm-8">
                            <input type="text" id="fld_surname" name="fld_surname" class="form-control" value="<%: fld_surname %>" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="fld_knownas">Known as</label>
                        <div class="col-sm-8">
                            <input type="text" id="fld_knownas" name="fld_knownas" class="form-control" value="<%: fld_knownas %>" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="fld_customertype">Customer type</label>
                        <div class="col-sm-4">
                            <select id="fld_customertype" name="fld_customertype" class="form-control" required="required">
                                <option value="">--- Please select ---</option>
                                <% 
                                    Dictionary<string, string> customertypesoptions = new Dictionary<string, string>();
                                    customertypesoptions["type"] = "select";
                                    customertypesoptions["valuefield"] = "value";
                                    Response.Write(Generic.Functions.buildselection(customertypes, fld_customertype_ctr, customertypesoptions));
                                %>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="fld_address">Address</label>
                        <div class="col-sm-8">
                            <textarea id="fld_address" name="fld_address" rows="5" class="form-control"><%: fld_address %></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="fld_emailaddress">Email address</label>
                        <div class="col-sm-8">
                            <input type="text" id="fld_emailaddress" name="fld_emailaddress" class="form-control" value="<%: fld_emailaddress %>" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="fld_mobilephone">Mobile phone</label>
                        <div class="col-sm-8">
                            <input type="text" id="fld_mobilephone" name="fld_mobilephone" class="form-control" value="<%: fld_mobilephone %>" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="fld_homephone">Home phone</label>
                        <div class="col-sm-8">
                            <input type="text" id="fld_homephone" name="fld_homephone" class="form-control" value="<%: fld_homephone %>" required />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="control-label col-sm-4" for="fld_workphone">Work phone</label>
                        <div class="col-sm-8">
                            <input type="text" id="fld_workphone" name="fld_workphone" class="form-control" value="<%: fld_workphone %>" required />
                        </div>
                    </div>
                </div>
                <!-- ================================= NOTE TAB ===================================  -->
                <div id="div_note" class="tab-pane fade in">
                    <!--<h3 class="tabheading">Note</h3>-->
                    <div class="form-group">
                        <!--<label class="control-label col-sm-4" for="fld_note">Note</label>-->
                        <div class="col-sm-12">
                            <textarea id="fld_note" name="fld_note" rows="10" class="form-control"><%: fld_note %></textarea>
                        </div>
                    </div>
                </div>
                <!-- ================================= VEHICLES TAB ===================================  -->
                <div id="div_vehicles" class="tab-pane fade in">
                    <table id="vehicletable" class="table" style="width: 100%">
                        <%= html_vehicles %>
                    </table>
                </div>

                
                <!-- ================================= END OF TABS ===================================  -->
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
