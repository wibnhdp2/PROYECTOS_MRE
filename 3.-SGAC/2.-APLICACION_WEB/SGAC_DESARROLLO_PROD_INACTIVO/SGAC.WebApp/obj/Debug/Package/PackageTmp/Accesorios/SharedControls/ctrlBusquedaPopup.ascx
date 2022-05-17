<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlBusquedaPopup.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlBusquedaPopup" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
    

<script src="<%=Page.ResolveUrl("~/Scripts/jquery-1.10.2.js") %>" type="text/javascript"></script>
<style type="text/css">
    .clLink 
    {
        font-size: 10pt;
	    font-family: Arial;
	    font-weight:bold;
	    color: Gray;	        
    }
        
    a:link {color: Black;}
    a:visited {color: Black;}
    a:hover {color:Black;}
    a:active {color:Black;} 
</style>  

<div>
<asp:HiddenField ID="HFGUID" runat="server" />
    <div>        
        <table width="90%">
            <tr>
                <td>
                    <h2><asp:Label ID="lblBusquedaTitulo" runat="server" Text="Búsqueda Titular"></asp:Label></h2>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="UpdGrvPaginada" UpdateMode="Conditional">
            <ContentTemplate>

                <asp:HiddenField ID="hEnter" runat="server" />
                <asp:HiddenField ID="hid_iPersonaId" runat="server" Value="0" />                
                <asp:HiddenField ID="hdn_tipo_persona" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_tipo_direccion" runat="server" Value="0" />

                <table class="mTblPrincipal">
                    <tr>
                        <td style="width:80px">
                            <asp:Label ID="Label1" runat="server" Text="Tipo Persona: "></asp:Label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlPersonaTipo" runat="server" Width="105px" OnSelectedIndexChanged="ddlPersonaTipo_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <font>
                                            <asp:Label ID="lblNroDocumento" runat="server" Text="Nro Documento:" Width="110px"></asp:Label></font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNroDocumento" runat="server" Width="100px" MaxLength="20" CssClass="txtLetra"
                                            onkeypress = "return isNumeroLetra(event)"
                                            ontextchanged="txtNroDocumento_TextChanged" ToolTip="Para poder realizar la búsqueda debe ingresar el Nro. Documento completo." />
                                    </td>
                                </tr>
                            </table>                            
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPriApellido" runat="server" Text="Primer Apellido:" Width="120px"></asp:Label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlPersonaNatural" Visible ="true" runat="server">
                                        <asp:TextBox ID="txtPriApellido" runat="server" Width="100px" 
                                            ToolTip="Para poder realizar la búsqueda debe ingresar como mínimo 3 caracteres."
                                            CssClass="txtLetra" MaxLength="50" />
                                        <asp:Label ID="lblSegApellido" runat="server" Text="Segundo Apellido:" Width="120px"></asp:Label>
                                        <asp:TextBox ID="txtSegApellido" runat="server" Width="100px" 
                                            ToolTip="Para poder realizar la búsqueda debe ingresar como mínimo 3 caracteres."
                                            CssClass="txtLetra" MaxLength="50" />
                                        <asp:Label ID="lblNombres" runat="server" Text="Nombres:"></asp:Label>
                                        <asp:TextBox ID="txtNombres" runat="server" Width="100px" 
                                            ToolTip="Para poder realizar la búsqueda debe ingresar como mínimo 3 caracteres."
                                            CssClass="txtLetra" MaxLength="50" />
                                        </asp:Panel>
                                        <asp:Panel ID="pnlPersonaJuridica" Visible = "false" runat="server">
                                            <asp:TextBox ID="txtRazonSocial" runat="server" 
                                                ToolTip="Para poder realizar la búsqueda debe ingresar como mínimo 3 caracteres."
                                                CssClass="txtLetra" MaxLength="50"
                                                Width="450px" />
                                        </asp:Panel>
                                    </td>                                    
                                    <td>
                                        <asp:Button ID="btn_Buscar" runat="server" Text="   Buscar" CssClass="btnSearch" OnClick="btn_Buscar_Click"
                                            UseSubmitBehavior="False" />
                                    </td>
                                    <td style="width:10px"></td>
                                    <td align="right">
                                        <asp:Button ID="btn_NewRUNE" runat="server" Text="Nuevo RUNE" OnClick="btn_NewRUNE_Click"
                                            CssClass="btnRUNE" UseSubmitBehavior="False" Visible="false" />
                                        <a class="clLink" href="FrmRegistroPersona.aspx" target="_parent">Nuevo RUNE</a>
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
                            <asp:GridView ID="Grd_Solicitante" runat="server" AlternatingRowStyle-CssClass="alt"
                                AutoGenerateColumns="False" CssClass="mGrid" DataKeyNames="vNombre,vNroDocumento,vFecNac"
                                GridLines="None" OnRowCommand="Grd_Solicitante_RowCommand"
                                Height="16px" Width="100%">
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:BoundField DataField="iPersonaId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="PersonaId"
                                        ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sPersonaTipoId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="PersonaTipoId"
                                        ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sDocumentoTipoId" HeaderStyle-CssClass="ColumnaOculta"
                                        HeaderText="DocumentoTipoId" ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vDescTipDoc" HeaderStyle-CssClass="ColumnaOculta" HeaderText="DescTipDoc"
                                        ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vApellidoPaterno" HeaderStyle-CssClass="ColumnaOculta"
                                        HeaderText="ApePat" ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vApellidoMaterno" HeaderStyle-CssClass="ColumnaOculta"
                                        HeaderText="ApeMat" ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vNombres" HeaderStyle-CssClass="ColumnaOculta" HeaderText="vNombres"
                                        ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vNombre" HeaderText="Apellidos y Nombres" >
                                        <ItemStyle Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vNroTipoDocumento" HeaderText="Número de Documento" >
                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vFecNac" HeaderText="Fecha Nacimiento" 
                                        DataFormatString="{0:MMM-dd-yyyy}">
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="pers_vNacionalidad" HeaderText="Nacionalidad" >
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <%--Celda: 11--%>
                                    <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnSeleccionar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                CommandName="Select" ImageUrl="~/Images/img_sel_check.png" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vNroDocumento" HeaderText="Nro Documento" HeaderStyle-CssClass="ColumnaOculta"
                                        ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sNacionalidadId" HeaderText="sNacionalidadId" HeaderStyle-CssClass="ColumnaOculta"
                                        ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="Grd_Empresa" runat="server" AlternatingRowStyle-CssClass="alt"
                                AutoGenerateColumns="False" CssClass="mGrid" GridLines="None" Height="16px" Width="100%"
                                OnRowCommand="Grd_Empresa_RowCommand" >
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:BoundField DataField="empr_iEmpresaId" HeaderStyle-CssClass="ColumnaOculta"
                                        HeaderText="empr_iEmpresaId" ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="empr_sTipoEmpresaId" HeaderStyle-CssClass="ColumnaOculta"
                                        HeaderText="empr_sTipoEmpresaId" ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="empr_sTipoDocumentoId" HeaderText="empr_sTipoDocumentoId"
                                        HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vDescTipDoc" HeaderText="vDescTipDoc" HeaderStyle-CssClass="ColumnaOculta"
                                        ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="empr_vRazonSocial" HeaderText="Razón Social">
                                        <HeaderStyle Width="250px" />
                                        <ItemStyle HorizontalAlign="Left" Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="empr_vNumeroDocumento" HeaderText="Número Documento">
                                        <HeaderStyle Width="250px" />
                                        <ItemStyle HorizontalAlign="Center" Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="empr_vActividadComercial" HeaderText="empr_vActividadComercial"
                                        HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="empr_vTelefono" HeaderText="Teléfono">
                                        <HeaderStyle Width="250px" />
                                        <ItemStyle HorizontalAlign="Center" Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="empr_vCorreo" HeaderText="Correo">
                                        <HeaderStyle Width="250px" />
                                        <ItemStyle HorizontalAlign="Center" Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="empr_cEstado" HeaderText="empr_cEstado" HeaderStyle-CssClass="ColumnaOculta"
                                        ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnSeleccionar0" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                CommandName="Select" ImageUrl="~/Images/img_sel_check.png" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:ctrlPageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
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
        $("#<%=txtNroDocumento.ClientID %>").bind("keypress", function (e) {

            if (e.keyCode == 13) {
                document.getElementById("<%=btn_Buscar.ClientID %>").click();
                e.preventDefault();
            }
        });

        $("#<%=txtPriApellido.ClientID %>").bind("keypress", function (e) {

            if (e.keyCode == 13) {
                document.getElementById("<%=btn_Buscar.ClientID %>").click();
                e.preventDefault();
            }
            ValidarSujeto(e);
        });
        $("#<%=txtSegApellido.ClientID %>").bind("keypress", function (e) {

            if (e.keyCode == 13) {
                document.getElementById("<%=btn_Buscar.ClientID %>").click();
                e.preventDefault();
            }
        });
        $("#<%=txtNombres.ClientID %>").bind("keypress", function (e) {

            if (e.keyCode == 13) {
                document.getElementById("<%=btn_Buscar.ClientID %>").click();
                e.preventDefault();
            }
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
</script>
