using System;
using System.Data;
using System.Windows.Forms;

namespace SGAC.WinApp
{
    public partial class FrmEmisionTicket : Form
    {
        private int intTipoDocumento;
        private int intPrioridadId = 7351;                 // 7351 = NORMAL; 7352 = INTERMEDIA; 7353 = PREFERENCIAL
        private int intTipoCliente = 7401;                 // 7401 = CONNACIONAL; 7402 = cONNACIONAL SIN DOCUMENTO DE IDENTIDAD, 7403 = RECURREMTE (EXTRANJERO)
        private int intPersonaId = 0;                   // ID DEL CONNACIONAL O RECURRENTE (PARA EL CASO DE LOS RECURRENTES EL ID SERA 0)
        private int intTicketeraId;                     // ID DE LA MAQUINA TICKETERA
        private string strPersonaNombre;                // NOMBRE DE LA PERSONA
        private string strTituloForm;
        private Funciones MiFun = new Funciones();
        private bool BolCargando = false;

        public FrmEmisionTicket()
        {
            InitializeComponent();
        }

        private void FrmEmisionTicket_Load(object sender, EventArgs e)
        {
            strTituloForm = "SISTEMA DE COLAS - Emision de Tickets";
            LblNomConsulado.Text = Program.strServOficinaConsularNombre;
            TxtNumDoc.Text = "";
            this.Text = strTituloForm;
            TxtNumDoc.Enabled = false;
            radioButton1.Checked = true;
            radioButton2.Checked = false;

            radioButton3.Checked = true;
            radioButton1_CheckedChanged(sender, e);

            TlyTitulo.Left = 0;
            TlyTitulo.Top = 0;

            TlyTitulo.Width = this.Width;

            TlyPrincipal.Left = 0;
            TlyPrincipal.Top = 93;

            TlyPrincipal.Width = this.Width - 20;
            TlyPrincipal.Height = this.Height - 80;

            if (MiFun.ArchivoExiste(Program.strArchivo) == false)
            {
                this.Hide();
                MessageBox.Show("No se ha encontrado la Base de datos local del Sistema de Colas", "Sistema de Colas", MessageBoxButtons.OK);
                this.Close();
            }

            string StrValor;
            StrValor = MiFun.IniLeer("PARAMETROS", "TICKETERA", Program.vRutaBDLocal + "\\Colas.ini");

            if (StrValor != "") { intTicketeraId = Convert.ToInt32(StrValor); }
            if (intTicketeraId == 0) { SeleccionarTicketera(); }

            // PREGUNTAMOS SI LA OFICINA CONSULAR DEL SERVIDOR COINCIDE CON LA OFICINA DE LA BASE DE DATOS
            string StrBDOficinaConsularId = Program.HallarOficinaConsularBD(Program.strArchivo);
            string StrBDOficinaConsularNombre = "";

            if (Program.strServOficinaConsularCodigo != StrBDOficinaConsularId)
            {
                StrBDOficinaConsularNombre = Program.TraeOficinaConsular(StrBDOficinaConsularId, Program.strArchivo);
                MessageBox.Show("La Oficina Consular de la BD es " + StrBDOficinaConsularNombre.ToUpper() + " no coincide con la Oficina Consular Actual " + Program.strServOficinaConsularNombre + ", Inicialice la BD en la opcion [Iniciar BD] del menu [Procesos]", "Sistema de Colas", MessageBoxButtons.OK);
                this.Close();
            }
        }

        private void FrmEmisionTicket_Activate(object sender, EventArgs e)
        {
            // ASIGNAMOS EL TIPO DE PRIORIDAD 7351 POR DEFECTO NORMAL
            radioButton3.Checked = true;
            intPrioridadId = 7351;
            intTipoCliente = 7401;

            //// ASIGNAMOS EL TIPO DE PERSONA CONNACIONAL
            radioButton1.Checked = true;       // documento de identidad
            radioButton8.Checked = true;
            TxtNumDoc.Text = "";
        }

