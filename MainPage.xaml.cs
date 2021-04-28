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

using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Collections;
using System.IO;
using System.Xml;

namespace WordLord
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainWindow mainWin;
        public bool hasErrors = false;
        //Page curr_page;
        public MainPage()
        {
            InitializeComponent();
        }
        public MainPage(MainWindow win)
        {
            InitializeComponent();
            this.mainWin = win;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            this.mainWin.Content = new GamePage(this.mainWin);
        }

        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            //mainWin.SetPage("ContinueGame");
            GamePage contitueGamePage = new GamePage(this.mainWin, true);
            if (!mainWin.hasErrors)
            {
                contitueGamePage.PrintGuessedWordsWithColors();
                this.mainWin.Content = contitueGamePage;
                
            }

            //GameSessionLoader gameSessionLoader = new GameSessionLoader(mainWin);
            /*if (gameSessionLoader.error != "")
            {
                mainWin.
            }*/
        }

        private void ButtonDictionary_Click(object sender, RoutedEventArgs e)
        {
            DictionaryWindow dictionaryWindow = new DictionaryWindow(this.mainWin);
            dictionaryWindow.Owner = this.mainWin;
        }
        public void OpenFile()
        {
        }
        public static void AddPages(FixedDocument fixedDocument)
        {
            var enumerator = fixedDocument.Resources.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var pageContent = ((DictionaryEntry)enumerator.Current).Value as PageContent;
                if (pageContent != null)
                {
                    fixedDocument.Pages.Add(pageContent);
                }
            }
        }

        private void ToAboutGamePage_Click(object sender, RoutedEventArgs e)
        {
            mainWin.SetPage("AboutGame");
        }

        private void ToAboutAuthorPage_Click(object sender, RoutedEventArgs e)
        {
            mainWin.SetPage("AboutAuthor");
        }
    }
}
