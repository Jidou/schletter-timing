using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;

namespace SchletterTiming.FileRepo {
    public class SaveLoad {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _configuration;


        public SaveLoad(IConfiguration configuration) {
            _configuration = configuration;
        }


        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public void SerializeObject<T>(T serializableObject, string fileName) {
            if (serializableObject == null) {
                return;
            }

            if (!fileName.EndsWith(".json")) {
                fileName += ".json";
            }

            fileName = $"{Environment.CurrentDirectory}/{_configuration["SaveFileDirectory"]}/{fileName}";

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
        public T DeSerializeObject<T>(string fileName) {
            if (string.IsNullOrEmpty(fileName)) {
                return default(T);
            }

            if (!fileName.EndsWith(".json")) {
                fileName += ".json";
            }

            fileName = $"{Environment.CurrentDirectory}/{_configuration["SaveFileDirectory"]}/{fileName}";

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
