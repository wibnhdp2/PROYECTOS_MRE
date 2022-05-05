using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SGAC.Accesorios;
using WIA;
using System.IO;

using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlScanner : System.Web.UI.UserControl
    {
        private static int loEscanIndex = 0;
        private const string loPreview = @"\Images\preliminar.bmp";
        private const string loPreviewFondo = @"\Images\scanner_fondo.png";
        private const string loPreviewPdf = @"\Images\scanner_pdf.pdf";

        private static List<ImageFile> loImageFile = new List<ImageFile>();
        private List<DeviceInfo> loDeviceInfo = new List<DeviceInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //loEscanIndex = 0; 
            var DeviceManager = new DeviceManager();
            if (DeviceManager.DeviceInfos.Count == 0) {
                Response.Write("<script>alert('No se encontraron dispositivos.');</script>");
                this.btnAddPagina.Enabled = false;
                this.btnQuitaPagina.Enabled = false;
            }
            else {
                for (int i = 1; i <= DeviceManager.DeviceInfos.Count; i++){
                    loDeviceInfo.Add(DeviceManager.DeviceInfos[i]);

                    var deviceName = DeviceManager.DeviceInfos[i].Properties["Name"].get_Value().ToString();
                    this.ddlEscaners.Items.Add(deviceName);
                }
            }
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {
            if ((loEscanIndex - 1) <= 1) { loEscanIndex = 1; }
            else { loEscanIndex--; }
            mtEscanerPreview(loEscanIndex);
        }

        protected void btnDown_Click(object sender, EventArgs e)
        {
            if ((loEscanIndex + 1) >= loImageFile.Count) { loEscanIndex = loImageFile.Count; }
            else { loEscanIndex++; }
            mtEscanerPreview(loEscanIndex);
        }

        private void mtEscanerPreview(int FileIndex)
        {
            if (loImageFile.Count != 0)
            {
                ImageFile imageFile = (ImageFile)loImageFile[FileIndex - 1];

                FileInfo fipreview = new FileInfo(Server.MapPath(loPreview));
                if (fipreview.Exists) { fipreview.Delete(); }

                imageFile.SaveFile(fipreview.FullName);
                imgPreview.ImageUrl = loPreview;
                btnEnabled();
            }
            else
            {
                imgPreview.ImageUrl = loPreviewFondo;
                btnEnabled(false);
            }
            mtScannerPaginado();
        }

        private void btnEnabled(bool estado = true) {
            this.btnQuitaPagina.Enabled = estado;
            this.btnPDF.Enabled = estado;
            this.btnUp.Enabled = estado;
            this.btnDown.Enabled = estado;
        }

        private void mtCreatePdf() {
            string xx = pdfFileName();

            FileInfo fi = new FileInfo(Server.MapPath(loPreviewPdf));
            if (fi.Exists) { fi.Delete(); }

            Document documentPdf = new Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
            PdfWriter.GetInstance(documentPdf, new FileStream(fi.FullName, FileMode.Create));
            documentPdf.Open();

            foreach (ImageFile scaneado in loImageFile) {

                FileInfo fipreview = new FileInfo(Server.MapPath(loPreview));
                if (fipreview.Exists) { fipreview.Delete(); }
                scaneado.SaveFile(fipreview.FullName);
                iTextSharp.text.Image img_scaneado = iTextSharp.text.Image.GetInstance(fipreview.FullName);
                documentPdf.Add(img_scaneado);
                }
            documentPdf.Close();

            FileInfo file = new System.IO.FileInfo(fi.FullName);

            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + pdfFileName().ToString());
            Response.TransmitFile(fi.FullName);
            Response.End();       

            //string FilePath = Server.MapPath(@"~\Images\test.pdf");
            //WebClient User = new WebClient();
            //Byte[] FileBuffer = User.DownloadData(FilePath);
            //if (FileBuffer != null)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
            //    Response.BinaryWrite(FileBuffer);
            //}
            }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            if (loImageFile.Count != 0) { mtCreatePdf(); }
        }

        protected void btnAddPagina_Click(object sender, EventArgs e)
        {
            DeviceInfo lDeviceInfo = (DeviceInfo)loDeviceInfo[0];

            Scanner scanner = new Scanner(lDeviceInfo);
            ImageFile file = scanner.Scan();
            loImageFile.Add(file);
            mtEscanerPreview(loImageFile.Count);
            loEscanIndex++;
            mtScannerPaginado();
        }

        protected void btnQuitaPagina_Click(object sender, EventArgs e)
        {
            ImageFile ImageFileToEliminar = (ImageFile)loImageFile[loEscanIndex - 1];
            loImageFile.Remove(ImageFileToEliminar);
            mtEscanerPreview(loImageFile.Count);
        }

        private string pdfFileName() {
            DateTime dt = DateTime.Today;
            string horas = DateTime.Now.Hour.ToString("0#");
            horas += DateTime.Now.Minute.ToString("0#");
            horas += DateTime.Now.Second.ToString("0#");
            horas += DateTime.Now.Millisecond.ToString("0##");


            return dt.ToString("yyyMMdd") + horas.ToString() + "_CodigoTramite.pdf";
            }

        private void mtScannerPaginado() {
            this.txtPaginado.Text = "Pagina " + loEscanIndex.ToString() + " de " + (loImageFile.Count).ToString();
            }
    }
}