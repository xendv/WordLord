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
        DictionaryWindow parentWindow;
        MainWindow parentWindowMain;
        public string errorType = "";
        public List<Word> wordsList;
        public WordsLoader(DictionaryWindow ParentWindow)
        {
            this.wordsList = new List<Word>();
            this.parentWindow = ParentWindow;
            CheckFileExistence();
        }
        public WordsLoader(MainWindow ParentWindowMain)
        {
            this.wordsList = new List<Word>();
            this.parentWindowMain = ParentWindowMain;
            CheckFileExistence();
        }
        public void CheckFileExistence()
        {
            string[] fileLines;
            string startupPath = Environment.CurrentDirectory;
            string fileName = @startupPath + "\\Dictionary.TXT";

            if (System.IO.File.Exists(fileName))
            {
                fileLines = File.ReadAllLines(fileName);
                if (!CheckFileContent(fileLines))
                {
                    errorType = "Wrong Content";
                }
            }
            else errorType = "No File";
        }
        public bool CheckFileContent(string[] fileLines)
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
                else
                {
                    wordsList.Add(new Word(line));
                }
            }
            return onlyAcceptableChars;
        }

        public ref List<Word> GetWords()
        {
            return ref wordsList;
            //parentWindow.WordsList.DataContext = wordsList;
            //parentWindow.WordsList.ItemsSource = wordsList;
            //throw new NotImplementedException();
        }

        public void PrintExistingWordsList()
        {

            throw new NotImplementedException();
        }
    }
}
