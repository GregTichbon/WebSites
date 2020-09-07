<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VehicleMaintenance.aspx.cs" Inherits="VehicleService.VehicleMaintenance" %>

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
            $(document).on('click', '.activityedit', function () {
                id = $(this).attr('link');
                customerid = $(this).attr('customer');
                if (customerid != "") {
                    customerid = "&customerid=" + customerid;
                }
                window.open("ActivityMaintenance.aspx?id=" + id + customerid, "ActivityMaintenance");
                //alert(id + "," + customerid);
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

            $(".nav-tabs a").click(function () {
                $(this).tab('show');
            });

            $('.nav-tabs a').on('shown.bs.tab', function (event) {
                var x = $(event.target).text();         // active tab
                var y = $(event.relatedTarget).text();  // previous tab
                $(".act span").text(x);
                $(".prev span").text(y);
            });

            $('.submitX').click(function () {
                window.parent.test();
                //window.close();

            });

            $("form").submit(function (e) {
                window.parent.test();
                e.preventDefault();
            });



        }); //document.ready
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <h1>Vehicle Maintenance
    </h1>

    <%= customername %>
    <div class="row">
        <div class="col-sm-4 form-group">
            <label>Registration</label>
            <input type="text" id="fld_registration" name="fld_registration" class="form-control" maxlength="6" value="<%: xxxxxxx %>" required />
        </div>
        <div class="col-sm-4 form-group">
            <label>Make/Model</label>
            <select id="fld_model" name="fld_model" class="form-control" required="required">
                <option value="">--- Please select ---</option>
                <% 
                    Dictionary<string, string> vehiclemodeloptions = new Dictionary<string, string>();
                    vehiclemodeloptions["type"] = "select";
                    vehiclemodeloptions["valuefield"] = "value";
                    Response.Write(Generic.Functions.buildselection(vehiclemodels, fld_vehiclemodel_ctr, vehiclemodeloptions));
                %>
            </select>
        </div>
        <div class="col-sm-4 form-group">
            <label>Type</label>
          
            <select id="fld_vehicletype" name="fld_vehicletype" class="form-control" required="required">
                <option value="">--- Please select ---</option>
                <% 
                    Dictionary<string, string> vehicletypeoptions = new Dictionary<string, string>();
                    vehicletypeoptions["type"] = "select";
                    vehicletypeoptions["valuefield"] = "value";
                    Response.Write(Generic.Functions.buildselection(vehicletypes, fld_vehicletype_ctr, vehicletypeoptions));
                %>
            </select>
        </div>
    </div>

    <!------------------------------------------ TABS ------------------------------------------------------------>

    <div class="form-horizontal">
        <ul class="nav nav-tabs">
            <li class="active"><a data-target="#div_general">General</a></li>
            <li><a data-target="#div_note">Note</a></li>
            <li><a data-target="#div_activity">Activity</a></li>
        </ul>
        <div class="tab-content">
            <!-- ================================= GENERAL TAB ===================================  -->
            <div id="div_general" class="tab-pane fade in active">
                <!--<h3 class="navbar"><span class="navbar-brand">General</span></h3>-->
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_description">Description</label>
                    <div class="col-sm-8">
                        <textarea id="fld_description" rows="6" name="fld_description" class="form-control"><%: xxxxxxx %></textarea>
                    </div>
                </div>
            </div>
            <!-- ================================= NOTE TAB ===================================  -->
            <div id="div_note" class="tab-pane fade in">
                <!--<h3 class="tabheading">Note</h3>-->
                <div class="form-group">
                    <!--<label class="control-label col-sm-4" for="fld_note">Note</label>-->
                    <div class="col-sm-12">
                        <textarea id="fld_note" name="fld_note" rows="10" class="form-control"><%: xxxxxxx %></textarea>
                    </div>
                </div>
            </div>
            <!-- ================================= ACTIVITY TAB ===================================  -->
            <div id="div_activity" class="tab-pane fade in">
                <table id="activitytable" class="table" style="width: 100%">
                    <%= html_activity %>
                </table>
            </div>
            <!-- ================================= END OF TABS ===================================  -->
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
