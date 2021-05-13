<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StockItemList.aspx.cs" Inherits="CommonGoodCoffee.StockItemList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .highlight {
            background-color:red;
            padding: 5px;
        }

        .hide0{
            display:none;
        }

    </style>


     <script>
        $(document).ready(function () {
         
            $('#menu').click(function () {
                window.location.href = "default.aspx";
            });

            $('#btn_toggle0').click(function () {
                if ($(this).val() == 'Show 0') {
                    $(this).val('Hide 0');
                } else {
                    $(this).val('Show 0');
                }
                $('table tbody tr.hide0').each(function () {
                    $(this).toggle();
                })
            })


        });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="toprighticon">
          <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <input type="button" id="btn_toggle0" class="btn btn-info" value="Show 0" />
    <table class="table" style="width: 100%">
        <%= html %>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
