using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace br.chatp2p
{
    [Serializable]
    public class Contexto : ISerializable
    {
        private const string CONFIG_FILE_NAME = "chatp2p.cfg";

        private List<Contato> m_contatos;
        private ProprioContato m_usuario;

        public Contexto()
        {
            m_contatos = new List<Contato>();
            m_usuario = new ProprioContato();
        }

        public Contexto(SerializationInfo info, StreamingContext context)
        {
            m_contatos = new List<Contato>();
            m_usuario = new ProprioContato();
            m_contatos = (List<Contato>)info.GetValue("Contatos", typeof(List<Contato>));
            m_usuario = (ProprioContato)info.GetValue("Usuario", typeof(ProprioContato));
        }

        public List<Contato> Contatos { get { return m_contatos; } }
        public ProprioContato Usuario { get { return m_usuario; } }

        private static string ConfigFileName()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            return Path.Combine(path, CONFIG_FILE_NAME);
        }

        public void ArmazenarContexto()
        {
            BinaryFormatter bf = new BinaryFormatter();
            // XmlSerializer bf = new XmlSerializer(typeof(Contexto));
            using (FileStream fs = new FileStream(ConfigFileName(), FileMode.Create, FileAccess.Write))
            {
                bf.Serialize(fs, this);
                fs.Flush();
                fs.Close();
            }
        }

        public static Contexto CarregarContexto()
        {
            string fileName = ConfigFileName();
            if (File.Exists(fileName))
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    Contexto ret = (Contexto)bf.Deserialize(fs);
                    return ret;
                }
            }
            return null;
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Contatos", m_contatos, typeof(List<Contato>));
            info.AddValue("Usuario", m_usuario, typeof(ProprioContato));
        }

        #endregion
    }
}
