﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CustomerSearch.aspx.cs" Inherits="VehicleService.CustomerSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                window.location.href = "<%=ResolveUrl("~/private/default.aspx")%>";
            });

    
            $("#name").autocomplete({
                source: "<%: ResolveUrl("~/_Dependencies/data.asmx/Customer_name_autocomplete?options=Allow Create")%>",
                minLength: 2,
                select: function (event, ui) {
                    console.log(ui);
                    event.preventDefault();
                    $('#name').val("");
                    selected = ui.item;
                    window.open("CustomerMaintenance.aspx?id=" + selected.customer_ctr, "_self");
                }
            })



        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
     <p>Start typing any part of the name, you will need at least 2 characters.</p> 
     <p>The more that you type the more accurate your search will become and the less options will be shown.</p>
     <p>To choose a record simply click on the name or choose "Create a new customer"</p>
     <p>You should always do a thorogh search before adding a new record.</p>


 </div>
    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <h1>Customer Search
    </h1>
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="name">Name</label>
            <div class="col-sm-8">
                <input id="name" name="name" type="text" class="form-control" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
