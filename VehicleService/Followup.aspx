<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Followup.aspx.cs" Inherits="VehicleService.Followup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .updatewof {
            width: 20px;
        }
    </style>


    <script>
        $(document).ready(function () {
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
            $('.customer').click(function () {
                customer = $(this).closest('tr').data('customer');
                window.open("/main.aspx?customer=" + customer, 'main');
                //window.location.href = "/main.aspx?customer=" + customer;
            })
            $('.customer_vehicle').click(function () {
                customer = $(this).closest('tr').data('customer');
                customer_vehicle = $(this).closest('tr').data('customer_vehicle');
                window.open("/main.aspx?customer=" + customer + '&customer_vehicle=' + customer_vehicle, 'main');
                //window.location.href = "/main.aspx?customer=" + customer + '&customer_vehicle=' + customer_vehicle;
            })


            $('.updatewof').click(function () {
                id = $(this).closest('tr').data('vehicle');
                $("#dialog_updatewof").dialog({
                    //autoOpen: false,
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true,
                    open: function () {
                        $(this).load('update_wof.aspx?id=' + id);
                        $('#dialogform').validate();
                    },
                    buttons: {
                        "Cancel": function () {
                            $(this).dialog('close');
                        },
                        "Save": function () {

                            alert("to do");
                            if ($('#dialogform').valid()) {

                                var arForm = $("#form1")
                                    .find("input,textarea,select,hidden")
                                    .not("[id^='__']")
                                    .serializeArray();

                                //arForm.push({ name: 'vehicle_ctr', value: $('#vehicle_ctr').val() });
                                var formData = JSON.stringify({ formVars: arForm });
                                alert(formData);

                                $.ajax({
                                    type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                                    url: '/_dependencies/posts.asmx/update_followup', // the url where we want to POST
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







                                $(this).dialog('close');
                            }
                        }
                    }
                });
            });


        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p>To do</p>
    </div>
    <div id="dialog_details" title="Details" style="display: none">
        <p>To do</p>
    </div>
    <div id="dialog_updatewof" title="Update" style="display: none" class="form-horizontal">
        
    </div>

    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <h1>Customer Followup
    </h1>
    <%= html %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
