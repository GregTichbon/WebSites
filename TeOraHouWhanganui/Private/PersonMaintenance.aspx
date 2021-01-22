<%@ Page ValidateRequest="false" Title="TOHW Person Maintenance" Language="C#" MasterPageFile="~/Private/Main.Master" AutoEventWireup="true" CodeBehind="PersonMaintenance.aspx.cs" Inherits="TeOraHouWhanganui.Private.PersonMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/cropper/4.0.0/cropper.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/cropper/4.0.0/cropper.min.css" />
    <!--<script type="text/javascript" src="//cdn.jsdelivr.net/jquery.dirtyforms/2.0.0/jquery.dirtyforms.min.js"></script>-->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.AreYouSure/1.9.0/jquery.are-you-sure.min.js"></script>

    <!--<script src='//cdn.tinymce.com/4/tinymce.min.js'></script>-->
    <script src="https://cdn.tiny.cloud/1/0rm57f61mkhtljml272kg6rniw3zjw84hmu9q5zoyebn7sb2/tinymce/5/tinymce.min.js" referrerpolicy="origin"></script>


    <style>
        .imagecontainer {
            max-width: 800px;
            max-height: 800px;
            margin: 20px auto;
        }

        #preview {
            overflow: hidden;
            width: 200px;
            height: 200px;
        }

        img {
            max-width: 100%;
        }

        #canvas {
            background-color: #ffffff;
            cursor: default;
            border: 1px solid black;
            width: 1200px;
        }
    </style>

    <script type="text/javascript">
        var newctr = 0;
        var mode = "<%=ViewState["person_ctr"]%>";
        var lastdata = "";

        $(document).ready(function () {
            //Generic.Functions.googleanalyticstracking()%>

            function makedirty() {
                //$('#hidden_dirty').addClass('dirty');
                $('#form1').addClass('dirty');
            }
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

            $('#search').click(function () {
                window.location.href = "<%=ResolveUrl("~/private/personsearch.aspx")%>";
            });
            //$("#form1").dirtyForms();

            $('#form1').areYouSure();

            $("#form1").validate();

            var confirmvalue = '';
            $('.confirm').focus(function () {
                confirmvalue = $(this).val();
            })
            $('.confirm').change(function () {
                if (mode != "new") {
                    if (!confirm('Are you sure you want to change this field?')) {
                        $(this).val(confirmvalue);
                    }
                }
            })

            var canvas = $("#canvas") 
            var context = canvas.get(0).getContext("2d") 

            $('#getphoto').click(function () {
                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }
                $("#dialog_getphoto").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true,
                    buttons: {
                        "Restore": function () {
                            canvas.cropper('reset');
                            //$result.empty();
                        },
                        "Cancel": function () {
                            canvas.cropper("destroy");
                            $(this).dialog("close");
                        },
                        "Upload": function () {
                            var image = canvas.cropper('getCroppedCanvas').toDataURL("image/jpg");
                            image = image.replace('data:image/png;base64,', '');
                            console.log(image);
                            $.ajax({
                                type: "POST",
                                //async: false,
                                url: "posts.asmx/SaveImage",
                                data: '{"imageData": "' + image + '", "id": "<%: person_ctr %>"}',
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (result) {
                                    //alert(result);
                                    d = new Date();
                                    $("#img_photo").attr("src", "images/<%:person_ctr %>.jpg?" + d.getTime());
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert("Status: " + textStatus); alert("Error: " + errorThrown);
                                }
                            });
                            canvas.cropper("destroy");
                            $(this).dialog("close");
                        }
                    }
                });
            })
            $('#fileInput').on('change', function () {
                if (this.files && this.files[0]) {
                    if (this.files[0].type.match(/^image\//)) {
                        var reader = new FileReader();
                        reader.onload = function (evt) {
                            var img = new Image();
                            img.onload = function () {
                                context.canvas.height = img.height;
                                context.canvas.width = img.width;
                                context.drawImage(img, 0, 0);
                                var cropper = canvas.cropper({
                                    preview: '#preview',
                                    dragMode: 'crop',
                                    autoCropArea: 0.65,
                                    rotatable: true,
                                    cropBoxMovable: true,
                                    cropBoxResizable: true
                                });
                            };
                            img.src = evt.target.result;
                        };
                        reader.readAsDataURL(this.files[0]);
                    }
                    else {
                        alert("Invalid file type! Please select an image file.");
                    }
                }
                else {
                    alert('No file(s) selected.');
                }
            });

            $('#img_photo').click(function () {
                $("#fullphoto").attr('src', $('#img_photo').attr('src'));
                $("#dialog_fullphoto").dialog({
                    resizable: false,
                    height: 600,
                    width: 800,
                    modal: true
                });
            });

            $('.geocode').click(function () {
                address = $('#fld_address_address').val();
                if (address.toLowerCase().indexOf("whanganui") == -1 && address.toLowerCase().indexOf("wanganui") == -1) {
                    address += ", Whanganui";
                }
                $.ajax({
                    type: "POST",
                    //async: false,
                    url: "posts.asmx/googlegeocode",
                    data: '{"address": "' + address + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        $('#fld_address_address').val(result.d.address.replace(/, /g, '\n'));
                        $('#fld_address_coordinates').val(result.d.lat + "," + result.d.lng);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Status: " + textStatus); alert("Error: " + errorThrown);
                    }
                });
            })

            $(document).on('click', '.send_text', function () {
                phonenumber = $(this).closest('tr').find('td').eq(1).text();
                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }
                $("#dialog-sendtext").dialog({
                    resizable: false,
                    height: 250,
                    width: mywidth,
                    modal: true,
                    buttons: {
                        "Cancel": function () {
                            $(this).dialog("close");
                        },
                        "Send": function () {
                            $.post("posts.asmx/send_text", { PhoneNumber: phonenumber, Message: $('#tb_textmessage').val() }, function (data) {
                                alert(data);
                            },
                                'html'
                            );
                            $(this).dialog("close");
                        }
                    }
                });
            })

            $(document).on('click', '.send_email_system', function () {
                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }
                $("#dialog-sendemail").dialog({
                    resizable: false,
                    height: 400,
                    width: mywidth,
                    modal: true,
                    buttons: {
                        "Cancel": function () {
                            $(this).dialog("close");
                        },
                        "Send": function () {
                            alert("to do - send to: " + $('td:eq(1)', $(this).parents('tr')).text());
                            $(this).dialog("close");
                        }
                    }
                });
            })

            $(document).on('click', '.send_email_local', function () {
                email = $('td:eq(1)', $(this).parents('tr')).text();
                firstname = $('#tb_firstname').val();
                knownas = $('#tb_knownas').val();
                if (knownas == "") {
                    knownas = firstname;
                }
                $(this).attr("href", "mailto:" + email + "?subject=Te Ora Hou Whanganui&body=Hi " + knownas);
            })

            $(".nav-tabs a").click(function () {
                $(this).tab('show');
            });

            $('.nav-tabs a').on('shown.bs.tab', function (event) {
                var x = $(event.target).text();         // active tab
                var y = $(event.relatedTarget).text();  // previous tab
                $(".act span").text(x);
                $(".prev span").text(y);
            });

            //$(".uicheckbox").checkboxradio({
            //    icon: false
            //});

            $('.standarddate').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true
                //,maxDate: moment().add(-1, 'year')
            });

            $('.datetime').datetimepicker({
                format: 'D MMM YYYY HH:mm',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true,

                //,maxDate: moment().add(-1, 'year')
            });

            $('.birthdate').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: false,
                sideBySide: true,
                viewMode: 'years'
                //,maxDate: moment().add(-1, 'year')
            });

            $(".birthdate").on("dp.change", function (e) {
                showage_id = $(this).data('showage');
                asat = $(this).data('asat');
                years = calculateage(e.date, asat);
                if (showage_id != "") {
                    $('#' + showage_id).text('Age: ' + years + ' years')
                }
            });

            var e = moment($(".birthdate").find("input").val());
            showage_id = $(this).data('showage');
            asat = $(this).data('asat');
            years = calculateage(e.date, asat);
            if (showage_id != "") {
                $('#' + showage_id).text('Age: ' + years + ' years')
            }

            function calculateage(e, asat) {   //MOVE THIS TO MAIN.JS
                if (moment().diff(e, 'seconds') < 0) {
                    e.date = moment(e).subtract(100, 'years');
                    //$("#tb_birthdate").val(moment(e).format('D MMM YYYY'));
                }
                var years = moment().diff(e, 'years');
                thisyear = moment().year();
                thismonth = moment().month() + 1;


                //$("#span_age").text('Age: ' + years + ' years, ' + jan1.diff(e, 'years') + ' years at 1 Jan ' + thisyear);
                return (years);
            }

            function get_newctr() {
                newctr++;
                return newctr;
            }

            $("#fld_assigned_person").autocomplete({
                source: "<%: ResolveUrl("~/_Dependencies/data.asmx/Person_name_autocomplete?options=")%>",
                minLength: 2 //,
                , appendTo: "#dialog_assigned"
                , select: function (event, ui) {
                    event.preventDefault();
                    $('#fld_assigned_person').val(ui.item.label);
                    $('#fld_assigned_person').attr('person_ctr', ui.item.value);
                }
            });

            $('.submit').click(function () { //Started creating functions so that I can group code together - see update_enrolment() - not yet tested
                delim = String.fromCharCode(254);

                /*----------------------------------------------WORKER ROLE-----------------------------------------*/
                $('#workerroletable > tbody > tr[maint="changed"]').each(function () {
                    tr_id = $(this).attr('id');
                    tr_role = $(this).find('td:eq(1)').attr('role_ctr');
                    tr_startdate = $(this).find('td:eq(2)').text();
                    tr_enddate = $(this).find('td:eq(3)').text();
                    tr_note = $(this).find('td:eq(4)').text();

                    value = tr_role + delim + tr_startdate + delim + tr_enddate + delim + tr_note;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
                /*
                $('#workerroletable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });
                */
                /*----------------------------------------------ASSIGNMENT-----------------------------------------*/
                $('#assignedtable > tbody > tr[maint="changed"]').each(function () {
                    tr_id = $(this).attr('id');
                    tr_type = $(this).find('td:eq(1)').text();
                    tr_person = $(this).find('td:eq(2)').attr('person_ctr');
                    tr_startdate = $(this).find('td:eq(3)').text();
                    tr_enddate = $(this).find('td:eq(4)').text();
                    tr_note = $(this).find('td:eq(5)').text();
                    tr_level = $(this).find('td:eq(6)').attr('accesslevel');

                    value = tr_type + delim + tr_person + delim + tr_startdate + delim + tr_enddate + delim + tr_note + delim + tr_level;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
                /*
                $('#assignedtable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });
                */
                /*----------------------------------------------ENCOUNTERS-----------------------------------------*/
                $('#encountertable > tbody > tr[maint="changed"]').each(function () {

                    tr_id = $(this).attr('id');
                    tr_startdatetime = $(this).find('td:eq(1)').text();
                    tr_enddatetime = $(this).find('td:eq(2)').text();
                    tr_narrative = $(this).find('td:eq(3)').html();
                    tr_workers = $(this).find('td:eq(4)').attr('workerid');
                    tr_level = $(this).find('td:eq(5)').attr('encounteraccesslevel');

                    value = tr_startdatetime + delim + tr_enddatetime + delim + tr_narrative + delim + tr_workers + delim + tr_level;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
                /*
                $('#encountertable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });
                */

                update_enrolment();
                update_address();
                update_phone();

                $('#form1').removeClass('dirty');
                $('#form2').removeClass('dirty');

            });  //.submit end

            /* ========================================= EDIT ENCOUNTERS ===========================================*/
            $(document).on('click', '.encounteredit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_encounter").find(':input').val('');
                    workers = '';
                } else {
                    tr = $(this).closest('tr');
                    $('#fld_encounter_startdatetime').val($(tr).find('td').eq(1).text());
                    $('#fld_encounter_enddatetime').val($(tr).find('td').eq(2).text());
                    $('#fld_encounter_narrative').val($(tr).find('td').eq(3).html());
                    workers = $(tr).find('td').eq(4).attr("workerid");
                    $('#fld_encounter_level').val($(tr).find('td').eq(5).attr('encounteraccesslevel'));
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_encounter").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true

                    , open: function (type, data) {
                        //$(this).appendTo($('form')); // reinsert the dialog to the form   
                        $("#form1 :button").prop("disabled", true);
                        $('#form1 :input[type="submit"]').prop('disabled', true);
 
                        var myarr = [];
                        var workersarray = workers.toString().split('|');
                        $.each(workersarray, function (index, value) {
                            myarr.push(value);
                        });

                        $('#fld_encounter_worker').select2();
                        $('#fld_encounter_worker').val(myarr);
                        $("#fld_encounter_worker").trigger("change");

                        datalogger = setInterval(function () { datalog(tinyMCE.activeEditor.getContent()); }, 2000);

                    }

                    , close: function (event, ui) {
                        //$('#fld_encounter_worker').select2('destroy');
                        clearInterval(datalogger);
                        $("#form1 :button").prop("disabled", false);
                        $('#form1 :input[type="submit"]').prop('disabled', false);
                    }

                    , appendTo: "#form2"
                });

                tinymce.init({
                    selector: '.tinymce',
                    menubar: "tools edit format view",
                    paste_as_text: true,
                    plugins: [
                        "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
                        "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                        "save table contextmenu directionality emoticons template paste textcolor"
                    ]
                });

                var myButtons = {
                    "Cancel": function () {
                        tinymce.remove('.tinymce');;
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#div_encounter > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_encounter > table > tbody > tr:first').before(tr);
                                $(tr).attr('id', 'encounter_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");
                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_encounter_startdatetime').val());
                            $(tr).find('td').eq(2).text($('#fld_encounter_enddatetime').val());
                            $(tr).find('td').eq(3).html(tinyMCE.activeEditor.getContent());
                            var workerid = "";
                            var workertext = "";
                            var delim1 = "";
                            var delim2 = "";
                            $('#fld_encounter_worker > option:selected').each(function () {
                                workerid += delim1 + $(this).val();
                                workertext += delim2 + $(this).text();
                                delim1 = '|';
                                delim2 = '<br />';
                            });
                            $(tr).find('td').eq(4).html(workertext);
                            $(tr).find('td').eq(4).attr("workerid", workerid);
                            $(tr).find('td').eq(5).text($('#fld_encounter_level option:selected').text());
                            $(tr).find('td').eq(5).attr('encounteraccesslevel', $('#fld_encounter_level').val());

                            tinymce.remove('.tinymce');
                            //$('#hidden_dirty').addClass('dirty');
                            makedirty();
                            $(this).dialog("close");
                        }
                    }
                }


                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this encounter?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            tinymce.remove('.tinymce');;
                            $(this).dialog("close");
                        }
                    }
                }

                $("#dialog_encounter").dialog('option', 'buttons', myButtons);
            })

            /* ========================================= EDIT WORKER ROLES ===========================================*/
            $(document).on('click', '.workerroleedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_workerrole").find(':input').val('');
                    $("#dialog_workerrole").find(':input').prop("disabled", false);
                } else {
                    tr = $(this).closest('tr');

                    $('#fld_workerrole_role').val($(tr).find('td').eq(1).attr('role_ctr'));
                    $('#fld_workerrole_startdate').val($(tr).find('td').eq(2).text());
                    $('#fld_workerrole_enddate').val($(tr).find('td').eq(3).text());
                    $('#fld_workerrole_note').val($(tr).find('td').eq(4).html());

                    $('#fld_workerrole_role').prop("disabled", true);
                    $('#fld_workerrole_startdate').prop("disabled", true);
                    if ($('#fld_workerrole_enddate').val() != "") {
                        $('#fld_workerrole_enddate').prop("disabled", true);
                        $('#fld_workerrole_note').prop("disabled", true);
                    }
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_workerrole").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true
                    , open: function (type, data) {
                        //$(this).appendTo($('form')); // reinsert the dialog to the form   
                        $("#form1 :button").prop("disabled", true);
                        $('#form1 :input[type="submit"]').prop('disabled', true);
                    }
                    , close: function (event, ui) {
                        //$('#fld_encounter_worker').select2('destroy');
                        $("#form1 :button").prop("disabled", false);
                        $('#form1 :input[type="submit"]').prop('disabled', false);
                    }
                    , appendTo: "#form2"
                });


                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#div_workerrole > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_workerrole > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'workerrole_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');

                            $(tr).find('td').eq(1).text($('#fld_workerrole_role option:selected').text());
                            $(tr).find('td').eq(1).attr('role_ctr', $('#fld_workerrole_role').val());
                            $(tr).find('td').eq(2).text($('#fld_workerrole_startdate').val());
                            $(tr).find('td').eq(3).text($('#fld_workerrole_enddate').val());
                            $(tr).find('td').eq(4).text($('#fld_workerrole_note').val());
                            $(this).dialog("close");
                        }
                    }
                }

                /*
                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this workerrole?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }
                */

                $("#dialog_workerrole").dialog('option', 'buttons', myButtons);
            })

            /* ========================================= EDIT ADDRESSES ===========================================*/
            $(document).on('click', '.addressedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_address").find(':input').val('');
                    $("#geocode").val('Refresh');
                } else {
                    tr = $(this).closest('tr');

                    $('#fld_address_address').val($(tr).find('td').eq(1).text());
                    //$('#fld_address_status').val($(tr).find('td').eq(2).text());
                    $('#fld_address_current').val($(tr).find('td').eq(2).attr('current'));
                    $('#fld_address_note').val($(tr).find('td').eq(3).text());
                    $('#fld_address_coordinates').val($(tr).find('td').eq(4).text());
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_address").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true
                    , open: function (type, data) {
                        //$(this).appendTo($('form')); // reinsert the dialog to the form   
                        $("#form1 :button").prop("disabled", true);
                        $('#form1 :input[type="submit"]').prop('disabled', true);
                    }
                    , close: function (event, ui) {
                        //$('#fld_encounter_worker').select2('destroy');
                        $("#form1 :button").prop("disabled", false);
                        $('#form1 :input[type="submit"]').prop('disabled', false);
                    }
                    , appendTo: "#form2"
                });


                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#div_address > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_address > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'address_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");
                            }
                            $(tr).attr('maint', 'changed');

                            $(tr).find('td').eq(1).text($('#fld_address_address').val());
                            //$(tr).find('td').eq(2).text($('#fld_address_status').val());
                            $(tr).find('td').eq(2).text($('#fld_address_current option:selected').text());
                            $(tr).find('td').eq(2).attr('current', $('#fld_address_current').val());
                            $(tr).find('td').eq(3).text($('#fld_address_note').val());
                            $(tr).find('td').eq(4).html('<a href="https://maps.google.com/?q=' + $('#fld_address_coordinates').val() + '" target="map">' + $('#fld_address_coordinates').val() + '</a>');

                            $(this).dialog("close");
                        }
                    }
                }

                /*
                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this address?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }
                */

                $("#dialog_address").dialog('option', 'buttons', myButtons);
            })

            
            function update_address() {
                /*----------------------------------------------ADDRESS-----------------------------------------*/
                delim = String.fromCharCode(254);
                $('#addresstable > tbody > tr[maint="changed"]').each(function () {

                    tr_id = $(this).attr('id');
                    tr_address = $(this).find('td:eq(1)').text();
                    tr_current = $(this).find('td:eq(2)').attr('current')
                    tr_notes = $(this).find('td:eq(3)').text();
                    tr_coordinates = $(this).find('td:eq(4)').text();

                    value = tr_address + delim + tr_current + delim + tr_notes + delim + tr_coordinates;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
                /*
                $('#addresstable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });
                */
            }

            /* ========================================= EDIT PHONES ===========================================*/
            $(document).on('click', '.phoneedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_phone").find(':input').val('');
                } else {
                    tr = $(this).closest('tr');

                    $('#fld_phone_number').val($(tr).find('td').eq(1).text());
                    //$('#fld_phone_mobile').val($(tr).find('td').eq(2).attr('mobile'));
                    //$('#fld_phone_own').val($(tr).find('td').eq(3).attr('own'));
                    //$('#fld_phone_sendtexts').val($(tr).find('td').eq(4).attr('sendtexts'));
                    $('#fld_phone_mobile').val($(tr).find('td').eq(2).text());
                    $('#fld_phone_own').val($(tr).find('td').eq(3).text());
                    $('#fld_phone_sendtexts').val($(tr).find('td').eq(4).text());
                    $('#fld_phone_note').val($(tr).find('td').eq(6).text());
                }
                if ($('#fld_phone_mobile').val() == 'Yes') {
                    $('#fld_phone_sendtexts').attr("disabled", false)
                } else {
                    $('#fld_phone_sendtexts').attr("disabled", true)
                }


                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_phone").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true
                    , open: function (type, data) {
                        //$(this).appendTo($('form')); // reinsert the dialog to the form   
                        $("#form1 :button").prop("disabled", true);
                        $('#form1 :input[type="submit"]').prop('disabled', true);
                    }
                    , close: function (event, ui) {
                        //$('#fld_encounter_worker').select2('destroy');
                        $("#form1 :button").prop("disabled", false);
                        $('#form1 :input[type="submit"]').prop('disabled', false);
                    }
                    , appendTo: "#form2"
                });


                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#div_phone > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_phone > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'phone_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");
                            }
                            $(tr).attr('maint', 'changed');

                            $(tr).find('td').eq(1).text($('#fld_phone_number').val());

                            //$(tr).find('td').eq(2).text($('#fld_phone_mobile option:selected').text());
                            //$(tr).find('td').eq(2).attr('mobile', $('#fld_phone_mobile').val());
                            $(tr).find('td').eq(2).text($('#fld_phone_mobile').val());

                            //$(tr).find('td').eq(3).text($('#fld_phone_own option:selected').text());
                            //$(tr).find('td').eq(3).attr('own', $('#fld_phone_own').val());
                            $(tr).find('td').eq(3).text($('#fld_phone_own').val());

                            //$(tr).find('td').eq(4).text($('#fld_phone_sendtexts option:selected').text());
                            //$(tr).find('td').eq(4).attr('sendtexts', $('#fld_phone_sendtexts').val());
                            $(tr).find('td').eq(4).text($('#fld_phone_sendtexts').val());

                            if ($('#fld_phone_sendtexts').val() == 'Yes') {
                                $(tr).find('td').eq(5).html('<a class="send_text">Send</a>');
                            } else {
                                $(tr).find('td').eq(5).text('');
                            }
                            $(tr).find('td').eq(6).text($('#fld_phone_note').val());

                            $(this).dialog("close");
                        }
                    }
                }

                /*
                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this phone?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }
                */

                $("#dialog_phone").dialog('option', 'buttons', myButtons);
            })

            $('#fld_phone_mobile').change(function () {
                if ($(this).val() == 'Yes') {
                    $('#fld_phone_sendtexts').attr("disabled", false)
                } else {
                    $('#fld_phone_sendtexts').val('');
                    $('#fld_phone_sendtexts').attr("disabled", true)
                }
            })

            function update_phone() {
                /*----------------------------------------------PHONE-----------------------------------------*/
                delim = String.fromCharCode(254);
                $('#phonetable > tbody > tr[maint="changed"]').each(function () {

                    tr_id = $(this).attr('id');
                    tr_number = $(this).find('td:eq(1)').text();
                    tr_mobile = $(this).find('td:eq(2)').text()
                    tr_own = $(this).find('td:eq(3)').text();
                    tr_sendtext = $(this).find('td:eq(4)').text();
                    tr_note = $(this).find('td:eq(6)').text();

                    value = tr_number + delim + tr_mobile + delim + tr_own + delim + tr_sendtext + delim + tr_note;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });

                $('#phonetable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });

            }


            /* ========================================= EDIT ASSIGNMENTS ===========================================*/
            $(document).on('click', '.assignededit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_assigned").find(':input').val('');
                    $("#dialog_assigned").find(':input').prop("disabled", false);
                    $('#fld_assigned_level').val(4);
                } else {
                    tr = $(this).closest('tr');
                    $('#fld_assigned_type').val($(tr).find('td').eq(1).text());
                    $('#fld_assigned_person').val($(tr).find('td').eq(2).text());
                    $('#fld_assigned_person').attr('person_ctr', $(tr).find('td').eq(2).attr('person_ctr'));
                    $('#fld_assigned_startdate').val($(tr).find('td').eq(3).text());
                    $('#fld_assigned_enddate').val($(tr).find('td').eq(4).text());
                    $('#fld_assigned_note').val($(tr).find('td').eq(5).html());
                    $('#fld_assigned_level').val($(tr).find('td').eq(6).attr('accesslevel'));

                    $('#fld_assigned_type').prop("disabled", true);
                    $('#fld_assigned_person').prop("disabled", true);
                    $('#fld_assigned_startdate').prop("disabled", true);
                    if ($('#fld_assigned_enddate').val() != "") {
                        $('#fld_assigned_enddate').prop("disabled", true);
                        $('#fld_assigned_note').prop("disabled", true);
                    }
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_assigned").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true
                    , open: function (type, data) {
                        //$(this).appendTo($('form')); // reinsert the dialog to the form   
                        $("#form1 :button").prop("disabled", true);
                        $('#form1 :input[type="submit"]').prop('disabled', true);
                    }
                    , close: function (event, ui) {
                        //$('#fld_encounter_worker').select2('destroy');
                        $("#form1 :button").prop("disabled", false);
                        $('#form1 :input[type="submit"]').prop('disabled', false);
                    }
                    , appendTo: "#form2"
                });


                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#div_assigned > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_assigned > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'assigned_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_assigned_type option:selected').text());

                            $(tr).find('td').eq(2).text($('#fld_assigned_person').val());
                            $(tr).find('td').eq(2).attr('person_ctr', $('#fld_assigned_person').attr('person_ctr'));

                            $(tr).find('td').eq(3).text($('#fld_assigned_startdate').val());
                            $(tr).find('td').eq(4).text($('#fld_assigned_enddate').val());
                            $(tr).find('td').eq(5).text($('#fld_assigned_note').val());

                            $(tr).find('td').eq(6).text($('#fld_assigned_level option:selected').text());
                            $(tr).find('td').eq(6).attr('accesslevel', $('#fld_assigned_level').val());

                            $(this).dialog("close");
                        }
                    }
                }

                /*
                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this assigned?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            //$(tr).remove
                            totalassigned();
                            alert('To do: Delete in database');
                            $(this).dialog("close");
                        }
                    }
                }
                */

                $("#dialog_assigned").dialog('option', 'buttons', myButtons);
            })

            /* ========================================= EDIT ENROLMENTS ===========================================*/
            $(document).on('click', '.enrolmentedit', function () {
                mode = $(this).data('mode');
                if (mode == "add") {
                    $("#dialog_enrolment").find(':input').val('');
                    //$("#dialog_enrolment").find(':input').prop("disabled", false);
                    $('#fld_enrolment_program').prop("disabled", false);
                } else {
                    tr = $(this).closest('tr');
                    $('#fld_enrolment_program').prop("disabled", true);

                    $('#fld_enrolment_program').val($(tr).find('td').eq(1).attr('programid'));
                    $('#fld_enrolment_startdate').val($(tr).find('td').eq(2).text());
                    $('#fld_enrolment_enddate').val($(tr).find('td').eq(3).text());
                    $('#fld_enrolment_status').val($(tr).find('td').eq(6).text());
                    $('#fld_enrolment_worker').val($(tr).find('td').eq(7).attr('worker'));
                    $('#fld_enrolment_alwayspickup').val($(tr).find('td').eq(8).attr('alwayspickup'));
                    $('#fld_enrolment_note').val($(tr).find('td').eq(9).text());
                }

                mywidth = $(window).width() * .95;
                if (mywidth > 800) {
                    mywidth = 800;
                }

                $("#dialog_enrolment").dialog({
                    resizable: false,
                    height: 600,
                    width: mywidth,
                    modal: true
                    , open: function (type, data) {
                        //$(this).appendTo($('form')); // reinsert the dialog to the form   
                        $("#form1 :button").prop("disabled", true);
                        $('#form1 :input[type="submit"]').prop('disabled', true);
                    }
                    , close: function (event, ui) {
                        //$('#fld_encounter_worker').select2('destroy');
                        $("#form1 :button").prop("disabled", false);
                        $('#form1 :input[type="submit"]').prop('disabled', false);
                    }
                    , appendTo: "#form2"
                });

                var myButtons = {
                    "Cancel": function () {
                        $(this).dialog("close");
                    },
                    "Save": function () {
                        if ($("#form2").valid()) {
                            if (mode == "add") {
                                tr = $('#div_enrolment > table > tbody tr:first').clone();
                                $(tr).removeAttr('style');
                                $('#div_enrolment > table > tbody > tr:last').after(tr);
                                $(tr).attr('id', 'enrolment_new_' + get_newctr());
                                $(tr).find('td:first').attr("class", "inserted");
                            } else {
                                $(tr).find('td:first').attr("class", "changed");

                            }
                            $(tr).attr('maint', 'changed');
                            $(tr).find('td').eq(1).text($('#fld_enrolment_program option:selected').text());
                            $(tr).find('td').eq(1).attr('programid', $('#fld_enrolment_program').val());
                            $(tr).find('td').eq(2).text($('#fld_enrolment_startdate').val());
                            $(tr).find('td').eq(3).text($('#fld_enrolment_enddate').val());

                            $(tr).find('td').eq(6).text($('#fld_enrolment_status option:selected').text());
                            //$(tr).find('td').eq(5).attr('status', $('#fld_enrolment_status').val());

                            $(tr).find('td').eq(7).text($('#fld_enrolment_worker option:selected').text());
                            $(tr).find('td').eq(7).attr('worker', $('#fld_enrolment_worker').val());

                            $(tr).find('td').eq(8).text($('#fld_enrolment_alwayspickup option:selected').text());
                            $(tr).find('td').eq(8).attr('alwayspickup', $('#fld_enrolment_alwayspickup').val());

                            $(tr).find('td').eq(9).text($('#fld_enrolment_note').val());
                            
                            $(this).dialog("close");
                        }
                    }
                }
               /*
                if (mode != 'add') {
                    myButtons["Delete"] = function () {
                        if (window.confirm("Are you sure you want to delete this enrolment?")) {
                            $(tr).find('td:first').attr("class", "deleted");
                            $(tr).attr('maint', 'deleted');
                            $(this).dialog("close");
                        }
                    }
                }
                */
                $("#dialog_enrolment").dialog('option', 'buttons', myButtons);
            })

            function update_enrolment() {
                /*----------------------------------------------ENROLMENT-----------------------------------------*/
                delim = String.fromCharCode(254);
                $('#enrolmenttable > tbody > tr[maint="changed"]').each(function () {

                    tr_id = $(this).attr('id');
                    tr_program = $(this).find('td:eq(1)').attr('programid');
                    tr_startdate = $(this).find('td:eq(2)').text();
                    tr_enddate = $(this).find('td:eq(3)').text();
                    tr_status = $(this).find('td:eq(6)').text();
                    tr_worker = $(this).find('td:eq(7)').attr('worker');
                    tr_alwayspickup = $(this).find('td:eq(8)').attr('alwayspickup');
                    tr_note = $(this).find('td:eq(9)').text();

                    value = tr_program + delim + tr_status + delim + tr_worker + delim + tr_alwayspickup + delim + tr_note + delim + tr_startdate + delim + tr_enddate;
                    $('<input>').attr({
                        type: 'hidden',
                        name: tr_id,
                        value: value
                    }).appendTo('#form1');
                });
                /*
                $('#enrolmenttable > tbody > tr[maint="deleted"]').each(function () {
                    tr_id = $(this).attr('id') + '_delete';
                    if (tr_id.substring(0, 3) != 'new') {
                        $('<input>').attr({
                            type: 'hidden',
                            name: tr_id,
                            value: ""
                        }).appendTo('#form1');
                    }
                });
                */
            }
            /*
            $('#btn_test').click(function () {
                $('#hidden_dirty').addClass('dirty');
            })
            */
        }); //document.ready

        function datalog(data) {
            if (data != lastdata && data.length > 20) {
                console.log(data);
            }
            lastdata = data;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%=username %>
    <!--
    <input type="button" value="test" id="btn_test" />
    <input type="text" id="hidden_dirty" />
    -->
    <div id="dialog_assistance" title="<%: Title + " Assistance"%>" style="display: none">
        <p>When adding a new record, enter the name fields and then click on the <span class="btn btn-info">Submit</span> button.  Tabs will then be shown to allow you to add more data.</p>
        <p>Making changes to the data under the various tabs will not be actioned until the record is submitted. </p>
        <p><b>Upload photo: </b>Click on the link and choose a photo from your computer or internet, crop it to the correct size and upload it.  It will be saved immediatly.</p>
        <p><b>Addresses: </b>Enter an address and then <span class="btn btn-info">Refresh</span> to verify it against Google Maps.  This will also format the address properly and resolve the co-ordinates.  Generally, you only need to enter the number and the street. " Where there are Google co-ordinates a link will allow you to go to that address in Google Maps.</p>
        <p><b>Encounters: </b>There are different levels of access given to Te Ora Hou workes.  Each worker is granted a level of between 1 and 4 for each person in the database.  They will only see the encounter notes where their access level is equal to or less than the level assigned to the encounter.  They may additionally see encounter notes where they have been recorded as a worker in that record.</p>
        <p>The levels are</p>
        <ul>
            <li>1. Confidential</li>
            <li>2. Highly sensitive</li>
            <li>3. Sensitive</li>
            <li>4. General</li>
        </ul>
        <p>Everyone has access to encounter notes at level 4.</p>
    </div>
    <div id="dialog_fullphoto" title="Full Photo" style="display: none">
        <img id="fullphoto" src="x" />
    </div>
    <div class="toprighticon">
        <input type="button" id="search" class="btn btn-info" value="Search" />
        <input type="button" id="assistance" class="btn btn-info" value="Assistance" />
        <input type="button" id="menu" class="btn btn-info" value="MENU" />
    </div>
    <div class="bottomrighticon">
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" class="submit btn btn-info" Text="Submit" />
    </div>
    <h1>Person Maintenance
    </h1>
    <div class="form-horizontal row">
        <div class="col-md-8">
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_firstname">First name</label>
                <div class="col-md-6">
                    <input id="fld_firstname" name="fld_firstname" type="text" class="form-control confirm" value="<%:fld_firstname%>" maxlength="20" required="required" />
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_knownas">Known as</label>
                <div class="col-md-6">
                    <input id="fld_knownas" name="fld_knownas" type="text" class="form-control confirm" value="<%:fld_knownas%>" maxlength="20" />
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-md-6" for="fld_surname">Surname</label>
                <div class="col-md-6">
                    <input id="fld_surname" name="fld_surname" type="text" class="form-control confirm" value="<%:fld_surname%>" maxlength="20" />
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <img id="img_photo" alt="" src="Images/<%: person_ctr %>.jpg" style="height: 200px" /><br />
            <a id="getphoto">Upload Photo</a> <%=photoalbumlink %>
        </div>
    </div>

    <div id="dialog_getphoto" title="Upload Photo" style="display: none">
        <div class="imagecontainer">
            <input type="file" id="fileInput" class="btn btn-info" accept="image/*" />
            <canvas id="canvas" style="display: none">Your browser does not support the HTML5 canvas element.
            </canvas>
            <br />
            <input type="button" id="btn_Crop" class="btn btn-info" value="Crop" style="display: none" />
            <div id="preview"></div>
            <div id="result"></div>
        </div>
    </div>
    <!------------------------------------------ TABS ------------------------------------------------------------>

    <div class="form-horizontal">
        <ul class="nav nav-tabs">
            <li class="active"><a data-target="#div_general">General</a></li>
            <%=html_tab %>
        </ul>

        <div class="tab-content">
            <!-- ================================= GENERAL TAB ===================================  -->
            <div id="div_general" class="tab-pane fade in active">
                <h3 class="tabheading">General</h3>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_gender">Gender</label>
                    <div class="col-sm-4">
                        <select id="fld_gender" name="fld_gender" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <% 
                                Dictionary<string, string> genderoptions = new Dictionary<string, string>();
                                genderoptions["type"] = "select";
                                genderoptions["valuefield"] = "label";
                                Response.Write(Generic.Functions.buildselection(genders, fld_gender, genderoptions));
                            %>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label for="fld_birthdate" class="control-label col-sm-4">
                        Date of birth
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group birthdate" data-showage="span_age">
                            <input id="fld_birthdate" name="fld_birthdate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" value="<%: fld_birthdate %>" />

                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>

                            <span id="span_age" class="input-group-addon"></span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_medical">Medical notes</label>
                    <div class="col-sm-8">
                        <textarea id="fld_medical" name="fld_medical" class="form-control"><%: fld_medical %></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_dietry">Dietary notes</label>
                    <div class="col-sm-8">
                        <textarea id="fld_dietary" name="fld_dietary" class="form-control"><%: fld_dietary %></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_notes">General notes</label>
                    <div class="col-sm-8">
                        <textarea id="fld_notes" name="fld_notes" class="form-control"><%: fld_notes %></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_photoalbumlink">Photo album link</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_photoalbumlink" name="fld_photoalbumlink" class="form-control" value="<%: fld_photoalbumlink %>" />
                    </div>
                </div>

                 <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_windowsuser">Windows User</label>
                    <div class="col-sm-8">
                        <input readonly="readonly" type="text" id="fld_windowsuser" name="fld_windowsuser" class="form-control" value="<%: fld_windowsuser %>" />
                    </div>
                </div>   



            </div>



            <!-- ================================= ENROLMENTS TAB ===================================  -->
            <div id="div_enrolment" class="tab-pane fade in">
                <h3 class="tabheading">Enrolments</h3>
                <table id="enrolmenttable" class="table" style="width: 100%">
                    <%= html_enrolments %>
                </table>
            </div>

            <!-- ================================= ENROLMENTS DIALOG ===================================  -->

            <div id="dialog_enrolment" title="Maintain enrolments" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_enrolment_program">Program</label>
                    <div class="col-sm-8">
                        <select id="fld_enrolment_program" name="fld_enrolment_program" class="form-control" required="required">
                            <%                                                                                                                                            
                                //string[] nooptions = { }; 
                                Dictionary<string, string> enrolment_programoptions = new Dictionary<string, string>();
                                enrolment_programoptions["type"] = "select";
                                enrolment_programoptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(programs, nooptions, enrolment_programoptions));
                            %>
                        </select>
                    </div>
                </div>

                 <div class="form-group">
                    <label for="fld_enrolment_startdate" class="control-label col-sm-4">
                        Start Date<br /><span style="font-size:smaller">(Need to ensure can not prior to first event)</span>
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_enrolment_startdate" name="fld_enrolment_startdate" required="required" placeholder="eg: 23 Jun 1985" type="text" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label for="fld_enrolment_enddate" class="control-label col-sm-4">
                        End Date<br /><span style="font-size:smaller">(Need to ensure can not be prior to last event)</span>
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_enrolment_enddate" name="fld_enrolment_enddate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_enrolment_status">Status</label>
                    <div class="col-sm-8">
                        <select id="fld_enrolment_status" name="fld_enrolment_status" class="form-control" required="required">
                            <%                                                                                                                                            
                                //string[] nooptions = { }; 
                                Dictionary<string, string> enrolment_statusoptions = new Dictionary<string, string>();
                                enrolment_statusoptions["type"] = "select";
                                enrolment_statusoptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(enrolmentstatus, nooptions, enrolment_statusoptions));
                            %>
                        </select>
                    </div>
                </div>

                 <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_enrolment_alwayspickup">Always Pickup</label>
                    <div class="col-sm-8">
                        <select id="fld_enrolment_alwayspickup" name="fld_enrolment_alwayspickup" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                //string[] nooptions = { }; //temp
                                Dictionary<string, string> YesNoOptions = new Dictionary<string, string>();
                                YesNoOptions["type"] = "select";
                                YesNoOptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(YesNoBit, nooptions, YesNoOptions));
                            %>
                        </select>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_enrolment_worker">Worker</label>
                    <div class="col-sm-8">
                        <select id="fld_enrolment_worker" name="fld_enrolment_worker" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                //string[] nooptions = { }; //temp
                                YesNoOptions["type"] = "select";
                                YesNoOptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(YesNoBit, nooptions, YesNoOptions));
                            %>
                        </select>
                    </div>
                </div>



                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_enrolment_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_enrolment_note" name="fld_enrolment_note" class="form-control tinymce"></textarea>
                    </div>
                </div>
            </div>

            <!-- ================================= ENCOUNTERS TAB ===================================  -->
            <div id="div_encounter" class="tab-pane fade in">
                <h3 class="tabheading">Encounters</h3>
                <table id="encountertable" class="table" style="width: 100%">
                    <%= html_encounter %>
                </table>
            </div>

            <!-- ================================= ENCOUNTERS DIALOG ===================================  -->
            <div id="dialog_encounter" title="Maintain encounters" style="display: none" class="form-horizontal">

                <div class="form-group">
                    <label for="fld_encounter_startdatetime" class="control-label col-sm-4">
                        Start Date/Time
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group datetime">
                            <input id="fld_encounter_startdatetime" name="fld_encounter_startdatetime" placeholder="eg: 23 Jun 1985 21:00" type="text" class="form-control" required="required" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label for="fld_encounter_enddatetime" class="control-label col-sm-4">
                        End Date/Time
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group datetime">
                            <input id="fld_encounter_enddatetime" name="fld_encounter_enddatetime" placeholder="eg: 23 Jun 1985 22:00" type="text" class="form-control" required="required" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_encounter_narrative">Narrative</label>
                    <div class="col-sm-8">
                        <textarea id="fld_encounter_narrative" name="fld_encounter_narrative" class="form-control tinymce"></textarea>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_encounter_worker">Worker(s)</label>
                    <div class="col-sm-8">
                        <select id="fld_encounter_worker" name="fld_encounter_worker" class="form-control" required="required" multiple="multiple">
                            <%                                                                                                                                            
                                //string[] nooptions = { }; 
                                Dictionary<string, string> encounter_workeroptions = new Dictionary<string, string>();
                                encounter_workeroptions["type"] = "select";
                                encounter_workeroptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(workers, nooptions, encounter_workeroptions));
                            %>
                        </select>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_encounter_level">Access level</label>
                    <div class="col-sm-8">
                        <select id="fld_encounter_level" name="fld_encounter_level" class="form-control" required="required">
                            <%                                                                                                                                            
                                //string[] noleveloptions = { };  
                                Dictionary<string, string> encounter_leveloptions = new Dictionary<string, string>();
                                encounter_leveloptions["type"] = "select";
                                encounter_leveloptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(encounterAccessLevels, nooptions, encounter_leveloptions));
                            %>
                        </select>
                    </div>
                </div>
            </div>

            <!-- ================================= PHONES TAB ===================================  -->
            <div id="div_phone" class="tab-pane fade in">
                <h3 class="tabheading">Phones</h3>
                <table id="phonetable" class="table" style="width: 100%">
                    <%= html_phones %>
                </table>
            </div>

            <!-- ================================= PHONES DIALOG ===================================  -->
            <div id="dialog_phone" title="Maintain Phones" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_phone_number">Number</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_phone_number" name="fld_phone_number" class="form-control" required="required" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_phone_mobile">Mobile</label>
                    <div class="col-sm-8">
                        <select id="fld_phone_mobile" name="fld_phone_mobile" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                //string[] nooptions = { }; //temp
                                //Dictionary<string, string> YesNoOptions = new Dictionary<string, string>();
                                YesNoOptions["type"] = "select";
                                YesNoOptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(YesNo, nooptions, YesNoOptions));
                            %>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_phone_own">Own</label>
                    <div class="col-sm-8">
                        <select id="fld_phone_own" name="fld_phone_own" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                //string[] nooptions = { }; //temp
                                //Dictionary<string, string> YesNoOptions = new Dictionary<string, string>();
                                //YesNoOptions["type"] = "select";
                                //YesNoOptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(YesNo, nooptions, YesNoOptions));
                            %>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_phone_sendtexts">Send Texts</label>
                    <div class="col-sm-8">
                        <select id="fld_phone_sendtexts" name="fld_phone_sendtexts" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                //string[] nooptions = { }; //temp
                                //Dictionary<string, string> YesNoOptions = new Dictionary<string, string>();
                                //YesNoOptions["type"] = "select";
                                //YesNoOptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(YesNo, nooptions, YesNoOptions));
                            %>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_phone_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_phone_note" name="fld_phone_note" class="form-control"></textarea>
                    </div>
                </div>
            </div>

            <div id="dialog-sendtext" title="Send Text" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-4" for="tb_textmessage">Message</label>
                    <div class="col-sm-8">
                        <textarea id="tb_textmessage" name="tb_textmessage" class="form-control"></textarea>
                    </div>
                </div>
            </div>

            <!-- ================================= EMAIL TAB ===================================  -->
            <div id="div_email" class="tab-pane fade in">
                <h3 class="tabheading">Email</h3>
                <table id="emailtable" class="table" style="width: 100%">
                    <%= html_email %>
                </table>
            </div>
            <!-- ================================= EMAIL DIALOG ===================================  -->

            <div id="dialog-sendemail" title="Send Email" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-4" for="tb_subject">Subject</label>
                    <div class="col-sm-8">
                        <input id="tb_subject" name="tb_subject" type="text" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="tb_body">Message</label>
                    <div class="col-sm-8">
                        <textarea id="tb_body" name="tb_body" class="form-control" rows="6"></textarea>
                    </div>
                </div>
            </div>


            <!-- ================================= ADDRESSES TAB ===================================  -->
            <div id="div_address" class="tab-pane fade in">
                <h3 class="tabheading">Addresss</h3>
                <table id="addresstable" class="table" style="width: 100%">
                    <%= html_addresses %>
                </table>
            </div>

            <!-- ================================= ADDRESSES DIALOG ===================================  -->
            <div id="dialog_address" title="Maintain addresss" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_address_address">Address</label>
                    <div class="col-sm-8">
                        <textarea id="fld_address_address" name="fld_address_address" class="form-control" rows="6"></textarea>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_address_current">Current</label>
                    <div class="col-sm-8">

                        <select id="fld_address_current" name="fld_address_current" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                //string[] nooptions = { }; //temp
                                //Dictionary<string, string> YesNoOptions = new Dictionary<string, string>();
                                //YesNoOptions["type"] = "select";
                                //YesNoOptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(YesNoBit, nooptions, YesNoOptions));
                            %>
                        </select>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_address_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_address_note" name="fld_address_note" class="form-control"></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_address_coordinates">Google Co-ordinates</label>
                    <div class="col-sm-8">
                        <input id="fld_address_coordinates" name="fld_address_coordinates" class="form-control" readonly="readonly" />
                        <input id="geocode" type="button" class="geocode btn btn-info" value="Refresh" />
                    </div>
                </div>
            </div>

            <!-- ================================= WORKER ROLE TAB ===================================  -->
            <div id="div_workerrole" class="tab-pane fade in">
                <h3 class="tabheading">Worker Roles</h3>
                <span>Record roles and periods that this person was designated as a worker.</span>
                <table id="workerroletable" class="table" style="width: 100%">
                    <%= html_workerroles %>
                </table>
            </div>

            <!-- ================================= WORKER ROLE DIALOG ===================================  -->
            <div id="dialog_workerrole" title="Maintain worker roles" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_workerrole_role">Role</label>
                    <div class="col-sm-8">
                        <select id="fld_workerrole_role" name="fld_workerrole_role" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                //string[] nooptions = { }; //temp
                                Dictionary<string, string> workerroleoptions = new Dictionary<string, string>();
                                workerroleoptions["type"] = "select";
                                workerroleoptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(workerRoles, nooptions, workerroleoptions));
                            %>
                        </select>
                    </div>
                </div>

                <div class="form-group">
                    <label for="fld_workerrole_startdate" class="control-label col-sm-4">
                        Start Date 
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_workerrole_startdate" name="fld_workerrole_startdate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" required="required" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label for="fld_workerrole_enddate" class="control-label col-sm-4">
                        End Date 
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_workerrole_enddate" name="fld_workerrole_enddate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_workerrole_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_workerrole_note" name="fld_workerrole_note" class="form-control tinymce"></textarea>
                    </div>
                </div>
            </div>

            <!-- ================================= ATTENDANCE TAB ===================================  -->
            <div id="div_attendance" class="tab-pane fade in">
                <h3 class="tabheading">Attendance</h3>
                <table id="attendancetable" class="table" style="width: 100%">
                    <%= html_attendance %>
                </table>
            </div>

            <!-- ================================= ASSIGNED TAB ===================================  -->
            <div id="div_assigned" class="tab-pane fade in">
                <h3 class="tabheading">Assigned Workers</h3>
                <span>Record workers that are assigned to this person.</span>
                <table id="assignedtable" class="table" style="width: 100%">
                    <%= html_assigned %>
                </table>
            </div>

            <!-- ================================= ASSIGNED DIALOG ===================================  -->
            <div id="dialog_assigned" title="Maintain Assigned Workers" style="display: none" class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_assigned_type">The person you are linking to is a</label>
                    <div class="col-sm-8">
                        <select id="fld_assigned_type" name="fld_assigned_type" class="form-control" required="required">
                            <option value="">--- Please select ---</option>
                            <%                                                                                                                                            
                                Dictionary<string, string> assignmenttypeoptions = new Dictionary<string, string>();
                                assignmenttypeoptions["type"] = "select";
                                Response.Write(Generic.Functions.buildselection(assignmenttypes, nooptions, assignmenttypeoptions));
                            %>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_assigned_person">Person</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_assigned_person" name="fld_assigned_person" class="form-control" required="required" />
                    </div>
                </div>


                <div class="form-group">
                    <label for="fld_assigned_startdate" class="control-label col-sm-4">
                        Start Date
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_assigned_startdate" name="fld_assigned_startdate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" required="required" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label for="fld_assigned_enddate" class="control-label col-sm-4">
                        End Date 
                    </label>
                    <div class="col-sm-8">
                        <div class="input-group standarddate">
                            <input id="fld_assigned_enddate" name="fld_assigned_enddate" placeholder="eg: 23 Jun 1985" type="text" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_assigned_note">Note</label>
                    <div class="col-sm-8">
                        <textarea id="fld_assigned_note" name="fld_assigned_note" class="form-control"></textarea>
                    </div>
                </div>

                <div class="form-group" <%=show_assigned_level %>>
                    <label class="control-label col-sm-4" for="fld_assigned_level">Level</label>
                    <div class="col-sm-8">
                        <select id="fld_assigned_level" name="fld_assigned_level" class="form-control" required="required">
                            <%                                                                                                                                            
                                //string[] noleveloptions = { };  
                                Dictionary<string, string> Allencounter_leveloptions = new Dictionary<string, string>();
                                Allencounter_leveloptions["type"] = "select";
                                Allencounter_leveloptions["valuefield"] = "value";
                                Response.Write(Generic.Functions.buildselection(AllencounterAccessLevels, nooptions, Allencounter_leveloptions));
                            %>
                        </select>
                    </div>
                </div>
            </div>
        </div>
        <!-- ================================= END OF TABS ===================================  -->
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <form id="form2">
    </form>
</asp:Content>

