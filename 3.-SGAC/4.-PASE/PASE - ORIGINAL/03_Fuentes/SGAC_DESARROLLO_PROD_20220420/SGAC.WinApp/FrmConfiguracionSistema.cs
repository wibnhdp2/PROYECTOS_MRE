using System;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace SGAC.WinApp
{
    public partial class FrmConfiguracionSistema : Form
    {
        private Funciones MiFun = new Funciones();
        private DataTable DtVentanillas = new DataTable();
        private int intDeDonde = 0;

        public int OrigenInvocacion
        {
            get { return intDeDonde; }
            set { intDeDonde = value; }
        }

        public FrmConfiguracionSistema()
        {
            InitializeComponent();
        }

        private void FrmConfigurar_Load(object sender, EventArgs e)
        {
            this.Text = Program.strTituloSistema + " - Configuracion de Parametros";
            if (MiFun.ArchivoExiste(Program.strArchivo) == false)
            {
                MessageBox.Show("El archivo de datos no existe, especifique la Ruta", Program.strTituloSistema, MessageBoxButtons.OK);
            }
            else
            {
                CargarCombos(Program.strArchivo);
                MostrarDatos();
            }
        }

        private void CmdCancelar_Click(object sender, EventArgs e)
        {
            if (intDeDonde == 1)
            {
                //MessageBox.Show("No se ha configurado el sistema, no se puede continuar", Program.strTituloSistema, MessageBoxButtons.OK);
                //this.Close();
                Application.Exit();
            }
            else
            {
                this.Close();
            }
        }

        private void CmdAceptar_Click(object sender, EventArgs e)
        {
            #region VALIDACION DATOS
            if (TxtRutaBD.Text == "")
            {
                MessageBox.Show("No ha especificado la ruta de la BD", Program.strTituloSistema, MessageBoxButtons.OK);
                TxtRutaBD.Focus();
                return;
            }

            if (TxtRutaVideo.Text == "")
            {
                MessageBox.Show("No ha especificado la ruta para los videos del sistema", Program.strTituloSistema, MessageBoxButtons.OK);
                TxtRutaBD.Focus();
                return;
            }

            if (TxtRutaRep.Text == "")
            {
                MessageBox.Show("No ha especificado la ruta para los reportes del sistema", Program.strTituloSistema, MessageBoxButtons.OK);
                TxtRutaBD.Focus();
                return;
            }

            if (CboOficina.SelectedIndex == -1)
            {
                MessageBox.Show("No ha especificado la Oficina Consular", Program.strTituloSistema, MessageBoxButtons.OK);
                CboOficina.Focus();
                return;
            }

            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("No ha especificado el nombre de la impresora para la emisión de Ticket", Program.strTituloSistema, MessageBoxButtons.OK);
                listBox1.Focus();
                return;
            }

            //if (TxtDominioRuta.Text == "")
            //{
            //    MessageBox.Show("No ha especificado la ruta del Dominio", Program.strTituloSistema, MessageBoxButtons.OK);
            //    TxtDominioRuta.Focus();
            //    return;
            //}

            //if (TxtDominioNombre.Text == "")
            //{
            //    MessageBox.Show("No ha especificado el nombre del Dominio", Program.strTituloSistema, MessageBoxButtons.OK);
            //    TxtDominioNombre.Focus();
            //    return;
            //}

            if ((TxtNumLLamadas.Text == "") || (TxtNumLLamadas.Text == "0"))
            {
                MessageBox.Show("No ha indicado el numero de veces que se llamara a un Ticket", Program.strTituloSistema, MessageBoxButtons.OK);
                TxtNumLLamadas.Text = "";
                TxtNumLLamadas.Focus();
                return;
            }
            #endregion

            #region ESCRIBIR INI
            // ***********************
            // ESCRIBIMOS EL INI LOCAL

            // RUTA DE LA BD
            MiFun.IniEscribir("PREFERENCIAS", "RUTABD", TxtRutaBD.Text, Environment.CurrentDirectory + "\\" + Program.strIniLocal);

            // RUTA DE LOS VIDEO
            MiFun.IniEscribir("PREFERENCIAS", "RUTAVIDEO", TxtRutaVideo.Text, Environment.CurrentDirectory + "\\" + Program.strIniLocal);

            // RUTA DE LOS REPORTES
            MiFun.IniEscribir("PREFERENCIAS", "RUTAREPORTE", TxtRutaRep.Text, Environment.CurrentDirectory + "\\" + Program.strIniLocal);

            //////// NUMERO DE VENTANILLA PARA ESTE CLIENTE
            //////if (Convert.ToInt32(CboVentanilla.SelectedValue) != 0)
            //////{
            //////    MiFun.IniEscribir("PREFERENCIAS", "VENTANILLA", CboVentanilla.SelectedValue.ToString(), Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            //////}

            // NOMBRE DE LA IMPRESORA QUE SE SETEO PARA ESTE CLIENTE
            MiFun.IniEscribir("PREFERENCIAS", "IMPRESORA", listBox1.SelectedItem.ToString(), Environment.CurrentDirectory + "\\" + Program.strIniLocal);

            // ID DEL USUARIO ASIGNADO AL CLIENTE
            if (Convert.ToInt32(CboUsuario.SelectedValue) != 0)
            {
                MiFun.IniEscribir("PREFERENCIAS", "USUARIOVENTANILLA", CboUsuario.SelectedValue.ToString(), Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            }

            // ************************
            // ESCRIBIMOS EL INI DE RED

            // ID DEL OFICINA CONSULAR
            MiFun.IniEscribir("PREFERENCIAS", "OFICINACONSULAR", CboOficina.SelectedValue.ToString(), Program.vRutaBDLocal + "\\" + Program.strIniServidor);

            // ID DEL VIDEO YOUTUBE
            MiFun.IniEscribir("PREFERENCIAS", "YOUTUBE", cboYoutube.SelectedValue.ToString(), Program.vRutaBDLocal + "\\" + Program.strIniServidor);




            // INDICA SI SE IMPRIMIRA UN TICKET
            if (OptNo.Checked == true) { MiFun.IniEscribir("PARAMETROS", "IMPRIMIR", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor); }
            if (OptSi.Checked == true) { MiFun.IniEscribir("PARAMETROS", "IMPRIMIR", "1", Program.vRutaBDLocal + "\\" + Program.strIniServidor); }

            // NUMERO DE VECES QUE SERA LLAMADO UN TIKECT
            MiFun.IniEscribir("PARAMETROS", "TIKECTNUMEROLLAMADAS", TxtNumLLamadas.Text, Program.vRutaBDLocal + "\\" + Program.strIniServidor);

            // ARCHIVO DE IMAGEN DEL FONDO DEL MENU
            MiFun.IniEscribir("PARAMETROS", "IMAGENFONDO", TxtFondoEscritorio.Text, Program.vRutaBDLocal + "\\" + Program.strIniServidor);

            // TAMAÑO DEL TICKET
            if (CboTamañoTicket.Text != "")
            {
                if (CboTamañoTicket.SelectedValue.ToString() != "")
                {
                    MiFun.IniEscribir("PARAMETROS", "TICKETTAMANO", CboTamañoTicket.SelectedValue.ToString(), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                }
            }

            // NUMERO DE TICKETERA ASIGNADA AL CLIENTE
            if (CboTicketera.Text != "")
            {
                if (Convert.ToInt32(CboTicketera.SelectedValue) != 0)
                {
                    MiFun.IniEscribir("PARAMETROS", "TICKETERA", CboTicketera.SelectedValue.ToString(), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                }
            }

            // ACTUALIZAMOS LOS DATOS DE LA FUENTE
            if (CboFuente.Text != "")
            {
                if (CboFuente.SelectedItem.ToString() != "")
                {
                    MiFun.IniEscribir("PARAMETROS", "FUENTE", CboFuente.SelectedItem.ToString(), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                }
            }

            if (CboTamaño.Text != "")
            {
                if (CboTamaño.SelectedItem.ToString() != "")
                {
                    MiFun.IniEscribir("PARAMETROS", "FUENTETAMAÑO", CboTamaño.SelectedItem.ToString(), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                }
            }

            MessageBox.Show("Los parámetros se establecierón con éxito", Program.strTituloSistema, MessageBoxButtons.OK);

            // ACTUALIZAMOS LA VARIABLE  Program.IImprimirTicket CON LOS NUEVOS VALORES SETEADOS
            if ((MiFun.IniLeer("PREFERENCIAS", "IMPRIMIR", Environment.CurrentDirectory + "\\" + Program.strIniLocal) == "") ||
                (MiFun.IniLeer("PREFERENCIAS", "IMPRIMIR", Environment.CurrentDirectory + "\\" + Program.strIniLocal) == "0"))
            {
                Program.intImprimirTicket = 0;
            }
            else
            {
                Program.intImprimirTicket = 1;
            }

            // SI EL ID DE LA OFICINA CONSULAR HA CAMBIADO REINICIAMOS EL SISTEMA DE COLAS
            if (Program.strServOficinaConsularCodigo == "") { Program.strServOficinaConsularCodigo = "0"; }
            if (Program.vRutaBDLocal == "") { Program.vRutaBDLocal = TxtRutaBD.Text; }


            //// SE QUITA ESTOS DATOS POR QUE NO APLICA LA AUTENTICACION DEL USUARIO A TRAVEZ DE ESTA APLICACION
            ////MiFun.IniEscribir("PREFERENCIAS", "DOMINIORUTA", TxtDominioRuta.Text, Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            ////MiFun.IniEscribir("PREFERENCIAS", "DOMINIONOMBRE", TxtDominioNombre.Text, Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            #endregion

            #region GUARDAR USUARIOS
            int intFila = 0;
            DataGridViewCell dgc;
            int intUsuarioId = 0;
            int intVentanillaId = 0;
            string strArchivo = Program.vRutaBDLocal + "\\usuarios.xml";

            for (intFila = 0; intFila < DgUsuarioVentanilla.Rows.Count; intFila++)
            {
                dgc = DgUsuarioVentanilla.Rows[intFila].Cells[2];
                intUsuarioId = Convert.ToInt32(dgc.Value);

                dgc = DgUsuarioVentanilla.Rows[intFila].Cells[1];
                intVentanillaId = Convert.ToInt32(dgc.Value);

                bool BolOk = false;
                string[,] Valores = new string[2, 3] {
                                                    { "veus_iUsuarioId", intUsuarioId.ToString(), "C" },
                                                    { "veus_iVentanillaId", intVentanillaId.ToString(), "" },
                                                  };
                //
                BolOk = MiFun.XMLModificarNodo(strArchivo, "Table", Valores);
            }
            #endregion

            if (Convert.ToInt32(Program.strServOficinaConsularCodigo) != Convert.ToInt32(CboOficina.SelectedValue))
            {
                MiFun.IniEscribir("PARAMETROS", "OFICINACONSULAR", CboOficina.SelectedValue.ToString(), Program.vRutaBDLocal + "\\" + Program.strIniServidor);

                MessageBox.Show("ha cambiado la configuración del Sistema de Colas, Se requiere descargar la Nueva Base de Datos.", Program.strTituloSistema, MessageBoxButtons.OK);
                FrmDescargaDatos xFrm = new FrmDescargaDatos();
                xFrm.intOficinaConsularId = Convert.ToInt32(CboOficina.SelectedValue);
                xFrm.strRutaBD = TxtRutaBD.Text;
                xFrm.intDeDonde = 1;
                xFrm.ShowDialog();
                MessageBox.Show("Se cerrará el sistema para inicializarse con los nuevos datos obtenidos. Debe abrir nuevamente el sistema.", Program.strTituloSistema, MessageBoxButtons.OK);
                Application.Exit();
            }

            Program.CargarConstante();
            this.Close();
        }

        private void CmdBusFondo_Click(object sender, EventArgs e)
        {
            string StrFiltro = "Archivos de Imagen JGP files (*.jpg)|*.jpg";
            //"Bitmap files (*.bmp)|*.bmp|Gif files (*.gif)|*.gif|JGP files (*.jpg)|*.jpg|All (*.*)|*.* |PNG (*.patito)|*.png ";
            string StrArchivo = MiFun.ShowDialogVerArchivo(openFileDialog1, StrFiltro, 1, "Busqueda de imagenes", "Sistema de Colas");

            if (StrArchivo != "")
            {
                TxtFondoEscritorio.Text = StrArchivo;
            }
        }

        private void CmdBusData_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                TxtRutaBD.Text = folderBrowserDialog1.SelectedPath;
                TxtRutaBD.Refresh();

                int intOficinaId = Convert.ToInt32(MiFun.IniLeer("PARAMETROS", "OFICINACONSULAR", TxtRutaBD.Text.Trim() + "\\" + Program.strIniServidor));

                Program.intServOficinaConsularId = intOficinaId;

                CargarCombos(TxtRutaBD.Text + "\\data.xml");

                CboOficina.SelectedValue = Program.intServOficinaConsularId;
                //MostrarUsuarios();
            }
        }

        // *************************************************************************************************************

        private void MostrarDatos()
        {
            string StrValor;

            // ************************
            // TRAEMOS LA RUTA DE LA BD
            TxtRutaBD.Text = MiFun.IniLeer("PREFERENCIAS", "RUTABD", Environment.CurrentDirectory + "\\" + Program.strIniLocal);

            // TRAEMOS LA RUTA DEL VIDEO
            TxtRutaVideo.Text = MiFun.IniLeer("PREFERENCIAS", "RUTAVIDEO", Environment.CurrentDirectory + "\\" + Program.strIniLocal);

            // TRAEMOS LA RUTA DE LOS REPORTES
            TxtRutaRep.Text = MiFun.IniLeer("PREFERENCIAS", "RUTAREPORTE", Environment.CurrentDirectory + "\\" + Program.strIniLocal);

            // TRAEMOS EL NOMBRE DE LA OFICINA CONSULAR
            StrValor = MiFun.IniLeer("PARAMETROS", "OFICINACONSULAR", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            if ((StrValor == "") || (StrValor == "0"))
            {
                CboOficina.SelectedValue = 0;
            }
            else
            {
                CboOficina.SelectedValue = StrValor;
            }

            // TRAEMOS EL NOMBRE DEL VIDEO YOUTUBE
            StrValor = MiFun.IniLeer("PREFERENCIAS", "YOUTUBE", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            if ((StrValor == "") || (StrValor == "0"))
            {
                cboYoutube.SelectedValue = 0;
            }
            else
            {
                cboYoutube.SelectedValue = StrValor;
            }



            // NUMERO DE LLAMADAS PARA UN TICKET
            StrValor = MiFun.IniLeer("PARAMETROS", "TIKECTNUMEROLLAMADAS", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            if (StrValor == "") { StrValor = "0"; }
            TxtNumLLamadas.Text = StrValor;

            string VImprimir;
            // INDICA SI SE IMPRIMIRAN LOS TICKET
            VImprimir = MiFun.IniLeer("PARAMETROS", "IMPRIMIR", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            if (VImprimir == "") { VImprimir = "0"; }
            if (VImprimir == "0") { OptNo.Checked = true; }
            if (VImprimir == "1") { OptSi.Checked = true; }

            // ARCHIVO DE IMAGEN PARA EL FONDO DEL MENU
            string StrNombreArchivo;
            StrNombreArchivo = MiFun.IniLeer("PARAMETROS", "IMAGENFONDO", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            TxtFondoEscritorio.Text = StrNombreArchivo;

            // TAMAÑO DEL TICKET
            StrValor = MiFun.IniLeer("PARAMETROS", "TICKETTAMANO", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            if ((StrValor == "") || (StrValor == "0"))
            {
                CboTamañoTicket.SelectedValue = 0;
            }
            else
            {
                CboTamañoTicket.SelectedValue = StrValor;
            }

            // TRAEMOS EL CODIGO DE LA TIKETERA ASIGNADA AL CLIENTE
            StrValor = MiFun.IniLeer("PARAMETROS", "TICKETERA", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            if ((StrValor == "") || (StrValor == "0"))
            {
                CboTicketera.SelectedValue = 0;
            }
            else
            {
                CboTicketera.SelectedValue = StrValor;
            }

            // CARGAMOS LAS FUENTES DISPONIBLES Y SU TAMAÑO
            string strFuenteNombre = "";
            int intFuenteTamaño = 0;
            string strFuenteTamaño = "";

            strFuenteNombre = MiFun.IniLeer("PARAMETROS", "FUENTE", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            strFuenteTamaño = MiFun.IniLeer("PARAMETROS", "FUENTETAMAÑO", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            if (strFuenteTamaño == "")
            {
                intFuenteTamaño = 0;
            }
            else
            {
                intFuenteTamaño = Convert.ToInt32(strFuenteTamaño);
            }

            if (strFuenteNombre != "") { CboFuente.SelectedItem = strFuenteNombre; }
            if (intFuenteTamaño != 0) { CboTamaño.SelectedItem = Convert.ToString(intFuenteTamaño).Trim(); }

            // *******************************
            // TRAEMOS LOS DATOS DEL INI LOCAL

            // TRAEMOS EL ID DEL USUARIO ASIGNADO AL CLIENTE
            StrValor = MiFun.IniLeer("PREFERENCIAS", "USUARIOVENTANILLA", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            if ((StrValor == "") || (StrValor == "0"))
            {
                CboUsuario.SelectedValue = 0;
            }
            else
            {
                CboUsuario.SelectedValue = StrValor;
            }

            // TRAEMOS EL NUMERO VENTANILLA ASIGNADO AL CLIENTE
            StrValor = MiFun.IniLeer("PREFERENCIAS", "VENTANILLA", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            if ((StrValor == "") || (StrValor == "0"))
            {
                CboVentanilla.SelectedValue = 0;
            }
            else
            {
                CboVentanilla.SelectedValue = StrValor;
            }

            // TRAEMOS EL NOMBRE DE LA IMPRESORA ASIGNADA AL CLIENTE
            StrValor = MiFun.IniLeer("PREFERENCIAS", "IMPRESORA", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            if ((StrValor == "") || (StrValor == "0"))
            {
                listBox1.SelectedItem = 0;
            }
            else
            {
                listBox1.SelectedItem = StrValor;
            }

            TxtDominioRuta.Text = MiFun.IniLeer("PREFERENCIAS", "DOMINIORUTA", Environment.CurrentDirectory + "\\" + Program.strIniLocal); ;
            TxtDominioNombre.Text = MiFun.IniLeer("PREFERENCIAS", "DOMINIONOMBRE", Environment.CurrentDirectory + "\\" + Program.strIniLocal);

            // MOSTRAMOS LOS USUARIOS Y SUS RESPECTIVAS VENTANAS
            MostrarUsuarios();
        }

        private void MostrarUsuarios()
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            DgUsuarioVentanilla.AutoGenerateColumns = false;
            DgUsuarioVentanilla.AllowUserToAddRows = false;

            Program.vRutaBDLocal = TxtRutaBD.Text;

            dsResult = Program.AbrirArchivoDatos(Program.vRutaBDLocal + @"\usuarios.xml");
            dtResult = dsResult.Tables[0];

            DataGridViewComboBoxColumn Column3 = DgUsuarioVentanilla.Columns["Ventanilla"] as DataGridViewComboBoxColumn;

            Column3.DataSource = DtVentanillas;
            Column3.DisplayMember = "vent_vDescripcion";
            Column3.ValueMember = "vent_sVentanillaId";

            DgUsuarioVentanilla.DataSource = dtResult;

            DgUsuarioVentanilla.Columns[0].Width = 100;
            DgUsuarioVentanilla.Columns[1].Width = 140;
            DgUsuarioVentanilla.Columns[2].Visible = false;
        }

        private void CargarCombos(string StrRutaArchivo)
        {
            DataSet dsLeerFicheroXML = new DataSet();
            DataTable DtResult = new DataTable();
            dsLeerFicheroXML.Clear();
            dsLeerFicheroXML.ReadXml(StrRutaArchivo);

            DtResult = dsLeerFicheroXML.Tables[9]; // TABLA OFICINA CONSULAR
            MiFun.ComboBoxCargarDataTable(CboOficina, DtResult, "ofco_sOficinaConsularId", "ofco_vNombre");

            DtVentanillas = dsLeerFicheroXML.Tables[6]; // TABLA VENTANILLAS

            DataRow newRow = DtVentanillas.NewRow();
            newRow["vent_sVentanillaId"] = "0";
            newRow["vent_vDescripcion"] = "SIN VENTANILLA";
            DtVentanillas.Rows.Add(newRow);

            //MiFun.ComboBoxCargarDataTable(CboVentanilla, DtVentanillas, "vent_sVentanillaId", "vent_vDescripcion");

            DtResult = dsLeerFicheroXML.Tables[5]; // TABLA TICKETERA
            MiFun.ComboBoxCargarDataTable(CboTicketera, DtResult, "tira_sTicketeraId", "tira_vNombre");

            DtResult = dsLeerFicheroXML.Tables[11]; // TABLA USUARIO DE OFICINA CONSULAR
            MiFun.ComboBoxCargarDataTable(CboUsuario, DtResult, "usua_sUsuarioId", "usua_vAlias");

            DtResult = dsLeerFicheroXML.Tables[10]; // TABLA PARAMETROS
            DtResult = MiFun.DataTableFiltrar(DtResult, "para_vGrupo = 'COLAS-TAMAÑO TICKET'", "para_sParametroId");
            MiFun.ComboBoxCargarDataTable(CboTamañoTicket, DtResult, "para_sParametroId", "para_vDescripcion");

            DtResult = dsLeerFicheroXML.Tables[8]; // TABLA VIDEOS
            DtResult = MiFun.DataTableFiltrar(DtResult, "vide_vUrl like '%YOUTUBE%' or vide_vUrl like '%youtube%'", "vide_vDescripcion");

            DataRow newRowVideo = DtResult.NewRow();
            newRowVideo["vide_sVideoId"] = "0";
            newRowVideo["vide_vDescripcion"] = "<<<< SELECCIONAR VIDEO DE YOUTUBE >>>";
            newRowVideo["vide_IOrden"] = "0";
            DtResult.Rows.Add(newRowVideo);

            DtResult = MiFun.DataTableFiltrar(DtResult, "", "vide_vDescripcion");

            MiFun.ComboBoxCargarDataTable(cboYoutube, DtResult, "vide_sVideoId", "vide_vDescripcion");
            cboYoutube.SelectedIndex = 0;



            PrinterSettings impresora = new PrinterSettings();

            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                listBox1.Items.Add(PrinterSettings.InstalledPrinters[i].ToString());
            }
            listBox1.SelectedItem = 1;
            listBox1.Focus();

            CboOficina.SelectedValue = 0;
            //CboVentanilla.SelectedValue = 0;
            listBox1.SelectedItem = 0;
            OptNo.Checked = true;

            MiFun.FuentesCargarCombo(CboFuente);
            MiFun.CombocargarNumero(CboTamaño, 50);
        }

        private void TxtNumLLamadas_KeyPress(object sender, KeyPressEventArgs e)
        {
            string cadena = "123456789" + (char)8;

            if (!cadena.Contains(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void CmdBorrarBD_Click(object sender, EventArgs e)
        {
            TxtRutaBD.Text = "";
        }

        private void CmdBorrarImagen_Click(object sender, EventArgs e)
        {
            TxtFondoEscritorio.Text = "";
        }

        private void CmdBorrarRutaVideo_Click(object sender, EventArgs e)
        {
            TxtRutaVideo.Text = "";
        }

        private void CmdBusRutaVideo_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                TxtRutaVideo.Text = folderBrowserDialog1.SelectedPath;
                TxtRutaVideo.Refresh();
            }
        }

        private void CmdBusRutRep_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                TxtRutaRep.Text = folderBrowserDialog1.SelectedPath;
                TxtRutaRep.Refresh();
            }
        }

        private void CmdBorrarRutaReporte_Click(object sender, EventArgs e)
        {
            TxtRutaRep.Text = "";
        }
      
        private void FrmConfigurar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (intDeDonde == 1)
            {
                //MessageBox.Show("No se ha configurado el sistema, no se puede continuar", Program.strTituloSistema, MessageBoxButtons.OK);
                //this.Close();
                Application.Exit();
            }
        }

        private void DgUsuarioVentanilla_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //DataGridViewComboBoxEditingControl dgvCombo = e.Control as DataGridViewComboBoxEditingControl;

            //// se remueve el handler previo que pudiera tener asociado, a causa ediciones previas de la celda
            //// evitando asi que se ejecuten varias veces el evento
            //dgvCombo.SelectedIndexChanged -= new  EventHandler(dvgCombo_SelectedIndexChanged);
            //dgvCombo.SelectedIndexChanged += new EventHandler(dvgCombo_SelectedIndexChanged);
        }

        private void dvgCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ComboBox combo = sender as ComboBox;
            //int intVentanillaid = Convert.ToInt32(combo.SelectedValue.ToString());

            //MessageBox.Show("ventana ID " + intVentanillaid.ToString(), Program.strTituloSistema, MessageBoxButtons.OK);
        }

        private void DgUsuarioVentanilla_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Se mejoro en la captura del error
            //Fecha: 01-06-2016
            //*************************************

            int intVentanilla = 0;            
            DataGridViewCell dgc;

            if (e.ColumnIndex == 1)
            {
                dgc = DgUsuarioVentanilla.Rows[e.RowIndex].Cells[1];

                try
                {
                    intVentanilla = Convert.ToInt32(dgc.Value);
                }
                catch (Exception ex)
                {
                    intVentanilla = 0;                    
                    MessageBox.Show(ex.Message);
                }
                
                
                if (intVentanilla != 0)
                {
                    string strUsuario = "";
                    string strVentanilla = "";
                    if (BuscarVentanilla(intVentanilla, e.RowIndex, ref strUsuario, ref strVentanilla) == true) 
                    {
                        MessageBox.Show("La " + strVentanilla + " ya fue asignada al usuario: " + strUsuario, Program.strTituloSistema, MessageBoxButtons.OK);
                        DgUsuarioVentanilla.Rows[e.RowIndex].Cells[1].Value = "0";
                    }
                }
            }
        }

        private bool BuscarVentanilla(int intVentanillaId, int intFilaRegistro, ref string strUsuario, ref string strVentanilla)
        {
            bool booOk = false;
            int intFila = 0;
            DataGridViewCell dgc;
            //int intUsuarioId = 0;
            int intVentaId = 0;

            for (intFila = 0; intFila < DgUsuarioVentanilla.Rows.Count; intFila++)
            {
                //dgc = DgUsuarioVentanilla.Rows[intFila].Cells[2];
                //intUsuarioId = Convert.ToInt32(dgc.Value);
                dgc = DgUsuarioVentanilla.Rows[intFila].Cells[1];
                

                intVentaId = Convert.ToInt32(dgc.Value);

                if (intFilaRegistro != intFila)
                {
                    if (intVentanillaId == intVentaId)
                    {
                        booOk = true;

                        strUsuario = DgUsuarioVentanilla.Rows[intFila].Cells[0].Value.ToString();
                        strVentanilla = Convert.ToString(DgUsuarioVentanilla.Rows[intFila].Cells[1].FormattedValue.ToString());
                        break;
                    }
                }
            }

            return booOk;
        }      

    }
}