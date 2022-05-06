<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlBusquedaAvanzada.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlBusquedaAvanzada" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>

<style type="text/css">
    .link{ text-decoration:none; color: #000 !important; }
    .link : hover { text-decoration: underline; }
    
 
    .style1
    {
        width: 125px;
    }
    .style2
    {
        width: 110px;
    }
    
 
    .style3
    {
        width: 124px;
    }
    
 
</style>

<div>
 <asp:HiddenField ID="HFGUID" runat="server" />
    <div>        
        <table width="90%" style="margin-bottom:5px">
            <tr>
                <td>
                    <h2><asp:Label ID="lblBusquedaTitulo" runat="server" Text="Búsqueda Titular"></asp:Label></h2>
                </td>
            </tr>
        </table>
    </div>
   
    <div>
    <asp:UpdatePanel runat="server" ID="UpdGrvPaginada" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hEnter" runat="server" />
                <asp:HiddenField ID="hid_iPersonaId" runat="server" Value="0" />                
                <asp:HiddenField ID="hdn_tipo_persona" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_tipo_direccion" runat="server" Value="0" />

                 <table class="mTblPrincipal">
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label1" runat="server" Text="Tipo Persona: "></asp:Label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlPersonaTipo" runat="server" Width="105px" OnSelectedIndexChanged="ddlPersonaTipo_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                    </td>
                                    <td align="right" class="style1">
                                        <font>
                                            <asp:Label ID="lblNroDocumento" runat="server" Text="Nro Documento:" Width="116px"></asp:Label></font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNroDocumento" runat="server" Width="100px" MaxLength="20" CssClass="txtLetra"
                                                onkeypress = "return isNumeroLetra(event)" onBlur="QuitarEspacios(event);"
                                              ToolTip="Para poder realizar la búsqueda debe ingresar el Nro. Documento completo." />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="lblPriApellido" runat="server" Text="Primer Apellido:" Width="110px"></asp:Label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlPersonaNatural" Visible ="true" runat="server">
                                            <table border="0">
                                                <tr align="left">
                                                    <td>
                                                <asp:TextBox ID="txtPriApellido" runat="server" Width="100px" 
                                                    ToolTip="Para poder realizar la búsqueda debe ingresar como mínimo 2 caracteres."
                                                    CssClass="txtLetra" MaxLength="50"
                                                    onkeypress="return ValidarSujeto(event)" />
                                                    </td>
                                                  <td align="right" class="style3">
                                                    <asp:Label ID="lblSegApellido" runat="server" Text=" Segundo Apellido:" Width="120px"></asp:Label>

                                                    </td>
                                                    <td>
                                                    <asp:TextBox ID="txtSegApellido" runat="server" Width="100px" 
                                                        ToolTip="Para poder realizar la búsqueda debe ingresar como mínimo 2 caracteres."
                                                        onkeypress="return ValidarSujeto(event)"
                                                        CssClass="txtLetra" MaxLength="50" />

                                                    </td>
                                                    <td align="right">
                                                      <asp:Label ID="lblNombres" runat="server" Text=" Nombres:" Width="90px"></asp:Label>
                                                    </td>
                                                    <td>
                                                    <asp:TextBox ID="txtNombres" runat="server" Width="100px" 
                                                        ToolTip="Para poder realizar la búsqueda debe ingresar como mínimo 2 caracteres."
                                                        onkeypress="return ValidarSujeto(event)"
                                                        CssClass="txtLetra" MaxLength="50" />
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                            
                                            
                                        </asp:Panel>
                                        <asp:Panel ID="pnlPersonaJuridica" Visible = "false" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtRazonSocial" runat="server" 
                                                        ToolTip="Para poder realizar la búsqueda debe ingresar como mínimo 3 caracteres."
                                                        CssClass="txtLetra" MaxLength="50"
                                                        Width="450px" />

                                                </td>
                                            </tr>
                                        </table>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_Buscar" runat="server" Text="   Buscar" CssClass="btnSearch" OnClick="btn_Buscar_Click"
                                            UseSubmitBehavior="False" />
                                    </td>
                                    <td style="width:10px"></td>
                                    <td align="right">
                                        <asp:Button ID="btn_NewRUNE" runat="server" Text="Nuevo RUNE" OnClick="btn_NewRUNE_Click"
                                            CssClass="btnRUNE" UseSubmitBehavior="False" Width="110px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table> 

                <table width="90%">
                   
                    <tr>
                        <td>
                            <Label:Validation ID="ctrlValidacion" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                          
                        <asp:Repeater ID="rptResultPersona" runat="server" 
                        onitemcommand="rptResultPersona_ItemCommand" onitemdatabound="rptResultPersona_ItemDataBound">
                        <HeaderTemplate>
                            <table  class="mGrid" width="100%">
                                <tr>
                                    <th class="ColumnaOculta">PersonaTipoId</th>
                                    <th class="ColumnaOculta">DocumentoTipoId</th>
                                    <th class="ColumnaOculta">DescTipDoc</th>
                                    <th class="ColumnaOculta">ApellidoPaterno</th>
                                    <th class="ColumnaOculta">ApellidoMaterno</th>
                                    <th class="ColumnaOculta">Nombres</th>

                                    <th class="ColumnaOculta">NroDocumento</th>
                                    <th class="ColumnaOculta">NacionalidadId</th>
                                    <th class="ColumnaOculta">GeneroId</th>
                                    <th class="ColumnaOculta">ApellidoCasada</th>
                                    <th class="ColumnaOculta">TipoDocumento</th>

                                    <th align="center">Apellidos y Nombres</th>
                                    <th align="center">Número de Documento</th>
                                    <th align="center">Fecha Nacimiento</th>
                                    <th align="center">Nacionalidad</th>
                                    <th align="center">Seleccionar</th>
                                </tr>
                        </HeaderTemplate>
                        <AlternatingItemTemplate>
                        
                        
                             <tr id="fila" runat="server" title="Haga click aquí para seleccionar la fila.">
                                <td  class="ColumnaOculta">
                                    <asp:LinkButton runat="server" ID="lnkPersonaTipoId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("sPersonaTipoId")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkDocumentoTipoId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("sDocumentoTipoId")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkDescTipDoc"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vDescTipDoc")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkApePat"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vApellidoPaterno")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkApeMat"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vApellidoMaterno")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkvNombres"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vNombres")%>'></asp:LinkButton> 
                                </td>
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkvNroDocumento"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vNroDocumento")%>'></asp:LinkButton> 
                                </td>
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnksNacionalidadId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("sNacionalidadId")%>'></asp:LinkButton> 
                                </td>
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnksGeneroId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("sGeneroId")%>'></asp:LinkButton> 
                                </td>
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkpers_vApellidoCasada"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("pers_vApellidoCasada")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkvTipoDocumento"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vTipoDocumento")%>'></asp:LinkButton> 
                                </td>

                                <td style="background:#cccccc; width:250px;" >
                                    <asp:LinkButton runat="server" ID="lnkNombre"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vNombre")%>'></asp:LinkButton> 
                                </td>
                                <td style="background:#cccccc; width:120px; text-align:center">
                                    <asp:LinkButton runat="server" ID="lnkTipoDocumento" CssClass="link" CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vNroTipoDocumento")%>'></asp:LinkButton>
                                </td>
                                <td style="background:#cccccc; width:80px;text-align:center">
                                <asp:LinkButton runat="server" ID="lnkFecNac" CssClass="link" CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vFecNac")%>'></asp:LinkButton>                                                                                                  
                                </td>
                                <td style="background:#cccccc; width:100px;text-align:center">
                                <asp:LinkButton runat="server" ID="lnkNacionalidad" CssClass="link" CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("pers_vNacionalidad")%>'></asp:LinkButton>                                                                                                                                  
                                </td>
                                <td align="center" style="background:#cccccc; width:80px;">
                                <asp:ImageButton ID="btnSeleccionar"  runat="server" ImageUrl="~/Images/img_sel_check.png"  CommandName="select" 
                                            CommandArgument='<%# Eval("iPersonaId") %>' />
                                </td>

                             </tr>
                           
                      </AlternatingItemTemplate>
                        <ItemTemplate>
                              
                                
                            <tr id="fila" runat="server" title="Haga click aquí para seleccionar la fila.">

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkPersonaTipoId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("sPersonaTipoId")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkDocumentoTipoId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("sDocumentoTipoId")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkDescTipDoc"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vDescTipDoc")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkApePat"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vApellidoPaterno")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkApeMat"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vApellidoMaterno")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkvNombres"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vNombres")%>'></asp:LinkButton> 
                                </td>
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkvNroDocumento"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vNroDocumento")%>'></asp:LinkButton> 
                                </td>
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnksNacionalidadId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("sNacionalidadId")%>'></asp:LinkButton> 
                                </td>
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnksGeneroId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("sGeneroId")%>'></asp:LinkButton> 
                                </td>
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkpers_vApellidoCasada"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("pers_vApellidoCasada")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkvTipoDocumento"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vTipoDocumento")%>'></asp:LinkButton> 
                                </td>

                                <td style="width:250px;">
                                    <asp:LinkButton runat="server" ID="lnkNombre" CssClass="link" CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vNombre")%>'></asp:LinkButton>                                          
                                </td>
                                <td style="width:120px; text-align:center">
                                <asp:LinkButton runat="server" ID="lnkTipoDocumento" CssClass="link" CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vNroTipoDocumento")%>'></asp:LinkButton>                                                                                                      
                                </td>
                                <td style="width:80px;text-align:center">                                
                                <asp:LinkButton runat="server" ID="lnkFecNac" CssClass="link" CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("vFecNac")%>'></asp:LinkButton> 
                                </td>
                                <td style="width:100px;text-align:center">                                
                                <asp:LinkButton runat="server" ID="lnkNacionalidad" CssClass="link" CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' Text='<%#Eval("pers_vNacionalidad")%>'></asp:LinkButton>                                                                                                                                 
                                </td>
                                 <td align="center" style="width:80px;">
                                <asp:ImageButton ID="btnSeleccionar"  runat="server" ImageUrl="~/Images/img_sel_check.png"  
                                        CommandName="select" CommandArgument='<%# Eval("iPersonaId") %>' />
                                </td>
                            </tr>
                            
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                          
                        </td>
                    </tr>
                    <tr>
                        <td>
                              <asp:Repeater ID="rptResultEmpresa" runat="server" 
                        onitemcommand="rptResultEmpresa_ItemCommand" onitemdatabound="rptResultEmpresa_ItemDataBound">
                        <HeaderTemplate>
                            <table  class="mGrid" width="100%">
                                <tr>
                                    <th class="ColumnaOculta">TipoEmpresaId</th>
                                    <th class="ColumnaOculta">TipoDocumentoId</th>
                                    <th class="ColumnaOculta">DescTipDoc</th>
                                    <th class="ColumnaOculta">ActividadComercial</th>
                                    <th class="ColumnaOculta">Estado</th>


                                    <th align="center">Razón Social</th>
                                    <th align="center">Número de Documento</th>
                                    <th align="center">Teléfono</th>
                                    <th align="center">Correo</th>
                                    <th align="center">Tipo</th>

                                    <th>Seleccionar</th>
                                </tr>
                        </HeaderTemplate>
                        <AlternatingItemTemplate>
                        
                        
                             <tr id="fila" runat="server" title="Haga click aquí para seleccionar la fila.">
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkTipoEmpresaId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_sTipoEmpresaId")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkTipoDocumentoId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_sTipoDocumentoId")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkDescTipDoc"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("vDescTipDoc")%>'></asp:LinkButton> 
                                </td>
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkActividadComercial"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vActividadComercial")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkEstado"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_cEstado")%>'></asp:LinkButton> 
                                </td>

                                <td style="background:#cccccc; width:350px;" >
                                    <asp:LinkButton runat="server" ID="lnkRazonSocial"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vRazonSocial")%>'></asp:LinkButton> 
                                </td>

                                <td style="background:#cccccc; width:150px;text-align:center">
                                    <asp:LinkButton runat="server" ID="lnkNumeroDocumento"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vNumeroDocumento")%>'></asp:LinkButton> 
                                </td>

                                <td style="background:#cccccc; width:100px;text-align:center">
                                <asp:LinkButton runat="server" ID="lnkTelefono" CssClass="link" CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vTelefono")%>'></asp:LinkButton>                                                                                                                                  
                                </td>

                                <td style="background:#cccccc; width:250px;">
                                <asp:LinkButton runat="server" ID="lnkCorreo" CssClass="link" CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vCorreo")%>'></asp:LinkButton>                                                                                                                                  
                                </td>

                                <td style="background:#cccccc; width:100px;text-align:center">
                                <asp:LinkButton runat="server" ID="lnkTipoEmpresa" CssClass="link" CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vTipoEmpresa")%>'></asp:LinkButton>                                                                                                                                  
                                </td>

                                <td align="center" style="background:#cccccc; width:80px;">
                                <asp:ImageButton ID="btnSeleccionar"  runat="server" ImageUrl="~/Images/img_sel_check.png"  CommandName="select" 
                                            CommandArgument='<%# Eval("empr_iEmpresaId") %>' />
                                </td>

                             </tr>
                           
                      </AlternatingItemTemplate>
                        <ItemTemplate>
                              
                                
                             <tr id="fila" runat="server" title="Haga click aquí para seleccionar la fila.">
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkTipoEmpresaId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_sTipoEmpresaId")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkTipoDocumentoId"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_sTipoDocumentoId")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkDescTipDoc"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("vDescTipDoc")%>'></asp:LinkButton> 
                                </td>
                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkActividadComercial"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vActividadComercial")%>'></asp:LinkButton> 
                                </td>

                                <td class="ColumnaOculta" >
                                    <asp:LinkButton runat="server" ID="lnkEstado"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_cEstado")%>'></asp:LinkButton> 
                                </td>

                                <td style="width:350px;" >
                                    <asp:LinkButton runat="server" ID="lnkRazonSocial"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vRazonSocial")%>'></asp:LinkButton> 
                                </td>

                                <td style="width:150px;text-align:center">
                                    <asp:LinkButton runat="server" ID="lnkNumeroDocumento"  CssClass="link"  CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vNumeroDocumento")%>'></asp:LinkButton> 
                                </td>

                                <td style="width:100px;text-align:center">
                                <asp:LinkButton runat="server" ID="lnkTelefono" CssClass="link" CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vTelefono")%>'></asp:LinkButton>                                                                                                                                  
                                </td>

                                <td style="width:250px;">
                                <asp:LinkButton runat="server" ID="lnkCorreo" CssClass="link" CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vCorreo")%>'></asp:LinkButton>                                                                                                                                  
                                </td>

                                <td style="width:100px;text-align:center">
                                <asp:LinkButton runat="server" ID="lnkTipoEmpresa" CssClass="link" CommandName="select" CommandArgument='<%# Eval("empr_iEmpresaId") %>' Text='<%#Eval("empr_vTipoEmpresa")%>'></asp:LinkButton>                                                                                                                                  
                                </td>

                                <td align="center" style="width:80px;">
                                <asp:ImageButton ID="btnSeleccionar"  runat="server" ImageUrl="~/Images/img_sel_check.png"  CommandName="select" 
                                            CommandArgument='<%# Eval("empr_iEmpresaId") %>' />
                                </td>

                             </tr>
                            
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

                        </td>
                    </tr>
                    <tr>
                        <td>
                         <div>
                    <table  id="tabpaginado" runat="server" style="width: 100%; text-align:center; background-color: #CCCCCC;" >
                        <tr>
                            <td style="width:40px">
                                   <asp:imagebutton id="btnPrimero" Runat="server" ToolTip="Haga clic aquí para seleccionar la primera página."
                                        ImageUrl="../../Images/NavFirstPageDisabled.gif" 
                                        onclick="lbFirst_Click" Width="22px" />
                            </td>
                            <td style="width:40px; text-align:left">
                                    <asp:imagebutton id="btnAnterior" Runat="server" Enabled="false" Tooltip="Haga clic aquí para seleccionar la página anterior."
                                        ImageUrl="../../Images/NavPreviousPageDisabled.gif" onclick="lbPrevious_Click" />

                            </td>
                            <td>
                                <asp:DataList ID="rptPaging" runat="server"
                                    OnItemCommand="rptPaging_ItemCommand"
                                    OnItemDataBound="rptPaging_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaging" runat="server"  ForeColor="Black" Tooltip="Haga clic aquí para seleccionar esta página."
                                            style="margin-left:2px;margin-right:2px"
                                            CommandArgument='<%# Eval("PageIndex") %>' CommandName="newPage"
                                            Text='<%# Eval("PageText") %> ' width="30px" ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="width:40px; text-align:left">
                                    <asp:imagebutton id="btnSiguiente" Runat="server" Enabled="false" Tooltip="Haga clic aquí para seleccionar la siguiente página."
                                            ImageUrl="../../Images/NavNextPageDisabled.gif" onclick="lbNext_Click" />                            
                            </td>
                            <td style="width:40px; text-align:left" >
                                    <asp:imagebutton id="btnUltimo" Runat="server" Enabled="false" Tooltip="Haga clic aquí para seleccionar la última página."
                                            ImageUrl="../../Images/NavLastPageDisabled.gif" onclick="lbLast_Click" />                               
                            </td>
                            <td style="width:230px">
                            </td>
                            <td>
                                <asp:Label ID="lblpage" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>

                        </div>
                        </td>
                    </tr>
                </table>

             </ContentTemplate>
    </asp:UpdatePanel>

    </div>
