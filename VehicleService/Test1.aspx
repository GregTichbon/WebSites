<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test1.aspx.cs" Inherits="VehicleService.Test1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script type="text/javascript">


        $(document).ready(function () {

            $('#btn_submit').click(function () {



                /*
                var formData = new FormData();
                formData.append('mode', 'eventsearch');
                formData.append('fld_program', $('#fld_program').val());
                formData.append('fld_date', $('#fld_date').val());
                formData.append('fld_description', $('#fld_description').val());
*/
                var arForm = $("#form1")
                    .find("input,textarea,select,hidden")
                    .not("[id^='__']")
                    .serializeArray();

                b = { name: 1, value: 2};
                arForm.push({ name: 3, value: 4 });

                console.log(arForm);
                /*
                var formData = JSON.stringify({ formVars: arForm });

                $.ajax({
                    type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                    url: '/_dependencies/posts.asmx/update_vehicle', // the url where we want to POST
                    //processData: false,  // tell jQuery not to process the data
                    //contentType: false,  // tell jQuery not to set contentType
                    contentType: "application/json; charset=utf-8",
                    data: formData,
                    dataType: 'json', // what type of data do we expect back from the server
                    success: function (result) {
                        alert(result);
                        console.log(result);
                    },
                    error: function (xhr, status) {
                        console.log(xhr);
                        console.log(status);
                        alert('error');

                    }
                });
                */

            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="text" id="test1" name="test1" value="Greg was here" />
        <input type="button" id="btn_submit" value="Submit" />
    </form>
</body>
</html>
