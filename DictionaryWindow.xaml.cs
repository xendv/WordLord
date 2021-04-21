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
using System.Windows.Shapes;

namespace WordLord
{
    /// <summary>
    /// Логика взаимодействия для DictionaryWindow.xaml
    /// </summary>
    public partial class DictionaryWindow : Window
    {
        public DictionaryWindow(MainWindow win)
        {
            InitializeComponent();
            WordsLoader wordsLoader = new WordsLoader(this);
            switch (wordsLoader.errorType){
                case "Wrong Content":
                    MessageBox.Show("Похоже, файл поврежден!\nИсправьте его содержимое", "Ошибка содержания файла", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    break;
                case "No File":
                    MessageBox.Show("Файл словаря пустой, создаю новый...", "Словарь не найден", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "":
                    {
                        PrintExistingWordsList(ref wordsLoader.GetWords());
                        //this.ShowDialog();
                    }
                    break;
                default:
                    MessageBox.Show("Необработанное исключение!", "ОЙ", MessageBoxButton.OK, MessageBoxImage.Error);
                    /*Difference between Show() and ShowDialog() methods
                     * is in restricting access to the window that opened new window
                     * in the second method*/
                    break;
            }

            //DictionaryTextBlock.Text = wordsLoader.fileText;
        }
        public void PrintExistingWordsList(ref List<Word> words)
        {
            this.ShowDialog();
        
        }
        public static readonly DependencyProperty wordsListProperty =
           DependencyProperty.Register("wordsList", typeof(List<Word>), typeof(DictionaryWindow), new PropertyMetadata(null));
    }
}
