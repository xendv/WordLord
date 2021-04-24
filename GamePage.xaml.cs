﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WordLord
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        MainWindow ParentWindowMain;
        GameSession game;
        public GamePage(MainWindow win)
        {
            InitializeComponent();
            ParentWindowMain = win;
            WordsLoader wordsLoader = new WordsLoader(ParentWindowMain);
            if (!win.DisplayDictionaryErrors(wordsLoader.errorType))
            {
                game = new GameSession(wordsLoader.GetRandomWord());
                UpdateScore();

                //создание textblock'а для каждой буквы слова
                TextBlock letterTextBlock;
                for (int letterIndex = 0; letterIndex < game.wordToGuess.WordFull.Length; letterIndex++)
                {
                    letterTextBlock = new TextBlock()
                    {
                        Tag = game.wordToGuess.WordFull[letterIndex],
                        //Name = game.wordToGuess.WordFull[letterIndex].ToString(),
                        //Name = letterIndex.ToString(),
                        Margin = new Thickness(10, 10, 0, 0),
                        Text = "-",
                        FontSize = 48,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    WordToGuessStackPanel.Children.Add(letterTextBlock);
                }
                letterToGuessTextBlock.ToolTip = game.wordToGuess;
            }
        }

        private void ToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindowMain.SetPage("Main");
        }

        private void letterToGuess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (letterToGuessTextBlock.Text.Length != 0)
                {
                    letterToGuessPopup.IsOpen = false;
                    char letterTG = letterToGuessTextBlock.Text.ToString()[0];
                    if (!game.GuessALetter(letterTG))
                    {
                        letterToGuessPopupTextBlock.Text = "Буквы '" + letterTG + "' нет в слове!";
                        letterToGuessPopup.IsOpen = true;
                        UpdateScore();
                    }
                    else
                    {
                        foreach (int pos in game.wordToGuess.GetLetterPositions(letterTG))
                        {
                            //this.FindName(letterTG.ToString());
                            var result = WordToGuessStackPanel.Children.OfType<TextBlock>().Where(x => x.Tag.ToString() == letterTG.ToString()).ToList<TextBlock>();

                            foreach (TextBlock tb in result)
                            {
                                tb.Text = letterTG.ToString();
                            }
                        }
                    }
                    letterToGuessTextBlock.Text = "";
                    if (game.GetScore() == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Вы проиграли!\nЗагаданное слово - " + game.wordToGuess.ToString() + "\nНачать новую игру?", "Проигрыш", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                        letterToGuessTextBlock.IsEnabled = false;
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                ParentWindowMain.SetPage("StartGame");
                                break;
                            case MessageBoxResult.No:
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    letterToGuessPopupTextBlock.Text = "Введите букву!";
                    letterToGuessPopup.IsOpen = true;
                }
            }
        }
        private void UpdateScore()
        {
            ScoreLabel.Content = game.GetScore();
        }
    }
}
