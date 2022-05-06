<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmRegistroEmpresa.aspx.cs"
    Inherits="SGAC.WebApp.Registro.FrmRegistroEmpresa" %>


<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>


<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.4.custom.min.js"></script>
    

    <script type="text/javascript">
        $(function () {
            $('#tabs').tabs();
        });


        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
    </script>
   
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--Consulta --%>
    <asp:HiddenField ID="HFGUID" runat="server" />
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloPerJuridica" runat="server" Text="Registro Persona Jurídica"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <table class="mTblPrincipal" align="center">
        <tr>
            <td>
                <ToolBar:ToolBarContent ID="ctrlToolBar" runat="server"></ToolBar:ToolBarContent>
                <asp:UpdatePanel ID="upnlEmpresa" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="hidDocumentoSoloNumero" runat="server" value="0"/>
                        <asp:HiddenField ID="HF_ValoresDocumentoIdentidad" Value="" runat="server" />
                        <asp:HiddenField ID="HF_TipoDocumento_Representante_Columna_Indice" Value="" runat="server" />                        
                        <asp:HiddenField ID="HF_NumeroDocumento_Representante_Columna_Indice" Value="" runat="server" />
                        <asp:Panel ID="Panel" runat="server" GroupingText="Empresa" BorderColor="#990000">
                            <table align="center" width="100%">
                                <tr>
                                    <td id="tdMsje" colspan="5">
                                        <asp:Label ID="lblValidacion" runat="server" Text="Debe ingresar los campos requeridos (*)"
                                            ForeColor="Red" Font-Size="14px">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTipoDocumento" runat="server" Text="Tipo Documento :" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="200px" Enabled="True"
                                            Height="21px" onchange="javascript:ddlTipoDocumento_SelectedIndexChanged();">
                                            <asp:ListItem Text="- SELECCIONE -"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblTipDocumento" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                    </td>
                                    <td width="50px">
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNroDocumento" runat="server" Text="Nro Documento :" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNroDocumento" runat="server" Width="280px" MaxLength="20" style="text-transform: uppercase;" />
                                        <asp:Label ID="lblVal_txtNroDocumento" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRazonSocial" runat="server" Text="Razón Social :" />
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtRazonSocial" runat="server" Width="600px" CssClass="txtLetra"
                                            onkeypress="return isLetraNumeroDoc(event)" onBlur="return conMayusculas(this)"
                                            MaxLength="150" />
                                        <asp:Label ID="lblVal_txtRazonSocial" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTipoEmpresa" runat="server" Text="Tipo Empresa :" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTipoEmpresa" runat="server" Width="200px" Height="21px">
                                            <asp:ListItem Text=" - SELECCIONE -"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblTipoEmpresa2" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                    </td>
                                    <td width="50px">
                                    </td>
                                   <%-- <td>
                                        &nbsp;
                                        <asp:Label ID="lblCondicion" runat="server" Text="Condición :" />
                                    </td>--%>
                                    <%--<td>
                                        &nbsp;
                                        <asp:DropDownList ID="ddlCondicion" runat="server" Enabled="False" Height="21px"
                                            Width="200px">
                                            <asp:ListItem Text="ACTIVO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table width="100%;">
                    <tr>
                        <td>
                            <div id="tabs">
                                <ul>
                                    <li><a href="#tab-1">
                                        <asp:Label ID="lblTabDatAdic" runat="server" Text="Datos Adicionales"></asp:Label></a></li>
                                    <li><a href="#tab-2">
                                        <asp:Label ID="lblTabFiliacion" runat="server" Text="Representantes Legales"></asp:Label></a></li>
                                    <li><a href="#tab-3">
                                        <asp:Label ID="lblTabDirecciones" runat="server" Text="Direcciones"></asp:Label></a></li>
                                </ul>
                                <div id="tab-1">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTelefono" runat="server" Text="Teléfono:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTelefono" runat="server" Width="200px" onkeypress="return validarTelefonos(event)"
                                                    MaxLength="30" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCorreo" runat="server" Text="Correo:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="txtLetra" Width="400px" onkeydown="return (event.keyCode!=13);"
                                                    onBlur="if(this.value!=''){ validarEmail(this); }" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblActividadComercial" runat="server" Text="Actividad Comercial :" />
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtActividadComercial" runat="server" Width="400px" CssClass="txtLetra"
                                                    onkeypress="return NoCaracteresEspeciales(event)" onBlur="conMayusculas(this)" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="tab-2">
                                    <asp:UpdatePanel ID="UpnlRepresentante" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRepresentanteTipoDoc" runat="server" Text="Tipo Documento:" />
                                                    </td>
                                                    <td>
                                                        <%-- 
                                                        <asp:DropDownList ID="ddlRepresentanteTipoDoc" runat="server" Width="200px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlRepresentanteTipoDoc_SelectedIndexChanged" Height="21px">
                                                        </asp:DropDownList> --%>
                                                        <asp:DropDownList ID="ddlRepresentanteTipoDoc" runat="server" Width="200px" Height="21px"
                                                            onchange="javascript:ddlRepresentanteTipoDoc_SelectedIndexChanged();">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label3" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    <td width="30px">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRepresentanteNroDoc" runat="server" Text="Nro. Documento:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRepresentanteNroDoc" runat="server" Width="180px" onkeypress="return isCantidadTipoDocumento(event)"
                                                            MaxLength="20" CssClass="txtLetra" onblur="BuscarPersona();"/>
                                                        <asp:ImageButton ID="imgBuscar" ImageUrl="~/Images/img_16_search.png" runat="server"
                                                            OnClick="imgBuscar_Click" />
                                                        <asp:Label ID="Label5" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNacionalidad" runat="server" Text="Nacionalidad" />
                                                        :</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlNacionalidad" runat="server" Width="200px" Height="21px">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label7" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    <td width="30px">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblNombres" runat="server" Text="Nombres:" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNombres" runat="server" Width="200px" CssClass="txtLetra" onkeypress="return isNombreApellido(event)"
                                                            onBlur="conMayusculas(this)" MaxLength="30" />
                                                        <asp:Label ID="Label9" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPrimerApellido" runat="server" Text="Primer Apellido:" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPrimerApellido" runat="server" Width="196px" CssClass="txtLetra"
                                                            onkeypress="return isNombreApellido(event)" onBlur="conMayusculas(this)" MaxLength="30" />
                                                        <asp:Label ID="Label11" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    <td width="30px">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSegundoApellido" runat="server" Text="Segundo Apellido:" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSegundoApellido" runat="server" Width="200px" CssClass="txtLetra"
                                                            onkeypress="return isNombreApellido(event)" onBlur="conMayusculas(this)" MaxLength="30" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAgregarRepresentante" runat="server" Text="    Adicionar" CssClass="btnNewDirFil"
                                                            Width="120px" OnClick="btnAgregarRepresentante_Click" OnClientClick="return ValidarDatosRepresentante()" />
                                                        <asp:Button ID="btnLimpiarRep" runat="server" Text="    Limpiar" Width="120px" CssClass="btnLimpiar"
                                                            OnClick="btnLimpiarRep_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField runat="server" ID="hid_iRepresentanteId" Value="0" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="gdvRepresentante" runat="server" CssClass="mGrid" AutoGenerateColumns="False"
                                                            Width="100%" ShowHeaderWhenEmpty="True" DataKeyNames="sPersonaId,sDocumentoTipoId,sNacionalidadId"
                                                            OnRowCommand="gdvRepresentante_RowCommand" OnRowDataBound="gdvRepresentante_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField DataField="vDocumentoTipo" HeaderText="Tipo Documento">
                                                                    <HeaderStyle Width="100px" />
                                                                    <ItemStyle Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vDocumentoNumero" HeaderText="Nro. Documento">
                                                                    <HeaderStyle Width="100px" />
                                                                    <ItemStyle Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vApellidoPaterno" HeaderText="Primer Apellido">
                                                                    <HeaderStyle Width="120px" />
                                                                    <ItemStyle Width="120px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vApellidoMaterno" HeaderText="Segundo Apellido">
                                                                    <HeaderStyle Width="120px" />
                                                                    <ItemStyle Width="120px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vNombres" HeaderText="Nombres">
                                                                    <ItemStyle Width="200px" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEliminarRL" CommandName="Eliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            runat="server" ImageUrl="../Images/img_grid_delete.png" OnClientClick="return confirm('Desea Eliminar el registro');" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="sDocumentoTipoId" HeaderText="sDocumentoTipoId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                                    <HeaderStyle Width="120px" />
                                                                    <ItemStyle Width="120px" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <SelectedRowStyle CssClass="slt" />
                                                            <RowStyle Font-Names="Arial" Font-Size="11px"></RowStyle>
                                                            <EmptyDataTemplate>
                                                                <table id="tbSinDatos">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td width="10%">
                                                                                <asp:Image runat="server" ID="imgWarning" ImageUrl="../Images/img_16_warning.png" />
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
                                                            <AlternatingRowStyle />
                                                            <PagerStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="tab-3">
                                    <asp:UpdatePanel ID="UpnlDirecciones" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="LblDirDir" runat="server" Text="Dirección :" />
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:TextBox ID="txtEmpresaDireccion" runat="server" CssClass="txtLetra" MaxLength="200"
                                                            Width="580px" onkeydown="return (event.keyCode!=13);" />
                                                        <asp:Label ID="Label1" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="LblTipRes" runat="server" Text="Tipo:" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlTipoResidencia" runat="server" Width="200px" Height="21px">
                                                            <asp:ListItem Text="DOMICILIO FISCAL"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label2" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    <td width="20px">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDepartamentoCont" runat="server" Text="Dpto./Continente:" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlContinente" runat="server" Width="200px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlContinente_SelectedIndexChanged" Height="21px">
                                                            <asp:ListItem Text=" - SELECCIONE - "></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label4" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblProvinciaPais" runat="server" Text="Prov./País:" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlPais" runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged"
                                                            Height="21px">
                                                            <asp:ListItem Text=" - SELECCIONE - "></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label6" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    <td width="20px">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDistritoCiudad" runat="server" Text="Dist./Ciudad/Estado:" 
                                                            Width="120px" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCiudad" runat="server" Width="200px" Height="21px">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label8" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="120px">
                                                        <asp:Label ID="lblCodigoPostal" runat="server" Text="Código Postal:"></asp:Label>
                                                    </td>
                                                    <td align="left" width="230px">
                                                        <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="txtLetra" MaxLength="20"
                                                            onkeypress="return isLetraNumeroDoc(event)" Width="196px" />
                                                        <asp:Label ID="Label10" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    <td width="20px">
                                                    </td>
                                                    <td align="left" width="120px">
                                                        <asp:Label ID="LblTelfDir" runat="server" Text="Teléfonos :" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtResidenciaTelefono" runat="server" Width="196px" MaxLength="30"
                                                            onkeypress="return validarTelefonos(event)" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAgregarDireccion" runat="server" Text="    Adicionar" Width="120px"
                                                            CssClass="btnNewDirFil" OnClick="btnAgregarDireccion_Click" OnClientClick="return ValidarDatosResidencia()" />
                                                        <asp:Button ID="btnLimpiarDir" runat="server" Text="    Limpiar" Width="120px" CssClass="btnLimpiar"
                                                            OnClick="btnLimpiarDir_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="gdvDirecciones" runat="server" CssClass="mGrid" AutoGenerateColumns="False"
                                                            Width="100%" OnRowCommand="gdvDirecciones_RowCommand" ShowHeaderWhenEmpty="True"
                                                            DataKeyNames="iEmpresaId,iResidenciaId,sResidenciaTipoId,cResidenciaUbigeo,cUbigeo01,cUbigeo02,cUbigeo03"
                                                            OnRowDataBound="gdvDirecciones_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField DataField="vResidenciaDireccion" HeaderText="Dirección">
                                                                    <HeaderStyle Width="150px" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vDescripcionResidencia" HeaderText="Tipo Residencia">
                                                                    <HeaderStyle Width="100px" />
                                                                    <ItemStyle Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vDepartamento" HeaderText="Dpto./Cont.">
                                                                    <HeaderStyle Width="120px" />
                                                                    <ItemStyle Width="120px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vProvincia" HeaderText="Prov./País">
                                                                    <HeaderStyle Width="120px" />
                                                                    <ItemStyle Width="120px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vDistrito" HeaderText="Dist./Ciudad/Estado">
                                                                    <HeaderStyle Width="120px" />
                                                                    <ItemStyle Width="120px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vCodigoPostal" HeaderText="Código Postal">
                                                                    <HeaderStyle Width="80px" />
                                                                    <ItemStyle Width="80px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vResidenciaTelefono" HeaderText="Teléfonos">
                                                                    <HeaderStyle Width="120px" />
                                                                    <ItemStyle Width="120px" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEditarER" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEliminarER" CommandName="Eliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            runat="server" ImageUrl="../Images/img_grid_delete.png" OnClientClick="return confirm('Desea Eliminar el registro');" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <SelectedRowStyle CssClass="slt" />
                                                            <RowStyle Font-Names="Arial" Font-Size="11px"></RowStyle>
                                                            <EmptyDataTemplate>
                                                                <table id="tbSinDatos">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td width="10%">
                                                                                <asp:Image runat="server" ID="imgWarning" ImageUrl="../Images/img_16_warning.png" />
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
                                                            <AlternatingRowStyle />
                                                            <PagerStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
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
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">

        function ddlTipoDocumento_SelectedIndexChanged() {
            var txtNroDocumento = $("#<%=txtNroDocumento.ClientID %>");
            txtNroDocumento.val("");
            txtNroDocumento.focus();
        }
        function ddlRepresentanteTipoDoc_SelectedIndexChanged() {

            var txtRepresentanteNroDoc = $("#<%=txtRepresentanteNroDoc.ClientID %>");
            var ddlRepresentanteTipoDoc = $("#<%=ddlRepresentanteTipoDoc.ClientID %>");
            var ddlRepresentanteTipoDocDesc = $.trim($("#<%= ddlRepresentanteTipoDoc.ClientID %> option:selected").text());
            var ddlNacionalidad = $("#<%=ddlNacionalidad.ClientID %>");

            txtRepresentanteNroDoc.val("");

            if (ddlRepresentanteTipoDoc.val() != "0") {
                var valoresDocumento = ObtenerMaxLenghtDocumentos(ddlRepresentanteTipoDoc.val()).split(",");

                txtRepresentanteNroDoc.attr('maxlength', valoresDocumento[1]);


                if (valoresDocumento[2] == "True") {
                    $("#<%= hidDocumentoSoloNumero.ClientID %>").val("1");
                }
                else {
                    $("#<%= hidDocumentoSoloNumero.ClientID %>").val("0");
                }


                if (valoresDocumento[4] != '') {
                    ddlNacionalidad.val(valoresDocumento[4]);
                    ddlNacionalidad.attr('disabled', true);
                }
                else {
                    ddlNacionalidad.val(0);
                    ddlNacionalidad.attr('disabled', false);
                }

                txtRepresentanteNroDoc.focus();
            }
            else {
                ddlNacionalidad.val(0);
                ddlNacionalidad.attr('disabled', false);
            }

            
            

        }

        function ObtenerMaxLenghtDocumentos(id) {

            var hfDocumentoIdentidad = $("#<%=HF_ValoresDocumentoIdentidad.ClientID %>").val();

            var documentos = hfDocumentoIdentidad.split("|");

            for (i = 0; i < documentos.length - 1; i++) {

                var valores = documentos[i].split(",");

                if (valores[0] == id) {
                    return valores[1] + "," + valores[2] + "," + valores[3] + "," + valores[5] + "," + valores[4];
                }

            }

            return "";



        }


        function BuscarPersona() {

            if ($("#<%=txtRepresentanteNroDoc.ClientID %>").val() != '')
                $("#<%=imgBuscar.ClientID %>").click();
            

        }

        function execute(urlmetodo, parametros) {
            var rsp;
            $.ajax({
                url: urlmetodo,
                type: "POST",
                data: JSON.stringify(parametros),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: false,
                cancel: false,
                success: function (response) {
                    rsp = response;
                },
                failure: function (msg) {
                    alert(msg);
                    rsp = msg;
                },
                error: function (xhr, status, error) {
                    alert(error);
                    rsp = error;
                }
            });

            return rsp;
        }

        function funSetUbigeoProvincia() {
            var url = 'FrmRegistroEmpresa.aspx/obtener_provincias';
            var prm = {};
            prm.ubigeo = $("#<%=ddlContinente.ClientID %>").val();
            var rspta = execute(url, prm);
            var ubigeo = $.parseJSON(rspta.d);

            var ddl = $("#<%= ddlPais.ClientID %>");
            ddl.empty();
            ddl.append($('<option></option>').val("0").html(" - SELECCIONAR - "));
            for (var i = 0; i < ubigeo.length; i++) {
                var Ubigeo = ubigeo[i];
                ddl.append($('<option></option>').val(Ubigeo.ValueField).html(Ubigeo.TextField));
            }
        }

        function funSetUbigeoDistrito() {
            var url = 'FrmRegistroEmpresa.aspx/obtener_distritos';
            var prm = {};
            prm.departamento = $("#<%=ddlContinente.ClientID%>").val();
            prm.provincia = $("#<%=ddlPais.ClientID%>").val();
            var rspta = execute(url, prm);
            var ubigeo = $.parseJSON(rspta.d);

            var ddl = $("#<%= ddlCiudad.ClientID %>");
            ddl.empty();
            ddl.append($('<option></option>').val("0").html(" - SELECCIONAR - "));
            for (var i = 0; i < ubigeo.length; i++) {
                var Ubigeo = ubigeo[i];
                ddl.append($('<option></option>').val(Ubigeo.ValueField).html(Ubigeo.TextField));
            }
        }
        //-- FUNCIONES-------------------

        function validarEmail(ctrl) {
            var email = ctrl.value;

            if (email.length > 0) {
                expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!expr.test(email)) {
                    //alert("xxxx");
                    showpopupother("a", "Información", "Error: La dirección de correo " + email + " es incorrecta.", "false", 200, 250);
                    ctrl.value = "";
                }
            }
        }

        //-- VALIDACION GENERAL---------
        function txtcontrolError(ctrl) {
            var x = ctrl.value.trim();
            var bolValida = true;

            if (x.length == 0) {
                ctrl.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ctrl.style.border = "1px solid #888888";
            }
            return bolValida;
        }

        function ddlcontrolError(ctrl) {
            var x = ctrl.selectedIndex;
            var bolValida = true;
            if (x < 1) {
                ctrl.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ctrl.style.border = "1px solid #888888";
            }
            return bolValida;
        }

        function ValidarDatosRepresentante() {





            var bolValida = true;

            if (ddlcontrolError(document.getElementById("<%= ddlRepresentanteTipoDoc.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtRepresentanteNroDoc.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddlNacionalidad.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtNombres.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtPrimerApellido.ClientID %>")) == false) bolValida = false;
            //            if (txtcontrolError(document.getElementById("<%= txtSegundoApellido.ClientID %>")) == false) bolValida = false;




            var tipoDocumento = $("#<%= ddlRepresentanteTipoDoc.ClientID %>").val();
            var nroDocumento = $("#<%= txtRepresentanteNroDoc.ClientID %>").val();
            var tipoDocumentoIndiceColumna = $("#<%= HF_TipoDocumento_Representante_Columna_Indice.ClientID %>").val();
            var nroDocumentoIndiceColumna = $("#<%= HF_NumeroDocumento_Representante_Columna_Indice.ClientID %>").val();



            $('#<%=gdvRepresentante.ClientID %> tbody tr').each(function () {

                var bNumeroTipoDocumento = false;
                $(this).children("td").each(function (index) {

                    if (!bolValida)
                        return false;

                    if (index == nroDocumentoIndiceColumna) {

                        if (nroDocumento  == $(this).text())
                            bNumeroTipoDocumento = true;
                        else
                            return;
                    }
                    else if (index == tipoDocumentoIndiceColumna) {
                        if (tipoDocumento == $(this).text() && bNumeroTipoDocumento) {

                            showpopupother("a", "Información", "El tipo y número de documento ya se encuentran ingresados.", "false", 200, 250);

                            bolValida = false;
                            return;

                        }
                    }

                });


            });

            return bolValida;
        }

        function ValidarDatosResidencia() {
            var bolValida = true;

            if (txtcontrolError(document.getElementById("<%= txtEmpresaDireccion.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddlTipoResidencia.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddlContinente.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddlPais.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddlCiudad.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtCodigoPostal.ClientID %>")) == false) bolValida = false;

            return bolValida;
        }

        //-- FIN VALIDACION GENERAL---------
        function ValidarGrabar() {
            var bolValida = true;

            if (ddlcontrolError(document.getElementById("<%= ddlTipoDocumento.ClientID %>")) == false) bolValida = false;

            if (txtcontrolError(document.getElementById("<%= txtNroDocumento.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtRazonSocial.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddlTipoEmpresa.ClientID %>")) == false) bolValida = false;

            //            if (ddlcontrolError(document.getElementById("<%= ddlRepresentanteTipoDoc.ClientID %>")) == false) bolValida = false;
            //            if (txtcontrolError(document.getElementById("<%= txtRepresentanteNroDoc.ClientID %>")) == false) bolValida = false;
            //            if (ddlcontrolError(document.getElementById("<%= ddlNacionalidad.ClientID %>")) == false) bolValida = false;
            //            if (txtcontrolError(document.getElementById("<%= txtNombres.ClientID %>")) == false) bolValida = false;
            //            if (txtcontrolError(document.getElementById("<%= txtPrimerApellido.ClientID %>")) == false) bolValida = false;
            //            if (txtcontrolError(document.getElementById("<%= txtSegundoApellido.ClientID %>")) == false) bolValida = false;

            //            if (txtcontrolError(document.getElementById("<%= txtEmpresaDireccion.ClientID %>")) == false) bolValida = false;
            //            if (ddlcontrolError(document.getElementById("<%= ddlTipoResidencia.ClientID %>")) == false) bolValida = false;
            //            if (ddlcontrolError(document.getElementById("<%= ddlContinente.ClientID %>")) == false) bolValida = false;
            //            if (ddlcontrolError(document.getElementById("<%= ddlPais.ClientID %>")) == false) bolValida = false;
            //            if (ddlcontrolError(document.getElementById("<%= ddlCiudad.ClientID %>")) == false) bolValida = false;
            //            if (txtcontrolError(document.getElementById("<%= txtCodigoPostal.ClientID %>")) == false) bolValida = false;

            if (bolValida) {
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }

            return bolValida;

        }

        //--VALIDACION HORAS--

        function validaHora(ctrl) {
            var x = ctrl.value;
            var valor = true;

            if (x.length == 4) {
                x = x + "0";
                ctrl.value = x;
            }

            var hora = x.substring(0, 2);
            var minuto = x.substring(3, 5);

            if (hora > 23) {
                valor = false;
            }
            if (minuto > 59) {
                valor = false;
            }
            if (x.length < 5) {
                valor = false;
            }
            if (x.length == 0) {
                valor = true;
            }
            if (!valor) {

                ctrl.focus();
                showpopupother("a", "Información", "Formato de hora Incorrecto.",
                        "false", 200, 250);
                ctrl.value = "";
            }
        }

        function isHoraKey(ctrl, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var FIND = ":";
            var x = ctrl.value;
            var y = x.indexOf(FIND);

            var minuto = x.split(':');
            if (x.length == 2) {
                x = x + ":";
                ctrl.value = x;
            }

            if (x.split(':').length = 1) {
                if (x.length > 1 && x.length < 3) {
                    if (minuto[0].length > 1 && charCode != 58) {
                        return false;
                    }
                }
            }

            if (charCode == 8) {
                letra = true;
            }

            if (charCode == 58) {
                return false;
            }

            if (charCode < 48 || charCode > 58)
                return false;
            return true;
        }
        //--FIN VALIDACION HORAS--

        function conMayusculas(field) {
            field.value = field.value.toUpperCase();
        }

        function isNombreApellido(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 13) {
                letra = false;
            }
            if (charCode == 32) {
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
            if (charCode > 159 && charCode < 166) {
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

            var letras = "áéíóúÁÉÍÓÚñÑäëïöüÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }

        function NoCaracteresEspeciales(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = true;

            if (charCode == 13) {
                letra = false;
            }
            if (charCode == 37) {
                letra = false;
            }
            if (charCode == 38) {
                letra = false;
            }
            if (charCode == 60) {
                letra = false;
            }
            if (charCode == 62) {
                letra = false;
            }

            return letra;
        }

        function isLetraNumero(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 13) {
                letra = false;
            }
            if (charCode == 32) {
                letra = true;
            }
            if (charCode > 39 && charCode < 60) {
                letra = true;
            }
            if (charCode > 63 && charCode < 91) {
                letra = true;
            }
            if (charCode > 94 && charCode < 123) {
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

            var letras = "áéíóúÁÉÍÓÚñÑäëïöüÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }

        function isLetraNumeroDoc(evt) {
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
            if (charCode >= 58 && charCode <= 59) {
                letra = true;
            }
            if (charCode >= 46 && charCode <= 60) {
                letra = true;
            }
            if (charCode > 63 && charCode < 91) {
                letra = true;
            }
            if (charCode > 94 && charCode < 123) {
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

            var letras = "aeiouñAEIOU-Ñ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }

        //--VALIDACION NUMEROS--
        function validanumeroLostFocus(control) {
            var valor = true;
            var texto = control.value.trim();
            control.value = texto;

            var letras = "0123456789.";
            var n = 0;
            while (n < texto.length) {
                var x = texto.substring(n, n + 1)
                if (letras.indexOf(x) < 0) {
                    valor = false;
                }
                n++;
            }

            if (texto.substring(0, 1) == ".") {
                valor = false;
            }

            if (texto.substring(n - 1, n) == ".") {
                valor = false;
            }

            if (!valor) {
                control.focus();
                showpopupother("a", "Información", "NÚMERO INCORRECTO",
                        "false", 200, 250);
                control.value = "";
            }

        }

        function isNumeroTelefono(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;

            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 32) {
                letra = true;
            }
            if (charCode > 47 && charCode < 58) {
                letra = true;
            }

            var letras = "áéíóúñÑ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }

        function numDigitosDocumento(field) {
            field.value = field.value.toUpperCase();
            var ddl = document.getElementById("<%= ddlRepresentanteTipoDoc.ClientID %>")

            var codDocumento = ddl.options[ddl.selectedIndex].value;

            if (field.value.length > 0) {
                if (codDocumento == 1) {
                    if (field.value.length != 8) {
                        showpopupother("a", "Información", "NÚMERO DE DOCUMENTO INCORRECTO",
                        "false", 200, 250);
                        field.value = "";
                    }
                }
                else {
                    if (field.value.length < 5) {
                        showpopupother("a", "Información", "NÚMERO DE DOCUMENTO INCORRECTO",
                        "false", 200, 250);
                        field.value = "";
                    }
                }
                if (field.value.length == 8) {
                    document.getElementById("<%= imgBuscar.ClientID %>").click();
                }
            }
        }

        //        function numDigitosDocumentoRuc(field) {
        //            field.value = field.value.toUpperCase();
        //            var ddl = document.getElementById("<%= ddlTipoDocumento.ClientID %>")

        //            var codDocumento = ddl.options[ddl.selectedIndex].text;

        //            if(field.value.trim()!='')
        //                numero = parseInt(field.value.trim());
        //            if (field.value.length > 0) {
        //                if (codDocumento == "RUC") {
        //                    if (field.value.length != 11 || numero===0) {
        //                        $("#<%= lblValidacion.ClientID %>").html('NÚMERO DE DOCUMENTO INCORRECTO.');
        //                        field.css("border", "solid Red 1px");
        //                        field.value = "";
        //                    }
        //                }
        //                else if (codDocumento != "RUC") {
        //                    if (field.value.length < 5) {
        //                        $("#<%= lblValidacion.ClientID %>").html('NÚMERO DE DOCUMENTO INCORRECTO.');
        //                        field.css("border", "solid Red 1px");
        //                        field.value = "";
        //                    }
        //                }
        //            }
        //        }

        function numDigitosDocumentoRuc() {

            var strNumeroDocumento = $("#<%=txtNroDocumento.ClientID %>").val();
            var codDocumento = $("#<%=ddlTipoDocumento.ClientID %>").val();


            if (codDocumento != '0') {

                if ($.trim(strNumeroDocumento) != '') {

                    if ($.trim(strNumeroDocumento).length > 0) {

                        if (codDocumento == '2851') {
                            if ($.trim(strNumeroDocumento).length != 11) {
                                $("#<%=lblValidacion.ClientID %>").show();
                                $("#<%=lblValidacion.ClientID %>").html("NÚMERO DE DOCUMENTO INCORRECTO.");
                                $("#<%=txtNroDocumento.ClientID %>").css("border", "solid Red 1px");
                                $("#<%=txtNroDocumento.ClientID %>").val("");
                            }
                            else {
                                $("#<%=txtNroDocumento.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=lblValidacion.ClientID %>").hide();
                            }
                        }
                        else {

                            if ($.trim(strNumeroDocumento).length < 5) {
                                $("#<%=lblValidacion.ClientID %>").show();
                                $("#<%= lblValidacion.ClientID %>").html('NÚMERO DE DOCUMENTO INCORRECTO.');
                                $("#<%=txtNroDocumento.ClientID %>").css("border", "solid Red 1px");
                                $("#<%=txtNroDocumento.ClientID %>").val("");
                            }
                            else {
                                $("#<%=txtNroDocumento.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=lblValidacion.ClientID %>").hide();
                            }
                        }
                    }
                }
            }
            else {
                $("#<%=lblValidacion.ClientID %>").show();
                $("#<%= lblValidacion.ClientID %>").html('SELECCIONE TIPO DE DOCUMENTO.');
                $("#<%=ddlTipoDocumento.ClientID %>").css("border", "solid Red 1px");
                $("#<%=txtNroDocumento.ClientID %>").val("");
                $("#<%=ddlTipoDocumento.ClientID %>").focus();
            }

        }
        function isCantidadTipoDocumento(evt) {
           

            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;

            var codDocumento = $("#<%= ddlRepresentanteTipoDoc.ClientID %>").val();

            if (codDocumento == 0)
                return false;


            var hidDocumentoSoloNumero = $("#<%= hidDocumentoSoloNumero.ClientID %>").val();

            if (hidDocumentoSoloNumero == "1") {
                letra = true;

                if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                    letra = false;
                }
                if (charCode == 13) {
                    letra = false;
                }
            }
            else {

                letra = false;
                if (charCode > 1 && charCode < 4) {
                    letra = true;
                }
                if (charCode == 8) {
                    letra = true;
                }
                if (charCode == 13) {
                    letra = false;
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
                if (charCode == 45) {
                    letra = true;
                }

                var letras = "áéíóúÁÉÍÓÚñÑäëïöüÄËÏÖÜ";
                var tecla = String.fromCharCode(charCode);
                var n = letras.indexOf(tecla);
                if (n > -1) {
                    letra = true;
                }
            }





            return letra;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode

            var letra = true;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                letra = false;
            }
            if (charCode == 13) {
                letra = false;
            }
            return letra;
        }

        function isDecimalKey(ctrl, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var FIND = "."
            var x = ctrl.value
            var y = x.indexOf(FIND)

            if (charCode == 46) {
                if (y != -1 || x.length == 0)
                    return false;
            }
            if (charCode != 46 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        //--FIN VALIDACION NUMEROS--


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        $(document).ready(function () {
            Load();
        });

        function Load() {
            $("#<%=txtRepresentanteNroDoc.ClientID %>").bind("keypress", function (e) {
                if (e.keyCode == 13) {
                    var ddl = document.getElementById("<%= ddlRepresentanteTipoDoc.ClientID %>");
                    var codDocumento = ddl.options[ddl.selectedIndex].value;
                    if (codDocumento == 1) {
                        numDigitosDocumento(document.getElementById("<%= txtRepresentanteNroDoc.ClientID %>"));
                    }
                    else {
                        document.getElementById("<%=imgBuscar.ClientID %>").click();
                    }

                    e.preventDefault();
                }
            });


//            $("#<%=txtRepresentanteNroDoc.ClientID %>").bind("blur", function (e) {
//                document.getElementById("<%=imgBuscar.ClientID %>").click();
//                e.preventDefault();

//            });
        }

    </script>
</asp:Content>
