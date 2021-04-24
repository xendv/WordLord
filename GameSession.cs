using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLord
{
    public class GameSession
    {
        //string User = null;
        private int score;
        List<Letter> guessedLetters;
        public Word wordToGuess;
        public GameSession(string word)
        {
            score = 15;
            wordToGuess = new Word(word);
            guessedLetters = new List<Letter>();
        }
        public bool GuessALetter(char letter)
        {
            guessedLetters.Add(new Letter(letter));
            if (wordToGuess.letters.Exists((item) => item.letter == letter))
            {
                Letter lt = wordToGuess.letters.Find((item) => item.letter == letter);
                lt.checkLetter();
                return true;
            }
            else
            {
                score--;
                return false;
            }
        }
        public int GetScore()
        {
            return score;
        }
        public void SetScore(int value)
        {
            score = value;
        }
    }
}
