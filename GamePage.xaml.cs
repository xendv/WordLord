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
using System.Threading;

namespace WordLord
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        MainWindow ParentWindowMain;
        public GameSession game;
        bool asComp = false;
        public Task task;
        public CancellationTokenSource cts;
        public CancellationToken token;

        public GamePage(MainWindow win)
        {
            asComp = false;
            InitializeComponent();
            ParentWindowMain = win;
            WordsLoader wordsLoader = new WordsLoader(ParentWindowMain);
            if (!win.DisplayDictionaryErrors(wordsLoader.errorType))
            {

                game = new GameSession(wordsLoader.GetRandomWord());
                BuildGamePage();
                ParentWindowMain.gameStarted = true;
                ParentWindowMain.gamePageChild = this;
            }
        }

        public GamePage(MainWindow win, bool asComp, string t)
        {
            this.asComp = asComp;
            win.asComp = true;
            InitializeComponent();
            ParentWindowMain = win;
            WordsLoader wordsLoader = new WordsLoader(ParentWindowMain);
            if (!win.DisplayDictionaryErrors(wordsLoader.errorType))
            {
                game = new GameSession(wordsLoader.GetRandomWord(), true);
                BuildGamePage();
                ParentWindowMain.gameStarted = true;
                ParentWindowMain.gamePageChild = this;
            }
            letterToGuessTextBlock.IsEnabled = false;

            /*cts = new System.Threading.CancellationTokenSource();
            token = cts.Token;
            task = new Task(() => { compGuess(); }, token);
            task.Start();

            //if (cts.Token.IsCancellationRequested) return;
            cts.Dispose();
            cts = null;*/
            compGuess();
        }

        public GamePage(MainWindow win, bool restoreSession = false)
        {
            this.asComp = false;
            ParentWindowMain = win;
            if (restoreSession)
            {
                game = new GameSession();
                if (!win.DisplayDBErrors(game.error))
                {
                    game.GetSavedSession();
                    InitializeComponent();
                    BuildGamePage();
                    ParentWindowMain.gameStarted = true;
                    ParentWindowMain.gamePageChild = this;
                }
                else win.hasErrors = true;
            }
        }

        private void ToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindowMain.SetPage("Main");
        }
        
        string alphabet = "абвгдеёжзиклмнопрстуфхцчшщьыъэюя";
        Random random = new Random();

        public async void compGuess()
        {
            game.isFinished = false;

            //
            while (!game.isFinished && !game.isPaused/*token.IsCancellationRequested*/)
            {
                int temp_ind = random.Next(0, alphabet.Length);
                letterToGuessTextBlock.Text = alphabet[temp_ind].ToString();
                alphabet = alphabet.Remove(temp_ind, 1);
                //while
                //тест 27 к  13 у второй группе
                await Task.Delay(1000);
                letterToGuessPopup.IsOpen = false;
                char letterTG = letterToGuessTextBlock.Text.ToString().ToLower()[0];
                if (!game.CheckIfGuessedALetter(letterTG))
                {
                    if (!game.LetterWasTried(letterTG))
                        game.AddToGuessedAndSort(letterTG);
                    letterToGuessPopupTextBlock.Text = "Буквы '" + letterTG + "' нет в слове!";
                    letterToGuessPopup.IsOpen = true;
                    letterToGuessTextBlock.Text = "";
                    UpdateScore();
                }
                else
                {
                    if (!game.LetterWasTried(letterTG))
                    {
                        game.AddToGuessedAndSort(letterTG, true);
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
                    }
                    if (GuessedAllLetters())
                    {
                        MessageBoxResult result = MessageBox.Show("Вы выиграли!\nВаш счёт: " + game.GetScore() + "\nЗагаданное слово: " + game.wordToGuess.ToString() + "\nНачать новую игру?", "Выигрыш", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        ShowEndGameMessageBox(result);
                        game.isFinished = true;
                    }
                    letterToGuessTextBlock.Text = "";
                }
                PrintGuessedWordsWithColors();
                letterToGuessTextBlock.Text = "";
                if (game.GetScore() == 0)
                {
                    MessageBoxResult result = MessageBox.Show("Вы проиграли!\nЗагаданное слово - " + game.wordToGuess.ToString() + "\nНачать новую игру?", "Проигрыш", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    ShowEndGameMessageBox(result);
                    game.isFinished = true;
                }
                await Task.Delay(1000);
            }
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
                        {
                            game.AddToGuessedAndSort(letterTG, true);
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
                        }
                        else
                        {
                            letterToGuessPopupTextBlock.Text = "Буква '" + letterTG + "' уже угадана!";
                            letterToGuessPopup.IsOpen = true;
                        }

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
            if (inp < 'а' || inp > 'я' && inp != 'ё')
                e.Handled = true;
        }

        private void letterToGuessTextBlock_OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        public void SaveGame()
        {
            GameSessionLoader gameSessionLoader = new GameSessionLoader();
            if (!ParentWindowMain.DisplayDBErrors(gameSessionLoader.error))
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
            //letterToGuessTextBlock.ToolTip = game.wordToGuess;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            switch (menuItem.Name)
            {
                case "SaveGameMenuItem":
                    SaveGame();
                    break;
                case "SaveAndExitToMainMenu":
                    cts.Cancel(true);
                    SaveGame();
                    ParentWindowMain.SetPage("Main");
                    break;
                case "ExitToMainMenu":
                    MessageBoxResult result = MessageBox.Show("Выйти без сохранения?", "Уверены?", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            ParentWindowMain.SetPage("Main");
                            break;
                        default: break;
                    }
                    break;
                case "Exit":
                    ParentWindowMain.Close();
                    break;
                default: break;
            }
        }
    }
}
