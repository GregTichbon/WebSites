<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="office.dataInn.co.nz.Raffles.UBC2019B.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #centred {
            position: fixed;
            width: 30%; /* Set your desired with */
            z-index: 2; /* Make sure its above other items. */
            top: 50%;
            left: 50%;
            margin-top: -10%; /* Changes with height. */
            margin-left: -15%; /* Your width divided by 2. */
            /* You will not need the below, its only for styling   purposes. */
            padding: 10px;
            border: 2px solid #555555;
            background-color: #ccc;
            border-radius: 5px;
            text-align: center;
                        font-size: 32px;

        }

        td, input {
            font-size: 32px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="centred">
            Access Code: 
            <asp:TextBox ID="tb_accesscode" runat="server" TextMode="Password" style="width:90%"></asp:TextBox>
            <br />
            <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" Text="Submit" />
        </div>

    </form>
</body>
</html>