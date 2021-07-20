<%@ Page Title="" Language="C#" MasterPageFile="~/Auction.Master" AutoEventWireup="true" CodeBehind="ArtistAuthor.aspx.cs" Inherits="Auction.Administration.ArtistAuthor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src='//cdn.tinymce.com/4/tinymce.min.js'></script>
    <script>
        tinymce.init({
            selector: '#Description'
        });

        function checkform() {
            var msg = ''
            var delim = ''
            var frm = document.form1;

            if (frm.Title.value == '') {
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
    <input name="artistauthor_ctr" id="artistauthor_ctr" type="hidden" value="<%=artistauthor_ctr%>" />
    <input name="_all" id="_all" type="hidden" value="ALL" />
    <table class="table">
        <tr>
            <td>Name</td>
            <td>
                <input type="text" name="name" id="name" value="<%=name%>" /></td>
        </tr>
        <tr>
            <td>Alive</td>
            <td>
                <input type="text" name="alive" id="alive" value="<%=alive%>" /></td>

            </td>
        </tr>
        <tr>
            <td>Description</td>
            <td>
                <textarea name="Information" id="Information"><%=information%></textarea>
                <code style="width: 400px"><%=information%></code>

            </td>
        </tr>

        <tr>
            <td>URL</td>
            <td>
                <input type="text" name="url" id="url" value="<%=url%>" /></td>
        </tr>
        <tr>
            <td>Image(s)</td>
            <td>
                <asp:FileUpload ID="fu_images" name="fu_images" runat="server" AllowMultiple="true" /></td>
        </tr>
        <tr>
            <td></td>
            <td><%=images %></td>
        </tr>
        <tr>
            <td>Sequence</td>
            <td>
                <input type="text" name="seq" id="seq" value="<%=seq%>" /></td>
        </tr>
        <tr>
            <td>Hide</td>
            <td>
                <select name="hide" id="hide" size="1">
                    <option value="">Please Select</option>
                    <%=   Generic.Functions.populateselect(yesno_values, hide, "None")%>
                </select>
            </td>
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
