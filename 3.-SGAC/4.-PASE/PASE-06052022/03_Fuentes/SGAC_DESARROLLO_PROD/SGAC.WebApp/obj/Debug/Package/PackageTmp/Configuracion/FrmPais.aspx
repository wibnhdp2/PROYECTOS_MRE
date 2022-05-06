<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmPais.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmPais" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="PageBar" %> 
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>    
    
    <style type="text/css">
        .style1
        {
            width: 88px;
        }
        .style2
        {
            width: 119px;
        }
        .style7
        {
            width: 85px;
        }
        .style8
        {
            width: 100px;
        }
        .style9
        {
            width: 30px;
        }
        .style10
        {
            width: 73px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript">

         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
         $(document).ready(function () {
             Load();
         });

         function Load() {

             //Previene el postback al hacer enter
             $(function () {
                 $(':text').bind('keydown', function (e) {
                     //on keydown for all textboxes
                     if (e.target.className != "searchtextbox") {
                         if (e.keyCode == 13) { //if this is enter key
                             e.preventDefault();
                             return false;
                         } else
                             return true;
                     } else
                         return true;
                 });
             });

             //Posibilita desplazamiento con enter entre campos
             $(function () {
                 $('input:text:first').focus();
                 var $inp = $('input:text');
                 $inp.bind('keydown', function (e) {
                     //var key = (e.keyCode ? e.keyCode : e.charCode);
                     var key = e.which;
                     if (key == 13) {
                         e.preventDefault();
                         var nxtIdx = $inp.index(this) + 1;
                         $(":input:text:eq(" + nxtIdx + ")").focus();
                     }
                 });
             });
         }

         function validarSoloLetras(lbl, txt) {
             var charpos = txt.value.search("[^A-ZÁÉÍÓÚÜáéíóúüÑña-z ]");

             if (txt.value.length > 0 && charpos >= 0) {
                 lbl.style.display = 'inline';
                 txt.value = '';
             }
             else {
                 lbl.style.display = 'none';
             }
         }
         function isLetras(lbl, txt) {
             var charpos = txt.value.toUpperCase().search("[^A-Z]");

             if (txt.value.length > 0 && charpos >= 0) {
                 lbl.style.display = 'inline';
                 txt.value = '';
             }
             else {
                 lbl.style.display = 'none';
             }
         }

         function isZonaHoraria(lbl, txt) {
             var charpos = txt.value.toUpperCase().search("(GMT)[?+|-][0-9][0-9](:)[0-9][0-9]");

             if (txt.value.length > 0 && charpos >= 0) {
                 lbl.style.display = 'none';
             }
             else {
                 lbl.style.display = 'inline';
                 txt.value = '';
             }
         }
       
    </script>
   
    <div>
        <%--Titulo--%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTituloPais" runat="server" Text="País"></asp:Label></h2>
                </td>
            </tr>
        </table>
        <%--Cuerpo--%>
        <table style="width: 90%" align="center">
            <tr>
                <td align="left">
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">Consulta</a></li>
                            <li><a href="#tab-2">Registro</a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <%-- Consulta --%>
                                    <table>
                                        <tr>
                                            <td class="style2">
                                                <asp:Label ID="lblConsultaNombrePais" runat="server" Text="Nombre del País:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtConsultaNombrePais" runat="server" Width="250px"
                                                    CssClass="txtLetra" onkeypress="return isNombre(event)" 
                                                    onblur ="return validarSoloLetras(lblMensajeConsultaPais , this)"  
                                                    MaxLength="100" ></asp:TextBox>
                                                    <br />
                                                    <label id="lblMensajeConsultaPais" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                            </td>
                                            <td class="style10"></td>
                                            <td class="style1">
                                            <asp:Label ID="lblConsultaContinente" runat="server" Text="Continente:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlConsultaContinente" runat="server" Width="300px" TabIndex="1">
                                                </asp:DropDownList>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2"> 
                                                <asp:Label ID="lblEstado" runat="server" Text="Estado:" ></asp:Label>                                              
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" Checked="True" />
                                            </td>
                                            <td class="style10">                                               
                                            </td>
                                            <td>
                                                <asp:Label ID="lblConsultaIdioma" runat="server" Text="Idioma:"></asp:Label>
                                            </td>
                                            <td>
                                            <asp:DropDownList ID="ddlConsultaIdioma" runat="server" Width="300px" TabIndex="2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                       
                                    </table>
                                    <%--Opciones--%>
                                    <table>
                                        <tr>
                                            <td>
                                                <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                            </td>
                                        </tr>
                                    </table>
                                    <%-- Grilla --%>
                                    <table>
                                        <tr>
                                            <td style="width: 870px;">
                                                <Label:Validation ID="ctrlValidacion" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gdvPais" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                    OnRowCommand="gdvPais_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="pais_spaisid" HeaderText="ID">
                                                           <HeaderStyle Width="50px"  HorizontalAlign="Center"/>
                                                            <ItemStyle Width="50px"  HorizontalAlign="Center"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="pais_vnombre" HeaderText="Nombre del País">
                                                            <HeaderStyle Width="150px" />
                                                            <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="continente" HeaderText="Continente">
                                                            <HeaderStyle Width="150px" />
                                                            <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="pais_vnacionalidad" HeaderText="Nacionalidad">
                                                            <HeaderStyle Width="150px" />
                                                            <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="idioma" HeaderText="Idioma">
                                                            <HeaderStyle Width="150px" />
                                                            <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="moneda" HeaderText="Moneda">
                                                            <HeaderStyle Width="150px" />
                                                            <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="pais_vzonahoraria" HeaderText="Zona horaria">
                                                            <HeaderStyle Width="50px" />
                                                            <ItemStyle Width="50px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PAIS_CESTADO" HeaderText="Estado">
                                                            <HeaderStyle Width="50px"  HorizontalAlign="Center"/>
                                                            <ItemStyle Width="50px"  HorizontalAlign="Center"/>
                                                        </asp:BoundField>

                                                        <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnConsultar" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="slt" />
                                                </asp:GridView>
                                                <PageBar:ctrlPageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-2">
                            <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <ToolBar:ToolBarContent ID="ctrlToolBarMantenimiento" runat="server"></ToolBar:ToolBarContent>
                                    <table>
                                        <tr>
                                            <td id="tdMsje" colspan="5">
                                                <asp:Label ID="lblValidacion"
                                                           runat="server"
                                                           Text="Debe ingresar los campos requeridos (*)"
                                                           CssClass="hideControl"
                                                           ForeColor="Red"
                                                           Font-Size="14px">
                                                </asp:Label>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style8">
                                                <asp:Label ID="lblregNombrePais" runat="server" Text="Nombre del País:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtregNombrePais" runat="server" Width="300px" 
                                                    CssClass="txtLetra" onblur ="return validarSoloLetras(lblMensajeregNombrePais , this)"
                                                    onkeypress="return isNombre(event)" onpaste="return false" MaxLength="100" 
                                                    TabIndex="2"></asp:TextBox>
                                                    <label style="color:Red; font-size:medium"> *</label>
                                                    <br />
                                                    <label id="lblMensajeregNombrePais" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                            </td>
                                            <td class="style9"></td>
                                            <td style="width:100px">
                                                <asp:Label ID="lblregnacionalidad" runat="server" Text="Nacionalidad:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtregNacionalidad" runat="server" Width="240px" 
                                                    CssClass="txtLetra" onblur ="return validarSoloLetras(lblMensajeregNacionalidad , this)"
                                                    onkeypress="return isNombre(event)" onpaste="return false" MaxLength="100" 
                                                    TabIndex="3"></asp:TextBox>
                                                    <label style="color:Red; font-size:medium"> *</label>
                                                    <br />
                                                    <label id="lblMensajeregNacionalidad" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style8">
                                                <asp:Label ID="lblregCapital" runat="server" Text="Capital:"></asp:Label>
                                            </td>
                                             <td>
                                               <asp:TextBox ID="txtregCapital" runat="server" Width="300px" CssClass="txtLetra" onblur ="return validarSoloLetras(lblMensajeregCapital , this)"
                                                    onkeypress="return isNombre(event)" onpaste="return false" MaxLength="100" 
                                                     TabIndex="4"></asp:TextBox>
                                                    <label style="color:Red; font-size:medium"> *</label>
                                                    <br />
                                                    <label id="lblMensajeregCapital" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                            </td>
                                            <td class="style9"></td>
                                            <td>
                                                <asp:Label ID="lblregContinente" runat="server" Text="Continente:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlregContinente" runat="server" Width="250px" 
                                                    TabIndex="5">
                                                </asp:DropDownList>
                                                <label style="color:Red; font-size:medium"> *</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style8">                                                
                                                <asp:Label ID="lblregZonaHoraria" runat="server" Text="Zona horaria:"></asp:Label>
                                                <br />
                                            </td>
                                            <td>
                                               <asp:TextBox ID="txtregZonaHoraria" runat="server" Width="100px" 
                                                    CssClass="txtLetra" onblur ="return isZonaHoraria(lblMensajeregZonaHoraria , this)"
                                                    onkeypress="return validarZonaHoraria(event)" onpaste="return false" 
                                                    MaxLength="9" TabIndex="6"></asp:TextBox>
                                                    <label style="color:Red; font-size:medium"> *</label>
                                                    <br />
                                                    <label id="lblMensajeregZonaHoraria" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permite el formato(GMT+/-nn:nn).</label>
                                                <br />
                                                <asp:Label ID="lblregHusoHorario" runat="server" Text="Huso horario (GMT+/-nn:nn)"></asp:Label>
                                                <asp:HiddenField ID="HF_sPaisId" runat="server" />
                                            </td>
                                            <td class="style9"></td>
                                            <td colspan="2">
                                                <asp:Label ID="lblregGentilicio" runat="server" Text="Gentilicio" Font-Bold="true"></asp:Label>       
                                                <table style="border-bottom: 1px solid gray; border-top: 1px solid gray; border-left: 1px solid gray; border-right: 1px solid gray; margin-top:5px;" width="100%" cellpadding="5px">
                                                    <tr>
                                                        <td class="style7">
                                                            <asp:Label ID="lblregGentilicioFemenino" runat="server" Text="Femenino:"></asp:Label>
                                                        </td>
                                                        <td>
                                                           <asp:TextBox ID="txtregGentilicioFemenino" runat="server" Width="240px" 
                                                                CssClass="txtLetra" onblur ="return validarSoloLetras(lblMensajeregGentilicioFemenino , this)"
                                                            onkeypress="return isNombre(event)" onpaste="return false" MaxLength="100" 
                                                                TabIndex="7"></asp:TextBox>
                                                            <label style="color:Red; font-size:medium"> *</label>
                                                            <br />
                                                            <label id="lblMensajeregGentilicioFemenino" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                                        
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style7">
                                                            &nbsp;<asp:Label ID="lblregGentilicioMasculino" runat="server" Text="Masculino:"></asp:Label>
                                                        </td>
                                                        <td>
                                                           <asp:TextBox ID="txtregGentilicioMasculino" runat="server" Width="240px" 
                                                                CssClass="txtLetra" onblur ="return validarSoloLetras(lblMensajeregGentilicioMasculino , this)"
                                                            onkeypress="return isNombre(event)" onpaste="return false" MaxLength="100" 
                                                                TabIndex="8"></asp:TextBox>
                                                            <label style="color:Red; font-size:medium"> *</label>
                                                            <br />
                                                            <label id="lblMensajeregGentilicioMasculino" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblregUnidadMonetaria" runat="server" Text="Unidad Monetaria" Font-Bold="true"></asp:Label>
                                                <table style="border-bottom: 1px solid gray; border-top: 1px solid gray; border-left: 1px solid gray; border-right: 1px solid gray; margin-top:5px" width="100%"  cellpadding="5px">
                                                    <tr>
                                                        <td>                                                           
                                                            <asp:Label ID="lblregNombreOficial" runat="server" Text="Nombre oficial:"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="ddlregMonedaOficial" runat="server" Width="250px" 
                                                                TabIndex="9" AutoPostBack="True" 
                                                                onselectedindexchanged="ddlregMonedaOficial_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <label style="color:Red; font-size:medium"> *</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>                                                            
                                                            <asp:Label ID="lblregCodigoMoneda" runat="server" Text="Código:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtregCodigoMoneda" runat="server" Width="50px" 
                                                                CssClass="txtLetra" ReadOnly="true"
                                                             onpaste="return false" MaxLength="100" TabIndex="10" BackColor="#CCCCCC"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblregSimboloMoneda" runat="server" Text="Símbolo:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtregSimboloMoneda" runat="server" Width="50px" 
                                                                CssClass="txtLetra" ReadOnly="true"
                                                             onpaste="return false" MaxLength="100" TabIndex="11" BackColor="#CCCCCC"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td class="style9"></td>
                                            <td colspan="2">
                                                <asp:Label ID="lblregISO3166" runat="server" Text="ISO 3166" Font-Bold="true"></asp:Label>     
                                                
                                                <table style="border-bottom: 1px solid gray; border-top: 1px solid gray; border-left: 1px solid gray; border-right: 1px solid gray; margin-top:5px" width="100%" cellpadding="5px">
                                                    <tr>
                                                        <td class="style7">
                                                            <asp:Label ID="lblregISOLetras" runat="server" Text="Letras:"></asp:Label>                                                            
                                                        </td>
                                                        <td>
                                                        <asp:TextBox ID="txtregISOLetra" runat="server" Width="50px" CssClass="txtLetra" onblur ="return isLetras(lblMensajeregISOLetra , this)"
                                                            onkeypress="return validarsololetra(event)" onpaste="return false" 
                                                                MaxLength="3" TabIndex="12"></asp:TextBox>
                                                            <label style="color:Red; font-size:medium"> *</label>
                                                            <br />
                                                            <label id="lblMensajeregISOLetra" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Debe digitar tres(3) letras (A-Z).</label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblregISONumero" runat="server" Text="Número:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtregISONumero" runat="server" Width="50px" CssClass="txtLetra" 
                                                                onkeypress="return validatenumber(event)" onpaste="return false" 
                                                                MaxLength="3" TabIndex="13"></asp:TextBox>
                                                            <label style="color:Red; font-size:medium"> *</label>                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEstadoMant" runat="server" Text="Estado:" ></asp:Label>
                                            </td>
                                            <td>
                                            <asp:CheckBox ID="chkActivoMant" runat="server" Text="Activo" Checked="true" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblregIdioma" runat="server" Text="Idioma:"></asp:Label>                                                                                                
                                            </td>
                                            <td>
                                            <asp:DropDownList ID="ddlregIdioma" runat="server" Width="250px" 
                                                    TabIndex="5">
                                                </asp:DropDownList>
                                                <label style="color:Red; font-size:medium"> *</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td colspan="2">
                                            (para el formato de Antecedentes Penales y Constancia de Inscripción)
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
        <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
    </div>
    <br />
    <script language="javascript" type="text/javascript">
        $(function () {
            $('#tabs').tabs();
        });

        function Validar() {

            var bolValida = true;

            var strregNombrePais = $.trim($("#<%= txtregNombrePais.ClientID %>").val());
            var strregNacionalidadPais = $.trim($("#<%= txtregNacionalidad.ClientID %>").val());
            var strregCapitalPais = $.trim($("#<%= txtregCapital.ClientID %>").val());
            var strregContinente = $.trim($("#<%= ddlregContinente.ClientID %>").val());
            var strregZonaHoraria = $.trim($("#<%= txtregZonaHoraria.ClientID %>").val());
            var strregGentilicioFemenino = $.trim($("#<%= txtregGentilicioFemenino.ClientID %>").val());
            var strregGentilicioMasculino = $.trim($("#<%= txtregGentilicioMasculino.ClientID %>").val());
            var strregMonedaOficial = $.trim($("#<%= ddlregMonedaOficial.ClientID %>").val());
            var strregISOLetra = $.trim($("#<%= txtregISOLetra.ClientID %>").val());
            var strregISONumero = $.trim($("#<%= txtregISONumero.ClientID %>").val());
            var strregIdioma = $.trim($("#<%= ddlregIdioma.ClientID %>").val());

            if (strregNombrePais.length == 0) {
                $("#<%=txtregNombrePais.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtregNombrePais.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregNacionalidadPais.length == 0) {
                $("#<%=txtregNacionalidad.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtregNacionalidad.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregCapitalPais.length == 0) {
                $("#<%=txtregCapital.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtregCapital.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregContinente == 0) {
                $("#<%=ddlregContinente.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlregContinente.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregZonaHoraria.length == 0) {
                $("#<%=txtregZonaHoraria.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtregZonaHoraria.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregGentilicioFemenino.length == 0) {
                $("#<%=txtregGentilicioFemenino.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtregGentilicioFemenino.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregGentilicioMasculino.length == 0) {
                $("#<%=txtregGentilicioMasculino.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtregGentilicioMasculino.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregMonedaOficial == 0) {
                $("#<%=ddlregMonedaOficial.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlregMonedaOficial.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregISOLetra.length == 0) {
                $("#<%=txtregISOLetra.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtregISOLetra.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregISONumero.length == 0) {
                $("#<%=txtregISONumero.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtregISONumero.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregIdioma == 0) {
                $("#<%=ddlregIdioma.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlregIdioma.ClientID %>").css("border", "solid #888888 1px");
            }

            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }
            return bolValida;
        }

        function isNombre(evt) {
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
            var letras = "áéíóúüñÑÁÉÍÓÚ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            return letra;
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
             
    </script>
</asp:Content>