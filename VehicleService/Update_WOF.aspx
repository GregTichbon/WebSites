<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Update_WOF.aspx.cs" Inherits="VehicleService.Update_WOF" %>


    <script>
        $(document).ready(function () {
            $('#fld_oldwofdate').change(function () {
                alert($(this).val());
            })

           

        })
    </script>

    <p></p>
<form id="dialogform" class="form-horizontal"> 
     
        <div class="form-group">
            <label class="control-label col-sm-4" for="fld_oldwofdate">Old WOF Date</label>
            <div class="col-sm-8">
                <input id="fld_oldwofdate" name="fld_oldwofdate" type="text" class="form-control" value="<%=WOF_Due %>" />
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
                <input id="fld_nextwofdate" name="fld_nextwofdate" required="required" type="text" class="form-control" value="<%=nextwofdate %>" />
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


