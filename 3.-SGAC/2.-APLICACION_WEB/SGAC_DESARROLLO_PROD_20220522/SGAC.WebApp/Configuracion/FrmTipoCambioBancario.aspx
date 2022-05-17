<%@ Page Language="C#" uiCulture="es-PE" culture="es-PE" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmTipoCambioBancario.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmTipoCambioBancario" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register src="~/Accesorios/SharedControls/ctrlPageBar.ascx" tagname="PageBar" tagprefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />    
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>  
    <script src="../Scripts/site.js" type="text/javascript"></script>    
   
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
     <script type="text/javascript">

         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
         $(document).ready(function () {
             Load();
         });

         function Load() {

             $("#<%=txtTCBancario.ClientID %>").on("blur", redondearNumero);
             $("#<%=txtTCBancario.ClientID %>").on("keydown", validarDecimal);

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

    </script>
  
    <div>
        
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2><asp:Label ID="lblTitutloTipoCambio" runat="server" Text="Tipo de Cambio Bancario"></asp:Label></h2>
                </td>
            </tr>
        </table>

        <%--Cuerpo--%>
        <table style="width: 90%" align="center">

            <tr>

                <td>

                    <div id="tabs">

                        <ul>
                            <li><a href="#tab-1"><asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                            <li><a href="#tab-2"><asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                        </ul>

                        <div id="tab-1">

                            <asp:UpdatePanel runat="server" ID="updConsulta" updatemode="Conditional">

                                <ContentTemplate>

                                    <table>

                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFecInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="dtpFecIniConsulta" runat="server" />                                
                                            </td>
                                            <td>
                                                <asp:Label ID="lblfechaFin" runat="server" Text="Fecha Fin:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="dtpFecFinConsulta" runat="server" /> 
                                            </td>

                                        </tr>

                                    </table>

                                    <%--Opciones--%>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <toolbar:toolbarcontent ID="ctrlToolBarConsulta" runat="server"></toolbar:toolbarcontent>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <Label:Validation ID="ctrlValidacion" runat="server"/>
                                            </td>
                                        </tr>
                                    </table>
                    
                                    <%-- Grilla --%>
                                    <table width="100%">

                                        <tr>

                                            <td>

                                                <asp:UpdatePanel runat="server" ID="updGrillaConsulta" updatemode="Conditional">

                                                    <ContentTemplate>

                                                        <asp:GridView ID="gdvTipoCambio" 
                                                                      runat="server"
                                                                      CssClass="mGrid"
                                                                      AlternatingRowStyle-CssClass="alt" 
                                                                      SelectedRowStyle-CssClass="slt"
                                                                      AutoGenerateColumns="False"
                                                                      GridLines="None"
                                                                      onrowcommand="gdvTipoCambio_RowCommand" 
                                                                      onrowdatabound="gdvTipoCambio_RowDataBound">

                                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

                                                            <Columns>
                                                                <asp:BoundField DataField="tiba_dFecha" HeaderText="Fecha" HeaderStyle-HorizontalAlign="Center" >
                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="tiba_fValorBancario" HeaderText="T.C. Bancario" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="moneda_oficial" HeaderText="Moneda" >
                                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate> 
                                                                        <asp:ImageButton ID="btnConsultar" 
                                                                            CommandName="Consultar" tooltip="Consultar"
                                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                                                            runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEditar" CommandName="Editar" tooltip="Editar"
                                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                                                            runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <SelectedRowStyle CssClass="slt" />

                                                        </asp:GridView>

                                                    </ContentTemplate>

                                                </asp:UpdatePanel>

                                                <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />

                                            </td>

                                        </tr>

                                    </table>

                                </ContentTemplate>

                            </asp:UpdatePanel>

                        </div>

                        <div id="tab-2">

                            <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">

                                <ContentTemplate>

                                    <toolbar:toolbarcontent ID="ctrlToolBarMantenimiento" runat="server"></toolbar:toolbarcontent>

                                    <table>
                                        <tr><td colspan="2">
                                            <%--Mensaje Por defecto--%>
                                            <asp:Label ID="lblValidacion" runat="server" Text="Falta ingresar Tipo de Cambio Bancario." CssClass="hideControl" ForeColor="Red"></asp:Label></td>
                                        </tr>                                        
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFecha" runat="server" Text="Fecha Inicio:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="dtpFecActual" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFechaFinRegistro" runat="server" Text="Fecha Fin:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTCBancario" runat="server" Text="T.C. Bancario:" CssClass="campoNumero"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTCBancario" 
                                                             runat="server" 
                                                             onkeypress="return isNumero(event)"
                                                             MaxLength="14" 
                                                             style="text-align:right"
                                                             CssClass="campoNumero" 
                                                             Width="120px" />
                                                <asp:Label ID="lblVal_txtTCBancario" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTipoMoneda" runat="server" Text="Tipo Moneda:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMonedaTipo" 
                                                                  runat="server" 
                                                                  Width="200px" 
                                                    Enabled="False"></asp:DropDownList>                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEstadoTipoCambio" runat="server" Text="Estado:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkActivoMant" runat="server" Text="Activo" Checked="true" />
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
    
    <script language="javascript" type="text/javascript">

        $(function () {
            $('#tabs').tabs();
        });

        function Validar(e) {
            var bolValida = true;
            var strTCBancario = $.trim($("#<%= txtTCBancario.ClientID %>").val());

            var txtTCBancario = document.getElementById('<%= txtTCBancario.ClientID %>');
            if (strTCBancario.length == 0) {
                txtTCBancario.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtTCBancario.style.border = "1px solid #888888";
            }

           
            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
                /*
                bolValida = confirm("¿Está seguro de grabar los cambios?"
                    + "\n"
                    + "NOTA: Si la fecha ya tiene registrado TIPO DE CAMBIO el valor se actualizará");
                */
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }


            return bolValida;
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

        function validarDecimal(e) {
            if (e.keyCode == 8 // backspace
            || e.keyCode == 9 // tab
            || e.keyCode == 13 // enter
            || e.keyCode == 27 // escape
            || e.keyCode == 46 // delete
            || (e.keyCode >= 35 && e.keyCode <= 39) // end, home, left arrow, up arrow, right arrow
            ) {
                return;
            }
            else {
                if (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105 ||
                (e.keyCode == 110) ||
                (e.keyCode == 190)))) {
                    // not 0-9, numpad 0-9                    
                    e.preventDefault();
                    return;
                }
                else {
                    var keyCode = e.keyCode;
                    if (keyCode >= 96 && keyCode <= 105) {
                        keyCode -= 48;
                    }

                    var value = $(this).val();

                    if (keyCode == 110 || keyCode == 190) {
                        if (value.indexOf('.') === -1) {
                            value += String.fromCharCode(keyCode);
                            value = parseInt(value, 10)
                            var maxNumber = $(this).data("maxnumber");
                            if (maxNumber) {
                                maxNumber = parseInt(maxNumber);
                                if (value > maxNumber) {
                                    e.preventDefault();
                                }
                            }
                        }
                        else {
                            e.preventDefault();
                            return;
                        }
                    }
                    else {
                        value += String.fromCharCode(keyCode);
                        value = parseInt(value, 10)
                        var maxNumber = $(this).data("maxnumber");
                        if (maxNumber) {
                            maxNumber = parseInt(maxNumber);
                            if (value > maxNumber) {
                                e.preventDefault();
                            }
                        }
                    }
                }
            }
        }

        function redondearNumero() {
            var floNumero = $.trim($("#<%= txtTCBancario.ClientID %>").val());
            var nummer = (Math.round(floNumero * 100000) / 100000);
            $("#<%= txtTCBancario.ClientID %>").val(nummer.toFixed(5));    
            return nummer
        }

        function redondeo(numero, decimales) {
            var flotante = parseFloat(numero);
            var resultado = Math.round(flotante * Math.pow(10, decimales)) / Math.pow(10, decimales);
            return resultado;
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

    </script>

</asp:Content>

