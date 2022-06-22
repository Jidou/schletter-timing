using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;

namespace SchletterTiming.FileRepo {
    public class SaveLoad {
        private readonly string _fileRepoBasePath = $"{Environment.CurrentDirectory}\\Data";
        private readonly string _fileRepoRacesBasePath = $"{Environment.CurrentDirectory}\\Data\\Races";

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
            var path = _fileRepoBasePath;
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            path = _fileRepoRacesBasePath;
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            path = $"{_fileRepoBasePath}\\Participants.json";
            if (!File.Exists(path)) {
                File.Create(path);
                File.WriteAllText(path, "[]");
            }

            path = $"{_fileRepoBasePath}\\Groups.json";
            if (!File.Exists(path)) {
                File.Create(path);
                File.WriteAllText(path, "[]");
            }

            path = $"{_fileRepoBasePath}\\Categories.json";
            if (!File.Exists(path)) {
                File.Create(path);
                File.WriteAllText(path,"[]");
            }

            path = $"{_fileRepoBasePath}\\Classes.json";
            if (!File.Exists(path)) {
                File.Create(path);
                File.WriteAllText(path, "[]");
            }
        }


        public void SerializeObjectFullPath<T>(T serializableObject, string path) {
            if (serializableObject == null) {
                return;
            }

            if (!path.EndsWith(".json")) {
                path += ".json";
            }

            try {
                using var file = File.CreateText(path);
                var serializer = new JsonSerializer { Formatting = Formatting.Indented };
                serializer.Serialize(file, serializableObject);
            } catch (Exception ex) {
                logger.Error(ex);
            }
        }


        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public void SerializeObjectFilename<T>(T serializableObject, string fileName) {
            if (!fileName.StartsWith(_fileRepoBasePath)) {
                fileName = $"{_fileRepoBasePath}\\{fileName}";
            }

            SerializeObjectFullPath(serializableObject, fileName);
        }


        public T DeSerializeObjectFullPath<T>(string fileName) {
            if (string.IsNullOrEmpty(fileName)) {
                return default(T);
            }

            if (!fileName.EndsWith(".json")) {
                fileName += ".json";
            }

            var objectOut = default(T);

            try {
                using var file = File.OpenText(fileName);
                var serializer = new JsonSerializer();
                objectOut = (T)serializer.Deserialize(file, typeof(T));
            } catch (Exception ex) {
                logger.Error(ex);
            }

            return objectOut;

        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T DeSerializeObjectFilename<T>(string fileName) {
            if (!fileName.StartsWith($"{Environment.CurrentDirectory}\\Data")) {
                fileName = $"{Environment.CurrentDirectory}\\Data\\{fileName}";
            }

            return DeSerializeObjectFullPath<T>(fileName);
        }


        public void RemoveTmpFiles(string defaultRaceTitel) {
            if (Directory.Exists($"{_fileRepoRacesBasePath}\\{defaultRaceTitel}")) {
                Directory.Delete($"{_fileRepoRacesBasePath}\\{defaultRaceTitel}", true);
            }

            if (File.Exists($"{_fileRepoRacesBasePath}\\{defaultRaceTitel}.json")) {
                File.Delete($"{_fileRepoRacesBasePath}\\{defaultRaceTitel}.json");
            }
        }

        public string[] GetFileList(string racename) {
            Directory.CreateDirectory($"{_fileRepoRacesBasePath}\\{racename}");
            return Directory.GetFiles($"{_fileRepoRacesBasePath}\\{racename}");
        }
    }
}
