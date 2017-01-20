using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Net;

namespace br.chatp2p.Networking
{
    public class DiscoveryMessage
    {
        private IPEndPoint m_endPoint;

        public enum TipoMensagem
        {
            MensagemEntrou,
            MensagemSaiu,
            MensagemParticipante
        }
        
        public TipoMensagem Tipo;
        public string ApelidoRemetente;
        public string EnderecoRemetente;

        public IPEndPoint GetEndPoint()
        {
            return m_endPoint;
        }

        public DiscoveryMessage()
        {
        }

        public static DiscoveryMessage Desempacotar(byte[] dados)
        {
            XmlSerializer xs = new XmlSerializer(typeof(DiscoveryMessage));
            MemoryStream ms = new MemoryStream(dados);
            DiscoveryMessage ret = (DiscoveryMessage)xs.Deserialize(ms);
            ret.m_endPoint = EnderecoIP.ParseEndPoint(ret.EnderecoRemetente);
            return ret;
        }

        public byte[] Empacotar()
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(DiscoveryMessage));
            xs.Serialize(ms, this);
            ms.Flush();
            return ms.ToArray();
        }
    }
}
