using System;
using System.Collections.Generic;

namespace SchletterTiming.ConsoleFrontend {
    public class MenuActionDictionary {

        public static readonly Dictionary<string, Dictionary<string, Action>> MenuAction = GenerateDict();

        public static Dictionary<string, Dictionary<string, Action>> GenerateDict() {
            var outerDict = GenerateMenuDictDict();

            FillMainMenuDict(outerDict["m"]);
            FillRaceDict(outerDict["r"]);

            return outerDict;
        }


        private static Dictionary<string, Dictionary<string, Action>> GenerateMenuDictDict() {
            var dict = new Dictionary<string, Dictionary<string, Action>>();

            //dict.Add("m", new Dictionary<string, Action>());
            dict.Add("r", new Dictionary<string, Action>());
            dict.Add("cat", new Dictionary<string, Action>());
            dict.Add("cla", new Dictionary<string, Action>());
            //dict.Add("m", new Dictionary<string, Action>());

            return dict;
        }


        private static void FillRaceDict(Dictionary<string, Action> dictionary) {
            //dictionary.Add("s", new Model.Race);
        }


        private static void FillMainMenuDict(Dictionary<string, Action> dictionary) {
            //dictionary.Add("r", Console.CheckInputLengthgAndCallFunction);
        }
    }
}
