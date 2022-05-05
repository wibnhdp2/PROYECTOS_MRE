<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlAdjuntoNotarial.ascx.cs"
    Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlAdjuntoNotarial" %>

<%--<link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />--%>

<%-- <script src="../../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
 <script src="../../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
 <script src="../../Scripts/site.js" type="text/javascript"></script>--%>

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
    .style1
    {
        width: 706px;
    }
</style>
<asp:UpdatePanel ID="updActuacionAdjuntar" UpdateMode="Conditional" runat="server">
        <Triggers>
              <asp:PostBackTrigger controlid="BtnGrabActAdj"  />
              <asp:AsyncPostBackTrigger controlid="BtnLimpiaAdj" eventname="Click" />
              <asp:PostBackTrigger controlid="Grd_Archivos"    />
              
         </Triggers> 
      <ContentTemplate>

         <table class="mTblSecundaria">
                                        <tr>
                                            <td id="pnlAdjuntos" runat="server">
                                                <asp:Label ID="lblValidacionAdjunto" runat="server" Text="Falta validar algunos campos."
                                                                CssClass="hideControl" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTipAdj" runat="server" Text="Tipo :"></asp:Label>
                                                        </td>
                                                        <td align="left" class="style1">
                                                            <asp:DropDownList ID="cmb_TipoArchivo" runat="server" Height="20px" 
                                                                Width="180px" Enabled="false"
                                                                onselectedindexchanged="cmb_TipoArchivo_SelectedIndexChanged" AutoPostBack="True" />
                                                            <asp:Label ID="lblValTipoArchivo" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                            <br />
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField ID="hifCodVinculado" runat="server" Value="" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr> 
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDescAdj" runat="server" Text="Descripción :" Width="100px"></asp:Label>
                                                        </td>
                                                        <td class="style1">
                                                            <asp:TextBox ID="txtDescAdj" runat="server" CssClass="txtLetra" Width="600px" Height="40px" MaxLength="500" TextMode="MultiLine" />
                                                            <asp:Label ID="lblValDescAdj" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                        </td>                                                            
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAdjArchiv" runat="server" Text="Adjuntar Archivo :"></asp:Label>
                                                        </td>
                                                        <td class="style1">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:FileUpload ID="FileUploader" runat="server" Width="600px"></asp:FileUpload>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td runat="server" id="msjeWarning" colspan="2">
                                                                        <table id="tMsjeWarnig" class="tMsjeWarnig" align="center">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblMsjeWarnig" runat="server" Text="" CssClass="lblMsjeWarnig" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td runat="server" id="msjeError" colspan="2">
                                                                        <table id="tMsjeError" class="tMsjeError" align="center">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblMsjeError" runat="server" Text="" CssClass="lblMsjeError" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td runat="server" id="msjeSucess" colspan="2">
                                                                        <table id="tMsjeSucess" class="tMsjeSucess" align="center">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblMsjeSucess" runat="server" Text="" CssClass="lblMsjeSucess" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:HiddenField ID="hd_Extension" runat="server" />                                                                        
                                                                        <asp:Label ID="lblNombreArchivo" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                        </table>
 

                                                            <asp:Label ID="lblLeyendaAdj" runat="server" Font-Bold="True" Text="Solo se permiten guardar archivos de un tamaño Max. de "></asp:Label>
                                                            <asp:Label ID="lblTamanioMax" runat="server" Font-Bold="True" Text=""></asp:Label>
                                                            <asp:Label ID="lblUnidad" runat="server" Font-Bold="True" Text=" KB"></asp:Label>
                                                        </td>
                                                  
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="left">
                                                            <asp:Button ID="BtnGrabActAdj" runat="server" Text="       Guardar Adjunto" CssClass="btnSaveAnotacion"
                                                                OnClick="BtnGrabActAdj_Click" />    
                                                            <asp:Button ID="BtnLimpiaAdj" runat="server" Width="150px" 
                                                                Text="          Limpiar Datos" CssClass="btnLimpiar" 
                                                                onclick="BtnLimpiaAdj_Click"></asp:Button>

                                                        <asp:HiddenField ID="hidNomAdjFile" runat="server" />
                                                        </td>
                                                        
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="Grd_Archivos" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    AutoGenerateColumns="False" GridLines="None" 
                                                    onrowcommand="Grd_Archivos_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ando_iActoNotarialDocumentoId" HeaderText="ando_iActoNotarialDocumentoId"
                                                            HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta" >
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ando_iActoNotarialId" HeaderText="ando_iActoNotarialId"
                                                            HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta" >
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ando_sTipoDocumentoId" HeaderText="ando_sTipoDocumentoId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta" >
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ando_dFechaCreacion" HeaderText="Fecha y Hora" 
                                                            DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}" >
                                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ando_vUsuarioCreacion" HeaderText="Usuario" >
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ando_vTipoDocumento" HeaderText="Tipo Archivo" >
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ando_vRutaArchivo" HeaderText="Nombre Archivo" >
                                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ando_vDescripcion" HeaderText="Descripción" >
                                                            <ItemStyle Width="300px" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ofco_vSiglas" HeaderText="ofco_vSiglas" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta" >
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="yyyymmdd" HeaderText="yyyymmdd" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta" >
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>

                                                        <asp:TemplateField HeaderText="Vista Previa" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnPrint"  CommandName="Imprimir" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../../Images/img_16_download.png"
                                                                     />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditar" CommandName="Editar"  ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../../Images/img_16_edit.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEliminar" CommandName="Eliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../../Images/img_grid_delete.png"/>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div>
                                                <%--    <ucc2:ctrolPageBar ID="ctrlPagActuacionAdjuntos" runat="server" OnClick="ctrlPagActuacionAdjuntos_Click"
                                                        Visible="False" />--%>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

      </ContentTemplate>
  
     

</asp:UpdatePanel>



<script language="javascript" type="text/javascript">
    function ValidarRegistroAdjunto() {
        var bolValida = true;
        $("#<%= lblValidacionAdjunto.ClientID %>").html("Falta validar algunos campos.");

        var strDescripcion = $.trim($("#<%= txtDescAdj.ClientID %>").val());
        var strNomAdjFile = $.trim($("#<%= hidNomAdjFile.ClientID %>").val());

        var txtDescAdj = document.getElementById('<%= txtDescAdj.ClientID %>');
        var hidNomAdjFile = document.getElementById('<%= hidNomAdjFile.ClientID %>');

        if (strDescripcion.length == 0) {
            txtDescAdj.style.border = "1px solid Red";
            bolValida = false;
        }
        else {
            txtDescAdj.style.border = "1px solid #888888";
        }

        if (strNomAdjFile.length == 0) {
            $("#<%= lblValidacionAdjunto.ClientID %>").html("Falta adjuntar archivo.");
            hidNomAdjFile.style.border = "1px solid Red";
            bolValida = false;
        }
        else {
            hidNomAdjFile.style.border = "1px solid #888888";
        }

        if (bolValida) {
            $("#<%= lblValidacionAdjunto.ClientID %>").hide();
            bolValida = confirm("¿Está seguro de grabar los cambios?");
        }
        else {
            $("#<%= lblValidacionAdjunto.ClientID %>").show();
        }
        return bolValida;
    }

    function MoveTabIndex(iTab) {
        $('#tabs').tabs("option", "active", iTab);
    }

    function Habilitar(iTab) {
        $('#tabs').enableTab(iTab);
    }

</script>
