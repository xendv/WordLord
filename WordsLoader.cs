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
        string startupPath = Environment.CurrentDirectory;
        string fileName;

        public WordsLoader(DictionaryWindow ParentWindow)
        {
            this.wordsList = new List<Word>();
            //this.parentWindow = ParentWindow;
           
            this.fileName = @startupPath + "\\Dictionary.TXT";
            CheckFileExistence();
        }

        public WordsLoader(MainWindow ParentWindowMain)
        {
            this.wordsList = new List<Word>();
            //this.parentWindowMain = ParentWindowMain;
            this.fileName = @startupPath + "\\Dictionary.TXT";
            CheckFileExistence();
        }

        public bool CheckFileExistence()
        {
            string[] fileLines;

            if (System.IO.File.Exists(fileName))
            {
                fileLines = File.ReadAllLines(fileName);
                /*string[] words = new string[fileLines.Length];
                for (int wordIndex=0; wordIndex < fileLines.Length; wordIndex++)
                {
                    words[wordIndex] = fileLines[wordIndex];
                }*/
                
                if (!CheckFileContent(fileLines))
                {
                    errorType = "Wrong Content";
                    return false;
                }
            }
            else
            {
                errorType = "No File";
                return false;
            }
            return true;
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

        public bool CheckOnlyFileExistence()
        {
            if (System.IO.File.Exists(fileName))
            {
                return true;
            }
            else
            {
                errorType = "No File";
                return false;
            }
           
        }
        public void RewriteFileContent(List<Word> lWords)
        {
            if (CheckOnlyFileExistence())
            {
                string[] fileLines = new string[lWords.Count()];
                int wordIndex = 0;
                foreach (Word w in lWords)
                {
                    fileLines[wordIndex] = w.WordFull;
                    wordIndex++;
                }
                File.WriteAllText(fileName,"");
                File.WriteAllLines(fileName, fileLines);
            }
        }

        public bool HasSameContentAsCurrentList(List<Word> lWords)
        {
            return lWords.SequenceEqual(wordsList);
        }

        public ref List<Word> GetWords()
        {
            SortWordsByAlphabet();
            return ref wordsList;
        }

        public string GetRandomWord()
        {
            Random random = new Random();
            int c = wordsList.Count();
            int w = random.Next(wordsList.Count());
            return wordsList[w].WordFull;
        }

        // Sorting and Comparison
        public void SortWordsByAlphabet()
        {
            wordsList.Sort(WordsComparisonByAlphabet);
        }
        static public void SortWordsByAlphabet(List<Word> wList)
        {
            wList.Sort(new WordsSorterByWordFull());
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

        class WordsSorterByWordFull : IComparer<Word>
        {
            public int Compare(Word x, Word y)
            {
                return x.WordFull.CompareTo(y.WordFull);
            }
        }
    }
}
