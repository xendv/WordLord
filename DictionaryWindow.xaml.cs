using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace WordLord
{
    /// <summary>
    /// Логика взаимодействия для DictionaryWindow.xaml
    /// </summary>
    public partial class DictionaryWindow : Window
    {
        MainWindow mainWindow;
        public List<Word> wordsFromDictionary { get; set; }
        WordsLoader wordsLoader;
        public DictionaryWindow(MainWindow win)
        {
            InitializeComponent();
            mainWindow = win;
            wordsLoader = new WordsLoader(this);
            if (!win.DisplayDictionaryErrors(wordsLoader.errorType))
            {
                List<Word> tempList = wordsLoader.GetWords();
                wordsFromDictionary = new List<Word>(tempList.Count());
                foreach (Word w in tempList)
                {
                    wordsFromDictionary.Add(w);
                }
                //wordsLoader.GetWords().Clo(wordsFromDictionary.ToArray());

                PrintExistingWordsList(wordsLoader.GetWords());
                this.ShowDialog();
            }
        }
        //public ObservableCollection<Word> Words { get; set; }
        public void PrintExistingWordsList(List<Word> words)
        {
            //UpdateWords(ref words);
            WordsListBox.ItemsSource = wordsFromDictionary;
        }

        public void UpdateWords(ref List<Word> words)
        {
            wordsFromDictionary = words;
        }
        public static readonly DependencyProperty wordsListProperty =
           DependencyProperty.Register("wordsList", typeof(List<Word>), typeof(DictionaryWindow), new PropertyMetadata(null));

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {

            }

        }

        private void newWordTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char inp = e.Text[0];
            if (inp < 'а' || inp > 'я' && inp!='ё')
                e.Handled = true;
            else if (inp >= 'А' && inp <= 'Я' || inp=='Ё')
            {
                e.Handled = true;
            }

        }

        private void newWordTextBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            newWordTextBoxPopup.IsOpen = true;
        }

        private void newWordTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        private void checkWordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chbx = sender as CheckBox;
            if (checkAllCheckBox.IsChecked == true && !wordsFromDictionary.Exists(x => x.IsSelected == false))
                checkAllCheckBox.IsChecked = false;

        }

        private void checkAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Word w in wordsFromDictionary)
            {
                w.IsSelected = true;

            }
            WordsListBox.ItemsSource = wordsFromDictionary;
            RefreshWordsList();
        }

        private void deleteSelectedWordsButton_Click(object sender, RoutedEventArgs e)
        {

            wordsFromDictionary.RemoveAll(item => item.IsSelected == true);
            WordsListBox.ItemsSource = wordsFromDictionary;
            RefreshWordsList();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chbx = (CheckBox)sender;
            wordsFromDictionary.Find(item => item.WordFull == chbx.Tag.ToString()).IsSelected = true;
        }

        private System.Windows.Threading.DispatcherTimer popupTimer;
        private void newWordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Space) ; Убрать ввод пробелов!!
            if (e.Key == Key.Enter && newWordTextBox.Text.Trim().Length != 0)
            {
                CheckWordExistence();
            }
        }
        void popupTimer_Tick(object sender, EventArgs e)
        {
            popupTimer.IsEnabled = false;
            addWordButtonPopup.IsOpen = false;
        }
        private void ShowAddWordPopup(bool added = true)
        {
            newWordTextBoxPopup.IsOpen = false;
            if (added)
                addWordButtonPopupTextBlock.Text = "Слово\n\"" + newWordTextBox.Text + "\"\nдобавлено!";
            else
                addWordButtonPopupTextBlock.Text = "Слово\n\"" + newWordTextBox.Text + "\"\nуже есть!";
            newWordTextBox.Text = "";
            addWordButtonPopup.IsOpen = true;

            popupTimer = new System.Windows.Threading.DispatcherTimer();

            popupTimer.Interval = new TimeSpan(0, 0, 3);
            popupTimer.IsEnabled = true;
            popupTimer.Tick += new EventHandler(popupTimer_Tick);
        }
        private void AddWord(String word)
        {
            wordsFromDictionary.Add(new Word(word));
            WordsLoader.SortWordsByAlphabet(wordsFromDictionary);
            RefreshWordsList();
            ShowAddWordPopup();
        }

        private void addWordButton_Click(object sender, RoutedEventArgs e)
        {
            CheckWordExistence();
        }
        private void CheckWordExistence()
        {
            if (!WordAlreadyExists(newWordTextBox.Text.Trim()))
            {
                AddWord(newWordTextBox.Text.Trim());
            }
            else
            {
                ShowAddWordPopup(false);
            }
        }

        private bool WordAlreadyExists(string newWord)
        {
            return wordsFromDictionary.Exists(word => word.WordFull == newWord);
        }

        public void RefreshWordsList()
        {
            WordsListBox.Items.Refresh();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!wordsLoader.HasSameContentAsCurrentList(wordsFromDictionary))
            {
                MessageBoxResult result = MessageBox.Show("В словаре есть несохраненные изменения.\n\nСохранить?", "Подтвердить изменения", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                CloseDictionaryMessageBox(result, e);
            }
        }
        public void CloseDictionaryMessageBox(MessageBoxResult result, System.ComponentModel.CancelEventArgs e)
        {

            switch (result)
            {
                case MessageBoxResult.Yes:
                    wordsLoader.RewriteFileContent(wordsFromDictionary);
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
                default:
                    MessageBox.Show("Некорректный обработчик кнопок!");
                    break;
            }
        }


        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            Button changeButton = (Button)sender;
            changeButton.Visibility = Visibility.Hidden;
            //Button saveButton = WordsListBox.Items.OfType<Button>().Find((item) => item.Tag == changeButton.Tag);
            
        }

        private void checkAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (Word w in wordsFromDictionary)
            {
                w.IsSelected = false;
                WordsListBox.ItemsSource = wordsFromDictionary;
                RefreshWordsList();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            wordsLoader.RewriteFileContent(wordsFromDictionary);
            ShowSavedPopup();
        }
        void popupSavedTimer_Tick(object sender, EventArgs e)
        {
            popupTimer.IsEnabled = false;
            SaveButtonPopup.IsOpen = false;
        }
        private void ShowSavedPopup()
        {
            SaveButtonPopup.IsOpen = true;

            popupTimer = new System.Windows.Threading.DispatcherTimer();

            popupTimer.Interval = new TimeSpan(0, 0, 3);
            popupTimer.IsEnabled = true;
            popupTimer.Tick += new EventHandler(popupSavedTimer_Tick);
        }
    }
}
