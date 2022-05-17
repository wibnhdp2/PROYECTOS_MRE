<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReactivarActuacion.aspx.cs" Inherits="SGAC.WebApp.Configuracion.frmReactivarActuacion" %>
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


    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = true;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                letra = false;
            }
            if (charCode == 13) {
                letra = false;
            }
            return letra;
        }

        function CalculaRangos() {
            var corrActuacion = document.getElementById('<%= txtCorActuacion.ClientID %>').value;

            if (parseInt(corrActuacion) <= 0) {
                document.getElementById('<%= txtCorActuacion.ClientID %>').value = '';
                alert("El valor no puede ser menor o igual a cero.");
                return;
            }

        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTitulo" runat="server" Text="Reactivar - Actuación:"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <table style="width: 90%;"  align="center" class="mTblPrincipal">
        <tr>
            <td>
                <div id="tabs">
                <table width="100%">

                                <tr>
                                     <td class="style1"><asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label></td>
                                     <td colspan="5">
                                     <uc1:ctrloficinaconsular ID="ctrlOficinaConsular" runat="server" 
                                             Width="604px" />
                                     </td>
                                </tr>
                              
                                <tr>
                                    <td> Correlativo Actuación: </td>
                                    <td>
                                        <asp:TextBox ID="txtCorActuacion" runat="server" CssClass="campoNumero" 
                                                                    Font-Size="10pt" MaxLength="10" onBlur="CalculaRangos()" 
                                                                    onkeypress="return isNumberKey(event)" Width="90px"></asp:TextBox>

                                     </td>                                 
                               </tr>
                               <tr>
                               <td> Año de Tramite: </td>
                               <td>
                                   <asp:DropDownList ID="ddlAnio" runat="server">
                                   <asp:ListItem>2016</asp:ListItem>
                                   <asp:ListItem>2017</asp:ListItem>
                                   <asp:ListItem>2018</asp:ListItem>
                                   <asp:ListItem>2019</asp:ListItem>
                                   <asp:ListItem>2020</asp:ListItem>
                                   <asp:ListItem>2021</asp:ListItem>
                                   <asp:ListItem>2022</asp:ListItem>
                                   <asp:ListItem>2023</asp:ListItem>
                                   <asp:ListItem>2024</asp:ListItem>
                                   <asp:ListItem>2025</asp:ListItem>
                                   <asp:ListItem>2026</asp:ListItem>
                                   <asp:ListItem>2027</asp:ListItem>
                                   <asp:ListItem>2028</asp:ListItem>
                                   <asp:ListItem>2029</asp:ListItem>
                                   <asp:ListItem>2030</asp:ListItem>
                                   </asp:DropDownList>
                               </td>
                               </tr>
                               <tr>
                                   <td class="style1">
                                        <asp:Label ID="lblMotivo" runat="server" Text="Motivo:"></asp:Label>
                                   </td>
                                   <td colspan="3">
                                       <asp:TextBox ID="txtMotivo" runat="server" Height="48px" Width="351px"></asp:TextBox>
                                   </td>
                               </tr>
                               
                                
                                
                               <tr>
                                <td>
                                    <asp:Button ID="btnBuscar"  runat="server" Text="     Buscar"   
                                        CssClass="btnBusqueda" Width="140px" onclick="btnBuscar_Click"/> 
                                    <asp:HiddenField ID="hFechaTramite" runat="server" />                          
                                    <asp:HiddenField ID="hActuacionID" runat="server" />   
                                    <asp:HiddenField ID="hActuacionDetalleID" runat="server" />   
                                    <asp:HiddenField ID="hOficinaConsularID" runat="server" />   
                                </td>
                                <td>
                                        
                                        
                                        <div>
                                        <div style="float:left">
                                            <asp:Button ID="btnReactivar"  runat="server" Text="     Reactivar"   
                                            CssClass="btnAprobar" Width="140px" Enabled="False" 
                                            onclick="btnReactivar_Click"/> 
                                            </div>
                                            <div style="margin-left:10px; float:left">
                                            <asp:Button ID="btnLimpiar"  runat="server" Text="     Limpiar"   
                                            CssClass="btnLimpiar" Width="140px" onclick="btnLimpiar_Click"/>  
                                            </div>
                                            
                                        </div>
                                                                      
                                </td>
                              
                               </tr>
                               <tr>
                                <td colspan="6"><label:validation ID="ctrlValidacion" runat="server" /></td>
                               </tr>
                        </table>
                        <table style="width: 90%;"  align="center" class="mTblPrincipal">
                                    <tr>
                                        <td class="style3">
                                            <table align="left" style="width: 594px; height: 28px; margin-top: 3px;">
                                               
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="updGrillaConsulta" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="grReporteGestion" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="alt"
                                                                    AutoGenerateColumns="False" CssClass="mGrid" GridLines="None"
                                                                    Font-Size="8pt" Width="850px">
                                                                    <AlternatingRowStyle CssClass="alt" Width="1000" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="iActuacionId" HeaderText="iActuacionId" HeaderStyle-CssClass="ColumnaOculta"
                                                                            ItemStyle-CssClass="ColumnaOculta">
                                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="iActuacionDetalleId" HeaderText="iActuacionDetalleId" HeaderStyle-CssClass="ColumnaOculta"
                                                                            ItemStyle-CssClass="ColumnaOculta">
                                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="vOficinaUsuarioRegistro" HeaderText="Oficina / Usuario Registro" ItemStyle-Width="250">
                                                                            <ItemStyle Width="150px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="vCorrelativoActuacion" HeaderText="Correlativo Actuación" ItemStyle-Width="80">
                                                                            <ItemStyle Width="80px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="vCorrelativoTarifario" HeaderText="Correlativo Tarifario" ItemStyle-Width="80">
                                                                            <ItemStyle Width="80px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="dFechaRegistro" HeaderText="Fecha Registro" ItemStyle-Width="100" DataFormatString="{0:MMM-dd-yyyy}">
                                                                            <ItemStyle Width="120px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="vTarifa" HeaderText="Tarifa" ItemStyle-Width="30" >
                                                                            <ItemStyle Width="30px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="vDescripcion" HeaderText="Descripcion" ItemStyle-Width="160" >
                                                                            <ItemStyle Width="160px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="acde_dFechaModificacion" HeaderText="Fecha Eliminación" ItemStyle-Width="100" DataFormatString="{0:MMM-dd-yyyy}">
                                                                            <ItemStyle Width="160px" />
                                                                        </asp:BoundField>
                                                  
                                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" ItemStyle-Width="250">
                                                                            <ItemStyle Width="250px" />
                                                                        </asp:BoundField>
                                                                         <asp:BoundField DataField="observacion" HeaderText="Observacion" ItemStyle-Width="100">
                                                                            <ItemStyle Width="100px" />
                                                                        </asp:BoundField>
                                                                        
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                     
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
