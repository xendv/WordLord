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
            win.asComp = asComp;
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
            ParentWindowMain = win;
            this.asComp = false;
            ParentWindowMain.asComp = false;
            if (restoreSession)
            {
                game = new GameSession();
                if (!win.DisplayDBErrors(game.error))
                {
                    game.GetSavedSession();
                    InitializeComponent();
                    BuildGamePage();
                    insertAllGuessedLettersToWord();
                    if (game.asComp)
                    {
                        this.asComp = true;
                        ParentWindowMain.asComp = true;
                        letterToGuessTextBlock.IsEnabled = false;
                        foreach (Letter lt in game.GetGuessedLetters())
                        {

                            alphabet = alphabet.Replace(lt.letter.ToString(), String.Empty);
                        }
                        if (GuessedAllLetters())
                        {
                            letterToGuessTextBlock.IsEnabled = false;
                            game.isFinished = true;
                        }
                        else if (game.GetScore() == 0)
                        {
                            letterToGuessTextBlock.IsEnabled = false;
                            game.isFinished = true;
                        }
                        else compGuess();
                    }
                    else
                    {
                        if (GuessedAllLetters())
                        {
                            letterToGuessTextBlock.IsEnabled = false;
                            game.isFinished = true;
                        }
                        else if (game.GetScore() == 0)
                        {
                            letterToGuessTextBlock.IsEnabled = false;
                            game.isFinished = true;
                        }
                    }
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

        public void insertAllGuessedLettersToWord()
        {
            foreach (Letter lt in game.GetGuessedLetters())
            {
                if (lt.IsChecked())
                {
                    var result = WordToGuessStackPanel.Children.OfType<TextBlock>().Where(x => x.Tag.ToString() == lt.letter.ToString()).ToList<TextBlock>();

                    foreach (TextBlock tb in result)
                    {
                        tb.Text = lt.letter.ToString();
                    }
                }

            }

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

                await Task.Delay(500);
                letterToGuessPopup.IsOpen = false;
                char letterTG = letterToGuessTextBlock.Text.ToString().ToLower()[0];
                if (!game.CheckIfGuessedALetter(letterTG) && !game.isPaused)
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
                    if (!game.LetterWasTried(letterTG) && !game.isPaused)
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
                        MessageBoxResult result = MessageBox.Show("Выигрыш!\nСчёт: " + game.GetScore() + "\nЗагаданное слово: " + game.wordToGuess.ToString() + "\nНачать новую игру за компьютер?", "Выигрыш", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        ShowEndGameMessageBox(result);
                        game.isFinished = true;
                    }
                    letterToGuessTextBlock.Text = "";
                }
                PrintGuessedWordsWithColors();
                letterToGuessTextBlock.Text = "";
                if (game.GetScore() <= 0)
                {
                    MessageBoxResult result = MessageBox.Show("Проигрыш!\nЗагаданное слово - " + game.wordToGuess.ToString() + "\nНачать новую игру за компьютер?", "Проигрыш", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    ShowEndGameMessageBox(result);
                    game.isFinished = true;
                }

                await Task.Delay(500);
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
                            game.isFinished = true;
                        }
                    }
                    PrintGuessedWordsWithColors();
                    letterToGuessTextBlock.Text = "";
                    if (game.GetScore() <= 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Вы проиграли!\nЗагаданное слово - " + game.wordToGuess.ToString() + "\nНачать новую игру?", "Проигрыш", MessageBoxButton.YesNo, MessageBoxImage.Error);
                        ShowEndGameMessageBox(result);
                        game.isFinished = true;
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
                    if (!asComp)
                    {
                        ParentWindowMain.SetPage("StartGame");
                    }
                    else ParentWindowMain.SetPage("StartCompGame");
                    break;
                case MessageBoxResult.No:
                    game.isPaused = true;
                    game.isFinished = true;
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
                    //cts.Cancel(true);
                    game.isPaused = true;
                    SaveGame();
                    ParentWindowMain.SetPage("Main");
                    break;
                case "ExitToMainMenu":
                    game.isPaused = true;
                    MessageBoxResult result = MessageBox.Show("Выйти без сохранения?", "Уверены?", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            ParentWindowMain.SetPage("Main");
                            break;
                        case MessageBoxResult.Cancel:
                            if (!game.isFinished) game.isPaused = false;
                            if (asComp && !game.isFinished) compGuess();
                            break;
                        default:
                            MessageBox.Show("Ошибка в форме выхода");
                            break;
                    }
                    break;
                case "Exit":
                    game.isPaused = true;
                    ParentWindowMain.Close();
                    break;
                default: break;
            }
        }
    }
}
