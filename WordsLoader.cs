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
        //DictionaryWindow parentWindow;
        //MainWindow parentWindowMain;
        public string errorType = "";
        public List<Word> wordsList;

        public WordsLoader(DictionaryWindow ParentWindow)
        {
            this.wordsList = new List<Word>();
            //this.parentWindow = ParentWindow;
            CheckFileExistence();
        }

        public WordsLoader(MainWindow ParentWindowMain)
        {
            this.wordsList = new List<Word>();
            //this.parentWindowMain = ParentWindowMain;
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
            int currID = 0;
            bool onlyAcceptableChars = true;
            foreach (string line in fileLines)
            {
                string pattern = @"^[а-я]+$"; //only russian letters
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                if (!regex.IsMatch(line))
                {
                    wordsList.RemoveRange(0, wordsList.Count());
                    onlyAcceptableChars = false;
                    break;
                }
                else
                {
                    wordsList.Add(new Word(line));
                    wordsList[currID].WordID = currID;
                    currID++;
                }
            }
            return onlyAcceptableChars;
        }

        public ref List<Word> GetWords()
        {
            SortWordsByAlphabet();
            return ref wordsList;
            //parentWindow.WordsList.DataContext = wordsList;
            //parentWindow.WordsList.ItemsSource = wordsList;
            //throw new NotImplementedException();
        }

        public string GetRandomWord()
        {
            Random random = new Random();
            int c = wordsList.Count();
            int w = random.Next(wordsList.Count());
            return wordsList[w].WordFull;
        }
        public void SortWordsByAlphabet()
        {
            wordsList.Sort(WordsComparisonByAlphabet);
        }

        /// <summary>
        /// Возвращает -1, если значение первого слова идет раньше по алфавиту,
        /// 0 если значения равны и 1, если значение первого слова идет позже по алфавиту
        /// </summary>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns></returns>
        private int WordsComparisonByAlphabet(Word word1, Word word2)
        {
            return String.Compare(word1.WordFull, word2.WordFull);
        }
    }
}
