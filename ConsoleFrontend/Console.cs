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
                var input = System.Console.ReadLine();

                if (input == "q" || input == "quit") {
                    break;
                }

                if (input.StartsWith("l")) {
                    // TODO: Load menu (old races, data for new race, ...)
                }

                if (input.StartsWith("c")) {
                    // TODO: combine menu
                }

                if (input.StartsWith("e")) {
                    // TODO: edit menu
                }

                if (input.StartsWith("cat")) {
                    if (input.Length >= 5) {
                        Categories(input);
                        continue;
                    } else {
                        Categories();
                    }
                }

                if (input.StartsWith("cla")) {
                    if (input.Length >= 5) {
                        Classes(input);
                        continue;
                    } else {
                        Classes();
                    }
                }

                if (input.StartsWith("r")) {

                }

                PrintMainMenu();
            } while (true);
        }


        private void Categories(string input) {
            var inputSplit = input.Split(' ').Select(x => x.Trim()).ToArray();
            
            if (input == "cat s") {
                DoAction(Category.ShowCategories);
                return;
            }

            if (inputSplit.Length < 3) {
                System.Console.WriteLine("Invalid input, redirect to Categories Menu");
                Categories();
                return;
            }

            var nextInputValue = inputSplit[1];

            if (!(nextInputValue == "a" || nextInputValue == "d")) {
                System.Console.WriteLine("Invalid input, redirect to Categories Menu");
                Categories();
                return;
            }

            if (nextInputValue == "a") {
                DoAction(Category.AddCategory, inputSplit.Skip(2).Select(x => x.Trim()));
                return;
            }

            if (nextInputValue == "d") {
                DoAction(Category.DeleteCategory, inputSplit.Skip(2).Select(x => x.Trim()));
                return;
            }
        }


        private void Categories() {
            PrintCategoriesMenu();

            do {
                var input = System.Console.ReadLine();

                if (input == "q" || input == "quit") {
                    break;
                }

                if (input.StartsWith("a")) {
                    var splitInput = input.Split(' ');
                    // first one is the option select
                    DoAction(Category.AddCategory, splitInput.Skip(1).Select(x => x.Trim()));
                }

                if (input.StartsWith("d")) {
                    var splitInput = input.Split(' ');
                    // first one is the option select
                    DoAction(Category.DeleteCategory, splitInput.Skip(1).Select(x => x.Trim()));
                }

                if (input.StartsWith("s")) {
                    DoAction(Category.ShowCategories);
                }

                PrintClassesMenu();
            } while (true);
        }


        private void Classes(string input) {
            var inputSplit = input.Split(' ').Select(x => x.Trim()).ToArray();

            if (input == "cla s") {
                DoAction(Class.ShowClasses);
                return;
            }

            if (inputSplit.Length < 3) {
                System.Console.WriteLine("Invalid input, redirect to Classes Menu");
                Classes();
                return;
            }

            var nextInputValue = inputSplit[1];

            if (!(nextInputValue == "a" || nextInputValue == "d")) {
                System.Console.WriteLine("Invalid input, redirect to Classes Menu");
                Classes();
                return;
            }

            if (nextInputValue == "a") {
                DoAction(Class.AddClass, inputSplit.Skip(2).Select(x => x.Trim()));
                return;
            }

            if (nextInputValue == "d") {
                DoAction(Class.DeleteClass, inputSplit.Skip(2).Select(x => x.Trim()));
                return;
            }
        }


        private void Classes() {
            PrintClassesMenu();

            do {
                var input = System.Console.ReadLine();

                if (input == "q" || input == "quit") {
                    break;
                }

                if (input.StartsWith("a")) {
                    var splitInput = input.Split(' ');
                    // first one is the option select
                    DoAction(Class.AddClass, splitInput.Skip(1).Select(x => x.Trim()));
                }

                if (input.StartsWith("d")) {
                    var splitInput = input.Split(' ');
                    // first one is the option select
                    DoAction(Class.DeleteClass, splitInput.Skip(1).Select(x => x.Trim()));
                }

                if (input.StartsWith("s")) {
                    DoAction(Class.ShowClasses);
                }

                PrintClassesMenu();
            } while (true);
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


        private void PrintMainMenu() {
            System.Console.WriteLine($"q: Quit Program \ncat: Change to Categories Menu \ncla: Change to Class Menu \nr: Read results from memory");
        }


        private void PrintCategoriesMenu() {
            System.Console.WriteLine($"q: Quit Categories Menu \na <CategorieName>: Add Category \nd <CategorieName>: Delete Category \ns: Show current Categories");
        }


        private void PrintClassesMenu() {
            System.Console.WriteLine($"q: Quit Classes Menu \na <ClassName>: Add Class \nd <ClassName>: Delete Class \ns: Show current Classes");
        }
    }
}
