<%@ Page Title="TOHW Login" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TeOraHouWhanganui.Login.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        @media (min-width: 768px) {
            .form-group.row > .col-form-label {
                text-align: right;
            }
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#form1").validate();

            //$("#pagehelp").colorbox({ href: "LoginHelp.html", iframe: true, height: "600", width: "600", overlayClose: false, escKey: false });

            //Generic.Functions.googleanalyticstracking()%>

        }); //document.ready
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <a id="pagehelp">
        <img id="helpicon" src="../_dependencies/images/qm.png" title="Click on me for specific help on this page." /></a>

    <h1>Log in
    </h1>
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_emailaddress">Email address</label>
            <div class="col-sm-8">
                <input id="fld_emailaddress" name="fld_emailaddress" type="text" class="form-control" required />
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_password">Password</label>
            <div class="col-sm-8">
                <input id="fld_password" name="fld_password" type="password" class="form-control" required />
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-4">
            </div>
            <div class="col-sm-8">
                <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="btn btn-info" Text="Submit" />
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
