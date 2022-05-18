using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SGAC.WinApp
{
    public partial class FrmConfiguracionImpresora : Form
    {
        private Funciones MiFun = new Funciones();

        public FrmConfiguracionImpresora()
        {
            InitializeComponent();
        }

        private void FrmConfiInpresora_Load(object sender, EventArgs e)
        {
            PrinterSettings impresora = new PrinterSettings();

            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                listBox1.Items.Add(PrinterSettings.InstalledPrinters[i].ToString());
            }
            listBox1.SelectedItem = 1;
            listBox1.Focus();
        }

        private void CmdCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CmdAcep_Click(object sender, EventArgs e)
        {
            MiFun.IniEscribir("PREFERENCIAS", "IMPRESORA", listBox1.SelectedItem.ToString(), Environment.CurrentDirectory + "\\SGAC.ini");
            MessageBox.Show("Se establecio la impresora " + listBox1.SelectedItem.ToString() + ", como impresora de Tickets", "Sistema de Colas", MessageBoxButtons.OK);
            this.Close();
        }
    }
}