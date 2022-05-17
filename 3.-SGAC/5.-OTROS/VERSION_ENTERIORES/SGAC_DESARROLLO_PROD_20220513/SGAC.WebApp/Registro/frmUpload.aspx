<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUpload.aspx.cs" Inherits="SGAC.WebApp.Registro.frmUpload" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlUploader.ascx" TagName="ctrlUploader"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <title></title>
    <style type="text/css">
        .tMsjeWarnig
        {
            background-color: #F2F1C2;
            border-color: Yellow; /*#6E4E1B;*/
            color: #4B4F5E;
            height: 15px;
            background-image: url('../Images/img_16_warning.png');
            background-repeat: no-repeat;
            background-position: 8px 2px;
            width: 100%;
        }
        
        .lblMsjeWarnig
        {
            margin-left: 25px;
        }
        
        .tMsjeError
        {
            background-color: #FE2E2E;
            border-color: Red; /*#6E4E1B;*/
            color: #FFFFFF;
            height: 15px;
            background-image: url('../Images/img_16_error.png');
            background-repeat: no-repeat;
            background-position: 8px 2px;
            width: 100%;
        }
        
        .lblMsjeError
        {
            margin-left: 25px;
        }
        
        .tMsjeSucess
        {
            background-color: #2E9AFE;
            border-color: Blue; /*#6E4E1B;*/
            color: #FFFFFF;
            height: 15px;
            background-image: url('../Images/img_16_success.png');
            background-repeat: no-repeat;
            background-position: 8px 2px;
            width: 100%;
        }
        
        .lblMsjeSucess
        {
            margin-left: 25px;
        }
    </style>
</head>
<body>
    <form id="frmUpload2" runat="server">
    <div align="center">
        <br />
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlArchivo" runat="server" GroupingText="Buscar Archivo">
                        <div>
                            <asp:FileUpload ID="FileUploader" runat="server" Width="400px" />
                            <asp:Button ID="btnSubir" runat="server" OnClick="btnSubir_Click" Text="  Subir"
                                CssClass="btnUpload" Width="99px" />
                        </div>
                        <br />
                        <div align="left">
                            <asp:Label ID="Label1" runat="server" ></asp:Label>
                            <br />
                            <asp:Label ID="lblMsjeWarnig" runat="server" Text="" />
                            <br />
                            <asp:Label ID="lblMsjeError" runat="server" Text="" />
                            <br />
                            <asp:Label ID="lblMsjeSucess" runat="server" Text="" />
                            <br />
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
</body>
</html>
