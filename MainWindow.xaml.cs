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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //SetPage(this,"Main");
            SetPage("Main");

        }
        public void SetPage(string page_name)
        {
            switch (page_name)
            {
                case "Main":
                    Content = new MainPage(this);
                    break;
                case "StartGame":
                    Content = new GamePage(this);
                    break;
                default:
                    MessageBox.Show("Запрашиваемая страница не найдена");
                    break;
            }
        }
        /*public void SetPage(MainWindow win, string page_name)
        {
            switch (page_name)
            {
                case "Main":
                    win.Content = new MainPage(win);
                    break;
                case "StartGame":
                    win.Content = new GamePage(win);
                    break;
                default:
                    MessageBox.Show("Запрашиваемая страница не найдена");
                    break;
            }
        }*/
        ///<summary>
        ///Возвращает true и отображает ошибки в файле словаря, если они есть
        ///и возвращает false, если их нет
        ///</summary>
        public bool DisplayDictionaryErrors(string error)
        {
            switch (error)
            {
                case "Wrong Content":
                    MessageBox.Show("Похоже, файл поврежден!\nИсправьте его содержимое", "Ошибка содержания файла", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    break;
                case "No File":
                    MessageBox.Show("Файл словаря пустой, создаю новый...", "Словарь не найден", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "":
                    {
                        return false;
                        //PrintExistingWordsList(ref wordsLoader.GetWords());
                        //this.ShowDialog();
                    }
                    //break;
                default:
                    MessageBox.Show("Необработанное исключение!", "ОЙ", MessageBoxButton.OK, MessageBoxImage.Error);
                    /*Difference between Show() and ShowDialog() methods
                     * is in restricting access to the window that opened new window
                     * in the second method*/
                    break;
            }
            return true;
        }
    }
}
