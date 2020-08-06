using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NLog;
using SchletterTiming.FileRepo;
using SchletterTiming.Model;

namespace SchletterTiming.RunningContext {
    public class GroupService {
        private const string SaveFileName = "Groups";

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IConfiguration _configuration;
        private readonly SaveLoad _repo;


        public GroupService(IConfiguration configuration, SaveLoad repo) {
            _configuration = configuration;
            _repo = repo;
        }


        public IEnumerable<Group> LoadAllAvailableGroups() {
            return _repo.DeSerializeObjectFilename<IEnumerable<Group>>(SaveFileName);
        }


        public Group AddGroup(Group newGroup) {
            var allGroups = LoadAllAvailableGroups();
            var nextGroupId = allGroups.Max(x => x.GroupId) + 1;
            newGroup.GroupId = nextGroupId;
            var allGroupsAsList = allGroups.ToList();
            allGroupsAsList.Add(newGroup);
            _repo.SerializeObjectFilename(allGroupsAsList, SaveFileName);
            return newGroup;
        }


        public Group Update(Group groupToUpdate) {
            var allGroups = LoadAllAvailableGroups();
            var allGroupsAsList = allGroups.ToList();
            var oldGroup = allGroupsAsList.Find(x => x.GroupId == groupToUpdate.GroupId);
            allGroupsAsList[allGroupsAsList.IndexOf(oldGroup)] = groupToUpdate;
            _repo.SerializeObjectFilename(allGroupsAsList, SaveFileName);
            return groupToUpdate;
        }


        public Group LoadGroupById(int groupId) {
            return LoadAllAvailableGroups().SingleOrDefault(x => x.GroupId == groupId);
        }
    }
}
