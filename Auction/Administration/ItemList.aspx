<%@ Page Title="" Language="C#" MasterPageFile="~/Auction.Master" AutoEventWireup="true" CodeBehind="ItemList.aspx.cs" Inherits="Auction.Administration.ItemList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .cycle-slideshow {
            height: 150px;
        }

            .cycle-slideshow img {
                width: auto;
                height: 100%;
            }
    </style>

    <script src="/_Dependencies/Scripts/cycle2/jquery.cycle2.min.js"></script>



    <script type="text/javascript">
        $(document).ready(function () {
            $('.itembids').click(function () {
                item_ctr = $(this).attr("id").substring(5);
                $('#dialog_itemsbids').dialog({
                    modal: true,
                    open: function () {
                        $(this).load('itembids.aspx?item=' + item_ctr);
                    },
                    width: $(window).width() * .75,
                    height: 500,
                    close: function () {
                        $(this).html('');
                    },
                    closeText: false
                });
            })
            //$.fn.cycle.defaults.autoSelector = '.slideshow';
        }); //document.ready

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <a href="Item.aspx" class="btn btn-info" role="button">Create</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="default.aspx" class="btn btn-info" role="button">Menu</a><br />
    <div id="dialog_itemsbids" title="Bids on an item"></div>
    <table class="table">
        <%=html%>
    </table>
    <br />
    <a href="default.aspx" class="btn btn-info" role="button">Menu</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

