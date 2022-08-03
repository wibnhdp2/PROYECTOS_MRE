<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cuUploadFile.ascx.cs" Inherits="SolCARDIP_REGLINEA.UserControl.cuUploadFile" %>
<script src='<%= VirtualPathUtility.ToAbsolute("~/Scripts/jquery-1.11.3.min.js")%>'
        type="text/javascript"></script>
  <div class="custom-file">
    <asp:FileUpload class="custom-file-input" ID="fileupload" runat="server"/>
    <label class="custom-file-label" for="customFile">Seleccione su Archivo</label>
  </div>


<script>
    // Add the following code if you want the name of the file appear on select
    $(".custom-file-input").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });
</script>
