using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
namespace WordLord
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            InitializeComponent();
        }
       
        [STAThreadAttribute]
        static void main(string[] args)
        {
            App app = new App();
            // Создание главного окна.
            MainWindow win = new MainWindow();
            //SetPage(win,"Main");
            win.Show();
            app.Run(win);

            //string s = "Слово";
            //Word w = new Word(s);

            //w.PrintLetters();
            //Window_1 w = new Window_1();
            //w.Show();
        }
    }
}
