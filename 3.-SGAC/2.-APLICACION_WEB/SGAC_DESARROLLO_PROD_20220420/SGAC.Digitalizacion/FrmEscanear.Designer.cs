using System.Configuration;

namespace SGAC.Digitalizacion
{
    partial class frmEscanear
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEscanear));
            this.btnEscanear = new System.Windows.Forms.Button();
            this.Devices = new System.Windows.Forms.ListBox();
            this.picImagen = new System.Windows.Forms.PictureBox();
            this.btnAgregarPagina = new System.Windows.Forms.Button();
            this.btnEliminarPagina = new System.Windows.Forms.Button();
            this.lstPagina = new System.Windows.Forms.ListBox();
            this.btnFinalizar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnAbrirPDF = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picImagen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEscanear
            // 
            this.btnEscanear.BackColor = System.Drawing.Color.DarkRed;
            this.btnEscanear.ForeColor = System.Drawing.Color.Coral;
            this.btnEscanear.Location = new System.Drawing.Point(374, 236);
            this.btnEscanear.Name = "btnEscanear";
            this.btnEscanear.Size = new System.Drawing.Size(173, 32);
            this.btnEscanear.TabIndex = 0;
            this.btnEscanear.Text = "Escanear nuevo documento";
            this.btnEscanear.UseVisualStyleBackColor = false;
            this.btnEscanear.Click += new System.EventHandler(this.btnEscanear_Click);
            // 
            // Devices
            // 
            this.Devices.FormattingEnabled = true;
            this.Devices.Location = new System.Drawing.Point(374, 147);
            this.Devices.Name = "Devices";
            this.Devices.Size = new System.Drawing.Size(173, 69);
            this.Devices.TabIndex = 1;
            // 
            // picImagen
            // 
            this.picImagen.BackColor = System.Drawing.SystemColors.ControlDark;
            this.picImagen.Location = new System.Drawing.Point(27, 112);
            this.picImagen.Name = "picImagen";
            this.picImagen.Size = new System.Drawing.Size(315, 485);
            this.picImagen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picImagen.TabIndex = 2;
            this.picImagen.TabStop = false;
            // 
            // btnAgregarPagina
            // 
            this.btnAgregarPagina.BackColor = System.Drawing.Color.DarkRed;
            this.btnAgregarPagina.ForeColor = System.Drawing.Color.Coral;
            this.btnAgregarPagina.Location = new System.Drawing.Point(374, 301);
            this.btnAgregarPagina.Name = "btnAgregarPagina";
            this.btnAgregarPagina.Size = new System.Drawing.Size(173, 32);
            this.btnAgregarPagina.TabIndex = 3;
            this.btnAgregarPagina.Text = "Agregar Pagina";
            this.btnAgregarPagina.UseVisualStyleBackColor = false;
            this.btnAgregarPagina.Click += new System.EventHandler(this.btnAgregarPagina_Click);
            // 
            // btnEliminarPagina
            // 
            this.btnEliminarPagina.BackColor = System.Drawing.Color.DarkRed;
            this.btnEliminarPagina.ForeColor = System.Drawing.Color.Coral;
            this.btnEliminarPagina.Location = new System.Drawing.Point(374, 336);
            this.btnEliminarPagina.Name = "btnEliminarPagina";
            this.btnEliminarPagina.Size = new System.Drawing.Size(173, 32);
            this.btnEliminarPagina.TabIndex = 4;
            this.btnEliminarPagina.Text = "Eliminar Pagina";
            this.btnEliminarPagina.UseVisualStyleBackColor = false;
            this.btnEliminarPagina.Click += new System.EventHandler(this.btnEliminarPagina_Click);
            // 
            // lstPagina
            // 
            this.lstPagina.FormattingEnabled = true;
            this.lstPagina.Location = new System.Drawing.Point(374, 378);
            this.lstPagina.Name = "lstPagina";
            this.lstPagina.Size = new System.Drawing.Size(173, 147);
            this.lstPagina.TabIndex = 5;
            this.lstPagina.SelectedIndexChanged += new System.EventHandler(this.lstPagina_SelectedIndexChanged);
            // 
            // btnFinalizar
            // 
            this.btnFinalizar.BackColor = System.Drawing.Color.DarkRed;
            this.btnFinalizar.ForeColor = System.Drawing.Color.Coral;
            this.btnFinalizar.Location = new System.Drawing.Point(374, 546);
            this.btnFinalizar.Name = "btnFinalizar";
            this.btnFinalizar.Size = new System.Drawing.Size(173, 32);
            this.btnFinalizar.TabIndex = 6;
            this.btnFinalizar.Text = "Generar documento PDF";
            this.btnFinalizar.UseVisualStyleBackColor = false;
            this.btnFinalizar.Click += new System.EventHandler(this.btnFinalizar_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel1.Location = new System.Drawing.Point(12, 105);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(342, 502);
            this.panel1.TabIndex = 7;
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.DarkRed;
            this.btnCerrar.ForeColor = System.Drawing.Color.Coral;
            this.btnCerrar.Location = new System.Drawing.Point(374, 671);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(173, 32);
            this.btnCerrar.TabIndex = 8;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnAbrirPDF
            // 
            this.btnAbrirPDF.BackColor = System.Drawing.Color.DarkRed;
            this.btnAbrirPDF.ForeColor = System.Drawing.Color.Coral;
            this.btnAbrirPDF.Location = new System.Drawing.Point(12, 671);
            this.btnAbrirPDF.Name = "btnAbrirPDF";
            this.btnAbrirPDF.Size = new System.Drawing.Size(342, 32);
            this.btnAbrirPDF.TabIndex = 9;
            this.btnAbrirPDF.Text = "Abrir documento PDF";
            this.btnAbrirPDF.UseVisualStyleBackColor = false;
            this.btnAbrirPDF.Click += new System.EventHandler(this.btnAbrirPDF_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(374, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "DIGITALIZACIÓN DOCUMENTO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(377, 283);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Max.  Páginas por Doc. PDF";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 614);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(342, 23);
            this.progressBar1.TabIndex = 12;
            this.progressBar1.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(22, 646);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 18);
            this.label3.TabIndex = 13;
            this.label3.Text = "Digitalizando : ";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = System.Drawing.Color.White;
            this.lblInfo.Location = new System.Drawing.Point(133, 646);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(46, 18);
            this.lblInfo.TabIndex = 14;
            this.lblInfo.Text = "lblInfo";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(116, 87);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label7);
            this.panel5.Location = new System.Drawing.Point(115, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(455, 87);
            this.panel5.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(9, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(343, 18);
            this.label7.TabIndex = 1;
            this.label7.Text = "Ministerio de Relaciones Exteriores del Perú";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.PapayaWhip;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Location = new System.Drawing.Point(1, -8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(570, 100);
            this.panel2.TabIndex = 2;
            // 
            // frmEscanear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Brown;
            this.ClientSize = new System.Drawing.Size(568, 727);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAbrirPDF);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.picImagen);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnFinalizar);
            this.Controls.Add(this.lstPagina);
            this.Controls.Add(this.btnEliminarPagina);
            this.Controls.Add(this.btnAgregarPagina);
            this.Controls.Add(this.Devices);
            this.Controls.Add(this.btnEscanear);
            this.Controls.Add(this.panel2);
            this.MaximizeBox = false;
            this.Name = "frmEscanear";
            this.Text = "Escanear Documento";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picImagen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEscanear;
        private System.Windows.Forms.ListBox Devices;
        private System.Windows.Forms.PictureBox picImagen;
        private System.Windows.Forms.Button btnAgregarPagina;
        private System.Windows.Forms.Button btnEliminarPagina;
        private System.Windows.Forms.ListBox lstPagina;
        private System.Windows.Forms.Button btnFinalizar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnAbrirPDF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
    }
}

