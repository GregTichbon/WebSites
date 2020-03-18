<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommunicateWithWinners.aspx.cs" Inherits="office.dataInn.co.nz.Raffles.UBC2019B.CommunicateWithWinners" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        html, body {
            width: 100%;
        }

        table {
            margin: 0 auto;
        }

        th, td {
            border: solid;
            border-width: thin;
            border-collapse: collapse;
            padding: 5px;
            text-wrap: none;
        }

        th {
            text-align: left;
        }
    </style>

    <script src="<%: ResolveUrl("~/Dependencies/jquery-2.2.0.min.js")%>"></script>

    <script>
        $(document).ready(function () {
            $('#cb_toggleall').click(function (event) {
                if (this.checked) {
                    // Iterate each checkbox
                    $('.checkbox').each(function () {
                        this.checked = true;
                    });
                } else {
                    $('.checkbox').each(function () {
                        this.checked = false;
                    });
                }
            });

            /*
            $('.itininja').click(function () {
                cell = $(this)
                itininjaid = $(cell).text();
                $.ajax({
                    dataType: 'html', // what type of data do we expect back from the server
                    url: "http://iti.ninja/data.aspx?mode=Track_link&link=" + itininjaid, success: function (result) {
                        $(cell).html(result);
                        $(cell).unbind( "click" );
                    }
                });
            });
            */
        });
    </script>


</head>
<body>

    <form id="form1" runat="server">
        <br />
        ||greeting|| 
                             <br />
        ||ticketnumber||<br />
                             ||guid||<br />
        ||identifier||<br />
        ||voucher||<br />
        <br />
&nbsp;<asp:TextBox ID="tb_message" runat="server" Height="134px" TextMode="MultiLine" Width="874px" >Hi ||greeting||
Your ticket, ||identifier||/||ticketnumber||, is a winner in the Maadi rowing raffle.
Please go to: ||voucher|| to view/download your Chef's Choice Voucher.
Thanks for your support.
Please contact Greg (0272495088) if you need assistance.
      </asp:TextBox>
        <table style="width:100%">
            <%=html %>
        </table>
        <asp:Button ID="btn_submit" runat="server" Text="Send" OnClick="btn_submit_Click" />
    </form>
</body>
</html>

