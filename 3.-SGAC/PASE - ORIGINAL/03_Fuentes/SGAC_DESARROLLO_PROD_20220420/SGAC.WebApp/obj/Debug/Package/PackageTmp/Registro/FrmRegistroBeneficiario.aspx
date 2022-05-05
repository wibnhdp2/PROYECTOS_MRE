<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmRegistroBeneficiario.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmRegistroBeneficiario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>

    <style type="text/css">
        body
        {
            background-color:White;
            font-size: 12px;
	        font-family: Arial;
	        margin: 0px;
	        padding: 0px;
	        color: #696969;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdn_actu_iPersonaId" runat="server" value="0"/>
            <asp:HiddenField ID="hidDocumentoSoloNumero" runat="server" value="0"/>
            <asp:HiddenField ID="HF_ValoresDocumentoIdentidad" runat="server" value="0"/>
            <table style="margin-top:15px; margin-left:30px; width:90%;">
                <tr>
                    <td width="120px">
                        <asp:Label ID="lblTipoDocumento" runat="server" Text="Tipo Documento:"></asp:Label>
                    </td>
                    <td width="220px">
                        <asp:DropDownList ID="ddl_TipoDocumento" runat="server" Height="21px" Width="204px"
                            AutoPostBack="True" OnSelectedIndexChanged="ddl_TipoDocumento_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td width="120px">
                        <asp:Label ID="lblNumeroDoc" runat="server" Text="Nro. de Documento:"></asp:Label>
                    </td>
                    <td width="210px" align="right">
                        <asp:TextBox ID="txtDocumento" runat="server" CssClass="campoNumero" onkeypress="return isCantidadTipoDocumento(event)"
                            Width="180px" MaxLength="20" onblur="PresionarBotonBuscar();"
                            ></asp:TextBox>
                        <asp:ImageButton ID="imgBuscar" ImageUrl="~/Images/img_16_search.png" runat="server"
                            OnClick="imgBuscar_Click" style="width: 16px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNombreBene" runat="server" Text="Nombre:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombreBene" runat="server" CssClass="txtLetra" onkeypress="return isNombreApellido(event)"
                            onBlur="conMayusculas(this)" Width="200px" Enabled="false" />
                    </td>
                    <td>
                        &nbsp;</td>
                    <td align="right">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblApellPatBene" runat="server" Text="Primer Apellido:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtApellidoPatBene" runat="server" CssClass="txtLetra" 
                            MaxLength="20" onBlur="conMayusculas(this)" Enabled="false"
                            onkeypress="return isNombreApellido(event)" Width="200px" />
                    </td>
                    <td>
                        <asp:Label ID="Label14" runat="server" Text="Segundo Apellido:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtApellidoMatBene" runat="server" CssClass="txtLetra" Enabled="false"
                            onBlur="conMayusculas(this)" onkeypress="return isNombreApellido(event)" 
                            Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblGenero" runat="server" Text="Género:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Genero" runat="server" 
                            Width="170px" Enabled="False">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar" OnClick="btnAdicionar_Click"
                            Width="90px" CssClass="btnGeneral" OnClientClick="return ValidarBeneficiario()" />
                        <asp:Button ID="btnLimpiar" runat="server" OnClick="btnLimpiar_Click" Text="Limpiar"
                            Width="90px" CssClass="btnGeneral" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <script type="text/javascript">

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }


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

        function conMayusculas(control) {
            control.value = control.value.toUpperCase();
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
        function ValidarBeneficiario() {
            var bolValida = true;

            var nroDoc = $("#<%=txtDocumento.ClientID %>");
            var nroTipoDoc = $("#<%=ddl_TipoDocumento.ClientID %>");


            if (ddlcontrolError(document.getElementById("<%=txtDocumento.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=ddl_TipoDocumento.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtNombreBene.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtApellidoPatBene.ClientID %>")) == false) bolValida = false;

            if (ddlcontrolError(document.getElementById("<%=ddl_Genero.ClientID %>")) == false) bolValida = false;

            var valoresDocumento = ObtenerMaxLenghtDocumentos(nroTipoDoc.val()).split(",");

            if (bolValida) {
                if (valoresDocumento[0] > nroDoc.val().length || valoresDocumento[1] < nroDoc.val().length) {
                    nroDoc.css("border", "1px solid Red");
                    bolValida = false;
                }
                else {
                    nroDoc.css("border", "1px solid #888888");
                }
            }



            return bolValida;

        }

        function ObtenerMaxLenghtDocumentos(id) {

            var bEsCui = false;

            if (id == 6) {
                id = 1;
                bEsCui = true;

            }


            var hfDocumentoIdentidad = $("#<%=HF_ValoresDocumentoIdentidad.ClientID %>").val();

            var documentos = hfDocumentoIdentidad.split("|");

            for (i = 0; i < documentos.length - 1; i++) {

                var valores = documentos[i].split(",");

                if (bEsCui)
                    valores[5] = valores[5].replace('DNI', 'CUI');

                if (valores[0] == id) {
                    return valores[1] + "," + valores[2] + "," + valores[3] + "," + valores[5] + "," + valores[4];
                }

            }

            return "";



        }

        function PresionarBotonBuscar() {

            $("#<%=imgBuscar.ClientID %>").click();

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

        function isCantidadTipoDocumento(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;

            var codDocumento = $("#<%= ddl_TipoDocumento.ClientID %>").val();

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

        function Load() {
            $("#<%=txtDocumento.ClientID %>").bind("keypress", function (e) {
                if (e.keyCode == 13) {
                    document.getElementById("<%=imgBuscar.ClientID %>").click();
                    e.preventDefault();
                }
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        $(document).ready(function () {
            Load();
        });
    </script>
</body>
</html>
