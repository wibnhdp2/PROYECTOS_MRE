<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReporteAntecedentesPenales.aspx.cs" Inherits="SGAC.WebApp.Reportes.frmReporteAntecedentesPenales" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>   
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">

        $(function () {
            $('#tabs').tabs();
        });

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function cerrarPopupEspera() {
            document.getElementById('modalEspera').style.display = 'none';
        }
        function abrirPopupEspera() {
            document.getElementById('modalEspera').style.display = 'block';
        }

        function verDIV() {
            var ddlReporte = document.getElementById("<%=ddlTipoReporte.ClientID %>").value;

            if (ddlReporte == "1") {
                document.getElementById("<%=lblUsuario.ClientID %>").style.display = "block";
                document.getElementById("<%=ddlUsuarios.ClientID %>").style.display = "block";
            }
            else {
                document.getElementById("<%=lblUsuario.ClientID %>").style.display = "none";
                document.getElementById("<%=ddlUsuarios.ClientID %>").style.display = "none";
            }
        }
    </script>
  
    <style type="text/css">
.mensajePie
{
    color:Black;
    font-family:@Arial Unicode MS;
    font-size:smaller;
    font-weight:bold; 
}        
        .style1
        {
            width: 132px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <%--Cuerpo--%>
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloLibroContableRpt" runat="server" Text="Reportes Antecedentes Penales"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <br />

    <%--Opciones--%>
    <table style="width: 90%" align="center">
        <tr>
            <td>
                <div id="tabs">
                    <asp:UpdatePanel runat="server" ID="updConsulta" UpdateMode="Conditional">
                    <Triggers>
                    </Triggers>
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="2">
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" Width="500" style="cursor:pointer;"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblReportes" runat="server" Text="Reportes:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlTipoReporte" runat="server" Width="500px" style="cursor:pointer;" onchange="verDIV();" />
                                                                                    
                                    </td>
                                </tr> 
                                     <tr>
                                        <td class="style1">
                                            <asp:Label ID="lblUsuario" runat="server" Text="Usuarios:" style="display:none;"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlUsuarios" runat="server" Width="500px" style="cursor:pointer;display:none;" />
                                                                                    
                                        </td>
                                    </tr>   
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblFecInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                    </td>
                                    <td>
                                        <SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Fin:" CssClass="lblPadding"></asp:Label>
                                    </td>
                                    <td>
                                        <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        &nbsp;</td>
                                    <td colspan="3">                                                                                
                                        <asp:CheckBox ID="chkFechaRegistroMSIAP" runat="server" style="cursor:pointer"
                                            Checked="True" Text="Por fecha de registro MSIAP" AutoPostBack="True" 
                                            oncheckedchanged="chkFechaRegistroMSIAP_CheckedChanged"/> 
                                             
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkFechaRegistroRGE" runat="server" style="cursor:pointer"
                                            Text="Por fecha de registro de actuación RGE" AutoPostBack="True" 
                                            oncheckedchanged="chkFechaRegistroRGE_CheckedChanged"/> 
                                            
                                        </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <%--Opciones--%>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    <asp:Button ID="btnImprimir" runat="server" CssClass="btnPrint" Width="90px"  Text="     Imprimir" onclick="btnImprimir_Click" />
                                                    &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>                                                                                                                                                                
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <Label:Validation ID="ctrlValidacion" runat="server" />                                        
                                    </td>
                                </tr>
                                
                            </table>   
                            <div id="modalEspera" class="modal">
                                        <div class="modal-window">
                                            <div class="modal-titulo">
                                                <span>Procesando...</span>
                                            </div>
                                            <div class="modal-cuerpo">
                                                <br />
                                                <h3 style="font-size:small">Procesando la información. Por favor espere...</h3>
                                                <asp:Image ID="imgEspera" ImageUrl="../Images/espera.gif" runat="server" />
                                            </div>
                                        </div>
                                    </div>                        
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                </div>
            </td>
        </tr>
    </table>
   
</asp:Content>
