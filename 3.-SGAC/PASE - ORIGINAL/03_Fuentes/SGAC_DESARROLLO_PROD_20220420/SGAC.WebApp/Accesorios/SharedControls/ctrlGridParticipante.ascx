<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlGridParticipante.ascx.cs"
    Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlGridParticipante" %>
<%@ Register Src="ctrlUbigeo.ascx" TagName="ctrlUbigeo" TagPrefix="uc1" %>

<script language="javascript" type="text/javascript">
    function ActoMilitar_Participantes() {
        var bolValida = true;

        if (ddlcontrolError(document.getElementById("<%= ddl_TipoParticipante.ClientID %>")) == false) bolValida = false;
        if (ddlcontrolError(document.getElementById("<%= ddl_TipoDocParticipante.ClientID %>")) == false) bolValida = false;
        if (txtcontrolError(document.getElementById("<%= txtNroDocParticipante.ClientID %>")) == false) bolValida = false;
        if (ddlcontrolError(document.getElementById("<%= ddl_NacParticipante.ClientID %>")) == false) bolValida = false;
        if (txtcontrolError(document.getElementById("<%= txtNomParticipante.ClientID %>")) == false) bolValida = false;
        if (txtcontrolError(document.getElementById("<%= txtApePatParticipante.ClientID %>")) == false) bolValida = false;
        return bolValida;
    }

    function conMayusculas(field) {
        field.value = field.value.toUpperCase()
    }

    function ValidarPersona() {
        var bolValida = true;

        var strTipoDocParticipante = $.trim($("#<%= ddl_TipoDocParticipante.ClientID %>").val());
        var ddlTipoDocParticipante = document.getElementById('<%= ddl_TipoDocParticipante.ClientID %>');
        if (strTipoDocParticipante == "0") {
            bolValida = false;
            ddlTipoDocParticipante.style.border = "1px solid Red";
        }
        else {
            ddlTipoDocParticipante.style.border = "1px solid #888888";
        }

        var strNroDocumento = $.trim($("#<%= txtNroDocParticipante.ClientID %>").val());
        var txtNroDocumento = document.getElementById('<%= txtNroDocParticipante.ClientID %>');
        if (strNroDocumento == "") {
            bolValida = false;
            txtNroDocumento.style.border = "1px solid Red";
        }
        else {
            txtNroDocumento.style.border = "1px solid #888888";
        }
        return bolValida;
    }
</script>

<table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
    <tr>
        <td width="170px">
            <asp:Label ID="lblPartTipoPart" runat="server" Text="Tipo Participante:" />
        </td>
        <td width="35%">
            <asp:DropDownList ID="ddl_TipoParticipante" runat="server" Width="250px" Height="21px">
            </asp:DropDownList>
            <asp:Label ID="Label60" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
        </td>
        <td width="15%">
            <asp:Label ID="lblPartTipoVinc" runat="server" Text="Tipo Dato:" />
        </td>
        <td width="33%">
            <asp:DropDownList ID="ddl_TipoDatoParticipante" runat="server" Width="265px" Style="margin-bottom: 0px"
                Height="21px">
            </asp:DropDownList> 
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label36" runat="server" Text="Tipo Vínculo:" />
        </td>
        <td>
            <asp:DropDownList ID="ddl_TipoVinculoParticipante" runat="server" Width="250px" Height="21px">
            </asp:DropDownList>
        </td>
        <td>
            <asp:TextBox ID="txtPersonaId" runat="server" Text="0" CssClass="hideControl"></asp:TextBox>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblPartTipoDoc" runat="server" Text="Tipo Documento:" />
        </td>
        <td>
            <asp:DropDownList ID="ddl_TipoDocParticipante" runat="server" Width="250px" Height="21px">
            </asp:DropDownList>
            <asp:Label ID="Label63" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPartNroDoc" runat="server" Text="Nro. Documento:"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtNroDocParticipante" runat="server" Width="242px" CssClass="campoNumero"
                onkeypress="return isNumberKey(event)"  />
            <asp:ImageButton ID="imgBuscar" ImageUrl="~/Images/img_16_search.png" runat="server"
                OnClick="imgBuscar_Click" Style="width: 16px" />
            <asp:Label ID="Label64" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label49" runat="server" Text="Nacionalidad" />
        </td>
        <td>
            <asp:DropDownList ID="ddl_NacParticipante" runat="server" Width="250px" Height="21px">
            </asp:DropDownList>
            <asp:Label ID="Label65" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label34" runat="server" Text="Nombres:" />
        </td>
        <td>
            <asp:TextBox ID="txtNomParticipante" runat="server" Width="261px" onBlur="conMayusculas(this)" CssClass="txtLetra" />
            <asp:Label ID="Label66" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label32" runat="server" Text="Primer Apellido:" />
        </td>
        <td>
            <asp:TextBox ID="txtApePatParticipante" runat="server" Width="246px" onBlur="conMayusculas(this)" CssClass="txtLetra" />
            <asp:Label ID="Label70" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label45" runat="server" Text="Segundo Apellido:" />
        </td>
        <td>
            <asp:TextBox ID="txtApeMatParticipante" runat="server" Width="261px" onBlur="conMayusculas(this)" CssClass="txtLetra"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label73" runat="server" Text="Dirección:" />
        </td>
        <td colspan="3">
            <asp:TextBox ID="txtDireccionParticipante" runat="server" Width="430px" onBlur="conMayusculas(this)" CssClass="txtLetra"/>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <uc1:ctrlUbigeo ID="ctrlUbigeo1" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:Label ID="Label4" runat="server" Text="Observaciones:" />
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:TextBox ID="txtObservacionesAP" runat="server" Height="49px" Width="100%" TextMode="MultiLine"
                CssClass="txtLetra" Enabled="False" onBlur="conMayusculas(this)"/>
        </td>
        <td align="right">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btnGeneral" OnClick="btnAceptar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnGeneral"
                OnClick="btnCancelar_Click" />
        </td>
    </tr>
