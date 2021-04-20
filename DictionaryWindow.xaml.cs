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
            if (wordsLoader.errorType == "Wrong Content")
            {
                MessageBox.Show("Похоже, файл поврежден!\nИсправьте его содержимое", "Ошибка содержания файла");
            }
            else this.ShowDialog();
            /*Difference between Show() and ShowDialog() methods
             * is in restricting access to the window that opened new window
             * in the second method*/

            //DictionaryTextBlock.Text = wordsLoader.fileText;
        }
    }
}
