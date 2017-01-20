namespace br.chatp2p.Forms
{
    partial class FormPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.label1 = new System.Windows.Forms.Label();
            this.lvContatos = new System.Windows.Forms.ListView();
            this.imgIcones = new System.Windows.Forms.ImageList(this.components);
            this.lvMensagens = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.textMensagem = new System.Windows.Forms.TextBox();
            this.buttonEnviar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelApelidoContato = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonAdicionar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Contatos:";
            // 
            // lvContatos
            // 
            this.lvContatos.Location = new System.Drawing.Point(20, 31);
            this.lvContatos.Margin = new System.Windows.Forms.Padding(4);
            this.lvContatos.Name = "lvContatos";
            this.lvContatos.Size = new System.Drawing.Size(311, 527);
            this.lvContatos.SmallImageList = this.imgIcones;
            this.lvContatos.TabIndex = 1;
            this.lvContatos.UseCompatibleStateImageBehavior = false;
            this.lvContatos.View = System.Windows.Forms.View.SmallIcon;
            this.lvContatos.SelectedIndexChanged += new System.EventHandler(this.lvContatos_SelectedIndexChanged);
            // 
            // imgIcones
            // 
            this.imgIcones.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcones.ImageStream")));
            this.imgIcones.TransparentColor = System.Drawing.Color.Transparent;
            this.imgIcones.Images.SetKeyName(0, "enviada_confirmada");
            this.imgIcones.Images.SetKeyName(1, "enviada_nao_confirmada");
            this.imgIcones.Images.SetKeyName(2, "offline");
            this.imgIcones.Images.SetKeyName(3, "online");
            this.imgIcones.Images.SetKeyName(4, "recebida");
            this.imgIcones.Images.SetKeyName(5, "refresh");
            // 
            // lvMensagens
            // 
            this.lvMensagens.Location = new System.Drawing.Point(340, 31);
            this.lvMensagens.Margin = new System.Windows.Forms.Padding(4);
            this.lvMensagens.Name = "lvMensagens";
            this.lvMensagens.Size = new System.Drawing.Size(367, 473);
            this.lvMensagens.SmallImageList = this.imgIcones;
            this.lvMensagens.TabIndex = 2;
            this.lvMensagens.UseCompatibleStateImageBehavior = false;
            this.lvMensagens.View = System.Windows.Forms.View.List;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(336, 514);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Enviar mensagem:";
            // 
            // textMensagem
            // 
            this.textMensagem.Enabled = false;
            this.textMensagem.Location = new System.Drawing.Point(340, 534);
            this.textMensagem.Margin = new System.Windows.Forms.Padding(4);
            this.textMensagem.Name = "textMensagem";
            this.textMensagem.Size = new System.Drawing.Size(268, 22);
            this.textMensagem.TabIndex = 4;
            this.textMensagem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textMensagem_KeyDown);
            // 
            // buttonEnviar
            // 
            this.buttonEnviar.Enabled = false;
            this.buttonEnviar.Location = new System.Drawing.Point(619, 534);
            this.buttonEnviar.Margin = new System.Windows.Forms.Padding(4);
            this.buttonEnviar.Name = "buttonEnviar";
            this.buttonEnviar.Size = new System.Drawing.Size(89, 23);
            this.buttonEnviar.TabIndex = 5;
            this.buttonEnviar.Text = "enviar";
            this.buttonEnviar.UseVisualStyleBackColor = true;
            this.buttonEnviar.Click += new System.EventHandler(this.buttonEnviar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(336, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Mensagens de";
            // 
            // labelApelidoContato
            // 
            this.labelApelidoContato.AutoSize = true;
            this.labelApelidoContato.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApelidoContato.Location = new System.Drawing.Point(444, 11);
            this.labelApelidoContato.Name = "labelApelidoContato";
            this.labelApelidoContato.Size = new System.Drawing.Size(14, 17);
            this.labelApelidoContato.TabIndex = 11;
            this.labelApelidoContato.Text = "-";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.ImageKey = "refresh";
            this.buttonRefresh.ImageList = this.imgIcones;
            this.buttonRefresh.Location = new System.Drawing.Point(116, 5);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(25, 23);
            this.buttonRefresh.TabIndex = 13;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonAdicionar
            // 
            this.buttonAdicionar.ImageKey = "online";
            this.buttonAdicionar.ImageList = this.imgIcones;
            this.buttonAdicionar.Location = new System.Drawing.Point(84, 5);
            this.buttonAdicionar.Name = "buttonAdicionar";
            this.buttonAdicionar.Size = new System.Drawing.Size(26, 23);
            this.buttonAdicionar.TabIndex = 12;
            this.toolTip1.SetToolTip(this.buttonAdicionar, "Adicionar contato usando endereço:porta");
            this.buttonAdicionar.UseVisualStyleBackColor = true;
            this.buttonAdicionar.Click += new System.EventHandler(this.buttonAdicionar_Click);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 581);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.buttonAdicionar);
            this.Controls.Add(this.labelApelidoContato);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonEnviar);
            this.Controls.Add(this.textMensagem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lvMensagens);
            this.Controls.Add(this.lvContatos);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormPrincipal";
            this.Text = "FormPrincipal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPrincipal_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPrincipal_FormClosed);
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvContatos;
        private System.Windows.Forms.ListView lvMensagens;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textMensagem;
        private System.Windows.Forms.Button buttonEnviar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelApelidoContato;
        private System.Windows.Forms.ImageList imgIcones;
        private System.Windows.Forms.Button buttonAdicionar;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonRefresh;
    }
}