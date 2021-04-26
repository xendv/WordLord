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
        List<Word> wordsFromDictionary;
        public DictionaryWindow(MainWindow win)
        {
            InitializeComponent();
            WordsLoader wordsLoader = new WordsLoader(this);
            if (!win.DisplayDictionaryErrors(wordsLoader.errorType))
            {
                PrintExistingWordsList(ref wordsLoader.GetWords());
            }
        }
        public ObservableCollection<Word> Words { get; set; }
        public void PrintExistingWordsList(ref List<Word> words)
        {
            UpdateWords(ref words);
            WordsListBox.ItemsSource = wordsFromDictionary;
            //Binding binding = BindingOperations.GetBinding(WordsListBox, ItemsControl.ItemsSourceProperty);
            //binding.Mode = BindingMode.TwoWay;
            this.ShowDialog();
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
            if (inp < 'а' || inp > 'я')
                e.Handled = true;
            else if (inp >= 'А' && inp <= 'Я')
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
            if (checkAllCheckBox.IsChecked == true && !wordsFromDictionary.Exists(x => x.IsSelected==false))
                checkAllCheckBox.IsChecked = false;

        }

        private void checkAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Word w in wordsFromDictionary)
            {
                w.IsSelected = true;
            }
        }
    }
}
