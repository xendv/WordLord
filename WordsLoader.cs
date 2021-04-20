using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions; // for regex

namespace WordLord
{
    class WordsLoader
    {
        public string errorType = "";
        public string[] fileLines;
        public WordsLoader(DictionaryWindow ParentWindow)
        {
            string startupPath = Environment.CurrentDirectory;
            string fileName = @startupPath+"\\Dictionary.TXT";
            
            if (System.IO.File.Exists(fileName))
            {
                fileLines = File.ReadAllLines(fileName);
                if (!CheckFileContent())
                {
                    errorType = "Wrong Content";
                }
            }
            else errorType = "No File";
        }
        public bool CheckFileContent()
        {
            bool onlyAcceptableChars = true;
            foreach (string line in fileLines)
            {
                string pattern = @"^[а-я]+$"; //only russian letters
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                if (!regex.IsMatch(line))
                {
                    onlyAcceptableChars = false;
                    break;
                }
            }
            return onlyAcceptableChars;
        }

        public void BuildExistingWordsList()
        {

        }
    }
}
