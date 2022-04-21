<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SolCARDIP.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="PRAGMA" content="NO-CACHE"/>
<meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate"/>
<meta http-equiv="Expires" content="0"/>
<base href="Login.aspx" target="_self" />
<link rel="SHORTCUT ICON" href="Images/Iconos/escudo.ico" />
<title>Login</title>
<script type="text/javascript">
    if (history.forward(1)) { location.replace(history.forward(1)) }
    </script>
    <script type="text/javascript">
        function desLbl() {
            var lbl1 = document.getElementById("lblMensajeCaptcha");
            if (lbl1 != null) {
                setTimeout(function () { lbl1.style.display = "none"; }, 5000);
            }
        }
    </script>
    <script type="text/javascript">
        function onSubmit(token) {
            //alert("thanks" + document.getElementById("field").value);
            document.getElementById("ibtPostBack").click();
        }

        function validate(event) {
            event.preventDefault();
            grecaptcha.execute();
        }

        function onload() {
            var element = document.getElementById("btnLoginUsuario")
            element.onclick = validate;
        }
    </script>
    <script type="text/javascript" src="Scripts/Script.js"></script>
    <script src="https://www.google.com/recaptcha/api.js" async defer></script>
    <style type="text/css">
        .BordeEtiqueta
        {
            border-width:1px;
            border-color:Black;
            border-style:solid;
        }
        .etiqueta
        {
            font-family  :Trebuchet MS;
            font-style:normal;
            font-size:12px;
            font-weight:bold;
            text-align:left;  
            margin-left: 80px;
        }
        .EtiquetaCentro
        {
          font-family  :Trebuchet MS;
          font-style:normal;
          font-size:12px;
          font-weight:bold;
          text-align:center;  
          margin-left: 80px;
        }
        .style1
        {
            height: 60px;
            width: 325px;
        }
        .style2
        {
            height: 10%;
            width: 325px;
        }
        .style3
        {
            height: 40%;
            width: 325px;
        }
        .style4
        {
            height: 7%;
            width: 325px;
        }
        .botonMarron
    {}
    .style6
    {
        width: 325px;
    }
    .style8
    {
        font-size: 18px;
        text-align: center;
        font-weight: bold;
        width: 286px;
        height: 49px;
    }
    .style9
    {
        width: 286px;
    }
    </style>
    <script type="text/javascript">
        function RefreshPage() {
            window.document.getElementById("RebindFlagHiddenField").value = "1";
            window.document.getElementById("RebindFlagSpan").firstChild.Value = "1";
            window.document.forms(0).submit();
        }

    </script>
    <%--ESTLOS NUEVO LOGIN--%>
    <style type="text/css">
    .divLogin
    {
	    border: 1px solid #cccccc;
	    /*border-radius: 25px;*/
	    width: 380px;
	    height: 370px;
	    box-shadow: 10px 10px 5px #888888;
	    background-color: #f7f7f7 /*#dfdfdf*/;
	    padding-left: 70px; 
    }
    .txtLogin
    {
	    text-transform:uppercase;
	    height: 21px;
	    width: 200px;
	    border: 1px solid #888888;
	    border-top-left-radius: 5px;
	    border-top-right-radius: 5px;
	    border-bottom-left-radius: 5px;
	    border-bottom-right-radius: 5px;    
	    padding-left:5px; 
    }
    .txtLogin:focus
    {
	    text-transform:uppercase;
	    height: 21px;
	    width: 200px;
	    border: 1px solid #888888;
	    border-top-left-radius: 5px;
	    border-top-right-radius: 5px;
	    border-bottom-left-radius: 5px;
	    border-bottom-right-radius: 5px;    
	    background-color: #f8e97e;
	    padding-left:5px; 
    }
    .btnLogin
    {    
	    -moz-box-shadow:inset 0px 0px 2px 0px #ffffff;
	    -webkit-appearance: none;    
	    -webkit-box-shadow:inset 0px 0px 2px 0px #ffffff;
	    box-shadow:inset 0px 0px 2px 0px #ffffff;
	    background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
	    background:-moz-linear-gradient( center top, #ededed 5%, #dfdfdf 100% );
	    filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	    background-color:#ededed;
	    -webkit-border-top-left-radius:5px;
	    -moz-border-radius-topleft:5px;
	    border-top-left-radius:5px;
	    -webkit-border-top-right-radius:5px;
	    -moz-border-radius-topright:5px;
	    border-top-right-radius:5px;
	    -webkit-border-bottom-right-radius:5px;
	    -moz-border-radius-bottomright:5px;
	    border-bottom-right-radius:5px;
	    -webkit-border-bottom-left-radius:5px;
	    -moz-border-radius-bottomleft:5px;
	    border-bottom-left-radius:5px;
	    text-indent:0;
	    text-decoration:none;
	    text-align:center;
	    border:1px solid #dcdcdc;
	    height:26px;
	    width: 205px;
	    border-style:outset;
	    margin-top: 5px;   	
	    cursor:pointer;
    }
    .btnLogin:hover 
    {    
	    background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
	    background:-moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
	    filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
	    background-color:#cccccc;
	    cursor: pointer;
    }
    html, body
    {
        height:100%;
        margin:0;
    }
    #divPri
    {
        height: 100%; width: 100%; background-color:Gray;
    }
    </style>
</head>
<body>
    <div id="divPri">
        <form id="form1" runat="server" action="?" method="post" autocomplete="off">
        <table style="width:100%;">
            <tr>
                <td align="center" style="vertical-align:middle;">
                    <table style="width:50%;height:600px;background-color:White;" align="center">
                        <tr>
                            <td>
                                <table style="width:47%;" align="center">
                                    <tr>
                                        <td style="background-color:White;" align="center">
                                            <img src="Imagenes/Logos/rree_membrete.gif" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-family:Trebuchet MS;font-style:normal;font-size:16px;font-weight:bold;padding-bottom:10px;padding-top:20px;background-color:White;" align="center">
                                            Sistema de Emisión de Carnés para Extranjeros en el Perú
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <div class="divLogin">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td colspan="2" align="center" style="padding:25px">
                                                            <asp:Image ID="Image1" runat="server" CssClass="imgLogin" ImageUrl="Imagenes/Iconos/img_48_user_gray.png" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="font-family:Trebuchet MS;font-style:normal;font-size:12px;font-weight:bold;text-align:left;margin-left: 80px;">Alias</td>
                                                        <td>
                                                            <asp:TextBox ID="txtLogin" runat="server" CssClass="txtLogin"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="font-family:Trebuchet MS;font-style:normal;font-size:12px;font-weight:bold;text-align:left;margin-left: 80px;">Contraseña</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPassword" runat="server" CssClass="txtLogin" TextMode="Password"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td colspan="2">
                                                            <div class="g-recaptcha" data-sitekey="6LdwGy0UAAAAAE4Av7LlEu2JTEOVSl2Xba1uNIBo" data-theme="dark"></div>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                            <img id="imgCaptcha" width="150" height="50" enableviewstate="false" alt="" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center" class="EtiquetaCentro">
                                                            <asp:LinkButton ID="lbnActualizarCaptcha" Text="Actualizar Codigo Captcha" runat="server" OnClick="lbnActualizarCaptcha_Click"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                            <asp:TextBox ID="txtCodCaptcha" runat="server" CssClass="input" Width="70%" style="text-align:center; text-transform: uppercase;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="padding-top:20px;padding-bottom:20px;" align="center">
                                                            <asp:Label ID="lblMensajeCaptcha" runat="server" Font-Names="sans-serif" Font-Size="12px" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="padding-top:10px;" align="center">
                                                            <asp:Button ID="btnLoginUsuario" runat="server" Text="Iniciar sesión" CssClass="btnLogin" TabIndex="4" OnClick="btnLoginUsuario_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center" id="footer" class="EtiquetaCentro" style="font-family:Trebuchet MS;font-style:normal;font-size:12px;font-weight:bold;text-align:center;margin-left: 80px;">Fecha de actualización: &nbsp;
                                                            <asp:Label ID="lblfechaUpdate" runat="server" Text="04/06/2014"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center" id="Td1" class="EtiquetaCentro" style="font-family:Trebuchet MS;font-style:normal;font-size:12px;font-weight:bold;text-align:center;margin-left: 80px;">&nbsp;
                                                            <asp:Label ID="lblVersion" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color:#480b20;font-family:Trebuchet MS;font-style:normal;font-size:12px;font-weight:bold;color:White;text-align:center;">
                                Derechos Reservados <a href="http://www.rree.gob.pe/" target="_blank">Ministerio de Relaciones Exteriores</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
    </div>
    <%--<form id="form1" runat="server" action="?" method="post">
     <table class="BordeEtiqueta" id="table-border_login" border="0" cellpadding="0" style=" height:400px; width:15%;border-width:1px;border-color:Black;border-style:solid;" cellspacing="0" >
       <tr>
          <td valign="top"  align="center" class="style1"> 
              <img src="Imagenes/Logos/rree_membrete.gif" />
           </td>
       </tr>

       <tr>
        <td  align="center" class="style2" style="width: 100%" > 
            <table style="width: 100%">
                <tr>
                    <td class="EtiquetaCentro" style="width:100%;font-family:Trebuchet MS;font-style:normal;font-size:12px;font-weight:bold;text-align:center;margin-left: 80px;"> 
                        <asp:Label ID="Label1"  runat="server"  Text="Sistema de Envio Virtual de Documentos v2.0" Font-Size="Medium"></asp:Label>
                    </td>
                 </tr>

                
                   
                    <tr><td class="EtiquetaCentro" style="font-family  :Trebuchet MS;font-style:normal;font-size:12px;font-weight:bold;text-align:center;margin-left: 80px;"> Introduzca su contraseña respectiva y luego haga click en "Ingresar".</td></tr>
             </table> 
         </td>
       </tr>
           
      <tr>
       <td  align="center" valign="middle" class="etiqueta">
     <table style="width: 100%;" align="center">
      <tr>
        <td align="right" class="etiqueta" style="font-family:Trebuchet MS;font-style:normal;font-size:12px;font-weight:bold;text-align:left;margin-left: 80px;">
            <asp:Label ID="Label2" runat="server" Text="Usuario NT:"></asp:Label>
        </td>
        <td class="style6">
            <asp:TextBox ID="txtLogin" runat="server"  CssClass="input" onpaste="return false" oncut="return false" oncopy="return false"
                    onblur="this.className='input'" onFocus="this.className='inputFocus'" 
                        onmouseout="this.className='input'" onmouseover="this.className='mouseOver'" 
                        onKeyDown="if(event.keyCode==13) event.keyCode=9;" Width="200px" MaxLength="30" Font-Names="Arial"></asp:TextBox>
        </td>
      </tr>
      <tr>
        <td align="right" class="etiqueta" style="font-family:Trebuchet MS;font-style:normal;font-size:12px;font-weight:bold;text-align:left;margin-left: 80px;">
            <asp:Label ID="Label3" runat="server" Text="Contraseña:" CssClass="etiquetaRoja"></asp:Label>
        </td>
        <td class="style1">
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="input" onpaste="return false" oncut="return false" oncopy="return false" AutoComplete="off"
                        onblur="this.className='input'" onFocus="this.className='inputFocus'" 
                        onmouseout="this.className='input'" onmouseover="this.className='mouseOver'"
                        TextMode="Password" 
                        onKeyDown="if(event.keyCode==13) RefreshPage(); " Width="200px" MaxLength="30"></asp:TextBox>           
        </td>
      </tr>
      <tr style="display:none;">
        <td colspan="2" align="center">
            <img id="imgCaptcha" width="150" height="50" enableviewstate="false" alt="" runat="server" />
        </td>
      </tr>
      <tr style="display:none;">
        <td colspan="2" align="center">
            <asp:LinkButton ID="lbnActualizarCaptcha" Text="Actualizar Codigo Captcha" runat="server" OnClick="lbnActualizarCaptcha_Click"/>
        </td>
      </tr>
      <tr style="display:none;">
        <td colspan="2" align="center">
            <asp:TextBox ID="txtCodCaptcha" runat="server" CssClass="input" Width="70%" style="text-align:center;"></asp:TextBox>
        </td>
      </tr>
      <tr style="display:none;">
        <td>
            <asp:ImageButton ID="ibtPostBack" runat="server" ImageUrl="~/Imagenes/Botones/BIngresar_off.gif" onclick="btnLoginUsuario_Click" Visible="true"/>
        </td>
      </tr>
      <tr>
        <td colspan="2" align="center">
            <div class="g-recaptcha"
                  data-sitekey="6LcGkC0UAAAAAOVxflOfVKrFA4T2pWXo5CyrX9y1"
                  data-callback="onSubmit"
                  data-size="invisible">
            </div>
            <button id="submit" style="display:none;">submit</button>
            <asp:Label ID="lblMensajeCaptcha" runat="server" Font-Names="sans-serif" Font-Size="11px" ForeColor="Red"></asp:Label>
        </td>
      </tr>
      <tr>
        <td colspan="2" class="style2" align="right">
            <asp:ImageButton ID="btnLoginUsuario" runat="server"  
                ImageUrl="~/Imagenes/Botones/BIngresar_off.gif" onclick="btnLoginUsuario_Click"/>
        </td>
     </tr>
</table>
</td>
</tr>
<tr align="center">
  <td align="center" id="footer" class="EtiquetaCentro" style="font-family:Trebuchet MS;font-style:normal;font-size:12px;font-weight:bold;text-align:center;margin-left: 80px;">Fecha de actualización: &nbsp;
  <asp:Label ID="lblfechaUpdate" runat="server" Text="04/06/2014"></asp:Label>
   </td>
        </tr>
 

        <tr>
        <td align="center" id="footer" class="EtiquetaCentro" style="font-family:Trebuchet MS;font-style:normal;font-size:12px;font-weight:bold;text-align:center;margin-left: 80px;">Ministerio de Relaciones Exteriores - MRE&nbsp;  &copy; 2014
          </td>
        </tr>
    </table>

 <span id="RebindFlagSpan">
            <asp:HiddenField ID="RebindFlagHiddenField" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCodigo" runat="server" Visible="true" />
        </span>

  </form>--%>
  <%--  <script>      onload();</script>--%>
</body>
</html>
