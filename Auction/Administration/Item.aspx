<%@ Page Title="" Language="C#" MasterPageFile="~/Auction.Master" AutoEventWireup="true" CodeBehind="Item.aspx.cs" Inherits="Auction.Administration.Item" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.tiny.cloud/1/h8c8vm83eegm7bokpsuicg7w70sce3jm8580mnbfldp7tsxk/tinymce/5/tinymce.min.js" referrerpolicy="origin"></script>
    <style>
        #table_donor thead  th {
            color:black;
        }
    </style>
    <script>
        tinymce.init({
            selector: '#Description',
            plugins: "code paste",
            menubar: "tools edit format view",
            paste_as_text: true
        });

        var increment = <% =increment%>;

        $(document).ready(function () {

            var donorctr = $('#table_donor tr').length - 1;

            $('#add').click(function () {
                var $tr = $('#table_template tr:first');
                var $clone = $tr.clone();
                donorctr = donorctr + 1;
                $clone = $clone.repeater_changer(donorctr);
                $('#table_donor tr:last').after($clone);

            });
            $('.delete').click(function () {
                $(this).closest('tr').remove();
            });



            var fixHelperModified = function (e, tr) {
                //alert('here');
                var $originals = tr.children();
                var $helper = tr.clone();
                $helper.children().each(function (index) {
                    $(this).width($originals.eq(index).width())
                });
                return $helper;
            }

            updateIndex = function (e, ui) {
                $('.index', ui.item.parent()).each(function (i) {
                    //alert($(this).attr("id"));
                    //alert($(this).get(0).tagName);
                    $(this).val(i + 1);
                });
            };

            $("#table_donor tbody").sortable({
                helper: fixHelperModified,
                stop: updateIndex
            });

            $("[myid='btn_delete']").click(function (e) {
                if (confirm('Are you sure you want to delete this item?') != true) {
                    e.preventDefault();
                }
            })

            $("[myid='btn_submit']").click(function (e) {
                var msg = ''
                var delim = ''

                if ($('#Title').val() == '') {
                    msg = msg + delim + ' - Title';
                    delim = '\n';
                }

                if ($('#increment').val() != '') {
                    useincrement = $('#increment').val();
                } else {
                    useincrement = increment;
                }

                if ($("#startbid").val() % useincrement != 0) {
                    msg = msg + delim + ' - A start bid that is a multiple of the increment';
                    delim = '\n';
                }

                /*
                //alert(frm.AuctionType.Index);
                if (frm.AuctionType.Index == 0) {
                    msg = msg + delim + ' - Auction Type';
                    delim = '\n';
                }
                */
                if (msg != '') {
                    alert('You must enter:\n' + msg);
                    e.preventDefault();
                }
            })

            $('.itembids').click(function () {
                item_ctr = $(this).attr("id").substring(5);
                $('#dialog_itemsbids').dialog({
                    modal: true,
                    open: function () {
                        $(this).load('itembids.aspx?item=' + item_ctr);
                    },
                    width: $(window).width() * .75,
                    height: 500,
                    close: function () {
                        $(this).html('');
                    },
                    closeText: false
                });
            })




            //$('#submit').click(function () {
            //$( window ).unload(function() {
            //	$('[name^="_donor_"]').val(function(i, v) { //index, current value
            //		return v.replace(/,/g,"\b");
            //	});
            //});
            //window.addEventListener("beforeunload", function(event) {
            //	alert("Write something clever here..");
            //});
        });  //document.ready


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input name="item_ctr" id="item_ctr" type="hidden" value="<%=item_ctr%>" />
    <input name="_all" id="_all" type="hidden" value="All" />
    <div id="dialog_itemsbids" title="Bids on an item"></div>
    <table class="table">
        <tr>
            <td>Title</td>
            <td>
                <input type="text" name="Title" id="Title" value="<%=title%>" maxlength="255" />&nbsp;&nbsp;&nbsp;<%= bids %></td>
        </tr>
        <tr>
            <td>Short description</td>
            <td>
                <input type="text" name="shortdescription" id="shortdescription" value="<%=shortdescription%>" style="width: 100%" /></td>
        </tr>

        <tr>
            <td>Description</td>
            <td>
                <textarea name="Description" id="Description"><%=description%></textarea>
                <code style="width: 400px"><%=description%></code>
            </td>
        </tr>

        <!--
        <tr>
            <td>Type</td>
            <td>
                <select name="AuctionType" id="AuctionType" size="1">
                    <option value="">Please Select</option>
                    <//%=General.Functions.Functions.populateselect(auctiontype_values, auctiontype, "None")%>
                </select>
            </td>
        </tr>
        -->
        <tr>
            <td>Category</td>
            <td>
                <select name="category_ctr" id="category_ctr" size="1">
                    <option value="">Please Select</option>
                    <%= categories %>
                </select>
            </td>
        </tr>
        <tr>
            <td>Reserve ($)</td>
            <td>
                <input type="text" name="Reserve" id="Reserve" value="<%=reserve%>" class="numeric" />
            </td>
        </tr>

        <% if (parameters["LowerValue"] != "")
{ %>

        <tr>
            <td><%=parameters["LowerValue"] %> ($)</td>
            <td>
                <input type="text" name="RetailPrice" id="RetailPrice" value="<%=retailprice%>" class="numeric" /></td>
        </tr>
        <%} %>

               <% if (parameters["UpperValue"] != "")
{ %>

        <tr>
            <td><%=parameters["UpperValue"] %></td>
            <td>
                <input type="text" name="UpperValue" id="UpperValue" value="<%=uppervalue%>" class="numeric" /></td>
        </tr>
        <%} %>
        
        
        <tr>
            <td>Increment ($)</td>
            <td>
                <input type="text" name="increment" id="increment" value="<%=increment%>" class="numeric" /></td>
        </tr>
        <tr>
            <td>Starting bid ($)</td>
            <td>
                <input type="text" name="startbid" id="startbid" value="<%=startbid%>" class="numeric" /></td>
        </tr>


        <tr>
            <td>Image(s)</td>
            <td>
                <asp:FileUpload ID="fu_images" name="fu_images" runat="server" AllowMultiple="true" /></td>
        </tr>
        <tr>
            <td></td>
            <td><%=images %></td>
        </tr>


        <tr>
            <td>Sequence</td>
            <td>
                <input type="text" name="seq" id="seq" value="<%=seq%>" /></td>
        </tr>
        <tr>
            <td>Hide</td>
            <td>
                <select name="hide" id="hide" size="1">
                    <option value="">Please Select</option>
                    <%= Generic.Functions.populateselect(yesno_values, hide, "None")%>
                </select>
        </tr>
        <%if (parameters["DoDonors"] == "Yes")
            { %>
        <tr>
            <td colspan="2">Donors (Drag & Drop to change order)<br />
                <table id="table_donor" class="table">
                    <thead>
                        <tr>
                            <th>Donor</th>
                            <th>Amount</th>
                            <th><a id="add">Add</a></th>
                        </tr>
                    </thead>

                    <%=get_donors(item_ctr) %>
                </table>
            </td>
        </tr>
        <%} %>
        <tr>
            <td>&nbsp;</td>
            <td>

                <asp:Button ID="btn_delete" myid="btn_delete" runat="server" OnClick="btn_submit_Delete" class="btn btn-info" Text="Delete" />&nbsp;&nbsp;&nbsp;&nbsp;
                  <asp:Button ID="btn_submit" myid="btn_submit" runat="server" OnClick="btn_submit_Click" class="btn btn-info" Text="Submit" />
        </tr>
    </table>

    <%
