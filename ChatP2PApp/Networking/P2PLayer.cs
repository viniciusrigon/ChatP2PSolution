using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net.NetworkInformation;

namespace br.chatp2p.Networking
{
    public class P2PLayer
    {
        private const int AGUARDAR_DESCONEXAO_MS = 1000;
        private const int MAX_CONNECTIONS = 10;

        private TcpListener m_tcpListener;
        private Thread m_serverThread;
        private int m_porta;
        private bool m_parar;
        private IPEndPoint m_currentEndPoint;
        private IPAddress m_broadcastAddress;

        public P2PLayer()
        {
            m_porta = ChatApp.PORTA_CONVERSA;
        }

        public bool Rodando { get { return m_serverThread != null && m_serverThread.IsAlive; } }
        public int PortaServidor { get { return m_porta; } set { m_porta = value; } }
        public IPAddress BroadcastAddress { get { return m_broadcastAddress; } }

        /// <summary>
        /// Descobre o ip de rede local do computador, dando preferência para IPv4 sobre IPv6.
        /// Ignora as placas de rede virtuais do VMWare, e a interface local/loopback.
        /// </summary>
        /// <returns></returns>
        private IPAddress GetMyIPAddress()
        {
            m_broadcastAddress = IPAddress.Broadcast;
            // Verifica as placas de rede
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Ignora as placas de rede desativadas
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    // Ignora as placas de rede do VMWare
                    if (!ni.Name.ToLower().Contains("vmware"))
                    {
                        IPInterfaceProperties ipp = ni.GetIPProperties();
                        UnicastIPAddressInformationCollection uic = ipp.UnicastAddresses;                        

                        // Testa cada IP unicast da placa de rede
                        foreach (UnicastIPAddressInformation ui in uic)
                        {
                            // Ignora o IP se for um IP loopback
                            if(!IPAddress.IsLoopback(ui.Address))
                            {
                                // Dá preferência para IPv4 sobre IPv6
                                if (ui.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    m_broadcastAddress = EnderecoIP.GetBroadcastAddress(ui.Address, ui.IPv4Mask);
                                    return ui.Address;
                                }
                            }
                        }

                        // Verifica novamente e seleciona sem testar IPv4 / IPv6
                        foreach (UnicastIPAddressInformation ui in uic)
                        {
                            if (!IPAddress.IsLoopback(ui.Address))
                            {
                                return ui.Address;
                            }
                        }
                    }
                }
            }

            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in ips)
            {
                if (!IPAddress.IsLoopback(ip)) return ip;
            }

            if (ips.Length > 0)
                return ips[0];
            else
                return IPAddress.Any;
        }

        private bool ConectarPorta(int porta)
        {
            IPEndPoint p = new IPEndPoint(IPAddress.Any, porta);
            try
            {
                m_tcpListener = new TcpListener(p);
                m_tcpListener.Start(MAX_CONNECTIONS);
                m_currentEndPoint = new IPEndPoint(GetMyIPAddress(), porta);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Não foi possível alocar o soquete TCP {0}: {1}.", p.ToString(), ex.Message));
                return false;
            }
        }

        public void Iniciar()
        {
            if (Rodando) return;
            if (!ConectarPorta(m_porta))
            {
                if (!ConectarPorta(m_porta + 1))
                {
                    if (!ConectarPorta(m_porta + 2)) return;
                }
            }
            m_serverThread = new Thread(RunThread);
            m_serverThread.Start();
        }

        public void Finalizar()
        {
            if (m_serverThread == null) return;
            try
            {
                m_parar = true;
                if (m_tcpListener != null) m_tcpListener.Stop();
                m_tcpListener = null;
            }
            catch (Exception) { }
            m_serverThread.Join(AGUARDAR_DESCONEXAO_MS);
            m_serverThread = null;
            m_tcpListener = null;
        }

        private void RunThread()
        {
            TcpListener tcpListener = m_tcpListener;
            while ((m_tcpListener != null) && (!m_parar))
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                try
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    NetworkStream ns = client.GetStream();
                    TextReader tr = new StreamReader(ns);
                    string strMensagem = tr.ReadToEnd();
                    tr.Close();
                    tr.Dispose();
                    client.Close();

                    P2PMessage mensagem = P2PMessage.Desempacotar(strMensagem);

                    if (mensagem != null)
                    {
                        // TODO: obter o endpoint de origem
                        MensagemRecebida(mensagem, null);
                    }
                }
                catch (Exception) { }
            }
        }

        private void MensagemRecebida(P2PMessage m, IPEndPoint origem)
        {
            // DEBUG: teste de envio para si próprio
            // if (m.ApelidoRemetente == ChatApp.Default.Usuario.Apelido) return;

            if (m.ApelidoDestinatario != ChatApp.Default.Usuario.Apelido) return;

            switch(m.Tipo)
            {
                case P2PMessage.TipoMensagem.MensagemPing:
                case P2PMessage.TipoMensagem.MensagemPong:
                case P2PMessage.TipoMensagem.MensagemTextoLido:
                case P2PMessage.TipoMensagem.MensagemTexto:
                    EnfileirarMensagem(m);
                    break;
                default:
                    Debug.WriteLine("TCP: mensagem recebida com tipo inválido.");
                    break;
            }
        }

        private void EnfileirarMensagem(P2PMessage m)
        {
            ChatApp.Default.EnfileirarMensagemRecebida(m);
        }

        public void EnviarUnicast(P2PMessage mensagem, IPEndPoint destino)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(destino);
                NetworkStream ns = client.GetStream();
                TextWriter tw = new StreamWriter(ns);
                string strMensagem = mensagem.Empacotar();
                tw.Write(strMensagem);
                tw.Flush();
                tw.Close();
                tw.Dispose();
                client.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("TCP: falha no envio de unicast para {0}: {1}", destino.ToString(), ex.Message));
            }
        }

        public IPEndPoint GetEndPoint()
        {
            return m_currentEndPoint;
        }
    }
}
