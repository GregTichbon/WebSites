<%@ Page Title="" Language="C#" MasterPageFile="~/Private/Main.Master" AutoEventWireup="true" CodeBehind="Access.aspx.cs" Inherits="TeOraHouWhanganui.Private.Administration.Access" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var newctr = 0;

        $(document).ready(function () {
            //Generic.Functions.googleanalyticstracking()%>
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



            function get_newctr() {
                newctr++;
                return newctr;
            }

            $("#fld_person").autocomplete({
                source: "<%: ResolveUrl("~/_Dependencies/data.asmx/Person_name_autocomplete?options=")%>",
                minLength: 2 //,
                , appendTo: "#dialog_edit"
                , select: function (event, ui) {
                    event.preventDefault();
                    $('#fld_person').val(ui.item.label);
                    $('#fld_person').attr('person_ctr', ui.item.value);
                }
            });

            $('.submit').click(function () { //Started creating functions so that I can group code together - see update_enrolement() - not yet tested
                delim = String.fromCharCode(254);
                $('#accesstable > tbody > tr[maint="changed"]').each(function () {
                    tr_id = $(this).attr('id');
                    tr_person = $(this).find('td:eq(1)').attr('person_ctr');
                    tr_access = $(this).find('td:eq(2)').attr('access');
                    tr_note = $(this).find('td:eq(3)').text();

                    value = tr_person + delim + tr_access + delim + tr_note;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
                
                $('#accesstable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });
           
            });
            $(document).on('click', '.edit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_edit").find(':input').val('');
                } else {
                    tr = $(this).closest('tr');

                    $('#fld_person').val($(tr).find('td').eq(1).text());
                    $('#fld_person').attr('person_ctr', $(tr).find('td').eq(1).attr('person_ctr'));
                    $('#fld_access').val($(tr).find('td').eq(2).attr('access'));
                    $('#fld_note').val($(tr).find('td').eq(3).text());
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_edit").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true
                    /*
                    ,open: function (type, data) {
                        $(this).appendTo($('form')); // reinsert the dialog to the form       
                    }*/
                    , appendTo: "#form2"
                });


                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#accesstable > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#accesstable > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'id_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');


                            $(tr).find('td').eq(1).text($('#fld_person').val());
                            $(tr).find('td').eq(1).attr('person_ctr', $('#fld_person').attr('person_ctr'));
                            $(tr).find('td').eq(2).text($('#fld_access option:selected').text());
                            $(tr).find('td').eq(2).attr('access', $('#fld_access').val());
                            $(tr).find('td').eq(3).text($('#fld_note').val());
                            $(this).dialog("close");
                        }
                    }
                }

                 
                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this record?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }
                

                $("#dialog_edit").dialog('option', 'buttons', myButtons);
            })



        }); //document.ready
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%=username %>
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
    </div>

    <div class="toprighticon">
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <h1>Person Access
    </h1>



    <span>xxxxxxxxxx.</span>
    <table id="accesstable" class="table" style="width: 100%">
        <%= html %>
    </table>


    <div id="dialog_edit" title="Maintain Access" style="display: none" class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_person">Person</label>
            <div class="col-sm-8">
                <input type="text" id="fld_person" name="fld_person" class="form-control" required="required" />
            </div>
        </div>

        <div class="form-group">
            <label for="fld_access" class="control-label col-sm-4">
                Access
            </label>
            <div class="col-sm-8">
                <select id="fld_access" name="fld_access" class="form-control" required="required">
                    <option value="">--- Please select ---</option>
                    <%                                                                                                                                            
                        Dictionary<string, string> accessoptions = new Dictionary<string, string>();
                        accessoptions["type"] = "select";
                        accessoptions["valuefield"] = "value";
                        Response.Write(Generic.Functions.buildselection(access, nooptions, accessoptions));
                    %>
                </select>
            </div>
        </div>


        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_note">Note</label>
            <div class="col-sm-8">
                <textarea id="fld_note" name="fld_note" class="form-control"></textarea>
            </div>
        </div>
    </div>
    <p></p>
        <p></p>
        <div class="form-group">
            <div class="col-sm-4">
            </div>
            <div class="col-sm-8">
                <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />
            </div>
        </div>
        <br />
        <br />
        <br />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <form id="form2">
    </form>
</asp:Content>
