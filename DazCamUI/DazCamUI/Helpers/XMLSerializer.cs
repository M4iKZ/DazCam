using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DazCamUI.Helpers
{
    public class XMLSerializer
    {

        #region Public Methods

        /// <summary>
        /// Converts a custom Object to XML string
        /// </summary>
        public static String SerializeObject(Object sourceObject)
        {
            Encoding encode = Encoding.UTF8;
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encode);
            xmlTextWriter.Formatting = Formatting.Indented;

            XmlSerializer xs = new XmlSerializer(sourceObject.GetType());

            xs.Serialize(xmlTextWriter, sourceObject);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;

            return ByteArrayToString(memoryStream.ToArray(), encode);
        }

        /// <summary>
        /// Reconstructs an Object from XML string
        /// </summary>
        public static Object DeserializeObject(String xml, Type objectType)
        {
            Encoding encode = Encoding.UTF8;
            XmlSerializer xs = new XmlSerializer(objectType);
            MemoryStream memoryStream = new MemoryStream(StringToByteArray(xml, encode));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encode);
            return xs.Deserialize(memoryStream);
        }

#endregion

        #region Private Methods

        private static String ByteArrayToString(Byte[] characters, Encoding encode)
        {
            String constructedString = encode.GetString(characters);
            return (constructedString);
        }

        private static Byte[] StringToByteArray(String pXmlString, Encoding encode)
        {
            Byte[] byteArray = encode.GetBytes(pXmlString);
            return byteArray;
        }

        #endregion
    }
}
