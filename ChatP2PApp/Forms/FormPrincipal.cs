using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using br.chatp2p.Networking;

namespace br.chatp2p.Forms
{
    public partial class FormPrincipal : Form
    {
        private ChatApp m_chatApp;
        private Contato m_contatoSelecionado;

        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            m_chatApp = ChatApp.Default;
            m_chatApp.ContatosAtualizadosEvent += new ChatApp.ContatosAtualizadosEventHandler(m_chatApp_ContatosAtualizadosEvent);
            m_chatApp.MensagensAtualizadasEvent += new ChatApp.MensagensAtualizadasEventHandler(m_chatApp_MensagensAtualizadasEvent);

            this.Text = String.Format("{0} - {1} @ {2}", Application.ProductName, ChatApp.Default.Usuario.Apelido, ChatApp.Default.Usuario.EndPoint.ToString());

            AtualizarContatos();
        }

        private void m_chatApp_MensagensAtualizadasEvent(object sender, ChatApp.MensagensAtualizadasEventArgs args)
        {
            if (m_contatoSelecionado == args.Contato)
            {
                AtualizarMensagens(args.Contato);
            }
        }

        private void m_chatApp_ContatosAtualizadosEvent(object sender, ChatApp.ContatosAtualizadosEventArgs args)
        {
            AtualizarContatos();
        }

        private void FormPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChatApp.Default.SairDoChat();
            Application.Exit();
        }

        private void AtualizarContatos()
        {
            string selecionado = "";
            try
            {
                if (lvContatos.SelectedItems.Count > 0)
                {
                    selecionado = ((Contato)lvContatos.SelectedItems[0].Tag).Apelido;
                }
                lvContatos.Items.Clear();
                foreach (Contato contato in ChatApp.Default.Contatos)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = String.Format("{0} @ {1}", contato.Apelido, contato.Endereco.ToString());
                    lvi.ImageKey = (contato.Online ? "online" : "offline");
                    lvi.Tag = contato;
                    if (contato.Apelido == selecionado) lvi.Selected = true;
                    lvContatos.Items.Add(lvi);
                }
            }
            catch (Exception) { }
        }

        private void AtualizarMensagens(Contato contato)
        {
            lvMensagens.Items.Clear();
            try
            {
                foreach (Mensagem mensagem in contato.Mensagens)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = mensagem.Texto;
                    if (mensagem.Tipo == Mensagem.TipoMensagem.MensagemEnviada)
                    {
                        lvi.ImageKey = (mensagem.Confirmada ? "enviada_confirmada" : "enviada_nao_confirmada");
                    }
                    else
                    {
                        lvi.ImageKey = "recebida";
                    }
                    lvi.Tag = mensagem;
                    lvMensagens.Items.Add(lvi);
                }
            }
            catch (Exception) { }
        }

        private void lvContatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Contato contato = null;
            if (lvContatos.SelectedItems.Count > 0)
            {
                contato = lvContatos.SelectedItems[0].Tag as Contato;
            }
            SelecionarContato(contato);
        }

        private void SelecionarContato(Contato contato)
        {
            m_contatoSelecionado = contato;
            if (m_contatoSelecionado != null)
            {
                textMensagem.Enabled = true;
                buttonEnviar.Enabled = true;
                labelApelidoContato.Text = String.Format("{0} (@ {1})", contato.Apelido, contato.Endereco.ToString());
                AtualizarMensagens(contato);
            }
            else
            {
                labelApelidoContato.Text = "";
                lvMensagens.Items.Clear();
                buttonEnviar.Enabled = false;
                textMensagem.Enabled = false;
            }
        }

        private void EnviarMensagem()
        {
            if (m_contatoSelecionado == null) return;
            if (textMensagem.Text == "") return;
            m_chatApp.EnviarMensagem(m_contatoSelecionado, textMensagem.Text);
            textMensagem.Text = "";
            try
            {
                textMensagem.Focus();
            }
            catch (Exception) { }
        }

        private void buttonEnviar_Click(object sender, EventArgs e)
        {
            EnviarMensagem();
        }

        private void textMensagem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EnviarMensagem();
                e.Handled = true;
            }
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            AtualizarContatos();
        }

        private void buttonAdicionar_Click(object sender, EventArgs e)
        {
            AdicionarContato();
        }

        private void AdicionarContato()
        {
            using (FormAdicionarContato f = new FormAdicionarContato())
            {
                DialogResult r = f.ShowDialog(this);
                if (r == System.Windows.Forms.DialogResult.OK)
                {
                    Contato c = new Contato();
                    c.Apelido = f.Apelido;
                    c.Endereco = EnderecoIP.ParseEndPoint(f.Endereco);
                    ChatApp.Default.Contatos.Add(c);
                    AtualizarContatos();
                }
            }
        }
    }
}
