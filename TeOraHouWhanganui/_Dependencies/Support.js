$(document).ready(function () {
    $('#supporturl').val(window.location.href);
    $('body').append('<form id="frmDialog">');
    $('#support').click(function () {
        $("#dialog_support").dialog({
            resizable: false,
            height: 600,
            width: 800,
            modal: true,
            buttons: {
                "Send": function () {
                    if ($("#frmDialog").valid()) {
                        $(this).dialog("close");
                        var arr = [];
                        arr.push({ 'name': 'supportusername', 'value': $('#supportusername').val() });
                        arr.push({ 'name': 'supporturl', 'value': $('#supporturl').val() });
                        arr.push({ 'name': 'supportmessage', 'value': $('#supportmessage').val() });
                        var formVars = JSON.stringify({ formVars: arr });
                        $.ajax({
                            type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                            url: '/_dependencies/posts.asmx/support', // the url where we want to POST
                            contentType: "application/json; charset=utf-8",
                            data: formVars,
                            dataType: 'json', // what type of data do we expect back from the server
                            async: false,
                            success: function (result) {
                            },
                            error: function (xhr, status) {
                                alert('error');
                            }
                        });
                    }
                }
            }
            , appendTo: "#frmDialog"
        });
    })
    $("#frmDialog").validate();
});
