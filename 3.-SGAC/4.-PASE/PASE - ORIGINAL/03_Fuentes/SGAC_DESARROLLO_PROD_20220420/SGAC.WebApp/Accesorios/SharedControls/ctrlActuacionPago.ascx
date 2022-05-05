<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlActuacionPago.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlActuacionPago" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>




<div>
    <div style="padding: 5px 5px 0px 5px; display:inline-block; width:325px">
        <asp:Label ID="lblTipoPago" runat="server" Text="Tipo de Pago:" Width="100" style=" text-align:left;"></asp:Label>
        <asp:DropDownList ID="ddlTipoPago" runat="server" Width="200px" ClientIDMode="AutoID"
            style="cursor:pointer;" AutoPostBack="True" 
            onselectedindexchanged="ddlTipoPago_SelectedIndexChanged">
        </asp:DropDownList>
        <asp:Label ID="lblTipoPagoObl" runat="server" Text="*" Width="10" style=" text-align:left; color:Red"></asp:Label>
    </div>

    <div style="padding: 5px 5px 0px 5px; display:inline-block; width:530px">
        <table>
            <tr>
                <td align="right" style="width:70px">
                    <asp:Label ID="lblExoneracion" runat="server" Text="Ley que exonera el pago:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlExoneracion" runat="server"  style="cursor:pointer"
                    Enabled="True" Height="20px" 
                    Width="414px" />                                            
                    <asp:Label ID="lblValExoneracion" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                    <asp:RadioButton ID="RBNormativa" runat="server" Text=""  AutoPostBack="true" style="cursor:pointer"
                    GroupName="GrupoExonera" Width="15px" 
                    oncheckedchanged="RBNormativa_CheckedChanged"/>
                </td>
            </tr>
        </table>        
    </div>
</div>

<div>
    <div style="padding: 5px 5px 0px 5px; display:inline-block; width:325px">
        <asp:Label ID="lblNroVoucher" runat="server" Text="N° Voucher:" Width="100px" 
            style=" text-align:left;" Visible="False"></asp:Label>
        <asp:TextBox ID="txtNroVoucher" runat="server" Width="200px" 
            CssClass="txtLetra" Visible="False"></asp:TextBox>
        <asp:Label ID="lblNroVoucherObl" runat="server" Text="*" Width="10" style=" text-align:left; color:Red"></asp:Label>
    </div>
    <div style="padding: 5px 5px 0px 5px; display:inline-block; width:530px">
        <table>
            <tr>
                <td align="right" style="width:70px">
                    <asp:Label ID="lblSustentoTipoPago" runat="server" Text="Sustento:"></asp:Label>        
                </td>
                <td>
                    <asp:TextBox ID="txtSustentoTipoPago" runat="server" Width="410px" CssClass="txtLetra" ></asp:TextBox>
                    <asp:Label ID="lblValSustentoTipoPago" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                    <asp:RadioButton ID="RBSustentoTipoPago" runat="server" Text=""  AutoPostBack="true" style="cursor:pointer"
                        GroupName="GrupoExonera" Width="15px" 
                        oncheckedchanged="RBNormativa_CheckedChanged" />                
                </td>
            </tr>
        </table>
        
    </div>
</div>

<div>

    <div style="padding: 5px 5px 0px 5px; display:inline-block; width:325px">
        <asp:Label ID="lblBanco" runat="server" Text="Nombre de Banco:" Width="100" style=" text-align:left;"></asp:Label>
        <asp:DropDownList ID="ddlBanco" runat="server" Width="200px" style="cursor:pointer;">
        </asp:DropDownList>
        <asp:Label ID="lblBancoObl" runat="server" Text="*" Width="10" style=" text-align:left; color:Red"></asp:Label>
    </div>

    <div style="padding: 5px 5px 0px 5px; display:inline-block; width:325px">
        <asp:Label ID="lblNroOperacion" runat="server" Text="N° Operacion:" Width="100" style=" text-align:left;"></asp:Label>
        <asp:TextBox ID="txtNroOperacion" runat="server" Width="200px" CssClass="txtLetra"></asp:TextBox>
        <asp:Label ID="lblNroOperacionObl" runat="server" Text="*" Width="10" style=" text-align:left; color:Red"></asp:Label>
    </div>

    
</div>


