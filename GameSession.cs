using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLord
{
    public class GameSession
    {
        string User = null;
        int score;
        List<Letter> guessedLetters;
        Word wordToGuess;
        public GameSession(string word)
        {
            score = 15;
            wordToGuess = new Word(word);
        }
        public bool GuessALetter(char letter)
        {
            guessedLetters.Add(new Letter(letter));
            if (wordToGuess.letters.Exists((item) => item.letter == letter))
            {
                
                return true;
            }
            else
            {
                score--;
                return false;
            }
        }
    }
}
