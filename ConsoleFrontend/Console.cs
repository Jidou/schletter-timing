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
                var input = ReadTrimAndSplit();

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintMainMenu();
                    continue;
                }

                if (input[0] == "r") {
                    CheckInputLengthgAndCallFunction(Races, Races, input, 3);
                    continue;
                    // TODO: race menu
                }

                if (input[0] == "p") {
                    CheckInputLengthgAndCallFunction(Participants, Participants, input, 3);
                    continue;
                    // TODO: participants menu
                }

                if (input[0] == "g") {
                    CheckInputLengthgAndCallFunction(Groups, Groups, input, 3);
                    continue;
                    // TODO: groups menu
                }

                if (input[0] == "t") {
                    continue;
                    // TODO: timing data menu
                }

                if (input[0] == "cat") {
                    CheckInputLengthgAndCallFunction(Categories, Categories, input, 5);
                    continue;
                }

                if (input[0] == "cla") {
                    CheckInputLengthgAndCallFunction(Classes, Classes, input, 5);
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
                var input = ReadTrimAndSplit();

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintCategoriesMenu();
                    continue;
                }

                if (input[0] == "s") {
                    DoAction(Category.ShowCategories);
                }

                if (input[0] == "a") {
                    DoAction(Category.AddCategory, input.Skip(1));
                }

                if (input[0] == "d") {
                    DoAction(Category.DeleteCategory, input.Skip(1));
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
                var input = ReadTrimAndSplit();

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintClassesMenu();
                    continue;
                }

                if (input[0] == "s") {
                    DoAction(Class.ShowClasses);
                }

                if (input[0] == "a") {
                    DoAction(Class.AddClass, input.Skip(1));
                }

                if (input[0] == "d") {
                    DoAction(Class.DeleteClass, input.Skip(1));
                }
            } while (true);
        }

        #endregion

        #region Race

        private void Races(string[] input) {
            if (input.Length < 2) {
                logger.Info("Invalid input, redirect to Races Menu");
                Races();
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

            if (input[0] == "c") {
                TryCreateNewRace(input.Skip(1).ToArray());
            }
        }


        private void Races() {
            PrintRacesMenu();

            do {
                var input = ReadTrimAndSplit();

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintRacesMenu();
                    continue;
                }

                if (input[0] == "s") {
                    DoAction(RunningContext.Race.Save, input.Skip(1));
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
                }

                if (input[0] == "ag") {
                    RunningContext.Race.AddGroup(input.Skip(1).ToArray());
                    return;
                }

                if (input[0] == "c") {
                    TryCreateNewRace(input.Skip(1).ToArray());
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
                var input = ReadTrimAndSplit();

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintParticipantsMenu();
                    continue;
                }

                if (input[0] == "c") {
                    TryCreateNewParticipant(input.Skip(1).ToArray());
                }

                if (input[0] == "s") {
                    DoAction(RunningContext.Participant.Save, input.Skip(1));
                }

                if (input[0] == "l") {
                    DoAction(RunningContext.Participant.Load, input.Skip(1));
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
                var input = ReadTrimAndSplit();

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintGroupsMenu();
                    continue;
                }

                if (input[0] == "c") {
                    TryCreateNewGroup(input.Skip(1).ToArray());
                }

                if (input[0] == "s") {
                    DoAction(RunningContext.Group.Save, input.Skip(1));
                }

                if (input[0] == "l") {
                    DoAction(RunningContext.Group.Load, input.Skip(1));
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
            if (input.Length < 2) {
                logger.Info("Invalid input, redirect to Groups Menu");
                Timing();
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
                Timing();
                return;
            }

            if (input[0] == "ap") {
                RunningContext.Group.AddParticipants(input.Skip(1).ToArray());
            }
        }


        private void Timing() {
            PrintTimingMenu();

            do {
                var input = ReadTrimAndSplit();

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "h") {
                    PrintTimingMenu();
                    continue;
                }

                if (input[0] == "rm") {
                    TryCreateNewGroup(input.Skip(1).ToArray());
                }

                if (input[0] == "s") {
                    DoAction(RunningContext.Group.Save, input.Skip(1));
                }

                if (input[0] == "l") {
                    DoAction(RunningContext.Group.Load, input.Skip(1));
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


        private string[] ReadTrimAndSplit() {
            return System.Console.ReadLine()
                .Split(' ')
                .Select(x => x.Trim())
                .ToArray();
        }

        #endregion

        #region PrintMenues

        private void PrintMainMenu() {
            logger.Info($"q: Quit Program \ncat: Change to Categories Menu \ncla: Change to Class Menu \nr: Read results from memory");
        }


        private void PrintCategoriesMenu() {
            logger.Info($"q: Quit Categories Menu \na <CategorieName>: Add Category \nd <CategorieName>: Delete Category \ns: Show current Categories");
        }


        private void PrintClassesMenu() {
            logger.Info($"q: Quit Classes Menu \na <ClassName>: Add Class \nd <ClassName>: Delete Class \ns: Show current Classes");
        }


        private void PrintRacesMenu() {
            logger.Info($"q: Quit Races Menu \ns: Show current Race to file");
        }


        private void PrintParticipantsMenu() {
            logger.Info($"q: Quit Races Menu \ns: Show current Race to file");
        }


        private void PrintGroupsMenu() {
            logger.Info($"q: Quit Races Menu \ns: Show current Race to file");
        }


        private void PrintTimingMenu() {
            logger.Info($"q: Quit Races Menu \ns: Show current Race to file");
        }

        #endregion
    }
}
