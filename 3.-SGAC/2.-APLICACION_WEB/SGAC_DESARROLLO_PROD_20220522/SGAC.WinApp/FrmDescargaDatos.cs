using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SGAC.WinApp
{
    public partial class FrmDescargaDatos : Form
    {
        private Funciones MiFun = new Funciones();
        private bool BolPararHilo = false;
        public int intDeDonde = 0;                         // INDICA DE DONDE SE ESTA INVOCANDO AL FORMULARIO (1 = DESDE CUALQUIER SITIO MENOS DEL MENU ;  0 = DESDE EL MENU PRIMNCIPAL)
        public int intOficinaConsularId;                   // INDICA EL ID DE LA OFICINA CONSULAR, SOLO SE TOMARA EN CUENTA CUANDO intDeDonde = 1
        public string strRutaBD = "";                      // TRAE LA RUTA DE LA BD  SOLO SE TOMARA EN CUENTA CUANDO intDeDonde = 1

        public FrmDescargaDatos()
        {
            InitializeComponent();
        }

        private void FrmIniciarTicket_Load(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            this.Text = Program.strTituloSistema + " - " + "Inicializar Base de Datos";
            if (intDeDonde == 1)
            {
                Program.strServOficinaConsularCodigo = intOficinaConsularId.ToString();
                Program.strArchivo = strRutaBD + "\\data.xml";
                CmdInicio_Click(sender, e);
            }
        }

        private void CmdSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CmdInicio_Click(object sender, EventArgs e)
        {
            // ******************************
            // * PARA BAJAR DEL WEB SERVICE *
            // ******************************
            CmdInicio.Enabled = false;
            CmdInicio.BackColor = Color.Gray;
            Application.DoEvents();
            Cursor.Current = Cursors.WaitCursor;
           // DialogResult Result = MessageBox.Show("¿ Esta seguro de inicializar la BD Local del sistema de Colas ?", Program.strTituloSistema, MessageBoxButtons.YesNo);
           // MessageBox.Show("Se inicializará la BD Local del sistema de Colas", Program.strTituloSistema, MessageBoxButtons.OK);
            //if (Result == DialogResult.Yes)
            //{
                if (!backgroundWorker1.IsBusy)
                {
                    if (Program.CrearDataLocal(Program.strArchivo, Convert.ToInt32(Program.strServOficinaConsularCodigo), DateTime.Now.ToString(Program.strFormatoFecha)) == true)
                    {
                        backgroundWorker1.RunWorkerAsync();
                        MiFun.IniEscribir("LLAMAR", "NUMERO", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                        MiFun.IniEscribir("LLAMAR", "NOMBRE", ".", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                        MiFun.IniEscribir("LLAMAR", "RELLAMAR", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                        MiFun.IniEscribir("LLAMAR", "VENTANA", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                        MiFun.IniEscribir("LLAMAR", "IDTICKETFINALIZADO", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                        MiFun.IniEscribir("LLAMAR", "IDTICKET", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);

                        // AQUI GUARDAMOS EN LA TABLA DE USUARIOS LOS USUARIOS ACTIVOS DEL CONSULADO, SE ASIGNA SIN VENTANILLA PARA QUE EL
                        //USUARIO LOS CONFIGURE
                        GenerarUsuarios();
                    }
                    else
                    {
                        BolPararHilo = true;
                        backgroundWorker1.CancelAsync();

                        MiFun.VerArchivoTexto(Environment.CurrentDirectory + "\\testdatos.txt", "Inicializacion de Datos");

                        MiFun.IniEscribir("PREFERENCIAS", "RUTABD", "", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
                        MiFun.IniEscribir("PREFERENCIAS", "RUTAVIDEO", "", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
                        MiFun.IniEscribir("PREFERENCIAS", "RUTAREPORTE", "", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
                        MiFun.IniEscribir("PREFERENCIAS", "IMPRESORA", "", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
                        MiFun.IniEscribir("PREFERENCIAS", "DOMINIORUTA", "", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
                        MiFun.IniEscribir("PREFERENCIAS", "DOMINIONOMBRE", "", Environment.CurrentDirectory + "\\" + Program.strIniLocal);

                        this.Close();
                        Application.Exit();
                    }
                }
            //}

            Cursor.Current = Cursors.Arrow;
        }

        private void GenerarUsuarios()
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();
            DataTable dtVentanilla = new DataTable();
            Funciones miFun = new Funciones();
            int intFila = 0;
            int intVentanillaId = 0;
            string XMLArchivo = Program.vRutaBDLocal + @"\usuarios.xml";

            // *********************************************************************
            // ELIMINAMOS LOS USUARIOS DEL TABLA USUARFIO EN EL ARCHIVO usuarios.xml
            dsResult = Program.AbrirArchivoDatos(Program.vRutaBDLocal + @"\usuarios.xml");
            dtResult = dsResult.Tables[0];

            for (intFila = 1; intFila <= dtResult.Rows.Count - 1; intFila++)
            {
                miFun.XMLEliminarNodo(XMLArchivo, "Table", "veus_iUsuarioId", dtResult.Rows[intFila]["veus_iUsuarioId"].ToString());
            }

            dsResult = Program.AbrirArchivoDatos(Program.vRutaBDLocal + @"\data.xml");
            dtResult = dsResult.Tables[11];
            dtVentanilla = dsResult.Tables[6];

            intVentanillaId = 0;

            for (intFila = 0; intFila <= dtResult.Rows.Count - 1; intFila++)
            {
                object[] MisCampos = new object[4] { new XElement("veus_iUsuarioId", dtResult.Rows[intFila]["usua_sUsuarioId"].ToString()),
                                                     new XElement("veus_iVentanillaId",intVentanillaId.ToString()),
                                                     new XElement("veus_vUsuario", dtResult.Rows[intFila]["usua_vAlias"].ToString()),
                                                     new XElement("veus_vVentanilla", "0"),
                                                   };

                miFun.XMLAgregarNodo(XMLArchivo, "Table", MisCampos);
            }

            // *******************************************************************************
            // BORRAMOS EL PRIMER REGISTRO, YA QUE ESTE REGISTRO PERTENECE A LA CARGA ANTERIOR
            dsResult = Program.AbrirArchivoDatos(Program.vRutaBDLocal + @"\usuarios.xml");
            dtResult = dsResult.Tables[0];

            for (intFila = 0; intFila < 1; intFila++)
            {
                miFun.XMLEliminarNodo(XMLArchivo, "Table", "veus_iUsuarioId", dtResult.Rows[intFila]["veus_iUsuarioId"].ToString());
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                //Realiza una tarea
                System.Threading.Thread.Sleep(100);
                backgroundWorker1.ReportProgress(i);
                if (backgroundWorker1.CancellationPending)
                    return;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Notificar el progreso de la tarea
            progressBar1.Value = e.ProgressPercentage;
            lblInfo.Text = e.ProgressPercentage + "%";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            {
                if (BolPararHilo == false)
                {
                    //Realizamos las operaciones que haya que realizar al terminar el progreso
                    lblInfo.Text = "Tarea terminada";
                    MessageBox.Show("La BD del Sistema de Colas se inicio con exito", Program.strTituloSistema, MessageBoxButtons.OK);
                    this.Close();
                    //btnCancelar.Enabled = false;
                    CmdInicio.Enabled = true;
                    progressBar1.Value = 0;
                }
            }
        }

    }
}