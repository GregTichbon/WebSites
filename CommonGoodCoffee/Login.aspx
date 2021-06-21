<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CommonGoodCoffee.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        $(document).ready(function () {


            $("#form1").validate();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Login
    </h1>
    <div class="form-horizontal row">
        <div class="form-group row">
            <label class="control-label col-md-2" for="fld_businessname">Stick your password in ...... </label>
            <div class="col-md-4">

                <asp:TextBox ID="fld_password" class="form-control" required="required" runat="server" TextMode="Password"></asp:TextBox>
            </div>
        </div>

        <div class="form-group row">
            <label class="control-label col-md-2"></label>
            <div class="col-md-4">
                <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />
            </div>
        </div>

    </div>






</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
