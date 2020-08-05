using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;

namespace SchletterTiming.FileRepo {
    public class SaveLoad {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        // TODO: fix getting SaveFileDirectory from configuration
        private readonly IConfiguration _configuration;


        public SaveLoad(IConfiguration configuration) {
            _configuration = configuration;
        }


        /// <summary>
        /// Checks and creates all files and directories needed
        /// </summary>
        public void Init() {
            var path = $"{Environment.CurrentDirectory}\\Data";
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            path = $"{Environment.CurrentDirectory}\\Data\\Races";
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            path = $"{Environment.CurrentDirectory}\\Data\\Participants.json";
            if (!File.Exists(path)) {
                File.Create(path);
                File.WriteAllText(path, "[]");
            }

            path = $"{Environment.CurrentDirectory}\\Data\\Groups.json";
            if (!File.Exists(path)) {
                File.Create(path);
                File.WriteAllText(path, "[]");
            }

            path = $"{Environment.CurrentDirectory}\\Data\\Categories.json";
            if (!File.Exists(path)) {
                File.Create(path);
                File.WriteAllText(path,"[]");
            }

            path = $"{Environment.CurrentDirectory}\\Data\\Classes.json";
            if (!File.Exists(path)) {
                File.Create(path);
                File.WriteAllText(path, "[]");
            }
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

            if (!fileName.StartsWith($"{Environment.CurrentDirectory}\\Data")) {
                fileName = $"{Environment.CurrentDirectory}\\Data\\{fileName}";
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
        public T DeSerializeObject<T>(string fileName) {
            if (string.IsNullOrEmpty(fileName)) {
                return default(T);
            }

            if (!fileName.EndsWith(".json")) {
                fileName += ".json";
            }

            if (!fileName.StartsWith($"{Environment.CurrentDirectory}\\Data")) {
                fileName = $"{Environment.CurrentDirectory}\\Data\\{fileName}";
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
