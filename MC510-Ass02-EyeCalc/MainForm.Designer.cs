namespace MC510_Ass02_EyeCalc
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.calcControl = new MC510_Ass02_EyeCalc.CalcControl();
            this.SuspendLayout();
            // 
            // calcControl
            // 
            this.calcControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calcControl.Location = new System.Drawing.Point(0, 0);
            this.calcControl.Name = "calcControl";
            this.calcControl.Size = new System.Drawing.Size(584, 712);
            this.calcControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 712);
            this.Controls.Add(this.calcControl);
            this.Name = "MainForm";
            this.Text = "MC510-Ass02-EyeCalc";
            this.ResumeLayout(false);

        }

        #endregion

        private CalcControl calcControl;
    }
}

