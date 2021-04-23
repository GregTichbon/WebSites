<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CustomerSearch.aspx.cs" Inherits="CommonGoodCoffee.CustomerSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script>
        $(document).ready(function () {
         
            $('#menu').click(function () {
                window.location.href = "default.aspx";
            });


            $("#name").autocomplete({
                source: "<%: ResolveUrl("~/_Dependencies/data.asmx/Customer_name_autocomplete?options=Allow Create")%>",
                minLength: 2,
                select: function (event, ui) {
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
     <div class="toprighticon">
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
