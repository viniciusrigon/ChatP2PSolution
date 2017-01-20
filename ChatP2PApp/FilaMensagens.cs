using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using br.chatp2p.Networking;
using System.Threading;
using System.Diagnostics;

namespace br.chatp2p
{
    public class FilaMensagens
    {
        private const int TEMPO_POLL_MENSAGENS_RECEBIDAS_MS = 1000;
        private const int TEMPO_POLL_MENSAGENS_REENVIO_MS = 5000;

        private bool m_parar;
        private long m_idMensagem;
        private Queue<P2PMessage> m_mensagensRecebidas;
        private Dictionary<string, MensagemAguardando> m_mensagensAguardandoConfirmacao;
        private Thread m_threadRecebidas;
        private Thread m_threadAguardando;

        private class MensagemAguardando
        {
            public P2PMessage mensagemp2p;
            public Mensagem mensagemContato;
            public Contato contatoDestino;

            public bool PrecisaReenviar()
            {
                // TODO: acertar o reenvio de mensagens
                return true;
            }
        }

        public FilaMensagens()
        {
            m_parar = false;
            m_idMensagem = DateTime.Now.Ticks;
            m_mensagensAguardandoConfirmacao = new Dictionary<string, MensagemAguardando>();
            m_mensagensRecebidas = new Queue<P2PMessage>();
            m_threadRecebidas = null;
            m_threadAguardando = null;
        }

        public void Iniciar()
        {
            if (m_threadRecebidas != null && m_threadRecebidas.IsAlive) return;

            m_threadRecebidas = new Thread(ThreadProcessarMensagensRecebidas);
            m_threadAguardando = new Thread(ThreadMensagensAguardandoConfirmacao);

            m_mensagensAguardandoConfirmacao.Clear();
            m_mensagensRecebidas.Clear();

            m_threadRecebidas.Start();
            m_threadAguardando.Start();
        }

        public void Parar()
        {
            m_parar = true;
            if (m_threadAguardando != null)
            {
                m_threadAguardando.Join(3 * TEMPO_POLL_MENSAGENS_RECEBIDAS_MS);
            }
            if (m_threadRecebidas != null)
            {
                m_threadRecebidas.Join(3 * TEMPO_POLL_MENSAGENS_RECEBIDAS_MS);
            }
            m_threadAguardando = null;
            m_threadRecebidas = null;
            m_mensagensAguardandoConfirmacao.Clear();
            m_mensagensRecebidas.Clear();
        }

        // TODO: verificar race condition
        private long GetNewIDMensagem()
        {
            m_idMensagem++;
            return m_idMensagem;
        }

        private string GetMessageKey(string usuario, P2PMessage mensagem)
        {
            return String.Format("{0}-{1}", usuario, mensagem.IDMensagem);
        }

        private void AdicionarAguardandoConfirmacao(P2PMessage mensagemP2P, Mensagem mensagemContato, Contato destino)
        {
            MensagemAguardando m = new MensagemAguardando();
            m.mensagemp2p = mensagemP2P;
            m.mensagemContato = mensagemContato;
            m.contatoDestino = destino;
            m_mensagensAguardandoConfirmacao.Add(GetMessageKey(destino.Apelido, mensagemP2P), m);
        }

        public void EnviarMensagem(Mensagem mensagem, Contato destino)
        {
            P2PMessage m = new P2PMessage();
            m.ApelidoRemetente = ChatApp.Default.Usuario.Apelido;
            m.EnderecoRemetente = ChatApp.Default.Usuario.EndPoint.ToString();
            m.ApelidoDestinatario = destino.Apelido;
            m.Tipo = P2PMessage.TipoMensagem.MensagemTexto;
            m.IDMensagem = GetNewIDMensagem();
            m.Texto = mensagem.Texto;

            AdicionarAguardandoConfirmacao(m, mensagem, destino);

            EnviarMensagem(m, destino);
        }

        public void EnviarMensagem(P2PMessage mensagem, Contato contatoDestino)
        {
            ChatApp.Default.P2PLayer.EnviarUnicast(mensagem, contatoDestino.Endereco);
        }

        // ------------------------------------
        // ----- RECEBIMENTO DE MENSAGENS -----
        // ------------------------------------

        public void EnfileirarMensagemRecebida(Networking.P2PMessage mensagem)
        {
            m_mensagensRecebidas.Enqueue(mensagem);
        }

