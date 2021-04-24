using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WordLord
{
    public class Word
    {
        public string WordFull { get; set; }
        public int WordID { get; set; }
        public List<Letter> letters;
        private bool guessed;
        private bool isSelected = false;
        public bool IsSelected { get { return isSelected; } set { isSelected =value;} }
        public bool Guessed { get { return guessed; } set { guessed = value; } }
        public Word(string word)
        {
            WordFull = word.ToLower();
            //letters = new List<Letter>();
            letters = GetLettersWithPositions(word);
        }
        public Word()
        {
            WordFull = null;
        }

        private List<Letter> GetLettersWithPositions(string word)
        {
            List<Letter> lts = new List<Letter>();
            for (int ch_index = 0; ch_index < word.Length; ch_index++)
            {
                char ch = word[ch_index];
                if (lts.Exists((item) => item.letter == ch)) // using Predicate
                {
                    FindLetterObjByChar(ref lts, ch).AddPosition(ch_index);
                }
                else
                    lts.Add(new Letter(ch, ch_index));
            }
            return lts;
        }
        public void PrintLetters()
        {
            foreach(Letter l in letters)
            {
                Console.Write("Буква: "+l.letter + " Позиции: ");
                foreach (int pos in l.GetPositions())
                {
                    Console.Write(pos + ", ");
                }
                Console.Write("\n");
            }
        }
        public override string ToString()
        {
            return WordFull;
        }
        public List<int> GetLetterPositions(char ch)
        {
            return FindLetterObjByChar(ref letters, ch).GetPositions();
        }
        public Letter FindLetterObjByChar(ref List<Letter> lts, char ch)
        {
            return lts.Find((item) => item.letter == ch);// using Predicate
        }
    }

}
