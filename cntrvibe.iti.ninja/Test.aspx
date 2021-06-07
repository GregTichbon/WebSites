<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="cntrvibe.iti.ninja.Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        $(document).ready(function () {

            $('#btn').click(function () {
             
                var arForm = $("#form1")
                    .find("input,textarea,select,hidden")
                    .not("[id^='__']")
                    .serializeArray();

                console.log(arForm);
                //var arForm = [{ "name": "URL", "value": "Test" }, { "name": "emailaddress", "value": "xxx" }];  //THIS WORKS
                //arForm.push({ name: 'vehicle_ctr', value: $('#vehicle_ctr').val() });  //THIS WORKS
                var formData = JSON.stringify({ formVars: arForm });
                console.log(formData);

                //to pass as formVars:  contentType: "application/json; charset=utf-8" is required but this changes how data is returned ie: expects to be d:




                $.ajax({
                    type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                    url: '/Testpost.asmx/test' + $('#txt').val(), // the url where we want to POST
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json', // what type of data do we expect back from the server
                    //data: { id: "xxxxxx" },
                    data: formData,
                    async: false,
                    success: function (result) {
                        alert('success');
                        console.log("result=" + result);
                        console.log("result.id=" + result.d.id);
                        //console.log("result.d.id=" + result.d.id);
                    },
                    error: function (xhr, status) {
                        console.log(xhr);
                        console.log(status);
                        alert('error');
                    }
                });
            });
        });
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="text" id="txt" />
    <input type="button" id="btn" value="Go" />


     <input type="text" name="txt1" value="1" />
     <input type="text" name="txt2" value="2" />
     <input type="text" name="txt3" value="3" />

    <a href="https://localhost:44373/testpost.asmx/test">https://localhost:44373/testpost.asmx/test</a>
</asp:Content>
