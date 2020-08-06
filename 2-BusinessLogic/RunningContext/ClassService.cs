using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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


        public IEnumerable<AvailableClass> LoadClasses() {
            return _repo.DeSerializeObjectFilename<IEnumerable<AvailableClass>>(SaveFileName);
        }


        public void AddClass(string newClass) {
            var availableClasses = LoadClasses();

            if (availableClasses.Any(x => x.ClassName == newClass)) {
                return;
            }

            var nextId = 0;

            if (availableClasses.Any()) {
                nextId = availableClasses.Max(x => x.ClassId) + 1;
            }

            var availableClassesAsList = availableClasses.ToList();

            availableClassesAsList.Add(new AvailableClass {
                ClassId = nextId,
                ClassName = newClass
            });

            _repo.SerializeObjectFilename(availableClassesAsList, SaveFileName);
        }


        public void UpdateClass(AvailableClass @class) {
            var currentClasses = LoadClasses();
            var oldClass = currentClasses.SingleOrDefault(x => x.ClassId == @class.ClassId);

            if (oldClass is null) {
                // TODO: Error handling
                return;
            }

            oldClass.ClassName = @class.ClassName;
            _repo.SerializeObjectFilename(currentClasses, SaveFileName);
        }


        public void DeleteClass(string @class) {
            var availableClasses = LoadClasses();
            var availableClassesAsList = availableClasses.ToList();
            var classToDelete = availableClassesAsList.Find(x => x.ClassName == @class);

            if (classToDelete is null) {
                return;
            }

            availableClassesAsList.Remove(classToDelete);
            _repo.SerializeObjectFilename(availableClassesAsList, SaveFileName);
        }


        public void ShowClasses() {
            var availableClasses = LoadClasses();
            var allClasses = availableClasses.Aggregate("", (current, @class) => current + $"{@class.ClassName}\n");
            Console.WriteLine(allClasses);
        }
    }
}
