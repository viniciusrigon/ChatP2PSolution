using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Net;
using System.Diagnostics;

namespace br.chatp2p
{
    [Serializable]
    public class Contato : ISerializable
    {
        private string m_apelido;
        private List<Mensagem> m_mensagens;
        private IPEndPoint m_endereco;
        private bool m_online;
        private Dictionary<long, Mensagem> m_idMensagens;

        public Contato()
        {
            m_mensagens = new List<Mensagem>();
            m_idMensagens = new Dictionary<long, Mensagem>();
            m_apelido = "";
            m_endereco = null;
            m_online = false;
        }

        public Contato(SerializationInfo info, StreamingContext context)
        {
            m_mensagens = new List<Mensagem>();
            m_idMensagens = new Dictionary<long, Mensagem>();
            m_apelido = info.GetString("Apelido");
            m_endereco = (IPEndPoint)info.GetValue("Endereco", typeof(IPEndPoint));
            m_online = false;
        }

        public string Apelido { get { return m_apelido; } set { m_apelido = value; } }
        public IPEndPoint Endereco { get { return m_endereco; } set { m_endereco = value; } }
        public bool Online { get { return m_online; } set { m_online = value; } }
        public List<Mensagem> Mensagens { get { return m_mensagens; } }

        public override string ToString()
        {
            return m_apelido + "@" + m_endereco.ToString();
        }

        public Mensagem AddNotification(string texto)
        {
            Mensagem m = new Mensagem();
            m.Tipo = Mensagem.TipoMensagem.MensagemNotificacao;
            m.Texto = texto;
            m_mensagens.Add(m);
            return m;
        }

        public void AddMensagem(Mensagem mensagem, long id)
        {
            m_idMensagens[id] = mensagem;
            m_mensagens.Add(mensagem);
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Apelido", m_apelido);
            info.AddValue("Endereco", m_endereco);
        }

        #endregion

        public bool MensagemExiste(long id)
        {
            return m_idMensagens.ContainsKey(id);
        }
    }
}