<div>

    <div style="padding: 5px 5px 0px 5px; display:inline-block; width:325px">
        <asp:Label ID="lblMonto" runat="server" Text="Monto Cancelado:" Width="100" style=" text-align:left;"></asp:Label>
        <asp:TextBox ID="txtMonto" runat="server" Width="95px" Text="0.00" Enabled="False" CssClass="campoNumero"></asp:TextBox>
        <asp:Label ID="lblMontoObl" runat="server" Text="*" Width="10" style=" text-align:left; color:Red"></asp:Label>
    </div>

    <div style="padding: 5px 5px 0px 5px; display:inline-block; width:325px">
        <asp:Label ID="lblFechaPago" runat="server" Text="Fecha de Pago:" Width="100" style=" text-align:left;"></asp:Label>
        <SGAC_Fecha:ctrlDate ID="ctrFecPago" runat="server" Width="100px" Ejecutar_Scrip_Pago="true"/> 
        <asp:Label ID="lblFechaPagoObl" runat="server" Text="*" Width="10" style=" text-align:left; color:Red"></asp:Label>
    </div>
    <asp:UpdatePanel ID="updTextoValidacion" runat="server">
        <ContentTemplate>
            <asp:TextBox ID="txtControlValidado" runat="server" AutoPostBack="true" style=" display:none"></asp:TextBox>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Label ID="lbltarifa" runat="server" Text="" style=" display:none"></asp:Label>
    <asp:Button ID="btnValidar" runat="server" Text="Button" style=" display:none"/>
</div>

<asp:HiddenField ID="HF_TipoPagoId" runat="server" />

