using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLord
{
    public class Letter
    {
        public char letter { get; set; }
        private bool guessed = false;
        List<int> positions = null;
        public Letter(char ch)
        {
            letter = ch;
        }
        public Letter(char ch, int pos)
        {
            positions = new List<int>();
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
        public bool IsChecked()
        {
            return guessed;
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
