using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GenerateRandomScatter.PascalVOC
{
    public class PascalVocXmlSerializer : XmlSerializer
    {
        private XmlSerializerNamespaces _ns;
        private XmlWriterSettings _settings;

        public PascalVocXmlSerializer() : base((typeof(PascalVocAnnotation)))
        {
            _ns = new XmlSerializerNamespaces();
            _ns.Add("", "");

            _settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "\t"
            };
        }

        public PascalVocAnnotation Deserialize(string xmlText)
        {
            PascalVocAnnotation obj = new PascalVocAnnotation();
            using (var reader = new StringReader(xmlText))
            {
                obj = (PascalVocAnnotation)base.Deserialize(reader);
            }
            return obj;
        }

        public string Serialize(PascalVocAnnotation pascalVocAnnotation)
        {
            string xml = "";
            using (StringWriter sw = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sw, _settings))
            {
                this.Serialize(writer, pascalVocAnnotation, _ns);
                xml = sw.ToString();
            }
            return xml;
        }
    }
}
