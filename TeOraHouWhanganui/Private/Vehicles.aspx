<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Vehicles.aspx.cs" Inherits="TeOraHouWhanganui.Private.Vehicles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
       <script src="/_Dependencies/RowSorter/Rowsorter.js"></script>

    <style>
        table.sorting-table {
            cursor: move;
        }

        table tr.sorting-row td {
            background-color: #8b8;
        }

        td {
            position: relative;
        }

        .date {
            width: 120px;
        }

        .container {
            width: 100%;
        }

        .icon {
            width: 25px;
        }

        .sorter {
            background-image: url('/_dependencies/images/sort.png');
            background-repeat: no-repeat;
            background-size: 25px 25px;
            background-position: center;
        }
        .editdescription {
            max-width: 200px;
        }

        .highlight {
            color: red;
        }
      
    </style>


    <script type="text/javascript">


        $(document).ready(function () {

            $('#btn_submit').click(function (e) {
                //e.preventDefault(); //testing
                seq = 100;
                $('table tbody tr').each(function () {
                    id = $(this).data('id');
                    if ($('#sequence_' + id).val() != -999) {
                        $('#sequence_' + id).val(seq);
                        seq = seq + 10;
                    }
                });
            });

            $('.add').click(function () {
                alert('to do: clone last row (new_xx) and remove values')
            })

            $('.date').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false//,
                //, inline: true
                //useCurrent: true,
                //stepping: 15
                //, defaultDate: false
                , useCurrent: false

                //,maxDate: moment().add(-1, 'year')
            });

            $('#menu').click(function () {
                window.location.href = "<%=ResolveUrl("default.aspx")%>";
            });

            $('#assistance').click(function () {
                $("#dialog_assistance").dialog({
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true
                });
            })

            $("#form1").validate();

            $('.editdescription').click(function () {
                id = $(this).data('id');
                $('#description').val($('#description_' + id).val());
                //$('#description').val($('#spandescription_' + id).html());

 
                $("#dialog").dialog({
                    modal: true,
                    width: 800,
                    close: function (event, ui) {
  
                    },
                    buttons: [
                        {
                            text: "Cancel",
                            click: function () {
                                $(this).dialog("close");
                            }
                        },
                        {
                            text: "OK",
                            //icon: "ui-icon-heart",
                            click: function () {
                                content = $('#description').val();
                                //$('#spandescription_' + id).html('<pre>' + content.replaceAll('<', '&lt;').replaceAll('>', '&gt;') + '</pre>');
                                $('#spandescription_' + id).text(content);
                                $('#description_' + id).val(content);
                                $(this).dialog("close");
                            }
                        }
                    ]

                });
            });

            new RowSorter('table', {
                handler: 'td.sorter'

                /*,
                //stickFirstRow: true,
                //stickLastRow: false,
                onDragStart: function (tbody, row, index) {
                    log('start index is ' + index);
                },
                onDrop: function (tbody, row, new_index, old_index) {
                    log('sorted from ' + old_index + ' to ' + new_index + " should update these numbers in the database");
                    if (old_index > new_index) {
                        s = new_index;
                        e = old_index;
                    } else {
                        s = old_index;
                        e = new_index;
                    }
                    for (var i = s; i < e; i++) {
                        //console.log(tbody);
                        //console.log(row);
                        //console.log($(row));
                        //console.log($(row).find("td:nth-child(4)").text());
                        //$(row).find("td:nth-child(4)").text("99");


                        var currentRow = $(tbody).find("tr:nth-child(" + (i + 1) + ")");

                        val = currentRow.find("td:nth-child(4)").text();
                        console.log(i + ',' + val);
                        currentRow.find("td:nth-child(4)").text("99")
                    }
                },
                onDragEnd: function (tbody, row, current_index) {
                    log('Dragging the ' + current_index + '. row canceled.');
                }
                */
            });

        }); //document.ready

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

     <div id="dialog" style="display: none">
        <textarea style="width:100%" rows="20" name="description" id="description"></textarea>
    </div>

    <div class="toprighticon">
        <button id="assistance" type="button" class="btn btn-info">Assistance</button>
        <button id="menu" type="button" class="btn btn-info">MENU</button>
    </div>
    <div class="bottomrighticon">
        <asp:Button ID="btn_submit"  runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" ClientIDMode="Static" />
    </div>

    <h1>Vehicle Maintenance
    </h1>
    



    <%=html %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

