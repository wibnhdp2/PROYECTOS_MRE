<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmVideoRegistro.aspx.cs"
    Inherits="SGAC.WebApp.Colas.FrmVideRegistro" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagName="Validation"
    TagPrefix="Label" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(function () {
            $('#tabs').tabs();

            $("#datepicker1").datepicker();
            $('#datepicker1').datepicker('option', { dateFormat: 'dd/mm/yy' });

            $("#datepicker2").datepicker();
            $('#datepicker2').datepicker('option', { dateFormat: 'dd/mm/yy' });

            $("#datepicker3").datepicker();
            $('#datepicker3').datepicker('option', { dateFormat: 'dd/mm/yy' });
        });

    </script>
    <script type="text/javascript">
        function validatenumber(event) {
            return validatedecimal(event);
        }

        function showpopupother(typeMsg, title, msg, resize, height, width) {
            showdialog(typeMsg, title, msg, resize, height, width);
        }

        function conMayusculas(field) {
            field.value = field.value.toUpperCase();
        }

        function ValidarTextBox() {
            var bolValida = true;
            var strDesc = $.trim($("#<%= txtDescripcion.ClientID %>").val());
            var strUrl = $.trim($("#<%= txtNombre.ClientID %>").val());
            var strOrden = $.trim($("#<%= TXTORDEN.ClientID %>").val());
            var strValidacion = $.trim($("#<%= lblValidacion.ClientID %>").val());


            if (strDesc.length == 0) {
                document.getElementById('<%= txtDescripcion.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtDescripcion.ClientID %>').style.border = "1px solid #888888";
            }


            if (strUrl.length == 0) {
                document.getElementById('<%= txtNombre.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            } 
            else {
                if ((strUrl.substring(0, 4).toUpperCase() == "HTTP") && (strUrl.toUpperCase().indexOf("YOUTUBE") > -1)) {
                    document.getElementById('<%= txtNombre.ClientID %>').style.border = "1px solid #888888";
                }
                else {
                    if (comprueba_extension(strUrl) == 0) {
                        document.getElementById('<%= txtNombre.ClientID %>').style.border = "1px solid Red";
                        bolValida = false;
                    }
                    else {

                        document.getElementById('<%= txtNombre.ClientID %>').style.border = "1px solid #888888";
                    }
                }
            }

            if (strOrden.length == 0) {
                document.getElementById('<%= TXTORDEN.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= TXTORDEN.ClientID %>').style.border = "1px solid #888888";
            }

            /*MENSAJE DE CONFIRMACIÓN*/
            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }
            return bolValida;
        }

        function comprueba_extension(archivo) {
            extensiones_permitidas = new Array(".mpg", ".mpeg", ".mp4", ".MPG", ".MPEG", ".MP4");
            mierror = "";

            //recupero la extensión de este nombre de archivo 
            extension = (archivo.substring(archivo.lastIndexOf("."))).toLowerCase();

            permitida = false;
            for (var i = 0; i < extensiones_permitidas.length; i++) {
                if (extensiones_permitidas[i] == extension) {
                    permitida = true;
                    break;
                }
            }
            if (!permitida) {
                //mierror = "Comprueba la extensión de los archivos a subir. \nSólo se pueden subir archivos con extensiones: " + extensiones_permitidas.join();
                return 0;
            } else {

                return 1;
            }
        }

    </script>
  
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--Opciones--%>
    <table style="width: 90%; border-spacing: 0px;" align="center">
        <tr>
            <td>
                <h2>
                    MANTENIMIENTO DE VIDEOS</h2>
            </td>
        </tr>
    </table>
    <%--Opciones--%>
    <table style="width: 90%; border-spacing: 0px;" align="center">
        <tr>
            <td align="left">
                <div id="tabs">
                    <ul>
                        <li><a href="#tab-1">Consulta</a></li>
                        <li><a href="#tab-2">Registro</a></li>
                    </ul>
                    <div id="tab-1">
                        <asp:UpdatePanel runat="server" ID="updConsulta" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td width="130px">
                                            Oficina Consular:
                                        </td>
                                        <td>
                                            <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" Width="350px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <ToolBar:ToolBarContent ID="ctrlToolBar1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <Label:Validation ID="ctrlValidacionVideo" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="updGrillaConsulta" runat="server" UpdateMode="Conditional" OnInit="updGrillaConsulta_Init">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdVideos" runat="server" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False"
                                                        CssClass="mGrid" GridLines="None" OnRowCommand="grdVideos_RowCommand" OnSelectedIndexChanged="grdVideos_SelectedIndexChanged"
                                                        SelectedRowStyle-CssClass="slt" Width="870px" DataKeyNames="vide_sOficinaConsularId">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:BoundField DataField="vide_sVideoId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="Codigo"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vide_vDescripcion" HeaderText="Descripción">                                                                                                                                
                                                                <ItemStyle HorizontalAlign="Left" Width="390px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vide_vUrl" HeaderText="Nombre Video">
                                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vide_IOrden" HeaderText="N° Orden">
                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnConsultar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        CommandName="Consultar" ImageUrl="../Images/img_gridbuscar.gif" ToolTip="Consultar" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        CommandName="Editar" ImageUrl="../Images/img_grid_modify.png" ToolTip="Editar" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="slt" />
                                                    </asp:GridView>
                                                    &nbsp;
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click"
                                                Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-2">
                        <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <ToolBar:ToolBarContent ID="ctrlToolBarMantenimiento" runat="server"></ToolBar:ToolBarContent>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblValidacion" runat="server" Text="Falta validar algunos campos."
                                                CssClass="hideControl" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            Oficina Consular:
                                        </td>
                                        <td>
                                            <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular1" runat="server" Width="500px" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            Archivo de Video :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNombre" runat="server" Width="496px" MaxLength="50" 
                                                 onkeypress="return isLetraNumero(event)"></asp:TextBox>
                                            <asp:Label ID="Label1" runat="server" Text="(*.mpg, *.mpge, *.mp4, Url de Youtube)"></asp:Label>
                                            <asp:Label ID="lblUrlVal" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            Descripción Video :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescripcion" runat="server" Width="496px" onBlur="conMayusculas(this)"
                                                CssClass="txtLetra" onkeypress="return isLetraNumero(event)" MaxLength="50"></asp:TextBox>
                                            <asp:Label ID="lbldescripcionVal" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            N° Orden:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TXTORDEN" runat="server" Width="50px" onkeypress="return isNumberKey(event)"
                                                MaxLength="4"></asp:TextBox>
                                            <asp:Label ID="lblordenVal" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="lblHide"></asp:Label>
    <script type="text/javascript">

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

            if (charCode == 47) {
                letra = true;
            }
            if (charCode == 63) {
                letra = true;
            }
            if (charCode == 61) {
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

    </script>
</asp:Content>
