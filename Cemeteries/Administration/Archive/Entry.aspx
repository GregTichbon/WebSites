<%@ Page Title="" Language="C#" MasterPageFile="~/Cemeteries.Master" AutoEventWireup="true" CodeBehind="Entry.aspx.cs" Inherits="Cemeteries.Administration.Archive.Entry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
             $('#div_dateofbirth').datetimepicker({
                //$('#tb_dateofbirth').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: false,
                sideBySide: true,
                viewMode: 'years'
            });

            $("#div_dateofbirth").on("dp.change", function (e) {
                //$("#tb_dateofbirth").on("dp.change", function (e) {
                if (moment().diff(e.date, 'seconds') < 0) {
                    e.date = moment(e.date).subtract(100, 'years');
                    $("#tb_dateofbirth").val(moment(e.date).format('D MMM YYYY'));
                }
            });



            $('.dateselector').datetimepicker({
                //$('#tb_dateofbirth').datetimepicker({
                format: 'D MMM YYYY',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: false,
                sideBySide: true,
            });

  
        });
    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_warrant">Warrant</label>
        <div class="col-sm-8">
            <input id="tb_warrant" name="tb_warrant" type="text" class="form-control" maxlength="50" value="<%: warrant %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_burydate">Internment Date</label>
        <div class="col-sm-8">
            <input id="tb_burydate" name="tb_burydate" type="text" class="form-control" maxlength="50" value="<%: burydate %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_burialdate">Internment Date</label>
        <div class="col-sm-8">
            <div class="input-group date dateselector" id="div_burialdate">
                <input id="tb_burialdate" name="tb_burialdate" type="text" class="form-control" maxlength="50" value="<%: burialdate %>" />
                <span class="input-group-addon">
                    <span class="glyphicon glyphicon-calendar"></span>
                </span>
            </div>
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_dob">Date of Birth</label>
        <div class="col-sm-8">
            <div class="input-group date" id="div_dateofbirth">
                <input id="tb_dob" name="tb_dob" type="text" class="form-control" maxlength="50" value="<%: dob %>" />
                <span class="input-group-addon">
                    <span class="glyphicon glyphicon-calendar"></span>
                </span>
            </div>
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_dod">Date of Death</label>
        <div class="col-sm-8">
            <input id="tb_dod" name="tb_dod" type="text" class="form-control" maxlength="50" value="<%: dod %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_fullname">Full Name</label>
        <div class="col-sm-8">
            <input id="tb_fullname" name="tb_fullname" type="text" class="form-control" maxlength="250" value="<%: fullname %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_surname">Surname</label>
        <div class="col-sm-8">
            <input id="tb_surname" name="tb_surname" type="text" class="form-control" maxlength="150" value="<%: surname %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_forenames">Forenames</label>
        <div class="col-sm-8">
            <input id="tb_forenames" name="tb_forenames" type="text" class="form-control" maxlength="150" value="<%: forenames %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_age">Age</label>
        <div class="col-sm-8">
            <input id="tb_age" name="tb_age" type="text" class="form-control" maxlength="50" value="<%: age %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_residence">Residential Address</label>
        <div class="col-sm-8">
            <textarea id="tb_residence" name="tb_residence" class="form-control" rows="3" maxlength="150"><%: residence %></textarea>
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_occupation">Occupation</label>
        <div class="col-sm-8">
            <input id="tb_occupation" name="tb_occupation" type="text" class="form-control" maxlength="150" value="<%: occupation %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_minister">Minister</label>
        <div class="col-sm-8">
            <input id="tb_minister" name="tb_minister" type="text" class="form-control" maxlength="150" value="<%: minister %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_director">Director</label>
        <div class="col-sm-8">
            <input id="tb_director" name="tb_director" type="text" class="form-control" maxlength="150" value="<%: director %>" />
        </div>
    </div>

    <br />

    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_remarks">Remarks</label>
        <div class="col-sm-8">
            <textarea id="tb_remarks" name="tb_remarks" class="form-control" rows="3" maxlength="1000"><%: remarks %></textarea>
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_book">Book</label>
        <div class="col-sm-8">
            <input id="tb_book" name="tb_book" type="text" class="form-control" maxlength="20" value="<%: book %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_pageref">Page</label>
        <div class="col-sm-8">
            <input id="tb_pageref" name="tb_pageref" type="text" class="form-control" maxlength="20" value="<%: pageref %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_dateentered">Date Entered</label>
        <div class="col-sm-8">
            <input id="tb_dateentered" name="tb_dateentered" type="text" class="form-control" readonly="readonly" value="<%: dateentered %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_dateupdated">Date Last Updated</label>
        <div class="col-sm-8">
            <input id="tb_dateupdated" name="tb_dateupdated" type="text" class="form-control" readonly="readonly" value="<%: dateupdated %>" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_datechecked">Date Checked</label>
        <div class="col-sm-8">
             <div class="input-group date dateselector" id="div_datechecked">
            <input id="tb_datechecked" name="tb_datechecked" type="text" class="form-control" value="<%: datechecked %>" />
                <span class="input-group-addon">
                    <span class="glyphicon glyphicon-calendar"></span>
                </span>
            </div>
        </div>
    </div>





    <div class="form-group">
        <label class="control-label col-sm-4" for="tb_ischecked">Checked</label>
        <div class="col-sm-8">
            <select id="dd_ischecked" name="dd_ischecked" class="form-control">
                <option></option>
                <%
                    Dictionary<string, string> yesnoOptions = new Dictionary<string, string>();
                    yesnoOptions["type"] = "select";
                    yesnoOptions["valuefield"] = "label";
                    Response.Write(Generic.Functions.buildselection(yesno, ischecked, yesnoOptions));
                %>
            </select>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
