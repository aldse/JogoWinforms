namespace JogoWinforms
{
    partial class Menu
    {
        /// <summary>
        /// Variável de designer obrigatória..
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Descartar
        /// </summary>
        /// <param name="disposing">verdadeiro se os recursos gerenciados devem ser descartados; caso contrário, falso.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

            #region Windows Form Designer generated code

            private void InitializeComponent()
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
                this.loreLabel = new System.Windows.Forms.Label();
                this.SuspendLayout();
                // 
                // loreLabel
                // 
                this.loreLabel.BackColor = System.Drawing.Color.Transparent;
                this.loreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.loreLabel.ForeColor = System.Drawing.Color.Black;
                this.loreLabel.Location = new System.Drawing.Point(115, 99);
                this.loreLabel.Name = "loreLabel";
                this.loreLabel.Size = new System.Drawing.Size(554, 102);
                this.loreLabel.TabIndex = 1;
                this.loreLabel.Text = ".";
                this.loreLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                // 
                // Menu
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.BackgroundImage = global::Menu.Properties.Resources.cena;
                this.ClientSize = new System.Drawing.Size(810, 479);
                this.ControlBox = false;
                this.Controls.Add(this.loreLabel);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
                this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
                this.Name = "Menu";
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.Text = "Menu";
                this.ResumeLayout(false);

            }

        #endregion

        private System.Windows.Forms.Label loreLabel;
    }
}