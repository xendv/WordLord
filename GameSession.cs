using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLord
{
    class GameSession
    {
        string User = null;
        int score;
        Word WordToGuess;
        public GameSession()
        {
            score = 15;
            int wordsInDictionary = 1;
        }
    }
}
