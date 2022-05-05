<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlBajaAutoadhesivo.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlBajaAutoadhesivo" %>
<asp:Button ID="btnEliminar" runat="server" Text="   Baja" 
    CssClass="btnDelete" Enabled="False" onclick="btnEliminar_Click"/>
<div id="modalBaja" class="modal">
        <div class="modal-window">
            <div class="modal-titulo">
                <asp:ImageButton ID="imgCerrarPopup" CssClass="close" ImageUrl="~/Images/close.png"
                    OnClientClick="cerrarPopupBaja();return false" runat="server" />
                <span>Motivo de Baja del Insumo</span>
            </div>
            <div class="modal-cuerpo">
                <h3>
                    INGRESE EL MOTIVO</h3>
                <asp:TextBox ID="txtMotivo" CssClass="txtLetra" TextMode="MultiLine" runat="server"
                    Height="80px" Width="350px"></asp:TextBox>
                <asp:HiddenField ID="hInsumoID" runat="server" />
            </div>
            <div class="modal-pie">
                <asp:Button ID="BtnAceptarBaja" CssClass="btnLogin" runat="server" OnClientClick="if(!ValidarDarBaja()) return false;"
                    Text="Aceptar" OnClick="BtnAceptarBaja_Click" />
                <button class="btnLogin" onclick="cerrarPopupBaja(); return false">
                    Cancelar</button>
            </div>
        </div>
    </div>

<script language="javascript" type="text/javascript">
    function ValidarDarBaja() {
        var txtMotivo = $('#<%= txtMotivo.ClientID %>').val();
        if (txtMotivo == "") {
            document.getElementById('<%=txtMotivo.ClientID %>').style.border = "1px solid Red";
            document.getElementById('<%=txtMotivo.ClientID %>').focus();
            return false;
        }
        else { document.getElementById('<%=txtMotivo.ClientID %>').style.border = ""; }
        return true;
    }
    function Popup(valorInsumo) {
        var myHidden = document.getElementById('<%= hInsumoID.ClientID %>');

        if (myHidden)//checking whether it is found on DOM, but not necessary
        {
            myHidden.value = valorInsumo;
        }
        document.getElementById('modalBaja').style.display = 'block';
    }
    function cerrarPopupBaja() {
        document.getElementById('<%=txtMotivo.ClientID %>').value = '';
        document.getElementById('modalBaja').style.display = 'none';
    }
    </script>