using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;
using SGAC.Accesorios;

namespace SGAC.WinApp
{
    public partial class FrmMenuServicio : Form
    {
        public DataTable RsServicios = new DataTable();       // ALMACENA LOS SERVICIOS DE LA OFICINA CONSULAR PARA MOSTRAR LOS SUBSERVCIOS
        public string NombrePersona = "";
        public int intPrioridad;
        public int intPersonaId;
        public int intTicketeraId;                            // ID DEL CONSULADO
        public string strPersonaNombre;                       // NOMBRE DEL CONNACIONAL
        private int intServicioID;                                    // ALMACENA EL ID DEL SERVICIO SELECCIONADO
        public int intTipoCliente;                            // ALMACENA EL TIPO DE CLIENTE
        private Funciones MiFun = new Funciones();

        private List<CL_SERVICIO> LstServicio = new List<CL_SERVICIO>();    // LISTA QUE ALMACENA LOS SERVICIOS DEL CONSULADO ACTUAL

        public FrmMenuServicio()
        {
            InitializeComponent();
        }

        private void FrmServicio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void FrmServicio_Load(object sender, EventArgs e)
        {
            LblNomConsulado.Text = Program.strServOficinaConsularNombre;
            TlyTitulo.Top = 0;
            TlyTitulo.Left = 0;
            TlyTitulo.Width = this.Width;

            TlyPrincipal.Left = 0;
            TlyPrincipal.Top = 93;

            TlyPrincipal.Width = this.Width - 20;
            TlyPrincipal.Height = this.Height - 80;

            TlyTituloPie.Width = this.Width;
            TlyTituloPie.Left = 0;
            TlyTituloPie.Top = this.Height - 30;

            if (intTipoCliente == 7401) { LblNombre.Text = strPersonaNombre; }
            if (intTipoCliente == 7402) { LblNombre.Text = "CONNACIONAL SIN DOCUMENTO"; }
            if (intTipoCliente == 7403) { LblNombre.Text = "RECURRENTE"; }

            if (intPrioridad == 7351) { LblTipoAtencion.Text = "ATENCION NORMAL"; }
            if (intPrioridad == 7352) { LblTipoAtencion.Text = "ATENCION INTERMEDIA"; }
            if (intPrioridad == 7353) { LblTipoAtencion.Text = "ATENCION PREFERENCIAL"; }

            LblNombre.Refresh();
            panel1.Refresh();
            intServicioID = 0;
            timer1.Start();

            CargarMenu();
        }

        private void MostrarSubServcios(int intServicioId, string strNombreServicio)
        {
            DataView DvServicioMuestra = new DataView();

            int A = 0;

            tableLayoutPanel1.Visible = false;

            LblNombre.Visible = false;
            LblTituloServicio.Visible = true;
            LblTituloServicio.Text = strNombreServicio;

            TlySubServicio.Visible = true;
            PnlSubServicio.Visible = true;

            CmdPrev.Visible = true;

            DvServicioMuestra = MiFun.DataViewFiltrar(RsServicios, "serv_sServicioIdCab = " + intServicioId + "", "serv_IOrden DESC");

            if (DvServicioMuestra.Count != 0)
            {
                System.Windows.Forms.Button[] radioButtons2 = new System.Windows.Forms.Button[DvServicioMuestra.Count];

                System.Windows.Forms.Button[] btnArray = new System.Windows.Forms.Button[26];
                for (A = 0; A < DvServicioMuestra.Count; A++)
                {
                    radioButtons2[A] = new Button();

                    radioButtons2[A].Text = DvServicioMuestra[A]["serv_vDescripcion"].ToString(); // LstServicio[A].serv_vDescripcion.ToString();
                    radioButtons2[A].Location = new System.Drawing.Point(10, 10 + A * 20);
                    radioButtons2[A].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);

                    radioButtons2[A].FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                    radioButtons2[A].BackColor = System.Drawing.Color.DarkRed;
                    radioButtons2[A].ForeColor = System.Drawing.Color.White;
                    radioButtons2[A].Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    radioButtons2[A].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    radioButtons2[A].Cursor = Cursors.Hand;
                    radioButtons2[A].Click += new EventHandler(CmdGeneraTicket2_Click);

                    this.Controls.Add(radioButtons2[A]);
                }

                for (A = 0; A < DvServicioMuestra.Count; A++)
                {
                    this.TlySubServicio.Controls.Add(radioButtons2[A], 0, 0);
                }
            }
        }

        private void CmdGeneraTicket2_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            DataTable rsServicioMuestra = new DataTable();
            //int intServicioId = 0;
            string strServicio = btn.Text;

