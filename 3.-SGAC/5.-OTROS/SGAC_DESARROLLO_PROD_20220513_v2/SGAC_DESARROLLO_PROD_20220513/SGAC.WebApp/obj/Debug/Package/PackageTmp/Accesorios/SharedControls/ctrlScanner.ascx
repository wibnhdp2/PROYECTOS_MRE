<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlScanner.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlScanner" %>

<style type="text/css">


.btnUndo
{
	-moz-box-shadow:inset 0px 0px 2px 0px #ffffff;
	-webkit-box-shadow:inset 0px 0px 2px 0px #ffffff;
	box-shadow:inset 0px 0px 2px 0px #ffffff;
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	background-color:#ededed;
	-webkit-border-top-left-radius:5px;
	-moz-border-radius-topleft:5px;
	border-top-left-radius:5px;
	-webkit-border-top-right-radius:5px;
	-moz-border-radius-topright:5px;
	border-top-right-radius:5px;
	-webkit-border-bottom-right-radius:5px;
	-moz-border-radius-bottomright:5px;
	border-bottom-right-radius:5px;
	-webkit-border-bottom-left-radius:5px;
	-moz-border-radius-bottomleft:5px;
	border-bottom-left-radius:5px;
	text-indent:0;
	text-decoration:none;
	text-align:center;
	border:1px outset #dcdcdc;
	height:26px;
	width:91px;	
	background-image: url('../../Images/img_16_undo.png');
	background-repeat: no-repeat;
	background-position: 8px 3px;
	cursor:pointer;
	}

.btnEscanear
{
	-moz-box-shadow:inset 0px 0px 2px 0px #ffffff;
	-webkit-box-shadow:inset 0px 0px 2px 0px #ffffff;
	box-shadow:inset 0px 0px 2px 0px #ffffff;
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	background-color:#ededed;
	-webkit-border-top-left-radius:5px;
	-moz-border-radius-topleft:5px;
	border-top-left-radius:5px;
	-webkit-border-top-right-radius:5px;
	-moz-border-radius-topright:5px;
	border-top-right-radius:5px;
	-webkit-border-bottom-right-radius:5px;
	-moz-border-radius-bottomright:5px;
	border-bottom-right-radius:5px;
	-webkit-border-bottom-left-radius:5px;
	-moz-border-radius-bottomleft:5px;
	border-bottom-left-radius:5px;
	text-indent:0;
	text-decoration:none;
	text-align:center;
	border:1px outset #dcdcdc;
	height:26px;
	width:91px;	
	background-image: url('../../Images/img_16_scanner.png');
	background-repeat: no-repeat;
	background-position: 10px 4px;   
	cursor:pointer; 	
}

.btnAgregarPagina
{
	-moz-box-shadow:inset 0px 0px 2px 0px #ffffff;
	-webkit-box-shadow:inset 0px 0px 2px 0px #ffffff;
	box-shadow:inset 0px 0px 2px 0px #ffffff;
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	background-color:#ededed;
	-webkit-border-top-left-radius:5px;
	-moz-border-radius-topleft:5px;
	border-top-left-radius:5px;
	-webkit-border-top-right-radius:5px;
	-moz-border-radius-topright:5px;
	border-top-right-radius:5px;
	-webkit-border-bottom-right-radius:5px;
	-moz-border-radius-bottomright:5px;
	border-bottom-right-radius:5px;
	-webkit-border-bottom-left-radius:5px;
	-moz-border-radius-bottomleft:5px;
	border-bottom-left-radius:5px;
	text-indent:0;
	text-decoration:none;
	text-align:center;
	border:1px outset #dcdcdc;
	height:50px;
	width:80px;	
	background-image: url('../../Images/img_32_add_page.png');
	background-repeat: no-repeat;
	background-position: 10px 4px;    
	cursor:pointer;	
}

.btnAnularPagina
{
	-moz-box-shadow:inset 0px 0px 2px 0px #ffffff;
	-webkit-box-shadow:inset 0px 0px 2px 0px #ffffff;
	box-shadow:inset 0px 0px 2px 0px #ffffff;
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	background-color:#ededed;
	-webkit-border-top-left-radius:5px;
	-moz-border-radius-topleft:5px;
	border-top-left-radius:5px;
	-webkit-border-top-right-radius:5px;
	-moz-border-radius-topright:5px;
	border-top-right-radius:5px;
	-webkit-border-bottom-right-radius:5px;
	-moz-border-radius-bottomright:5px;
	border-bottom-right-radius:5px;
	-webkit-border-bottom-left-radius:5px;
	-moz-border-radius-bottomleft:5px;
	border-bottom-left-radius:5px;
	text-indent:0;
	text-decoration:none;
	text-align:center;
	border:1px outset #dcdcdc;
	height:50px;
	width:80px;	
	background-image: url('../../Images/img_32_delete_page.png');
	background-repeat: no-repeat;
	background-position: 10px 4px;    
	cursor:pointer;	
}

