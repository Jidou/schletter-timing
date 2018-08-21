using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using NLog;

namespace Model {
    public class SaveLoad {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public static void SerializeObject<T>(T serializableObject, string fileName) {
            if (serializableObject == null) {
                return;
            }

            if (!fileName.EndsWith(".json")) {
                fileName += ".json";
            }

            try {
                using (StreamWriter file = File.CreateText(fileName)) {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, serializableObject);
                }
            } catch (Exception ex) {
                logger.Error(ex);
            }
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>(string fileName) {
            if (string.IsNullOrEmpty(fileName)) {
                return default(T);
            }

            if (!fileName.EndsWith(".json")) {
                fileName += ".json";
            }

            T objectOut = default(T);

            try {
                using (StreamReader file = File.OpenText(fileName)) {
                    JsonSerializer serializer = new JsonSerializer();
                    objectOut = (T)serializer.Deserialize(file, typeof(T));
                }


                //T objectOut = JsonConvert.DeserializeObject<T>(json);
                //using (StringReader read = new StringReader(xmlString)) {
                //    Type outType = typeof(T);

                //    XmlSerializer serializer = new XmlSerializer(outType);
                //    using (XmlReader reader = new XmlTextReader(read)) {
                //        objectOut = (T)serializer.Deserialize(reader);
                //        reader.Close();
                //    }

                //    read.Close();
                //}
            } catch (Exception ex) {
                logger.Error(ex);
            }

            return objectOut;
        }
    }
}
