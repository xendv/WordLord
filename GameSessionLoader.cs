using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace WordLord
{
    public class GameSessionLoader
    {
        //MainWindow win;
        string DBName = "GameSessionsDB.db";
        public string error = "";
        SqliteConnection connection;
        public Word wTG;

        public GameSessionLoader()
        {
            ConnectToDB();
        }
        private void ConnectToDB()
        {
            if (!System.IO.File.Exists(DBName))
            {
                error = "Нет БД!";
            }
            else
            {
                connection = new SqliteConnection("Data Source=" + DBName);
                connection.Open();
            }
        }

        public List<Letter> getGuessedLetters(List<Letter> wordsLetters)
        {
            string guessedLetterString = "";
            List<Letter> guessedLetters = new List<Letter>();
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText =
                @"SELECT guessedLettersChars FROM sessions_data WHERE id = 1";
            //command.CommandText = "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL)";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    guessedLetterString = reader.GetString(0);
                }
            }
            foreach (Char ch in guessedLetterString)
            {
                bool isGuessed;
                if (wordsLetters.Exists(letter => letter.letter == ch))
                {
                    isGuessed = true;
                    wordsLetters.Find(letter => letter.letter == ch).checkLetter();
                }
                else isGuessed = false;
                guessedLetters.Add(new Letter(ch, isGuessed));
                //guessedLetters.Add()
            }
            return guessedLetters;
        }

        public Word getWordToGuess()
        {
            string wordFull="";
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText =
                @"SELECT wordFull FROM sessions_data WHERE id = 1";
            //command.CommandText = "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL)";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    wordFull = reader.GetString(0);
                }
            }
            wTG = new Word(wordFull);
            return wTG;
        }

        public int getScore()
        {
            string scoreStr = "";
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText =
                @"SELECT score FROM sessions_data WHERE id = 1";
            //command.CommandText = "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL)";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    scoreStr = reader.GetString(0);
                }
            }
            int score = Convert.ToInt32(scoreStr);
            return score;
        }

        public void SaveGameSessionData(List<Letter> guessedLetters, string WordFull, string score)
        {
            string guessedLettersStr = "";
            foreach (Letter letter in guessedLetters)
            {
                guessedLettersStr += letter.letter.ToString();
            }
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            string sqlExpression = $"UPDATE sessions_data SET wordFull='{WordFull}', " +
                $"guessedLettersChars='{guessedLettersStr}', score='{score}' " +
                $"WHERE session_id=1";
            command.CommandText = sqlExpression;
            command.ExecuteNonQuery();
        }

    }
}
