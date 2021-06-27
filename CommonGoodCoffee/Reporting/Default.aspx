<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CommonGoodCoffee.Reports.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript">
        $(document).ready(function () {

                      $('#menu').click(function () {
                window.location.href = "/default.aspx";
            });

          
         });
      </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="toprighticon">
         <input type="button" id="menu" class="btn btn-info" value="MENU" />
     </div>
    <h1>Reports Menu</h1>
   
    <%=html %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
