using Model;
using RunningContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ConsoleFrontend {
    public class Console {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        
        public void Start() {
            PrintMainMenu();
                
            do {
                var input = ReadTrimAndSplit("Main");

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintMainMenu();
                    continue;
                }

                if (input[0] == "r") {
                    CheckInputLengthgAndCallFunction(Race, Race, input, 3);
                    continue;
                }

                if (input[0] == "f") {
                    CheckInputLengthgAndCallFunction(Finish, Finish, input, 3);
                    continue;
                }

                if (input[0] == "p") {
                    CheckInputLengthgAndCallFunction(Participants, Participants, input, 3);
                    continue;
                }

                if (input[0] == "g") {
                    CheckInputLengthgAndCallFunction(Groups, Groups, input, 3);
                    continue;
                }

                if (input[0] == "t") {
                    CheckInputLengthgAndCallFunction(Timing, Timing, input, 3);
                    continue;
                }

                if (input[0] == "cat") {
                    CheckInputLengthgAndCallFunction(Categories, Categories, input, 3);
                    continue;
                }

                if (input[0] == "cla") {
                    CheckInputLengthgAndCallFunction(Classes, Classes, input, 3);
                    continue;
                }
            } while (true);
        }


        #region Category

        private void Categories(string[] input) {
            if (input[0] == "s") {
                DoAction(Category.ShowCategories);
                return;
            }

            if (input.Length < 2) {
                logger.Info("Invalid input, redirect to Categories Menu");
                Categories();
                return;
            }

            if (input[0] == "a") {
                DoAction(Category.AddCategory, input.Skip(1));
                return;
            }

            if (input[0] == "d") {
                DoAction(Category.DeleteCategory, input.Skip(1));
                return;
            }

            logger.Info("Invalid input, redirect to Categories Menu");
            Categories();
        }


        private void Categories() {
            PrintCategoriesMenu();

            do {
                var input = ReadTrimAndSplit("Categories");

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintCategoriesMenu();
                    continue;
                }

                if (input[0] == "s") {
                    DoAction(Category.ShowCategories);
                    continue;
                }

                if (input[0] == "a") {
                    DoAction(Category.AddCategory, input.Skip(1));
                    continue;
                }

                if (input[0] == "d") {
                    DoAction(Category.DeleteCategory, input.Skip(1));
                    continue;
                }
            } while (true);
        }

        #endregion

        #region Class

        private void Classes(string[] input) {
            if (input[0] == "s") {
                DoAction(Class.ShowClasses);
                return;
            }

            if (input.Length < 2) {
                logger.Info("Invalid input, redirect to Classes Menu");
                Classes();
                return;
            }

            if (input[0] == "a") {
                DoAction(Class.AddClass, input.Skip(1));
                return;
            }

            if (input[0] == "d") {
                DoAction(Class.DeleteClass, input.Skip(1));
                return;
            }

            logger.Info("Invalid input, redirect to Classes Menu");
            Classes();
        }


        private void Classes() {
            PrintClassesMenu();

            do {
                var input = ReadTrimAndSplit("Classes");

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintClassesMenu();
                    continue;
                }

                if (input[0] == "s") {
                    DoAction(Class.ShowClasses);
                    continue;
                }

                if (input[0] == "a") {
                    DoAction(Class.AddClass, input.Skip(1));
                    continue;
                }

                if (input[0] == "d") {
                    DoAction(Class.DeleteClass, input.Skip(1));
                    continue;
                }
            } while (true);
        }

        #endregion

        #region Race

        private void Race(string[] input) {
            if (input.Length < 2) {
                logger.Info("Invalid input, redirect to Races Menu");
                Race();
                return;
            }

            if (input[0] == "s") {
                DoAction(RunningContext.Race.Save, input.Skip(1));
                return;
            }

            if (input[0] == "sp") {
                DoAction(RunningContext.Participant.SaveFromRace, input.Skip(1));
                return;
            }

            if (input[0] == "sg") {
                DoAction(RunningContext.Group.SaveFromRace, input.Skip(1));
                return;
            }

            if (input[0] == "l") {
                DoAction(RunningContext.Race.Load, input.Skip(1));
                return;
            }

            if (input[0] == "t") {
                DoAction(RunningContext.Race.SetStartTime, input.Skip(1));
                return;
            }

            if (input[0] == "ag") {
                RunningContext.Race.AddGroup(input.Skip(1).ToArray());
                return;
            }

            if (input[0] == "at") {
                RunningContext.Race.AddTimingValues();
                return;
            }

            if (input[0] == "c") {
                TryCreateNewRace(input.Skip(1).ToArray());
            }
        }


        private void Race() {
            PrintRacesMenu();

            do {
                var input = ReadTrimAndSplit("Race");

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintRacesMenu();
                    continue;
                }

                if (input[0] == "s") {
                    DoAction(RunningContext.Race.Save, input.Skip(1));
                    continue;
                }

                if (input[0] == "sp") {
                    DoAction(RunningContext.Participant.SaveFromRace, input.Skip(1));
                    continue;
                }

                if (input[0] == "sg") {
                    DoAction(RunningContext.Group.SaveFromRace, input.Skip(1));
                    continue;
                }

                if (input[0] == "l") {
                    DoAction(RunningContext.Race.Load, input.Skip(1));
                    continue;
                }

                if (input[0] == "ag") {
                    RunningContext.Race.AddGroup(input.Skip(1).ToArray());
                    continue;
                }

                if (input[0] == "at") {
                    RunningContext.Race.AddTimingValues();
                    continue;
                }

                if (input[0] == "c") {
                    TryCreateNewRace(input.Skip(1).ToArray());
                    continue;
                }
            } while (true);
        }


        private void TryCreateNewRace(string[] input) {
            if (input.Length == 5) {
                CurrentContext.Race = new Model.Race(input);
                logger.Info("Race created");
                return;
            }

            logger.Info("Could not create race, wrong number of arguments");
        }

        #endregion

        #region Participants

        private void Participants(string[] input) {
            if (input.Length < 2) {
                logger.Info("Invalid input, redirect to Races Menu");
                Participants();
                return;
            }

            if (input[0] == "s") {
                DoAction(RunningContext.Participant.Save, input.Skip(1));
                return;
            }

            if (input[0] == "l") {
                DoAction(RunningContext.Participant.Load, input.Skip(1));
                return;
            }

            if (input[0] == "c") {
                TryCreateNewParticipant(input.Skip(1).ToArray());
            }
        }


        private void Participants() {
            PrintParticipantsMenu();

            do {
                var input = ReadTrimAndSplit("Participants");

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintParticipantsMenu();
                    continue;
                }

                if (input[0] == "c") {
                    TryCreateNewParticipant(input.Skip(1).ToArray());
                    continue;
                }

                if (input[0] == "s") {
                    DoAction(RunningContext.Participant.Save, input.Skip(1));
                    continue;
                }

                if (input[0] == "l") {
                    DoAction(RunningContext.Participant.Load, input.Skip(1));
                    continue;
                }
            } while (true);
        }


        private void TryCreateNewParticipant(string[] input) {
            if (input.Length == 4) {
                if (CurrentContext.AllAvailableParticipants == null) {
                    CurrentContext.AllAvailableParticipants = new List<Model.Participant>();
                }

                CurrentContext.AllAvailableParticipants.Add(new Model.Participant(input));
                logger.Info("Added new Participant");
                return;
            }

            logger.Info("Could not add participant, wrong number of arguments");
        }

        #endregion

        #region Groups

        private void Groups(string[] input) {
            if (input.Length < 2) {
                logger.Info("Invalid input, redirect to Groups Menu");
                Groups();
                return;
            }

            if (input[0] == "s") {
                DoAction(RunningContext.Group.Save, input.Skip(1));
                return;
            }

            if (input[0] == "l") {
                DoAction(RunningContext.Group.Load, input.Skip(1));
                return;
            }

            if (input[0] == "c") {
                TryCreateNewGroup(input.Skip(1).ToArray());
            }

            if (input.Length < 3) {
                logger.Info("Invalid input, redirect to Groups Menu");
                Groups();
                return;
            }

            if (input[0] == "ap") {
                RunningContext.Group.AddParticipants(input.Skip(1).ToArray());
            }
        }


        private void Groups() {
            PrintGroupsMenu();

            do {
                var input = ReadTrimAndSplit("Groups");

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintGroupsMenu();
                    continue;
                }

                if (input[0] == "c") {
                    TryCreateNewGroup(input.Skip(1).ToArray());
                    continue;
                }

                if (input[0] == "s") {
                    DoAction(RunningContext.Group.Save, input.Skip(1));
                    continue;
                }

                if (input[0] == "l") {
                    DoAction(RunningContext.Group.Load, input.Skip(1));
                    continue;
                }

                if (input[0] == "ap") {
                    RunningContext.Group.AddParticipants(input.Skip(1).ToArray());
                    continue;
                }
            } while (true);
        }


        private void TryCreateNewGroup(string[] input) {
            if (input.Length == 3) {
                if (CurrentContext.AllAvailableGroups == null) {
                    CurrentContext.AllAvailableGroups = new List<Model.Group>();
                }

                CurrentContext.AllAvailableGroups.Add(new Model.Group(input));
                logger.Info("Added new Group");
                return;
            }

            logger.Info("Could not add group, wrong number of arguments");
        }

        #endregion

        #region Timing

        private void Timing(string[] input) {
            if (input[0] == "rm") {
                var memoryDump = CurrentContext.Reader.WaitForBulk();
                CurrentContext.Timing = memoryDump;
                RunningContext.TimingValue.Save();
                return;
            }

            if (input.Length < 2) {
                logger.Info("Invalid input, redirect to Timing Menu");
                Timing();
                return;
            }

            if (input[0] == "s") {
                DoAction(RunningContext.TimingValue.Save);
                return;
            }

            if (input[0] == "l") {
                DoAction(RunningContext.TimingValue.Load, input.Skip(1));
                return;
            }

            if (input.Length < 3) {
                logger.Info("Invalid input, redirect to Timing Menu");
                Timing();
                return;
            }
        }


        private void Timing() {
            PrintTimingMenu();

            do {
                var input = ReadTrimAndSplit("Timing");

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintTimingMenu();
                    continue;
                }

                if (input[0] == "rm") {
                    var memoryDump = CurrentContext.Reader.WaitForBulk();
                    CurrentContext.Timing = memoryDump;
                    RunningContext.TimingValue.Save();
                    continue;
                }

                if (input[0] == "s") {
                    DoAction(RunningContext.TimingValue.Save);
                    continue;
                }

                if (input[0] == "l") {
                    DoAction(RunningContext.TimingValue.Load, input.Skip(1));
                    continue;
                }
            } while (true);
        }

        #endregion

        #region Finish

        private void Finish(string[] input) {

        }


        private void Finish() {
            PrintFinishMenu();

            do {
                var input = ReadTrimAndSplit("Finish");

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintFinishMenu();
                    continue;
                }

                if (input[0] == "c") {
                    DoAction(RunningContext.Race.CalculateFinishTimes);
                }

                if (input[0] == "pg") {
                    PdfGenerator.PdfRenderer.GroupByClassAndOrder(CurrentContext.Race);
                }

                if (input[0] == "pa") {
                    PdfGenerator.PdfRenderer.Order(CurrentContext.Race);
                }

                if (input[0] == "sl") {
                    PdfGenerator.PdfRenderer.CreateStartList(CurrentContext.Race);
                }

            } while (true);

        }

        #endregion

        #region Helpers

        private void CheckInputLengthgAndCallFunction(Action simpleAction, Action<string[]> actionWithParameters, string[] input, int requiredLength) {
            if (input.Length >= 2) {
                actionWithParameters(input.Skip(1).ToArray());
                return;
            } else {
                simpleAction();
                return;
            }
        }


        private void DoAction(Action action) {
            logger.Debug($"Executing action: {action.Method}");
            action();
        }


        private void DoAction(Action<string> action, IEnumerable<string> elements) {
            logger.Debug($"Executing action: {action.Method} with {elements.Count()} parameters");
            foreach (var element in elements) {
                action(element);
            }
        }


        private string[] ReadTrimAndSplit(string currentMenu) {
            System.Console.Write($"{currentMenu}:");
            return System.Console.ReadLine()
                .Split(' ')
                .Select(x => x.Trim())
                .ToArray();
        }

        #endregion

        #region PrintMenues

        private void PrintMainMenu() {
            logger.Info(@"
q: Quit Program 
h: Show this text
r: Change to Race Menu
p: Chnage to Participants Menu
g: Change to Groups Menu
t: Change to Timing Menu
cla: Change to Class Menu 
cat: Change to Categories Menu");
        }


        private void PrintCategoriesMenu() {
            logger.Info(@"
q: Quit Categories Menu
a <CategorieName>: Add Category
d <CategorieName>: Delete Category
s: Show current Categories");
        }


        private void PrintClassesMenu() {
            logger.Info(@"
q: Quit Classes Menu
a <ClassName>: Add Class
d <ClassName>: Delete Class
s: Show current Classes");
        }


        private void PrintRacesMenu() {
            logger.Info(@"
q: Quit Menu
h: Show this text
s <Filename>: Save current Race to file
sp <Filename>: Save all Participants in current Race to file
sg <Filename>: Save all Groups in current Race to file
l <Filename>: Load race from file
t <Time>: Set the start time of the race
ag <Group1> [Group2 Group3 ...]: Adds Groups to current race
at: Combines the Timing values with the Groups by matching the groupnumbers
c <RaceType Titel Date Place Judge>: Creates a new Race with the given Parameter");
        }


        private void PrintFinishMenu() {
            logger.Info(@"
q: Quit Menu
h: Show this text
c: (re)calculate finishing times of all groups
pg: group by group.class and print ordered result to pdf
pa: order result and print to pdf");
        }


        private void PrintParticipantsMenu() {
            logger.Info(@"
q: Quit Menu
h: Show this text
s <Filename>: Save all participants to file
l <Filename>: Load participants from file
c <Firstname Lastname YearOfBirth Class>: Create new Participant with the given parameters");
        }


        private void PrintGroupsMenu() {
            logger.Info(@"
q: Quit Menu
h: Show this text
s <Filename>: Save all groups to file
l <Filename>: Load groups from file
c <Groupname Groupnumber Category>: Creates new group with the given parameters
ap <(Groupname|Groupnumber) Participant1 Participant2>: Adds Participants to groups");
        }


        private void PrintTimingMenu() {
            logger.Info(@"
q: Quit Menu
h: Show this text
s: Save current Timing Values to a new file
l <Filename>: Load Timing Values from file
rm: Start waiting for memory dump from Timy");
        }

        #endregion
    }
}
