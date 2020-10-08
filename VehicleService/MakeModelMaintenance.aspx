<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MakeModelMaintenance.aspx.cs" Inherits="VehicleService.MakeModelMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        var newctr = 0;

        $(document).ready(function () {
            $('#sel_vehiclemake').change(function () {
                $(this).hide();
                var vehiclemake_ctr = $('#sel_vehiclemake').val();
                $('#hf_vehiclemake_ctr').val(vehiclemake_ctr);
                var vehiclemake = $('#sel_vehiclemake option:selected').text();
                $('#fld_vehiclemake').val(vehiclemake);
                if (vehiclemake_ctr != 'new') {
                    $.post("/_Dependencies/data.aspx", { mode: "get_vehiclemodels", vehiclemake_ctr: vehiclemake_ctr })
                        .done(function (data) {
                            $("#div_vehiclemodels").html(data);
                            $('#div_vehiclemodels table tbody tr').each(function () {
                                id = $(this).attr('link');
                                value = $(this).find('td').eq(1).text();
                                var s = $('<input type="text" class="form-control vehiclemodelchange" required="required" value="' + value + '" data-original="' + value + '" />');
                                //var s = $('<input type="text" class="form-control vehiclemodelchange" value="' + value + '" required="required"  data-original="' + value + '" />');
                                $(this).find('td').eq(1).html(s);
                            });
                        });
                }
                $('#fld_vehiclemake').show();
            });

            $(document).on('click', '.vehiclemodeledit', function () {
                vehiclemake_ctr = $(this).attr('link');
                if (vehiclemake_ctr == 'new') {
                    id = 'new_' + get_newctr();
                    $('#div_vehiclemodels table tbody').append('<tr link="' + id + '" maint="changed"><td><img src="/_dependencies/images/star.png"></td><td><input type="text" class="form-control vehiclemodelchange" required="required" value="" /></td><td></td><td><a link="' + id + '" class="vehiclemodeledit">Remove</a></td></tr>');
                    //$('#div_vehiclemodels table tbody').append('<tr><td></td><td><input type="text" class="form-control vehiclemodelchange" required="required" data-original="" value="" /></td><td><a link="' + id + '" class="vehiclemodeledit">Remove</a></td></tr>');
                } else {
                    tr = $(this).closest('tr');
                    if ($(this).text() == 'Remove') {
                        if ($(tr).find('td').eq(1).find('input').data('original') == null) {
                            $(tr).remove();
                        } else {
                            //$(this).closest('tr').remove();
                            $(this).text("Restore");
                            $(tr).find('td').eq(0).html('<img src="/_dependencies/images/scissors.png" />');
                            $(tr).attr('maint', 'remove');
                        }
                    } else {
                        //Restore
                        $(this).text("Remove");
                        if ($(tr).find('td').eq(1).find("input").val() == $(tr).find('td').eq(1).find('input').data('original')) {
                            $(tr).find('td').eq(0).html('');
                            $(tr).attr('maint', '');
                        } else {
                            $(tr).find('td').eq(0).html('<img src="/_dependencies/images/pencil.png" />');
                            $(tr).attr('maint', 'changed');

                        }

                    }
                }
            });

            $(document).on('change', '.vehiclemodelchange', function () {
                if ($(this).data('original') == $(this).val()) {
                    $(this).closest('tr').find('td').eq(0).html('');
                    $(this).closest('tr').attr('maint', '');
                } else {
                    if ($(this).data('original') != null) {
                        $(this).closest('tr').find('td').eq(0).html('<img src="/_dependencies/images/pencil.png" />');
                        $(this).closest('tr').attr('maint', 'changed');
                    }
                }
            });

            $("#form1").submit(function (e) {
                //$('.submit').click(function () { 
                //delim = String.fromCharCode(254);
                $('#div_vehiclemodels > table > tbody > tr').each(function () {
                    action = $(this).attr('maint');
                    if (action != null) {
                        tr_id = 'model_' + $(this).attr('link');
                        if (action == 'Remove') {
                            tr_id = tr_id + "_deleted";
                            tr_model = "";
                        } else {
                            tr_model = $(this).find('td:eq(1)').find('input').val();
                        }
                        value = tr_model; // + delim + tr_xxxxxx;
                        console.log('model=' + value);
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: value
                        }).appendTo('#form1');
                    }
                });
                //e.preventDefault();
            });

            $('#btn_cancel').click(function () {
                $('#fld_vehiclemake').hide();
                $('#sel_vehiclemake').val('');
                $("#div_vehiclemodels").html('');
                $('#sel_vehiclemake').show();
            });


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

            $("#form1").validate();

            function get_newctr() {
                newctr++;
                return newctr;
            }
        }); //document ready
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="hf_vehiclemake_ctr" name="hf_vehiclemake_ctr" />
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p></p>
    </div>

    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <div class="bottomrighticon">
        <input type="button" id="btn_cancel" class="submit btn btn-info" value="Cancel" />
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />
    </div>
    <h1>Vehicle Make/Model Maintenance
    </h1>


    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_vehiclemake">Make</label>
            <div class="col-sm-8">
                <input type="text" id="fld_vehiclemake" name="fld_vehiclemake" class="form-control" style="display: none" required />
                <select id="sel_vehiclemake" class="form-control">
                    <option value="">--- Please Select ---</option>
                    <% 
                        Dictionary<string, string> vehiclemakeoptions = new Dictionary<string, string>();
                        vehiclemakeoptions["type"] = "select";
                        vehiclemakeoptions["valuefield"] = "value";
                        Response.Write(Generic.Functions.buildselection(vehiclemakes, nooptions, vehiclemakeoptions));
                    %>
                    <option value="new">Add new make</option>
                </select>
            </div>
        </div>
        <div id="div_vehiclemodels"></div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
