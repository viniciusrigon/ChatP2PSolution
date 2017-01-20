using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization;

namespace br.chatp2p
{
    public class ProprioContato : ISerializable
    {
        public string Apelido;
        public IPEndPoint EndPoint;

        public ProprioContato()
        {            
            // cria um nome fictício no formato "UsuárioNNNN"
            string id = DateTime.Now.Ticks.ToString();
            Apelido = "Usuário" + id.Substring(id.Length - 4);
            EndPoint = new IPEndPoint(IPAddress.Any, 10);
        }

        public ProprioContato(SerializationInfo info, StreamingContext context)
        {
            Apelido = info.GetString("Apelido");
            EndPoint = new IPEndPoint(IPAddress.Any, 10);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Apelido", Apelido);
            EndPoint = new IPEndPoint(IPAddress.Any, 10);
        }
    }
}
