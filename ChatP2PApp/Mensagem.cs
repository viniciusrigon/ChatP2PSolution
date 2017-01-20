using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace br.chatp2p
{
    public class Mensagem
    {
        public enum TipoMensagem
        {
            MensagemRecebida,
            MensagemEnviada,
            MensagemNotificacao
        }

        public long ID;
        public TipoMensagem Tipo;
        public bool Confirmada;
        public string Texto;

        public static Mensagem Desempacotar(byte[] dados)
        {
            // string texto = Encoding.UTF8.GetString(dados);
            // XmlDocument x = new XmlDocument();
            // x.LoadXml(texto);
            XmlSerializer xs = new XmlSerializer(typeof(Mensagem));
            MemoryStream ms = new MemoryStream(dados);
            Mensagem ret = (Mensagem)xs.Deserialize(ms);
            return ret;
        }

        public byte[] Empacotar()
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(Mensagem));
            xs.Serialize(ms, this);
            ms.Flush();
            return ms.ToArray();
        }
    }
}