.btnPDF
{
	-moz-box-shadow:inset 0px 0px 2px 0px #ffffff;
	-webkit-box-shadow:inset 0px 0px 2px 0px #ffffff;
	box-shadow:inset 0px 0px 2px 0px #ffffff;
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	background-color:#ededed;
	-webkit-border-top-left-radius:5px;
	-moz-border-radius-topleft:5px;
	border-top-left-radius:5px;
	-webkit-border-top-right-radius:5px;
	-moz-border-radius-topright:5px;
	border-top-right-radius:5px;
	-webkit-border-bottom-right-radius:5px;
	-moz-border-radius-bottomright:5px;
	border-bottom-right-radius:5px;
	-webkit-border-bottom-left-radius:5px;
	-moz-border-radius-bottomleft:5px;
	border-bottom-left-radius:5px;
	text-indent:0;
	text-decoration:none;
	text-align:center;
	border:1px outset #dcdcdc;
	height:26px;
	width:91px;	
	background-image: url('../../Images/img_16_pdf.png');
	background-repeat: no-repeat;
	background-position: 10px 4px;
	cursor:pointer;    	
}

.btnVisualizar
{
	-moz-box-shadow:inset 0px 0px 2px 0px #ffffff;
	-webkit-box-shadow:inset 0px 0px 2px 0px #ffffff;
	box-shadow:inset 0px 0px 2px 0px #ffffff;
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	background-color:#ededed;
	-webkit-border-top-left-radius:5px;
	-moz-border-radius-topleft:5px;
	border-top-left-radius:5px;
	-webkit-border-top-right-radius:5px;
	-moz-border-radius-topright:5px;
	border-top-right-radius:5px;
	-webkit-border-bottom-right-radius:5px;
	-moz-border-radius-bottomright:5px;
	border-bottom-right-radius:5px;
	-webkit-border-bottom-left-radius:5px;
	-moz-border-radius-bottomleft:5px;
	border-bottom-left-radius:5px;
	text-indent:0;
	text-decoration:none;
	text-align:center;
	border:1px outset #dcdcdc;
	height:26px;
	width:91px;	
	background-image: url('../../Images/img_16_preview.png');
	background-repeat: no-repeat;
	background-position: 10px 4px;    	
	cursor:pointer;
}

.btnSave
{
	-moz-box-shadow:inset 0px 0px 2px 0px #ffffff;
	-webkit-box-shadow:inset 0px 0px 2px 0px #ffffff;
	box-shadow:inset 0px 0px 2px 0px #ffffff;
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	background-color:#ededed;
	-webkit-border-top-left-radius:5px;
	-moz-border-radius-topleft:5px;
	border-top-left-radius:5px;
	-webkit-border-top-right-radius:5px;
	-moz-border-radius-topright:5px;
	border-top-right-radius:5px;
	-webkit-border-bottom-right-radius:5px;
	-moz-border-radius-bottomright:5px;
	border-bottom-right-radius:5px;
	-webkit-border-bottom-left-radius:5px;
	-moz-border-radius-bottomleft:5px;
	border-bottom-left-radius:5px;
	text-indent:0;
	text-decoration:none;
	text-align:center;
	border:1px outset #dcdcdc;
	height:26px;
	width:91px;	
	background-image: url('../../Images/img_16_save.png');
	background-repeat: no-repeat;
	background-position: 10px 4px;
	cursor:pointer;
	}

.style1
{
    width: 38px;
}

</style>

<table id="Scanner">
    <tr>
        <td class="style1">Escáner</td>
        <td><asp:DropDownList ID="ddlEscaners" runat="server" Height="16px" Width="298px"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Button ID="btnUp"   runat="server" Text="Up" onclick="btnUp_Click" 
                Width="35px" Enabled="False" Height="35px" />
            <br />
            <asp:Button ID="btnDown" runat="server" Text="Down" onclick="btnDown_Click" 
                Enabled="False" Height="35px" Width="35px" />
            </td>
        <td>
            <div>
            <table id = "Preview">
                <tr>
                    <td>
                    <asp:Image ID="imgPreview" runat="server" ImageUrl="~/Images/scanner_fondo.png" 
                            Height="427px" Width="295px" BorderColor="Black" BorderStyle="Groove"/></td>
                </tr>
                </table>
            </div>
            </td>
        <td>
            <br />
                    <asp:Button runat="server" ID="btnAddPagina" Text="" 
                CssClass="btnAgregarPagina" Width="50px" onclick="btnAddPagina_Click" 
                ToolTip="Escanear" />
                    <br />
                    <asp:Button runat="server" ID="btnQuitaPagina" Text="" CssClass="btnAnularPagina" 
                        Width="50px" Height="44px" onclick="btnQuitaPagina_Click" 
                ToolTip="Quitar pagina" Enabled="False" />
            </td>
        </tr>
    <tr>
        <td colspan="3" align="center">
            <asp:TextBox ID="txtPaginado" runat="server" Enabled="False"></asp:TextBox>
        </td>
        </tr>
    <tr>
        <td colspan = "3" align="center">
                    <asp:Button runat="server" ID="btnPDF" Text="    Generar PDF" 
                CssClass="btnPDF" Width="120px" onclick="btnPDF_Click" Enabled="False" />
            <asp:Button runat="server" ID="Button5" Text="    Cancelar" CssClass="btnUndo" 
                Width="100px" />
        </td>
    </tr>

</table>