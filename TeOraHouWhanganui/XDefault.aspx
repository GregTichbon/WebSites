﻿<%@ Page Title="TOHW Main Menu" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="XDefault.aspx.cs" Inherits="TeOraHouWhanganui.Default" %>
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
    
    <a href="PersonSearch.aspx" class="btn btn-info">Person Maintenance</a>
    

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
