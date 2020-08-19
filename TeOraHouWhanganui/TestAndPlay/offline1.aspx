<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="offline1.aspx.cs" Inherits="TeOraHouWhanganui.TestAndPlay.offline1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" manifest="offline.manifest">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        window.addEventListener('online', updateStatus);
        window.addEventListener('offline', updateStatus);

        function updateStatus(event) {
            if (navigator.online) {
                alert('Online');
            } else {
                alert('Offline');
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    Greg was here
    </form>
</body>
<script type="text/javascript">
    console.log(navigator.online);
    var appCache = window.applicationCache;
    console.log(appCache.status);
    /*
    switch (appCache.status) {
        case appCache.UNCACHED: // UNCACHED == 0
            alert('UNCACHED');
            break;
        case appCache.IDLE: // IDLE == 1
            alert('IDLE');
            break;
        case appCache.CHECKING: // CHECKING == 2
            alert('CHECKING');
            break;
        case appCache.DOWNLOADING: // DOWNLOADING == 3
            alert('DOWNLOADING');
            break;
        case appCache.UPDATEREADY:  // UPDATEREADY == 4
            alert('UPDATEREADY');
            break;
        case appCache.OBSOLETE: // OBSOLETE == 5
            alert('OBSOLETE');
            break;
        default:
            alert('UKNOWN CACHE STATUS');
            break;
    };
    */
</script>
</html>
