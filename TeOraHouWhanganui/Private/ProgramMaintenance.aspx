<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ProgramMaintenance.aspx.cs" Inherits="TeOraHouWhanganui.Private.ProgramMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var newctr = 0;
        var program_ctr = "<%=ViewState["program_ctr"]%>";
        var ids = [<%=ids%>];
        $(document).ready(function () {
            //Generic.Functions.googleanalyticstracking()%>

            /*
            if (program_ctr != 'new') {
                $('#fld_program option:not(:selected)').prop('disabled', true);
            }
            
            */
            $('.enrolmentedit').click(function () {
                mode = $(this).data('mode');

                if (mode == "add") {
                    tr = $('#table_enrolments > tbody tr:first');
                    trclone = $(tr).clone();
                    $(trclone).removeAttr('style');
                    $(trclone).attr('id', 'enrolment_new' + get_newctr());
                    $(trclone).find('td:first').attr("class", "inserted");
                    $(trclone).insertAfter(tr);
                } else {
                    $(tr).find('td:first').attr("class", "changed");
                }
            })

            $('body').on('click', '.name', function () {
                $(this).autocomplete({
                    source: "<%: ResolveUrl("~/_Dependencies/data.asmx/Person_name_autocomplete?options=")%>",
                    minLength: 2,
                    select: function (event, ui) {
                        person_ctr = parseInt(ui.item.person_ctr);
                        if (ids.includes(person_ctr)) {
                            $(this).val('');
                            event.preventDefault();
                            alert('This has already been entered');
                        } else {
                            $(this).replaceWith(ui.item.name);
                            //$(this).val(ui.item.name);
                            //$(this).prop('name', 'name_' + ui.item.person_ctr);
                            $('<input>', {
                                type: 'hidden',
                                name: 'name_new' + get_newctr(),
                                value: ui.item.person_ctr
                            }).appendTo('#form1');
                            event.preventDefault();
                            ids.push(person_ctr);
                        }
                    }
                });
            });

            $('.entity').click(function () {
                window.open("personMaintenance.aspx?id=" + $(this).data('id'), "person");
            })

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

            $('#search').click(function () {
                window.location.href = "<%=ResolveUrl("~/private/programsearch.aspx")%>";
            });
            $("#form1").validate();


        }); //document.ready

        function get_newctr() {
            newctr++;
            return newctr;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%=username %>
    <!--
    <input type="button" value="test" id="btn_test" />
    <input type="text" id="hidden_dirty" />
    -->
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p></p>
    </div>

    <div class="toprighticon">
        <input type="button" id="search" class="btn btn-info" value="Search" />
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <div class="bottomrighticon">
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />
    </div>
    <h1>Program Maintenance
    </h1>
    <div class="form-horizontal">



        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_programname">Program</label>
            <div class="col-sm-8">
                <input type="text" id="fld_programname" name="fld_programname" class="form-control" value="<%: fld_program %>" required />
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_description">Description</label>
            <div class="col-sm-8">
                <textarea id="fld_description" name="fld_description" class="form-control"><%: fld_description %></textarea>
            </div>
        </div>
    </div>


    <table id="table_enrolments" class="table table-striped">
        <%= html_enrolments %>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
