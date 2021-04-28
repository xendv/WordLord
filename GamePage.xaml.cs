using System;
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
                BuildGamePage();
               
            }
        }

        public GamePage(MainWindow win, bool restoreSession=false)
        {
            ParentWindowMain = win;
            if (restoreSession)
            {
                game = new GameSession();
                if (!win.DisplayDBErrors(game.error))
                {
                    game.GetSavedSession();
                    InitializeComponent();
                    BuildGamePage();
                }
                else win.hasErrors = true;
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
                    char letterTG = letterToGuessTextBlock.Text.ToString().ToLower()[0];
                    if (!game.CheckIfGuessedALetter(letterTG))
                    {
                        if (!game.LetterWasTried(letterTG))
                            game.AddToGuessedAndSort(letterTG);
                        letterToGuessPopupTextBlock.Text = "Буквы '" + letterTG + "' нет в слове!";
                        letterToGuessPopup.IsOpen = true;
                        UpdateScore();
                    }
                    else
                    {
                        if (!game.LetterWasTried(letterTG))
                            game.AddToGuessedAndSort(letterTG, true);
                        else
                        {
                            letterToGuessPopupTextBlock.Text = "Буква '" + letterTG + "' уже угадана!";
                            letterToGuessPopup.IsOpen = true;
                        }
                        foreach (int pos in game.wordToGuess.GetLetterPositions(letterTG))
                        {
                            //this.FindName(letterTG.ToString());
                            var result = WordToGuessStackPanel.Children.OfType<TextBlock>().Where(x => x.Tag.ToString() == letterTG.ToString()).ToList<TextBlock>();

                            foreach (TextBlock tb in result)
                            {
                                tb.Text = letterTG.ToString();
                            }
                        }
                        PrintGuessedWordsWithColors();
                        if (GuessedAllLetters())
                        {
                            MessageBoxResult result = MessageBox.Show("Вы выиграли!\nВаш счёт: " + game.GetScore() + "\nЗагаданное слово: " + game.wordToGuess.ToString() + "\nНачать новую игру?", "Выигрыш", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            ShowEndGameMessageBox(result);
                        }
                    }
                    PrintGuessedWordsWithColors();
                    letterToGuessTextBlock.Text = "";
                    if (game.GetScore() == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Вы проиграли!\nЗагаданное слово - " + game.wordToGuess.ToString() + "\nНачать новую игру?", "Проигрыш", MessageBoxButton.YesNo, MessageBoxImage.Error);
                        ShowEndGameMessageBox(result);
                    }

                }
                else
                {
                    letterToGuessPopupTextBlock.Text = "Введите букву!";
                    letterToGuessPopup.IsOpen = true;
                }
            }
            else letterToGuessPopup.IsOpen = false;
        }

        private void UpdateScore()
        {
            ScoreLabel.Content = game.GetScore();
        }

        private bool GuessedAllLetters()
        {
            return !game.wordToGuess.letters.Exists(ltr => !ltr.IsChecked());
        }

        public void ShowEndGameMessageBox(MessageBoxResult result)
        {
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

        public void PrintGuessedWordsWithColors()
        {
            guessedLettersWrapPanel.Children.Clear();
            TextBlock guessedLetterTextBlock;
            foreach (Letter lt in game.GetGuessedLetters())
            {
                guessedLetterTextBlock = new TextBlock()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 20,
                    Padding = new Thickness(5, 0, 0, 0),
                    Margin = new Thickness(20, 0, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    Text = lt.letter.ToString().ToUpper()
                };
                if (lt.IsChecked())
                {
                    guessedLetterTextBlock.Foreground = Brushes.Green;
                    guessedLetterTextBlock.TextDecorations = TextDecorations.Underline;
                    guessedLetterTextBlock.FontWeight = FontWeights.Bold;
                }
                guessedLettersWrapPanel.Children.Add(guessedLetterTextBlock);
            }
        }

        private void letterToGuessTextBlock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char inp = e.Text.ToLower()[0];
            if (inp < 'а' || inp > 'я')
                e.Handled = true;
        }

        private void letterToGuessTextBlock_OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        private void SaveGameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GameSessionLoader gameSessionLoader = new GameSessionLoader();
            if(!ParentWindowMain.DisplayDBErrors(gameSessionLoader.error))
            {
                game.gameSessionLoader = gameSessionLoader;
                game.SaveSession();
            }
        }
        private void BuildGamePage()
        {
            UpdateScore();

            //создание textblock'а для каждой буквы слова
            TextBlock letterTextBlock;
            for (int letterIndex = 0; letterIndex < game.wordToGuess.WordFull.Length; letterIndex++)
            {
                letterTextBlock = new TextBlock()
                {
                    Tag = game.wordToGuess.WordFull[letterIndex],
                    Margin = new Thickness(10, 10, 0, 0),
                    Text = "-",
                    FontSize = 48,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                WordToGuessStackPanel.Children.Add(letterTextBlock);
            }
            letterToGuessTextBlock.Focus();
            letterToGuessTextBlock.ToolTip = game.wordToGuess;
        }
    }
}
