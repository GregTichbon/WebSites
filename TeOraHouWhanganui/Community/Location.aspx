<%@ Page Title="" Language="C#" MasterPageFile="~/Community/Community.Master" AutoEventWireup="true" CodeBehind="Location.aspx.cs" Inherits="TeOraHouWhanganui.Community.Location" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <h1>Location
    </h1>

    <%= location %>
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="name">By Person</label>
            <div class="col-sm-8">
                <input id="name" name="name" type="text" class="form-control" />
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="location">By Location</label>
            <div class="col-sm-8">
                <input id="location" name="location" type="text" class="form-control" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
