<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReporteMigratorio.aspx.cs" Inherits="SGAC.WebApp.Reportes.frmReporteMigratorio" MasterPageFile="~/Site.Master" %>

<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlValidation.ascx" TagName="validation" TagPrefix="label" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

 
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloLibroContableRpt" runat="server" Text="Reportes"></asp:Label></h2>
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
                                    <td><asp:Label ID="Label2" runat="server" Text="Tipo de Reporte:"></asp:Label></td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddl_TipoReporte" runat="server" 
                                            AutoPostBack="True"
                                            onselectedindexchanged="ddl_TipoReporte_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="IDV_1" runat="server">
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="Número Pasaporte (Inicial) :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_nro_pasaporte_ini" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="Número Pasaporte (Final) :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_nro_pasaporte_fin" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="IDV_2" runat="server">
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text="Mision :"></asp:Label>
                                    </td>
                                    <td>
                                        <uc1:ctrloficinaconsular ID="ddlMision_IDV" runat="server" 
                                             Width="400px" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label15" runat="server" Text="Estado del Pasaporte :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEstadoPass_IDV" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="CTA" runat="server">
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="Seleccione por año :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAnio" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="PG_1" runat="server">
                                     <td><asp:Label ID="lblOficinaConsular" runat="server" Text="Misión:"></asp:Label></td>
                                     <td colspan="3"><uc1:ctrloficinaconsular ID="ctrlOficinaConsular" runat="server" 
                                             Width="400px" /></td>
                                </tr>
                              <tr id="PG_2" runat="server">
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Número de Pasaporte :"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_nun_Pasaporte" runat="server"></asp:TextBox>
                                </td>

                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Expediente con Imagen :"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chk_expediente" runat="server" />
                                </td>
                              </tr>
                              <tr id="PG_3" runat="server">
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Tipo Documento :"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoDocumento" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label16" runat="server" Text="Número Documento"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_nro_Documento" runat="server"></asp:TextBox>
                                </td>
                              </tr>
                              
                              <tr id="PG_8" runat="server">
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Estado del Pasaporte :"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEstadoPasaporte" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Nro. del Expediente :"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_nro_Expediente" runat="server" MaxLength="10" 
                                                        onkeypress="return isNumberKey(event)" onBlur="validanumeroLostFocus(this)"></asp:TextBox>
                                </td>
                              </tr>
                              <tr id="PG_5" runat="server">
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="Apellido Paterno :"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_apellido_parterno" runat="server" onkeypress="return NoCaracteresEspeciales(event)" CssClass="txtLetra" MaxLength="50"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Apellido Materno :"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_apellido_materno" runat="server" CssClass="txtLetra" onkeypress="return NoCaracteresEspeciales(event)" MaxLength="50"></asp:TextBox>
                                </td>
                              </tr>
                              <tr id="PG_6" runat="server">
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Nombres :"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_nombres" runat="server" CssClass="txtLetra" onkeypress="return NoCaracteresEspeciales(event)" MaxLength="50"></asp:TextBox>
                                </td>
                              </tr>
                                <tr id="PG_7" runat="server">
                                    <td><asp:Label ID="lblFecInicio" runat="server" Text="Fecha Inicio:"></asp:Label></td>
                                    <td><SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" /></td>
                                    <td><asp:Label ID="lblFechaFin" runat="server" CssClass="lblPadding" Text="Fecha Fin:"></asp:Label></td>
                                    <td><SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" /></td>
                                </tr>

                               <tr>
                                <td>
                                    <asp:Button ID="BtnAceptar"  runat="server" Text="     Imprimir"   CssClass="btnPrint"
                                        onclick="BtnAceptar_Click" />                                    
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
    <script type="text/javascript">
        function NoCaracteresEspeciales(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = true;
            var letras = "-*?´(),;.:_|°=¿#%&/{}[]+!<>~@'$¡";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = false;
            }

            return letra;

        }
    </script>
</asp:Content>
