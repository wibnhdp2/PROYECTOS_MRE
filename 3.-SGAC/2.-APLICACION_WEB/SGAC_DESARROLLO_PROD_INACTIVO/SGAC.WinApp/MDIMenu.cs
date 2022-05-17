using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

//using DS = Microsoft.DirectX.DirectSound;

namespace SGAC.WinApp
{
    public partial class MDIMenu : Form
    {
        private Funciones MiFun = new Funciones();

        public MDIMenu()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEmisionTicket xFrm = new FrmEmisionTicket();
            AbrirFormulario(xFrm);
        }

        private bool ExisteBD()
        {
            string strRutaBD = "";
            string strArchivoBD = "";
            bool bolOk = true;
            int intOficinaIdServidor = 0;

            strRutaBD = MiFun.IniLeer("PREFERENCIAS", "RUTABD", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            strArchivoBD = strRutaBD + "\\data.xml";

            if (strRutaBD != "")
            {
                //CARGAMOS LO DATOS DE LA OFICINA DEL SERVIDOR Colas.ini
                intOficinaIdServidor = Convert.ToInt32(MiFun.IniLeer("PARAMETROS", "OFICINACONSULAR", strRutaBD + "\\" + Program.strIniServidor));

                if (MiFun.ArchivoExiste(strArchivoBD) == false)
                {
                    DialogResult Result = MessageBox.Show("No se ha encontrado la BD del sistema de Cola, ¿ Desea generarla ahora ?", Program.strTituloSistema, MessageBoxButtons.YesNo);

                    if (Result == DialogResult.Yes)
                    {
                        if (Program.CrearDataLocal(strArchivoBD, intOficinaIdServidor, DateTime.Now.ToString()) == true)
                        {
                            MiFun.IniEscribir("LLAMAR", "NUMERO", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                            MiFun.IniEscribir("LLAMAR", "NOMBRE", ".", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                            MiFun.IniEscribir("LLAMAR", "RELLAMAR", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                        }
                        else
                        {
                            bolOk = false;
                            MessageBox.Show("No se pudo inicializar la BD del Sistema de Colas", Program.strTituloSistema, MessageBoxButtons.OK);
                            MiFun.VerArchivoTexto(Environment.CurrentDirectory + "\\testdatos.txt", "Inicializacion de Datos");
                            this.Close();
                            Application.Exit();
                        }
                    }
                    else
                    {
                        bolOk = false;
                    }
                }
            }
            else
            {
                bolOk = false;
            }
            return bolOk;
        }

        private void MDIMenu_Load(object sender, EventArgs e)
        {
            Boolean BolConfigurar = false;
            if (Program.vRutaBDLocal == "") { Program.CargarConstante(); }

            if (ExisteBD() == false)
            {
                MessageBox.Show("No se puede continuar el programa de colas sin una BD asignada", Program.strTituloSistema, MessageBoxButtons.OK);
                FrmConfiguracionSistema xFrm = new FrmConfiguracionSistema();
                xFrm.OrigenInvocacion = 1;   // LE INDICAMOS AL FORMULARIO QUE ES INVOCADO DESDE OTRA PARTE DEL MENU
                xFrm.ShowDialog();
            }

            if (Program.intServOficinaConsularId != 0)
            {
                FrmLogin Frm = new FrmLogin();
                Frm.ShowDialog();
            }

            Program.CargarConstante();                        // CARGAMOS LAS CONSTANTES DEL SISTEMA

            toolLblUsuario.Text = Program.strUsuario;         // .UsuarioNombre(Program.vRutaBDLocal.Trim() + @"\data.xml", Program.intUsuarioId);
        
            VerificarVideos();

            // VERIFICAMOS QUE LA EXISTA LA RUTA DE LA BASE DE DATOS
            if (Program.vRutaBDLocal == "")
            {
                MessageBox.Show("No ha indicado la ruta de Acceso para la BD del Sistema de Colas, Defina la Ruta para Continuar", Program.strTituloSistema, MessageBoxButtons.OK);
                BolConfigurar = true;
            }

            // CARGAMOS LA IMAGEN SELECCIONADA
            string StrArchivo = MiFun.IniLeer("PARAMETROS", "IMAGENFONDO", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            if (StrArchivo != "")
            {
                PicImagen.Left = 0;
                PicImagen.Top = 65;
                PicImagen.Width = this.Width;
                PicImagen.Height = this.Height - 65;
                PicImagen.Visible = true;
                PicImagen.Image = Image.FromFile(StrArchivo);
            }

            // PREGUTAMOS SI LA OFICINA CONSULAR DEL SERVIDOR COINCIDE CON LA OFICINA DE LA BASE DE DATOS
            string StrBDOficinaConsularId = Program.HallarOficinaConsularBD(Program.strArchivo);
            string StrBDOficinaConsularNombre = "";

            if (Program.strServOficinaConsularCodigo != StrBDOficinaConsularId)
            {
                StrBDOficinaConsularNombre = Program.TraeOficinaConsular(StrBDOficinaConsularId, Program.strArchivo);
                MessageBox.Show("La Oficina Consular de la BD es " + StrBDOficinaConsularNombre.ToUpper() + " no coincide con la Oficina Consular actual " + Program.strServOficinaConsularNombre + ", Inicialice la BD en la opción [Iniciar BD] del menú [Procesos]", Program.strTituloSistema, MessageBoxButtons.OK);
            }
            else
            {
                // BUSCAMOS SI EN LA DATA HAY TICKET DE DIAS ANTERIORES
                if (BuscarDiasAnteriores(DateTime.Now.ToShortDateString().ToString(), Program.vRutaBDLocal + Program.strIniServidor) == true)
                {
                    MessageBox.Show("Se han encontrado Tickets de dias anteriores, es necesario subir estos ticket al Servidor, para ello haga clic en la opción [Subir BD al Servidor] del menú [Procesos]", Program.strTituloSistema, MessageBoxButtons.OK);
                    MiFun.VerArchivoTexto(Environment.CurrentDirectory + "\\tickethallados.txt", "Sistema de Colas");
                }

                if (Program.intVentanillaId == 0)
                {
                    MessageBox.Show("El numero de ventanilla asignado al cliente no existe, defina un nuevo número de ventanilla, para ello haga clic en la opción [Parametros del Sistema] del menú [Procesos]", Program.strTituloSistema, MessageBoxButtons.OK);
                }
            }

            if (BolConfigurar == true)
            {
                FrmConfiguracionSistema xFrm = new FrmConfiguracionSistema();
                xFrm.ShowDialog();
                Application.Exit();
                return;
            }
        }

        private void MDIMenu_Activated(object sender, EventArgs e)
        {
            string strVersion = MiFun.ObtenerVersionAplicacion();
            // MOSTRAMOS LA OFICINA CONSULAR DEL CLIENTE, QUE DEBE DE SER IGUAL AL DEL SERVIDOR
            this.Text = Program.strTituloSistema.ToUpper() + " - (" + Program.strServOficinaConsularNombre + ")" + " " + strVersion;
        }

        private void subirBDAlServidorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCargaDatos xFrm = new FrmCargaDatos();
            AbrirFormulario(xFrm);
        }

        private void iniciarBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDescargaDatos xFrm = new FrmDescargaDatos();
            AbrirFormulario(xFrm);
        }

        private void llamarUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTicketLlamada xFrm = new FrmTicketLlamada();
            AbrirFormulario(xFrm);
        }

        private void emisionDeTicketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEmisionTicket xFrm = new FrmEmisionTicket();
            AbrirFormulario(xFrm);
        }

        private void mostrarPantallaDeLlamadasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTelevisorDisplay xFrm = new FrmTelevisorDisplay();
            //AbrirFormulario(xFrm);

            xFrm.ShowDialog();            
            //xFrm = null;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FrmEmisionTicket xFrm = new FrmEmisionTicket();
            AbrirFormulario(xFrm);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FrmTicketLlamada xFrm = new FrmTicketLlamada();
            xFrm.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //FrmDemos xFrm = new FrmDemos();
            //xFrm.Show();

            //CrystalReport2.rpt

            //string VRutaArchivo = "";
            //Funciones FunFunciones = new Funciones();

            //VRutaArchivo = Environment.CurrentDirectory + "\\Reportes\\CrystalReport2.rpt";

            //string[,] Parametros = new string[4, 2] {{"OficinaConsular","Mi casa"},
            //                                         {"Titulo1","Titulo 1"},
            //                                         {"Titulo2","(TICKET DEL DIA)"},
            //                                         {"FechaEmision","01/01/2014"},
            //                                        };

            //FunFunciones.CrystalVisor(Parametros, "Mi Reportes", VRutaArchivo);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            FrmReporteTipos xFrm = new FrmReporteTipos();
            xFrm.ShowDialog();
        }

        private void asignarImpresoraPuntoDeEmisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmReporteTipos xFrm = new FrmReporteTipos();
            //xFrm.ShowDialog();
            AbrirFormulario(xFrm);
        }

        private void parametrosDelSistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmConfiguracionSistema xFrm = new FrmConfiguracionSistema();
            xFrm.ShowDialog();
        }

        // ----------------------------------------------------------------------------------------------------------------------

        private void AbrirFormulario(Form xForm)
        {
            bool BolCargado = false;

            foreach (Form xF in Application.OpenForms)
            {
                if (xF.Name == xForm.Name)
                {
                    BolCargado = true;
                    break;
                }
            }
            if (BolCargado == false)
            {
                xForm.Show();
            }
        }

      

        private bool BuscarDiasAnteriores(string StrFechaActual, string StrArchivoDatos)
        {
            bool BolOk = false;
            DataSet DsBD = new DataSet();
            DataTable DtTicket = new DataTable();
            DataTable DtTicketFiltrado = new DataTable();

            DsBD = Program.AbrirArchivoDatos(Program.vRutaBDLocal + "\\data.xml");
            DtTicket = DsBD.Tables["Table1"];

            DtTicketFiltrado = DataTableAgregarCampo(DtTicket, "tick_FechaEmision", "tick_dFechaHoraGeneracion");
            try
            {
                var dt_filtro = (from dt in DtTicketFiltrado.AsEnumerable()
                                 where Convert.ToInt32(dt["tick_bCargado"]) == 0
                                 select dt).CopyToDataTable();


                DtTicketFiltrado = dt_filtro.Copy();
                dt_filtro.Dispose();                
            }
            catch
            {
                DtTicketFiltrado = new DataTable();
            }

            if (DtTicketFiltrado.Columns.Count > 0)
            {
                DtTicketFiltrado = MiFun.DataTableDistinc("Table1", DtTicketFiltrado, "tick_FechaEmision", "tick_iNumero");

                if (DtTicketFiltrado.Rows.Count != 0)
                {
                    if (DtTicketFiltrado.Rows[0]["tick_iNumero"].ToString() != "0")
                    {
                        // SI HAY DIAS ANTERIORES MOSTRAMOS ESCRIBIMOS LA LISTA DE DIAS EN UN ARCHIVO DE TEXTO
                        int IntFila = 0;
                        string StrCadena = "";
                        string StrNombreArchivo = Environment.CurrentDirectory + "\\tickethallados.txt";

                        StreamWriter ObjArchivo = new StreamWriter(StrNombreArchivo);

                        StrCadena = "                                 REPORTE SISTEMA DE COLAS";
                        ObjArchivo.WriteLine(StrCadena);
                        StrCadena = "                                 ========================";
                        ObjArchivo.WriteLine(StrCadena);
                        StrCadena = "";
                        ObjArchivo.WriteLine(StrCadena);

                        StrCadena = " Se han encontrado tickets en los siguientes dias:";
                        ObjArchivo.WriteLine(StrCadena);

                        StrCadena = "    DIA    ";
                        ObjArchivo.WriteLine(StrCadena);

                        StrCadena = " ----------";
                        ObjArchivo.WriteLine(StrCadena);

                        for (IntFila = 0; IntFila <= DtTicketFiltrado.Rows.Count - 1; IntFila++)
                        {
                            StrCadena = " " + (DtTicketFiltrado.Rows[IntFila]["tick_FechaEmision"].ToString()).Remove(0, 6) +
                                "/" + (DtTicketFiltrado.Rows[IntFila]["tick_FechaEmision"].ToString()).Substring(4, 2) +
                                "/" + (DtTicketFiltrado.Rows[IntFila]["tick_FechaEmision"].ToString()).Remove(4, 4);
                            ObjArchivo.WriteLine(StrCadena);
                        }
                        ObjArchivo.Close();

                        BolOk = true;
                    }
                }
            }
            return BolOk;
        }

        private DataTable DataTableAgregarCampo(DataTable DtModificar, string StrNombreCampo, String StrCampoCopiar)
        {
            DataTable DtNuevo = new DataTable();
            string StrCadena = "";
            int IntFilas = 0;

            DtNuevo = DtModificar;
            DtNuevo.Columns.Add(StrNombreCampo, typeof(string));

            for (IntFilas = 0; IntFilas <= DtNuevo.Rows.Count - 1; IntFilas++)
            {
                StrCadena = DtNuevo.Rows[IntFilas][StrCampoCopiar].ToString();
                if (StrCadena.Length != 0)
                {
                    DtNuevo.Rows[IntFilas][StrNombreCampo] = StrCadena.Substring(0, 8);
                }
            }
            return DtNuevo;
        }

        private void VerificarVideos()
        {
            //bool BolOk = false;
            DataSet DsBD = new DataSet();
            DataTable DtVideo = new DataTable();
            int IntFila = 0;
            string strArchivoNombre = "";
            string texto = "";
            int intNumeroVideos = 0;

            DsBD = Program.AbrirArchivoDatos(Program.vRutaBDLocal + "\\data.xml");
            DtVideo = DsBD.Tables["Table8"];

            string strRutaArchivo = @"" + Program.vRutaVideos + "\\" + "Reproduce.wpl";
            if (MiFun.ArchivoExiste(strRutaArchivo) == false)
            {
                MiFun.ArchivoBorrar(strRutaArchivo);
            }

            string Archivo = @"" + Program.vRutaVideos + "\\Reproduce.wpl";
            System.IO.StreamWriter sw = new System.IO.StreamWriter(Archivo);

            //ESCRIBIMOS LA LISTA DE REPRODUCCION
            texto = "<?wpl version=\"1.0\"?>";
            sw.WriteLine(texto);
            texto = "<smil>";
            sw.WriteLine(texto);
            texto = "    <head>";
            sw.WriteLine(texto);
            texto = "        <meta name=\"Generator\" content=\"Microsoft Windows Media Player -- 12.0.7601.18150\"/>";
            sw.WriteLine(texto);
            texto = "        <meta name=\"ItemCount\" content=\"0\"/>";
            sw.WriteLine(texto);
            texto = "        <meta name=\"IsFavorite\"/>";
            sw.WriteLine(texto);
            texto = "        <meta name=\"ContentPartnerListID\"/>";
            sw.WriteLine(texto);
            texto = "        <meta name=\"ContentPartnerNameType\"/>";
            sw.WriteLine(texto);
            texto = "        <meta name=\"ContentPartnerName\"/>";
            sw.WriteLine(texto);
            texto = "        <meta name=\"Subtitle\"/>";
            sw.WriteLine(texto);
            texto = "        <author/>";
            sw.WriteLine(texto);
            texto = "        <title></title>";
            sw.WriteLine(texto);
            texto = "    </head>";
            sw.WriteLine(texto);
            texto = "    <body>";
            sw.WriteLine(texto);
            texto = "        <seq>";
            sw.WriteLine(texto);

            for (IntFila = 0; IntFila <= DtVideo.Rows.Count - 1; IntFila++)
            {
                //strArchivoNombre = @"" + Program.vRutaVideos + "\\VIDEO01.MP4"; 
                strArchivoNombre = @"" + Program.vRutaVideos + "\\" + DtVideo.Rows[IntFila]["vide_vUrl"].ToString();
                if (DtVideo.Rows[IntFila]["vide_vUrl"].ToString().ToUpper().IndexOf("YOUTUBE") == -1)
                {
                    if (MiFun.ArchivoExiste(strArchivoNombre) == true)
                    {
                        strArchivoNombre = strArchivoNombre.ToUpper();

                        texto = "            <media src=\"" + strArchivoNombre + "\"/>";
                        sw.WriteLine(texto);
                        intNumeroVideos = intNumeroVideos + 1;
                    }
                }
            }
            texto = "        </seq>";
            sw.WriteLine(texto);
            texto = "    </body>";
            sw.WriteLine(texto);
            texto = "</smil>";
            sw.WriteLine(texto);
            sw.Close();

            if (intNumeroVideos == 0)
            {
                MiFun.ArchivoBorrar(strRutaArchivo);
            }
        }

        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       
    }
}