<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlUploader.ascx.cs"
    Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlUploader" %>
<style type="text/css">
    .tMsjeWarnig
    {
        background-color: #F2F1C2;
        border-color: Yellow; /*#6E4E1B;*/
        color: #4B4F5E;
        height: 15px;
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
        width: 100%;
    }
    
    .lblMsjeSucess
    {
        margin-left: 25px;
    }   
</style>
 
 
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
  
   <ContentTemplate>
                
                 <table>
    <tr>        
        <td>
            <asp:FileUpload ID="FileUploader" runat="server" size="65" accept=".pdf" />
             <br />
             <p style="display: inline; background-color:White;">Archivo valido: PDF.</p>
        </td>
        <td>
            <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="    Subir"   
                CssClass="btnDownload" />
        </td>
    </tr>
    <tr>
        <td runat="server" id="msjeWarning" colspan="2">
            <table id="tMsjeWarnig" class="tMsjeWarnig" align="center">
                <tr>
                    <td style="width:30px"><asp:Image ID="Image3" runat="server"  ImageUrl="~/Images/img_16_warning.png"/></td>
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
                    <td style="width:30px"><asp:Image ID="Image2" runat="server"  ImageUrl="~/Images/img_16_error.png"/></td>
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
                    <td style="width:30px"><asp:Image ID="Image1" runat="server"  ImageUrl="~/Images/img_16_success.png"/></td>
                    <td>
                        <asp:Label ID="lblMsjeSucess" runat="server" Text="" CssClass="lblMsjeSucess" />
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <asp:HiddenField ID="hd_Extension" runat="server" />  
            <asp:HiddenField ID="hRutaArchivoNuevo" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="lblNombreArchivo" runat="server" Text="" Height="25px"></asp:Label>
        </td>
    </tr>
</table>
                
   </ContentTemplate>
</asp:UpdatePanel>
 


 <script type="text/javascript">

     function LoadArchivos() {
         var formData = new FormData($("#FrmAdjunto")[0]);
         $.ajax({
             type: "POST",
             url: "ctrlUploader.ascx/LoadArchivos",
             data: formData,
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             cache: false,
             async: false,
             contentType: false,
             processData: false,
             success: function (rspt) {
                 alert(rspt.d);
                 return false;
             }
         });
     }
 </script>