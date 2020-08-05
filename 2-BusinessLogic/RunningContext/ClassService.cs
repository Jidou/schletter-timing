using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SchletterTiming.FileRepo;
using SchletterTiming.Model;

namespace SchletterTiming.RunningContext {
    public class ClassService {

        private const string SaveFileName = "Classes";

        private readonly IConfiguration _configuration;
        private readonly SaveLoad _repo;


        public ClassService(IConfiguration configuration, SaveLoad repo) {
            _configuration = configuration;
            _repo = repo;
        }


        public AvailableClasses LoadClasses() {
            return _repo.DeSerializeObject<AvailableClasses>(SaveFileName);
        }


        public void AddClass(string newClass) {
            var availableClasses = LoadClasses();
            var availableClassesAsList = availableClasses.Classes.ToList();
            availableClassesAsList.Add(newClass);
            availableClasses.Classes = availableClassesAsList;
            _repo.SerializeObject(availableClasses, SaveFileName);
        }


        public void DeleteClass(string @class) {
            var availableClasses = LoadClasses();
            var availableClassesAsList = availableClasses.Classes.ToList();
            availableClassesAsList.Remove(@class);
            availableClasses.Classes = availableClassesAsList;
            _repo.SerializeObject(availableClasses, SaveFileName);
        }


        public void ShowClasses() {
            var availableClasses = LoadClasses().Classes;
            var allClasses = availableClasses.Aggregate("", (current, @class) => current + $"{@class}\n");
            Console.WriteLine(allClasses);
        }
    }
}