        private void FrmEmisionTicket_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            intTipoDocumento = 1;

            label2.Visible = true;
            label2.Text = "Nº de D.N.I.";
            TxtNumDoc.MaxLength = 8;
            if (radioButton1.Checked == true)
            {
                TxtNumDoc.Text = "";
                TxtNumDoc.Enabled = true;
                TxtNumDoc.Focus();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            intTipoCliente = 7401;
            intTipoDocumento = 2;
            TxtNumDoc.Clear();
            TxtNumDoc.MaxLength = 12;
            label2.Visible = true;
            label2.Text = "Nº de Pasaporte";
            if (radioButton2.Checked)
            {
                TxtNumDoc.Focus();
                TxtNumDoc.Enabled = true;
                TxtNumDoc.Focus();
            }
        }

        private void TxtNumDoc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }

            //Para obligar a que sólo se introduzcan números
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso
                {
                    e.Handled = false;
                }
                else
                {
                    //el resto de teclas pulsadas se desactivan
                    e.Handled = true;
                }
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked == true)
            {
                intTipoCliente = 7402;
                LLamarVentanaTipoServicio(intTipoCliente, intPrioridadId, intTipoDocumento, 0);
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked == true)
            {
                intTipoCliente = 7403;
                LLamarVentanaTipoServicio(intTipoCliente, intPrioridadId, intTipoDocumento, 0);
            }
        }

        private string TraeRune(int intDocumentoTipo, string DocumentoNumero)
        {
            string strPersonaNombre = "";
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                prxIntegrador.ColasAtencionClient FunBuscarPersona = new prxIntegrador.ColasAtencionClient();

                strPersonaNombre = FunBuscarPersona.BuscarPersonaRune(intDocumentoTipo, DocumentoNumero);
                Cursor.Current = Cursors.Arrow;
            }
            catch
            {
                LblMensajes.Visible = true;
                MessageBox.Show("No se pudo acceder al Host para realizar la consulta", "Sistema de Colas", MessageBoxButtons.OK);
                //LblMensajes.Text = "No se pudo acceder al Host para realizar la consulta";
                strPersonaNombre = "";
            }
            return strPersonaNombre;
        }

        private void CmdOk_Click(object sender, EventArgs e)
        {
            bool BolSeguir = true;
            int intDocumentoTipo = 0;

            if (TxtNumDoc.Text == "")
            {
                if (radioButton1.Checked == true)
                {
                    LblMensajes.Visible = true;
                    LblMensajes.Text = "El número del D.N.I. no puede ser de longitud 0";
                }
                if (radioButton2.Checked == true)
                {
                    LblMensajes.Visible = true;
                    LblMensajes.Text = "El número de Pasaporte no puede ser de longitud 0";
                }
                BolSeguir = false;
            }
            else
            {
                if (radioButton1.Checked == true)
                {
                    if (TxtNumDoc.TextLength != 8)
                    {
                        //MessageBox.Show("El numero del D.N.I. debe de tener 8 digitos como minimo", "Sistema de Colas", MessageBoxButtons.OK);
                        LblMensajes.Visible = true;
                        LblMensajes.Text = "El número del D.N.I. debe de tener 8 dígitos como mínimo";
                        BolSeguir = false;
                    }
                }

                if (radioButton2.Checked == true)
                {
                    if (TxtNumDoc.TextLength != 10)
                    {
                        //MessageBox.Show("El numero del Pasaporte debe de tener 10 digitos como minimo", "Sistema de Colas", MessageBoxButtons.OK);
                        LblMensajes.Visible = true;
                        LblMensajes.Text = "El número del Pasaporte debe de tener 10 dígitos como mínimo";
                        BolSeguir = false;
                    }
                }
            }

            if (radioButton1.Checked == true) { intDocumentoTipo = 1; }
            if (radioButton2.Checked == true) { intDocumentoTipo = 5; }

            if (BolSeguir == false)
            {
                TxtNumDoc.Text = "";
                TxtNumDoc.Focus();
                return;
            }
            else
            {
                BolSeguir = false;
                // BUSCAMOS EL NUMERO DE DNI o PASAPORTE EN LA BD DE SISTEMA SGAC
                //prxIntegrador.ColasAtencionClient FunBuscarPersona = new prxIntegrador.ColasAtencionClient();

                strPersonaNombre = TraeRune(intDocumentoTipo, TxtNumDoc.Text);   // FunBuscarPersona.BuscarPersonaRune(intDocumentoTipo, TxtNumDoc.Text);
                //FunBuscarPersona.Close();

                if (strPersonaNombre != "")
                {
                    string[] strNombre = strPersonaNombre.Split('|');
                    strPersonaNombre = strNombre[1];
                    intPersonaId = Convert.ToInt32(strNombre[3]);
                }

                if (strPersonaNombre != "")
                {
                    BolSeguir = true;
                }
                else
                {
                    LblMensajes.Visible = true;
                    LblMensajes.Text = "Número de documento de identidad no existe en la BD, ingrese otro";
                    TxtNumDoc.Text = "";
                    //// SI NO LO ENCUENTRA EN LA BD DEL SISTEMA TRAE LOS DATOS DE LA RENIEC
                    //if (TraerDatosReniec(TxtNumDoc.Text) == true) {
                    //    BolSeguir = true;
                    //}
                    //else {
                    //    //MessageBox.Show("Numero de documento no existe en la BD de la RENIEC, ingrese otro", "Sistema de Colas", MessageBoxButtons.OK);
                    //    LblMensajes.Visible = true;
                    //    LblMensajes.Text = "Numero de documento no existe en la BD de la RENIEC, ingrese otro";
                    //}
                }

                if (BolSeguir == true)
                {
                    LLamarVentanaTipoServicio(intTipoCliente, intPrioridadId, intTipoDocumento, intPersonaId);
                }
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            intPrioridadId = 7351;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            intPrioridadId = 7352;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            intPrioridadId = 7353;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            TxtNumDoc.Focus();
        }

        private void CboTicketeras_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BolCargando == false)
            {
                intTicketeraId = Convert.ToInt32(CboTicketeras.SelectedValue);
            }
        }

        private void CmdAceptaTicketera_Click(object sender, EventArgs e)
        {
            intTicketeraId = Convert.ToInt32(CboTicketeras.SelectedValue);
            MiFun.IniEscribir("PREFERENCIAS", "TICKETERA", Convert.ToString(CboTicketeras.SelectedValue), Environment.CurrentDirectory + "\\SGAC.ini");
            //MessageBox.Show("Se asigno correctamente la Ticketera " + CboTicketeras.Text + " a esta  unidad", "Sistema de Colas", MessageBoxButtons.OK);
            LblMensajes.Visible = true;
            LblMensajes.Text = "Se asigno correctamente la Ticketera " + CboTicketeras.Text + " a esta  unidad";
            CmdCancelaTicketera_Click(sender, e);
        }

        private void seleccionarTicketeraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SeleccionarTicketera();
        }

        private void CmdCancelaTicketera_Click(object sender, EventArgs e)
        {
            TxtNumDoc.Enabled = true;
            panel1.Visible = false;
            if (intTicketeraId == 0)
            {
                LblMensajes.Visible = true;
                LblMensajes.Text = "No ha especificado la Ticketera para este punto, No podra emitir Ticket";
                this.Close();
            }
        }

        //=============================================================================================================

        private void LLamarVentanaTipoServicio(int intTipoCli, int IntTipoPrio, int intTipDoc, int intIdPer)
        {
            FrmMenuServicio FSR = new FrmMenuServicio();
            FSR.intPrioridad = IntTipoPrio;
            FSR.intTipoCliente = intTipoCli;
            FSR.intPersonaId = intIdPer;

            FSR.strPersonaNombre = strPersonaNombre;
            FSR.ShowDialog();
            FSR.Close();
        }

        // ESTA FUNCION NO SE IMPLEMENTO PORQUE POR AHORA EL PROGRAMA NO ACCEDERA A LA BD DE LA RENIEC
        public bool TraerDatosReniec(string strNumDocumento)
        {
            bool b_Ok = false;

            return b_Ok;
        }

        private void SeleccionarTicketera()
        {
            BolCargando = true;
            DataSet dsLeerFicheroXML = new DataSet();
            DataTable DtResult = new DataTable();
            dsLeerFicheroXML.Clear();
            dsLeerFicheroXML.ReadXml(Program.strArchivo);
            DtResult = dsLeerFicheroXML.Tables[5];

            MiFun.ComboBoxCargarDataTable(CboTicketeras, DtResult, "tick_ITicketeraId", "tick_vNombre");

            CmdOk.Enabled = false;
            TxtNumDoc.Enabled = false;
            panel1.Left = ((this.Width - panel1.Width) / 2);
            panel1.Top = ((this.Height - panel1.Height) / 2);
            BolCargando = false;
            CboTicketeras.Focus();
            panel1.Visible = true;
        }

        private bool PersonaRuneBuscar2(string StrNumeroDocumento, int IntTipoDocumento, string StrArchivo, ref int IntPersonaId)
        {
            bool BolOk = false;

            int A;

            DataSet dsResult = new DataSet();
            dsResult.Clear();
            DataTable dtResult = new DataTable();

            dsResult.ReadXml(StrArchivo);
            dtResult = dsResult.Tables[0];

            for (A = 0; A < dtResult.Rows.Count; A++)
            {
                if (dtResult.Rows[A]["peid_vDocumentoNumero"].ToString().ToUpper() == StrNumeroDocumento.ToUpper())
                {
                    BolOk = true;
                    IntPersonaId = Convert.ToInt32(dtResult.Rows[A]["pers_iPersonaId"].ToString());
                    break;
                }
            }
            return BolOk;
        }

        public static string PersonaTraerNombre(int IntPersonaId, string StrArchivo)
        {
            int A;
            string vNombre = "";
            DataSet dsResult = new DataSet();
            dsResult.Clear();
            DataTable dtResult = new DataTable();

            dsResult.ReadXml(StrArchivo);
            dtResult = dsResult.Tables[0];

            for (A = 0; A < dtResult.Rows.Count; A++)
            {
                if (Convert.ToInt32(dtResult.Rows[A]["pers_iPersonaId"].ToString()) == IntPersonaId)
                {
                    vNombre = dtResult.Rows[A]["pers_vNombre"].ToString();
                    break;
                }
            }
            return vNombre;
        }

        private void TxtNumDoc_TextChanged(object sender, EventArgs e)
        {
        }

        private bool ValidarLongitud(string strCadena, int intTipo)
        {
            bool booOk = true;

            if (intTipo == 1)
            {
                int intLongitud = strCadena.Length;
                if (intLongitud == 8) { booOk = false; }
            }

            if (intTipo == 2)
            {
                int intLongitud = strCadena.Length;
                if (intLongitud == 12) { booOk = false; }
            }

            return booOk;
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            TxtNumDoc.Text = "";
            TxtNumDoc.Focus();
        }

        private void BtmCero_Click(object sender, EventArgs e)
        {
            if (ValidarLongitud(TxtNumDoc.Text, intTipoDocumento) == false) { return; }

            TxtNumDoc.Text = TxtNumDoc.Text + "0";
            TxtNumDoc.Focus();
        }

        private void BtnUno_Click(object sender, EventArgs e)
        {
            if (ValidarLongitud(TxtNumDoc.Text, intTipoDocumento) == false) { return; }

            TxtNumDoc.Text = TxtNumDoc.Text + "1";
            TxtNumDoc.Focus();
            SendKeys.Send("{End}");
        }

        private void BtnDos_Click(object sender, EventArgs e)
        {
            if (ValidarLongitud(TxtNumDoc.Text, intTipoDocumento) == false) { return; }

            TxtNumDoc.Text = TxtNumDoc.Text + "2";
            TxtNumDoc.Focus();
            SendKeys.Send("{End}");
        }

        private void BtnTres_Click(object sender, EventArgs e)
        {
            if (ValidarLongitud(TxtNumDoc.Text, intTipoDocumento) == false) { return; }

            TxtNumDoc.Text = TxtNumDoc.Text + "3";
            TxtNumDoc.Focus();
            SendKeys.Send("{End}");
        }

        private void BtnCuatro_Click(object sender, EventArgs e)
        {
            if (ValidarLongitud(TxtNumDoc.Text, intTipoDocumento) == false) { return; }

            TxtNumDoc.Text = TxtNumDoc.Text + "4";
            TxtNumDoc.Focus();
            SendKeys.Send("{End}");
        }

        private void BtnCinco_Click(object sender, EventArgs e)
        {
            if (ValidarLongitud(TxtNumDoc.Text, intTipoDocumento) == false) { return; }

            TxtNumDoc.Text = TxtNumDoc.Text + "5";
            TxtNumDoc.Focus();
            SendKeys.Send("{End}");
        }

        private void BtnSeis_Click(object sender, EventArgs e)
        {
            if (ValidarLongitud(TxtNumDoc.Text, intTipoDocumento) == false) { return; }

            TxtNumDoc.Text = TxtNumDoc.Text + "6";
            TxtNumDoc.Focus();
            SendKeys.Send("{End}");
        }

        private void BtnSiete_Click(object sender, EventArgs e)
        {
            if (ValidarLongitud(TxtNumDoc.Text, intTipoDocumento) == false) { return; }

            TxtNumDoc.Text = TxtNumDoc.Text + "7";
            TxtNumDoc.Focus();
            SendKeys.Send("{End}");
        }

        private void BtnOcho_Click(object sender, EventArgs e)
        {
            if (ValidarLongitud(TxtNumDoc.Text, intTipoDocumento) == false) { return; }

            TxtNumDoc.Text = TxtNumDoc.Text + "8";
            TxtNumDoc.Focus();
            SendKeys.Send("{End}");
        }

        private void BtnNueve_Click(object sender, EventArgs e)
        {
            if (ValidarLongitud(TxtNumDoc.Text, intTipoDocumento) == false) { return; }

            TxtNumDoc.Text = TxtNumDoc.Text + "9";
            TxtNumDoc.Focus();
            SendKeys.Send("{End}");
        }

        private void BtnBacspace_Click(object sender, EventArgs e)
        {
            CmdOk_Click(sender, e);
        }

        private void Degradar()
        {
        }

        private void FrmEmisionTicket_Paint(object sender, PaintEventArgs e)
        {
            ////LinearGradientBrush linGrBrush = new LinearGradientBrush(new Point(0, 0), new Point(200, 200), Color.FromArgb(255, 0, 0, 255), Color.FromArgb(255, 0, 255, 0));  // opaque green
            //Rectangle rect = new Rectangle(0, 0,this.Width, this.Height);
            //LinearGradientBrush linGrBrush = new LinearGradientBrush(rect, Color.Maroon, Color.White, LinearGradientMode.Horizontal);

            //Pen pen = new Pen(linGrBrush, 10);
            //e.Graphics.FillRectangle(linGrBrush, 0, 0, this.Width, this.Height);
        }

        private void CmdSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void TxtNumDoc_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            CmdOk_Click(sender, e);
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox2.Image = Properties.Resources.DownLupa3;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.Lupa3;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox2.Image = Properties.Resources.UpLupa3;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            LblMensajes.Visible = false;
        }

        private void BtnBacspace_MouseLeave(object sender, EventArgs e)
        {
            LblMensajes.Visible = false;
        }

        private void radioButton8_CheckedChanged_1(object sender, EventArgs e)
        {
        }
    }
}