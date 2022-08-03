<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verificar.aspx.cs" Inherits="SolCARDIP_REGLINEA.Verificar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href='<%= VirtualPathUtility.ToAbsolute("~/Estilos/login/bootstrap.min.css")%>'rel="stylesheet" type="text/css" />
    <link href='<%= VirtualPathUtility.ToAbsolute("~/Estilos/login/fontawesome-all.min.css")%>'rel="stylesheet" type="text/css" />
    <link href='<%= VirtualPathUtility.ToAbsolute("~/Estilos/login/iofrm-style.css")%>'rel="stylesheet" type="text/css" />
    <link href='<%= VirtualPathUtility.ToAbsolute("~/Estilos/login/iofrm-theme9.css")%>'rel="stylesheet" type="text/css" />
    <link href='<%= VirtualPathUtility.ToAbsolute("~/Estilos/Alert/sweetalert.css")%>'rel="stylesheet" type="text/css" />
    <script src='<%= VirtualPathUtility.ToAbsolute("~/Scripts/Alert/sweetalert-dev.js")%>'type="text/javascript"></script>
    
    <script src='<%= VirtualPathUtility.ToAbsolute("~/Scripts/jquery.min.js")%>'type="text/javascript"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

     <script>
         function navegador() {
             var agente = window.navigator.userAgent;
             var navegadores = ["Chrome", "Firefox", "Safari", "Opera", "Trident", "MSIE", "Edge"];
             for (var i in navegadores) {
                 if (agente.indexOf(navegadores[i]) != -1) {
                     return navegadores[i];
                 }
             }
         }
         var browser = document.getElementById("navegador");
         window.onload = function () {
             if (navegador() == "Trident" || navegador() == "MSIE") {
                 alert('UTILIZAR INTERNET EXPLORER PUEDE CAUSAR QUE ALGUNAS FUNCIONALIDADES NO SE EJECUTEN CORRECTAMENTE');
             }
         }
     </script>
    <script type="text/javascript">
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
            //Tecla de espacio, siempre la permite
            if (tecla == 32) {
                return true;
            }
            // Patron de entrada, en este caso solo acepta letras
            patron = /[A-Za-z áéíóú ÁÉÍÓÚ Üü]/;
            tecla_final = String.fromCharCode(tecla);
            return patron.test(tecla_final);
        }
    </script>
</head>
<body>
    <div id="form1" runat="server" class="form-body">
        <div class="row">
            <div class="img-holder">
                <div class="bg"></div>
                <div class="info-holder">
                    <h3>Verificación de Carnet</h3>
                    <p>Si usted, cuenta con la siguiente calidad migratoria Residente: Diplomático, Oficial, Consular, Familiar de Oficial, 
                        Cooperante, Intercambio, Periodista o Producción Artística, puede solicitar su carnet de identidad</p>
                    <%--<img src="../images/logo_n_2016.png" alt="">--%>
                </div>
            </div>
            <div class="form-holder">
                <div class="form-content">
                    <div class="form-items" style="max-width: 440px">
                        <div class="website-logo-inside">
                            <a href="https://www.gob.pe/rree">
                                <div>
                                    <img class="logo-size" src="Imagenes/logo_n_2016.png" alt=""/>
                                </div>
                            </a>
                        </div>
                        <div class="page-links">
                            <a href="#" class="active">Datos del ciudadano</a>
                        </div>
                        <form id="form2" runat="server">
                            <div class="row">
<%--                                <div class="col">
                                    <asp:DropDownList ID="ddlTipoDocumento" data-toggle="tooltip" data-placement="top" title="Tipo Documento" CssClass="form-control" placeholder="Tipo Documento" required="" runat="server" onchange="CantidadDigitos();"></asp:DropDownList>
                                </div>--%>
                                <div class="col">
                                    <div class="input-group mb-3">
                                        <asp:TextBox ID="txtNumDocumento" data-toggle="tooltip" data-placement="top" title="Nro de Documento" Style="text-transform: uppercase;" CssClass="form-control" placeholder="Ingrese número de pasaporte" required="" runat="server" onkeypress="return AlfaNumerico(event)" MaxLength="12" minlength="7"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <asp:TextBox ID="txtApePaterno" data-toggle="tooltip" data-placement="top" title="Apellido Paterno" Style="text-transform: uppercase;" CssClass="form-control" placeholder="Apellido Paterno" required="" runat="server" onkeypress="return letras(event)"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <asp:TextBox ID="txtApeMaterno" data-toggle="tooltip" data-placement="top" title="Apellido Materno" Style="text-transform: uppercase;" CssClass="form-control" placeholder="Apellido Materno" runat="server" onkeypress="return letras(event)"></asp:TextBox>
                                </div>
                            </div>
                            

                            <div class="row">
                                <div class="col">
                                    <asp:TextBox ID="txtNombres" data-toggle="tooltip" data-placement="top" title="Nombres" Style="text-transform: uppercase;" CssClass="form-control" placeholder="Nombres" required="" runat="server" onkeypress="return letras(event)"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col">
                                    <asp:TextBox ID="txtFecNac" data-toggle="tooltip" data-placement="top" title="Fecha de Nacimiento" Style="text-transform: uppercase;" CssClass="form-control" placeholder="Fecha de Nacimiento" MaxLength="10" required="" type="text" runat="server" onfocus="(this.type='date')" onblur="(this.type='text')"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-button">
                                <asp:Button ID="btnVerificar" CssClass="ibtn" runat="server" Text="Verificar" 
                                    Width="120px" OnClick="btnVerificar_Click" />
                            </div>

                        </form>
                        <div class="other-links">
                            <span>Derechos Reservados </span><a href="https://www.gob.pe/rree" target="_blank">Ministerio de Relaciones Exteriores</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade " id="modalMsg">
        <div class="modal-dialog modal-sm">
            <div class="modal-content overflow-auto">
                <div class="modal-header">
                    <h5 class="modal-title" id="H41"></h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body  m-2" id="modalbodyMsg">
                   
                </div>
                <div class="modal-footer align-content-center">
                    <div class="btn btn-primary-wizard " data-dismiss="modal">
                        Cerrar
                    </div>
                </div>
            </div>
        </div>
    </div>

</body>
</html>
