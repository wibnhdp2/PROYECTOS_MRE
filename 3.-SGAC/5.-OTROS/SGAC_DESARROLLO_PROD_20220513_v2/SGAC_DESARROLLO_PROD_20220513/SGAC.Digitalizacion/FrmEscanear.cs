using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using WIA;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;


namespace SGAC.Digitalizacion
{
    public partial class frmEscanear : Form
    {
        
        List<Image> limage = new List<Image>();
        string spathPdf = ConfigurationManager.AppSettings["spathPdf"];
        string spathDestinoPdf = System.IO.Path.GetTempPath() + @"\doc.pdf";
        bool BolPararHilo = false;
        string srutafilename = "";

        public frmEscanear()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            lblInfo.Text = "";
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                              (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
            
            Devices.Items.Clear();
            var deviceManager = new DeviceManager();

            for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
            {
                if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType)
                {
                    return;
                }
                Devices.Items.Add(new Scanner(deviceManager.DeviceInfos[i]));
            }           
        }

        private void btnEscanear_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
                if (limage.Count > 0)
                {
                    limage.Clear();
                    this.lstPagina.Items.Clear();
                }
                lblInfo.Text = "Empezando Digitalización";
                escanearDocumento();
            }
            else
            {
                BolPararHilo = true;
                backgroundWorker1.CancelAsync();
            }

        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            int nItems = Devices.Items.Count;

            if (nItems == 0)
            {
                MessageBox.Show("Por favor conecte el escaner", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var device = Devices.SelectedItem as Scanner;

            if (device == null)
            {
                Devices.SelectedIndex = 0;
            }

            //scodEscPublica = frmListaEscPublica.scodEscPublica;
            //scodMision = frmListaEscPublica.scodMision;
            String sfilename = GenerateFileName("PDF");

            //if (File.Exists(srutafilename))
            //{
            //    File.Delete(srutafilename);
            //}

            String sAnio = DateTime.Now.ToString("yyyy").ToString();
            String sMes = DateTime.Now.ToString("MM").ToString();

            String spathAnio = spathPdf + "\\" + sAnio;

            if (Directory.Exists(spathAnio) == false)
            {
                Directory.CreateDirectory(spathAnio);
            }

            String spathAnioMes = spathAnio + "\\" + sMes;

            if (Directory.Exists(spathAnioMes) == false)
            {
                Directory.CreateDirectory(spathAnioMes);
            }

            srutafilename = spathAnioMes + "\\" + sfilename + ".pdf";

            device.CreatePDF(limage, srutafilename);
                    
        }

        public string GenerateFileName(string context)
        {
            return context + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        private void escanearDocumento()
        {
            int nItems = Devices.Items.Count;

            if (nItems == 0)
            {
                MessageBox.Show("Por favor conecte el escaner", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Devices.SelectedIndex = 0;

            var device = Devices.SelectedItem as Scanner;


            if (device == null)
            {
                MessageBox.Show("Por favor seleccione el escaner", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //WIA.ImageFile imgFile;
            //imgFile.FileData.get_BinaryData();

            ImageFile imgFile = device.Scan();
            //String sPathImagen = @"d:\img.jpeg";

            //----------------------------

            //----------------------------

            //if (File.Exists(sPathImagen))
            //{
            //    File.Delete(sPathImagen);
            //}

            //imgFile.SaveFile(sPathImagen);



            //convierte imgFile a un array de byte.
            Byte[] imageBytes = (byte[])imgFile.FileData.get_BinaryData();


            MemoryStream ms = new MemoryStream(imageBytes);
            Image objimage = Image.FromStream(ms);

           
            picImagen.Image = objimage;

            limage.Add(objimage);

            this.lstPagina.Items.Add("documento_" + limage.Count.ToString("000"));

        }

        private void btnAgregarPagina_Click(object sender, EventArgs e)
        {
            int CantidadPaginas = Convert.ToInt16(ConfigurationManager.AppSettings["CantidadPaginas"]);
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
                if (lstPagina.Items.Count < CantidadPaginas)
                {                    
                    lblInfo.Text = "Empezando Digitalización";
                    escanearDocumento();
                }
                else
                {
                    MessageBox.Show("Solo se admiten "+CantidadPaginas+" paginas por archivo PDF", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                BolPararHilo = true;
                backgroundWorker1.CancelAsync();
            }


        }

        private void btnEliminarPagina_Click(object sender, EventArgs e)
        {
            if (lstPagina.SelectedIndex > -1)
            {
                int nindex = lstPagina.SelectedIndex;

                this.lstPagina.Items.RemoveAt(nindex);
                limage.RemoveAt(nindex);
                if (limage.Count > 0)
                {
                    lstPagina.SelectedIndex = 0;
                }
                else
                {
                    lstPagina.SelectedIndex = -1;
                    this.picImagen.Image = null;
                }
            }
            else
            {
                if (limage.Count > 0)
                {
                    MessageBox.Show("Por favor seleccione la pagina a eliminar", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No existe ninguna pagina", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void lstPagina_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPagina.SelectedIndex > -1)
            {
                Image imageSel = limage[lstPagina.SelectedIndex];
                this.picImagen.Image = imageSel;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {

                if (File.Exists(spathDestinoPdf) == true)
                {
                    File.Delete(spathDestinoPdf);
                }
                this.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnAbrirPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(srutafilename) == true)
                {
                    if (File.Exists(spathDestinoPdf) == true)
                    {
                        File.Delete(spathDestinoPdf);
                    }

                    Process proc = new Process();

                    File.Copy(srutafilename, spathDestinoPdf, true);

                    proc.StartInfo.FileName = spathDestinoPdf;
                    proc.Start();
                    proc.Close();
                }
                else
                {
                    MessageBox.Show("Documento PDF no existe", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir documento PDF", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 50; i++)
            {
                //Realiza una tarea
                System.Threading.Thread.Sleep(50);
                backgroundWorker1.ReportProgress(i);
                if (backgroundWorker1.CancellationPending)
                    return;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Notificar el progreso de la tarea
            progressBar1.Value = e.ProgressPercentage;
            //lblInfo.Text = e.ProgressPercentage + "%";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            {
                if (BolPararHilo == false)
                {
                    //Realizamos las operaciones que haya que realizar al terminar el progreso
                    lblInfo.Text = "Digitalización terminada";                   
                    //btnCancelar.Enabled = false;
                    progressBar1.Value = 0;
                }
            }
        }
    }
}
