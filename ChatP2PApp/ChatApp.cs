using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using br.chatp2p.Networking;

namespace br.chatp2p
{
    public class ChatApp
    {
        public class ContatosAtualizadosEventArgs : EventArgs
        {
            public Contato Contato;
        }

        public class MensagensAtualizadasEventArgs : EventArgs
        {
            public Contato Contato;
            public Mensagem Mensagem;
        }

        public const int PORTA_DESCOBERTA = 55100;
        public const int PORTA_CONVERSA = 55101;

        private static ChatApp m_singleTonChatApp = new ChatApp();

        public static ChatApp Default { get { return m_singleTonChatApp; } }

        // ---------------------------------------------------

        private Contexto m_contexto;
        private DiscoveryLayer m_discoveryLayer;
        private P2PLayer m_p2pLayer;
        private FilaMensagens m_filaMensagens;
        private bool m_logado;

        public delegate void ContatosAtualizadosEventHandler(object sender, ContatosAtualizadosEventArgs args);
        public event ContatosAtualizadosEventHandler ContatosAtualizadosEvent;

        public delegate void MensagensAtualizadasEventHandler(object sender, MensagensAtualizadasEventArgs args);
        public event MensagensAtualizadasEventHandler MensagensAtualizadasEvent;

        private ChatApp()
        {
            m_contexto = new Contexto();
            m_discoveryLayer = new DiscoveryLayer();
            m_p2pLayer = new P2PLayer();
            m_filaMensagens = new FilaMensagens();
            m_logado = false;
        }

        public void CarregarContexto()
        {
            m_contexto = null;
            try
            {
                m_contexto = Contexto.CarregarContexto();
                Debug.WriteLine("Contexto carregado.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha na carga do contexto: " + ex.Message);
            }
            if (m_contexto == null) m_contexto = new Contexto();
        }

        public void ArmazenarContexto()
        {
            try
            {
                m_contexto.ArmazenarContexto();
                Debug.WriteLine("Contexto armazenado.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha no armazenamento do contexto: " + ex.Message);
            }
        }

        public void EnviarMensagem(Contato destino, string textoMensagem)
        {
            Mensagem mensagem = new Mensagem();
            mensagem.Tipo = Mensagem.TipoMensagem.MensagemEnviada;
            mensagem.Texto = textoMensagem;
            destino.Mensagens.Add(mensagem);
            m_filaMensagens.EnviarMensagem(mensagem, destino);
            NotificarMensagemAtualizada(destino, mensagem);
        }

        public List<Contato> Contatos { get { return m_contexto.Contatos; } }
        public ProprioContato Usuario { get { return m_contexto.Usuario; } }
        public P2PLayer P2PLayer { get { return m_p2pLayer; } }
        public DiscoveryLayer DiscoveryLayer { get { return m_discoveryLayer; } }
        public bool Logado { get { return m_logado; } }

        public Contato AtualizarStatusContato(string apelido, string endereco, bool online)
        {
            return AtualizarStatusContato(apelido, EnderecoIP.ParseEndPoint(endereco), online);
        }

        public Contato AtualizarStatusContato(string apelido, IPEndPoint endereco, bool online)
        {
            Contato c = GetContato(apelido);
            if (c == null)
            {
                c = new Contato();
                c.Apelido = apelido;
                m_contexto.Contatos.Add(c);
            }
            bool estadoAnterior = c.Online;
            c.Endereco = endereco;
            c.Online = online;
            if (estadoAnterior != c.Online)
            {
                Mensagem m;
                if (c.Online)
                    m = c.AddNotification("entrou no chat");
                else
                    m = c.AddNotification("saiu do chat");
                NotificarContatoAtualizado(c);
                NotificarMensagemAtualizada(c, m);
            }
            return c;
        }

        public Contato GetContato(string apelido)
        {
            // TODO: acertar a rotina para busca em tempo constante
            foreach (Contato c in m_contexto.Contatos)
            {
                if (c.Apelido == apelido) return c;
            }
            return null;
        }

        public void EntrarNoChat()
        {
            // ----- inicialização -----
            m_filaMensagens.Iniciar();
            m_p2pLayer.Iniciar();
            m_discoveryLayer.Iniciar();

            // ----- configuração -----
            m_contexto.Usuario.EndPoint = m_p2pLayer.GetEndPoint();
            m_discoveryLayer.BroadcastAddress = m_p2pLayer.BroadcastAddress;

            // ----- notificação -----
            m_discoveryLayer.NotificarUsuarioEntrou();

            m_logado = true;
        }

        public void SairDoChat()
        {
            // ----- notificação -----
            m_discoveryLayer.NotificarUsuarioSaiu();

            // ----- finalização -----
            m_discoveryLayer.Finalizar();
            m_p2pLayer.Finalizar();
            m_filaMensagens.Parar();

            m_logado = false;
        }

        public void EnfileirarMensagemRecebida(P2PMessage mensagem)
        {
            m_filaMensagens.EnfileirarMensagemRecebida(mensagem);
        }

        public void NotificarContatoAtualizado(Contato contato)
        {
            ContatosAtualizadosEventArgs args = new ContatosAtualizadosEventArgs();
            args.Contato = contato;
            RaiseContatosAtualizadosEvent(args);
        }

        public void NotificarMensagemAtualizada(Contato contato, Mensagem mensagem)
        {
            MensagensAtualizadasEventArgs args = new MensagensAtualizadasEventArgs();
            args.Contato = contato;
            args.Mensagem = mensagem;
            RaiseMensagensAtualizadasEvent(args);
        }

        protected virtual void RaiseContatosAtualizadosEvent(ContatosAtualizadosEventArgs args)
        {
            ContatosAtualizadosEventHandler ev = ContatosAtualizadosEvent;
            if (ev != null)
            {
                Control c = ev.Target as Control;
                if (c != null)
                    c.BeginInvoke(ev, new object[] { this, args });
                else
                    ev.Invoke(this, args);
            }
        }

        protected virtual void RaiseMensagensAtualizadasEvent(MensagensAtualizadasEventArgs args)
        {
            MensagensAtualizadasEventHandler ev = MensagensAtualizadasEvent;
            if (ev != null)
            {
                Control c = ev.Target as Control;
                if (c != null)
                    c.BeginInvoke(ev, new object[] { this, args });
                else
                    ev.Invoke(this, args);
            }
        }
    }
}
