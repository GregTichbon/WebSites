<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vouchers.aspx.cs" Inherits="office.dataInn.co.nz.Raffles.UBC2019B.Vouchers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        img {
            display: block;
            margin-left: auto;
            margin-right: auto;
            width: 50%;
        }

        .pc {
            text-align:center;
        }

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

        .centered {
            position: absolute;
            width:100px;
            top: 40%;
            left: 45%;
        }

        .bigfont {
            font-size: x-large;
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.2.1.min.js" integrity="sha256-hwg4gsxgFZhOsEEamdOYGBf13FyQuiTwlAQgxVSNgt4=" crossorigin="anonymous"></script>
    <script type="text/javascript">


        $(document).ready(function () {
            $('#sel_status').change(function () {
                $('#processing').show();

                $.ajax({
                    url: "data.asmx/updaterafflestatus?id=" + $('#hf_id').val() + "&status=" + $(this).val(), success: function (result) {
                        //alert('Success');
                    }, error: function (XMLHttpRequest, textStatus, error) {
                        alert("AJAX error: " + textStatus + "; " + error);
                    }
                });
                setTimeout(
                    function () {
                        $('#processing').hide();
                    }, 1000);
            });
            $(':checkbox').change(function () {
                var selected = [];
                $(':checkbox:checked').each(function() {
                    selected.push($(this).val());
                });

                $('tbody tr').each(function () {
                    thisstatus = $(this).find('td').eq(4).text();
                    if (selected.indexOf(thisstatus) != -1) {
                        $(this).show();
                    }
                    else {
                        $(this).hide();
                    }
                });

            });
        });
    </script>


</head>
<body>
    <form id="form1" runat="server">

        <!--<img src="Images/ChefsChoice%20Logo.PNG" />-->
        <div>
            <p></p>
            <%= html%>
        </div>

        <div id="processing" style="display: none">
            <img src="../Dependencies/Images/processing2.gif" class="centered" />
        </div>


    </form>
    
</body>
</html>