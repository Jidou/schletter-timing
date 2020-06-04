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


        public void AddParticipants(string[] input) {
            int.TryParse(input[0], out int groupId);
            var group = CurrentContext.AllAvailableGroups.SingleOrDefault(x => x.Groupname == input[0] || x.GroupId == groupId);

            if (group == null) {
                logger.Info($"Unable to find group {input[0]}");
                return;
            }

            var part1Ident = input[1];
            var part1 = CurrentContext.AllAvailableParticipants.SingleOrDefault(x => $"{x.Firstname}_{x.Lastname}" == part1Ident);

            if (part1 == null) {
                logger.Info($"Unable to find participant {input[1]}");
                return;
            }

            var part2Ident = input[2];
            var part2 = CurrentContext.AllAvailableParticipants.SingleOrDefault(x => $"{x.Firstname}_{x.Lastname}" == part2Ident);

            if (part1 == null) {
                logger.Info($"Unable to find participant {input[2]}");
                return;
            }

            group.Participant1 = part1;
            group.Participant2 = part2;

            logger.Info($"Group successfully updated");
        }


        public void Save() {
            var allParticipants = CurrentContext.AllAvailableGroups;
            _repo.SerializeObject(allParticipants, SaveFileName);
        }


        public IEnumerable<Group> Load() {
            var groups = (IEnumerable<Group>)CurrentContext.AllAvailableGroups;
            if (groups != null) {
                return groups;
            }

            groups = _repo.DeSerializeObject<IEnumerable<Group>>(SaveFileName);

            if (groups != null) {
                CurrentContext.AllAvailableGroups = groups.ToList();
                return groups;
            }

            groups = new List<Group>();
            CurrentContext.AllAvailableGroups = groups.ToList();
            return groups;
        }
    }
}
