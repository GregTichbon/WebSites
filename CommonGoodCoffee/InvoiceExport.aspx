<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InvoiceExport.aspx.cs" Inherits="CommonGoodCoffee.InvoiceExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        $(document).ready(function () {

            $('#menu').click(function () {
                window.location.href = "/default.aspx";
            });

            var button;


            $("#form1").submit(function (e) {
                if (button == 'Submit') {
                    //e.preventDefault();
                    var arr = [];
                    $('.invoice').each(function (index) {
                        if ($(this).val() != "") {
                            invoice = $(this).val();
                            customer = $(this).data('customer');
                            arr.push(invoice + '.' + customer);
                        }
                    })

                    if (arr.length == 0) {
                        alert('You need to enter at least one invoice');
                        e.preventDefault();
                    }

                    var errors = false;
                    var warnings = false;

                    arr.forEach(function (item1, index1) {
                        //alert(index1 + "=" + item1);
                        s1 = item1.split('.');
                        arr.forEach(function (item2, index2) {
                            if (index2 != index1) {
                                s2 = item2.split('.');
                                //alert('Check against: ' + index2 + "=" + item2);
                                if (s2[0] == s1[0] && s2[1] != s1[1]) {
                                    errors = true;
                                }
                                if (s2[1] == s1[1] && s2[0] != s1[0]) {
                                    warnings = true;
                                }
                            }
                        })
                    })

                    if (errors) {
                        message = 'Error: You can not have the same invoice for different customers';
                        if (warnings) {
                            message = message + '\r\nWarning: You have a customer with difference invoices';
                        }
                        alert(message);
                        e.preventDefault();
                    } else if (warnings) {
                        if (confirm('Warning: You have a customer with difference invoices\r\nDo you want to goto the next step anyway?')) {
                        } else {
                            e.preventDefault();
                        }
                    }

                   
                }
            })
            
            $('#btn_submit').click(function () {
                button = $(this).val();
            })

            $('#btn_cancel').click(function () {
                button = 'cancel';
            })



            $("#form1").validate();
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="toprighticon">
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <div class="bottomrighticon">
        <asp:Button ID="btn_cancel" runat="server" OnClick="btn_cancel_Click" class="submit btn btn-info" Text="Go back" Visible="false" ClientIDMode="Static" />
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit"  ClientIDMode="Static" />
    </div>

    <h2>Invoice Export</h2>
     <h3>   <%=highestinvoice %></h3>
    <%=html %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