</div>

<script language="javascript" type="text/javascript">

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
    $(document).ready(function () {
        Load();
    });

    function Load() {

        $(function () {


            $("#<%=txtNroDocumento.ClientID %>").bind("keypress", function (e) {

                if (e.keyCode == 13) {
                    document.getElementById("<%=btn_Buscar.ClientID %>").click();
                    e.preventDefault();
                }
                else {

                    if (($.trim($("#<%=txtNroDocumento.ClientID %>").val()).length == 0) && ($.trim($("#<%=txtPriApellido.ClientID %>").val()).length == 0) && ($.trim($("#<%=txtSegApellido.ClientID %>").val()).length == 0)) {
                        $('#<%= ctrlValidacion.FindControl("trMensaje").ClientID %>').hide();
                    }
                }
            });

            $("#<%=txtRazonSocial.ClientID %>").bind("keypress", function (e) {

                if (e.keyCode == 13) {
                    document.getElementById("<%=btn_Buscar.ClientID %>").click();
                    e.preventDefault();
                } else {
                    if (($.trim($("#<%=txtNroDocumento.ClientID %>").val()).length == 0) && ($.trim($("#<%=txtPriApellido.ClientID %>").val()).length == 0) && ($.trim($("#<%=txtSegApellido.ClientID %>").val()).length == 0)) {
                        $('#<%= ctrlValidacion.FindControl("trMensaje").ClientID %>').hide();
                    }
                }
            });

            $("#<%=txtPriApellido.ClientID %>").bind("keypress", function (e) {

                if (e.keyCode == 13) {
                    document.getElementById("<%=btn_Buscar.ClientID %>").click();
                    e.preventDefault();
                } else {
                    if (($.trim($("#<%=txtNroDocumento.ClientID %>").val()).length == 0) && ($.trim($("#<%=txtPriApellido.ClientID %>").val()).length == 0) && ($.trim($("#<%=txtSegApellido.ClientID %>").val()).length == 0)) {
                        $('#<%= ctrlValidacion.FindControl("trMensaje").ClientID %>').hide();
                    }
                }
            });
            $("#<%=txtSegApellido.ClientID %>").bind("keypress", function (e) {

                if (e.keyCode == 13) {
                    document.getElementById("<%=btn_Buscar.ClientID %>").click();
                    e.preventDefault();
                } else {
                    if (($.trim($("#<%=txtNroDocumento.ClientID %>").val()).length == 0) && ($.trim($("#<%=txtPriApellido.ClientID %>").val()).length == 0) && ($.trim($("#<%=txtSegApellido.ClientID %>").val()).length == 0)) {
                        $('#<%= ctrlValidacion.FindControl("trMensaje").ClientID %>').hide();
                    }
                }
            });
            $("#<%=txtNombres.ClientID %>").bind("keypress", function (e) {

                if (e.keyCode == 13) {
                    document.getElementById("<%=btn_Buscar.ClientID %>").click();
                    e.preventDefault();
                } else {
                    if (($.trim($("#<%=txtNroDocumento.ClientID %>").val()).length == 0) && ($.trim($("#<%=txtPriApellido.ClientID %>").val()).length == 0) && ($.trim($("#<%=txtSegApellido.ClientID %>").val()).length == 0)) {
                        $('#<%= ctrlValidacion.FindControl("trMensaje").ClientID %>').hide();
                    }
                }
            });

        });

    }

    function conMayusculas(field) {
        field.value = field.value.trim();
        field.value = field.value.toUpperCase()
    }

    function ValidarSujeto(evt) {
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
        if (charCode == 39) {
            letra = true;
        }
        if (charCode == 231) {
            letra = true;
        }
        if (charCode == 199) {
            letra = true;
        }
        var letras = "áéíóúüñÑäëïöüÁÉÍÓÚÄËÏÖÜ";
        var tecla = String.fromCharCode(charCode);
        var n = letras.indexOf(tecla);
        if (n > -1) {
            letra = true;
        }

        return letra;
    }

    function isNumeroLetra(evt) {

        var letra = false;
        var charCode = (evt.which) ? evt.which : evt.keyCode


        if (charCode > 1 && charCode < 4) {
            letra = true;
        }
        if (charCode == 8) {
            letra = true;
        }
        if (charCode == 32) {
            letra = true;
        }
        if (charCode == 45) {
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

        var letras = "áéíóúñÑ'";
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
    function QuitarEspacios(event) {

        var documento = document.getElementById("<%=txtNroDocumento.ClientID %>").value;
        var documentosinespacios = documento.replace(/ /g, "");
        document.getElementById("<%=txtNroDocumento.ClientID %>").value = documentosinespacios;
    }

</script>