using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace RunningContext {
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
                    serializer.Formatting = Formatting.Indented;
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
            } catch (Exception ex) {
                logger.Error(ex);
            }

            return objectOut;
        }
    }
}
