namespace SGAC.WinApp
{
    partial class FrmReporteTipos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteTipos));
            this.CmdImprimir = new System.Windows.Forms.Button();
            this.CmdCancelar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RdoOpcion2 = new System.Windows.Forms.RadioButton();
            this.RdoOpcion1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.DtpFchEmision = new System.Windows.Forms.DateTimePicker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmdImprimir
            // 
            this.CmdImprimir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(94)))), ((int)(((byte)(188)))));
            this.CmdImprimir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdImprimir.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmdImprimir.ForeColor = System.Drawing.Color.White;
            this.CmdImprimir.Location = new System.Drawing.Point(68, 136);
            this.CmdImprimir.Name = "CmdImprimir";
            this.CmdImprimir.Size = new System.Drawing.Size(100, 33);
            this.CmdImprimir.TabIndex = 0;
            this.CmdImprimir.Text = "&Imprimir";
            this.CmdImprimir.UseVisualStyleBackColor = false;
            this.CmdImprimir.Click += new System.EventHandler(this.CmdImprimir_Click);
            // 
            // CmdCancelar
            // 
            this.CmdCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmdCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CmdCancelar.Font = new System.Drawing.Font("Arial", 9F);
            this.CmdCancelar.Location = new System.Drawing.Point(185, 136);
            this.CmdCancelar.Name = "CmdCancelar";
            this.CmdCancelar.Size = new System.Drawing.Size(100, 33);
            this.CmdCancelar.TabIndex = 1;
            this.CmdCancelar.Text = "&Cerrar";
            this.CmdCancelar.UseVisualStyleBackColor = true;
            this.CmdCancelar.Click += new System.EventHandler(this.CmdCancelar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RdoOpcion2);
            this.groupBox1.Controls.Add(this.RdoOpcion1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.DtpFchEmision);
            this.groupBox1.Location = new System.Drawing.Point(15, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 122);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // RdoOpcion2
            // 
            this.RdoOpcion2.AutoSize = true;
            this.RdoOpcion2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RdoOpcion2.Font = new System.Drawing.Font("Arial", 9F);
            this.RdoOpcion2.ForeColor = System.Drawing.Color.Black;
            this.RdoOpcion2.Location = new System.Drawing.Point(31, 88);
            this.RdoOpcion2.Name = "RdoOpcion2";
            this.RdoOpcion2.Size = new System.Drawing.Size(198, 19);
            this.RdoOpcion2.TabIndex = 3;
            this.RdoOpcion2.TabStop = true;
            this.RdoOpcion2.Text = "Detalle de Ticket Emitidos x Dia";
            this.RdoOpcion2.UseVisualStyleBackColor = true;
            // 
            // RdoOpcion1
            // 
            this.RdoOpcion1.AutoSize = true;
            this.RdoOpcion1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RdoOpcion1.Font = new System.Drawing.Font("Arial", 9F);
            this.RdoOpcion1.ForeColor = System.Drawing.Color.Black;
            this.RdoOpcion1.Location = new System.Drawing.Point(31, 64);
            this.RdoOpcion1.Name = "RdoOpcion1";
            this.RdoOpcion1.Size = new System.Drawing.Size(214, 19);
            this.RdoOpcion1.TabIndex = 2;
            this.RdoOpcion1.TabStop = true;
            this.RdoOpcion1.Text = "Resumen de Ticket Emitidos x Dia";
            this.RdoOpcion1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(31, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fecha de Emision";
            // 
            // DtpFchEmision
            // 
            this.DtpFchEmision.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DtpFchEmision.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpFchEmision.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpFchEmision.Location = new System.Drawing.Point(148, 31);
            this.DtpFchEmision.Name = "DtpFchEmision";
            this.DtpFchEmision.Size = new System.Drawing.Size(129, 21);
            this.DtpFchEmision.TabIndex = 0;
            this.DtpFchEmision.ValueChanged += new System.EventHandler(this.DtpFchEmision_ValueChanged);
            this.DtpFchEmision.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DtpFchEmision_KeyDown);
            this.DtpFchEmision.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DtpFchEmision_KeyPress);
            this.DtpFchEmision.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DtpFchEmision_KeyUp);
            // 
            // FrmReporteTipos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(200)))), ((int)(((byte)(240)))));
            this.CancelButton = this.CmdCancelar;
            this.ClientSize = new System.Drawing.Size(334, 181);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CmdCancelar);
            this.Controls.Add(this.CmdImprimir);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(360, 210);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(340, 210);
            this.Name = "FrmReporteTipos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tipo de Reportes";
            this.toolTip1.SetToolTip(this, "Presione la tecla ESC para cerrar la ventana");
            this.Activated += new System.EventHandler(this.FrmReportesTicket_Activated);
            this.Load += new System.EventHandler(this.FrmReportesTicket_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CmdImprimir;
        private System.Windows.Forms.Button CmdCancelar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DtpFchEmision;
        private System.Windows.Forms.RadioButton RdoOpcion2;
        private System.Windows.Forms.RadioButton RdoOpcion1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}