<%@ Page Title="" Language="C#" MasterPageFile="~/Auction.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Auction.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="/_dependencies/Scripts/Magnifier/magnify.css" />

    <style>
        body {
            font-family: 'Montserrat', sans-serif;
        }

        .item-slideshow {
            max-height: 250px;
        }

            .item-slideshow img {
                width: auto;
                height: auto;
                max-height: 100%;
                max-width: 100%;
            }

        .donor-slideshow {
            height: 100px;
        }

            .donor-slideshow img {
                width: auto;
                height: 100%;
            }

        .stop-scrolling {
            height: 100%;
            overflow: hidden;
        }

        .stop-scrollingInfo {
            height: 100%;
            overflow: hidden;
        }

        .cycle-slide {
            height: 200px;
        }

        .hidden {
            display: none;
        }

        .categoryselected {
            background-color: green;
            color: white;
        }
    </style>
    <script src="/_Dependencies/Scripts/cycle2/jquery.cycle2.min.js"></script>
    <script src="/_Dependencies/Scripts/cycle2/jquery.cycle2.center.min.js"></script>

    <script>
        $(document).ready(function () {

            var stopscrolling = -1;
            var showingitem = false;

            <%=startupmessage%>

            $(window).scroll(function (e) {
                if (stopscrolling != -1) {
                    //$('html, body').animate({ scrollTop: stopscrolling }, "fast");
                    $('html, body').scrollTop(stopscrolling);
                    //$('#debug').html($('#debug').html() + '<br />scrolling=' + stopscrolling);                   
                }
            });

            $('.categoryselect').click(function () {
                $('.categoryselect').removeClass('categoryselected');
                $(this).addClass('categoryselected');
                id = $(this).attr('id').substring(13);
                $('.div_category').each(function () {
                    if ($(this).attr('data-category') == id || id == 'All') {
                        $(this).removeClass('hidden');
                    } else {
                        $(this).addClass('hidden');
                    }
                })
                //$('.hidden').removeClass('hidden');
                //$('#' + id).toggleClass("hidden");
            })


            $(".showitem, .canclick").click(function () {
                $('#div_main').hide();
                itemid = this.id.substring(8);
                $.post("/_Dependencies/data.aspx", { mode: "get_item", id: itemid })
                    .done(function (data) {
                        console.log(data);
                        $("#div_item_images").html(data);
                    });
                $('#div_item').show();
            });

            $(".returnfromitem").click(function () {
                thisdiv = $(this).data('thisdiv');
                $('#' + thisdiv).hide();
                $('#div_main').show();
            })

            $('body').on('click', '.register', function () {
                thisdiv = $(this).data('thisdiv');
                $('#' + thisdiv).hide();
                $('#div_register').show();
            })

            $('body').on('click', '.login', function () {
                thisdiv = $(this).data('thisdiv');
                $('#' + thisdiv).hide();
                $('#div_login').show();
            })

            $('#btn_login').click(function () {
                thisdiv = $(this).data('thisdiv');
                $('#' + thisdiv).hide();
                $('#div_item').show();
            })

            $('#btn_register').click(function () {
                thisdiv = $(this).data('thisdiv');
                $('#' + thisdiv).hide();
                $('#div_item').show();
            })


            $('#informationIcon').click(function () {
                $('body').addClass('stop-scrollingInfo');
                stopscrolling = window.pageYOffset;
                $('#dialog_showinformation').dialog({
                    modal: true,
                    open: function () {
                        $(this).load('ItemsInformation.aspx');
                    },
                    width: ($(window).width() - 0) * .95,  //75 x 2 is the width of the question mark top right
                    height: 800,
                    close: function () {
                        $('body').removeClass('stop-scrollingInfo');
                        if (!showingitem) {
                            stopscrolling = -1;
                        }
                    }, title: 'Bidding Information',
                    closeText: false
                });
            })

            // $('.donor-link').click(function () {
            //     alert($(this).prop('href'));
            // });

            $('.slideshow').cycle();
            $('.showitem, .canclick').css('cursor', 'pointer');

        });  //document.ready

        function startupmessage(pageurl, pagetitle) {
            $('body').addClass('stop-scrollingInfo');
            stopscrolling = window.pageYOffset;
            $('#dialog_showinformation').dialog({
                modal: true,
                open: function () {
                    $(this).load(pageurl);
                },
                width: ($(window).width() - 0) * .95,  //75 x 2 is the width of the question mark top right
                height: 800,
                close: function () {
                    $('body').removeClass('stop-scrollingInfo');
                    //if (!showingitem) {
                    //    stopscrolling = -1;
                    //}
                }, title: pagetitle,
                closeText: false
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    Auction ID:<%=parameters["Auction_ID"]%>
    <img id="informationIcon" src="Images/Auction<%=parameters["Auction_ID"]%>/question.png" title="Click on me for information on bidding." />
    <div id="debug"></div>



    <div id="div_main">
        <button type="button" class="register <%=buttonclasses %>" data-thisdiv="div_main">Register</button>
        <button type="button" class="login <%=buttonclasses %>" data-thisdiv="div_main">Login</button><br />
        <%= categories %>
        <div class="flex flex-wrap">
            <%=html%>
        </div>
    </div>

    <div id="div_item" class="pa3 mw9 center" style="display: none">
        <div class="cf">
            <div id="div_item_images" class="fl w-100 pa4">
            </div>
        </div>
        <button type="button" class="returnfromitem <%=buttonclasses %>" data-thisdiv="div_item">Return to Auction</button>
    </div>

    <div id="div_register" class="pa3 mw9 center" style="display: none">
        Register
        First name:<input type="text" id="register_firstname" />
        Last name:<input type="text" id="register_lastname" />
        Email Address:<input type="email" id="register_emailaddress" />



        <button type="button" class="<%=buttonclasses %>" data-thisdiv="">Return to Auction</button>
        <button type="button" id="btn_register" class="<%=buttonclasses %>" data-thisdiv="">Submit</button>
    </div>

    <div id="div_login" class="pa3 mw9 center" style="display: none">
        Login
    <button type="button" class="<%=buttonclasses %>" data-thisdiv="div_login">Return to Item</button>
        <button type="button" id="btn_login" class="<%=buttonclasses %>" data-thisdiv="div_login">Submit</button>
    </div>

    <div id="dialog_showinformation"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

