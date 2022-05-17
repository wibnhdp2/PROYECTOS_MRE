<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmCorrelativo.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmCorrelativo" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagName="Validation" TagPrefix="Label" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar" TagPrefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <style type="text/css">
        .style2
        {
            width: 102px;
        }
        .style3
        {
            width: 100px;
        }
    </style>
   <style type="text/css">
          
        
        .scrolling-table-container {
            height: 378px;
            overflow-y: scroll;
            overflow-x: hidden;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2><asp:Label ID="lblTituloLibros" runat="server" Text="Configuración de Carga Inicial - Registro de Actuación"></asp:Label></h2>
                </td>
            </tr>
        </table>

        <table style="width: 90%" align="center">
            <tr>
                <td align="left">
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">
                                <asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                            <li><a href="#tab-2">
                                <asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>  
                                <asp:HiddenField ID="hdn_sOficinaConsularId" runat="server" Value="0" />                              
                                <table width="100%">  
                                    <tr>
                                        <td><asp:Label ID="lblOficinaConsularConsulta" runat="server" Text="Oficina Consular:"></asp:Label>
                                            <uc1:ctrlOficinaConsular ID="ddlOficinaConsularConsulta" runat="server" 
                                                Width="400px" />
                                        </td>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblPeriodo" runat="server" Text="Periodo:" Width="92"></asp:Label>
                                        <asp:DropDownList ID="ddlPeriodo" runat="server" Enabled="true" Width="60px">
                                            </asp:DropDownList>
                                        </td>
             
                                    </tr>
                                   
                                    <tr>
                                        <td colspan="3">
                                            <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;" colspan="2">
                                            <Label:Validation ID="ctrlValidacion" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="gdvCorrelativos" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt" Width="100%"
                                            AutoGenerateColumns="False" GridLines="None">
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Correlativo" DataField="corr_sCorrelativoId">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Oficina Consular" DataField="ubge_vDistrito">
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Periodo" DataField="CORR_sPeriodo">
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:BoundField>                                                
                                                <asp:BoundField HeaderText="Numero" DataField="tari_sNumero">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Letra" DataField="tari_vLetra">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Correlativo" DataField="corr_ICorrelativo">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="slt" />
                                        </asp:GridView>
                                        <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                    </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-2">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                
                                 <table width="100%">
                                <tr>

                                        <td colspan="1" class="style2">
                                            <asp:Button ID="btnGrabarCorrelativa" runat="server" Text="      Grabar" 
                                                CssClass="btnSave" onclick="btnGrabarCorrelativa_Click" OnClientClick="return EsRGEValido();"/> 
                                        </td>

                                        <td colspan="1">
                                        
                                        </td>

                                        
                                    </tr>
                                </table>
                           

                                <table width="100%">  
                                    

                                    <tr>
                                        <td class="style2"><asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label></td>
                                        <td colspan="2"><uc1:ctrlOficinaConsular ID="ddlOficinaConsularRegistro" runat="server" Width="400px" />
                                       
                                    </tr>

                                    <tr>
                                        <td class="style2"><asp:Label ID="lblPeriodoRegistro" runat="server" Text="Periodo:"></asp:Label></td>
                                        <td colspan="2">
                                        <asp:DropDownList ID="ddlPeriodoRegistro" runat="server" Width="400px"></asp:DropDownList>      

                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="style2"><asp:Label ID="lblSecciones" runat="server" Text="Sección:"></asp:Label></td>
                                        <td colspan="2">
                                        <asp:DropDownList ID="ddlSeccion" runat="server" Width="400px"></asp:DropDownList></td>
                                    </tr>
                              
                                    
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button ID="btnBuscar" runat="server" Text="     Buscar" CssClass="btnSearch" 
                                                onclick="btnBuscar_Click" /> 

                                            <asp:Button ID="btnSumarizar" runat="server" Text="     Buscar" CssClass="btnSearch" 
                                                onclick="btnSumarizar_Click" style="display:none;" /> 
                                        </td>
                                    </tr>
                                    </table>


                                    <table width="100%">  
                                    <tr>
                                        <td class="style3">

                                        <asp:Label ID="Label2" runat="server" Text="RGE:"></asp:Label>
                                        </td>
                                        <td>
                                        <asp:TextBox ID="txtRGE" runat="server" TabIndex="10" Width="100" MaxLength="10" 
                                                CssClass="txtLetra" onkeypress="return isNumberKey(event)">0</asp:TextBox>
                                        </td>
                                        <td>
                                        <asp:Label ID="Label1" runat="server" Text="TOTAL:" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="txtTOTAL" runat="server" Enabled="false" CssClass="txtLetra" Text="0" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            
                                        </td>

                                    </tr>

                                    </table>

                                    <table width="100%">  
                                    <tr>
                                        <td colspan="5">
                                        <div id="divGridView" class="scrolling-table-container" runat="server" style="height:350px;">
                                        
                                           <asp:GridView ID="gvdTarifas" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                            AutoGenerateColumns="False" GridLines="None" ShowHeaderWhenEmpty="True" 
                                                onrowdatabound="gvdTarifas_RowDataBound">
                                            <Columns>
                                                <asp:BoundField HeaderText="Nro." DataField="tari_sNumero" >
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:BoundField>

                                                <asp:BoundField HeaderText="Letra" DataField="tari_vLetra">
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:BoundField>

                                                <asp:BoundField HeaderText="Descripción" DataField="tari_vDescripcionCorta">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>

                                                <asp:BoundField HeaderText="" DataField="tari_sTarifarioId" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <ItemStyle HorizontalAlign="Center" CssClass="ColumnaOculta" />
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>

                                                <asp:BoundField HeaderText="" DataField="corr_ICorrelativo" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <ItemStyle HorizontalAlign="Center" CssClass="ColumnaOculta" Width="100px" />
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>

                                               <asp:TemplateField HeaderText="Correlativo" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCorrelativo" runat="server" Width="35px"  style=" margin:0px 0px 0px 5px;" 
                                                        onkeypress="return isNumberKey(event)" MaxLength="5" onblur='ClickSumarizacion();'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px"/>
                                                </asp:TemplateField>
                                               
                                            </Columns>
                                            <SelectedRowStyle CssClass="slt"></SelectedRowStyle>
                                            <EmptyDataTemplate>
                                                <table ID="tbSinDatos">
                                                    <tbody>
                                                        <tr>
                                                            <td width="10%">
                                                                <asp:Image ID="imgWarning" runat="server" 
                                                                    ImageUrl="../Images/img_16_warning.png" />
                                                            </td>
                                                            <td width="5%">
                                                            </td>
                                                            <td width="85%">
                                                                <asp:Label ID="lblSinDatos" runat="server" Text="No se encontraron Datos..."></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        </div>
                                        <PageBarContent:PageBar ID="PageBarRegistro" runat="server" 
                                                OnClick="ctrlPageBarRegistro_Click" PageSize="10"/>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        $(function () {
            $('#tabs').tabs();
        });

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

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

        function isNumeroLetra(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 32) {
                letra = true;
            }
            if (charCode > 47 && charCode < 58) {
                letra = true;
            }
            if (charCode > 64 && charCode < 91) {
                letra = true;
            }
            if (charCode > 96 && charCode < 123) {
                letra = true;
            }
            if (charCode == 130) {
                letra = true;
            }
            if (charCode == 144) {
                letra = true;
            }
            if (charCode > 159 && charCode < 164) {
                letra = true;
            }
            if (charCode == 181) {
                letra = true;
            }
            if (charCode == 214) {
                letra = true;
            }
            if (charCode == 224) {
                letra = true;
            }
            if (charCode == 233) {
                letra = true;
            }

            var letras = "áéíóúñÑ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }


            letras = "¡";
            tecla = String.fromCharCode(charCode);
            n = letras.indexOf(tecla);
            if (n > -1) {
                letra = false;
            }

            return letra;
        }


        function EsRGEValido() {

            var txtRGE = document.getElementById("<%=txtRGE.ClientID %>");

            if ($.trim(txtRGE.value) == '') {
                txtRGE.style.border = "1px solid Red";
                return false;

            }
            else {
                txtRGE.style.border = "1px solid #888888";
                return true;
            }
        }

        function ClickSumarizacion() {
        }
    </script>
</asp:Content>