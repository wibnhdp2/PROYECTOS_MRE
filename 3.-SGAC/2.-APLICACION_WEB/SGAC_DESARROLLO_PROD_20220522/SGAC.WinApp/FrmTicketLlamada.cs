using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using SGAC.Accesorios;

namespace SGAC.WinApp
{
    public partial class FrmTicketLlamada : Form
    {
        private bool bCargando = false;
        private int IntNumTicketNuevo;
        private int IntNumeroTicketLlamado;
        private int IntNumeroVecesLlamado = 0;              // ALMACENA EL NUMERO DE VECES QUE ES LLAMADO UN TIKECT
        private int IntNumeroLlamadasATicket = 0;           // ALMACENA EL NUMERO DE VECES QUE SE LLAMARA A UN TICKET
        private DataTable DtVentanillas = new DataTable();  // ESTE DATATABLE ALMACENARA LA TABLA VENTANILLAS
        private int IntNumVentana = 0;                      // ALMACENA EL NUMERO DE VENTANILLA SELECCIONADA (CAMPO vent_INumeroOrden)
        private string strServicioNombre = "";              // ALMACENA EL NOMBRE DEL SERVICIO QUE SE HA SELECCIONADO
        private string strSubServicioNombre = "";           // ALMACENA EL NOMBRE DEL SubSERVICIO QUE SE HA SELECCIONADO
        private int intNumeroClientesPendientes = 0;        // ALMACENA EL NUMERO DE CLIENTES PENDIENTES DE ATENCION
        private int intNumeroClientesAtendidos = 0;         // ALAMCENA EL NUMERO DE CLIENTES ATENDIDOS
        private int IntNumTickActual;
        private DataTable DtServicios = new DataTable();
        private Funciones MiFun = new Funciones();          // VARIABLE QUE REFERENCIA A LA CLASE FUNCIONES
        private DataTable DtlTicket = new DataTable();      // ALMACENAMOS LOS TIKECT, ESTE DATATABLE SE ATUALIZARA CONSTANTEMENTE
        private int intTipoEmision = 0;                    // INDICA EN QUE MODO SE EMITIRA EL TICKET EN ESTA VENTANA (1 = DERIVACION / 2 = TICKET PREFERENCIAL / 3 = SERVICIO ADICIONAL)
        private int intEstadoActual = 0;
        private DataTable DtlTicketLlamados = new DataTable();
        private DataTable DtlTicketEnAtencion = new DataTable();
        private DataTable DtlTicketAbandonado = new DataTable();
        
        //---------------------------------------
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Creación de variables
        //---------------------------------------
        int intIdTicketGrilla_New = 0;
        int intTicketGrillaActual_New = 0;
        string strTicketGrillaActualNombre_New = "";
        string strLlamadasRealizadas_New = "";
        string strServicioNombre_New = "";
        string strSubServicioNombre_New = "";
        private DataTable DtlTicketAtendidos;
        //---------------------------------------

        public FrmTicketLlamada()
        {
            InitializeComponent();
            KeyDown += new KeyEventHandler(FrmTicketLlamada_KeyDown);
            this.KeyPreview = true;
        }

        private void FrmTicketLlamada_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void FrmTicketLlamada_Load(object sender, EventArgs e)
        {
            try
            {
                this.Width = 465;
                this.Height = 760;
                this.Left = Screen.PrimaryScreen.Bounds.Width - 465;
                this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;

                Blanquea();
                //Alinear();
                AlinearHeaderGrid();
                CargarControles();

                PnlTickets.Visible = true;
                if (Convert.ToInt32(CboVentana.SelectedValue) != 0)
                {
                    MostrarClientes(Convert.ToInt32(Program.strServOficinaConsularCodigo));
                    MostrarClientesNoAtendidos(Program.intServOficinaConsularId);
                    MostrarEstatus(Program.strArchivo);

                    if (DtlTicket.Rows.Count != 0)
                    {
                        IntNumTickActual = Convert.ToInt32(DtlTicket.Rows[DtlTicket.Rows.Count - 1]["tick_iNumero"].ToString());
                    }
                    MiFun.IniEscribir("GENERADO", "TICKET", Convert.ToString(IntNumTickActual), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                }

                if (Program.intVentanillaId == 0)
                {
                    MessageBox.Show("No ha definido ventanilla para este usuario, vaya al menú [Procesos] opción [Parametros del Sistema]", Program.strTituloSistema, MessageBoxButtons.OK);
                    this.Close();
                    return;
                }
                CmdDerTicket.Enabled = false;
                CmdDerTicket.BackColor = Color.DarkGray;

                CmdSiguiente.Enabled = false;
                CmdSiguiente.BackColor = Color.DarkGray;

                timer1.Interval = 1000;
                timer1.Start();
                IntNumeroTicketLlamado = Convert.ToInt32(MiFun.IniLeer("LLAMAR", "NUMERO", Program.vRutaBDLocal + "\\" + Program.strIniServidor));

                // NUMERO MAXIMO DE LLAMADAS A UN TICKET
                IntNumeroLlamadasATicket = Convert.ToInt32(MiFun.IniLeer("PARAMETROS", "TIKECTNUMEROLLAMADAS", Program.vRutaBDLocal + "\\" + Program.strIniServidor));


                System.Windows.Forms.ToolTip tooltip1 = new System.Windows.Forms.ToolTip();

                //if (intNumeroClientesAtendidos == 0) { CmdDerTicket.Enabled = false; };
                MostrarEstatus(Program.strArchivo);

                CmdFinalizaAtencion.Enabled = false;
                CmdFinalizaAtencion.BackColor = Color.DarkGray;
                //----------------------------------------------------
                //Fecha: 09/01/2018
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Determinar si hay Tickets pendientes 
                //----------------------------------------------------
                strServicioNombre = "";
                strSubServicioNombre = "";
                int intIdTicketGrilla = 0;
                int intTicketGrillaActual = 0;
                string intTicketGrillaActualNombre = "";
                intEstadoActual = 0;

                MostrarClientesLlamados(Convert.ToInt32(Program.strServOficinaConsularCodigo));

                if (DtlTicketLlamados.Rows.Count > 0)
                {
                    intEstadoActual = Program.intEstadoLlamado;
                    intIdTicketGrilla = Convert.ToInt32(GrdConsultaLlamados.Rows[0].Cells[4].Value);
                    intTicketGrillaActual = Convert.ToInt32(GrdConsultaLlamados.Rows[0].Cells[2].Value);
                    intTicketGrillaActualNombre = GrdConsultaLlamados.Rows[0].Cells[0].Value.ToString();
                    strLlamadasRealizadas_New = Convert.ToString(GrdConsultaLlamados.Rows[0].Cells[5].Value);

                    CmdLlamada.Enabled = false;
                    CmdLlamada.BackColor = Color.DarkGray;

                    CmdEmiTicket.Enabled = false;
                    CmdEmiTicket.BackColor = Color.DarkGray;

                    CmdDerTicket.Enabled = true;
                    CmdDerTicket.BackColor = Color.Gold;

                    CmdSiguiente.Enabled = true;
                    CmdSiguiente.BackColor = Color.FromArgb(192, 0, 0);
                }

                MostrarClientesEnAtencion(Convert.ToInt32(Program.strServOficinaConsularCodigo));

                if (DtlTicketEnAtencion.Rows.Count > 0)
                {
                    intEstadoActual = Program.intEstadoEnAtencion;
                    intIdTicketGrilla = Convert.ToInt32(GrdConsultaEnAtencion.Rows[0].Cells[4].Value);
                    intTicketGrillaActual = Convert.ToInt32(GrdConsultaEnAtencion.Rows[0].Cells[2].Value);
                    intTicketGrillaActualNombre = GrdConsultaEnAtencion.Rows[0].Cells[0].Value.ToString();
                    strLlamadasRealizadas_New = Convert.ToString(GrdConsultaEnAtencion.Rows[0].Cells[5].Value);

                    tabControl1.Enabled = false;
                                        
                    CmdEmiTicket.Enabled = false;
                    CmdEmiTicket.BackColor = Color.DarkGray;

                    CmdRemite.Enabled = false;
                    CmdRemite.BackColor = Color.DarkGray;

                    CmdSiguiente.Enabled = false;
                    CmdSiguiente.BackColor = Color.DarkGray;

                    CmdLlamada.Enabled = false;
                    CmdLlamada.BackColor = Color.DarkGray;

                    CmdFinalizaAtencion.Enabled = true;
                    CmdFinalizaAtencion.BackColor = Color.FromArgb(120, 89, 176);

                    IntNumeroVecesLlamado = 0;
                    LblTitulo.Text = "Atendiendo a:";
                    LblHorIni.Text = DateTime.Now.ToLongTimeString().Substring(0, 8);
                    LblHorFin.Text = "";
                    LblHorTra.Text = "";            

                }
                intIdTicketGrilla_New = intIdTicketGrilla;
                intTicketGrillaActual_New = intTicketGrillaActual;
                strTicketGrillaActualNombre_New = intTicketGrillaActualNombre;

                if (intEstadoActual == 0)
                { intEstadoActual = Program.intEstadoEmitido; }
                else
                {                    
                    lblNoTicket.Text = intTicketGrillaActual_New.ToString();
                    TxtNombre.Text = strTicketGrillaActualNombre_New;
                    IntNumeroTicketLlamado = intTicketGrillaActual_New;
                    strServicioNombre_New = strServicioNombre;
                    strSubServicioNombre_New = strSubServicioNombre;
                    TxtNomServ.Text = strServicioNombre_New;
                    TxtSubServicio.Text = strSubServicioNombre_New;
                }
                lblusuario.Text = Program.strUsuario;
                
            }            
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        } 

        private void GrdConsulta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void CmdLlamada_Click(object sender, EventArgs e)
        {
           // tabControl1.SelectedIndex = 0;            

            timer1_Tick(sender, e);

            if (IntNumeroVecesLlamado == 0) // Solo para nuevas llamadas de cliente
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    if (GrdConsulta.Rows.Count == 0)
                    {
                        MessageBox.Show("No hay Ticket para efectuar llamada", Program.strTituloSistema, MessageBoxButtons.OK);                       
                        return;
                    }
                }
                else
                {
                    if (GridConsultaNoAtendido.Rows.Count == 0)
                    {
                        MessageBox.Show("No hay Ticket en abandono para efectuar llamada", Program.strTituloSistema, MessageBoxButtons.OK);                        
                        return;
                    }
                }
            }

            if (IntNumeroVecesLlamado >= IntNumeroLlamadasATicket)
            {
                MessageBox.Show("Se ha excedido el número de llamada para este ticket, el número máximo de llamadas por ticket es " + IntNumeroLlamadasATicket.ToString() + " veces", Program.strTituloSistema, MessageBoxButtons.OK);                
                return;
            }

          


            LblTitulo.Text = "LLamando a:";
            LLamarCLiente();

            //------------------------------------------------
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Cambiar el estado del ticket a espera
            //Fecha: 03/01/2018
            //------------------------------------------------
            ActualizarEstado(intIdTicketGrilla_New, Program.intEstadoLlamado, Convert.ToInt32(CboVentana.SelectedValue.ToString()), Program.strArchivo);    
            //------------------------------------------------

            if (tabControl1.SelectedIndex == 0)
            {
                MostrarClientesLlamados(Convert.ToInt32(Program.strServOficinaConsularCodigo));
            }
            else
            {
                MostrarClientesLlamados(Convert.ToInt32(Program.strServOficinaConsularCodigo));
                MostrarClientesNoAtendidos(Convert.ToInt32(Program.strServOficinaConsularCodigo));                
            }
            Application.DoEvents();

