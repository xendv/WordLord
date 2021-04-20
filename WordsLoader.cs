using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace WordLord
{
    class WordsLoader
    {
        public bool errors = false;
        public string fileText;
        public WordsLoader()
        {
            string startupPath = Environment.CurrentDirectory;
            string fileName = @startupPath+"\\Dictionary.TXT";
            
            //string fileName = @"C:\\Users\\xendv\\source\\repos\\WordLord\\Dictionary.TXT";
            if (System.IO.File.Exists(fileName))
            {
                fileText = File.ReadAllText(fileName);
                errors = false;
            }
            else errors = true;
        }
    }
}