            rsServicioMuestra = RsServicios;
            rsServicioMuestra = MiFun.DataTableFiltrar(rsServicioMuestra, "serv_vDescripcion = '" + strServicio + "' and serv_sTipo = 2", "serv_iOrden ASC");

            intServicioID = Convert.ToInt32(rsServicioMuestra.Rows[0]["serv_sServicioId"].ToString());

            int intTiketId = 0;


            string strArchivo = Program.strArchivo.ToString();


            if (GenerarTicket(ref intTiketId, strArchivo) == true)
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
                                                    {"NombreServicio",btn.Text},
                                                };

                    // SI LA CONSTANTE IImprimirTicket = 1 QUIERE DECIR QUE SE IMPRIMIRAN LOS TICKETS
                    DataSet DsDatos = new DataSet();
                    if (Program.intImprimirTicket == 1)
                    {
                        xMiFun.CrystalVisor(Parametros, "SISTEMA DE COLAS - EMISION DE TICKET", VRutaArchivo, DsDatos);
                        //xMiFun.parameters(Parametros, "SISTEMA DE COLAS - EMISION DE TICKET", VRutaArchivo, DsDatos);

                    }
                    else
                    {
                        // Imprimir directamente a la impresora
                        MasterImpresion masterPrinter = new MasterImpresion();

                        masterPrinter._pLocalReport.ReportPath = VRutaArchivo;
                        masterPrinter._parrparametros = Parametros;
                        masterPrinter._pOrigenesDatosReporte = null;
                        masterPrinter.Imprimir();
                        if (masterPrinter._pintErrorNumero != 0)
                        {
                            MessageBox.Show("No se pudo accesar a la impresora " + Program.strImpresoraTicket + ", Su número de Ticket es " + strNumero + " espere su llamada", "Módulo Colas de Atención", MessageBoxButtons.OK);
                        }

                        //xMiFun.CrystalImprimir(Parametros, "SISTEMA DE COLAS - EMISION DE TICKET", VRutaArchivo, DsDatos, Program.strImpresoraTicket);
                        ////MessageBox.Show("TICKET Nº " + Convert.ToString(intTiketId), "Módulo Colas de Atención", MessageBoxButtons.OK);
                        //if (xMiFun.intErrorNumero != 0)
                        //{
                        //    MessageBox.Show("No se pudo accesar a la impresora " + Program.strImpresoraTicket + ", Su número de Ticket es " + strNumero + " espere su llamada", "Módulo Colas de Atención", MessageBoxButtons.OK);
                        //}
                    }
                }
            }
            else
            {
                //MessageBox.Show("No se pudo generar el ticket, Consulte con el administrador del sistema", "Módulo Colas de Atencion", MessageBoxButtons.OK);
                LblMensajes.Text = "No se pudo generar el ticket, Consulte con el administrador del sistema";
            }
            this.Close();
        }

        private void CmdGeneraTicket_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            DataTable rsServicioMuestra = new DataTable();
            int intServicioId = 0;

            string strServicio = btn.Text;

            rsServicioMuestra = RsServicios;
            rsServicioMuestra = MiFun.DataTableFiltrar(rsServicioMuestra, "serv_vDescripcion = '" + strServicio + "' and serv_sTipo = 1", "serv_iOrden ASC");

            intServicioId = Convert.ToInt16(rsServicioMuestra.Rows[0]["serv_sServicioId"].ToString());

            bool bExisteserv_sServicioIdCab = rsServicioMuestra.Columns.Contains("serv_sServicioIdCab");

            if (bExisteserv_sServicioIdCab == true)
            {
                rsServicioMuestra = RsServicios;
                rsServicioMuestra = MiFun.DataTableFiltrar(rsServicioMuestra, "serv_sServicioIdCab = " + intServicioId + " and serv_sTipo = 2", "serv_iOrden ASC");
            }
            if (rsServicioMuestra.Rows.Count != 0 && bExisteserv_sServicioIdCab)
            {
                MostrarSubServcios(intServicioId, strServicio);
            }
            else
            {
                int A;
                for (A = 0; A < LstServicio.Count; A++)
                {
                    if (LstServicio[A].serv_vDescripcion.ToString() == btn.Text)
                    {
                        intServicioID = Convert.ToInt32(LstServicio[A].serv_sServicioId.ToString());

                        int intTiketId = 0;
                        if (GenerarTicket(ref intTiketId, Program.strArchivo) == true)
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
                                                         {"NombreServicio",btn.Text},
                                                        };

                                // SI LA CONSTANTE IImprimirTicket = 1 QUIERE DECIR QUE SE IMPRIMIRAN LOS TICKETS
                                DataSet DsDatos = new DataSet();
                                if (Program.intImprimirTicket == 1)
                                {
                                    xMiFun.CrystalVisor(Parametros, "SISTEMA DE COLAS - EMISION DE TICKET", VRutaArchivo, DsDatos);
                                }
                                else
                                {
                                    //Impresión directamente a la impresora

                                    MasterImpresion masterPrinter = new MasterImpresion();

                                    masterPrinter._pLocalReport.ReportPath = VRutaArchivo;
                                    masterPrinter._parrparametros = Parametros;
                                    masterPrinter._pOrigenesDatosReporte = null;
                                    masterPrinter.Imprimir();

                                    if (masterPrinter._pintErrorNumero != 0)
                                    {
                                        MessageBox.Show("No se pudo accesar a la impresora " + Program.strImpresoraTicket + ", Su número de Ticket es " + strNumero + " espere su llamada", "Módulo Colas de Atención", MessageBoxButtons.OK);
                                    }

                                    //xMiFun.CrystalImprimir(Parametros, "SISTEMA DE COLAS - EMISION DE TICKET", VRutaArchivo, DsDatos, Program.strImpresoraTicket);
                                    ////MessageBox.Show("TICKET Nº " + Convert.ToString(intTiketId), "Módulo Colas de Atención", MessageBoxButtons.OK);
                                    //if (xMiFun.intErrorNumero != 0)
                                    //{
                                    //    MessageBox.Show("No se pudo accesar a la impresora " + Program.strImpresoraTicket + ", Su número de ticket es " + strNumero + " espere su llamada", "Módulo Colas de Atención", MessageBoxButtons.OK);
                                    //}
                                }
                            }
                        }
                        else
                        {
                            //MessageBox.Show("No se pudo generar el ticket, Consulte con el administrador del sistema", "Módulo Colas de Atencion", MessageBoxButtons.OK);
                            LblMensajes.Text = "No se pudo generar el ticket, Consulte con el administrador del sistema";
                        }
                    }
                }
                this.Close();
            }
        }

        //*********************************************************************************
        //**                             F U N C I O N E S                               **
        //*********************************************************************************

        private bool GenerarTicket(ref int intTiketId, string StrArchivo)
        {
            int ITicketId = 0;
            int ITicketNumero = 0;
            bool bOk = false;

            
            //  SI LA TABLA TICKET ESTA VACIA INICIALIZAMOS LA NUMERACION
            if (MiFun.XMLContarRegistros(StrArchivo, "Table1", "tick_iTicketId") == 0)
            {
                ITicketId = 1;
                ITicketNumero = 1;
            }
            else
            {
                ITicketId = MiFun.XMLMaximoRegistro(StrArchivo, "Table1", "tick_iTicketId") + 1;
                ITicketNumero = MiFun.XMLMaximoRegistroTicket(StrArchivo, "Table1", "tick_iNumero", Fecha.ConvertirFecha(DateTime.Today)) + 1;
            }
            

            string strFechaActual = Fecha.ConvertirFecha(DateTime.Now);

            object[] MisCampos = new object[24] {   new XElement("tick_iTicketId",ITicketId),
                                                    new XElement("tick_iOficinaConsularId",Program.strServOficinaConsularCodigo),
                                                    new XElement("tick_sTipoServicioId",intServicioID),
                                                    new XElement("tick_iPersonalId",intPersonaId),
                                                    new XElement("tick_iNumero",ITicketNumero),
                                                    new XElement("tick_dFechaHoraGeneracion", strFechaActual),
                                                    new XElement("tick_dAtencionInicio",null),
                                                    new XElement("tick_dAtencionFinal",null),
                                                    new XElement("tick_sPrioridadId",Convert.ToString(intPrioridad)),
                                                    new XElement("tick_sTipoCliente",intTipoCliente),
                                                    new XElement("tick_sTamanoTicket",Program.intTamañoTicketId.ToString()),
                                                    new XElement("tick_sTipoEstado",Program.intEstadoEmitido),
                                                    new XElement("tick_sTicketeraId",Program.intTiketeraId),
                                                    new XElement("tick_vLLamada",null),
                                                    new XElement("tick_sUsuarioAtendio",0),
                                                    new XElement("tick_cEstado","A"),
                                                    new XElement("tick_sUsuarioCreacion",1),
                                                    new XElement("tick_vIPCreacion",""),
                                                    new XElement("tick_dFechaCreacion", strFechaActual),
                                                    new XElement("tick_sUsuarioModificacion",0),
                                                    new XElement("tick_vIPModificacion",null),
                                                    new XElement("tick_dFechaModificacion",null),
                                                    new XElement("tick_vNombreApellido",LblNombre.Text),
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
                        //MessageBox.Show("No se pudo eliminar el registro Nº 0");
                        LblMensajes.Text = "No se pudo eliminar el registro Nº 0";
                    }
                }
            }
            
            //intTiketId = ITicketId;
            intTiketId = ITicketNumero;
            return bOk;
        }

        private List<CL_SERVICIO> ListarServiciosConsulado2(string StrArchivo)
        {
            //List<SGAC.BE.CL_SERVICIO> LstServicio = new List<SGAC.BE.CL_SERVICIO>();
            List<CL_SERVICIO> LstServicio = new List<CL_SERVICIO>();
            int A;
            DataTable dtResult = new DataTable();
            DataSet dsResult = new DataSet();
            dsResult.Clear();

            dsResult.ReadXml(StrArchivo);
            dtResult = dsResult.Tables[2];     // TABLA CL_SERVICIO
            RsServicios = dtResult;            // ALMACENAMOS LOS SERVICIOS CARGADOS

            dtResult = MiFun.DataTableFiltrar(dtResult, "serv_sTipo = 1", "serv_iOrden ASC");

            for (A = 0; A <= dtResult.Rows.Count - 1; A++)
            {
                //SGAC.BE.CL_SERVICIO Servicio = new SGAC.BE.CL_SERVICIO();
                CL_SERVICIO Servicio = new CL_SERVICIO();

                Servicio.serv_cEstado = dtResult.Rows[A]["serv_cEstado"].ToString();
                Servicio.serv_sOficinaConsularId = Convert.ToInt16(dtResult.Rows[A]["serv_sOficinaConsularId"].ToString());
                Servicio.serv_vDescripcion = dtResult.Rows[A]["serv_vDescripcion"].ToString();
                Servicio.serv_sServicioId = Convert.ToInt16(dtResult.Rows[A]["serv_sServicioId"].ToString());

                LstServicio.Add(Servicio);
            }
            return LstServicio;
        }

        private void CargarMenu()
        {
            int A;

            //ServicioConsultaBL xFun = new ServicioConsultaBL();
            LstServicio = ListarServiciosConsulado2(Program.strArchivo);

            if (LstServicio.Count != 0)
            {
                System.Windows.Forms.Button[] radioButtons = new System.Windows.Forms.Button[LstServicio.Count];
                System.Windows.Forms.Button[] btnArray = new System.Windows.Forms.Button[26];

                for (A = 0; A < LstServicio.Count; A++)
                {
                    radioButtons[A] = new Button();
                    radioButtons[A].Text = LstServicio[A].serv_vDescripcion.ToString();
                    radioButtons[A].Location = new System.Drawing.Point(10, 10 + A * 20);
                    radioButtons[A].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);

                    radioButtons[A].Click += new EventHandler(CmdGeneraTicket_Click);
                    radioButtons[A].FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                    radioButtons[A].BackColor = System.Drawing.Color.DarkRed;
                    radioButtons[A].ForeColor = System.Drawing.Color.White;
                    radioButtons[A].Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    radioButtons[A].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    radioButtons[A].Cursor = Cursors.Hand;
                    this.Controls.Add(radioButtons[A]);
                }

                int intFila = 0;
                int intColumna = 0;

                A = 0;
                for (A = 0; A < LstServicio.Count; A++)
                {
                    this.tableLayoutPanel1.Controls.Add(radioButtons[A], intColumna, intFila);
                    A = A + 1;
                    if (A < LstServicio.Count)
                    {
                        intColumna = intColumna + 1;
                        this.tableLayoutPanel1.Controls.Add(radioButtons[A], intColumna, intFila);
                        intColumna = 0;
                    }
                    //A = A + 1;
                }
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TituloPie.Text = DateTime.Now.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void FrmMenuServicio_Paint(object sender, PaintEventArgs e)
        {
            //LinearGradientBrush linGrBrush = new LinearGradientBrush(new Point(0, 0), new Point(200, 200), Color.FromArgb(255, 0, 0, 255), Color.FromArgb(255, 0, 255, 0));  // opaque green
            //Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            //LinearGradientBrush linGrBrush = new LinearGradientBrush(rect, Color.Maroon, Color.White, LinearGradientMode.Horizontal);
            //Pen pen = new Pen(linGrBrush, 10);

            //e.Graphics.FillRectangle(linGrBrush, 0, 0, this.Width, this.Height);
        }

        private void CmdHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CmdPrev_Click(object sender, EventArgs e)
        {
            if (TlySubServicio.Visible == true)
            {
                TlySubServicio.Controls.Clear();
                PnlSubServicio.Visible = false;
                LblTituloServicio.Visible = false;
                CmdPrev.Visible = true;
                tableLayoutPanel1.Visible = true;
                tableLayoutPanel1.Refresh();

                LblNombre.Visible = true;
                panel1.Refresh();
            }
            else
            {
                this.Close();
            }
        }

        private void LblNombre_Click(object sender, EventArgs e)
        {
        }
    }
}