            //MostrarClientes(Program.intServOficinaConsularId);
            //MostrarEstatus(Program.strArchivo);
            // ESCRIBIMOS EL ID DEL TICKET EN EL ARCHIVO INI
            //TicketActualizarLLamadas(Program.strArchivo, Convert.ToInt32(GrdConsulta.Rows[0].Cells[4].Value), DateTime.Now.ToString(Program.strFormatoFecha), Convert.ToString(GrdConsulta.Rows[0].Cells[5].Value));
            TicketActualizarLLamadas(Program.strArchivo, intIdTicketGrilla_New, DateTime.Now.ToString(Program.strFormatoFecha), strLlamadasRealizadas_New);
            LblHorIni.Text = "";
            LblHorFin.Text = "";
            LblHorTra.Text = "";
            
            CmdRemite.Enabled = true; // Atender
            CmdRemite.BackColor = Color.FromArgb(54, 153, 235);

            CmdSiguiente.Enabled = true;
            CmdSiguiente.BackColor = Color.FromArgb(192, 0, 0);

            CmdEmiTicket.Enabled = false;
            CmdEmiTicket.BackColor = Color.DarkGray;

            CmdDerTicket.Enabled = true;
            CmdDerTicket.BackColor = Color.Gold;

        }

        private void CmdSiguiente_Click(object sender, EventArgs e)
        {
            string intTicketGrillaActual = "";
            //tabControl1.SelectedIndex = 0;

            //if (GrdConsulta.Rows.Count == 0)
            //{
            //    MessageBox.Show("No hay Ticket para efectuar abandono", Program.strTituloSistema, MessageBoxButtons.OK);
            //    return;
            //}
            if (GrdConsultaLlamados.Rows.Count == 0)
            {
                MessageBox.Show("No hay Ticket llamados para efectuar abandono", Program.strTituloSistema, MessageBoxButtons.OK);                
                return;
            }
            //intTicketGrillaActual = Convert.ToString(GrdConsulta.Rows[0].Cells[4].Value);

            intTicketGrillaActual = Convert.ToString(GrdConsultaLlamados.Rows[0].Cells[4].Value);
            

            //if (GrdConsulta.Rows.Count == 0)
            //{
            //    MessageBox.Show("No hay Ticket para realizar esta acción", Program.strTituloSistema, MessageBoxButtons.OK);
            //    return;
            //}

            IntNumeroVecesLlamado = 0;                 // INICIALIZAMOS EL NUMERO DE LLAMADAS PARA EL NUEVO TICKET

            int intTicketId;
//            intTicketId = Convert.ToInt32(GrdConsulta.Rows[0].Cells[4].Value);
            intTicketId = Convert.ToInt32(GrdConsultaLlamados.Rows[0].Cells[4].Value);

            ActualizarEstado(intTicketId, Program.intEstadoAbandonado, Convert.ToInt32(CboVentana.SelectedValue.ToString()), Program.strArchivo);    //SE ACTUALIZA A 2 EL ESADO POQUE EL CLIENTE FUE LLAMADO PERO NO RESPONDIO ASI QUE SE PASO AL SIGUIENTE CLIENTE
            
            MostrarClientesLlamados(Program.intServOficinaConsularId);
            MostrarClientesNoAtendidos(Program.intServOficinaConsularId);
            MostrarEstatus(Program.strArchivo);
            //LLAMANOS AL SIGUIENTE CLIENTE EN LA LISTA DE ESPERA
            MiFun.IniEscribir("LLAMAR", "IDTICKETFINALIZADO", intTicketGrillaActual, Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            //CmdLlamada_Click(sender, e);
            TxtNombre.Text = "";
            TxtNomServ.Text = "";
            TxtSubServicio.Text = "";
            intTicketGrillaActual = "";
            intIdTicketGrilla_New = 0;
            lblNoTicket.Text = "";

            CmdLlamada.Enabled = true; // Llamar
            CmdLlamada.BackColor = Color.FromArgb(0, 179, 134);

            CmdDerTicket.Enabled = false;
            CmdDerTicket.BackColor = Color.DarkGray;

            CmdEmiTicket.Enabled = true;
            CmdEmiTicket.BackColor = Color.FromArgb(241, 118, 0);

            CmdSiguiente.Enabled = false;
            CmdSiguiente.BackColor = Color.DarkGray;
        }

        private void CmdRemite_Click(object sender, EventArgs e)
        {
            int intTicketId = 0;

            tabControl1.Enabled = false;

            if (GrdConsultaLlamados.Rows.Count == 0)
            {
                MessageBox.Show("No hay Ticket llamados para efectuar atención", Program.strTituloSistema, MessageBoxButtons.OK);
                tabControl1.Enabled = true;
                return;
            }

            bool BolOk = false;

            CmdEmiTicket.Enabled = false;
            CmdEmiTicket.BackColor = Color.DarkGray;

            CmdRemite.Enabled = false; //Atender
            CmdRemite.BackColor = Color.DarkGray;

            CmdSiguiente.Enabled = false; //Abandonar
            CmdSiguiente.BackColor = Color.DarkGray;

            CmdLlamada.Enabled = false; // Llamar
            CmdLlamada.BackColor = Color.DarkGray;

            CmdFinalizaAtencion.Enabled = true;
            CmdFinalizaAtencion.BackColor = Color.FromArgb(120, 89, 176);

           
            //if (tabControl1.SelectedIndex == 0) { intTicketId = Convert.ToInt32(GrdConsulta.Rows[0].Cells[4].Value); }
            intTicketId = Convert.ToInt32(GrdConsultaLlamados.Rows[0].Cells[4].Value); 
            

            IntNumeroVecesLlamado = 0;                 // INICIALIZAMOS EL NUMERO DE LLAMADAS PARA EL NUEVO TICKET
            LblTitulo.Text = "Atendiendo a:";
            //IniciarAtencion();

            BolOk = ActualizarFchaInicio(intTicketId, DateTime.Now.ToString(Program.strFormatoFecha), Program.strArchivo);

            //------------------------------------------------
            //Autor: Miguel Márquez Beltrán            
            //Fecha: 03/01/2018
            //------------------------------------------------
            ActualizarEstado(intTicketId, Program.intEstadoEnAtencion, Convert.ToInt32(CboVentana.SelectedValue.ToString()), Program.strArchivo);
            //------------------------------------------------
            MostrarClientesLlamados(Convert.ToInt32(Program.strServOficinaConsularCodigo));
            MostrarClientesEnAtencion(Convert.ToInt32(Program.strServOficinaConsularCodigo));            

            int intIdTicketGrilla = 0;
            int intTicketGrillaActual = 0;
            string intTicketGrillaActualNombre = "";
            
            intEstadoActual = Program.intEstadoEnAtencion;

            if (DtlTicketEnAtencion.Rows.Count > 0)
            {
                intIdTicketGrilla = Convert.ToInt32(GrdConsultaEnAtencion.Rows[0].Cells[4].Value);
                intTicketGrillaActual = Convert.ToInt32(GrdConsultaEnAtencion.Rows[0].Cells[2].Value);
                intTicketGrillaActualNombre = GrdConsultaEnAtencion.Rows[0].Cells[0].Value.ToString();
                strLlamadasRealizadas_New = Convert.ToString(GrdConsultaEnAtencion.Rows[0].Cells[5].Value);                                
            }
           

            intIdTicketGrilla_New = intIdTicketGrilla;
            intTicketGrillaActual_New = intTicketGrillaActual;
            strTicketGrillaActualNombre_New = intTicketGrillaActualNombre;

            TxtNomServ.Text = strServicioNombre;
            TxtSubServicio.Text = strSubServicioNombre;

           
            TxtNombre.Text = strTicketGrillaActualNombre_New; 
            
            LblHorIni.Text = DateTime.Now.ToLongTimeString().Substring(0, 8);
            LblHorFin.Text = "";
            LblHorTra.Text = "";
            CmdDerTicket.Enabled = false;
            CmdDerTicket.BackColor = Color.DarkGray;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (Program.intVentanillaId == 0) { this.Close(); }

            IntNumTicketNuevo = Convert.ToInt32(MiFun.IniLeer("GENERADO", "TICKET", Program.vRutaBDLocal + "\\" + Program.strIniServidor));
          //  if (IntNumTicketNuevo != IntNumTickActual)
          //  {                                
                //if (tabControl1.SelectedIndex == 1)
                //{
                //    MostrarClientesNoAtendidos(Program.intServOficinaConsularId);
                //}
                //else
                //{
                //    MostrarClientes(Program.intServOficinaConsularId);
                //}

            if (tabControl1.SelectedIndex == 0)
            {
                MostrarClientes(Program.intServOficinaConsularId);                                
            }
            else
            {
                MostrarClientesNoAtendidos(Program.intServOficinaConsularId);
            }
                MostrarEstatus(Program.strArchivo);
                IntNumTickActual = IntNumTicketNuevo;
                Application.DoEvents();
          //  }
        }

        private void CmdAceptar_Click(object sender, EventArgs e)
        {
            TlpPrincipal.Enabled = true;
            panel2.Visible = false;
        }

        private void GridConsultaNoAtendido_DoubleClick(object sender, EventArgs e)
        {
            panel2.Left = ((this.Width - panel2.Width) / 2);
            panel2.Top = ((this.Height - panel2.Height) / 2);
            panel2.Visible = true;

            MostrarLLamadasEfectuadas(Convert.ToString(GridConsultaNoAtendido.Rows[GridConsultaNoAtendido.CurrentCell.RowIndex].Cells[5].Value));
        }

        private void CboVentana_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bCargando == false)
            {
                MiFun.IniEscribir("PREFERENCIAS", "VENTANILLA", Convert.ToString(CboVentana.SelectedValue), Environment.CurrentDirectory + "\\" + Program.strIniLocal);

                if (tabControl1.SelectedIndex == 0)
                {                    
                    MostrarClientes(Convert.ToInt32(Program.strServOficinaConsularCodigo));
                    MostrarEstatus(Program.strArchivo);
                }

                if (tabControl1.SelectedIndex == 1)
                {                    
                    MostrarClientesNoAtendidos(Convert.ToInt32(Program.strServOficinaConsularCodigo));
                }
            }
        }

        private void GrdConsulta_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            {
                if (Convert.ToInt32(GrdConsulta.Rows[e.RowIndex].Cells[6].Value) == 1)
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 224, 192);
                }
                else
                {
                    e.CellStyle.BackColor = Color.FromArgb(192, 255, 192);
                }
            }
        }

        //private void CmdCancelar2_Click(object sender, EventArgs e)
        //{
        //    CmdEmiTicket.Enabled = true;
        //    CmdDerTicket.Enabled = true;
        //    TlpPrincipal.Enabled = true;

        //    panel3.Visible = false;
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            //----------------------------------
            //Nuevo Ticket - preferencial
            //----------------------------------
            DataTable DtServ = new DataTable();
            DataSet dsLeerFicheroXML = new DataSet();

            intTipoEmision = 2;
            LblDescOpcion.Text = "Emisión de Ticket (Preferencial)";
            CmdEmiTicket.Enabled = false;
            CmdEmiTicket.BackColor = Color.DarkGray;
            
            TlpPrincipal.Enabled = false;
            panel4.Left = ((this.Width - panel4.Width) / 2);
            panel4.Top = ((this.Height - (panel4.Height + 100)) / 2);

            //dsLeerFicheroXML.Clear();

            dsLeerFicheroXML = LeerFicheroXML(Program.strArchivo).Copy();

            if (dsLeerFicheroXML.Tables.Count == 0)
            {
                do
                {
                    dsLeerFicheroXML = LeerFicheroXML(Program.strArchivo).Copy();
                } while (dsLeerFicheroXML.Tables.Count == 0);
                //MessageBox.Show("El tiempo de espera para registrar este proceso ha culminado, haga clic en aceptar para continuar");
            }
            //dsLeerFicheroXML.ReadXml(Program.strArchivo);



            string[,] Datos = new string[2, 5] {
                                                   { "Table2", "serv_sServicioId", "N", "","" },
                                                   { "Table7", "vede_sServicioId", "N", "serv_sServicioId","" },
                                               };

            DtServ = MiFun.DataSetCombinarTablas(dsLeerFicheroXML, Datos);

            bCargando = true;
            int intVentanillaId = Convert.ToInt32(CboVentana.SelectedValue);
            //DtServ = MiFun.DataTableFiltrar(DtServ, "serv_sTipo = 1 and vede_sVentanillaId <> " + intVentanillaId + " ", "serv_IOrden ASC");
            DtServ = MiFun.DataTableFiltrar(DtServ, "serv_sTipo = 1", "serv_IOrden ASC");
            MiFun.ComboBoxCargarDataTable(CboServicioDerivar, DtServ, "serv_sServicioId", "serv_vDescripcion");

            CboServicioDerivar.SelectedValue = 0;
            CboSubServicio.SelectedValue = 0;
            //CboVentanilla.SelectedValue = 0;

            //CboVentanilla.Visible = false;
            //LblTickDeriva.Visible = false;

           // CargarTickets();
            panel4.Visible = true;
            bCargando = false;
            //CmdDerTicket.Enabled = false;
        }

        //private void CmdAceptar2_Click(object sender, EventArgs e)
        //{
        //    int intTiketId = 0;
        //    int intServicioId = 0;

        //    if (CboServicio.Text == "")
        //    {
        //        intServicioId = Convert.ToInt32(CboServicio.SelectedValue);
        //    }
        //    else
        //    {
        //        intServicioId = Convert.ToInt32(CboSubServicio.SelectedValue);
        //    }

        //    if (GenerarTicketRef(ref intTiketId, Program.strArchivo, Convert.ToInt32(Program.strServOficinaConsularCodigo), intServicioId) == true)
        //    {
        //        if (intTiketId != 0)
        //        {
        //            MiFun.IniEscribir("GENERADO", "TICKET", Convert.ToString(intTiketId), Program.vRutaBDLocal + "\\" + Program.strIniServidor);

        //            Funciones xMiFun = new Funciones();
        //            string VRutaArchivo;

        //            //Se establecio el formato de la fecha y hora

        //            string VFecha = DateTime.Now.ToString("MMM-dd-yyyy HH:mm:ss").ToUpper();
        //            string strNumero = Convert.ToString(intTiketId);
        //            VRutaArchivo = Program.vRutaReportes + "\\crTicket.rpt";
        //            VFecha = VFecha.Substring(0, 19);
        //            string[,] Parametros = new string[5, 2] {
        //                                        {"Numero",strNumero},
        //                                        {"Ventanilla","V2"},
        //                                        {"NomConsulado",Program.strServOficinaConsularNombre},
        //                                        {"FchEmision",VFecha},
        //                                        {"NombreServicio",CboSubServicio.SelectedText},
        //                                    };

        //            // SI LA CONSTANTE IImprimirTicket = 1 QUIERE DECIR QUE SE IMPRIMIRAN LOS TICKETS
        //            DataSet DsDatos = new DataSet();

        //            if (Program.intImprimirTicket == 1)
        //            {
        //                xMiFun.CrystalVisor(Parametros, "SISTEMA DE COLAS - EMISION DE TICKET", VRutaArchivo, DsDatos);
        //            }
        //            else
        //            {
        //                //Habilita la impresión directa

        //                MessageBox.Show("TICKET Nº " + Convert.ToString(intTiketId), "Módulo Colas de Atención", MessageBoxButtons.OK);

        //                MasterImpresion masterPrinter = new MasterImpresion();

        //                masterPrinter._pLocalReport.ReportPath = VRutaArchivo;
        //                masterPrinter._parrparametros = Parametros;
        //                masterPrinter._pOrigenesDatosReporte = null;
        //                masterPrinter.Imprimir();

        //                if (masterPrinter._pintErrorNumero != 0)
        //                {
        //                    MessageBox.Show("No se pudo accesar a la impresora " + Program.strImpresoraTicket + ", Su número de Ticket es " + strNumero + " espere su llamada", "Módulo Colas de Atención", MessageBoxButtons.OK);
        //                }
        //                //xMiFun.CrystalImprimir(Parametros, "SISTEMA DE COLAS - EMISION DE TICKET", VRutaArchivo, DsDatos, Program.strImpresoraTicket);
                        
        //                //if (xMiFun.intErrorNumero != 0)
        //                //{
        //                //    MessageBox.Show("No se pudo accesar a la impresora " + Program.strImpresoraTicket + ", Su número de Ticket es " + strNumero + " espere su llamada", "Módulo Colas de Atención", MessageBoxButtons.OK);
        //                //}
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("No se pudo generar el ticket, Consulte con el administrador del sistema", "Módulo Colas de Atención", MessageBoxButtons.OK);
        //    }

        //    CmdEmiTicket.Enabled = true;
        //    CmdDerTicket.Enabled = true;
        //    TlpPrincipal.Enabled = true;
            
        //    MostrarClientes(Program.intServOficinaConsularId);
        //    MostrarEstatus(Program.strArchivo);
        //    IntNumTickActual = IntNumTicketNuevo;
        //    panel3.Visible = false;
        //}

        private void CmdFinalizaAtencion_Click(object sender, EventArgs e)
        {
            int intTicketId = 0;
            tabControl1.Enabled = true;

            //if (tabControl1.SelectedIndex == 0)
            //{
                //intTicketId = Convert.ToInt32(GrdConsulta.Rows[0].Cells[4].Value);
                //if (GrdConsulta.Rows.Count == 0)
                //{
                //    MessageBox.Show("No hay Ticket para efectuar finalización de atención", Program.strTituloSistema, MessageBoxButtons.OK);
                //    return;
                //}
                intTicketId = Convert.ToInt32(GrdConsultaEnAtencion.Rows[0].Cells[4].Value);
                if (GrdConsultaEnAtencion.Rows.Count == 0)
                {
                    MessageBox.Show("No hay Ticket en atención para efectuar la finalización", Program.strTituloSistema, MessageBoxButtons.OK);
                    return;
                }

            //}

            //if (tabControl1.SelectedIndex == 1)
            //{
            //    intTicketId = Convert.ToInt32(GridConsultaNoAtendido.Rows[0].Cells[4].Value);
            //    if (GridConsultaNoAtendido.Rows.Count == 0)
            //    {
            //        MessageBox.Show("No hay Ticket abandonados para efectuar finalización de atención", Program.strTituloSistema, MessageBoxButtons.OK);
            //        return;
            //    }
            //}

            bool BolOk = false;
            string intTicketGrillaActual = "";

//            if (GrdConsulta.RowCount != 0) { intTicketGrillaActual = Convert.ToString(GrdConsulta.Rows[0].Cells[4].Value); }

            if (GrdConsultaEnAtencion.RowCount != 0) { intTicketGrillaActual = Convert.ToString(GrdConsultaEnAtencion.Rows[0].Cells[4].Value); }
                       

            CmdEmiTicket.Enabled = true;
            CmdEmiTicket.BackColor = Color.FromArgb(241, 118, 0);

            CmdRemite.Enabled = true;
            CmdRemite.BackColor = Color.FromArgb(54, 153, 235);

            //CmdSiguiente.Enabled = true;
            //CmdSiguiente.BackColor = Color.FromArgb(192, 0, 0);

            CmdLlamada.Enabled = true;
            CmdLlamada.BackColor = Color.FromArgb(0, 179, 134);

            CmdFinalizaAtencion.Enabled = false;
            CmdFinalizaAtencion.BackColor = Color.DarkGray;

            IntNumeroVecesLlamado = 0;   // INICIALIZAMOS EL NUMERO DE LLAMADAS PARA EL NUEVO TICKET

            ActualizarEstado(intTicketId, Program.intEstadoAtendido, Convert.ToInt32(CboVentana.SelectedValue.ToString()), Program.strArchivo);
            BolOk = ActualizarFechaFinal(intTicketId, DateTime.Now.ToString(Program.strFormatoFecha), Program.strArchivo);

            MostrarClientesEnAtencion(Convert.ToInt32(Program.intServOficinaConsularId));

            //MostrarClientes(Program.intServOficinaConsularId);
            //if (tabControl1.SelectedIndex == 1)
            //{               
            //    MostrarClientesNoAtendidos(Program.intServOficinaConsularId);
            //}
            //MostrarEstatus(Program.strArchivo);

            double decHora = 0;
            LblHorFin.Text = DateTime.Now.ToLongTimeString().Substring(0, 8);
            decHora = MiFun.ConvertirHorasDecimal(LblHorFin.Text) - MiFun.ConvertirHorasDecimal(LblHorIni.Text);
            TxtNombre.Text = "";
            TxtNomServ.Text = "";
            TxtSubServicio.Text = "";
            LblHorTra.Text = MiFun.ConvertirDecimalHoras(decHora);

            MiFun.IniEscribir("LLAMAR", "IDTICKETFINALIZADO", intTicketGrillaActual, Program.vRutaBDLocal + "\\" + Program.strIniServidor);            
            intTicketGrillaActual = "";
            intIdTicketGrilla_New = 0;
            lblNoTicket.Text = "";
            CmdDerTicket.Enabled = false;
            CmdDerTicket.BackColor = Color.DarkGray;
        }

        //*********************************************************************************
        //**                             F U N C I O N E S                               **
        //*********************************************************************************

        private void CargarControles()
        {
            DataSet dsLeerFicheroXML = new DataSet();

            bCargando = true;
            //dsLeerFicheroXML.Clear();

            dsLeerFicheroXML = LeerFicheroXML(Program.strArchivo).Copy();

            //dsLeerFicheroXML.ReadXml(Program.strArchivo);
            if (dsLeerFicheroXML.Tables.Count == 0)
            {
                do
                {
                    dsLeerFicheroXML = LeerFicheroXML(Program.strArchivo).Copy();
                } while (dsLeerFicheroXML.Tables.Count == 0);
                //MessageBox.Show("El tiempo de espera para registrar este proceso ha culminado, haga clic en aceptar para continuar");
            }

            DtVentanillas = dsLeerFicheroXML.Tables[6];     // CARGAMOS LA TABLA DE VENTANILLAS
            DtServicios = dsLeerFicheroXML.Tables[2];       // CARGAMOS LA TABLA SERVICIOS

            // MOSTRAMOS LOS SERVICIOS
            DataTable DtServ = new DataTable();
            DtServ = MiFun.DataTableFiltrar(DtServicios, "serv_sTipo = 1", "serv_IOrden ASC");

            bCargando = true;
            MiFun.ComboBoxCargarDataTable(CboVentana, DtVentanillas, "vent_sVentanillaId", "vent_vDescripcion");
            DtVentanillas = dsLeerFicheroXML.Tables[2];     // CARGAMOS LA TABLA DE SERVICIOS
           // MiFun.ComboBoxCargarDataTable(CboServicio, DtVentanillas, "serv_sServicioId", "serv_vDescripcion");

            CboVentana.SelectedValue = Program.intVentanillaId;//IntNumeroVentana;

            string strNumVentana = "";

            strNumVentana = CboVentana.Text.Replace("VENTANILLA", "").Replace("-","").Trim();
            if (strNumVentana.Length == 0)
            { strNumVentana = "0"; }
            IntNumVentana = Convert.ToInt32(strNumVentana);

            CboVentana.Refresh();
            bCargando = false;
        }

        private bool TicketActualizarLLamadas(string StrArchivo, int IntTicketId, string StrLLamada, string StrLLamadaRealizadas)
        {
            if (StrLLamadaRealizadas.Length != 0)
            {
                if (StrLLamadaRealizadas.Split('|').Length >= 3)
                {
                    StrLLamadaRealizadas = StrLLamada;
                }
                else
                {
                    StrLLamadaRealizadas = StrLLamadaRealizadas + "|" + StrLLamada;
                }                
            }
            else
            {
                StrLLamadaRealizadas = StrLLamada;
            }

            string[,] Valores = new string[2, 3] {
                                                    { "tick_iTicketId", Convert.ToString(IntTicketId), "C" },
                                                    { "tick_vLLamada", StrLLamadaRealizadas, "" },
                                                 };
            if (MiFun.XMLModificarNodo(StrArchivo, "Table1", Valores) == false)
            {
                do
                {
                } while (MiFun.XMLModificarNodo(StrArchivo, "Table1", Valores) == false);
            }            
            
            return true;
        }


        private void Blanquea()
        {
            TxtNomServ.Text = "";
            TxtNumPerEsp.Text = "";
            TxtNumPerAte.Text = "";
            TxtTieProEsp.Text = "";
            TxtTieProAte.Text = "";
            TxtSubServicio.Text = "";
            TxtNombre.Text = "";

            LblHorFin.Text = "";
            LblHorIni.Text = "";
            LblHorTra.Text = "";
            lblNoTicket.Text = "";
        }

        private bool ActualizarEstado(int intTicketId, int intEstadoId, int intVentanillaId, string strArchivo)
        {
            string[,] Valores = new string[4, 3] {
                                                    { "tick_iTicketId", intTicketId.ToString(), "C" },
                                                    { "tick_sTipoEstado", intEstadoId.ToString(), "" },
                                                    { "tick_sVentanillaId", intVentanillaId.ToString(), "" },
                                                    { "tick_sUsuarioAtendio", Convert.ToString(Program.intUsuarioId), "" },
                                                  };
            if (MiFun.XMLModificarNodo(strArchivo, "Table1", Valores) == false)
            {
                do
                {
                } while (MiFun.XMLModificarNodo(strArchivo, "Table1", Valores) == false);
            }
            
            return true;
        }

        private bool ActualizarServicio(int intTicketId, int intEstadoId, int intServicioId, int intVentanillaId, string strArchivo)
        {
            string[,] Valores = new string[5, 3] {
                                                    { "tick_iTicketId", intTicketId.ToString(), "C" },
                                                    { "tick_sTipoEstado", intEstadoId.ToString(), "" },
                                                    { "tick_sTipoServicioId", intServicioId.ToString(), "" },
                                                    { "tick_sVentanillaId", intVentanillaId.ToString(), "" },
                                                    { "tick_sUsuarioAtendio", Convert.ToString(Program.intUsuarioId), "" },
                                                  };
            if (MiFun.XMLModificarNodo(strArchivo, "Table1", Valores) == false)
            {
                do
                {
                } while (MiFun.XMLModificarNodo(strArchivo, "Table1", Valores) == false);
            }
            
            return true;
        }

        private bool ActualizarFchaInicio(int intTicketId, string strFechaInicio, string strArchivo)
        {
            string[,] Valores = new string[2, 3] {
                                                    { "tick_iTicketId", intTicketId.ToString(), "C" },
                                                    { "tick_dAtencionInicio", strFechaInicio, "" },
                                                  };
            if (MiFun.XMLModificarNodo(strArchivo, "Table1", Valores) == false)
            {
                do
                {
                } while (MiFun.XMLModificarNodo(strArchivo, "Table1", Valores) == false);
            }
            
            return true;
        }

        private bool ActualizarFechaFinal(int intTicketId, string strFechaFinal, string strArchivo)
        {
            string[,] Valores = new string[2, 3] {
                                                    { "tick_iTicketId", intTicketId.ToString(), "C" },
                                                    { "tick_dAtencionFinal", strFechaFinal, "" },
                                                  };
            if (MiFun.XMLModificarNodo(strArchivo, "Table1", Valores) == false)
            {
                do
                {
                } while (MiFun.XMLModificarNodo(strArchivo, "Table1", Valores) == false);
            }
            
            return true;
        }


        private void LLamarCLiente()
        {
            int intTicketGrillaActual = 0;
            int intIdTicketGrilla = 0;
            string intTicketGrillaActualNombre = "";
            

            //OBTENEMOS EL NUMERO DE TIKECT DE LA GRILLA SELECCIONADA
            if (IntNumeroVecesLlamado == 0) // Solo para nuevas llamadas de cliente
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    intIdTicketGrilla = Convert.ToInt32(GrdConsulta.Rows[0].Cells[4].Value);
                    intTicketGrillaActual = Convert.ToInt32(GrdConsulta.Rows[0].Cells[2].Value);
                    intTicketGrillaActualNombre = GrdConsulta.Rows[0].Cells[0].Value.ToString();
                    strLlamadasRealizadas_New = Convert.ToString(GrdConsulta.Rows[0].Cells[5].Value);
                }
                else
                {
                    intIdTicketGrilla = Convert.ToInt32(GridConsultaNoAtendido.Rows[0].Cells[4].Value);
                    intTicketGrillaActual = Convert.ToInt32(GridConsultaNoAtendido.Rows[0].Cells[2].Value);
                    intTicketGrillaActualNombre = GridConsultaNoAtendido.Rows[0].Cells[0].Value.ToString();
                    strLlamadasRealizadas_New = Convert.ToString(GridConsultaNoAtendido.Rows[0].Cells[5].Value);
                }



                intIdTicketGrilla_New = intIdTicketGrilla;
                intTicketGrillaActual_New = intTicketGrillaActual;
                strTicketGrillaActualNombre_New = intTicketGrillaActualNombre;
                strServicioNombre_New = strServicioNombre;
                strSubServicioNombre_New = strSubServicioNombre;
            }
          

            IntNumeroVecesLlamado = IntNumeroVecesLlamado + 1;
            if (IntNumeroTicketLlamado != intTicketGrillaActual_New)
            {
                MiFun.IniEscribir("LLAMAR", "NUMERO", Convert.ToString(intTicketGrillaActual_New), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                MiFun.IniEscribir("LLAMAR", "NOMBRE", Convert.ToString(strTicketGrillaActualNombre_New), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                MiFun.IniEscribir("LLAMAR", "VENTANA", IntNumVentana.ToString(), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                MiFun.IniEscribir("LLAMAR", "IDTICKET", Convert.ToString(intIdTicketGrilla_New), Program.vRutaBDLocal + "\\" + Program.strIniServidor);

            }
            else
            {
                MiFun.IniEscribir("LLAMAR", "NUMERO", Convert.ToString(intTicketGrillaActual_New), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                MiFun.IniEscribir("LLAMAR", "NOMBRE", Convert.ToString(strTicketGrillaActualNombre_New), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                MiFun.IniEscribir("LLAMAR", "VENTANA", IntNumVentana.ToString(), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                MiFun.IniEscribir("LLAMAR", "IDTICKET", Convert.ToString(intIdTicketGrilla_New), Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            }
            lblNoTicket.Text = intTicketGrillaActual_New.ToString();
            TxtNombre.Text = strTicketGrillaActualNombre_New;
            IntNumeroTicketLlamado = intTicketGrillaActual_New;
            TxtNomServ.Text = strServicioNombre_New;
            TxtSubServicio.Text = strSubServicioNombre_New;

        }

        private DataSet LeerFicheroXML(string StrArchivo)
        {
            DataSet dsLeerFicheroXML = new DataSet();

            try
            {
                dsLeerFicheroXML.Clear();
                dsLeerFicheroXML.ReadXml(StrArchivo);
                return dsLeerFicheroXML;
            }
            catch
            {
                return dsLeerFicheroXML;
            }

        }

        private DataTable TicketConsultaVentanilla2(string StrArchivo, int IntTipoEstado)
        {
            DataSet dsLeerFicheroXML = new DataSet();
            DataTable DtResult = new DataTable();
            DataView dtview = new DataView(DtResult);
            int A;
            string StrCampoCadFiltro = "vede_sVentanillaId = " + Convert.ToString(CboVentana.SelectedValue) + "";


            dsLeerFicheroXML = LeerFicheroXML(StrArchivo).Copy();

            if (dsLeerFicheroXML.Tables.Count == 0)
            {
                do
                {
                    dsLeerFicheroXML = LeerFicheroXML(StrArchivo).Copy();
                } while (dsLeerFicheroXML.Tables.Count == 0);
            }
                                                                

            string[,] Datos = new string[4, 5] { { "Table1", "tick_iTicketId", "N", "","" },
                                                    { "Table2", "serv_sServicioId", "N", "tick_sTipoServicioId","" },
                                                    { "Table", "pers_iPersonaId", "N", "tick_IPersonalId","" },
                                                    { "Table7", "vede_sServicioId", "N", "tick_sTipoServicioId", StrCampoCadFiltro } };

            DtResult = MiFun.DataSetCombinarTablas(dsLeerFicheroXML, Datos);

            DtResult.Columns.Add("tick_VTiempoEspera", typeof(string));

            // CALCULAMOS EL TIEMPO DE ESPERA
            try
            {
                for (A = 0; A <= DtResult.Rows.Count - 1; A++)
                {
                    string HoraInicio;
                    string HoraFinal;
                    DateTime datHoraInicio;

                    if (DtResult.Rows[A]["tick_dFechaHoraGeneracion"].ToString() != "")
                    {

                        string strFecha = DtResult.Rows[A]["tick_dFechaHoraGeneracion"].ToString();

                        datHoraInicio = new DateTime
                        (
                        Convert.ToInt16(strFecha.Substring(0, 4)),
                        Convert.ToInt16(strFecha.Substring(4, 2)),
                        Convert.ToInt16(strFecha.Substring(6, 2)),
                        Convert.ToInt16(strFecha.Substring(9, 2)),
                        Convert.ToInt16(strFecha.Substring(11, 2)),
                        0
                        );

                        HoraInicio = datHoraInicio.ToString("HH:mm:ss");  //  HoraInicio.Substring(11, 12);
                        HoraFinal = DateTime.Now.ToString("HH:mm:ss");  // DateTime.Now.ToLongTimeString();

                        DtResult.Rows[A]["tick_VTiempoEspera"] = MiFun.RestarHoras(HoraInicio, HoraFinal);

                        if (IntTipoEstado == Program.intEstadoAbandonado)
                        {
                            string strLlamada = DtResult.Rows[A]["tick_vLLamada"].ToString().PadRight(9);
                            DtResult.Rows[A]["tick_vIPCreacion"] = strLlamada;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

            for (A = 0; A <= DtResult.Rows.Count - 1; A++)
            {
                try
                {
                    DataRow dr = DtResult.Rows[A];
                    if (dr["tick_dFechaHoraGeneracion"].ToString().Length != 0)
                    {
                        string strFecha = dr["tick_dFechaHoraGeneracion"].ToString();
                        DateTime datHoraInicio = new DateTime
                        (
                        Convert.ToInt16(strFecha.Substring(0, 4)),
                        Convert.ToInt16(strFecha.Substring(4, 2)),
                        Convert.ToInt16(strFecha.Substring(6, 2)),
                        Convert.ToInt16(strFecha.Substring(9, 2)),
                        Convert.ToInt16(strFecha.Substring(11, 2)),
                        0
                        );

                        if (datHoraInicio.ToString().Substring(0, 10) != DateTime.Now.Date.ToString().Substring(0, 10))
                        {
                            dr.Delete();
                            //A = A - 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }

            }

            DtResult = MiFun.DataTableFiltrar(DtResult, "((tick_sTipoEstado = " + IntTipoEstado.ToString() + ") AND vede_sVentanillaId = " + Convert.ToString(CboVentana.SelectedValue) + ")", "tick_iTicketId ASC, vede_IObligatorio ASC ");
            return DtResult;
        }

        private DataTable CombinarTicketServicio(string StrArchivo)
        {
            DataTable dtlPromedio = new DataTable();
            DataSet dsLeerFicheroXML = new DataSet();
            
            
            dsLeerFicheroXML = LeerFicheroXML(StrArchivo).Copy();

            if (dsLeerFicheroXML.Tables.Count == 0)
            {
                do
                {
                     dsLeerFicheroXML = LeerFicheroXML(StrArchivo).Copy();
                } while (dsLeerFicheroXML.Tables.Count == 0);
            }


            // UNIMOS LA LAS TABLAS tick_iTicketId Y vede_sServicioId
            string StrCampoCadFiltro = "vede_sVentanillaId = " + Convert.ToString(CboVentana.SelectedValue) + "";
            string[,] Datos = new string[2, 5] { { "Table1", "tick_iTicketId", "N", "","" },
                                                 { "Table7", "vede_sServicioId", "N", "tick_sTipoServicioId", StrCampoCadFiltro } };

            dtlPromedio = MiFun.DataSetCombinarTablas(dsLeerFicheroXML, Datos);

            return dtlPromedio;
        }

        private void MostrarEstatus(string StrArchivo)
        {
            DataTable dtlPromedio = new DataTable();
            string VPromedioEspera = "";
            string VPromedioAtencion = "";

            dtlPromedio = CombinarTicketServicio(StrArchivo);

            intNumeroClientesPendientes = ticketHallarTotales(DtlTicket, Program.intEstadoEmitido);  // NUMERO DE TICKETS PENDIENTES DE ATENCION

            if (tabControl1.SelectedIndex == 0)
            {
                label17.Text = "Total de Tickets : " + Convert.ToString(intNumeroClientesPendientes);
            }
            int intNumeroTicketsAbandonados = ticketHallarTotales(DtlTicketAbandonado, Program.intEstadoAbandonado);            

            if (tabControl1.SelectedIndex == 1)
            {
                
                label17.Text = "Total de Tickets : " + Convert.ToString(intNumeroTicketsAbandonados);
            }
            
            int intNumeroTicketAtendidos = ticketHallarTotales(DtlTicketAtendidos, Program.intEstadoAtendido);
            if (tabControl1.SelectedIndex == 2)
            {
                
                label17.Text = "Total de Tickets : " + Convert.ToString(intNumeroTicketAtendidos);
            }

            // FILTRAMOS LA TABLA OBTENIDAD POR EL CODIGO DE VENTANILLA SELECCIONADO
            string Filto = "vede_sVentanillaId = " + Convert.ToInt32(CboVentana.SelectedValue);
            dtlPromedio = MiFun.DataTableFiltrar(dtlPromedio, Filto, "tick_iTicketId");



            VPromedioEspera = TicketPromedioEspera(DtlTicket);
            VPromedioAtencion = TicketPromedioAtencion(dtlPromedio);


            TxtNumPerEsp.Text = Convert.ToString(intNumeroClientesPendientes);
            TxtTicketsAbandonados.Text = Convert.ToString(intNumeroTicketsAbandonados);
            TxtNumPerAte.Text = Convert.ToString(intNumeroTicketAtendidos);
            TxtTieProEsp.Text = VPromedioEspera;
            TxtTieProAte.Text = VPromedioAtencion;
            Application.DoEvents();
        }

        private void AlinearHeaderGrid()
        {
            GrdConsulta.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            GrdConsulta.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GrdConsulta.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            GrdConsulta.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            GrdConsulta.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GrdConsulta.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //--------------------------------------------------------------------------------------------------
            GridConsultaNoAtendido.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            GridConsultaNoAtendido.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridConsultaNoAtendido.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            GridConsultaNoAtendido.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridConsultaNoAtendido.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridConsultaNoAtendido.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //--------------------------------------------------------------------------------------------------
            GridAtendidos.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            GridAtendidos.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridAtendidos.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridAtendidos.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            GridAtendidos.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridAtendidos.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridAtendidos.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //--------------------------------------------------------------------------------------------------
        }

        private string HallarServicioDelSubServicio(Int16 SubServicioId)
        {
            int A = 0;
            int intServicioId = 0;
            string strNombreServicio = "";

            //BUSCAMOS EL SERVICIO DEL SUBSERVICIO
            for (A = 0; A <= DtServicios.Rows.Count - 1; A++)
            {
                if (Convert.ToInt16(DtServicios.Rows[A]["serv_sServicioId"].ToString()) == SubServicioId)
                {
                    if (DtServicios.Rows[A]["serv_sServicioIdCab"].ToString() != "")
                    {
                        intServicioId = Convert.ToInt16(DtServicios.Rows[A]["serv_sServicioIdCab"].ToString());
                        break;
                    }
                }
            }

            if (intServicioId != 0)
            {
                //BUSCAMOS EL SERVICIO ENCONTRADO
                for (A = 0; A <= DtServicios.Rows.Count; A++)
                {
                    if (Convert.ToInt16(DtServicios.Rows[A]["serv_sServicioId"].ToString()) == intServicioId)
                    {
                        strNombreServicio = DtServicios.Rows[A]["serv_vDescripcion"].ToString();
                        break;
                    }
                }
            }

            return strNombreServicio;
        }

        DataTable CrearTicketTemporal()
        {

            DataTable Dt = new DataTable();
            //Util MiFun = new Util();
            Funciones xFun = new Funciones();


            string[,] Columnas = new string[12, 2] { {"tick_vNombreApellido", "System.String"}, 
                                                    {"serv_vDescripcion", "System.String"},
                                                    {"tick_iNumero", "System.Int32"},
                                                    {"tick_vTiempoEspera", "System.String"},
                                                    {"tick_iTicketId", "System.Int32"},
                                                    {"tick_vLLamada", "System.String"},
                                                    {"vede_IObligatorio", "System.String"},
                                                    {"tick_sPrioridadId", "System.String"},
                                                    {"serv_sTipo", "System.String"},
                                                    {"tick_sTipoServicioId", "System.String"},
                                                    {"tick_sTipoEstado", "System.String"},
                                                    {"tick_dFechaHoraGeneracion", "System.String"}
                                                    
                                                  };

            Dt = xFun.DataTableCrear(Columnas, "MiTabla");
            return Dt;
        }

        private void PasarDatos(ref DataTable dtDestino, DataTable dtOrigen)
        {
            int A = 0;

            for (A = 0; A <= dtOrigen.Rows.Count - 1; A++)
            {
                DataRow newRow = dtDestino.NewRow();
                newRow["tick_vNombreApellido"] = dtOrigen.Rows[A]["tick_vNombreApellido"].ToString();
                newRow["serv_vDescripcion"] = dtOrigen.Rows[A]["serv_vDescripcion"].ToString();
                newRow["tick_iNumero"] = Convert.ToInt32(dtOrigen.Rows[A]["tick_iNumero"].ToString());
                newRow["tick_vTiempoEspera"] = dtOrigen.Rows[A]["tick_vTiempoEspera"].ToString();
                newRow["tick_iTicketId"] = Convert.ToInt32(dtOrigen.Rows[A]["tick_iTicketId"].ToString());
                newRow["tick_vLLamada"] = dtOrigen.Rows[A]["tick_vLLamada"].ToString();
                newRow["vede_IObligatorio"] = dtOrigen.Rows[A]["vede_IObligatorio"].ToString();
                newRow["tick_sPrioridadId"] = dtOrigen.Rows[A]["tick_sPrioridadId"].ToString();
                newRow["serv_sTipo"] = dtOrigen.Rows[A]["serv_sTipo"].ToString();
                newRow["tick_sTipoServicioId"] = dtOrigen.Rows[A]["tick_sTipoServicioId"].ToString();
                newRow["tick_sTipoEstado"] = dtOrigen.Rows[A]["tick_sTipoEstado"].ToString();
                newRow["tick_dFechaHoraGeneracion"] = dtOrigen.Rows[A]["tick_dFechaHoraGeneracion"].ToString();
                dtDestino.Rows.Add(newRow);
            }
        }

        private void MostrarClientes(int IntConsuladoId)
        {
            DtlTicket = TicketConsultaVentanilla2(Program.strArchivo, Program.intEstadoEmitido);
            if (DtlTicket.Rows.Count != 0)
            {
                // CREAMOS UN DT TEMPORAL PARA PODER ORDENAR POR NUMERO DE TICKET
                DataTable DtTemporal = CrearTicketTemporal();
                // PASAMOS LOS DATOS AL DT TEMPORAL
                PasarDatos(ref DtTemporal, DtlTicket);

                DtlTicket = DtTemporal;

                DataView dtview = new DataView(DtlTicket);

                dtview.Sort = "vede_IObligatorio ASC, tick_sPrioridadId DESC, tick_iNumero ASC";
                DtlTicket = dtview.ToTable();


                GrdConsulta.AutoGenerateColumns = false;
                GrdConsulta.DataSource = DtlTicket;

                if (Convert.ToInt32(DtlTicket.Rows[0]["serv_sTipo"].ToString()) == 1)
                {
                    strServicioNombre = DtlTicket.Rows[0]["serv_vDescripcion"].ToString();
                    strSubServicioNombre = "";
                }
                else
                {
                    // buscamos el id del servicio del ticket para hallar al padre del subseervicio
                    strServicioNombre = HallarServicioDelSubServicio(Convert.ToInt16(DtlTicket.Rows[0]["tick_sTipoServicioId"].ToString()));
                    strSubServicioNombre = DtlTicket.Rows[0]["serv_vDescripcion"].ToString();
                }
               
            }
            else
            {
                DataView dtview = new DataView(DtlTicket);
                dtview.Sort = "vede_IObligatorio ASC, tick_sPrioridadId DESC, tick_iNumero ASC";
                DtlTicket = dtview.ToTable();

                GrdConsulta.AutoGenerateColumns = false;
                GrdConsulta.DataSource = DtlTicket;                
            }
            GrdConsulta.Refresh();
        }

        private string ServicioPadre(Int16 intServicioId)
        {
            int intFila = 0;
            Int16 intServicioIdTabla = 0;
            string strServicioNombre1 = "";

            for (intFila = 0; intFila <= DtVentanillas.Rows.Count - 1; intFila++)
            {
                intServicioIdTabla = Convert.ToInt16(DtVentanillas.Rows[intFila]["serv_sServicioId"].ToString());
                if (intServicioIdTabla == intServicioId)
                {
                    strServicioNombre1 = DtVentanillas.Rows[intFila]["serv_vDescripcion"].ToString();
                    break;
                }
            }
            return strServicioNombre1;
        }

        private void MostrarClientesNoAtendidos(int iConsuladoId)
        {

            DtlTicketAbandonado = TicketConsultaVentanilla2(Program.strArchivo, Program.intEstadoAbandonado);

            if (DtlTicketAbandonado.Rows.Count != 0)
            {
                DataView dtview = new DataView(DtlTicketAbandonado);
                // dtview.RowFilter = "tick_sUsuarioAtendio = " + Convert.ToString(Program.intUsuarioId) + "";
                dtview.Sort = "tick_vIPCreacion ASC";
                DtlTicketAbandonado = dtview.ToTable();

                GridConsultaNoAtendido.AutoGenerateColumns = false;
                GridConsultaNoAtendido.DataSource = DtlTicketAbandonado;

                if (DtlTicketAbandonado.Rows[0]["serv_sTipo"].ToString() == "1")
                {
                    strServicioNombre = DtlTicketAbandonado.Rows[0]["serv_vDescripcion"].ToString();
                    strSubServicioNombre = "";
                }
                else
                {
                    strServicioNombre = ServicioPadre(Convert.ToInt16(DtlTicketAbandonado.Rows[0]["Serv_sServicioIdCab"].ToString()));
                    strSubServicioNombre = DtlTicketAbandonado.Rows[0]["serv_vDescripcion"].ToString();
                }
            }
            GridConsultaNoAtendido.AutoGenerateColumns = false;
            GridConsultaNoAtendido.DataSource = DtlTicketAbandonado;
            GridConsultaNoAtendido.Refresh();
                     
        }

        private string TicketPromedioEspera(DataTable DtTicket)
        {
            int IntFila = 0;
            int IntNumTicketSinAtender = 0;
            string VTiempoEspera = "";
            string StrHoraInicio;
            string StrHoraFinal;
            double DHoras;
            double DTotaHoras;
            double DHoraPromedio = 0;

            DTotaHoras = 0;
            for (IntFila = 0; IntFila <= DtTicket.Rows.Count - 1; IntFila++)
            {
                if (Convert.ToInt32(DtTicket.Rows[IntFila]["tick_sTipoEstado"].ToString()) == Program.intEstadoEmitido)
                {
                    IntNumTicketSinAtender = IntNumTicketSinAtender + 1;

                    StrHoraInicio = "";
                    StrHoraFinal = "";
                    DateTime datHoraInicio;

                    //datHoraInicio = Convert.ToDateTime(DtTicket.Rows[IntFila]["tick_dFechaHoraGeneracion"].ToString().Substring(0,23));
                    //datHoraInicio = Convert.ToDateTime(DtTicket.Rows[IntFila]["tick_dFechaHoraGeneracion"].ToString());

                    string strFecha = DtTicket.Rows[IntFila]["tick_dFechaHoraGeneracion"].ToString();

                    datHoraInicio = new DateTime
                    (
                    Convert.ToInt16(strFecha.Substring(0, 4)),
                    Convert.ToInt16(strFecha.Substring(4, 2)),
                    Convert.ToInt16(strFecha.Substring(6, 2)),
                    Convert.ToInt16(strFecha.Substring(9, 2)),
                    Convert.ToInt16(strFecha.Substring(11, 2)),
                    0
                    );

                    StrHoraInicio = datHoraInicio.ToString("HH:mm:ss"); //StrHoraInicio.Substring(11, 12);
                    StrHoraFinal = DateTime.Now.ToString("HH:mm:ss"); //DateTime.Now.ToLongTimeString("HH:mm:ss");
                    VTiempoEspera = MiFun.RestarHoras(StrHoraInicio, StrHoraFinal);

                    DHoras = 0;
                    DHoras = MiFun.ConvertirHorasDecimal(VTiempoEspera);
                    DTotaHoras = DTotaHoras + DHoras;
                }
            }

            if ((DTotaHoras != 0) && (IntNumTicketSinAtender != 0))
            {
                DHoraPromedio = Convert.ToDouble(DTotaHoras / IntNumTicketSinAtender);
            }

            string StrHora = MiFun.ConvertirDecimalHoras(DHoraPromedio);
            return StrHora;
        }

        private string TicketPromedioAtencion(DataTable DtTicket)
        {
            int IntFila = 0;
            int IntNumTicketSinAtender = 0;
            string VTiempoEspera = "";
            string StrHoraInicio;
            string StrHoraFinal;
            double DHoras;
            double DTotaHoras;
            double DHoraPromedio = 0;
            string StrHora = "";
            //DateTime datHoraInicio;

            DTotaHoras = 0;
            for (IntFila = 0; IntFila <= DtTicket.Rows.Count - 1; IntFila++)
            {
                if (Convert.ToInt32(DtTicket.Rows[IntFila]["tick_sTipoEstado"].ToString()) == Program.intEstadoAtendido)
                {
                    IntNumTicketSinAtender = IntNumTicketSinAtender + 1;

                    StrHoraInicio = "";
                    StrHoraFinal = "";

                    StrHoraInicio = DtTicket.Rows[IntFila]["tick_dAtencionInicio"].ToString();
                    if (StrHoraInicio.Length > 0)
                    {
                        StrHoraInicio = StrHoraInicio.Substring(11, 8);
                    }

                    StrHoraFinal = DtTicket.Rows[IntFila]["tick_dAtencionFinal"].ToString();

                    if (StrHoraFinal.Length > 0)
                    {
                        StrHoraFinal = StrHoraFinal.Substring(11, 8);
                    }

                    VTiempoEspera = MiFun.RestarHoras(StrHoraInicio, StrHoraFinal);

                    DHoras = 0;
                    DHoras = MiFun.ConvertirHorasDecimal(VTiempoEspera);
                    DTotaHoras = DTotaHoras + DHoras;
                }
            }
            if (DTotaHoras != 0)
            {
                DHoraPromedio = Convert.ToDouble(DTotaHoras / IntNumTicketSinAtender);
                StrHora = MiFun.ConvertirDecimalHoras(DHoraPromedio);
            }
            else
            {
                StrHora = "";
            }

            return StrHora;
        }

        private int ticketHallarTotales(DataTable DtTablaTicket, int IntEstadoTicket)
        {
            int A;
            int IAcumulador;
            IAcumulador = 0;

            for (A = 0; A <= DtTablaTicket.Rows.Count - 1; A++)
            {
                if (Convert.ToInt32(DtTablaTicket.Rows[A]["tick_sTipoEstado"].ToString()) == IntEstadoTicket)
                {
                    IAcumulador = IAcumulador + 1;
                }
            }

            return IAcumulador;
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void GridConsultaNoAtendido_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void MostrarLLamadasEfectuadas(string CadenaMostrar)
        {
            TlpPrincipal.Enabled = false;
            string[] words = CadenaMostrar.Split('|');
            MiFun.ListBoxLLenarConArray(ref LstLLamadas, words);
        }

        private void GrdConsulta_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void label14_Click(object sender, EventArgs e)
        {
        }

        private void label15_Click(object sender, EventArgs e)
        {
        }

        private void label18_Click(object sender, EventArgs e)
        {
        }

        private void label17_Click(object sender, EventArgs e)
        {
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label12_Click(object sender, EventArgs e)
        {
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
        }

        private void label18_Click_1(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Cuando se cancela
            if (intTipoEmision == 1 || intTipoEmision == 2)
            {
                if (DtlTicketLlamados.Rows.Count == 0)
                {
                    CmdEmiTicket.Enabled = true;
                    CmdEmiTicket.BackColor = Color.FromArgb(241, 118, 0);

                    CmdDerTicket.Enabled = false;
                    CmdDerTicket.BackColor = Color.DarkGray;
                }
                else
                {
                    CmdEmiTicket.Enabled = false;
                    CmdEmiTicket.BackColor = Color.DarkGray;

                    CmdDerTicket.Enabled = true;
                    CmdDerTicket.BackColor = Color.Gold;
                }

                TlpPrincipal.Enabled = true;

                panel4.Visible = false;
                MostrarClientes(Convert.ToInt32(Program.strServOficinaConsularCodigo));
                MostrarEstatus(Program.strArchivo);

                if (intTipoEmision == 1)
                {
                    CmdSiguiente.Enabled = true;
                    CmdSiguiente.BackColor = Color.FromArgb(192, 0, 0);
                }
            }
            else
            {
                TlpPrincipal.Enabled = true;
                panel4.Visible = false;
                btnNuevoServicio.Enabled = true;
                btnNuevoServicio.BackColor = Color.FromArgb(7, 94, 188);

            }
        }

        private void CmdDerTicket_Click(object sender, EventArgs e)
        {
            DataTable DtServ = new DataTable();
            DataSet dsLeerFicheroXML = new DataSet();

            intTipoEmision = 1;
            LblDescOpcion.Text = "Derivar Ticket";
            CmdEmiTicket.Enabled = false;
            CmdEmiTicket.BackColor = Color.DarkGray;

            CmdDerTicket.Enabled = false;
            CmdDerTicket.BackColor = Color.DarkGray;
            TlpPrincipal.Enabled = false;

            panel4.Left = ((this.Width - panel4.Width) / 2);
            panel4.Top = ((this.Height - (panel4.Height + 100)) / 2);

            dsLeerFicheroXML = LeerFicheroXML(Program.strArchivo).Copy();
            if (dsLeerFicheroXML.Tables.Count == 0)
            {
                do
                {
                    dsLeerFicheroXML = LeerFicheroXML(Program.strArchivo).Copy();
                } while (dsLeerFicheroXML.Tables.Count == 0);
            }


            string[,] Datos = new string[2, 5] { { "Table2", "serv_sServicioId", "N", "","" },
                                                 { "Table7", "vede_sServicioId", "N", "serv_sServicioId","" },
                                               };

            DtServ = MiFun.DataSetCombinarTablas(dsLeerFicheroXML, Datos);

            bCargando = true;
            int intVentanillaId = Convert.ToInt32(CboVentana.SelectedValue);
            ///*************************************************************************************************

            DtServ = MiFun.DataTableFiltrar(DtServ, "serv_sTipo = 1", "serv_IOrden ASC");

            MiFun.ComboBoxCargarDataTable(CboServicioDerivar, DtServ, "serv_sServicioId", "serv_vDescripcion");

            CboServicioDerivar.SelectedValue = 0;
            CboSubServicio.SelectedValue = 0;
                      
            bCargando = false;
            panel4.Visible = true;

            CmdSiguiente.Enabled = false;
            CmdSiguiente.BackColor = Color.DarkGray;

        }


        private void CboServicioDerivar_SelectedIndexChanged(object sender, EventArgs e)
        {
             int intVentanillaId = Convert.ToInt32(CboVentana.SelectedValue);
            if (bCargando == true) { return; }
            if (Convert.ToInt32(this.CboServicioDerivar.SelectedValue) == 0) { return; }

            bool bExisteserv_sServicioIdCab = DtServicios.Columns.Contains("serv_sServicioIdCab");

            if (bExisteserv_sServicioIdCab == true)
            {
                DataTable DtServ = new DataTable();
                DtServ = MiFun.DataTableFiltrar(DtServicios, "serv_sTipo = 2 AND serv_sServicioIdCab = " + CboServicioDerivar.SelectedValue, "serv_IOrden ASC");
                bCargando = true;
                MiFun.ComboBoxCargarDataTable(CboSubServicio, DtServ, "serv_sServicioId", "serv_vDescripcion");
                bCargando = false;
            }
        }

        private void CboSubServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bCargando == true) { return; }
            if (Convert.ToInt32(this.CboSubServicio.SelectedValue) == 0) { return; }
        }


        private bool GenerarTicketRef(ref int intTiketId, string StrArchivo, int intOficinaConsularId, int intServicioId)
        {
            int ITicketId = 0;
            int ITicketNumero = 0;
            bool bOk = false;
            string strNombreCliente = "";


            strNombreCliente = "RECURRENTE";               // SI ES UNA EMISION DE TICKET, SE CONSIDERA COMO RECURRENTE

            //  SI LA TABLA TICKET ESTA VACIA INICIALIZAMOS LA NUMERACION
            if (MiFun.XMLContarRegistros(StrArchivo, "Table1", "tick_iTicketId") == 0)
            {
                ITicketId = 1;
                ITicketNumero = 1;
            }
            else
            {
                ITicketId = MiFun.XMLMaximoRegistro(StrArchivo, "Table1", "tick_iTicketId") + 1;
                //Autor: Miguel
                //Modificación del formato de la fecha en año + mes +dia
                ITicketNumero = MiFun.XMLMaximoRegistroTicket(StrArchivo, "Table1", "tick_iNumero", DateTime.Today.ToString("yyyyMMdd")) + 1;

            }

            int intPrioridadId = 7353;    // Atención Preferencial


            string FechaActual = Fecha.ConvertirFecha(DateTime.Now);

            object[] MisCampos = new object[24] {   new XElement("tick_iTicketId",ITicketId),
                                                    new XElement("tick_iOficinaConsularId",intOficinaConsularId),
                                                    new XElement("tick_sTipoServicioId",intServicioId),
                                                    new XElement("tick_iPersonalId",0),
                                                    new XElement("tick_iNumero",ITicketNumero),
                                                    new XElement("tick_dFechaHoraGeneracion",FechaActual),
                                                    new XElement("tick_dAtencionInicio",null),
                                                    new XElement("tick_dAtencionFinal",null),
                                                    new XElement("tick_sPrioridadId",intPrioridadId),
                                                    new XElement("tick_sTipoCliente",7403), // Tipo de Cliente: Recurrente
                                                    new XElement("tick_sTamanoTicket",Program.intTamañoTicketId.ToString()),
                                                    new XElement("tick_sTipoEstado",Program.intEstadoEmitido),
                                                    new XElement("tick_sTicketeraId",Program.intTiketeraId),
                                                    new XElement("tick_vLLamada",null),
                                                    new XElement("tick_sUsuarioAtendio",0),
                                                    new XElement("tick_cEstado","A"),
                                                    new XElement("tick_sUsuarioCreacion",1),
                                                    new XElement("tick_vIPCreacion",""),
                                                    new XElement("tick_dFechaCreacion",FechaActual),
                                                    new XElement("tick_sUsuarioModificacion",0),
                                                    new XElement("tick_vIPModificacion",null),
                                                    new XElement("tick_dFechaModificacion",null),
                                                    new XElement("tick_vNombreApellido",strNombreCliente),
                                                    new XElement("tick_bCargado",0),
                                                };

            if (MiFun.XMLAgregarNodo(StrArchivo, "Table1", MisCampos) == true)
            {
                bOk = true;
                if (ITicketId == 1)
                {
                    if (MiFun.XMLEliminarNodo(StrArchivo, "Table1", "tick_iTicketId", "0") == false)
                    {
                        // ESTE MENSAJE NUNCA DEBE DE MOSTRARSE SI ESO OCURRIERA QUIERE DECIR QUE LA FUNCION NO ESTA HACIENDO SU TRABAJO
                        MessageBox.Show("No se pudo eliminar el registro Nº 0");
                    }
                }
            }
            intTiketId = ITicketNumero;
            return bOk;
        }

        private void CmdDerivarTicket_Click(object sender, EventArgs e)
        {

            switch (intTipoEmision)
            {
                case 1: 
                    DerivarTicket(); 
                    break;
                case 2: 
                    NuevoTicket(); 
                    break;
                case 3: 
                    NuevoServicio(); 
                    break;
                default:
                    break;
            }

        }

      

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void MostrarClientesLlamados(int iConsuladoId)
        {
            DtlTicketLlamados = TicketConsultaVentanilla2(Program.strArchivo, Program.intEstadoLlamado);

            DataView dtview = new DataView(DtlTicketLlamados);


            dtview.RowFilter = "tick_sUsuarioAtendio = " + Convert.ToString(Program.intUsuarioId) + "";
            dtview.Sort = "tick_sPrioridadId DESC, tick_iNumero ASC";
            DtlTicketLlamados = dtview.ToTable();

            if (DtlTicketLlamados.Rows.Count != 0)
            {
                if (DtlTicketLlamados.Rows[0]["serv_sTipo"].ToString() == "1")
                {
                    strServicioNombre = DtlTicketLlamados.Rows[0]["serv_vDescripcion"].ToString();
                    strSubServicioNombre = "";
                }
                else
                {
                    strServicioNombre = ServicioPadre(Convert.ToInt16(DtlTicketLlamados.Rows[0]["Serv_sServicioIdCab"].ToString()));
                    strSubServicioNombre = DtlTicketLlamados.Rows[0]["serv_vDescripcion"].ToString();
                }
            }
           

            GrdConsultaLlamados.AutoGenerateColumns = false;
            GrdConsultaLlamados.DataSource = DtlTicketLlamados;
            GrdConsultaLlamados.Refresh();
        }

        private void MostrarClientesEnAtencion(int iConsuladoId)
        {

            DtlTicketEnAtencion = TicketConsultaVentanilla2(Program.strArchivo, Program.intEstadoEnAtencion);

            DataView dtview = new DataView(DtlTicketEnAtencion);
            dtview.RowFilter = "tick_sUsuarioAtendio = " + Convert.ToString(Program.intUsuarioId) + "";
            dtview.Sort = "tick_sPrioridadId DESC, tick_iNumero ASC";
            DtlTicketEnAtencion = dtview.ToTable();

            if (DtlTicketEnAtencion.Rows.Count > 0)
            {
                if (DtlTicketEnAtencion.Rows[0]["serv_sTipo"].ToString() == "1")
                {
                    strServicioNombre = DtlTicketEnAtencion.Rows[0]["serv_vDescripcion"].ToString();
                    strSubServicioNombre = "";
                }
                else
                {
                    strServicioNombre = ServicioPadre(Convert.ToInt16(DtlTicketEnAtencion.Rows[0]["Serv_sServicioIdCab"].ToString()));
                    strSubServicioNombre = DtlTicketEnAtencion.Rows[0]["serv_vDescripcion"].ToString();
                }
            }
            

            GrdConsultaEnAtencion.AutoGenerateColumns = false;
            GrdConsultaEnAtencion.DataSource = DtlTicketEnAtencion;
            GrdConsultaEnAtencion.Refresh();

        }

        private void DerivarTicket()
        {
            int intTiketId = 0;
            int intServicioId = 0;

            if (CboSubServicio.Text == "")
            {
                intServicioId = Convert.ToInt32(CboServicioDerivar.SelectedValue);
            }
            else
            {
                intServicioId = Convert.ToInt32(CboSubServicio.SelectedValue);
            }


            if (lblNoTicket.Text.Length == 0)
            {
                MessageBox.Show("Debe hacer clic en el botón Llamar Cliente", "Módulo Colas de Atención", MessageBoxButtons.OK);
                return;
            }

            intTiketId = Convert.ToInt32(lblNoTicket.Text);
            //------------------------------------------------
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Cambiar el estado del ticket a emitido
            //Fecha: 09/02/2018
            //------------------------------------------------
            ActualizarServicio(intTiketId, Program.intEstadoEmitido, intServicioId, Convert.ToInt32(CboVentana.SelectedValue.ToString()), Program.strArchivo);
            //------------------------------------------------

            if (intTiketId != 0)
            {
                MiFun.IniEscribir("GENERADO", "TICKET", Convert.ToString(intTiketId), Program.vRutaBDLocal + "\\" + Program.strIniServidor);

                string strNumero = Convert.ToString(intTiketId);
               
                MiFun.IniEscribir("LLAMAR", "IDTICKETFINALIZADO", strNumero, Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                
            }
            else
            {
                MessageBox.Show("No se pudo generar el ticket, Consulte con el administrador del sistema", "Módulo Colas de Atención", MessageBoxButtons.OK);
            }
            //--------------------------------------------------------
            MostrarClientes(Program.intServOficinaConsularId);
            MostrarClientesLlamados(Convert.ToInt32(Program.strServOficinaConsularCodigo));

            Application.DoEvents();
            //--------------------------------------------------------

            CmdRemite.Enabled = true;
            CmdRemite.BackColor = Color.FromArgb(54, 153, 235);

            CmdSiguiente.Enabled = true;
            CmdSiguiente.BackColor = Color.FromArgb(192, 0, 0);

            CmdLlamada.Enabled = true;
            CmdLlamada.BackColor = Color.FromArgb(0, 179, 134);

            CmdFinalizaAtencion.Enabled = false;
            CmdFinalizaAtencion.BackColor = Color.DarkGray;

            TlpPrincipal.Enabled = true;

            panel4.Visible = false;

            double decHora = 0;
            LblHorFin.Text = DateTime.Now.ToLongTimeString().Substring(0, 8);
            decHora = MiFun.ConvertirHorasDecimal(LblHorFin.Text) - MiFun.ConvertirHorasDecimal(LblHorIni.Text);
            TxtNombre.Text = "";
            TxtNomServ.Text = "";
            TxtSubServicio.Text = "";
            LblHorTra.Text = MiFun.ConvertirDecimalHoras(decHora);
            intIdTicketGrilla_New = 0;
            lblNoTicket.Text = "";
            CmdEmiTicket.Enabled = true;
            CmdEmiTicket.BackColor = Color.FromArgb(241, 118, 0);
            IntNumeroVecesLlamado = 0;
        }

        private void NuevoTicket()
        {
            int intTiketId = 0;
            int intServicioId = 0;

            if (CboSubServicio.Text == "")
            {
                intServicioId = Convert.ToInt32(CboServicioDerivar.SelectedValue);
            }
            else
            {
                intServicioId = Convert.ToInt32(CboSubServicio.SelectedValue);
            }


            if (GenerarTicketRef(ref intTiketId, Program.strArchivo, Convert.ToInt32(Program.strServOficinaConsularCodigo), intServicioId) == true)
            {
                if (intTiketId != 0)
                {
                    MiFun.IniEscribir("GENERADO", "TICKET", Convert.ToString(intTiketId), Program.vRutaBDLocal + "\\" + Program.strIniServidor);

                    Funciones xMiFun = new Funciones();
                    string VRutaArchivo;
                    string VFecha = DateTime.Now.ToString("MMM-dd-yyyy HH:mm:ss").ToUpper();
                    string strNumero = Convert.ToString(intTiketId);
                    VRutaArchivo = Program.vRutaReportes + "\\rsTicket.rdlc";

                    string[,] Parametros = new string[5, 2] {
                                                {"Numero",strNumero},
                                                {"Ventanilla","V2"},
                                                {"NomConsulado",Program.strServOficinaConsularNombre},
                                                {"FchEmision",VFecha},
                                                {"NombreServicio",CboSubServicio.Text},
                                            };

                    // SI LA CONSTANTE IImprimirTicket = 1 QUIERE DECIR QUE SE IMPRIMIRAN LOS TICKETS
                    DataSet DsDatos = new DataSet();

                    if (Program.intImprimirTicket == 1)
                    {
                        xMiFun.CrystalVisor(Parametros, "SISTEMA DE COLAS - EMISION DE TICKET", VRutaArchivo, DsDatos);
                    }
                    else
                    {
                        MessageBox.Show("TICKET Nº " + Convert.ToString(intTiketId), "Módulo Colas de Atención", MessageBoxButtons.OK);

                        //Impresión directa

                        MasterImpresion masterPrinter = new MasterImpresion();

                        masterPrinter._pLocalReport.ReportPath = VRutaArchivo;
                        masterPrinter._parrparametros = Parametros;
                        masterPrinter._pOrigenesDatosReporte = null;
                        masterPrinter.Imprimir();

                        if (masterPrinter._pintErrorNumero != 0)
                        {
                            MessageBox.Show("No se pudo accesar a la impresora " + Program.strImpresoraTicket + ", Su número de Ticket es " + strNumero + " espere su llamada", "Módulo Colas de Atención", MessageBoxButtons.OK);
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("No se pudo generar el ticket, Consulte con el administrador del sistema", "Módulo Colas de Atención", MessageBoxButtons.OK);
            }

            CmdEmiTicket.Enabled = true;
            CmdEmiTicket.BackColor = Color.FromArgb(241, 118, 0);

            TlpPrincipal.Enabled = true;

            panel4.Visible = false;

        }

        private void btnNuevoServicio_Click(object sender, EventArgs e)
        {
            DataTable DtServ = new DataTable();
            DataSet dsLeerFicheroXML = new DataSet();

            intTipoEmision = 3;
            LblDescOpcion.Text = "Servicio Adicional";
            CmdEmiTicket.Enabled = false;
            CmdEmiTicket.BackColor = Color.DarkGray;

            TlpPrincipal.Enabled = false;
            panel4.Left = ((this.Width - panel4.Width) / 2);
            panel4.Top = ((this.Height - (panel4.Height + 100)) / 2);

            dsLeerFicheroXML = LeerFicheroXML(Program.strArchivo).Copy();

            if (dsLeerFicheroXML.Tables.Count == 0)
            {
                do
                {
                    dsLeerFicheroXML = LeerFicheroXML(Program.strArchivo).Copy();
                } while (dsLeerFicheroXML.Tables.Count == 0);                
            }
            string[,] Datos = new string[2, 5] {
                                                   { "Table2", "serv_sServicioId", "N", "","" },
                                                   { "Table7", "vede_sServicioId", "N", "serv_sServicioId","" },
                                               };

            DtServ = MiFun.DataSetCombinarTablas(dsLeerFicheroXML, Datos);

            bCargando = true;
            int intVentanillaId = Convert.ToInt32(CboVentana.SelectedValue);
            DtServ = MiFun.DataTableFiltrar(DtServ, "serv_sTipo = 1", "serv_IOrden ASC");
            MiFun.ComboBoxCargarDataTable(CboServicioDerivar, DtServ, "serv_sServicioId", "serv_vDescripcion");

            CboServicioDerivar.SelectedValue = 0;
            CboSubServicio.SelectedValue = 0;
            panel4.Visible = true;
            bCargando = false;

            btnNuevoServicio.Enabled = false;
            btnNuevoServicio.BackColor = Color.DarkGray;
        }

        private void NuevoServicio()
        {
            int ITicketNumero = 0;
            int intServicioId = 0;
            int intTicketId = 0;
            bool BolOk = false;
            string strTicketGrillaActual = "";

            if (GrdConsultaEnAtencion.RowCount != 0)
            { strTicketGrillaActual = Convert.ToString(GrdConsultaEnAtencion.Rows[0].Cells[4].Value); }
            else
            {
                MessageBox.Show("No existe ticket en atención.", Program.strTituloSistema, MessageBoxButtons.OK);
                return;
            }
            
            
            ITicketNumero = Convert.ToInt32(lblNoTicket.Text);
            //-------------------------------------------------------------
            //Fecha: 20/02/2018
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Finalizamos el ticket anterior
            //-------------------------------------------------------------
            if (GrdConsultaEnAtencion.RowCount != 0) { intTicketId = Convert.ToInt32(GrdConsultaEnAtencion.Rows[0].Cells[4].Value); }

            
            ActualizarEstado(intTicketId, Program.intEstadoAtendido, Convert.ToInt32(CboVentana.SelectedValue.ToString()), Program.strArchivo);
            
            BolOk = ActualizarFechaFinal(intTicketId, DateTime.Now.ToString(Program.strFormatoFecha), Program.strArchivo);
            //-------------------------------------------------------------
            double decHora = 0;
            LblHorFin.Text = DateTime.Now.ToLongTimeString().Substring(0, 8);
            LblHorTra.Text = MiFun.ConvertirDecimalHoras(decHora);
            MiFun.IniEscribir("LLAMAR", "IDTICKETFINALIZADO", strTicketGrillaActual, Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            
            intIdTicketGrilla_New = 0;
            lblNoTicket.Text = "";
            CmdDerTicket.Enabled = false;
            CmdDerTicket.BackColor = Color.DarkGray;

            MostrarClientesAtendidos(Convert.ToInt32(Program.intServOficinaConsularId));

            if (CboSubServicio.Text == "")
            {
                intServicioId = Convert.ToInt32(CboServicioDerivar.SelectedValue);
            }
            else
            {
                intServicioId = Convert.ToInt32(CboSubServicio.SelectedValue);
            }
            if (GenerarTicketServicio(ref intTicketId, Program.strArchivo, Convert.ToInt32(Program.strServOficinaConsularCodigo), intServicioId, Convert.ToInt32(CboVentana.SelectedValue.ToString())) == true)
            {

                IntNumeroVecesLlamado = 0;                 // INICIALIZAMOS EL NUMERO DE LLAMADAS PARA EL NUEVO TICKET
                LblTitulo.Text = "Atendiendo a:";

                MostrarClientesEnAtencion(Convert.ToInt32(Program.strServOficinaConsularCodigo));

                int intIdTicketGrilla = 0;
                int intTicketGrillaActual = 0;
                string intTicketGrillaActualNombre = "";

                intEstadoActual = Program.intEstadoEnAtencion;
                if (DtlTicketEnAtencion.Rows.Count > 0)
                {
                    intIdTicketGrilla = Convert.ToInt32(GrdConsultaEnAtencion.Rows[0].Cells[4].Value);
                    intTicketGrillaActual = Convert.ToInt32(GrdConsultaEnAtencion.Rows[0].Cells[2].Value);
                    intTicketGrillaActualNombre = GrdConsultaEnAtencion.Rows[0].Cells[0].Value.ToString();
                    strLlamadasRealizadas_New = Convert.ToString(GrdConsultaEnAtencion.Rows[0].Cells[5].Value);
                }
                intIdTicketGrilla_New = intIdTicketGrilla;
                intTicketGrillaActual_New = intTicketGrillaActual;
                strTicketGrillaActualNombre_New = intTicketGrillaActualNombre;
                TxtNomServ.Text = strServicioNombre;
                TxtSubServicio.Text = strSubServicioNombre;
                lblNoTicket.Text = intTicketGrillaActual_New.ToString();
                LblHorIni.Text = DateTime.Now.ToLongTimeString().Substring(0, 8);
                LblHorFin.Text = "";
                LblHorTra.Text = "";
                CmdDerTicket.Enabled = false;
                CmdDerTicket.BackColor = Color.DarkGray;

                //----------------------------------------------------------
            }
            else
            {
                MessageBox.Show("No se pudo generar el ticket, Consulte con el administrador del sistema", "Módulo Colas de Atención", MessageBoxButtons.OK);
            }

            TlpPrincipal.Enabled = true;

            panel4.Visible = false;
            btnNuevoServicio.Enabled = true;
            btnNuevoServicio.BackColor = Color.FromArgb(7, 94, 188);
        }

        private bool GenerarTicketServicio(ref int intTiketId, string StrArchivo, int intOficinaConsularId, int intServicioId, int intVentanillaId)
        {
            int ITicketId = 0;
            int ITicketNumero = 0;
            bool bOk = false;
            string strNombreCliente = "";


            strNombreCliente = "RECURRENTE";               // SI ES UNA EMISION DE TICKET, SE CONSIDERA COMO RECURRENTE

            //  SI LA TABLA TICKET ESTA VACIA INICIALIZAMOS LA NUMERACION
            if (MiFun.XMLContarRegistros(StrArchivo, "Table1", "tick_iTicketId") == 0)
            {
                intTiketId = 1;
                ITicketNumero = 1;
            }
            else
            {
                ITicketId = MiFun.XMLMaximoRegistro(StrArchivo, "Table1", "tick_iTicketId") + 1;
                ITicketNumero = MiFun.XMLMaximoRegistroTicket(StrArchivo, "Table1", "tick_iNumero", DateTime.Today.ToString("yyyyMMdd")) + 1;
            }

            int intPrioridadId = 7351;    // Atención Normal


            string FechaActual = Fecha.ConvertirFecha(DateTime.Now);

            object[] MisCampos = new object[25] {   new XElement("tick_iTicketId",ITicketId),
                                                    new XElement("tick_iOficinaConsularId",intOficinaConsularId),
                                                    new XElement("tick_sTipoServicioId",intServicioId),
                                                    new XElement("tick_iPersonalId",0),
                                                    new XElement("tick_iNumero",ITicketNumero),
                                                    new XElement("tick_dFechaHoraGeneracion",FechaActual),
                                                    new XElement("tick_dAtencionInicio",DateTime.Now.ToString(Program.strFormatoFecha)),
                                                    new XElement("tick_dAtencionFinal",null),
                                                    new XElement("tick_sPrioridadId",intPrioridadId),
                                                    new XElement("tick_sTipoCliente",7403), // Tipo de Cliente: Recurrente
                                                    new XElement("tick_sTamanoTicket",Program.intTamañoTicketId.ToString()),
                                                    new XElement("tick_sTipoEstado",Program.intEstadoEnAtencion),
                                                    new XElement("tick_sTicketeraId",Program.intTiketeraId),
                                                    new XElement("tick_vLLamada",DateTime.Now.ToString(Program.strFormatoFecha)),
                                                    new XElement("tick_sUsuarioAtendio",Convert.ToString(Program.intUsuarioId)),
                                                    new XElement("tick_cEstado","A"),
                                                    new XElement("tick_sUsuarioCreacion",1),
                                                    new XElement("tick_vIPCreacion",""),
                                                    new XElement("tick_dFechaCreacion",FechaActual),
                                                    new XElement("tick_sUsuarioModificacion",0),
                                                    new XElement("tick_vIPModificacion",null),
                                                    new XElement("tick_dFechaModificacion",null),
                                                    new XElement("tick_vNombreApellido",strNombreCliente),
                                                    new XElement("tick_bCargado",0),
                                                    new XElement("tick_sVentanillaId",intVentanillaId.ToString()),
                                                };

            if (MiFun.XMLAgregarNodo(StrArchivo, "Table1", MisCampos) == true)
            {
                bOk = true;
                if (intTiketId == 1)
                {
                    if (MiFun.XMLEliminarNodo(StrArchivo, "Table1", "tick_iTicketId", "0") == false)
                    {
                        // ESTE MENSAJE NUNCA DEBE DE MOSTRARSE SI ESO OCURRIERA QUIERE DECIR QUE LA FUNCION NO ESTA HACIENDO SU TRABAJO
                        MessageBox.Show("No se pudo eliminar el registro Nº 0");
                    }
                }
            }
            return bOk;
        }

        private void MostrarClientesAtendidos(int iConsuladoId)
        {

            DtlTicketAtendidos = TicketConsultaVentanilla2(Program.strArchivo, Program.intEstadoAtendido);

            DataView dtview = new DataView(DtlTicketAtendidos);
            dtview.RowFilter = "tick_sUsuarioAtendio = " + Convert.ToString(Program.intUsuarioId) + "";
            DtlTicketAtendidos = dtview.ToTable();
         
            GridAtendidos.AutoGenerateColumns = false;
            GridAtendidos.DataSource = DtlTicketAtendidos;
            GridAtendidos.Refresh();
        }

    }
}
