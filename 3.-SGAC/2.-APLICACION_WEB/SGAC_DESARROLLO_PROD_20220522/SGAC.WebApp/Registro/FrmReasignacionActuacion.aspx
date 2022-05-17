<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmReasignacionActuacion.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmActuacionReasignar" %>

<%@ Register Src="../Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar" TagPrefix="uc3" %>

 

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <style type="text/css">        
        
        .tMsjeWarnig
        {
            background-color: #F2F1C2;
            border-color: Yellow; /*#6E4E1B;*/
            color: #4B4F5E;
            height: 15px;
            background-image: url('../../Images/img_16_warning.png');
            background-repeat: no-repeat;
            background-position: 8px 2px;
            width: 100%;
        }
        
        .lblMsjeWarnig
        {
            margin-left: 25px;
        }
        
        .tMsjeError
        {
            background-color: #FE2E2E;
            border-color: Red; /*#6E4E1B;*/
            color: #FFFFFF;
            height: 15px;
            background-image: url('../../Images/img_16_error.png');
            background-repeat: no-repeat;
            background-position: 8px 2px;
            width: 100%;
        }
        
        .lblMsjeError
        {
            margin-left: 25px;
        }
        
        .tMsjeSucess
        {
            background-color: #2E9AFE;
            border-color: Blue; /*#6E4E1B;*/
            color: #FFFFFF;
            height: 15px;
            background-image: url('../../Images/img_16_success.png');
            background-repeat: no-repeat;
            background-position: 8px 2px;
            width: 100%;
        }
        
        .lblMsjeSucess
        {
            margin-left: 25px;
        }
        
        .AlineadoDerecha
        {
            text-align: right;
        }
        
        .Oculto
        {
            display: none;
        }
        
    </style>    

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HFGUID" runat="server" /> 
    <asp:HiddenField ID="hOfiConsularID" runat="server" />
    <div>
            
        <%--Titulo--%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server" Text="Reasignación de Actuaciones" /></h2>
                </td>
            </tr>
        </table>

        <br />

        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                     <asp:LinkButton ID="Tramite" runat="server" PostBackUrl="~/Registro/FrmTramite.aspx"
                        Font-Bold="True" Font-Size="10pt" ForeColor="Gray">Regresar a Trámites</asp:LinkButton>
                </td>
            </tr>
        </table>

        
        <%--Cuerpo--%>
        <table class="mTblPrincipal" align="center" style="width: 90%;">
            <tr>
                <td bgcolor="#4E102E">
                    <asp:Label ID="lblDestino" runat="server" Font-Bold="True" 
                               Font-Names="Arial" Font-Size="12pt"  
                               ForeColor="White" Text="Actuación" 
                               Width="202px"></asp:Label>
                </td>
            </tr>
        </table>

       

        <table class="mTblPrincipal" align="center">

            <tr>
                
                <td>
                     <asp:Label ID="lblNroDocL" runat="server" Text="Nro Documento: "></asp:Label>
                </td>

                <td align="left">
                     <asp:Label ID="lblNroDocD" runat="server" Text="09610111" Font-Bold="True"></asp:Label>
                </td>

                <td>
                
                </td>
                
                <td>
                    <asp:Label ID="lblPrimerApellidoL" runat="server" Text="Primer Apellido: " 
                        Font-Bold="False"></asp:Label>
                </td>

                <td align="left">
                    <asp:Label ID="lblPrimerApellidoD" runat="server" Text="GUINET" 
                        Font-Bold="True"></asp:Label>
                </td>

                <td>
                
                </td>

                <td>
                    <asp:Label ID="lblSegundoApellidoL" runat="server" Text="Segundo Apellido: " 
                        Font-Bold="False"></asp:Label>
                </td>

                <td align="left">
                    <asp:Label ID="lblSegundoApellidoD" runat="server" Text="HUERTA" 
                        Font-Bold="True"></asp:Label>
                </td>

                <td>
                
                </td>

                <td>
                    <asp:Label ID="lblNombresL" runat="server" Text="Nombres: "></asp:Label>
                </td>

                <td align="left">
                    <asp:Label ID="lblNombresD" runat="server" Text="SANDRO ALDO" Font-Bold="True"></asp:Label>
                </td>            
            </tr>

            <tr>
                <td colspan="11">
                
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblNroActL" runat="server" Text="Nro Actuación: "></asp:Label>
                </td>

                <td>
                    <asp:Label ID="lblNroActD" runat="server" Text="100" Font-Bold="True"></asp:Label>
                </td>

                <td>
                
                </td>

                <td>                
                    <asp:Label ID="lblRGEL" runat="server" Text="R.G.E:"></asp:Label>                
                </td>

                <td>
                    <asp:Label ID="lblRGED" runat="server" Text="15" Font-Bold="True"></asp:Label>
                </td>

                <td>
                  
                </td>

                <td>
                    <asp:Label ID="lblCorrTariL" runat="server" Text="Correlativo Tarifa: "></asp:Label>
                </td>

                <td>                
                    <asp:Label ID="lblCorrTariD" runat="server" Text="1" Font-Bold="True"></asp:Label>                
                </td>

                <td>
                
                </td>

                <td>
                    <asp:Label ID="lblFechaL" runat="server" Text="Fecha: "></asp:Label>
                </td>
                
                <td align="left">
                    <asp:Label ID="lblFechaD" runat="server" Text="28/11/2014" Font-Bold="True"></asp:Label>
                </td>

            </tr>

            <tr>
                <td colspan="11">
                
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblDescripcionL" runat="server" Text="Descripción: "></asp:Label>
                </td>

                <td colspan="10" align="left">
                    <asp:Label ID="lblDescripcionD" runat="server" 
                        Text="b) Cualquier certificación notarial para que no haya disposición especial" 
                        Font-Bold="True"></asp:Label>
                </td>
            
            </tr>

        </table>

        <br />

        <table class="mTblPrincipal" align="center" style="width: 90%;">
            <tr>
                <td bgcolor="#4E102E">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" 
                               Font-Names="Arial" Font-Size="12pt" 
                               ForeColor="White" Text="Búsqueda (Reasignar a:)" 
                               Width="250px"></asp:Label>
                </td>
            </tr>

        </table> 
        
         <asp:UpdatePanel ID="updBusqRecurrente" UpdateMode="Conditional" runat="server">
            <ContentTemplate>       
        
        <table class="mTblPrincipal" align="center">

            <tr>
                <td>
                        
                    <asp:Panel ID="panSearch" runat="server" DefaultButton="btnBuscarR" >

                        <table>
                            
                            <tr>
                                
                                <td style="height: 50px; font-weight: inherit;">
                                    <font>
                                        <asp:Label ID="lblNroDocumento" runat="server" Text="Nro Documento:" Width="95px"></asp:Label>
                                    </font>
                                </td>

                                <td style="width: 501px; height: 50px;">
                                    <asp:TextBox ID="txtNroDocumento" 
                                                    runat="server" 
                                                    Width="101px" 
                                                    AutoPostBack="true"                                 
                                                    MaxLength="20" ontextchanged="txtNroDocumento_TextChanged" />
                                </td>

                                <td style="height: 50px; font-weight: inherit;">
                                    <font>
                                        <asp:Label ID="lblPriApellido" runat="server" Text="Primer Apellido:" Width="95px"></asp:Label>
                                    </font>
                                </td>

                                <td style="width: 501px; height: 50px;">

                                    <asp:TextBox ID="txtPriApellido" 
                                                    runat="server" 
                                                    Width="101px" 
                                                    AutoPostBack="true"
                                                    ToolTip="Para poder realizar la búsqueda debe ingresar como minimo 3 caracteres"                                 
                                                    CssClass="txtLetra" 
                                                    MaxLength="50"
                                                    ontextchanged="txtPriApellido_TextChanged" 
                                                    onkeypress="return ValidarSujeto(event)"/>
                                </td>

                                <td style="height: 50px; font-weight: inherit;">
                                    <font>
                                        <asp:Label ID="lblSegApellido" 
                                                    runat="server" 
                                                    Text="Segundo Apellido:" 
                                                    Width="100px"></asp:Label>
                                    </font>
                                </td>
                                <td style="width: 501px; height: 50px;">
                                    <asp:TextBox ID="txtSegApellido" 
                                                    runat="server" 
                                                    Width="101px" 
                                                    AutoPostBack="true"
                                                    ToolTip="Para poder realizar la búsqueda debe ingresar como minimo 3 caracteres"                                 
                                                    CssClass="txtLetra" 
                                                    MaxLength="50"
                                                    onkeypress="return ValidarSujeto(event)" 
                                        ontextchanged="txtSegApellido_TextChanged" />
                                </td>                            
                            </tr>
                        
                        </table>
                        
                    </asp:Panel>
                </td>

                <td style="width: 101px; height: 50px;">
                        
                        <asp:Button ID="btnBuscarR" 
                                runat="server" 
                                Text="  Buscar" 
                                CssClass="btnSearch"                                 
                                UseSubmitBehavior="False" onclick="btnBuscarR_Click" />
                </td>
            </tr>

        </table>        

        <table class="mTblPrincipal" align="center">               

            <tr>

                <td runat="server" id="msjeWarningPersFN" colspan="8">

                    <table id="tMsjeWarnigPersFN" class="tMsjeWarnig" align="center">

                        <tr>
                            <td>
                                <asp:Label ID="lblMsjeWarnigPersFN" runat="server" Text="" CssClass="lblMsjeWarnig" />
                            </td>

                        </tr>

                    </table>

                </td>

            </tr>

            <tr>

                <td colspan="8">
                    <div>
                    <%--Grd_Solicitante_SelectedIndexChanged--%>
                        <asp:GridView ID="Grd_Solicitante" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                            runat="server" DataKeyNames="vNombre,vNroDocumento,vFecNac" AutoGenerateColumns="False"
                            GridLines="None" 
                            onrowdatabound="Grd_Solicitante_RowDataBound" 
                            onrowcommand="Grd_Solicitante_RowCommand" >
                            <AlternatingRowStyle CssClass="alt" />
                            <Columns>
                                <asp:BoundField DataField="iPersonaId" HeaderText="TipDoc" HeaderStyle-CssClass="ColumnaOculta"
                                    ItemStyle-CssClass="ColumnaOculta" >
                                    <HeaderStyle CssClass="ColumnaOculta" />
                                    <ItemStyle CssClass="ColumnaOculta" />
                                </asp:BoundField>
                                <asp:BoundField DataField="sPersonaTipoId" HeaderText="TipPers" HeaderStyle-CssClass="ColumnaOculta"
                                    ItemStyle-CssClass="ColumnaOculta" >
                                    <HeaderStyle CssClass="ColumnaOculta" />
                                    <ItemStyle CssClass="ColumnaOculta" />
                                </asp:BoundField>
                                <asp:BoundField DataField="sDocumentoTipoId" HeaderText="TipDoc" HeaderStyle-CssClass="ColumnaOculta"
                                    ItemStyle-CssClass="ColumnaOculta" >
                                    <HeaderStyle CssClass="ColumnaOculta" />
                                    <ItemStyle CssClass="ColumnaOculta" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vDescTipDoc" HeaderText="DescTipDoc" HeaderStyle-CssClass="ColumnaOculta"
                                    ItemStyle-CssClass="ColumnaOculta" >
                                    <HeaderStyle CssClass="ColumnaOculta" />
                                    <ItemStyle CssClass="ColumnaOculta" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vApellidoPaterno" HeaderText="ApePat" HeaderStyle-CssClass="ColumnaOculta"
                                    ItemStyle-CssClass="ColumnaOculta" >
                                    <HeaderStyle CssClass="ColumnaOculta" />
                                    <ItemStyle CssClass="ColumnaOculta" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vApellidoMaterno" HeaderText="ApeMat" HeaderStyle-CssClass="ColumnaOculta"
                                    ItemStyle-CssClass="ColumnaOculta" >
                                    <HeaderStyle CssClass="ColumnaOculta" />
                                    <ItemStyle CssClass="ColumnaOculta" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vNombres" HeaderText="Nombres" HeaderStyle-CssClass="ColumnaOculta"
                                    ItemStyle-CssClass="ColumnaOculta" >
                                    <HeaderStyle CssClass="ColumnaOculta" />
                                    <ItemStyle CssClass="ColumnaOculta" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vNombre" HeaderText="Apellidos y Nombres" />
                                <asp:BoundField DataField="vNroDocumento" HeaderText="Nro Documento" >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vFecNac" HeaderText="Fecha Nacimiento" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Reasignar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnSeleccionar" CommandName="Reasignar" runat="server" 
                                        ImageUrl="~/Images/img_16_order_attend.png" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>

                    <div>
                        <asp:HiddenField ID="hidPersonaId" runat="server" />
                    </div>

                </td>

            </tr>

            <tr>
                <td colspan="8">
                    <div>
                        <uc3:ctrlPageBar ID="ctrlPageBar1" runat="server" Visible="False" />
                    </div>
                </td>
            </tr>

        </table>

         </ContentTemplate>
        </asp:UpdatePanel>

    </div>

<script language="javascript" type="text/javascript">

    function custom_alert(output_msg, title_msg) {
        if (!title_msg)
            title_msg = 'Alert';

        if (!output_msg)
            output_msg = 'No Message to Display.';

        $("<div></div>").html("<table><tr><td><img src='../images/img_msg_warning.png' height='50' width='50' border='0' style='vertical-align:middle;' ></td><td>" +
                                   output_msg + "</td></tr></table>").dialog({
                                       title: title_msg,
                                       resizable: false,
                                       modal: true,
                                       buttons: {
                                           "Aceptar": function () {
                                               $(this).dialog("close");
                                           }
                                       }
                                   });
                               }

    function showpopupother(type_msg, title, msg, resize, height, width) {
        showdialog(type_msg, title, msg, resize, height, width);
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

</script>


</asp:Content>
