<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TestEncounterAccess.aspx.cs" Inherits="TeOraHouWhanganui.Private.Administration.TestEncounterAccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
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

            $('#menu').click(function () {
                window.location.href = "/private/default.aspx";
            });

            $("#fld_person").autocomplete({
                source: "<%: ResolveUrl("~/_Dependencies/data.asmx/Person_name_autocomplete?options=Allow Create")%>",
                minLength: 2,
                select: function (event, ui) {
                    event.preventDefault();
                    selected = ui.item;
                    $('#fld_person').val(selected.name);
                    $('#fld_person_ctr').val(selected.person_ctr);
                }
            })

            $('.date').datetimepicker({
                format: 'D MMM YYYY HH:mm',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true
            });

        }); //document.ready
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="fld_person_ctr" name="fld_person_ctr" />
    <%=username %>
    <!--
    <input type="button" value="test" id="btn_test" />
    <input type="text" id="hidden_dirty" />
    -->
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p></p>
    </div>

    <div class="toprighticon">

        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <div class="bottomrighticon">
        <asp:Button ID="btn_test" runat="server" OnClick="btn_test_Click" class="submit btn btn-info" Text="Test" />
    </div>
    <h1>Test Encounter Access
    </h1>
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_user">User</label>
            <div class="col-sm-4">

                <asp:DropDownList ID="fld_username" class="form-control" required="required" runat="server" ClientIDMode="Static">
                    <asp:ListItem Value="">-- Please Select --</asp:ListItem>
                    <asp:ListItem>toh\wthompson</asp:ListItem>
                </asp:DropDownList>
                
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_person">Person</label>
            <div class="col-sm-4">
                <asp:TextBox ID="fld_person" class="form-control" required="required" runat="server" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>


        <div class="form-group">
            <label for="fld_date" class="control-label col-sm-4">Date</label>
            <div class="col-sm-4">
                <div class="input-group datetime">
                     <asp:TextBox ID="fld_date" class="form-control date" required="required" runat="server" ClientIDMode="Static"></asp:TextBox>
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
    </div>

   <table id="encountertable" class="table" style="width: 100%">
                    <%= html_encounter %>
                </table>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
