using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AxWMPLib;

namespace SGAC.WinApp
{
    public partial class FrmTelevisorDisplay : Form
    {
        private Funciones MiFun = new Funciones();
        private int IntNumParpadeadas;
        private int IntNumParpadeadas2;
        private int IntNumParpadeadas3;
        private int IntNumParpadeadas4;
        private int IntNumParpadeadas5;
        private int IntNumParpadeadas6;

//        private string StrNumRecibido;                                                       // ALMACENA EL NUMERO ENVIADO POR LOS FORMULARIOS DE LLAMADA
                                         
        private DataTable DtVentanillas = new DataTable();                                   // ALMACENA LAS VENTANILLAS DE LA OFICINA CONSULAR
        private string[,] StrEnPantalla = new string[6, 7];                                  // COLUMNAS DEL ARRAY ("Id del ticket","Numero de ticket", "Nombre del Cliente", "Ventanilla", "TIPO DE CLIENTE  0 = SIN NOMBRE;  1 = CON NOMBRE", "PARPADEA", "ETIQUETA A MOSTRAS EN LA PANTALLA")

        //Thread th1;
        //Thread th2;
        //Thread th3;
        //Thread th4;

        public FrmTelevisorDisplay()
        {
            InitializeComponent();
        }

        private void FrmDisplay2_Load(object sender, EventArgs e)
        {
            //Resolución de Monitor
            //1024x768
            //1152x864
            //1280x720
            //1280x768
            //1280x800
            //1280x960
            //1280x1024
            //1360x768
            //1366x768
            //1400x1050
            //1440x900
            //1600x900
            //1680x1050
            //1920x1080

            axWindowsMediaPlayer2.Left = 10000;
            DataSet dsLeerFicheroXML = new DataSet();
            dsLeerFicheroXML.Clear();

            dsLeerFicheroXML.ReadXml(Program.strArchivo);

            if (dsLeerFicheroXML.Tables.Count == 0)
            {
                do
                {
                    dsLeerFicheroXML.ReadXml(Program.strArchivo);

                } while (dsLeerFicheroXML.Tables.Count == 0);
            }
                                    
            
            DtVentanillas = dsLeerFicheroXML.Tables[6];        // TABLA VENTANILLAS
            
            TlpPrincipal.Left = 0;
            TlpPrincipal.Top = 0;
            this.Left = 0;
            this.Top = 0;
            this.BackColor = Color.Black;            

            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = Screen.PrimaryScreen.Bounds.Width;

            TlpPrincipal.Height = Screen.PrimaryScreen.Bounds.Height;
            TlpPrincipal.Width = Screen.PrimaryScreen.Bounds.Width;            
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = SystemInformation.PrimaryMonitorSize;


            string strFuenteNombre = MiFun.IniLeer("PARAMETROS", "FUENTE", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            int intFuenteTamaño = Convert.ToInt32(MiFun.IniLeer("PARAMETROS", "FUENTETAMAÑO", Program.vRutaBDLocal + "\\" + Program.strIniServidor));

            LblEtiqueta1.Font = new Font(strFuenteNombre, intFuenteTamaño);
            LblEtiqueta2.Font = new Font(strFuenteNombre, intFuenteTamaño);
            LblEtiqueta3.Font = new Font(strFuenteNombre, intFuenteTamaño);
            LblEtiqueta4.Font = new Font(strFuenteNombre, intFuenteTamaño);
            LblEtiqueta5.Font = new Font(strFuenteNombre, intFuenteTamaño);
            LblEtiqueta6.Font = new Font(strFuenteNombre, intFuenteTamaño);


            LblEtiqueta1.Font = new Font(LblEtiqueta1.Font, FontStyle.Bold);
            LblEtiqueta2.Font = new Font(LblEtiqueta2.Font, FontStyle.Bold);
            LblEtiqueta3.Font = new Font(LblEtiqueta3.Font, FontStyle.Bold);
            LblEtiqueta4.Font = new Font(LblEtiqueta4.Font, FontStyle.Bold);
            LblEtiqueta5.Font = new Font(LblEtiqueta5.Font, FontStyle.Bold);
            LblEtiqueta6.Font = new Font(LblEtiqueta6.Font, FontStyle.Bold);

            LblEtiqueta1.Text = "";
            LblEtiqueta2.Text = "";
            LblEtiqueta3.Text = "";
            LblEtiqueta4.Text = "";
            LblEtiqueta5.Text = "";
            LblEtiqueta6.Text = "";

            TlpPrincipal.Left = 0;
            TlpPrincipal.Top = 0;
            this.Refresh();
            

            //--------------------------------------
            //Objetivo: Leer URL del video Youtube
            //--------------------------------------
            string strYoutubeId = MiFun.IniLeer("PREFERENCIAS", "YOUTUBE", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            //strYoutubeId = "www";
            if ((strYoutubeId == "") || (strYoutubeId == "0"))
            {
                axWindowsMediaPlayer1.Dock = DockStyle.None;
                axWindowsMediaPlayer1.Left = 0;
                axWindowsMediaPlayer1.Top = 0;
                axWindowsMediaPlayer1.Height = (TlpPrincipal.Height * 75 / 100); 
                axWindowsMediaPlayer1.Width = TlpPrincipal.Width;
                axWindowsMediaPlayer1.Visible = true;                
                gbBrowser.Visible = false;
                                
                CargarVideos();
                string strToolTip = "Docle clic con el botón izquierdo del mouse para ampliar a Pantalla completa o volver a su estado original / Tecla [ESC] = Cerrar Pantalla.";
                toolTip1.SetToolTip(TlpPrincipal, strToolTip);
                toolTip1.SetToolTip(axWindowsMediaPlayer1, strToolTip);
                toolTip1.SetToolTip(this, strToolTip);
            }
            else
            {                
                axWindowsMediaPlayer1.Visible = false;
                axWindowsMediaPlayer1.Dock = DockStyle.None;
                gbBrowser.Visible = true;
                
                
                ConfigurarSizeWebBrowser();

                int intYoutubeId = Convert.ToInt32(strYoutubeId);
                string strURLYotube = Program.ObtenerURLYoutube(Program.vRutaBDLocal.Trim() + "\\data.xml", intYoutubeId);
              //  string strURLYotube = "https://www.youtube.com/watch?v=yawsbr3fClk";
                VerVideoYoutube(strURLYotube);
                string strToolTip = "Tecla [F1] = Pantalla completa / Tecla [<-] = Restaurar Pantalla / Tecla [ESC] = Cerrar Pantalla.";
                toolTip1.SetToolTip(TlpPrincipal, strToolTip);
                toolTip1.SetToolTip(tableLayoutPanel2, strToolTip);
                toolTip1.SetToolTip(webBrowser1, strToolTip);
                toolTip1.SetToolTip(gbBrowser, strToolTip);
                toolTip1.SetToolTip(this, strToolTip);
            }
            //--------------------------------------

            

            for (int i = 0; i <= 5; i++)
            {
                    StrEnPantalla[i, 0] = "";
                    StrEnPantalla[i, 1] = "";
                    StrEnPantalla[i, 2] = "";
                    StrEnPantalla[i, 3] = "";
                    StrEnPantalla[i, 4] = "";
                    StrEnPantalla[i, 5] = "";
                    StrEnPantalla[i, 6] = "";
            }

            timer1.Interval = 300;            
            timer1.Start();

            timer3.Interval = 300;
            timer3.Start();


           


            //th1 = new Thread(new ThreadStart(Tiempo1));
            //th2 = new Thread(new ThreadStart(Tiempo2));
            //th3 = new Thread(new ThreadStart(Tiempo3));
            //th4 = new Thread(new ThreadStart(Tiempo4));
        }

        private void ReproducirWAV()
        {
            string strRutaArchivo = "Z:\\Sonidos\\Tema01.mp3";

            axWindowsMediaPlayer2.uiMode = "none";                                 // INDICA QUE SE OCULTARAN LOS CONTROLES DEL CONTROL
            //axWindowsMediaPlayer2.settings.setMode("loop", true);                  // INDICA QUE SE REPETIRA INDEFINIDAMENTE EL VIDEO
            axWindowsMediaPlayer2.URL = strRutaArchivo;    // REPRODUCE LA LISTA DE REPRODUCCION
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (IntNumParpadeadas <= 6)
            {
                IntNumParpadeadas = IntNumParpadeadas + 1;
                LblEtiqueta1.ForeColor = Color.FromArgb(255, 0, 0); // Color Rojo
                LblEtiqueta1.Visible = !LblEtiqueta1.Visible;                
            }
            else
            {
                IntNumParpadeadas = 0;
                LblEtiqueta1.ForeColor = Color.FromArgb(0, 255, 0); // Color Verde
                LblEtiqueta1.Visible = true;                
                timer2.Stop();
            }
        }

        // ESTE TIMER LEE A CADA MOMENTO EL ARCHIVO INI EN BUSCA DE NUEVAS LLAMADAS Y LO ALMACENA EN EL ARRAY PARA SER LLAMADO
        private void timer1_Tick(object sender, EventArgs e)
        {
            //LEEMOS EL NUMERO DE TICKET LLAMADO
            string strIdTicket;
            string strNumeroTicket;
            string strVentanilla;
            string strNombre;
            string strTipCliente = "0";
            string strParpadea = "1";
            string strTicketFinalizado = "0";

            strNumeroTicket = MiFun.IniLeer("LLAMAR", "NUMERO", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            strIdTicket = MiFun.IniLeer("LLAMAR", "IDTICKET", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            strNombre = MiFun.IniLeer("LLAMAR", "NOMBRE", Program.vRutaBDLocal + "\\" + Program.strIniServidor);            
            strVentanilla = MiFun.IniLeer("LLAMAR", "VENTANA", Program.vRutaBDLocal + "\\" + Program.strIniServidor);

            strTicketFinalizado = MiFun.IniLeer("LLAMAR", "IDTICKETFINALIZADO", Program.vRutaBDLocal + "\\" + Program.strIniServidor);

            //StrNumRecibido = strNumeroTicket;

            if ((strNombre != "RECURRENTE") && (strNombre != "CONNACIONAL SIN DOCUMENTO"))
            {
                strTipCliente = "1";
            }

            if (strTicketFinalizado != "0")
            {
                StrEnPantalla = ArrayBorrarElemento(strTicketFinalizado);
                MiFun.IniEscribir("LLAMAR", "IDTICKETFINALIZADO", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
            }
            else
            {
                // BUSCAMOS EL ID DEL TICKET EN EL ARRAY
                if (strIdTicket != "0" && strNumeroTicket != "0" && strVentanilla != "0")
                {
                    int? intIdBuscar = null;

                    intIdBuscar = ArrayBuscar(strIdTicket);
                    if (intIdBuscar == null)
                    {                        
                        ArrayInsertar(strIdTicket, strNumeroTicket, strNombre, strVentanilla, strTipCliente, strParpadea);
                    }
                    else
                    {
                        ArrayActualizar(strIdTicket, "1");
                    }
                    // ACTUALIZAMOS DE NUEVO A 0 PARA QUE EN LA PROX LLAMADA NO LEVANTE ESTE NUMERO
                    MiFun.IniEscribir("LLAMAR", "NUMERO", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                    MiFun.IniEscribir("LLAMAR", "IDTICKET", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                    MiFun.IniEscribir("LLAMAR", "NOMBRE", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                    MiFun.IniEscribir("LLAMAR", "VENTANA", "0", Program.vRutaBDLocal + "\\" + Program.strIniServidor);
                }
            }
            Application.DoEvents();
            
        }

        private string[,] ArrayBorrarElemento(string strIdTicketBorrar)
        {
            int intFila = 0;
            //int intFila2 = 0;

           // string[,] StrEnPantalla2 = new string[4, 7];               // COLUMNAS DEL ARRAY ("Id del ticket","Numero de ticket", "Nombre del Cliente", "Ventanilla", "TIPO DE CLIENTE  0 = SIN NOMBRE;  1 = CON NOMBRE", "PARPADEA", "ETIQUETA A MOSTRAS EN LA PANTALLA")

            for (intFila = 0; intFila <= 5; intFila++)
            {
                if (StrEnPantalla[intFila, 0] == strIdTicketBorrar)   // SI LA FILA DEL ARRAY ESTA VACIA INSERTAMOS EL NUMERO DE TICKET TRAIDO
                {
                    StrEnPantalla[intFila, 0] = "";
                    StrEnPantalla[intFila, 1] = "";
                    StrEnPantalla[intFila, 2] = "";
                    StrEnPantalla[intFila, 3] = "";
                    StrEnPantalla[intFila, 4] = "";
                    StrEnPantalla[intFila, 5] = "";
                    StrEnPantalla[intFila, 6] = "";

                    if (intFila == 0) { LblEtiqueta1.Text = StrEnPantalla[0, 6]; }
                    if (intFila == 1) { LblEtiqueta2.Text = StrEnPantalla[1, 6]; }
                    if (intFila == 2) { LblEtiqueta3.Text = StrEnPantalla[2, 6]; }
                    if (intFila == 3) { LblEtiqueta4.Text = StrEnPantalla[3, 6]; }
                    if (intFila == 4) { LblEtiqueta5.Text = StrEnPantalla[4, 6]; }
                    if (intFila == 5) { LblEtiqueta6.Text = StrEnPantalla[5, 6]; }

                    Application.DoEvents();
                    break;
                }
            }

            // PASAMOS EL ARRAY AL NUEVO ARRAY
            //for (intFila = 0; intFila < 4; intFila++)
            //{
            //    if (MiFun.NulosC(StrEnPantalla[intFila, 0]) != "")
            //    {
            //        StrEnPantalla2[intFila2, 0] = StrEnPantalla[intFila, 0];
            //        StrEnPantalla2[intFila2, 1] = StrEnPantalla[intFila, 1];
            //        StrEnPantalla2[intFila2, 2] = StrEnPantalla[intFila, 2];
            //        StrEnPantalla2[intFila2, 3] = StrEnPantalla[intFila, 3];
            //        StrEnPantalla2[intFila2, 4] = StrEnPantalla[intFila, 4];
            //        StrEnPantalla2[intFila2, 5] = StrEnPantalla[intFila, 5];
            //        StrEnPantalla2[intFila2, 6] = StrEnPantalla[intFila, 6];
            //        intFila2 = intFila2 + 1;
            //    }
            //}

            return StrEnPantalla;
        }

        private int? ArrayBuscar(string strIdTicket)
        {
            int intFila = 0;
            bool SeEncontro = false;

            for (intFila = 0; intFila <= 5; intFila++)
            {
                if (StrEnPantalla[intFila, 0] == strIdTicket)    // SI LA FILA DEL ARRAY ESTA VACIA INSERTAMOS EL NUMERO DE TICKET TRAIDO
                {
                    SeEncontro = true;
                    break;                        // TIPO DE CLIENTE
                }
            }

            if (SeEncontro == true)
            {
                return intFila;
            }
            else
            {
                return null;
            }
        }

        private void ArrayActualizar(string strIdTicket, string strParpadea)
        {
            int intFila = 0;

            for (intFila = 0; intFila <= 5; intFila++)
            {
                if (StrEnPantalla[intFila, 0] == strIdTicket)      
                {
                    StrEnPantalla[intFila, 5] = strParpadea;  // PARPADEA     
                    break;
                }
            }
        }

        private void ArrayInsertar(string strIdTicket, string strNumeroTicket, string strNombre, string strVentanilla, string strTipCliente, string strParpadea)
        {
            int intFila = 0;
            string strCadena = "";
            bool bExisteCasillaVacia = false;

            for (intFila = 0; intFila <= 5; intFila++)
            {
                //if (StrEnPantalla[intFila, 0] == null)                // SI LA FILA DEL ARRAY ESTA VACIA INSERTAMOS EL NUMERO DE TICKET TRAIDO
                  if (StrEnPantalla[intFila, 0].Length == 0)                // SI LA FILA DEL ARRAY ESTA VACIA INSERTAMOS EL NUMERO DE TICKET TRAIDO
                    {                        
                        bExisteCasillaVacia = true;
                        StrEnPantalla[intFila, 0] = strIdTicket;          // ID DEL TICKET
                        StrEnPantalla[intFila, 1] = strNumeroTicket;      // NUMERO DE TICKET
                        StrEnPantalla[intFila, 2] = strNombre;            // NOMBRE DEL CLIENTE
                        StrEnPantalla[intFila, 3] = strVentanilla;        // NUMERO DE VENTANILLA
                        StrEnPantalla[intFila, 4] = strTipCliente;        // TIPO DE CLIENTE
                        StrEnPantalla[intFila, 5] = strParpadea;          // PARPADEA

                        //if (strTipCliente == "1") { strCadena = strNombre.Trim() + " -> V " + strVentanilla.Trim(); }
                        //if (strTipCliente == "0") { strCadena = "Nº " + strNumeroTicket.Trim() + " -> V " + strVentanilla.Trim(); }
                    
                        strCadena = "Nº " + strNumeroTicket.Trim() + " -> V " + strVentanilla.Trim(); 
                    
                        StrEnPantalla[intFila, 6] = strCadena;                        // TIPO DE CLIENTE

                        //------------------------------------------------
                        //Fecha: 15/01/2018
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Inicializa el contador de parpadeo
                        //------------------------------------------------
                        if (intFila == 0) { IntNumParpadeadas = 0; LblEtiqueta1.Text = StrEnPantalla[0, 6]; }
                        if (intFila == 1) { IntNumParpadeadas2 = 0; LblEtiqueta2.Text = StrEnPantalla[1, 6]; }
                        if (intFila == 2) { IntNumParpadeadas3 = 0; LblEtiqueta3.Text = StrEnPantalla[2, 6]; }
                        if (intFila == 3) { IntNumParpadeadas4 = 0; LblEtiqueta4.Text = StrEnPantalla[3, 6]; }
                        if (intFila == 4) { IntNumParpadeadas5 = 0; LblEtiqueta5.Text = StrEnPantalla[4, 6]; }
                        if (intFila == 5) { IntNumParpadeadas6 = 0; LblEtiqueta6.Text = StrEnPantalla[5, 6]; }
                        //------------------------------------------------

                        break;
                    }
            }
            if (bExisteCasillaVacia == false)
            {             
                //-----------------------------------------------------------
                //Fecha: 10/01/2018
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Reemplazar el menor de las cuatro casillas
                //-----------------------------------------------------------
                string strNroMin = "0";
                int intNroMin = 99999;
                int intIndice = 0;

                for (int i = 0; i <= 5; i++)
                {
                    strNroMin = StrEnPantalla[i, 1];
                    if (strNroMin.Trim() == "")
                    { strNroMin = "0"; }
                    if (Convert.ToInt32(strNroMin) < intNroMin)
                    {                        
                        intNroMin = Convert.ToInt32(strNroMin);
                        intIndice = i;                        
                    }
                }

                StrEnPantalla[intIndice, 0] = strIdTicket;          // ID DEL TICKET
                StrEnPantalla[intIndice, 1] = strNumeroTicket;      // NUMERO DE TICKET
                StrEnPantalla[intIndice, 2] = strNombre;            // NOMBRE DEL CLIENTE
                StrEnPantalla[intIndice, 3] = strVentanilla;        // NUMERO DE VENTANILLA
                StrEnPantalla[intIndice, 4] = strTipCliente;        // TIPO DE CLIENTE
                StrEnPantalla[intIndice, 5] = strParpadea;          // PARPADEA
                strCadena = "Nº " + strNumeroTicket.Trim() + " -> V " + strVentanilla.Trim();
                StrEnPantalla[intIndice, 6] = strCadena;            // TIPO DE CLIENTE
                //------------------------------------------------
                //Fecha: 15/01/2018
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Inicializa el contador de parpadeo
                //------------------------------------------------
                if (intIndice == 0) { IntNumParpadeadas = 0; LblEtiqueta1.Text = StrEnPantalla[0, 6]; }
                if (intIndice == 1) { IntNumParpadeadas2 = 0; LblEtiqueta2.Text = StrEnPantalla[1, 6]; }
                if (intIndice == 2) { IntNumParpadeadas3 = 0; LblEtiqueta3.Text = StrEnPantalla[2, 6]; }
                if (intIndice == 3) { IntNumParpadeadas4 = 0; LblEtiqueta4.Text = StrEnPantalla[3, 6]; }
                if (intIndice == 4) { IntNumParpadeadas5 = 0; LblEtiqueta5.Text = StrEnPantalla[4, 6]; }
                if (intIndice == 5) { IntNumParpadeadas6 = 0; LblEtiqueta6.Text = StrEnPantalla[5, 6]; }
                //-----------------------------------------------------------


            }
        }

        //private int ArrayNumElementos()
        //{
        //    int intNumElementos = 0;
        //    int intFila = 0;

        //    for (intFila = 0; intFila <= 3; intFila++)
        //    {
        //        if (StrEnPantalla[intFila, 0] != null)
        //        {
        //            intNumElementos = intNumElementos + 1;
        //        }
        //    }

        //    return intNumElementos;
        //}

        private void timer3_Tick(object sender, EventArgs e)
        {
            LblEtiqueta1.Text = StrEnPantalla[0, 6]; if (StrEnPantalla[0, 5] == "1") { Parpadear(1); StrEnPantalla[0, 5] = "0"; }
            LblEtiqueta2.Text = StrEnPantalla[1, 6]; if (StrEnPantalla[1, 5] == "1") { Parpadear(2); StrEnPantalla[1, 5] = "0"; }
            LblEtiqueta3.Text = StrEnPantalla[2, 6]; if (StrEnPantalla[2, 5] == "1") { Parpadear(3); StrEnPantalla[2, 5] = "0"; }
            LblEtiqueta4.Text = StrEnPantalla[3, 6]; if (StrEnPantalla[3, 5] == "1") { Parpadear(4); StrEnPantalla[3, 5] = "0"; }
            LblEtiqueta5.Text = StrEnPantalla[4, 6]; if (StrEnPantalla[4, 5] == "1") { Parpadear(5); StrEnPantalla[4, 5] = "0"; }
            LblEtiqueta6.Text = StrEnPantalla[5, 6]; if (StrEnPantalla[5, 5] == "1") { Parpadear(6); StrEnPantalla[5, 5] = "0"; }
            Application.DoEvents();
        }

        private void FrmDisplay2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                timer1.Stop();
                timer2.Stop();
                timer3.Stop();
                timer4.Stop();
                timer5.Stop();
                timer6.Stop();
                timer7.Stop();
                timer8.Stop();   
                axWindowsMediaPlayer1.close();
                this.Hide();
                this.Close();
            }
        }

        //***************************************************************************************************
        private void CargarVideos()
        {
            DataTable DtVideos = new DataTable();
            DataSet dsLeerFicheroXML = new DataSet();
            string strRutaArchivo = @"" + Program.vRutaVideos + "\\" + "Reproduce.wpl";
            if (MiFun.ArchivoExiste(strRutaArchivo) == false)
            {
                MessageBox.Show("No se encontraron videos para reproducir ");
                return;
            }

            dsLeerFicheroXML.Clear();
            dsLeerFicheroXML.ReadXml(Program.strArchivo);
            DtVideos = dsLeerFicheroXML.Tables[8];   // TABLA VIDEOS

            if (DtVideos.Rows.Count != 0)
            {
                string strRutaVideo = string.Empty;

                strRutaVideo = MiFun.IniLeer("PREFERENCIAS", "RUTAVIDEO", Environment.CurrentDirectory + "\\" + Program.strIniLocal);

                axWindowsMediaPlayer1.uiMode = "none";                                 // INDICA QUE SE OCULTARAN LOS CONTROLES DEL CONTROL
                axWindowsMediaPlayer1.settings.setMode("loop", true);                  // INDICA QUE SE REPETIRA INDEFINIDAMENTE EL VIDEO
                axWindowsMediaPlayer1.URL = @strRutaVideo + "\\" + "Reproduce.wpl";    // REPRODUCE LA LISTA DE REPRODUCCION                                
            }
        }

        //private int SeEstaLLamando(string StrTicketId)
        //{
        //    int IntNumEtiqueta = 0;
        //    if (LblEtiqueta1.Text == StrTicketId) { IntNumEtiqueta = 1; }
        //    if (LblEtiqueta2.Text == StrTicketId) { IntNumEtiqueta = 2; }
        //    if (LblEtiqueta3.Text == StrTicketId) { IntNumEtiqueta = 3; }
        //    if (LblEtiqueta4.Text == StrTicketId) { IntNumEtiqueta = 4; }

        //    return IntNumEtiqueta;
        //}

        private void Parpadear(int IntNumEtiquet)
        {
            ReproducirWAV();
            //IntNumeroEtiqueta = IntNumEtiquet;
            if (IntNumEtiquet == 1) { timer2.Interval = 500; timer2.Start(); }
            if (IntNumEtiquet == 2) { timer4.Interval = 500; timer4.Start(); }
            if (IntNumEtiquet == 3) { timer5.Interval = 500; timer5.Start(); }
            if (IntNumEtiquet == 4) { timer6.Interval = 500; timer6.Start(); }
            if (IntNumEtiquet == 5) { timer7.Interval = 500; timer7.Start(); }
            if (IntNumEtiquet == 6) { timer8.Interval = 500; timer8.Start(); }
        }

        //private void LimpiarRotulos()
        //{
        //    LblEtiqueta1.Text = "";
        //    LblEtiqueta2.Text = "";
        //    LblEtiqueta3.Text = "";
        //    LblEtiqueta4.Text = "";
        //}

        private void FrmDisplay2_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void vlcPlayerControl1_Load(object sender, EventArgs e)
        {
        }

        private void axShockwaveFlash1_Enter(object sender, EventArgs e)
        {
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (IntNumParpadeadas2 <= 6)
            {
                IntNumParpadeadas2 = IntNumParpadeadas2 + 1;
                LblEtiqueta2.ForeColor = Color.FromArgb(255, 0, 0); // Color Rojo
                LblEtiqueta2.Visible = !LblEtiqueta2.Visible;                
            }
            else
            {
                IntNumParpadeadas2 = 0;
                LblEtiqueta2.ForeColor = Color.FromArgb(0, 255, 0); // Color Verde
                LblEtiqueta2.Visible = true;                
                timer4.Stop();
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            if (IntNumParpadeadas3 <= 6)
            {
                IntNumParpadeadas3 = IntNumParpadeadas3 + 1;
                LblEtiqueta3.ForeColor = Color.FromArgb(255, 0, 0); // Color Rojo
                LblEtiqueta3.Visible = !LblEtiqueta3.Visible; 
            }
            else
            {
                IntNumParpadeadas3 = 0;
                LblEtiqueta3.ForeColor = Color.FromArgb(0, 255, 0); // Color Verde
                LblEtiqueta3.Visible = true; 
                timer5.Stop();
            }
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            if (IntNumParpadeadas4 <= 6)
            {
                IntNumParpadeadas4 = IntNumParpadeadas4 + 1;
                LblEtiqueta4.ForeColor = Color.FromArgb(255, 0, 0); // Color Rojo
                LblEtiqueta4.Visible = !LblEtiqueta4.Visible; 
            }
            else
            {
                IntNumParpadeadas4 = 0;
                LblEtiqueta4.ForeColor = Color.FromArgb(0, 255, 0); // Color Verde
                LblEtiqueta4.Visible = true; 
                timer6.Stop();
            }
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            if (IntNumParpadeadas5 <= 6)
            {
                IntNumParpadeadas5 = IntNumParpadeadas5 + 1;
                LblEtiqueta5.ForeColor = Color.FromArgb(255, 0, 0); // Color Rojo
                LblEtiqueta5.Visible = !LblEtiqueta5.Visible;
            }
            else
            {
                IntNumParpadeadas5 = 0;
                LblEtiqueta5.ForeColor = Color.FromArgb(0, 255, 0); // Color Verde
                LblEtiqueta5.Visible = true;
                timer7.Stop();
            }
        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            if (IntNumParpadeadas6 <= 6)
            {
                IntNumParpadeadas6 = IntNumParpadeadas6 + 1;
                LblEtiqueta6.ForeColor = Color.FromArgb(255, 0, 0); // Color Rojo
                LblEtiqueta6.Visible = !LblEtiqueta6.Visible;
            }
            else
            {
                IntNumParpadeadas6 = 0;
                LblEtiqueta6.ForeColor = Color.FromArgb(0, 255, 0); // Color Verde
                LblEtiqueta6.Visible = true;
                timer8.Stop();
            }
        }

        private void VerVideoYoutube(string strURL)
        {
            if (strURL.Length > 0)
            {
                if (strURL.IndexOf("https://www.youtube.com/watch?") > -1)
                {

                    strURL = strURL.Replace("https://www.youtube.com/watch?v=", "");

                    string strWatch = "http://www.watchframebyframe.com/watch/yt/" + strURL;

                    try
                    {

                        webBrowser1.Navigate(strWatch);

                    }
                    catch (System.UriFormatException)
                    {
                        return;
                    }
                }
            }
        }


        private void ConfigurarSizeWebBrowser()
        {
            webBrowser1.ScriptErrorsSuppressed = true;

            if (this.Size == new Size(1920, 1080))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 280;
                webBrowser1.Width = (TlpPrincipal.Width * 75 / 100) + 45;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2) + 25;
                gbBrowser.Top = 0;

                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;

                
                
            }
            else if (this.Size == new Size(1680, 1050))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 100;
                webBrowser1.Width = TlpPrincipal.Width - 350;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - (webBrowser1.Width - 50)) / 2);
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;
            }
            else if (this.Size == new Size(1600, 900))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 100;
                webBrowser1.Width = TlpPrincipal.Width - 450;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - (webBrowser1.Width - 50)) / 2);
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;
            }
            else if (this.Size == new Size(1440, 900))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 100;
                webBrowser1.Width = TlpPrincipal.Width - 300;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2);
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;
            }
            else if (this.Size == new Size(1400, 1050))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 150;
                webBrowser1.Width = TlpPrincipal.Width;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2);
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;
            }
            else if (this.Size == new Size(1366, 768) || this.Size == new Size(1360, 768))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 100;
                webBrowser1.Width = TlpPrincipal.Width - 400;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2);
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;
            }
            else if (this.Size == new Size(1280, 1024))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 400;
                webBrowser1.Width = TlpPrincipal.Width;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2);
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;
            }
            else if (this.Size == new Size(1280, 960))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 400;
                webBrowser1.Width = TlpPrincipal.Width - 85;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2);
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;
            }
            else if (this.Size == new Size(1280, 800))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 100;
                webBrowser1.Width = TlpPrincipal.Width - 300;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2);
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;
            }
            else if (this.Size == new Size(1280, 768))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 100;
                webBrowser1.Width = TlpPrincipal.Width - 300;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2);
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;
            }
            else if (this.Size == new Size(1280, 720))
            {
                webBrowser1.Top = -100;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 100;
                webBrowser1.Width = TlpPrincipal.Width - 90;

                webBrowser1.Left = -135;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2);
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 270;
            }
            else if (this.Size == new Size(1152, 864))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 100;
                webBrowser1.Width = TlpPrincipal.Width;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2) + 25;
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;

            }
            else if (this.Size == new Size(1024, 768))
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 100;
                webBrowser1.Width = TlpPrincipal.Width - 100;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2);
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;
            }

            else
            {
                webBrowser1.Top = -30;
                webBrowser1.Height = (TlpPrincipal.Height * 75 / 100) + 280;
                webBrowser1.Width = (TlpPrincipal.Width * 75 / 100) + 45;

                webBrowser1.Left = -25;

                gbBrowser.Left = ((TlpPrincipal.Width - webBrowser1.Width) / 2) + 25;
                gbBrowser.Top = 0;
                gbBrowser.Height = (TlpPrincipal.Height * 75 / 100);
                gbBrowser.Width = webBrowser1.Width - 50;
            }
            Application.DoEvents();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.PreviewKeyDown += new PreviewKeyDownEventHandler(webBrowser1_PreviewKeyDown);            
        }
        private void webBrowser1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            { this.Dispose(); }
            if (e.KeyCode == Keys.F1)
            {
                
                TableLayoutRowStyleCollection styles = TlpPrincipal.RowStyles;
                for (int i = 0; i < styles.Count; i++)
                {
                    styles[i].SizeType = SizeType.Percent;
                    
                    if (i == 0) { styles[i].Height = 100; }
                    else { styles[i].Height = 0; }                    
                }


                gbBrowser.Left = 0;
                gbBrowser.Top = 0;
                gbBrowser.Height = TlpPrincipal.Height;
                gbBrowser.Width = TlpPrincipal.Width;
                
                webBrowser1.Top = -30;
                webBrowser1.Height = TlpPrincipal.Height + 30;
                webBrowser1.Width = TlpPrincipal.Width + 60;

                webBrowser1.Left = -30;
                Application.DoEvents();                                
            }
            if (e.KeyCode == Keys.Back)
            {
                TableLayoutRowStyleCollection styles = TlpPrincipal.RowStyles;
                for (int i = 0; i < styles.Count; i++)
                {
                    styles[i].SizeType = SizeType.Percent;

                    if (i == 0) { styles[i].Height = 75; }
                    else { styles[i].Height = 25; }
                }
                ConfigurarSizeWebBrowser();
            }
        }
    }
}