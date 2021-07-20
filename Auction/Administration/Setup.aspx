<%@ Page Title="" Language="C#" MasterPageFile="~/Auction.Master" AutoEventWireup="true" CodeBehind="Setup.aspx.cs" Inherits="Auction.Administration.Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdn.tiny.cloud/1/h8c8vm83eegm7bokpsuicg7w70sce3jm8580mnbfldp7tsxk/tinymce/5/tinymce.min.js" referrerpolicy="origin"></script>

    <script type="text/javascript">
 
        $(document).ready(function () {
            tinymce.init({
                selector: 'textarea',
                plugins: "code paste",
                menubar: "tools edit format view",
                paste_as_text: true
            });
        });
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <a href="default.aspx" class="btn btn-info" role="button">Menu</a><br />
    <h2>Auction Setup</h2>
    <table class="table">
        <tr>
            <td>Auction</td>
            <td>
                <input type="text" name="auction" id="auction" value="<%=auction%>" maxlength="255" /></td>
        </tr>
        <tr>
            <td>Message</td>
            <td>
                <textarea name="message" id="message"><%=message%></textarea>
                <code style="width: 400px"><%=message%></code>
        </tr>

        <tr>
            <td>Default Increment</td>
            <td>
                <input type="text" name="increment" id="increment" value="<%=increment%>" maxlength="2" /></td>

        </tr>
        <tr>
            <td>Lower Value</td>
            <td>
                <input type="text" name="lowervalue" id="lowervalue" value="<%=lowervalue%>" maxlength="20" /></td>

        </tr>
                <tr>
            <td>Upper Value</td>
            <td>
                <input type="text" name="uppervalue" id="uppervalue" value="<%=uppervalue%>" maxlength="20" /></td>

        </tr>
        <tr>
            <td>Open from</td>
            <td>
                <input type="text" name="openfrom" id="openfrom" value="<%=openfrom%>" maxlength="20" /></td>

        </tr>
        <tr>
            <td>Close at</td>
            <td>
                <input type="text" name="closeat" id="closeat" value="<%=closeat%>" maxlength="20" /></td>

        </tr>

        <tr>
            <td>Closed Message</td>
            <td>
                <textarea name="closedmessage" id="closedmessage"><%=closedmessage%></textarea>
                <code style="width: 400px"><%=closedmessage%></code>
        </tr>

        <tr>
            <td>Terms and Conditions</td>
            <td>
                <textarea name="termsandconditions" id="termsandconditions"><%=termsandconditions%></textarea>
                <code style="width: 400px"><%=termsandconditions%></code>
        </tr>

        <tr>
            <td>Type</td>
            <td>
                <select name="auctiontype" id="auctiontype" size="1">
                    <option value="">Please Select</option>
                    <%=  Generic.Functions.populateselect(auctiontype_values, auctiontype, "None")%>
                </select>
            </td>
        </tr>

        <tr>
            <td>Do you have donors?</td>
            <td>
                <select name="dodonors" id="dodonors" size="1" required="required">
                    <option value="">Please Select</option>
                    <%=  Generic.Functions.populateselect(yesno_values, dodonors, "None")%>
                </select>
            </td>
        </tr>

         <tr>
            <td>Do you have artists/authors?</td>
            <td>
                <select name="doartistsauthors" id="doartistsauthors" size="1" required="required">
                    <option value="">Please Select</option>
                    <%=  Generic.Functions.populateselect(yesno_values, doartistsauthors, "None")%>
                </select>
            </td>
        </tr>


        <tr>
            <td>Email alerts</td>
            <td>
                <input type="text" name="emailalerts" id="emailalerts" value="<%=emailalerts%>" /></td>

        </tr>
        <tr>
            <td>Text alerts</td>
            <td>
                <input type="text" name="textalerts" id="textalerts" value="<%=textalerts%>" /></td>

        </tr>

        <tr>
            <td>Enable Categories</td>
            <td>
                <select name="enablecategories" id="enablecategories" size="1">
                    <option value="">Please Select</option>
                    <%= Generic.Functions.populateselect(yesno_values, enablecategories, "None")%>
                </select>
            </td>
        </tr>

        <tr>
            <td>Show Highest Bidder</td>
            <td>
                <select name="showhighestbidder" id="showhighestbidder" size="1">
                    <option value="">Please Select</option>
                    <%= Generic.Functions.populateselect(yesno_values, showhighestbidder, "None")%>
                </select>
            </td>
        </tr>

        

        <tr>
            <td>Email from</td>
            <td>
                <input type="email" name="emailfrom" id="emailfrom" value="<%=emailfrom%>" /></td>
        </tr>

        <tr>
            <td>Email from name</td>
            <td>
                <input type="text" name="emailfromname" id="emailfromname" value="<%=emailfromname%>" /></td>
        </tr>
      <tr>
            <td>Email host</td>
            <td>
                <input type="text" name="emailhost" id="emailhost" value="<%=emailhost%>" /></td>
        </tr>
              <tr>
            <td>Email password</td>
            <td>
                <input type="password" name="emailpassword" id="emailpassword" value="<%=emailpassword%>" /></td>
        </tr>

             
              <tr>
            <td>Email reply to</td>
            <td>
                <input type="email" name="emailreplyto" id="emailreplyto" value="<%=emailreplyto%>" /></td>
        </tr>
                <tr>
            <td>Bid log</td>
            <td>
                <select name="bidlog" id="bidlog" size="1">
                    <option value="">Please Select</option>
                    <%=Generic.Functions.populateselect(yesno_values, bidlog, "None")%>
                </select>
            </td>
        </tr>
                <tr>
            <td>Advise test mode</td>
            <td>
                <select name="advisetest" id="advisetest" size="1">
                    <option value="">Please Select</option>
                    <%=Generic.Functions.populateselect(yesno_values, advisetest, "None")%>
                    <%  %>
                </select>
            </td>
        </tr>

        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="btn btn-info" Text="Submit" />
                Note: The application needs to be restarted to write to global.  May be change to Application values and update on submit?
        </tr>
    </table>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
