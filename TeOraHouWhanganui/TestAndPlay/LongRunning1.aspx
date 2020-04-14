<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LongRunning1.aspx.cs" Inherits="TeOraHouWhanganui.TestAndPlay.LongRunning1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('.submit').click(function () {
                startCounting();
            });
        });

        var timer;
        var counter = 0;

        function startCounting() {
            timer = window.setTimeout("count()", 1000);
            $('#seconds').html(counter);
        }

        function count() {
            counter = counter + 1;
            $('#seconds').html(counter);
            timer = window.setTimeout("count()", 1000);
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="seconds"></div>
        <asp:TextBox ID="fld_seconds" runat="server"></asp:TextBox>
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit" Text="Submit" />
    </form>
</body>
</html>