/*
folder = Server.MapPath("..\images\items\" & id)
Set fso = Server.CreateObject("Scripting.FileSystemObject")
If fso.FolderExists(folder) = True Then
    response.write "<table><tr>"
    Set fsoFolder = fso.GetFolder(folder)
    For Each fsoFile In fsoFolder.Files
        Response.Write "<td><img src=""../images/items/" & id & "/" & fsoFile.Name & """ height=""160"" border=""0"" alt=""" & fsoFile.Name & """><br />Delete <input name=""_imgdelete_" & fsoFile.Name & """ type=""checkbox"" id=""_imgdelete_" & fsoFile.Name & """ value=""-1""></td>"
    Next
    response.write "</table>"
End If
Set fso=nothing
Set fsoFolder=nothing
*/

    %>


    <table id="table_template" style="display: none">
        <tr>
            <td>
                <input id="_itemdonor_ctr_" type="hidden" value="0" />
                <input class="index" id="_itemdonor_index_" type="hidden" value="0" />
                <select id="_itemdonor_donor_ctr_" size="1">
                    <option value="">Please Select</option>
                    <%
                        for (int f1 = 0; f1 <= donors; f1++)
                        {
                            Response.Write("<option value=\"" + donor_ctrs[f1] + "\">" + donornames[f1] + "</option>");
                        }
                    %>
                </select>
            </td>
            <td>
                <input type="text" id="_itemdonor_amount_" name="_itemdonor_amount_" value="0" /></td>
            <td></td>
        </tr>
    </table>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
