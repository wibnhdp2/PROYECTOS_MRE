<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReactivarInsumos.aspx.cs" Inherits="SGAC.WebApp.Configuracion.frmReactivarInsumos" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlValidation.ascx" TagName="validation" TagPrefix="label" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <style type="text/css">
        .style1
        {
            width: 156px;
        }
    </style>
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
            var rangoInicio = document.getElementById('<%= txtRanIni.ClientID %>').value;
            var rangoFin = document.getElementById('<%= txtRanFin.ClientID %>').value;
            document.getElementById('<%= txtCant.ClientID %>').value = "";


            if (parseInt(rangoInicio) <= 0) {
                document.getElementById('<%= txtRanIni.ClientID %>').value = '';
                alert("El rango no puede ser menor o igual a cero.");
                return;
            }

            if (parseInt(rangoFin) <= 0) {
                document.getElementById('<%= txtRanFin.ClientID %>').value
                alert("El rango no puede ser menor o igual a cero.");
                return;
            }

            var RanIni = 0;
            var RanFin = 0;
            var Cant = 0;



            if (rangoInicio == "") {
                RanIni = 0;
            }
            else {
                RanIni = rangoInicio;
            }
            if (rangoFin == "") {
                RanFin = 0;
            }
            else {
                RanFin = rangoFin;
            }


            if (rangoFin != "" && rangoInicio != "") {
                if (parseInt(RanFin) >= parseInt(RanIni)) {
                    Cant = ((RanFin - RanIni) + 1);
                    document.getElementById('<%= txtCant.ClientID %>').value = Cant;
                }
                else {
                    alert("El rango Final debe ser mayor o igual al inicial");
                    Cant = 0;
                    document.getElementById('<%= txtCant.ClientID %>').value = Cant;
                }
            }
        }
</script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTitulo" runat="server" Text="Reactivar - Insumos:"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <table style="width: 90%;"  align="center" class="mTblPrincipal">
    <tr>
            <td>
                 <asp:Label ID="Label1" runat="server" 
                     Text="*   Para realizar la busqueda se tiene que agregar los rangos que se muestran en el lote del movimiento" 
                     ForeColor="#CC3300"></asp:Label><br />
                 <asp:Label ID="Label2" runat="server" 
                     Text="*   En caso sea un autoadhesivo de baja la busqueda se realiza de forma individual" 
                     ForeColor="#CC3300"></asp:Label>
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
                                    <td> Rangos: </td>
                                    <td>
                                        <asp:TextBox ID="txtRanIni" runat="server" CssClass="campoNumero" 
                                                                    Font-Size="10pt" MaxLength="10" onBlur="CalculaRangos()" 
                                                                    onkeypress="return isNumberKey(event)" Width="90px"></asp:TextBox>

                                     </td>
                                     <td>
                                     <span style="margin-left:10px;">Rango Final:</span> <asp:TextBox ID="txtRanFin" 
                                             runat="server" CssClass="campoNumero" 
                                                                    Font-Size="10pt" MaxLength="10" onBlur="CalculaRangos()" 
                                                                    onkeypress="return isNumberKey(event)" Width="90px"></asp:TextBox>
                                        <span style="margin-left:10px;">Cantidad:</span> <asp:TextBox ID="txtCant" runat="server" Enabled="False" 
                                                                    Font-Size="10pt" Style="text-align: right" Width="60px"></asp:TextBox>
                                    
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
                                <td colspan="6"><Label:Validation ID="ctrlValidacion" runat="server" /></td>
                               </tr>
                        </table>
                </div>
            </td>
        </tr>
    </table>
    <table style="width: 90%;"  align="center" class="mTblPrincipal">
                                    <tr>
                                        <td class="style3">
                                            <asp:Button ID="btnEjecutar" runat="server" Text="Dar Baja" Style="display: none;"/>
                                            <table align="left" style="width: 594px; height: 28px; margin-top: 3px;">
                                                <tr>
                                                    <td style="width: 594px" align="left">
                                                        <Label:Validation ID="Validation1" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="updGrillaConsulta" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="grReporteGestion" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="alt"
                                                                    AutoGenerateColumns="False" CssClass="mGrid" DataKeyNames="Estado" GridLines="None"
                                                                    Font-Size="8pt" Width="850px">
                                                                    <AlternatingRowStyle CssClass="alt" Width="1000" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="insu_iInsumoId" HeaderText="insu_iInsumoId" HeaderStyle-CssClass="ColumnaOculta"
                                                                            ItemStyle-CssClass="ColumnaOculta">
                                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="insu_iMovimientoId" HeaderText="insu_iMovimientoId" HeaderStyle-CssClass="ColumnaOculta"
                                                                            ItemStyle-CssClass="ColumnaOculta">
                                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" ItemStyle-Width="850">
                                                                            <ItemStyle Width="280px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="insu_vPrefijoNumeracion" HeaderText="Prefijo" ItemStyle-Width="80">
                                                                            <ItemStyle Width="80px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="insu_vCodigoUnicoFabrica" HeaderText="Código" ItemStyle-Width="120">
                                                                            <ItemStyle Width="120px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="FechaRGE" HeaderText="Fecha R.G.E" ItemStyle-Width="90" DataFormatString="{0:MMM-dd-yyyy}">
                                                                            <ItemStyle Width="160px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha Movimiento" ItemStyle-Width="90" DataFormatString="{0:MMM-dd-yyyy}">
                                                                            <ItemStyle Width="160px" />
                                                                        </asp:BoundField>
                                                                        
                                                                        <asp:BoundField DataField="insu_sEstadoId" HeaderText="insu_sEstadoId" HeaderStyle-CssClass="ColumnaOculta"
                                                                            ItemStyle-CssClass="ColumnaOculta">
                                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" ItemStyle-Width="250">
                                                                            <ItemStyle Width="350px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Observación" HeaderText="Observación" ItemStyle-Width="250">
                                                                            <ItemStyle Width="350px" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <pagebarcontent:pagebar ID="ctrlPaginador" runat="server" 
                                                            OnClick="ctrlPaginador_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
</asp:Content>
