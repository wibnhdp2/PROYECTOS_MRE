namespace SGAC.WinApp
{
    partial class FrmConfiguracionSistema
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConfiguracionSistema));
            this.CmdAceptar = new System.Windows.Forms.Button();
            this.CmdCancelar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboYoutube = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.CboTamaño = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.CboFuente = new System.Windows.Forms.ComboBox();
            this.CboTicketera = new System.Windows.Forms.ComboBox();
            this.CmdBorrarRutaReporte = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.CmdBusRutRep = new System.Windows.Forms.Button();
            this.TxtRutaRep = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.CmdBorrarRutaVideo = new System.Windows.Forms.Button();
            this.CmdBusRutaVideo = new System.Windows.Forms.Button();
            this.TxtRutaVideo = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.CboTamañoTicket = new System.Windows.Forms.ComboBox();
            this.CmdBorrarImagen = new System.Windows.Forms.Button();
            this.CmdBorrarBD = new System.Windows.Forms.Button();
            this.CmdBusFondo = new System.Windows.Forms.Button();
            this.TxtFondoEscritorio = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtNumLLamadas = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CmdBusData = new System.Windows.Forms.Button();
            this.TxtRutaBD = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.OptNo = new System.Windows.Forms.RadioButton();
            this.OptSi = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CboOficina = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.TxtDominioNombre = new System.Windows.Forms.TextBox();
            this.TxtDominioRuta = new System.Windows.Forms.TextBox();
            this.CboVentanilla = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DgUsuarioVentanilla = new System.Windows.Forms.DataGridView();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ventanilla = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CboUsuario = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgUsuarioVentanilla)).BeginInit();
            this.SuspendLayout();
            // 
            // CmdAceptar
            // 
            this.CmdAceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(94)))), ((int)(((byte)(188)))));
            this.CmdAceptar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdAceptar.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmdAceptar.ForeColor = System.Drawing.Color.White;
            this.CmdAceptar.Location = new System.Drawing.Point(210, 455);
            this.CmdAceptar.Name = "CmdAceptar";
            this.CmdAceptar.Size = new System.Drawing.Size(100, 30);
            this.CmdAceptar.TabIndex = 16;
            this.CmdAceptar.Text = "&Actualizar";
            this.CmdAceptar.UseVisualStyleBackColor = false;
            this.CmdAceptar.Click += new System.EventHandler(this.CmdAceptar_Click);
            // 
            // CmdCancelar
            // 
            this.CmdCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CmdCancelar.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmdCancelar.Location = new System.Drawing.Point(316, 456);
            this.CmdCancelar.Name = "CmdCancelar";
            this.CmdCancelar.Size = new System.Drawing.Size(100, 30);
            this.CmdCancelar.TabIndex = 17;
            this.CmdCancelar.Text = "&Cerrar";
            this.CmdCancelar.UseVisualStyleBackColor = true;
            this.CmdCancelar.Click += new System.EventHandler(this.CmdCancelar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(200)))), ((int)(((byte)(240)))));
            this.groupBox1.Controls.Add(this.cboYoutube);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.CboTamaño);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.CboFuente);
            this.groupBox1.Controls.Add(this.CboTicketera);
            this.groupBox1.Controls.Add(this.CmdBorrarRutaReporte);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.CmdBusRutRep);
            this.groupBox1.Controls.Add(this.TxtRutaRep);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.CmdBorrarRutaVideo);
            this.groupBox1.Controls.Add(this.CmdBusRutaVideo);
            this.groupBox1.Controls.Add(this.TxtRutaVideo);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.CboTamañoTicket);
            this.groupBox1.Controls.Add(this.CmdBorrarImagen);
            this.groupBox1.Controls.Add(this.CmdBorrarBD);
            this.groupBox1.Controls.Add(this.CmdBusFondo);
            this.groupBox1.Controls.Add(this.TxtFondoEscritorio);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.TxtNumLLamadas);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.CmdBusData);
            this.groupBox1.Controls.Add(this.TxtRutaBD);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.OptNo);
            this.groupBox1.Controls.Add(this.OptSi);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.CboOficina);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(20, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(544, 273);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Parametros del Sistema ";
            // 
            // cboYoutube
            // 
            this.cboYoutube.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.cboYoutube.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboYoutube.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYoutube.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboYoutube.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboYoutube.FormattingEnabled = true;
            this.cboYoutube.Location = new System.Drawing.Point(132, 241);
            this.cboYoutube.Name = "cboYoutube";
            this.cboYoutube.Size = new System.Drawing.Size(399, 21);
            this.cboYoutube.TabIndex = 46;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Location = new System.Drawing.Point(11, 247);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(71, 15);
            this.label17.TabIndex = 45;
            this.label17.Text = "Ver Youtube";
            // 
            // CboTamaño
            // 
            this.CboTamaño.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.CboTamaño.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CboTamaño.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboTamaño.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CboTamaño.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboTamaño.FormattingEnabled = true;
            this.CboTamaño.Location = new System.Drawing.Point(459, 190);
            this.CboTamaño.Name = "CboTamaño";
            this.CboTamaño.Size = new System.Drawing.Size(71, 21);
            this.CboTamaño.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Arial", 9F);
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(397, 194);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 15);
            this.label14.TabIndex = 44;
            this.label14.Text = "Tamaño";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(11, 194);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 15);
            this.label13.TabIndex = 43;
            this.label13.Text = "Fuente Display";
            // 
            // CboFuente
            // 
            this.CboFuente.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.CboFuente.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CboFuente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboFuente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CboFuente.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboFuente.FormattingEnabled = true;
            this.CboFuente.Location = new System.Drawing.Point(132, 190);
            this.CboFuente.Name = "CboFuente";
            this.CboFuente.Size = new System.Drawing.Size(241, 21);
            this.CboFuente.TabIndex = 9;
            // 
            // CboTicketera
            // 
            this.CboTicketera.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.CboTicketera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CboTicketera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboTicketera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CboTicketera.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboTicketera.FormattingEnabled = true;
            this.CboTicketera.Location = new System.Drawing.Point(132, 215);
            this.CboTicketera.Name = "CboTicketera";
            this.CboTicketera.Size = new System.Drawing.Size(399, 21);
            this.CboTicketera.TabIndex = 11;
            // 
            // CmdBorrarRutaReporte
            // 
            this.CmdBorrarRutaReporte.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdBorrarRutaReporte.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmdBorrarRutaReporte.ForeColor = System.Drawing.Color.Black;
            this.CmdBorrarRutaReporte.Image = global::SGAC.WinApp.Properties.Resources.img_16_clear;
            this.CmdBorrarRutaReporte.Location = new System.Drawing.Point(491, 91);
            this.CmdBorrarRutaReporte.Name = "CmdBorrarRutaReporte";
            this.CmdBorrarRutaReporte.Size = new System.Drawing.Size(43, 20);
            this.CmdBorrarRutaReporte.TabIndex = 41;
            this.toolTip1.SetToolTip(this.CmdBorrarRutaReporte, "Borrar");
            this.CmdBorrarRutaReporte.UseVisualStyleBackColor = true;
            this.CmdBorrarRutaReporte.Click += new System.EventHandler(this.CmdBorrarRutaReporte_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(11, 218);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 15);
            this.label7.TabIndex = 28;
            this.label7.Text = "Ticketera";
            // 
            // CmdBusRutRep
            // 
            this.CmdBusRutRep.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdBusRutRep.ForeColor = System.Drawing.Color.Black;
            this.CmdBusRutRep.Image = global::SGAC.WinApp.Properties.Resources.lupa4;
            this.CmdBusRutRep.Location = new System.Drawing.Point(468, 93);
            this.CmdBusRutRep.Name = "CmdBusRutRep";
            this.CmdBusRutRep.Size = new System.Drawing.Size(18, 18);
            this.CmdBusRutRep.TabIndex = 40;
            this.toolTip1.SetToolTip(this.CmdBusRutRep, "Seleccionar la carpeta: Reportes");
            this.CmdBusRutRep.UseVisualStyleBackColor = true;
            this.CmdBusRutRep.Click += new System.EventHandler(this.CmdBusRutRep_Click);
            // 
            // TxtRutaRep
            // 
            this.TxtRutaRep.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.TxtRutaRep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtRutaRep.Location = new System.Drawing.Point(132, 92);
            this.TxtRutaRep.Name = "TxtRutaRep";
            this.TxtRutaRep.ReadOnly = true;
            this.TxtRutaRep.Size = new System.Drawing.Size(356, 20);
            this.TxtRutaRep.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(11, 95);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(87, 15);
            this.label12.TabIndex = 39;
            this.label12.Text = "Ruta Reportes";
            // 
            // CmdBorrarRutaVideo
            // 
            this.CmdBorrarRutaVideo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdBorrarRutaVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmdBorrarRutaVideo.ForeColor = System.Drawing.Color.Black;
            this.CmdBorrarRutaVideo.Image = global::SGAC.WinApp.Properties.Resources.img_16_clear;
            this.CmdBorrarRutaVideo.Location = new System.Drawing.Point(491, 69);
            this.CmdBorrarRutaVideo.Name = "CmdBorrarRutaVideo";
            this.CmdBorrarRutaVideo.Size = new System.Drawing.Size(43, 20);
            this.CmdBorrarRutaVideo.TabIndex = 37;
            this.toolTip1.SetToolTip(this.CmdBorrarRutaVideo, "Borrar");
            this.CmdBorrarRutaVideo.UseVisualStyleBackColor = true;
            this.CmdBorrarRutaVideo.Click += new System.EventHandler(this.CmdBorrarRutaVideo_Click);
            // 
            // CmdBusRutaVideo
            // 
            this.CmdBusRutaVideo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdBusRutaVideo.ForeColor = System.Drawing.Color.Black;
            this.CmdBusRutaVideo.Image = global::SGAC.WinApp.Properties.Resources.lupa4;
            this.CmdBusRutaVideo.Location = new System.Drawing.Point(468, 71);
            this.CmdBusRutaVideo.Name = "CmdBusRutaVideo";
            this.CmdBusRutaVideo.Size = new System.Drawing.Size(18, 18);
            this.CmdBusRutaVideo.TabIndex = 36;
            this.toolTip1.SetToolTip(this.CmdBusRutaVideo, "Seleccionar la carpeta: Videos");
            this.CmdBusRutaVideo.UseVisualStyleBackColor = true;
            this.CmdBusRutaVideo.Click += new System.EventHandler(this.CmdBusRutaVideo_Click);
            // 
            // TxtRutaVideo
            // 
            this.TxtRutaVideo.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.TxtRutaVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtRutaVideo.Location = new System.Drawing.Point(132, 70);
            this.TxtRutaVideo.Name = "TxtRutaVideo";
            this.TxtRutaVideo.ReadOnly = true;
            this.TxtRutaVideo.Size = new System.Drawing.Size(356, 20);
            this.TxtRutaVideo.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(11, 73);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 15);
            this.label11.TabIndex = 35;
            this.label11.Text = "Ruta Video";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(11, 144);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 15);
            this.label9.TabIndex = 33;
            this.label9.Text = "Tamaño Ticket";
            // 
            // CboTamañoTicket
            // 
            this.CboTamañoTicket.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.CboTamañoTicket.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CboTamañoTicket.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboTamañoTicket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CboTamañoTicket.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboTamañoTicket.FormattingEnabled = true;
            this.CboTamañoTicket.Location = new System.Drawing.Point(132, 141);
            this.CboTamañoTicket.Name = "CboTamañoTicket";
            this.CboTamañoTicket.Size = new System.Drawing.Size(241, 21);
            this.CboTamañoTicket.TabIndex = 5;
            // 
            // CmdBorrarImagen
            // 
            this.CmdBorrarImagen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdBorrarImagen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmdBorrarImagen.ForeColor = System.Drawing.Color.Black;
            this.CmdBorrarImagen.Image = global::SGAC.WinApp.Properties.Resources.img_16_clear;
            this.CmdBorrarImagen.Location = new System.Drawing.Point(491, 46);
            this.CmdBorrarImagen.Name = "CmdBorrarImagen";
            this.CmdBorrarImagen.Size = new System.Drawing.Size(43, 20);
            this.CmdBorrarImagen.TabIndex = 31;
            this.toolTip1.SetToolTip(this.CmdBorrarImagen, "Borrar");
            this.CmdBorrarImagen.UseVisualStyleBackColor = true;
            this.CmdBorrarImagen.Click += new System.EventHandler(this.CmdBorrarImagen_Click);
            // 
            // CmdBorrarBD
            // 
            this.CmdBorrarBD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdBorrarBD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmdBorrarBD.ForeColor = System.Drawing.Color.Black;
            this.CmdBorrarBD.Image = global::SGAC.WinApp.Properties.Resources.img_16_clear;
            this.CmdBorrarBD.Location = new System.Drawing.Point(491, 23);
            this.CmdBorrarBD.Name = "CmdBorrarBD";
            this.CmdBorrarBD.Size = new System.Drawing.Size(43, 20);
            this.CmdBorrarBD.TabIndex = 30;
            this.toolTip1.SetToolTip(this.CmdBorrarBD, "Borrar");
            this.CmdBorrarBD.UseVisualStyleBackColor = true;
            this.CmdBorrarBD.Click += new System.EventHandler(this.CmdBorrarBD_Click);
            // 
            // CmdBusFondo
            // 
            this.CmdBusFondo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdBusFondo.ForeColor = System.Drawing.Color.Black;
            this.CmdBusFondo.Image = global::SGAC.WinApp.Properties.Resources.lupa4;
            this.CmdBusFondo.Location = new System.Drawing.Point(468, 48);
            this.CmdBusFondo.Name = "CmdBusFondo";
            this.CmdBusFondo.Size = new System.Drawing.Size(18, 18);
            this.CmdBusFondo.TabIndex = 29;
            this.toolTip1.SetToolTip(this.CmdBusFondo, "Seleccionar la carpeta: Fondo");
            this.CmdBusFondo.UseVisualStyleBackColor = true;
            this.CmdBusFondo.Click += new System.EventHandler(this.CmdBusFondo_Click);
            // 
            // TxtFondoEscritorio
            // 
            this.TxtFondoEscritorio.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.TxtFondoEscritorio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtFondoEscritorio.Location = new System.Drawing.Point(132, 47);
            this.TxtFondoEscritorio.Name = "TxtFondoEscritorio";
            this.TxtFondoEscritorio.ReadOnly = true;
            this.TxtFondoEscritorio.Size = new System.Drawing.Size(356, 20);
            this.TxtFondoEscritorio.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(11, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 15);
            this.label8.TabIndex = 28;
            this.label8.Text = "Fondo Pantalla";
            // 
            // TxtNumLLamadas
            // 
            this.TxtNumLLamadas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtNumLLamadas.Location = new System.Drawing.Point(132, 166);
            this.TxtNumLLamadas.MaxLength = 1;
            this.TxtNumLLamadas.Name = "TxtNumLLamadas";
            this.TxtNumLLamadas.Size = new System.Drawing.Size(54, 20);
            this.TxtNumLLamadas.TabIndex = 6;
            this.TxtNumLLamadas.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtNumLLamadas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtNumLLamadas_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(11, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 15);
            this.label6.TabIndex = 26;
            this.label6.Text = "Nº Llamadas x Ticket";
            // 
            // CmdBusData
            // 
            this.CmdBusData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdBusData.ForeColor = System.Drawing.Color.Black;
            this.CmdBusData.Image = global::SGAC.WinApp.Properties.Resources.lupa4;
            this.CmdBusData.Location = new System.Drawing.Point(468, 25);
            this.CmdBusData.Name = "CmdBusData";
            this.CmdBusData.Size = new System.Drawing.Size(18, 18);
            this.CmdBusData.TabIndex = 24;
            this.toolTip1.SetToolTip(this.CmdBusData, "Seleccionar la carpeta: DataColas");
            this.CmdBusData.UseVisualStyleBackColor = true;
            this.CmdBusData.Click += new System.EventHandler(this.CmdBusData_Click);
            // 
            // TxtRutaBD
            // 
            this.TxtRutaBD.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.TxtRutaBD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtRutaBD.Location = new System.Drawing.Point(132, 24);
            this.TxtRutaBD.Name = "TxtRutaBD";
            this.TxtRutaBD.ReadOnly = true;
            this.TxtRutaBD.Size = new System.Drawing.Size(356, 20);
            this.TxtRutaBD.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(11, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 15);
            this.label5.TabIndex = 21;
            this.label5.Text = "Ruta BD";
            // 
            // OptNo
            // 
            this.OptNo.AutoSize = true;
            this.OptNo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OptNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OptNo.ForeColor = System.Drawing.Color.Black;
            this.OptNo.Location = new System.Drawing.Point(486, 166);
            this.OptNo.Name = "OptNo";
            this.OptNo.Size = new System.Drawing.Size(41, 19);
            this.OptNo.TabIndex = 8;
            this.OptNo.TabStop = true;
            this.OptNo.Text = "&No";
            this.OptNo.UseVisualStyleBackColor = true;
            // 
            // OptSi
            // 
            this.OptSi.AutoSize = true;
            this.OptSi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OptSi.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OptSi.ForeColor = System.Drawing.Color.Black;
            this.OptSi.Location = new System.Drawing.Point(438, 166);
            this.OptSi.Name = "OptSi";
            this.OptSi.Size = new System.Drawing.Size(36, 19);
            this.OptSi.TabIndex = 7;
            this.OptSi.TabStop = true;
            this.OptSi.Text = "&Si";
            this.OptSi.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(338, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "Visualizar Ticket";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(11, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Oficina Consular";
            // 
            // CboOficina
            // 
            this.CboOficina.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.CboOficina.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CboOficina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboOficina.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CboOficina.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboOficina.FormattingEnabled = true;
            this.CboOficina.Location = new System.Drawing.Point(132, 116);
            this.CboOficina.Name = "CboOficina";
            this.CboOficina.Size = new System.Drawing.Size(399, 21);
            this.CboOficina.TabIndex = 4;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(642, 52);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 13);
            this.label15.TabIndex = 48;
            this.label15.Text = "Dominio Ruta";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(642, 75);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 13);
            this.label16.TabIndex = 47;
            this.label16.Text = "Dominio Nombre";
            // 
            // TxtDominioNombre
            // 
            this.TxtDominioNombre.Location = new System.Drawing.Point(757, 66);
            this.TxtDominioNombre.Name = "TxtDominioNombre";
            this.TxtDominioNombre.Size = new System.Drawing.Size(148, 20);
            this.TxtDominioNombre.TabIndex = 13;
            // 
            // TxtDominioRuta
            // 
            this.TxtDominioRuta.Location = new System.Drawing.Point(757, 43);
            this.TxtDominioRuta.Name = "TxtDominioRuta";
            this.TxtDominioRuta.Size = new System.Drawing.Size(183, 20);
            this.TxtDominioRuta.TabIndex = 12;
            // 
            // CboVentanilla
            // 
            this.CboVentanilla.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboVentanilla.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboVentanilla.FormattingEnabled = true;
            this.CboVentanilla.Location = new System.Drawing.Point(452, 459);
            this.CboVentanilla.Name = "CboVentanilla";
            this.CboVentanilla.Size = new System.Drawing.Size(69, 21);
            this.CboVentanilla.TabIndex = 21;
            this.CboVentanilla.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(253, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "Asignar Usuarios a Ventanillas";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(200)))), ((int)(((byte)(240)))));
            this.groupBox2.Controls.Add(this.DgUsuarioVentanilla);
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(20, 293);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(544, 152);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Parametros del Cliente ";
            // 
            // DgUsuarioVentanilla
            // 
            this.DgUsuarioVentanilla.BackgroundColor = System.Drawing.Color.Silver;
            this.DgUsuarioVentanilla.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgUsuarioVentanilla.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Ventanilla,
            this.Column1});
            this.DgUsuarioVentanilla.GridColor = System.Drawing.Color.Gray;
            this.DgUsuarioVentanilla.Location = new System.Drawing.Point(250, 35);
            this.DgUsuarioVentanilla.Name = "DgUsuarioVentanilla";
            this.DgUsuarioVentanilla.RowHeadersWidth = 15;
            this.DgUsuarioVentanilla.Size = new System.Drawing.Size(288, 108);
            this.DgUsuarioVentanilla.TabIndex = 15;
            this.DgUsuarioVentanilla.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgUsuarioVentanilla_CellEndEdit);
            this.DgUsuarioVentanilla.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DgUsuarioVentanilla_EditingControlShowing);
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "veus_vUsuario";
            this.Column2.HeaderText = "Usuario";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 110;
            // 
            // Ventanilla
            // 
            this.Ventanilla.DataPropertyName = "veus_iVentanillaId";
            this.Ventanilla.HeaderText = "Ventanilla";
            this.Ventanilla.Name = "Ventanilla";
            this.Ventanilla.Width = 145;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "veus_iUsuarioId";
            this.Column1.HeaderText = "IdUsuario";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // listBox1
            // 
            this.listBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 35);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(232, 108);
            this.listBox1.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(17, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 15);
            this.label2.TabIndex = 30;
            this.label2.Text = "Impresora de Reportes";
            // 
            // CboUsuario
            // 
            this.CboUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboUsuario.FormattingEnabled = true;
            this.CboUsuario.Location = new System.Drawing.Point(118, 459);
            this.CboUsuario.Name = "CboUsuario";
            this.CboUsuario.Size = new System.Drawing.Size(70, 21);
            this.CboUsuario.TabIndex = 20;
            this.CboUsuario.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(10, 461);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 15);
            this.label10.TabIndex = 32;
            this.label10.Text = "Usuario Ventanilla";
            this.label10.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FrmConfiguracionSistema
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(200)))), ((int)(((byte)(240)))));
            this.CancelButton = this.CmdCancelar;
            this.ClientSize = new System.Drawing.Size(582, 501);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.TxtDominioNombre);
            this.Controls.Add(this.CboUsuario);
            this.Controls.Add(this.TxtDominioRuta);
            this.Controls.Add(this.CmdCancelar);
            this.Controls.Add(this.CmdAceptar);
            this.Controls.Add(this.CboVentanilla);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(598, 540);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(598, 540);
            this.Name = "FrmConfiguracionSistema";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmConfigurar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConfigurar_FormClosing);
            this.Load += new System.EventHandler(this.FrmConfigurar_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgUsuarioVentanilla)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CmdAceptar;
        private System.Windows.Forms.Button CmdCancelar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox CboVentanilla;
        private System.Windows.Forms.TextBox TxtRutaBD;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton OptNo;
        private System.Windows.Forms.RadioButton OptSi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CboOficina;
        private System.Windows.Forms.Button CmdBusData;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox TxtNumLLamadas;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button CmdBusFondo;
        private System.Windows.Forms.TextBox TxtFondoEscritorio;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox CboTicketera;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CmdBorrarImagen;
        private System.Windows.Forms.Button CmdBorrarBD;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox CboTamañoTicket;
        private System.Windows.Forms.ComboBox CboUsuario;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button CmdBorrarRutaVideo;
        private System.Windows.Forms.Button CmdBusRutaVideo;
        private System.Windows.Forms.TextBox TxtRutaVideo;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button CmdBorrarRutaReporte;
        private System.Windows.Forms.Button CmdBusRutRep;
        private System.Windows.Forms.TextBox TxtRutaRep;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox CboTamaño;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox CboFuente;
        private System.Windows.Forms.DataGridView DgUsuarioVentanilla;
        private System.Windows.Forms.TextBox TxtDominioNombre;
        private System.Windows.Forms.TextBox TxtDominioRuta;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewComboBoxColumn Ventanilla;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cboYoutube;
        private System.Windows.Forms.Label label17;
    }
}