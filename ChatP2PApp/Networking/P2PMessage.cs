using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Serialization;
using System.IO;

namespace br.chatp2p.Networking
{
    public class P2PMessage
    {
        private IPEndPoint m_endPoint;

        public enum TipoMensagem
        {
            MensagemTexto,
            MensagemTextoLido,
            MensagemPing,
            MensagemPong
        }

        public TipoMensagem Tipo;
        public long IDMensagem;
        public string ApelidoDestinatario;
        public string ApelidoRemetente;
        public string EnderecoRemetente;
        public string Texto;

        public P2PMessage()
        {
            Texto = "-";
        }

        public IPEndPoint GetEndPoint()
        {
            return m_endPoint;
        }

        public static P2PMessage Desempacotar(string dados)
        {
            XmlSerializer xs = new XmlSerializer(typeof(P2PMessage));
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(dados));
            P2PMessage ret = (P2PMessage)xs.Deserialize(ms);
            ret.m_endPoint = EnderecoIP.ParseEndPoint(ret.EnderecoRemetente);
            return ret;
        }

        public string Empacotar()
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(P2PMessage));
            xs.Serialize(ms, this);
            ms.Flush();
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
