<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmBuscarFuncionario.aspx.cs"
    Inherits="SGAC.WebApp.Configuracion.FrmBuscarFuncionario" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    
    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.numeric.js" type="text/javascript"></script>
   
   
<html xmlns="http://www.w3.org/1999/xhtml">
    
    <head runat="server">
    
</head>
<body>       
    <form id="form1" runat="server">
    <div>
        <div>
            <br />
            <%--Cuerpo--%>
            <table class="mTblTituloM2" align="center">
                <tr>
                    <td>
                        <h2>
                            <asp:Label ID="lblTitBusqConnac" runat="server" Text="Búsqueda Solicitante"></asp:Label></h2>
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
                    <%--Cuerpo--%>
                    <table class="mTblPrincipal2" align="center">
                        <tr>
                            <td style="font-weight: inherit; text-align: center;">
                                <font>
                                    <asp:Label ID="lblNroDocumento" runat="server" Style="text-align: center" Text="Nro Documento"
                                        Width="120px"></asp:Label>
                                </font>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNroDocumento" runat="server" MaxLength="20"
                                     Width="101px"  />
                            </td>
                            <td style="font-weight: inherit; text-align: center;">
                                <font>
                                    <asp:Label ID="lblPriApellido" runat="server" Style="text-align: center" Text="Primer Apellido"
                                        Width="100px"></asp:Label>
                                </font>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPriApellido" runat="server" CssClass="txtLetra"
                                     MaxLength="50" Width="101px"
                                     ToolTip="Para poder realizar la búsqueda debe ingresar como mínimo 3 caracteres"                                    
                                     onkeypress = "return isNumeroLetra(event)" />
                            </td>
                            <td style="font-weight: inherit; text-align: center;">
                                <font>
                                    <asp:Label ID="lblSegApellido" runat="server" Style="text-align: center" Text="Segundo Apellido"
                                        Width="100px"></asp:Label>
                                </font>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSegApellido" runat="server" CssClass="txtLetra"
                                    MaxLength="50" Width="101px"
                                    ToolTip="Para poder realizar la búsqueda debe ingresar como mínimo 3 caracteres"                                    
                                    onkeypress = "return isNumeroLetra(event)" />
                            </td>
                            <td style="font-weight: inherit; text-align: center;">
                                <font>
                                    <asp:Label ID="Label1" runat="server" Style="text-align: center" Text="Nombres"
                                        Width="100px"></asp:Label>
                                </font>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNombres" runat="server" CssClass="txtLetra"
                                    MaxLength="50" Width="101px"
                                    ToolTip="Para poder realizar la búsqueda debe ingresar como mínimo 3 caracteres"
                                    onkeypress = "return isNumeroLetra(event)" />
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="btn_Buscar" runat="server" CssClass="btnSearch" OnClick="btn_Buscar_Click"
                                    Text="  Buscar" />
                            </td>                            
                        </tr>
                    </table>
                    <table width="95%" align="center">
                        <tr>
                            <td>
                               <Label:Validation ID="ctrlValidacion" runat="server" />                           
                            </td>
                        </tr>
                     
                        <tr>
                            <td style="text-align: center">
                                <asp:GridView ID="Grd_Solicitante" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                    runat="server" DataKeyNames="vCargoFuncionario" AutoGenerateColumns="False"
                                    GridLines="None" OnRowCommand="Grd_Solicitante_RowCommand" Width="100%">
                                    <AlternatingRowStyle CssClass="alt" />
                                    <Columns>
                                        <%--Indices utilizados 0-5--%>
                                        <asp:BoundField DataField="IfuncionarioId" HeaderText="FuncionarioId" HeaderStyle-CssClass="ColumnaOculta"
                                            ItemStyle-CssClass="ColumnaOculta">
                                            <HeaderStyle CssClass="ColumnaOculta" />
                                            <ItemStyle CssClass="ColumnaOculta" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sDocumentoFuncionario" HeaderText="Nro Documento">
                                            <HeaderStyle Width="120px" />
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="ocfu_vApellidoPaternoFuncionario" HeaderText="Primer Apellido">
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ocfu_vApellidoMaternoFuncionario" HeaderText="Segundo Apellido">
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ocfu_vNombreFuncionario" HeaderText="Nombres">
                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="vCargoFuncionario" HeaderText="Cargo" HeaderStyle-CssClass="ColumnaOculta">
                                            <HeaderStyle CssClass="ColumnaOculta" />
                                            <ItemStyle HorizontalAlign="Center" CssClass="ColumnaOculta" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="sGenero" HeaderText="sGenero" HeaderStyle-CssClass="ColumnaOculta">
                                            <HeaderStyle CssClass="ColumnaOculta" />
                                            <ItemStyle HorizontalAlign="Center" CssClass="ColumnaOculta" />
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnSeleccionar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                    CommandName="Select" ImageUrl="../Images/img_sel_check.png" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <uc1:ctrlPageBar ID="ctrlPaginador" runat="server" OnClick="MyUserControlPagingEvent_Click"
                                        Visible="False" />
                                </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
 <script type="text/javascript">

     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
     $(document).ready(function () {
         Load();
         $("#<%=txtNroDocumento.ClientID %>").numeric();

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

     function isNumero(evt) {

         var charCode = (evt.which) ? evt.which : event.keyCode

         var letra = false;
         if (charCode > 1 && charCode < 4) {
             letra = true;
         }

         if (charCode > 47 && charCode < 58) {
             letra = true;
         }


         var charpos = String.fromCharCode(charCode).search("[^A-Za-z]");

         if (charpos == -1) {
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

     function showpopupother(type_msg, title, msg, resize, height, width) {
         showdialog(type_msg, title, msg, resize, height, width);
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

     function isSujeto(evt) {
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

     function conMayusculas(field) {
         field.value = field.value.trim();
         field.value = field.value.toUpperCase()

     }
     function Ejecutar_Busqueda() {
         //Primero debes obtener el valor ascii de la tecla presionada 
         var key = window.event.keyCode;
         if (key == 13) {
             document.getElementById("<%= btn_Buscar.ClientID%>").click();
         }
     }
    </script>
</html>
