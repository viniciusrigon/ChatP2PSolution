using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using br.chatp2p.Networking;

namespace br.chatp2p.Forms
{
    public partial class FormAdicionarContato : Form
    {
        public string Apelido;
        public string Endereco;

        public FormAdicionarContato()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;
            Apelido = textApelido.Text.Trim();
            Endereco = textEndereco.Text.Trim();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private bool ValidarCampos()
        {
            if (textApelido.Text.Trim().Length == 0)
            {
                MessageBox.Show("Apelido inválido.");
                return false;
            }
            try
            {
                IPEndPoint i = EnderecoIP.ParseEndPoint(textEndereco.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Endereço inválido.");
                return false;
            }
            return true;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textApelido_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
