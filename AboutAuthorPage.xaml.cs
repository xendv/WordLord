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
    /// Логика взаимодействия для AboutAuthorPage.xaml
    /// </summary>
    public partial class AboutAuthorPage : Page
    {
        MainWindow ParentWindowMain;
        public AboutAuthorPage(MainWindow mainWindow)
        {
            ParentWindowMain = mainWindow;
            InitializeComponent();
        }

        private void ToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindowMain.SetPage("Main");
        }

        private void ToAboutGamePageButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindowMain.SetPage("AboutGame");
        }
    }
}
