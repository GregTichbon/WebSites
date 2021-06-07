<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="cntrvibe.iti.ninja.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.1/jquery.validate.min.js"></script>

    <style>
        table.entry {
            border: 1px solid black;
            background-color: white;
            width: 100%;
            text-align: left;
            border-collapse: collapse;
        }

            table.entry td {
                border: 1px solid black;
                padding: 3px 2px;
            }

            table.entry tbody td {
                font-size: 13px;
            }

        .centered {
            position: absolute;
            top: 40%;
            left: 45%;
            z-index: 9999;
        }
    </style>

    <script type="text/javascript">


        $(document).ready(function () {
            $("#form1").validate({
                rules: {
                    fld_confirmemailaddress: {
                        equalTo: '#fld_emailaddress'
                    },

                    messages: {
                        fld_confirmemailaddress: {
                            equalTo: 'This must be the same as the email address above'
                        }
                    }
                }
            });

            $("#tabs").tabs();

            $(document).uitooltip({
                content: function () {
                    return $(this).prop('title');
                }
            });

            $('#fld_type').change(function () {
                val = $(this).val();
                if (val == 'Group') {
                    $('#div_group').show();
                } else {
                    $('#div_group').hide();
                    $('#fld_groupname').val('');
                    $('#fld_groupnumber').val('');
                }
            });

            $(".numeric").keydown(function (event) {

                if (event.shiftKey == true) {
                    event.preventDefault();
                }

                if ((event.keyCode >= 48 && event.keyCode <= 57) ||
                    (event.keyCode >= 96 && event.keyCode <= 105) ||
                    event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 ||
                    event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190) {

                } else {
                    event.preventDefault();
                }

                if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
                    event.preventDefault();
                //if a decimal has been added, disable the "."-button

            });

            mywidth = $(window).width() * .95;
            if (mywidth > 800) {
                mywidth = 800;
            }

            $('#btn_test').click(function () {
                $('#processing').toggle();
            });

            $('#btn_submit').click(function () {
                
                valid = $("#form1").valid();
                if (valid) {
        
                    $('#processing').show();
                    
                    var arForm = $("#form1")
                        .find("input,textarea,select,hidden")
                        .not("[id^='__']")
                        .serializeArray();

                    //var arForm = [{ "name": "URL", "value": "Test" }, { "name": "emailaddress", "value": "xxx" }];  //THIS WORKS
                    //arForm.push({ name: 'vehicle_ctr', value: $('#vehicle_ctr').val() });  //THIS WORKS
                    var formData = JSON.stringify({ formVars: arForm });

                    $.ajax({
                        type: 'POST', // define the type of HTTP verb we want to use (POST for our form)
                        url: '/_dependencies/posts.asmx/update_registration', // the url where we want to POST
                        contentType: "application/json; charset=utf-8",
                        data: formData,
                        dataType: 'json', // what type of data do we expect back from the server
                        async: false,
                        success: function (result) {
                            entry_ctr = result.d.id;
                        },
                        error: function (xhr, status) {
                            alert('error');
                        }
                    });

                    $('#form1')[0].reset();
                    $('#processing').hide();
                    $.post("/_Dependencies/data.aspx", { mode: "get_entry", entry_ctr: entry_ctr })
                        .done(function (data) {
                            $("#div_entry").html(data);
                        });



                    $("#div_dialog").dialog({
                        resizable: false,
                        height: 600,
                        width: mywidth,
                        modal: true
                        , open: function (type, data) {

                        }
                        , close: function (event, ui) {
                        }

                    });
                }
            });



            /*
            $('#home').click(function () {
                $('#div_rules').hide();
                $('#div_organisers').hide();
                $('#div_registration').hide();
                $('#div_kidscan').hide();
                $('#div_home').show();
            });
            $('#rules').click(function () {
                $('#div_home').hide();
                $('#div_organisers').hide();
                $('#div_registration').hide();
                $('#div_kidscan').hide();
                $('#div_rules').show();
            });
            $('#organisers').click(function () {
                $('#div_home').hide();
                $('#div_rules').hide();
                $('#div_registration').hide();
                $('#div_kidscan').hide();
                $('#div_organisers').show();
            });
            $('#registration').click(function () {
                $('#div_home').hide();
                $('#div_rules').hide();
                $('#div_organisers').hide();
                $('#div_kidscan').hide();
                $('#div_registration').show();
            });
            $('#kidscan').click(function () {
                $('#div_home').hide();
                $('#div_rules').hide();
                $('#div_organisers').hide();
                $('#div_registration').hide();
                $('#div_kidscan').show();
            });
            */

        });

    </script>
    <style type="text/css">
        .auto-style1 {
            width: 817px;
            height: 970px;
            text-align: center;
        }

        .auto-style2 {
            width: 827px;
            height: 1170px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">

        <div id="div_dialog" title="Registration Acknowledgment" style="display: none">
            <p>Thanks for your registration.</p>
            <p>A copy of your registration has been sent to your email address.  Please contact us if anything changes, we will be in touch.</p>
            <div id="div_entry"></div>
        </div>
        <!--<a id="home">Home</a> -----         <a id="rules">Rules</a> -----         <a id="organisers">The Organisers</a> -----         <a id="registration">Registration</a>  -----         <a id="kidscan">KidsCan</a>-->
        <div id="tabs">

            <ul>
                <li><a href="#div_home">Home</a></li>
                <li><a href="#div_rules">Rules</a></li>
                <li><a href="#div_about">About</a></li>
                <li><a href="#div_registration">Registration</a></li>
                <li><a href="#div_kidscan">KidsCan</a></li>
            </ul>

            <div id="div_home">
                <span class="logoright">
                    <img src="_Dependencies/Images/Logo.PNG" style="width: 200px" />
                </span>
                <h2>WHANGANUI'S VERY OWN YOUTH LED<br />
                    RANGATAHI TALENT QUEST</h2>
                <br />
                A talent quest for Whanganui rangatahi ages 12-24
                <br />
                <br />
                <b>Auditions</b>: These will be held at Te Ora Hou Whanganui - 34 Manuka St at 3:30 - 6:00pm on 18th June. Please register with the link above.
                <br />
                <br />

                When: <strong>Saturday 3rd July 4:00pm - 7:00pm.</strong><br />
                Venue: Rutherford Junior High School<br />
                Prizes: <strong>1st place - $500.00, 2nd place - $300.00, 3rd place $100.00.&nbsp; Plus spot prizes!</strong><br />
                Cost: <strong>$2.00 entrance fee, under 12's free.</strong><br />
                <br />
                All profit is going to KidsCan<br />
                <br />
                Catch us on
                <a href="https://www.instagram.com/cntrvibe" target="_blank">Instagram</a> and <a href="https://www.facebook.com/events/319800449631925" target="_blank">Facebook</a>
                <br />
                <br />
                Supported by Praxis Youth Training,
                Te Ora Hou Whanganui, and DJ Watties
                <br />
                &nbsp;<br />
                <img src="_Dependencies/Images/SharpAs.JPG" style="width: 43%" />
                <img src="_Dependencies/Images/KidsCan.PNG" style="float: right; width: 32%" />
            </div>
            <div id="div_rules">
                <span class="logoright">
                    <img src="_Dependencies/Images/Logo.PNG" style="width: 200px" />
                </span>
                <h2>The Rules</h2>
                <p><strong>This is an alcohol and drug free event</strong></p>
                <p><strong>Auditions:</strong></p>
                <p>All acts must attend an audition prior to the main talent quest.</p>
                <p>The auditions will be held at Te Ora Hou Whanganui - 34 Manuka St at 3:30 - 6:00pm on 18th June</p>
                <p><strong>Categories:</strong></p>
                <p>What ever showcases yours or your group&#39;s talents in the performing arts.&nbsp; This could be music, vocals, dance, magic, juggling, balancing, spoken word, etc etc etc, or any combination of these.</p>
                <p><strong>Time Limits:</strong></p>
                <p>You can have upto 5 minutes for setup and upto 5 minutes for your performance.&nbsp; (It is very important that you give us as much information on your requirements as possible to be able to achieve quick setups)</p>
                <p><strong>Who is it for?</strong></p>
                <p>You and/or everyone in your group entering the talent quest must be aged 12 to 24 on the day of the talent quest (Saturday 3 July)</p>
                <p>This is an event for Whanganui rangatahi, unfortunately the competition is not open to people or groups from outside of our region.</p>
                <p>But everone is welcome to come and join in the fun and support the acts.</p>
                <p><strong>Judging:</strong></p>
                <p>Judging will be based on talent, skill, and entertainment value.</p>
                <p>The judges decision will be final and no discussion will be entered into.</p>
                <p><strong>Appropriateness and Quality:</strong></p>
                <p>Absolutely no profanity or insensitive subject matter will be permitted in songs or music. </p>
                <p>Costuming must be appropriate for all age groups.</p>
                <p>The organisers maintain the right to not allow any person or group to perform on the basis of appropriateness, suitability to the audience, quality and/or any other reason they may consider significant. </p>
                <p><strong>Equipment</strong></p>
                <p>The performers are responsible for their own props, costumes, backing tracks, and all other equipment. It is the participant’s responsibility to get these back after the auditions and talent quest.</p>
            </div>
            <div id="div_kidscan">
                <span class="logoright">
                    <img src="_Dependencies/Images/Logo.PNG" style="width: 200px" />
                </span>
                <h2>KidsCan</h2>
                <p>We are proud to be supporting <strong>KidsCan</strong> by donating all of the proceeds from the door sales.</p>
                <p>Please read all about the amazing work that <strong>KidsCan</strong> does throughout New Zealand.</p>
                <a href="_Dependencies/Images/KidsCanBrochure.pdf">Download the KidsCan Brochure</a>
                <br />
                <img alt="" class="auto-style2" src="_Dependencies/Images/KidsCanBrochure_Page_1.jpg" />
                <img alt="" class="auto-style2" src="_Dependencies/Images/KidsCanBrochure_Page_2.jpg" />
                <img alt="" class="auto-style2" src="_Dependencies/Images/KidsCanBrochure_Page_3.jpg" />
                <img alt="" class="auto-style2" src="_Dependencies/Images/KidsCanBrochure_Page_4.jpg" />
            </div>
            <div id="div_about">
                <span class="logoright">
                    <img src="_Dependencies/Images/Logo.PNG" style="width: 200px" />
                </span>
                <h2>About</h2>
                <p>Kia ora, I am Zeana Thomas.&nbsp; I am a youth worker studying with Praxis NZ.&nbsp; A group of young girls; Unique, Zion, Charis, and Sarah and I wanted to create a fun event for our rangatahi (12 - 24) and so have created <strong>cntrvibe</strong>. It’s youth led and for youth. </p>
                <p>We are extremely grateful for our very generous sponsor; <strong>Sharp As LineHaul Limited</strong>. <strong>Sharp As</strong> is a cargo and freight company that is a family business owned by Kylie and Daryl James of Whanganui. </p>
                <p>We are proud to be able to support KidsCan, check out more about them by clicking on the tab above.</p>
                <p>
                    <img alt="The Girls organisers" class="auto-style1" src="_Dependencies/Images/Girls.jpg" />
                </p>
            </div>
            <div id="div_registration" class="form-horizontal" style="position: relative;">
                <span class="logoleft">
                    <img src="_Dependencies/Images/Logo.PNG" style="width: 200px" />
                </span>
                <div id="processing" style="display: none"  class="centered">
                    <img src="/_Dependencies/Images/processing.gif" />
                </div>
                <div class="form-group">
                    <div class="col-sm-4"></div>
                    <div class="col-sm-8">
                        <h2>Registration</h2>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_type">Registering as a</label>
                    <div class="col-sm-8">
                        <select id="fld_type" name="fld_type" class="form-control" required="required">
                            <option value="">--- Please Select ---</option>
                            <option value="Individual">Individual</option>
                            <option value="Group">Group</option>
                        </select>
                    </div>
                </div>
                <div id="div_group" style="display: none">
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="fld_groupname">
                            Group name</label>
                        <div class="col-sm-8">
                            <input type="text" id="fld_groupname" name="fld_groupname" class="form-control" required="required" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="fld_groupnumber">
                            Number of people in group 
                            <img class="helpicon" src="_Dependencies/Images/qm.png" title="Those people on stage or actually involved in the performance." />
                        </label>
                        <div class="col-sm-8">
                            <input type="text" id="fld_groupnumber" name="fld_groupnumber" class="form-control numeric" required="required" maxlength="2" />
                        </div>
                    </div>
                    <div class="col-sm-4"></div>
                    <div class="col-sm-8">
                        <label class="control-label">Contact Person</label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_firstname">First name</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_firstname" name="fld_firstname" class="form-control" required="required" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_surname">Surname</label>
                    <div class="col-sm-8">
                        <input type="text" id="fld_surname" name="fld_surname" class="form-control" required="required" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_emailaddress">Email address</label>
                    <div class="col-sm-8">
                        <input type="email" id="fld_emailaddress" name="fld_emailaddress" class="form-control" required="required" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_confirmemailaddress">Please confirm your email address</label>
                    <div class="col-sm-8">
                        <input type="email" id="fld_confirmemailaddress" name="fld_confirmemailaddress" class="form-control" required="required" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_contact">
                        Other Contact details
                        <img class="helpicon" src="_Dependencies/Images/qm.png" title="What are the best ways to get hold of you? eg: Phone number(s), Other email addresses, Messenger apps etc." /></label>
                    <div class="col-sm-8">
                        <textarea id="fld_contact" name="fld_contact" class="form-control" required="required"></textarea>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_description">
                        Description
                        <img class="helpicon" src="_Dependencies/Images/qm.png" title="Please provide as much information as possible eg: Type of performance, style etc." /></label>
                    <div class="col-sm-8">
                        <textarea id="fld_description" name="fld_description" class="form-control" required="required"></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_requirements">
                        Requirements
                        <img class="helpicon" src="_Dependencies/Images/qm.png" title="Please provide as much information as possible eg: Stage area, number of microphones, power for amps, keyboards etc." /></label>
                    <div class="col-sm-8">
                        <textarea id="fld_requirements" name="fld_requirements" class="form-control" required="required"></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_otherinformation">Other useful information</label>
                    <div class="col-sm-8">
                        <textarea id="fld_otherinformation" name="fld_otherinformation" class="form-control"></textarea>
                    </div>
                </div>
                <div class="form-group" style="display: none">
                    <label class="control-label col-sm-4" for="fld_uploads">
                        Photos
                        <img class="helpicon" src="_Dependencies/Images/qm.png" title="These may be useful for better understanding your act and may be used in publicity." /></label>
                    <div class="col-sm-8">
                        <input id="fld_uploads" name="fld_uploads" type="file" multiple="multiple" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4" for="fld_declaration">Declaration</label>
                    <div class="col-sm-8">
                        I confirm that:<br />
                        <div style="float: left; width: 50px;">
                            <input id="fld_declaration" name="fld_declaration" class="form-control" type="checkbox" value="Delarations made." required="required" />
                        </div>
                        <div style="overflow: hidden;">
                            <ul>
                                <li>All participants are between the ages of 12 and 24 on the day of CNTR VIBE and that all reside in the Whanganui Area</li>
                                <li>I have read and agree to the rules</li>
                                <li>Photo, video, and audio from the day of the event may be used for at the organisers discretion. </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4"></div>
                    <div class="col-sm-8">
                        <input type="button" id="btn_submit" class="btn btn-info" value="Submit" />
                      <!--   <input type="button" id="btn_test" class="btn btn-info" value="Test" />-->
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
