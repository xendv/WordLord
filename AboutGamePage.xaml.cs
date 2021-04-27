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
    /// Логика взаимодействия для AboutGamePage.xaml
    /// </summary>
    public partial class AboutGamePage : Page
    {
        MainWindow ParentWindowMain;
        public AboutGamePage(MainWindow mainWindow)
        {
            ParentWindowMain = mainWindow;
            InitializeComponent();
        }

        private void ToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindowMain.SetPage("Main");
        }

        private void ToAboutAuthorPageButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindowMain.SetPage("AboutAuthor");
        }
    }
}
