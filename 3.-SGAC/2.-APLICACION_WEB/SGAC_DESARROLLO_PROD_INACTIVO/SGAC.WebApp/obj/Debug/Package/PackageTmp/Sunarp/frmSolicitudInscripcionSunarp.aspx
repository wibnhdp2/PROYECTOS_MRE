<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmSolicitudInscripcionSunarp.aspx.cs" Inherits="SGAC.WebApp.Sunarp.frmSolicitudInscripcionSunarp" %>

<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>    
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="https://dsp.reniec.gob.pe/refirma_invoker/resources/js/clientclickonce.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2><asp:Label ID="lblTitutloTipoCambio" runat="server" Text="Solicitud de Inscripción - SUNARP"></asp:Label></h2>
                </td>
            </tr>
        </table>
        <div id="tabs">
            <ul>
                <li><a href="#tab-1">
                    <asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                <li><a href="#tab-2">
                    <asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                <li><a href="#tab-3">
                    <asp:Label ID="lblTabReingreso" runat="server" Text="Reingreso"></asp:Label></a></li>
            </ul>
            <div id="tab-1">
                <fieldset class="Fieldset">
                    <legend>Datos de la Escritura:</legend>
                    <table>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="lblConsulado" runat="server" Text="Oficina Consular:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <uc1:ctrlOficinaConsular runat="server" id="ctrlOficinaConsular" Width="350px" />
                                <span style="color:red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="lblPeriodo" runat="server" Text="Nro. de Escritura:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtEscritura" runat="server" Width="345px" style="text-transform: uppercase;" onkeypress="return soloNumeros(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="lblFecha" runat="server" Text="Fecha de Extensíón:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtFechaInicio" runat="server" type="date" Width="165px"></asp:TextBox>
                                - 
                                <asp:TextBox ID="txtFechaFin" runat="server" type="date" Width="165px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label13" runat="server" Text="Rectificacion:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:CheckBox ID="chkReingreso" runat="server" Checked="false" />
                            </td>
                        </tr>
                    </table>
                </fieldset>

                 <fieldset class="Fieldset">
                    <legend><a href="#" style="color:brown; font-weight:bold; font-size:15px;" id="participantes">+</a> Datos del Participante:</legend>
                    <table id="TablaParticipantes">
                        <tr>
                            <td align="left" colspan="1" style="width: 150px;">
                                <asp:Label ID="lblTipParticipante" runat="server" Text="Tipo Participante:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList ID="ddlTipParticipante" runat="server" Width="225px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="1" style="width: 150px;">
                                <asp:Label ID="lblTipDoc" runat="server" Text="Tipo Documento:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList ID="ddlTipDoc" runat="server" Width="225px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="1" style="width: 150px;">
                                <asp:Label ID="lblNumDoc" runat="server" Text="Número Documento:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtNumDoc" runat="server" Width="220px" style="text-transform: uppercase;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="1" style="width: 150px;">
                                <asp:Label ID="lblPrimerApellido" runat="server" Text="Primer Apellido:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtPrimerApellido" runat="server" Width="220px" style="text-transform: uppercase;" onkeypress="return letras(event)"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td align="left" colspan="1" style="width: 150px;">
                                <asp:Label ID="lblSegundoApellido" runat="server" Text="Segundo Apellido:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtSegundoApellido" runat="server" Width="220px" style="text-transform: uppercase;" onkeypress="return letras(event)"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td align="left" colspan="1" style="width: 150px;">
                                <asp:Label ID="lblNombres" runat="server" Text="Nombres:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtNombres" runat="server" Width="220px" style="text-transform: uppercase;" onkeypress="return letras(event)"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:Button ID="btnBuscar" runat="server" Text="    Buscar" OnClientClick="return ValidarDatosBusqueda();"
                        CssClass="btnSearch" OnClick="btnBuscar_Click" />
                <asp:Button ID="btnLimpiar" runat="server" Text="    Limpiar"
                        CssClass="btnLimpiar" OnClick="btnLimpiar_Click" />
                
                <hr />
                <table>
                    <tr>
                        <td colspan="2">
                            <div style="width: 12px; height: 12px; background: #A4F7EB; float:left;  border: 1px solid #555; margin-right:5px;"></div>
                            <div style="width:350px;">
                                <span style="font-weight:bold; font-size:12px;"> Solicitudes con Parte Firmado Digitalmente</span>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grvSolicitudes"
                    runat="server"
                    CssClass="mGrid"
                    AlternatingRowStyle-CssClass="alt"
                    SelectedRowStyle-CssClass="slt"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    OnRowDataBound="grvSolicitudes_RowDataBound">

                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

                    <Columns>
                        <asp:BoundField DataField="SOIN_ISOLICITUDINSCRIPCIONID" HeaderText="SolicitudID"
                            HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                            <HeaderStyle CssClass="ColumnaOculta" />
                            <ItemStyle CssClass="ColumnaOculta" />
                        </asp:BoundField>
                        <asp:BoundField DataField="oficina" HeaderText="Oficina">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOIN_VCUO" HeaderText="C.U.O">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOLICITANTE" HeaderText="Solicitante">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TIPO_ACTO" HeaderText="Tipo Acto">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="acno_dFechaExtension" HeaderText="Fecha">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="esta_vDescripcionCorta" HeaderText="Estado">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="nroEscritura" HeaderText="Nro. Escritura">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOIN_VNUMHOJAPRESENTACION" HeaderText="Hoja Presentación">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOIN_VANIOTITULOSUNARP" HeaderText="Año Titulo">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOIN_VNUMTITULOSUNARP" HeaderText="Nro. Título">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOIN_DFECHAINSCRIPCIONSUNARP" HeaderText="Fecha de Inscripción">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOIN_DFECHAPRESENTACIONSUNARP" HeaderText="Fecha de Presentación">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOIN_DFECHAVENCIMIENTOSUNARP" HeaderText="Fecha de Vencimiento">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOIN_vPARTEFIRMADO" HeaderText="ParteFirmado"
                            HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                            <HeaderStyle CssClass="ColumnaOculta" />
                            <ItemStyle CssClass="ColumnaOculta" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Reingreso" HeaderText="Reingreso"
                            HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                            <HeaderStyle CssClass="ColumnaOculta" />
                            <ItemStyle CssClass="ColumnaOculta" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnVer" ToolTip="Ver"  runat="server" ImageUrl="../Images/i.p.buscar.gif" OnClick="Ver" iActuacion='<%# Eval("acno_iActuacionId")%>' iSolicitud='<%# Eval("SOIN_ISOLICITUDINSCRIPCIONID")%>' iActoNotarial='<%# Eval("acno_iActoNotarialId")%>'  estado='<%# Eval("esta_vDescripcionCorta")%>' correlativo='<%# Eval("SOIN_TICORRELATIVO")%>'/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnSolicitud" ToolTip="Solicitud"  runat="server" ImageUrl="../Images/img_16_add.png" OnClick="RegistrarSolicitud"  iActuacion='<%# Eval("acno_iActuacionId")%>' iSolicitud='<%# Eval("SOIN_ISOLICITUDINSCRIPCIONID")%>' iActoNotarial='<%# Eval("acno_iActoNotarialId")%>'   />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnReingreso" ToolTip="Regingreso"  runat="server" ImageUrl="../Images/img_16_tramite_rechazar.png" OnClick="Reingreso" iActuacion='<%# Eval("acno_iActuacionId")%>' iSolicitud='<%# Eval("SOIN_ISOLICITUDINSCRIPCIONID")%>' iActoNotarial='<%# Eval("acno_iActoNotarialId")%>'  />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnFirmar" ToolTip="Firmar"  runat="server" ImageUrl="../Images/img_grid_modify.png" OnClick="Firmar" iSolicitud='<%# Eval("SOIN_ISOLICITUDINSCRIPCIONID")%>' correlativo='<%# Eval("SOIN_TICORRELATIVO")%>' cuo='<%# Eval("SOIN_VCUO")%>'/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEnviar" ToolTip="Enviar"  runat="server" ImageUrl="../Images/img_16_order_attend.png" OnClick="Enviar" iSolicitud='<%# Eval("SOIN_ISOLICITUDINSCRIPCIONID")%>'/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                    </Columns>

                    <SelectedRowStyle CssClass="slt" />

                </asp:GridView>

            </div>
            <div id="tab-2">
                <asp:HiddenField ID="hiActuacion" runat="server" Value="0" />
                <asp:HiddenField ID="hISolicitud" runat="server" Value="0" />
               <asp:HiddenField ID="hiActoNotarial" runat="server" Value="0" />
                
                <table>
                    <tr>
                        <td align="left" colspan="3" style="width:280px;">
                            <asp:Button ID="btnGrabar" runat="server" Text="    Grabar" CssClass="btnSave" OnClick="btnGrabar_Click" OnClientClick="return verificarDatos();" />
                            <asp:Button ID="btnEliminar" runat="server" Text="   Anular" CssClass="btnDelete" OnClientClick="return seguro();" OnClick="btnEliminar_Click" />
                        </td>
                        <td align="right" colspan="3">
                            <fieldset class="Fieldset">
                                <legend>Código Único de Operación:</legend>
                                <asp:Label ID="lblCUO" runat="server" Font-Bold="True"></asp:Label>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <fieldset class="Fieldset">
                    <legend>Datos de la Solicitud:</legend>
                    <table>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label1" runat="server" Text="Acto Registral:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList ID="ddlActoRegistral" runat="server" Width="450px"></asp:DropDownList>
                                <span style="color:red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label3" runat="server" Text="Zona Registral:"></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:DropDownList ID="ddlZonaRegistral" runat="server" Width="166px" AutoPostBack="True" OnSelectedIndexChanged="ddlZonaRegistral_SelectedIndexChanged" />
                                <span style="color:red;">*</span>
                            </td>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label2" runat="server" Text="Oficina Registral:"></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:DropDownList ID="ddlOfiRegistral" runat="server" Width="166px" />
                                <span style="color:red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label4" runat="server" Text="No Inscribir:"></asp:Label>
                            </td>
                            <td align="left" colspan="5">
                                 <asp:TextBox ID="txtNoInscribir" runat="server" Width="445px" TextMode="MultiLine" Rows="5" style="text-transform: uppercase;"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset class="Fieldset">
                    <legend>Datos del Presentante:</legend>
                    <table>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label5" runat="server" Text="Funcionario:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList ID="ddlFuncionario" runat="server" Width="450px" />
                                <span style="color:red;">*</span>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset class="Fieldset">
                    <legend>Datos de la Notificación:</legend>
                    <table>
                        <tr>
                            <td align="left" colspan="1" style="width: 200px;">
                                <asp:Label ID="Label6" runat="server" Text="Correo Electrónico del Solicitante:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtCorreoSolicitante" type="email" runat="server" Width="350px" style="text-transform: uppercase;"></asp:TextBox>
                                <span style="color:red;">*</span>
                            </td>
                        </tr>
                         <tr>
                            <td align="left" colspan="1" style="width: 200px;">
                                <asp:Label ID="Label7" runat="server" Text="Correo Electrónico del Presentante:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtCorreoPresentante" type="email" runat="server" Width="350px" style="text-transform: uppercase;"></asp:TextBox>
                                <span style="color:red;">*</span>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <hr />
               <%-- <asp:Button ID="btnAgregarParticipante" runat="server" Text="     Agregar Participante"
                Width="170px" CssClass="btnNew" />--%>

                <fieldset class="Fieldset">
                    <legend>Participantes:</legend>
                    <asp:GridView ID="grvParticipantes"
                    runat="server"
                    CssClass="mGrid"
                    AlternatingRowStyle-CssClass="alt"
                    SelectedRowStyle-CssClass="slt"
                    AutoGenerateColumns="False"
                    GridLines="None">

                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

                    <Columns>
                        <asp:BoundField DataField="TIPO_PARTICIPANTE" HeaderText="Tipo Participante">
                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TipoDoc" HeaderText="Tipo Documento">
                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="peid_vDocumentoNumero" HeaderText="Nro.Documento">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PARTICIPANTE" HeaderText="Participante">
                            <ItemStyle HorizontalAlign="Center" Width="350px" />
                        </asp:BoundField>
                    </Columns>

                    <SelectedRowStyle CssClass="slt" />

                </asp:GridView>
                </fieldset>
            </div>
            <div id="tab-3">
                <asp:HiddenField ID="hiSolicitudReingreso" runat="server" Value="0" />
                <asp:HiddenField ID="hiActuacionReingreso" runat="server" Value="0" />
               <asp:HiddenField ID="hiActoNotarialReingreso" runat="server" Value="0" />
                <table>
                    <tr>
                        <td align="left" colspan="3" style="width:350px;">
                            <asp:Button ID="btnGrabarReingreso" runat="server" Text="    Grabar" CssClass="btnSave" OnClick="btnGrabarReingreso_Click" OnClientClick="return verificarDatosReingreso();" />
                            <asp:Button ID="btnEliminarReingreso" runat="server" Text="   Anular" CssClass="btnDelete" OnClientClick="return seguro();" OnClick="btnEliminarReingreso_Click" />
                            <asp:Button ID="btnLimpiarBusquedaSolAnterior" runat="server" Text="  Limpiar"
                                    CssClass="btnLimpiar" OnClick="btnLimpiarBusquedaSolAnterior_Click"/>
                        </td>
                        <td align="right" colspan="3">
                            <fieldset class="Fieldset">
                                <legend>Código Único de Operación:</legend>
                                <asp:Label ID="lblCUOReingreso" runat="server" Font-Bold="True"></asp:Label>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <fieldset class="Fieldset">
                    <legend>Solicitud Anterior:</legend>
                    <table>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label14" runat="server" Text="Nro. CUO"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtNroCUOBusqueda" runat="server" Width="345px" style="text-transform: uppercase;" onkeypress="return AlfaNumerico(event)"></asp:TextBox>
                                <span style="color:red;">*</span>
                            </td>
                            <td>
                                <asp:Button ID="btnBuscarSolicitudAnterior" runat="server" Text="    Buscar" 
                                    CssClass="btnSearch" OnClick="btnBuscarSolicitudAnterior_Click"/>
                            </td>
                        </tr>                        
                    </table>
                </fieldset>
                <fieldset class="Fieldset">
                    <legend>Datos de la Solicitud:</legend>
                    <table>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label8" runat="server" Text="Tipo de Reingreso:"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList ID="ddlTipReingreso" runat="server" Width="450px"></asp:DropDownList>
                                <span style="color:red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label9" runat="server" Text="Zona Registral:"></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:DropDownList ID="ddlZonaRegistralReingreso" runat="server" Width="166px" AutoPostBack="True" OnSelectedIndexChanged="ddlZonaRegistralReingreso_SelectedIndexChanged" />
                                <span style="color:red;">*</span>
                            </td>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label10" runat="server" Text="Oficina Registral:"></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:DropDownList ID="ddlOfiRegistralReingreso" runat="server" Width="166px" />
                                <span style="color:red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label11" runat="server" Text="Año de Título:"></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:TextBox ID="txtAnioTituloReingreso" runat="server" Width="160px" style="text-transform: uppercase;" onkeypress="return soloNumeros(event)"></asp:TextBox>
                                <span style="color:red;">*</span>
                            </td>
                            <td align="left" colspan="1" style="width: 100px;">
                                <asp:Label ID="Label12" runat="server" Text="Número de Título:"></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:TextBox ID="txtNumTituloReingreso" runat="server" Width="160px" style="text-transform: uppercase;" onkeypress="return soloNumeros(event)"></asp:TextBox>
                                <span style="color:red;">*</span>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
    </div>
    <div id="modalMensaje" class="modal">
            <div class="modal-windowsmall">
                <div class="modal-titulo">
                    <asp:ImageButton ID="ImageButton2" CssClass="close" ImageUrl="~/Images/img_cerrar.gif"
                        OnClientClick="cerrarPopupMensaje(); return false" runat="server" />
                    <span>¿Esta seguro que desea firmar el Parte?</span>
                </div>
                <div class="modal-cuerpo">
                    <asp:Label ID="lblMensaje" Font-Bold="true" runat="server" Text=""></asp:Label>
                </div>
                <div class="modal-pie" style="padding-top:10px;">
                    <input id="btnFirmar" type="button" value="Firmar" onclick="initInvoker('W');" />
                </div>
            </div>
        </div>
    <script>
        function verificarDatos()
        {
            var txtActaRegistral = $('#<%= ddlActoRegistral.ClientID %>').prop('selectedIndex');
            if (txtActaRegistral == 0) {
                alert('Ingrese el campo acta registral');
                return false;
            }
            var txtZonaRegistral = $('#<%= ddlZonaRegistral.ClientID %>').prop('selectedIndex');
            if (txtZonaRegistral == 0) {
                alert('Ingrese el campo Zona registral');
                return false;
            }
            var txtOficinaRegistral = $('#<%= ddlOfiRegistral.ClientID %>').prop('selectedIndex');
                if (txtOficinaRegistral == 0) {
                    alert('Ingrese el campo oficina registral');
                    return false;
                }
                var txtFuncionario = $('#<%= ddlFuncionario.ClientID %>').prop('selectedIndex');
                if (txtFuncionario == 0) {
                    alert('Ingrese el campo funcionario');
                    return false;
                }
                var txtCorreoSolicitante = $('#<%= txtCorreoSolicitante.ClientID %>').val();
                if (txtCorreoSolicitante.length == 0) {
                    alert('Ingrese el correo del solicitante');
                    return false;
                }
            var txtCorreoPresentante = $('#<%= txtCorreoPresentante.ClientID %>').val();
            if (txtCorreoPresentante.length == 0) {
                alert('Ingrese el correo del presentante');
                return false;
            }
            return true;
        }
        function PopupMensaje() {
            document.getElementById('modalMensaje').style.display = 'block';
        }
        function cerrarPopupMensaje() {

            document.getElementById('modalMensaje').style.display = 'none';
        }
    </script>

    <script language="javascript" type="text/javascript">

        $(function () {
            $('#tabs').tabs();
            $('#TablaParticipantes').show();
            $('#participantes').html("-");
            $('#tabs').disableTab(1, false);
            $('#tabs').disableTab(2, false);
        });

        $('#participantes').click(function () {
            if ($('#TablaParticipantes').is(':visible')) {
                $('#TablaParticipantes').hide();
                $('#participantes').html("+");
            }
            else {
                $('#TablaParticipantes').show();
                $('#participantes').html("-");
            }
        });
        function RegistrarSolicitud(element) {
            $('#tabs').enableTab(1, true);
            var valorActuacion = element.getAttribute("iActuacion");
            document.getElementById("<%=hiActuacion.ClientID %>").value = valorActuacion;

                return false;
            }

            function seguro() {
                var r = confirm("¿Esta seguro que desea eliminar la solicitud?");
                if (r == true) {
                    return true;
                } else {
                    return false;
                }
            }
        function isNumero(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode == 8) {
                letra = true;
            }

            if (charCode == 46) {
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
        function soloNumeros(e) {
            tecla = (document.all) ? e.keyCode : e.which;

            //Tecla de retroceso para borrar, siempre la permite
            if (tecla == 8) {
                return true;
            }
            // Patron de entrada, en este caso solo acepta numeros
            patron = /[0-9]/;
            tecla_final = String.fromCharCode(tecla);
            return patron.test(tecla_final);
        }
        function AlfaNumerico(e) {
            tecla = (document.all) ? e.keyCode : e.which;

            //Tecla de retroceso para borrar, siempre la permite
            if (tecla == 8) {
                return true;
            }
            // tecla ñ
            if (tecla == 241) {
                return true;
            }
            // tecla Ñ
            if (tecla == 209) {
                return true;
            }
            // tecla .
            if (tecla == 46) {
                return true;
            }
            // tecla -
            if (tecla == 45) {
                return true;
            }
            //Tecla de espacio, siempre la permite
            if (tecla == 32) {
                return true;
            }
            // Patron de entrada, en este caso solo acepta numeros y letras
            patron = /[A-Za-z0-9]/;
            tecla_final = String.fromCharCode(tecla);
            return patron.test(tecla_final);
        }
        function letras(e) {
            tecla = (document.all) ? e.keyCode : e.which;

            //Tecla de retroceso para borrar, siempre la permite
            if (tecla == 8) {
                return true;
            }
            // tecla ñ
            if (tecla == 241) {
                return true;
            }
            // tecla Ñ
            if (tecla == 209) {
                return true;
            }
            // tecla .
            if (tecla == 46) {
                return true;
            }
            // tecla -
            if (tecla == 45) {
                return true;
            }
            // tecla apostrofe
            if (tecla == 39) {
                return true;
            }
            //Tecla de espacio, siempre la permite
            if (tecla == 32) {
                return true;
            }
            // Patron de entrada, en este caso solo acepta letras
            patron = /[A-Za-z áéíóú ÁÉÍÓÚ Üü]/;
            tecla_final = String.fromCharCode(tecla);
            return patron.test(tecla_final);
        }
        function ValidarDatosBusqueda() {
            var txtOficina = $('#<%= ctrlOficinaConsular.ClientID %>').prop('selectedIndex');

            if (txtOficina == 0) {
                 alert('Ingrese la oficina consular');
                 return false;
            }

            var txtNumeroEscritura = $('#<%= txtEscritura.ClientID %>').val();
            var FechaInicio = $('#<%= txtFechaInicio.ClientID %>').val();
            var FechaFin = $('#<%= txtFechaFin.ClientID %>').val();


            if (FechaInicio.length >0 || FechaFin.length > 0) {
                if (FechaInicio == "" || FechaFin == "") {
                    alert('Ingrese la fecha de inicio y fin');
                    return false;
                }
            }

            var txtTipoParticipante = $('#<%= ddlTipParticipante.ClientID %>').prop('selectedIndex');
            var txtTipDoc = $('#<%= ddlTipDoc.ClientID %>').prop('selectedIndex');
            var txtNumDoc = $('#<%= txtNumDoc.ClientID %>').val();

            if (txtTipoParticipante > 0) {

                if (txtTipDoc == 0 && txtNumDoc.length == 0 && nombre.length == 0 && apepat.length == 0 && apemat.length == 0) {
                    alert('Ingrese algun dato mas a la consulta de participantes');
                    return false;
                }
            }

            if (txtTipDoc > 0) {
                if (txtNumDoc.length == 0) {
                    alert('Ingrese el número de documento del participante');
                    return false;
                }
            }

            var apepat = $('#<%= txtPrimerApellido.ClientID %>').val();
            var nombre = $('#<%= txtNombres.ClientID %>').val();
            var apemat = $('#<%= txtSegundoApellido.ClientID %>').val();

            if (nombre.length > 0 || apepat.length > 0 || apemat.length > 0) {
                if (nombre == "") {
                    alert('Ingrese el Nombre');
                    return false;
                }
                if (apepat == "") {
                    alert('Ingrese segundo apellido');
                    return false;
                }
                if (nombre.length < 3) {
                    alert( 'Ingrese mínimo 3 caracteres en el campo nombre');
                    return false;
                }
                if (apepat.length < 3) {
                    alert('Ingrese mínimo 3 caracteres en el campo primer apellido');
                    return false;
                }

                if (apemat.length < 3 && apemat.length > 0) {
                    alert('Ingrese mínimo 3 caracteres en el campo segundo apellido');
                    return false;
                }
            }

            
            if (nombre.length == 0 && apepat.length == 0 && apemat.length == 0 && txtTipDoc == 0 && txtNumDoc.length == 0 && txtNumeroEscritura.length == 0 && FechaInicio.length == 0 && FechaFin.length == 0) {
                alert('Ingrese algun filtro más de búsqueda');
                return false;
            }
             return true;
         }
    </script>
    <script type="text/javascript">	
        //<![CDATA[
        var documentName_ = null;
        //
        window.addEventListener('getArguments', function (e) {
            type = e.detail;
            if (type === 'W') {
                ObtieneArgumentosParaFirmaDesdeLaWeb(); // Llama a getArguments al terminar.
            } else if (type === 'L') {
                ObtieneArgumentosParaFirmaDesdeArchivoLocal(); // Llama a getArguments al terminar.
            }
        });
        function getArguments() {
            arg = document.getElementById("argumentos").value;
            dispatchEventClient('sendArguments', arg);
        }

        window.addEventListener('invokerOk', function (e) {
            type = e.detail;
            if (type === 'W') {
                MiFuncionOkWeb();
            } else if (type === 'L') {
                MiFuncionOkLocal();
            }
        });

        window.addEventListener('invokerCancel', function (e) {
            MiFuncionCancel();
        });



        //::LÓGICA DEL PROGRAMADOR::	
        function ObtieneArgumentosParaFirmaDesdeLaWeb() {
            var typeBusqueda = "W";
            var pageUrl = '<%= ResolveUrl("~/Sunarp/Refirma.aspx/retornarArgumentosJson") %>';
            $.ajax({
                type: "POST",
                url: pageUrl,
                data: '{"type" :"' + typeBusqueda + '", "documentName" : "' + documentName_ + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {
                        document.getElementById("argumentos").value = data.d;
                        getArguments();
                    }
                }
            });
        }
        function ObtieneArgumentosParaFirmaDesdeArchivoLocal() {
            var typeBusqueda = "L";
            $.ajax({
                type: "POST",
                url: "Refirma.aspx/retornarArgumentosJson",
                data: '{"type" :"' + typeBusqueda + '", "documentName" : "' + documentName_ + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {
                        document.getElementById("argumentos").value = data.d;
                        getArguments();
                    }
                }
            });
        }
        <%--function ActualizarDocumento() {
            var solicitud = $('#<%= hISolicitud.ClientID %>').val();
            $.ajax({
                type: "POST",
                url: "frmSolicitudInscripcionSunarp.aspx/ActualizarDocumentoSolicitud",
                data: '{"idSolicitud" :"' + solicitud  + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log("Se actualizó");
                },
                error: function () {
                    console.log("No se ha podido Actualizar");
                }
            });
        }--%>
        function ActualizarDocumento() {
            var solicitud = $('#<%= hISolicitud.ClientID %>').val();
            var pageUrl = '<%= ResolveUrl("~/Sunarp/frmSolicitudInscripcionSunarp.aspx/ActualizarDocumentoSolicitud") %>';
            $.ajax({
                type: "POST",
                url: pageUrl,
                data: '{"idSolicitud" :"' + solicitud  + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {
                        console.log("Se actualizó");
                    }
                },
                error: function () {
                    console.log("No se ha podido Actualizar");
                }
            });
        }
        function MiFuncionOkWeb() {
            alert("Documento firmado desde una URL correctamente.");
            ActualizarDocumento();
            cerrarPopupMensaje();
            $('#<%= btnBuscar.ClientID %>').click();
        }

        function MiFuncionOkLocal() {
            alert("Documento firmado desde la PC correctamente.");
            cerrarPopupMensaje();
        }

        function MiFuncionCancel() {
            alert("El proceso de firma digital fue cancelado.");
            cerrarPopupMensaje();
        }
        //]]>
    </script>	
    <input type="hidden" id="argumentos" value="" />
    <div id="addComponent"></div>
</asp:Content>
