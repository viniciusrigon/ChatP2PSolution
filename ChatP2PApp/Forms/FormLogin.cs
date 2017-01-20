using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace br.chatp2p.Forms
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void Login()
        {
            try
            {
                if (textApelido.Text.Trim() == "") return;
                ChatApp.Default.Usuario.Apelido = textApelido.Text;
                ChatApp.Default.EntrarNoChat();
                FormPrincipal f = new FormPrincipal();
                f.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonEntrar_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            ChatApp.Default.CarregarContexto();
            textApelido.Text = ChatApp.Default.Usuario.Apelido;
        }

        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!ChatApp.Default.Logado)
            {
                ChatApp.Default.SairDoChat();
                Application.Exit();
            }
        }

        private void textApelido_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
                e.Handled = true;
            }
        }
    }
}
