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
        private int score;
        private List<Letter> guessedLetters;
        public Word wordToGuess;

        public GameSession(string word)
        {
            score = 15;
            wordToGuess = new Word(word);
            guessedLetters = new List<Letter>();
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

        public void AddToGuessedAndSort(char letter, bool isGuessed=false)
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
    }
}
