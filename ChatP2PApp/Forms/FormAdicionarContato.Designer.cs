namespace br.chatp2p.Forms
{
    partial class FormAdicionarContato
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
            this.label1 = new System.Windows.Forms.Label();
            this.textApelido = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textEndereco = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Apelido:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textApelido
            // 
            this.textApelido.Location = new System.Drawing.Point(17, 36);
            this.textApelido.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textApelido.Name = "textApelido";
            this.textApelido.Size = new System.Drawing.Size(286, 26);
            this.textApelido.TabIndex = 1;
            this.textApelido.TextChanged += new System.EventHandler(this.textApelido_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Endereço de rede (ip:porta):";
            // 
            // textEndereco
            // 
            this.textEndereco.Location = new System.Drawing.Point(14, 110);
            this.textEndereco.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textEndereco.Name = "textEndereco";
            this.textEndereco.Size = new System.Drawing.Size(286, 26);
            this.textEndereco.TabIndex = 3;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(128, 145);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(84, 29);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Location = new System.Drawing.Point(219, 145);
            this.buttonCancelar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(84, 29);
            this.buttonCancelar.TabIndex = 5;
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);
            // 
            // FormAdicionarContato
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 189);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textEndereco);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textApelido);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormAdicionarContato";
            this.Text = "Adicionar Contato";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textApelido;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textEndereco;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancelar;
    }
}