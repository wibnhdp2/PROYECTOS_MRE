namespace SGAC.WinApp
{
    partial class FrmMenuServicio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMenuServicio));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.TlyTitulo = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.LblNomConsulado = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TlyPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CmdHome = new System.Windows.Forms.Button();
            this.CmdPrev = new System.Windows.Forms.Button();
            this.LblTituloServicio = new System.Windows.Forms.Label();
            this.PnlSubServicio = new System.Windows.Forms.Panel();
            this.TlySubServicio = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LblTipoAtencion = new System.Windows.Forms.Label();
            this.LblNombre = new System.Windows.Forms.Label();
            this.LblMensajes = new System.Windows.Forms.Label();
            this.TlyTituloPie = new System.Windows.Forms.TableLayoutPanel();
            this.TituloPie = new System.Windows.Forms.Label();
            this.TlyTitulo.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.TlyPrincipal.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PnlSubServicio.SuspendLayout();
            this.TlyTituloPie.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TlyTitulo
            // 
            this.TlyTitulo.BackColor = System.Drawing.Color.DarkRed;
            this.TlyTitulo.ColumnCount = 2;
            this.TlyTitulo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            this.TlyTitulo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlyTitulo.Controls.Add(this.panel5, 0, 0);
            this.TlyTitulo.Controls.Add(this.pictureBox1, 0, 0);
            this.TlyTitulo.Location = new System.Drawing.Point(-1, 0);
            this.TlyTitulo.Name = "TlyTitulo";
            this.TlyTitulo.RowCount = 1;
            this.TlyTitulo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TlyTitulo.Size = new System.Drawing.Size(825, 90);
            this.TlyTitulo.TabIndex = 28;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.LblNomConsulado);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(122, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(700, 84);
            this.panel5.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(17, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(343, 18);
            this.label7.TabIndex = 1;
            this.label7.Text = "Ministerio de Relaciones Exteriores del Perú";
            // 
            // LblNomConsulado
            // 
            this.LblNomConsulado.AutoSize = true;
            this.LblNomConsulado.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblNomConsulado.ForeColor = System.Drawing.Color.White;
            this.LblNomConsulado.Location = new System.Drawing.Point(14, 19);
            this.LblNomConsulado.Name = "LblNomConsulado";
            this.LblNomConsulado.Size = new System.Drawing.Size(203, 25);
            this.LblNomConsulado.TabIndex = 0;
            this.LblNomConsulado.Text = "LblNomConsulado";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(113, 84);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // TlyPrincipal
            // 
            this.TlyPrincipal.BackColor = System.Drawing.Color.Transparent;
            this.TlyPrincipal.ColumnCount = 3;
            this.TlyPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlyPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 699F));
            this.TlyPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlyPrincipal.Controls.Add(this.panel1, 1, 1);
            this.TlyPrincipal.Controls.Add(this.LblMensajes, 1, 2);
            this.TlyPrincipal.Location = new System.Drawing.Point(12, 93);
            this.TlyPrincipal.Name = "TlyPrincipal";
            this.TlyPrincipal.RowCount = 3;
            this.TlyPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.TlyPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 559F));
            this.TlyPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.TlyPrincipal.Size = new System.Drawing.Size(772, 592);
            this.TlyPrincipal.TabIndex = 30;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Brown;
            this.panel1.Controls.Add(this.CmdHome);
            this.panel1.Controls.Add(this.CmdPrev);
            this.panel1.Controls.Add(this.LblTituloServicio);
            this.panel1.Controls.Add(this.PnlSubServicio);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.LblTipoAtencion);
            this.panel1.Controls.Add(this.LblNombre);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(39, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(693, 553);
            this.panel1.TabIndex = 0;
            // 
            // CmdHome
            // 
            this.CmdHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdHome.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CmdHome.Image = ((System.Drawing.Image)(resources.GetObject("CmdHome.Image")));
            this.CmdHome.Location = new System.Drawing.Point(601, 473);
            this.CmdHome.Name = "CmdHome";
            this.CmdHome.Size = new System.Drawing.Size(81, 75);
            this.CmdHome.TabIndex = 35;
            this.CmdHome.UseVisualStyleBackColor = true;
            this.CmdHome.Visible = false;
            this.CmdHome.Click += new System.EventHandler(this.CmdHome_Click);
            // 
            // CmdPrev
            // 
            this.CmdPrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdPrev.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CmdPrev.Image = ((System.Drawing.Image)(resources.GetObject("CmdPrev.Image")));
            this.CmdPrev.Location = new System.Drawing.Point(11, 473);
            this.CmdPrev.Name = "CmdPrev";
            this.CmdPrev.Size = new System.Drawing.Size(81, 75);
            this.CmdPrev.TabIndex = 34;
            this.CmdPrev.UseVisualStyleBackColor = true;
            this.CmdPrev.Click += new System.EventHandler(this.CmdPrev_Click);
            // 
            // LblTituloServicio
            // 
            this.LblTituloServicio.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTituloServicio.ForeColor = System.Drawing.Color.White;
            this.LblTituloServicio.Location = new System.Drawing.Point(31, 4);
            this.LblTituloServicio.Name = "LblTituloServicio";
            this.LblTituloServicio.Size = new System.Drawing.Size(626, 29);
            this.LblTituloServicio.TabIndex = 33;
            this.LblTituloServicio.Text = "LblTituloServicio";
            this.LblTituloServicio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LblTituloServicio.Visible = false;
            // 
            // PnlSubServicio
            // 
            this.PnlSubServicio.Controls.Add(this.TlySubServicio);
            this.PnlSubServicio.Location = new System.Drawing.Point(192, 54);
            this.PnlSubServicio.Name = "PnlSubServicio";
            this.PnlSubServicio.Size = new System.Drawing.Size(302, 417);
            this.PnlSubServicio.TabIndex = 32;
            this.PnlSubServicio.Visible = false;
            // 
            // TlySubServicio
            // 
            this.TlySubServicio.ColumnCount = 1;
            this.TlySubServicio.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlySubServicio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlySubServicio.Location = new System.Drawing.Point(0, 0);
            this.TlySubServicio.Name = "TlySubServicio";
            this.TlySubServicio.RowCount = 6;
            this.TlySubServicio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TlySubServicio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TlySubServicio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TlySubServicio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TlySubServicio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TlySubServicio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TlySubServicio.Size = new System.Drawing.Size(302, 417);
            this.TlySubServicio.TabIndex = 31;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(113, 54);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(460, 417);
            this.tableLayoutPanel1.TabIndex = 31;
            // 
            // LblTipoAtencion
            // 
            this.LblTipoAtencion.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTipoAtencion.ForeColor = System.Drawing.Color.White;
            this.LblTipoAtencion.Location = new System.Drawing.Point(5, 522);
            this.LblTipoAtencion.Name = "LblTipoAtencion";
            this.LblTipoAtencion.Size = new System.Drawing.Size(683, 26);
            this.LblTipoAtencion.TabIndex = 16;
            this.LblTipoAtencion.Text = "LblTipoAtencion";
            this.LblTipoAtencion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblNombre
            // 
            this.LblNombre.BackColor = System.Drawing.Color.Brown;
            this.LblNombre.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblNombre.ForeColor = System.Drawing.Color.White;
            this.LblNombre.Location = new System.Drawing.Point(6, 4);
            this.LblNombre.Name = "LblNombre";
            this.LblNombre.Size = new System.Drawing.Size(681, 29);
            this.LblNombre.TabIndex = 30;
            this.LblNombre.Text = "LblNombre";
            this.LblNombre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LblNombre.Click += new System.EventHandler(this.LblNombre_Click);
            // 
            // LblMensajes
            // 
            this.LblMensajes.AutoSize = true;
            this.LblMensajes.BackColor = System.Drawing.Color.Brown;
            this.LblMensajes.Dock = System.Windows.Forms.DockStyle.Top;
            this.LblMensajes.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMensajes.ForeColor = System.Drawing.Color.White;
            this.LblMensajes.Location = new System.Drawing.Point(39, 572);
            this.LblMensajes.Name = "LblMensajes";
            this.LblMensajes.Size = new System.Drawing.Size(693, 14);
            this.LblMensajes.TabIndex = 1;
            this.LblMensajes.Text = "LblMensajes";
            this.LblMensajes.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.LblMensajes.Visible = false;
            // 
            // TlyTituloPie
            // 
            this.TlyTituloPie.BackColor = System.Drawing.Color.DarkRed;
            this.TlyTituloPie.ColumnCount = 1;
            this.TlyTituloPie.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlyTituloPie.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TlyTituloPie.Controls.Add(this.TituloPie, 0, 0);
            this.TlyTituloPie.Location = new System.Drawing.Point(2, 705);
            this.TlyTituloPie.Name = "TlyTituloPie";
            this.TlyTituloPie.RowCount = 1;
            this.TlyTituloPie.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlyTituloPie.Size = new System.Drawing.Size(782, 30);
            this.TlyTituloPie.TabIndex = 32;
            // 
            // TituloPie
            // 
            this.TituloPie.AutoSize = true;
            this.TituloPie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TituloPie.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TituloPie.ForeColor = System.Drawing.Color.White;
            this.TituloPie.Location = new System.Drawing.Point(3, 0);
            this.TituloPie.Name = "TituloPie";
            this.TituloPie.Size = new System.Drawing.Size(776, 30);
            this.TituloPie.TabIndex = 18;
            this.TituloPie.Text = "01/10/2014 08:00:00";
            this.TituloPie.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmMenuServicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 747);
            this.Controls.Add(this.TlyTituloPie);
            this.Controls.Add(this.TlyPrincipal);
            this.Controls.Add(this.TlyTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMenuServicio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Servicios de Atención";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmServicio_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmMenuServicio_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmServicio_KeyDown);
            this.TlyTitulo.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.TlyPrincipal.ResumeLayout(false);
            this.TlyPrincipal.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.PnlSubServicio.ResumeLayout(false);
            this.TlyTituloPie.ResumeLayout(false);
            this.TlyTituloPie.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TableLayoutPanel TlyTitulo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel TlyPrincipal;
        private System.Windows.Forms.Label LblNombre;
        private System.Windows.Forms.Label LblTipoAtencion;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel PnlSubServicio;
        private System.Windows.Forms.Label LblTituloServicio;
        private System.Windows.Forms.TableLayoutPanel TlySubServicio;
        private System.Windows.Forms.TableLayoutPanel TlyTituloPie;
        private System.Windows.Forms.Label TituloPie;
        private System.Windows.Forms.Button CmdHome;
        private System.Windows.Forms.Button CmdPrev;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LblNomConsulado;
        private System.Windows.Forms.Label LblMensajes;
    }
}