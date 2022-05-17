using System;
using System.Windows.Forms;

namespace SGAC.WinApp
{
    public partial class FrmVerArchivoTexto : Form
    {
        public string StrNombreArchivo;
        public string StrTitulo;

        public FrmVerArchivoTexto()
        {
            InitializeComponent();
        }


        private void FrmVerArchivoTexto_Load(object sender, EventArgs e)
        {
            this.Text = "Visor Texto - " + StrTitulo;
            richTextBox1.LoadFile(StrNombreArchivo, RichTextBoxStreamType.PlainText);
            
        }

     

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                this.Dispose();
            }
        }


    }
}