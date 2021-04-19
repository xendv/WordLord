using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLord
{
    class Letter
    {
        public char letter { get; set; }
        bool guessed = false;
        List<int> positions = new List<int>();
        public Letter(char ch, int pos)
        {
            letter = ch;
            positions.Add(pos);
        }
        public Letter(int pos)
        {
            positions.Add(pos);
        }

        public void checkLetter()
        {
            guessed = true;
        }
        public void AddPosition(int pos)
        {
            positions.Add(pos);
        }
        public List<int> GetPositions()
        {
            return positions;
        }
    }
}
