<%@ Page Title="" Language="C#" MasterPageFile="~/Community/Community.Master" AutoEventWireup="true" CodeBehind="ShowMap.aspx.cs" Inherits="TeOraHouWhanganui.Community.ShowMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style>
        /* Always set the map height explicitly to define the size of the div
       * element that contains the map. */
        #map {
            height: 100% !important;
            padding-bottom: 50%;
            position: relative;
            width: 100%;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {

        });




    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div id="map"></div>
    <script>
        var xmldata = "<%=xml%>";
        function initMap() {
            var myLatLng = { lat: -39.936008, lng: 175.016227 };
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 15,
                center: myLatLng
            });

            var xmlDoc = new window.DOMParser().parseFromString(xmldata, "text/xml");
            locations = xmlDoc.getElementsByTagName("locations");

            for (var i = 0; i < locations.length; i++) {
                var latlng = new google.maps.LatLng(locations[i].childNodes[2].firstChild.nodeValue, locations[i].childNodes[1].firstChild.nodeValue);
                var marker = new google.maps.Marker({
                    position: latlng,
                    title: locations[i].childNodes[0].firstChild.nodeValue,
                    map: map
                });
            } 
        }
    </script>
    <script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCCpsWhkuuHlAe6EKhSi5zSlmmIVMN9M8c&callback=initMap"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
