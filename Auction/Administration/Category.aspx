<%@ Page Title="" Language="C#" MasterPageFile="~/Auction.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="Auction.Administration.Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>


        function checkform() {
            var msg = ''
            var delim = ''
            var frm = document.form1;

            if (frm.category.value == '') {
                msg = msg + delim + ' - Title';
                delim = '\n';
            }

            if (msg != '') {
                alert('You must enter:\n' + msg);
                return (false);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input name="category_ctr" id="donor_ctr" type="hidden" value="<%=category_ctr%>" />
    
    <table class="table">
        <tr>
            <td>Category</td>
            <td>
                <input type="text" name="category" id="category" value="<%=category%>" /></td>
        </tr>
        
        <tr>
            <td>Sequence</td>
            <td>
                <input type="text" name="sequence" id="sequence" value="<%=sequence%>" /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="btn btn-info" Text="Submit" />
            </td>
        </tr>

    </table>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

