﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLord
{
    public class GameSession
    {
        //string User = null;
        private int score = 15;
        private List<Letter> guessedLetters;
        public Word wordToGuess;
        public bool isFinished = false;
        public string error = "";
        public GameSessionLoader gameSessionLoader;
        public bool asComp;
        public bool isPaused = false;

        public GameSession(string word)
        {

            score = 15;
            wordToGuess = new Word(word);
            guessedLetters = new List<Letter>();
        }

        public GameSession()
        {
            InitializeLoader();
        }


        public GameSession(string word, bool asComp)
        {
            score = 15;
            wordToGuess = new Word(word);
            guessedLetters = new List<Letter>();
            InitializeLoader();
            this.asComp = asComp;
        }



        public bool LetterWasTried(char letter)
        {
            if (!guessedLetters.Exists((item) => item.letter == letter))
            {
                //AddToGuessed(letter);
                return false;
            }
            else
                return true;
        }

        public void SortGuessedLettersByAlphabet()
        {
            guessedLetters.Sort(LettersComparisonByAlphabet);
        }

        private int LettersComparisonByAlphabet(Letter lt1, Letter lt2)
        {
            return String.Compare(lt1.letter.ToString(), lt2.letter.ToString());
        }

        public bool CheckIfGuessedALetter(char letter)
        {
            //LetterWasGuessed(letter);//поменять порядок
            if (wordToGuess.CheckLetterExistence(letter))
            {
                Letter lt = wordToGuess.FindLetterObjByChar(letter);
                lt.checkLetter();
                //lt.color = "Green";
                return true;
            }
            else
            {
                score--;
                return false;
            }
        }

        public void AddToGuessedAndSort(char letter, bool isGuessed = false)
        {
            guessedLetters.Add(new Letter(letter, isGuessed));
            SortGuessedLettersByAlphabet();
        }

        public List<Letter> GetGuessedLetters()
        {
            return guessedLetters;
        }

        public int GetScore()
        {
            return score;
        }

        public void SetScore(int value)
        {
            score = value;
        }

        public void InitializeLoader()
        {
            gameSessionLoader = new GameSessionLoader();
            error = gameSessionLoader.error;
        }

        public void SaveSession()
        {
            //InitializeLoader();
            int aC = 0;
            if (asComp) aC = 1;
            gameSessionLoader.SaveGameSessionData(guessedLetters, wordToGuess.WordFull, score.ToString(), aC);
        }

        public void GetSavedSession()
        {
            this.wordToGuess = gameSessionLoader.getWordToGuess();
            this.guessedLetters = gameSessionLoader.getGuessedLetters(wordToGuess.letters);
            this.score = gameSessionLoader.getScore();
            if (gameSessionLoader.getAsComp() == 0)
                this.asComp = false;
            else this.asComp = true;
            //this.gameSessionLoader = gSL;
        }

    }
}
