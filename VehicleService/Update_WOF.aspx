<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Update_WOF.aspx.cs" Inherits="VehicleService.Update_WOF" %>


    <!-- Style Sheets -->
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
    <link href="_Dependencies/bootstrap.css" rel="stylesheet" />
    <!-- Javascript -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <link href="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")%>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/_Dependencies/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js")%>"></script>

    <script>
        $(document).ready(function () {
            $('#fld_oldwofdate').change(function () {
                alert($(this).val());
            })
            $('.datetime').datetimepicker({
                format: 'D MMM YYYY HH:mm',
                extraFormats: ['D MMM YY', 'D MMM YYYY', 'DD/MM/YY', 'DD/MM/YYYY', 'DD.MM.YY', 'DD.MM.YYYY', 'DD MM YY', 'DD MM YYYY'],
                //daysOfWeekDisabled: [0, 6],
                showClear: true,
                viewDate: false,
                useCurrent: true,
                stepping: 15

                //,maxDate: moment().add(-1, 'year')
            });
           

        })
    </script>

    <p></p>
<form id="dialogform" class="form-horizontal"> 
    <div class="panel panel-default">
            <div class="panel-heading">Not currently active</div>
            <div class="panel-body">
                <p>
                    This will not currently update the records
                </p>
                <p>Work is underway to restructure followup and activity and then allow this functionality.</p>
            </div>
        </div>
     
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_oldwofdate">Old WOF Date</label>
            <div class="col-sm-8">
                <input id="fld_oldwofdate" name="fld_oldwofdate" type="text" class="form-control" readonly="readonly" value="<%=WOF_Due %>" />
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_wofcycle">WOF Cycle</label>
            <div class="col-sm-8">
                <input id="fld_wofcycle" required="required" name="fld_wofcycle" type="text" class="form-control" value="<%=WOF_Cycle %>" />
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_nextwofdate">Next WOF Date</label>
            <div class="col-sm-8">
                <input id="fld_nextwofdate" name="fld_nextwofdate" type="text" class="form-control datetime" value="<%=nextwofdate %>" />
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_activitydate">Activity Date</label>
            <div class="col-sm-8">
                <input id="fld_activitydate" name="fld_activitydate" type="text" class="form-control" />
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_activity">Activity</label>
            <div class="col-sm-8">
                <textarea id="fld_activity" name="fld_activity" class="form-control"></textarea>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_followupdate">Followup Date</label>
            <div class="col-sm-8">
                <textarea id="fld_followupdate" name="fld_followupdate" class="form-control"></textarea>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_followupdetail">Followup Detail</label>
            <div class="col-sm-8">
                <textarea id="fld_followupdetail" name="fld_followupdetail" class="form-control"></textarea>
            </div>
        </div>
    </form>


