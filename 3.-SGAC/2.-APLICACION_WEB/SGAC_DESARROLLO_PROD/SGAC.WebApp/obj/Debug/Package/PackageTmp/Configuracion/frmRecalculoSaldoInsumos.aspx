<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmRecalculoSaldoInsumos.aspx.cs" Inherits="SGAC.WebApp.Configuracion.frmRecalculoSaldoInsumos" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlValidation.ascx" TagName="validation" TagPrefix="label" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloLibroContableRpt" runat="server" Text="Recalculo de Saldos - Insumos:"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <br />
    <table style="width: 90%;"  align="center" class="mTblPrincipal">
        <tr>
            <td>
                <div id="tabs">
                 <asp:UpdatePanel runat="server" ID="updReportesGerenciales" UpdateMode="Conditional">
                        <ContentTemplate>
                          <table width="100%">

                                <tr>
                                     <td><asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label></td>
                                     <td colspan="4"><uc1:ctrloficinaconsular ID="ctrlOficinaConsular" runat="server" 
                                             Width="604px" /></td>
                                </tr>
                              
                                <tr>
                                    <td><asp:Label ID="lblAnioMes" runat="server" Text="Periodo desde:"></asp:Label></td>
                                    <td style="width:170px;">
                                        <asp:DropDownList ID="ddlAnio" runat="server" style="cursor:pointer;" Width="60px">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlMes" runat="server" style="cursor:pointer;">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:70px;"> hasta: </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAnioFinal" runat="server" style="cursor:pointer;" Width="60px">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlMesFinal" runat="server" style="cursor:pointer;">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkReiniciar" Text="Reiniciar Proceso" runat="server" />
                                    </td>
                                    
                               </tr>
                               
                                
                                
                               <tr>
                                <td>
                                    <asp:Button ID="BtnRecalcular"  runat="server" Text="     Recalcular"   
                                        CssClass="btnAprobar" Width="140px" onclick="BtnRecalcular_Click"/>                                    
                                </td>
                               </tr>
                               <tr>
                                <td colspan="6"><Label:Validation ID="ctrlValidacion" runat="server" /></td>
                               </tr>
                        </table>
                        </ContentTemplate>
                 </asp:UpdatePanel>
                </div>            
            </td>           
        </tr>
    </table>
</asp:Content>
