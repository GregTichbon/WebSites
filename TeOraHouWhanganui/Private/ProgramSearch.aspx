<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ProgramSearch.aspx.cs" Inherits="TeOraHouWhanganui.Private.ProgramSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>

    <script>
        $(document).ready(function () {
            $('#assistance').click(function () {
                $("#dialog_assistance").dialog({
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true
                });
            })
            $('#menu').click(function () {
                window.location.href = "<%=ResolveUrl("~/private/default.aspx")%>";
            });

            //$("#form1").validate();


            $('#fld_program').change(function () {
                window.open("programMaintenance.aspx?id=" + $(this).val(), "_self");
            });

            

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input name="mode" value="test" type="hidden" />
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p>Coming soon</p>
    </div>
    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <h1>Program Search
    </h1>
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_program">Program</label>
            <div class="col-sm-8">
                <select id="fld_program" name="fld_program" class="form-control">
                    <option value="">--- Please select ---</option>
                    <% 
                        Dictionary<string, string> programoptions = new Dictionary<string, string>();
                        programoptions["type"] = "select";
                        programoptions["valuefield"] = "value";
                        Response.Write(Generic.Functions.buildselection(programs, nooptions, programoptions));
                    %>
                </select>
            </div>
        </div>
   
        </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

