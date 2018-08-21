using Model;
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

                if (input[0] == "r") {
                    CheckInputLengthgAndCallFunction(Races, Races, input, 3);
                    // TODO: race menu
                }


                if (input[0] == "l") {
                    // TODO: Load menu (old races, data for new race, ...)
                }

                if (input[0] == "c") {
                    // TODO: combine menu
                }

                if (input[0] == "e") {
                    // TODO: edit menu
                }

                if (input[0] == "cat") {
                    CheckInputLengthgAndCallFunction(Categories, Categories, input, 5);
                }

                if (input[0] == "cla") {
                    CheckInputLengthgAndCallFunction(Classes, Classes, input, 5);
                }

                PrintMainMenu();
            } while (true);
        }


        #region Category

        private void Categories(string[] input) {
            if (input[0] == "s") {
                DoAction(Category.ShowCategories);
                return;
            }

            if (input.Length < 2) {
                System.Console.WriteLine("Invalid input, redirect to Categories Menu");
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

            System.Console.WriteLine("Invalid input, redirect to Categories Menu");
            Categories();
        }


        private void Categories() {
            PrintCategoriesMenu();

            do {
                var input = ReadTrimAndSplit();

                if (input[0] == "q" || input[0] == "quit") {
                    break;
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

                PrintClassesMenu();
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
                System.Console.WriteLine("Invalid input, redirect to Classes Menu");
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

            System.Console.WriteLine("Invalid input, redirect to Classes Menu");
            Classes();
        }


        private void Classes() {
            PrintClassesMenu();

            do {
                var input = ReadTrimAndSplit();

                if (input[0] == "q" || input[0] == "quit") {
                    break;
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

                PrintClassesMenu();
            } while (true);
        }

        #endregion

        #region Race

        private void Races(string[] input) {

        }


        private void Races() {
            PrintRacesMenu();

            do {
                var input = ReadTrimAndSplit();

                if (input[0] == "q" || input[0] == "quit") {
                    break;
                }

                if (input[0] == "c") {
                    if (input.Length == 6) {
                        CurrentContext.Race = new Race(input.Skip(1).ToArray());
                    }
                }

                if (input[0] == "s") {
                    Race.Save("SaveTest");
                }

                if (input[0] == "l") {
                    Race.Load("SaveTest");
                }

                PrintRacesMenu();
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
            System.Console.WriteLine($"q: Quit Program \ncat: Change to Categories Menu \ncla: Change to Class Menu \nr: Read results from memory");
        }


        private void PrintCategoriesMenu() {
            System.Console.WriteLine($"q: Quit Categories Menu \na <CategorieName>: Add Category \nd <CategorieName>: Delete Category \ns: Show current Categories");
        }


        private void PrintClassesMenu() {
            System.Console.WriteLine($"q: Quit Classes Menu \na <ClassName>: Add Class \nd <ClassName>: Delete Class \ns: Show current Classes");
        }


        private void PrintRacesMenu() {
            System.Console.WriteLine($"q: Quit Races Menu \ns: Show current Race to file");
        }

        #endregion
    }
}
