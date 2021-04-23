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
        public GamePage(MainWindow win)
        {
            InitializeComponent();
            ParentWindowMain = win;
            WordsLoader wordsLoader = new WordsLoader(ParentWindowMain);
            if (!win.DisplayDictionaryErrors(wordsLoader.errorType))
            {
                //int wordsCount = wordsLoader.wordsList.Count();

                GameSession game = new GameSession(wordsLoader.GetRandomWord());
                Label newLabel;
                for (int letterIndex=0; letterIndex<game.wordToGuess.letters.Count(); letterIndex++)
                {
                    newLabel = new Label()
                    {
                        Margin = new Thickness(10, 10, 0, 0),
                        Content = "—",
                        FontSize = 48,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                WordToGuessStackPanel.Children.Add(newLabel);
                }
            }
        }

        private void ToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindowMain.SetPage("Main");
        }
    }
}
