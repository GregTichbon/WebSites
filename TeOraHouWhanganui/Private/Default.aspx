<%@ Page Title="TOHW Main Menu" Language="C#" MasterPageFile="~/Private/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TeOraHouWhanganui.Private.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#assistance').click(function () {
                $("#dialog_assistance").dialog({
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true
                });
            })

            $("#form1").validate();
            //Generic.Functions.googleanalyticstracking()%>

        }); //document.ready
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none"></div>
    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
    </div>

    <h1>Main Menu
    </h1>

    <p><a href="PersonSearch.aspx" class="btn btn-info">Person Maintenance</a></p>

    <%            if (TeOraHouWhanganui._Dependencies.functions.AccessStringTest(username, "1"))
        {
             Response.Write("<p><a href=\"administration/access.aspx\" class=\"btn btn-info\">Security Access</a></p>");
        }
    %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
