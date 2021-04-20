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
            SetPage(this,"Main");
        }
        static public void SetPage(MainWindow win, string page_name)
        {
            switch (page_name)
            {
                case "Main":
                    win.Content = new MainPage(win);
                    break;
                case "StartGame":
                    win.Content = new GamePage();
                    break;
                default:
                    MessageBox.Show("Запрашиваемая страница не найдена");
                    break;
            }
        }
    }
}