        private void ThreadProcessarMensagensRecebidas()
        {
            while (!m_parar)
            {
                try
                {
                    if (m_mensagensRecebidas.Count != 0)
                    {
                        P2PMessage mensagem = m_mensagensRecebidas.Dequeue();
                        try
                        {
                            ProcessarMensagemRecebida(mensagem);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(String.Format("Falha no processamento de mensagem recebida: {0}", ex.Message));
                        }
                    }
                }
                catch (Exception) { }
                Thread.Sleep(TEMPO_POLL_MENSAGENS_RECEBIDAS_MS);
            }
        }

        private void ProcessarMensagemRecebida(P2PMessage mensagem)
        {
            if (mensagem.ApelidoDestinatario != ChatApp.Default.Usuario.Apelido)
            {
                Debug.WriteLine("Destinatário incorreto no recebimento. Mensagem descartada.");
                return;
            }

            switch (mensagem.Tipo)
            {
                case P2PMessage.TipoMensagem.MensagemTexto:
                    ProcessarMensagemRecebidaTexto(mensagem);
                    break;
                case P2PMessage.TipoMensagem.MensagemTextoLido:
                    ProcessarMensagemRecebidaTextoLido(mensagem);
                    break;
                case P2PMessage.TipoMensagem.MensagemPing:
                    ProcessarMensagemRecebidaPing(mensagem);
                    break;
                case P2PMessage.TipoMensagem.MensagemPong:
                    ProcessarMensagemRecebidaPong(mensagem);
                    break;
                default:
                    Debug.WriteLine("Fila de mensagens: Mensagem recebida com tipo inválido.");
                    break;
            }
        }

        private void ProcessarMensagemRecebidaTexto(P2PMessage mensagem)
        {
            Mensagem m = new Mensagem();
            m.Confirmada = false;
            m.ID = mensagem.IDMensagem;
            m.Texto = mensagem.Texto;

            Contato origem = ChatApp.Default.AtualizarStatusContato(mensagem.ApelidoRemetente, mensagem.EnderecoRemetente, true);

            // TODO: verificar duplicação de mensagens recebidas
            if (!origem.MensagemExiste(m.ID))
            {
                origem.AddMensagem(m, m.ID);
                ChatApp.Default.NotificarMensagemAtualizada(origem, m);
            }

            // Confirma o recebimento da mensagens
            P2PMessage resposta = new P2PMessage();
            resposta.Tipo = P2PMessage.TipoMensagem.MensagemTextoLido;
            resposta.IDMensagem = mensagem.IDMensagem;
            resposta.ApelidoRemetente = ChatApp.Default.Usuario.Apelido;
            resposta.ApelidoDestinatario = mensagem.ApelidoRemetente;
            resposta.EnderecoRemetente = ChatApp.Default.Usuario.EndPoint.ToString();

            EnviarMensagem(resposta, origem);
        }

        private void ProcessarMensagemRecebidaTextoLido(P2PMessage mensagem)
        {
            string key = GetMessageKey(mensagem.ApelidoRemetente, mensagem);
            MensagemAguardando m;
            if (!m_mensagensAguardandoConfirmacao.TryGetValue(key, out m)) return;
            m_mensagensAguardandoConfirmacao.Remove(key);
            if (m.mensagemContato != null)
            {
                m.mensagemContato.Confirmada = true;                
                ChatApp.Default.NotificarMensagemAtualizada(m.contatoDestino, m.mensagemContato);
            }
        }

        private void ProcessarMensagemRecebidaPing(P2PMessage mensagem)
        {
            // TODO: processamento do PING
        }

        private void ProcessarMensagemRecebidaPong(P2PMessage mensagem)
        {
            // TODO: processamento do PONG
        }

        // ------------------------------------
        // ----- REENVIO DE MENSAGENS -----
        // ------------------------------------

        private void ThreadMensagensAguardandoConfirmacao()
        {
            while (!m_parar)
            {
                Thread.Sleep(TEMPO_POLL_MENSAGENS_REENVIO_MS);

                try
                {
                    if (m_mensagensAguardandoConfirmacao.Count > 0)
                    {
                        foreach (MensagemAguardando m in m_mensagensAguardandoConfirmacao.Values)
                        {
                            if (m.PrecisaReenviar())
                            {
                                Debug.WriteLine(String.Format("Reenviando {0}.", m.mensagemp2p.IDMensagem));
                                EnviarMensagem(m.mensagemp2p, m.contatoDestino);
                            }
                        }
                    }
                }
                catch (Exception) { }
            }
        }
    }
}