<script type="text/javascript">

    var bMostrarValidacion = true;

    function ucActuacionPagoHabilitarPorTipoPago_<%=uniqueKey %>() {

        ucActuacionPagoLimpiarFaltantes_<%=uniqueKey %>();

        var tipoPago = $('#<%=ddlTipoPago.ClientID %>').val();


        if (tipoPago == '3501' || 
            tipoPago == '3507' || tipoPago == '3508') {
            ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>esActuacion_<%=uniqueKey %>(true, true, true, true, true);
            ucActuacionPagoEtiquetasObligatoriasPorParticipante_<%=uniqueKey %>(true, true, true, true, true);

//            $('#<%= txtControlValidado.ClientID %>').val("0");
        }
        else {
            ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>esActuacion_<%=uniqueKey %>(false, false, false, false, false);
            ucActuacionPagoEtiquetasObligatoriasPorParticipante_<%=uniqueKey %>(false, false, false, false, false);

            if(tipoPago=="0")
                $('#<%= txtControlValidado.ClientID %>').val("0");
            else
                $('#<%= txtControlValidado.ClientID %>').val("1");
        }

    }

    function ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>esActuacion_<%=uniqueKey %>(bVoucher, bBanco, bOperacion, bMonto, bFecha) {


        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=txtNroVoucher.ClientID %>'), bVoucher);
        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=lblNroVoucher.ClientID %>'), bVoucher);
        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=lblNroVoucherObl.ClientID %>'), bVoucher);

        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=ddlBanco.ClientID %>'), bBanco);
        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=lblBanco.ClientID %>'), bBanco);
        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=lblBancoObl.ClientID %>'), bBanco);

        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=txtNroOperacion.ClientID %>'), bOperacion);
        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=lblNroOperacion.ClientID %>'), bOperacion);
        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=lblNroOperacionObl.ClientID %>'), bOperacion);

        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=txtMonto.ClientID %>'), bMonto);
        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=lblMonto.ClientID %>'), bMonto);
        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=lblMontoObl.ClientID %>'), bMonto);

        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=ctrFecPago.FindControl("TxtFecha").ClientID %>'), bFecha);
        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=ctrFecPago.FindControl("btnFecha").ClientID %>'), bFecha);
        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=lblFechaPago.ClientID %>'), bFecha);
        ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>($('#<%=lblFechaPagoObl.ClientID %>'), bFecha);

    }

    function ucActuacionPagoDeshabilitarControl_<%=uniqueKey %>(cControl, bAccion) {
        cControl.toggle(bAccion);
    }

    function ucActuacionPagoLimpiarFaltantes_<%=uniqueKey %>() {

        $('#<%=txtNroVoucher.ClientID  %>').css("border", "solid #888888 1px");

        $('#<%=ddlBanco.ClientID  %>').css("border", "solid #888888 1px");

        $('#<%=txtNroOperacion.ClientID  %>').css("border", "solid #888888 1px");

        $('#<%=txtMonto.ClientID  %>').css("border", "solid #888888 1px");

        $('#<%=ctrFecPago.FindControl("TxtFecha").ClientID  %>').css("border", "solid #888888 1px");

    }

    function ucActuacionPagoValidarPorTipoPago_<%=uniqueKey %>(bVoucher, bBanco, bOperacion, bMonto, bFecha) {


        var bolValida = true;


        bolValida = ucActuacionPagoValidarControl_<%=uniqueKey %>($('#<%=ddlTipoPago.ClientID %>'), '0');

        if (!bolValida)
            return false;

        if (bVoucher)
            bolValida = ucActuacionPagoValidarControl_<%=uniqueKey %>($('#<%=txtNroVoucher.ClientID %>'), '');

        if (bBanco)
            bolValida = ucActuacionPagoValidarControl_<%=uniqueKey %>($('#<%=ddlBanco.ClientID %>'), '0');

        if (bOperacion)
            bolValida = ucActuacionPagoValidarControl_<%=uniqueKey %>($('#<%=txtNroOperacion.ClientID %>'), '');

        if (bMonto)
            bolValida = ucActuacionPagoValidarControl_<%=uniqueKey %>($('#<%=txtMonto.ClientID %>'), '');


        if (bFecha) {
            var strFecNac = $.trim($('#<%= ctrFecPago.FindControl("TxtFecha").ClientID  %>').val());
            var txtFecNac = document.getElementById('<%= ctrFecPago.FindControl("TxtFecha").ClientID  %>');

            if (strFecNac == "") {
                bolValida = false;

                if(bMostrarValidacion)
                txtFecNac.style.border = "1px solid Red";
            }
            else {
                txtFecNac.style.border = "1px solid #888888";
            }
        }


        return bolValida;
    }

    function ucActuacionPagoValidarControl_<%=uniqueKey %>(control, controlInitialValue) {
        var strControlValor = $.trim(control.val());

        if (strControlValor == controlInitialValue) {

            if (bMostrarValidacion) {
                
                control.css("border", "solid Red 1px");
            }

            return false;
        }
        else {
            control.css("border", "solid #888888 1px");
        }

        return true;
    }

    function ucActuacionPagoEtiquetasObligatoriasPorParticipante_<%=uniqueKey %>(bVoucher, bBanco, bOperacion, bMonto, bFecha) {

        $('#<%= lblNroVoucherObl.ClientID %>').toggle(bVoucher);
        $('#<%= lblBancoObl.ClientID %>').toggle(bBanco);
        $('#<%= lblNroOperacionObl.ClientID %>').toggle(bOperacion);
        $('#<%= lblMontoObl.ClientID %>').toggle(bMonto);
        $('#<%= lblFechaPagoObl.ClientID %>').toggle(bFecha);

    }

    function ucActuacionPagoValidarControles_<%=uniqueKey %>() {

        bMostrarValidacion = false;

        var bolValida = true;  

        var tipoPago = $('#<%=ddlTipoPago.ClientID %>').val();

        if (tipoPago == '3501' || 
            tipoPago == '3507' || tipoPago == '3508') {
            bolValida = ucActuacionPagoValidarPorTipoPago_<%=uniqueKey %>(true, true, true, true, true);
        }
        else {
            bolValida = ucActuacionPagoValidarPorTipoPago_<%=uniqueKey %>(false, false, false, false, false);
        }


        if (bolValida) {
            $('#<%= txtControlValidado.ClientID %>').val("1");
        }
        else {
            $('#<%= txtControlValidado.ClientID %>').val("0");
        }
    
        __doPostBack('<%= txtControlValidado.ClientID %>', '');

        return bolValida;
    }

    function ucActuacionPagoMostrarValidacion_<%=uniqueKey %>() {

        bMostrarValidacion = true;

        var tipoPago = $('#<%=ddlTipoPago.ClientID %>').val();

        if (tipoPago == '3501' ||
            tipoPago == '3507' || tipoPago == '3508') {
            bolValida = ucActuacionPagoValidarPorTipoPago_<%=uniqueKey %>(true, true, true, true, true);
        }
        else {
            bolValida = ucActuacionPagoValidarPorTipoPago_<%=uniqueKey %>(false, false, false, false, false);
        }

        if(bolValida)
            ucActuacionPagoValidarControles_<%=uniqueKey %>();

        return bolValida;
    }

    function Ejecuar_Script_Pago()
    {
        ucActuacionPagoValidarControles_<%=uniqueKey %>();
    }

    function InscribirTextoControlFecha()
    {
        $('#<%=ctrFecPago.FindControl("TxtFecha").ClientID %>').bind({

                blur: function (e) {
                    ucActuacionPagoValidarControles_<%=uniqueKey %>();       
                }

            });
    }

</script>


