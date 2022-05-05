<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmActaConformidadActoNotarial.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmActaConformidadActoNotarial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            var str_GUID = $("#<%= HFGUID.ClientID %>").val();
            var prm = {};
            prm.strGUID = str_GUID;

            $.ajax({
                type: "POST",
                url: "FrmActaConformidadActoNotarial.aspx/Imprimir_Acta_Conformidad",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(prm),
                dataType: "json",
                success: function (msg) {
                    document.body.innerHTML = msg.d;
                },
                error: function (s_Error) {
                }

            });
        });

        function btn_Imprimir_onclick() {
            $("#btn_Imprimir").hide();
            window.print();
            $("#btn_Imprimir").show();

        }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:HiddenField ID="HFGUID" runat="server" />
    </div>
    </form>
</body>
</html>
