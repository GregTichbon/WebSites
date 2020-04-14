<%@ Page Title="" Language="C#" MasterPageFile="~/Community/Community.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="TeOraHouWhanganui.Community.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script>
        var showmap = 0;
        $(document).ready(function () {
            $("#name").autocomplete({
                source: "<%: ResolveUrl("~/Community/_Dependencies/data.asmx/name_autocomplete")%>",
                minLength: 2,
                select: function (event, ui) {
                    $('#name').val("");
                    //event.preventDefault();
                    selected = ui.item;
                    window.open("person.aspx?id=" + selected.person_guid, "_self");
                },
                open: function (event, ui) {
                    if (navigator.userAgent.match(/iPad/)) {
                        // alert(1);
                        $('.autocomplete').off('menufocus hover mouseover');
                    }
                }
            })

            $("#location").autocomplete({
                source: "<%: ResolveUrl("~/Community/_Dependencies/data.asmx/postaladdress_autocomplete?option=localphysical")%>",
                 minLength: 3,
                 select: function (event, ui) {
                     $('#location').val("");
                     //event.preventDefault();
                     selected = ui.item;
                     window.open("location.aspx?id=" + address.assessment_no, "_self");
                 },
                 open: function (event, ui) {
                     if (navigator.userAgent.match(/iPad/)) {
                         // alert(1);
                         $('.autocomplete').off('menufocus hover mouseover');
                     }
                 }
            })
            $('#btn_map').click(function () {
                window.open("showmap.aspx", "_self");
            });
            $('#btn_listbyupdate').click(function () {
                window.open("reports/listall_updates.aspx", "_list");
            });

                /*
            $("#location").autocomplete({
                //source: "http://wdc.whanganui.govt.nz/onlinetest/functions/data.asmx/PropertySelect?mode=address",
                source: function (request, response) {
                    jQuery.get("http://wdc.whanganui.govt.nz/onlinetest/functions/data.asmx/PropertySelect?mode=address", {
                        term: request.term
                    }, function (data) {
                            mydata = JSON.parse(data);
                            mydata.push({ "label": "Add location", "value": "0", "area": "", "legaldescription": "", "assessment_no": "new" });
                            response(mydata);
                    });
                },
                //source: [{"label":"30 Totara St WHANGANUI","value":"2710","area":"0.1006 Ha","legaldescription":"LOT 2 DP 2933","assessment_no":"1311054400"}],
                minLength: 3,
                select: function (event, ui) {
                    event.preventDefault();
                    address = ui.item;
                    if (address) {
                        $("#tb_address").val(address.label);
                        window.location.replace("location.aspx?id=" + address.assessment_no, "_self");
                        if (showmap == '1') {
                            $("#propertymap").attr('src', 'http://maps.whanganui.govt.nz/IntraMaps/MapControls/EasiMaps/index.html?configId=00000000-0000-0000-0000-000000000000&form=2fb1ccb7-bfdd-4d20-b318-5a56d52d7fe7&mapkey=' + address.value + '&project=WhanganuiMapControls&module=WDCPublicEnquiry&layer=WDCProperty&search=false&info=false&slider=false&expand=false');
                            $("#propertymap").show();
                        }
                    } else {
                        alert('Not found');
                    }
                    //selectedproperty(ui.item ?
                    // passproperty(0, address.label, address.value, address.area, address.legaldescription, address.assessment_no) :
                    // "Nothing selected, input was " + this.value);
                },
                open: function (event, ui) {
                    if (navigator.userAgent.match(/iPad/)) {
                        // alert(1);
                        $('.autocomplete').off('menufocus hover mouseover');
                    }
                }
            })
            */
         });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Search
    </h1>
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-4" for="name">By Person</label>
            <div class="col-sm-8">
                <input id="name" name="name" type="text" class="form-control" />
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="location">By Location</label>
            <div class="col-sm-8">
                <input id="location" name="location" type="text" class="form-control" />
            </div>
        </div>



        <asp:Button ID="btn_geocode" runat="server" class="form-control" style="display:none" Text="GeoCode all locations" OnClick="btn_geocode_Click" />
        <input type="button" id="btn_map" value="Show Map" class="form-control" /><input type="button" id="btn_listbyupdate" value="List by updates" class="form-control" />
        btn_listbyupdate
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
