using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Diagnostics;

namespace br.chatp2p.Networking
{
    public class DiscoveryLayer
    {
        private const int AGUARDAR_DESCONEXAO_MS = 1000;

        private UdpClient m_udpClient;
        private Thread m_serverThread;
        private int m_porta;
        private bool m_parar;
        private IPAddress m_broadcastAddress;

        public DiscoveryLayer()
        {
            m_porta = ChatApp.PORTA_DESCOBERTA;
            m_broadcastAddress = IPAddress.Broadcast;
        }

        public bool Rodando { get { return m_serverThread != null && m_serverThread.IsAlive; } }
        public int PortaServidor { get { return m_porta; } set { m_porta = value; } }
        public IPAddress BroadcastAddress { get { return m_broadcastAddress; } set { m_broadcastAddress = value; } }

        private bool ConectarPorta(int porta)
        {
            IPEndPoint p = new IPEndPoint(IPAddress.Any, porta);
            try
            {                
                m_udpClient = new UdpClient(p);
                m_udpClient.EnableBroadcast = true;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Não foi possível alocar o soquete UDP {0}: {1}", p.ToString(), ex.Message));                
            }
        }

        public void Iniciar()
        {
            if (Rodando) return;
            if (!ConectarPorta(m_porta)) return;
            m_serverThread = new Thread(RunThread);
            m_serverThread.Start();
        }

        public void Finalizar()
        {
            if (m_serverThread == null) return;
            try
            {
                m_parar = true;
                if (m_udpClient != null) m_udpClient.Close();
                m_udpClient = null;
            }
            catch (Exception) { }
            m_serverThread.Join(AGUARDAR_DESCONEXAO_MS);
            m_serverThread = null;
            m_udpClient = null;
        }

        private void RunThread()
        {
            UdpClient udpClient = m_udpClient;
            while ((m_udpClient != null) && (!m_parar))
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                try
                {
                    byte[] dados = udpClient.Receive(ref remoteEP);
                    DiscoveryMessage m = DiscoveryMessage.Desempacotar(dados);
                    if (m != null)
                    {
                        MensagemRecebida(m, remoteEP);
                    }
                }
                catch (Exception) { }
            }
        }

        private void MensagemRecebida(DiscoveryMessage m, IPEndPoint origem)
        {
            // DEBUG: teste de envio para si próprio
            // if (m.ApelidoRemetente == ChatApp.Default.Usuario.Apelido) return;

            switch(m.Tipo)
            {
                case DiscoveryMessage.TipoMensagem.MensagemEntrou:
                    RecebidoEntrou(m, origem);
                    break;
                case DiscoveryMessage.TipoMensagem.MensagemSaiu:
                    RecebidoSaiu(m, origem);
                    break;
                case DiscoveryMessage.TipoMensagem.MensagemParticipante:
                    RecebidoParticipante(m, origem);
                    break;
                default:
                    Debug.WriteLine("UDP: mensagem recebida com tipo inválido.");
                    break;
            }
        }

        private void RecebidoEntrou(DiscoveryMessage m, IPEndPoint origem)
        {
            ArmazenarEndereco(m.ApelidoRemetente, m.GetEndPoint(), true);

            DiscoveryMessage dm = new DiscoveryMessage();
            dm.Tipo = DiscoveryMessage.TipoMensagem.MensagemParticipante;
            dm.EnderecoRemetente = ChatApp.Default.Usuario.EndPoint.ToString();
            dm.ApelidoRemetente = ChatApp.Default.Usuario.Apelido;
            EnviarBroadcast(dm);
        }

        private void RecebidoSaiu(DiscoveryMessage m, IPEndPoint origem)
        {
            // DEBUG: teste de envio para si próprio
            // if (m.ApelidoRemetente == ChatApp.Default.Usuario.Apelido) return;

            ArmazenarEndereco(m.ApelidoRemetente, m.GetEndPoint(), false);
        }

        private void RecebidoParticipante(DiscoveryMessage m, IPEndPoint origem)
        {
            ArmazenarEndereco(m.ApelidoRemetente, m.GetEndPoint(), true);
        }

        private void ArmazenarEndereco(string apelido, IPEndPoint endereco, bool online)
        {
            ChatApp.Default.AtualizarStatusContato(apelido, endereco, online);
        }

        private void EnviarBroadcast(DiscoveryMessage mensagem)
        {
            // Bug no envio/recebimento de broadcast UDP no Vista / XP / Seven
            IPEndPoint destino = new IPEndPoint(m_broadcastAddress, m_porta);
            EnviarBroadcast(mensagem, destino);
            if (!m_broadcastAddress.Equals(IPAddress.Broadcast))
            {
                destino = new IPEndPoint(IPAddress.Broadcast, m_porta);
                EnviarBroadcast(mensagem, destino);
            }
        }

        private void EnviarBroadcast(DiscoveryMessage mensagem, IPEndPoint destino)
        {
            if (m_udpClient == null) return;            
            byte[] dados = mensagem.Empacotar();
            m_udpClient.Send(dados, dados.Length, destino);
        }

        public void NotificarUsuarioEntrou()
        {
            DiscoveryMessage dm = new DiscoveryMessage();
            dm.Tipo = DiscoveryMessage.TipoMensagem.MensagemEntrou;
            dm.ApelidoRemetente = ChatApp.Default.Usuario.Apelido;
            dm.EnderecoRemetente = ChatApp.Default.Usuario.EndPoint.ToString();
            EnviarBroadcast(dm);
        }

        public void NotificarUsuarioSaiu()
        {
            DiscoveryMessage dm = new DiscoveryMessage();
            dm.Tipo = DiscoveryMessage.TipoMensagem.MensagemSaiu;
            dm.ApelidoRemetente = ChatApp.Default.Usuario.Apelido;
            dm.EnderecoRemetente = ChatApp.Default.Usuario.EndPoint.ToString();
            EnviarBroadcast(dm);
        }

    }
}