</table>
<br />
<table width="100%" class="mTblSecundaria" bgcolor="#4E102E">
    <tr>
        <td>
            <asp:Label ID="Label8" runat="server" Text="DETALLE DE PARTICIPANTE(S)" Style="font-weight: 700;"
                ForeColor="White" />
        </td>
    </tr>
</table>
<table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
    <tr>
        <td>
            <asp:GridView ID="Grd_Participantes" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                GridLines="None" SelectedRowStyle-CssClass="slt" DataKeyNames="iActuacionParticipanteId,iPersonaId,sTipoParticipanteId,sTipoDatoId,sTipoVinculoId"
                ShowHeaderWhenEmpty="True" OnRowCommand="Grd_Participantes_RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="Nro." DataField="iItemRow" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="iActuacionParticipanteId" DataField="iActuacionParticipanteId"
                        HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                        <HeaderStyle CssClass="ColumnaOculta" />
                        <ItemStyle CssClass="ColumnaOculta" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Apellidos y Nombres" DataField="vNombreCompleto" />
                    <asp:BoundField HeaderText="Tipo Participante" DataField="vTipoParticipante" />
                    <asp:BoundField DataField="sDocumentoTipoId" HeaderStyle-CssClass="ColumnaOculta"
                        HeaderText="sDocumentoTipoId" ItemStyle-CssClass="ColumnaOculta">
                        <HeaderStyle CssClass="ColumnaOculta" />
                        <ItemStyle CssClass="ColumnaOculta" />
                    </asp:BoundField>
                    <asp:BoundField DataField="vDocumentoNumero" HeaderStyle-CssClass="ColumnaOculta"
                        HeaderText="vDocumentoNumero" ItemStyle-CssClass="ColumnaOculta">
                        <HeaderStyle CssClass="ColumnaOculta" />
                        <ItemStyle CssClass="ColumnaOculta" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Nro. Documento" DataField="vDocumentoCompleto" />
                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEditar2" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                CommandName="Editar" ImageUrl="~/Images/img_grid_modify.png" ToolTip="Editar Notificación" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnAnular4" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                CommandName="Eliminar" ImageUrl="~/Images/img_grid_delete.png" ToolTip="Eliminar Notificación" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:TemplateField>
                </Columns>
                <SelectedRowStyle CssClass="slt" />
                <RowStyle Font-Names="Arial" Font-Size="11px"></RowStyle>
                <EmptyDataTemplate>
                    <table id="tbSinDatos0">
                        <tbody>
                            <tr>
                                <td width="10%">
                                    <asp:Image runat="server" ID="imgWarning0" ImageUrl="~/Images/img_16_warning.png" />
                                </td>
                                <td width="5%">
                                </td>
                                <td width="85%">
                                    <asp:Label ID="lblSinDatos0" runat="server" Text="No se encontraron Datos..."></asp:Label>
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