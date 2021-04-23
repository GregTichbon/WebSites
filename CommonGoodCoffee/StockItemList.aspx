<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StockItemList.aspx.cs" Inherits="CommonGoodCoffee.StockItemList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script>
        $(document).ready(function () {
         
            $('#menu').click(function () {
                window.location.href = "default.aspx";
            });


        });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="toprighticon">
          <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <table class="table" style="width: 100%">
        <%= html %>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
